// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Пальников М. С.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

namespace Common.Network
{
    using Packets;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    using ConnectionRequest = Packets.ConnectionRequest;
    using ConnectionResponse = Packets.ConnectionResponse;
    using MessageBroadcast = Packets.MessageBroadcast;

    public class TcpServer
    {
        #region Fields

        private readonly IPEndPoint _listenAddress;
        private readonly ConcurrentDictionary<IPEndPoint, TcpConnection> _connections;

        private SocketAsyncEventArgs _acceptEvent;
        private Socket _server;

        #endregion Fields

        #region Events

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        #endregion Events

        #region Constructors

        public TcpServer(IPEndPoint listenAddress)
        {
            _listenAddress = listenAddress;
            _connections = new ConcurrentDictionary<IPEndPoint, TcpConnection>();
        }

        #endregion Constructors

        #region Methods

        public void Start()
        {
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(_listenAddress);
            _acceptEvent = new SocketAsyncEventArgs();
            _acceptEvent.Completed += AcceptCompleted;
            _server.Listen(8);
            Accept();
        }

        public void Stop()
        {
            Socket server = _server;
            _server = null;
            server.Dispose();

            var connections = _connections.Select(item => item.Value).ToArray();
            foreach (var connection in connections)
            {
                connection.Stop();
            }

            _connections.Clear();
        }

        public void Send(string message)
        {
            var messageBroadcast = new MessageBroadcast(message).GetBytes();

            foreach (var connection in _connections)
            {
                connection.Value.Send(messageBroadcast);
            }
        }

        internal void HandlePacket(IPEndPoint remoteEndpoint, byte[] packet)
        {
            if (!_connections.TryGetValue(remoteEndpoint, out TcpConnection connection))
                return;

            var packetId = (PacketId)BufferPrimitives.GetUint8(packet, 0);
            switch (packetId)
            {
                case PacketId.ConnectionRequest:
                    var connectionRequest = new ConnectionRequest(packet);
                    if (_connections.Values.Any(item => item.Login == connectionRequest.Login))
                    {
                        connection.Send(new ConnectionResponse(
                            ResultCodes.Failure, 
                            $"Клиент с именем '{connectionRequest.Login}' уже подключен.").GetBytes());
                    }
                    else
                    {
                        connection.Login = connectionRequest.Login;
                        connection.Send(new ConnectionResponse(ResultCodes.Ok, string.Empty).GetBytes());
                        ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(connection.Login, true));
                    }
                    break;
                case PacketId.MessageRequest:
                    var messageRequest = new Packets.MessageRequest(packet);
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(connection.Login, messageRequest.Message));
                    break;
            }
        }

        internal void FreeConnection(IPEndPoint remoteEndpoint)
        {
            if (_connections.TryRemove(remoteEndpoint, out TcpConnection connection) && !string.IsNullOrEmpty(connection.Login))
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(connection.Login, false));
        }

        private void Accept()
        {
            _acceptEvent.AcceptSocket = null;
            if (_server?.AcceptAsync(_acceptEvent) == false)
                AcceptCompleted(_server, _acceptEvent);
        }

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            var remoteEndpoint = (IPEndPoint)e.AcceptSocket.RemoteEndPoint;
            var connection = new TcpConnection(remoteEndpoint, e.AcceptSocket, this);
            if (_connections.TryAdd(remoteEndpoint, connection))
            {
                connection.Start();
            }

            Accept();
        }

        #endregion Methods
    }
}
