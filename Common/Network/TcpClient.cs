namespace Common.Network
{
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    using Packets;

    public class TcpClient : ITransport
    {
        #region Constants

        private const int BUFFER_SIZE = ushort.MaxValue * 3;
        private const int SIZE_LENGTH = 2;

        #endregion

        #region Fields

        private readonly SocketAsyncEventArgs _receiveEvent;
        private readonly SocketAsyncEventArgs _sendEvent;
        private readonly SocketAsyncEventArgs _connectEvent;

        private readonly ConcurrentQueue<byte[]> _sendQueue;

        private readonly Socket _socket;

        private int _disposed;
        private int _sending;

        private string _login;

        #endregion Fields

        #region Events

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        #endregion Events

        #region Constructors

        public TcpClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _receiveEvent = new SocketAsyncEventArgs();
            _receiveEvent.SetBuffer(new byte[BUFFER_SIZE], 0, BUFFER_SIZE);
            _receiveEvent.Completed += ReceiveCompleted;

            _sendEvent = new SocketAsyncEventArgs();
            _sendEvent.SetBuffer(new byte[BUFFER_SIZE], 0, BUFFER_SIZE);
            _sendEvent.Completed += SendCompleted;

            _connectEvent = new SocketAsyncEventArgs();
            _connectEvent.Completed += ConnectCompleted;

            _sendQueue = new ConcurrentQueue<byte[]>();

            _disposed = 0;
            _sending = 0;
        }

        #endregion Constructors

        #region Methods

        public void Connect(string address, string port)
        {
            _connectEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(address), int.Parse(port));
            _socket.ConnectAsync(_connectEvent);
        }

        public void Disconnect()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 1)
                return;

            Safe(() => _socket.Dispose());
            Safe(() => _sendEvent.Dispose());
            Safe(() => _receiveEvent.Dispose());

            _login = string.Empty;
        }

        public void Login(string login)
        {
            _login = login;
            _sendQueue.Enqueue(new ConnectionRequest(_login).GetBytes());

            if (Interlocked.CompareExchange(ref _sending, 1, 0) == 0)
                SendImpl();
        }

        public void Send(string message)
        {
            _sendQueue.Enqueue(new MessageRequest(message).GetBytes());

            if (Interlocked.CompareExchange(ref _sending, 1, 0) == 0)
                SendImpl();
        }
        
        private void SendImpl()
        {
            if (_disposed == 1)
                return;

            if (!_sendQueue.TryDequeue(out var packet) && Interlocked.CompareExchange(ref _sending, 0, 1) == 1)
                return;
            
            // Установить размер в начале пакета.
            Array.Copy(packet, 0, _sendEvent.Buffer, SIZE_LENGTH, packet.Length);
            BufferPrimitives.SetUint16(_sendEvent.Buffer, 0, (ushort)packet.Length);

            _sendEvent.SetBuffer(0, packet.Length + SIZE_LENGTH);

            if (!_socket.SendAsync(_sendEvent))
                SendCompleted(_socket, _sendEvent);
        }

        private void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred != e.Count || e.SocketError != SocketError.Success)
            {
                Disconnect();
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, false));
                return;
            }

            SendImpl();
        }

        private void Receive()
        {
            if (_disposed == 1)
                return;

            if (!_socket.ReceiveAsync(_receiveEvent))
                ReceiveCompleted(_socket, _receiveEvent);
        }

        private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                Disconnect();
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, false));
                return;
            }

            int available = e.Offset + e.BytesTransferred;
            for (; ; )
            {
                if (available < SIZE_LENGTH)
                {
                    // WE NEED MORE DATA
                    break;
                }

                var offset = 0;
                var length = BufferPrimitives.GetUint16(e.Buffer, ref offset);
                if (length + SIZE_LENGTH > available)
                {
                    // WE NEED MORE DATA
                    break;
                }

                HandlePacket(BufferPrimitives.GetBytes(e.Buffer, ref offset, length));

                available = available - length - SIZE_LENGTH;
                if (available > 0)
                    Array.Copy(e.Buffer, length + SIZE_LENGTH, e.Buffer, 0, available);
            }

            e.SetBuffer(available, BUFFER_SIZE - available);
            Receive();
        }

        private void HandlePacket(byte[] packet)
        {
            var packetId = (PacketId)BufferPrimitives.GetUint8(packet, 0);
            switch (packetId)
            {
                case PacketId.ConnectionResponse:
                    var connectionResponse = new ConnectionResponse(packet);
                    if (connectionResponse.Result == ResultCodes.Failure)
                    {
                        _login = string.Empty;
                        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(_login, connectionResponse.Reason));
                    }
                    ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, true));
                    break;
                case PacketId.MessageBroadcast:
                    var messageBroadcast = new Packets.MessageBroadcast(packet);
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(_login, messageBroadcast.Message));
                    break;
            }
        }
        
        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, false));
                return;
            }

            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, true));
            Receive();
        }
        
        private void Safe(Action callback)
        {
            try
            {
                callback();
            }
            catch
            {
            }
        }

        #endregion Methods
    }
}
