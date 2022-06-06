// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Пальников М. С.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

namespace Server
{
    using System;
    using System.Net;

    using Common.Network;

    public class NetworkManager
    {
        #region Constants

        private const int WS_PORT = 65000;
        private const int TCP_PORT = 65001;

        #endregion Constants

        #region Fields

        private readonly WsServer _wsServer;
        private readonly TcpServer _tcpServer;

        #endregion Fields

        #region Constructors

        public NetworkManager()
        {
            _wsServer = new WsServer(new IPEndPoint(IPAddress.Any, WS_PORT));
            _wsServer.ConnectionStateChanged += HandleConnectionStateChanged;
            _wsServer.MessageReceived += HandleMessageReceived;

            _tcpServer = new TcpServer(new IPEndPoint(IPAddress.Any, TCP_PORT));
            _tcpServer.ConnectionStateChanged += HandleConnectionStateChanged;
            _tcpServer.MessageReceived += HandleMessageReceived;
        }

        #endregion Constructors

        #region Methods

        public void Start()
        {
            Console.WriteLine($"WebSocketServer: {IPAddress.Any}:{WS_PORT}");
            _wsServer.Start();

            Console.WriteLine($"TcpServer: {IPAddress.Any}:{TCP_PORT}");
            _tcpServer.Start();
        }

        public void Stop()
        {
            _wsServer.Stop();
            _tcpServer.Stop();
        }
        
        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            string message = $"Клиент '{e.ClientName}' отправил сообщение '{e.Message}'.";

            Console.WriteLine(message);

            _wsServer.Send(message);
            _tcpServer.Send(message);
        }

        private void HandleConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            string clientState = e.Connected ? "подключен" : "отключен";
            string message = $"Клиент '{e.ClientName}' {clientState}.";

            Console.WriteLine(message);

            _wsServer.Send(message);
            _tcpServer.Send(message);
        }
        
        #endregion Methods
    }
}
