namespace Client
{
    using System;
    using System.Windows.Forms;

    using Common.Network;

    public partial class MainForm : Form
    {
        private ITransport _currentTransport;

        public MainForm()
        {
            InitializeComponent();

            _transport.Items.Add(TransportType.WebSocket);
            _transport.Items.Add(TransportType.Tcp);
            _transport.SelectedItem = TransportType.WebSocket;

            SetDefaultButtonState();
        }

        private void HandleButtonStartClick(object sender, EventArgs e)
        {
            try
            {
                _currentTransport = TransportFactory.Create((TransportType)_transport.SelectedItem);
                _currentTransport.ConnectionStateChanged += HandleConnectionStateChanged;
                _currentTransport.MessageReceived += HandleMessageReceived;
                _currentTransport.Connect(_serverAddress.Text, _serverPort.Text);

                _serverAddress.Enabled = false;
                _serverPort.Enabled = false;
                _transport.Enabled = false;
                _message.Enabled = false;
                _buttonStart.Enabled = false;
                _buttonStop.Enabled = true;
                _buttonLogin.Enabled = true;
                _login.Enabled = true;
            }
            catch (Exception ex)
            {
                _messages.Items.Add(ex.Message);
                SetDefaultButtonState();
            }
        }

        private void HandleMessageReceived(MessageReceivedEventArgs e)
        {
            _messages.Items.Add(e.Message);
        }

        private void HandleConnectionStateChanged(ConnectionStateChangedEventArgs e)
        {
            if (e.Connected)
            {
                if (string.IsNullOrEmpty(e.ClientName))
                {
                    _messages.Items.Add("Клиент подключен к серверу.");
                    _messages.Items.Add("Авторизуйтеся, чтобы отправлять сообщения.");
                }
                else
                {
                    _messages.Items.Add("Авторизация выполнена успешно.");

                    _buttonLogin.Enabled = false;
                    _login.Enabled = false;
                    _message.Enabled = true;
                    _buttonSend.Enabled = true;
                }
            }
            else
            {
                _messages.Items.Add("Клиент отключен от сервера.");
                SetDefaultButtonState();
            }
        }

        private void HandleButtonStopClick(object sender, EventArgs e)
        {
            if (_currentTransport != null)
            {
                _currentTransport.ConnectionStateChanged -= HandleConnectionStateChanged;
                _currentTransport.MessageReceived -= HandleMessageReceived;
                _currentTransport.Disconnect();
            }
            
            SetDefaultButtonState();
        }

        private void HandleButtonLoginClick(object sender, EventArgs e)
        {
            _currentTransport?.Login(_login.Text);
        }

        private void HandleButtonSendClick(object sender, EventArgs e)
        {
            _currentTransport?.Send(_message.Text);
        }

        private void HandleButtonClearClick(object sender, EventArgs e)
        {
            _messages.Items.Clear();
        }

        private void SetDefaultButtonState()
        {
            _buttonStart.Enabled = true;
            _buttonStop.Enabled = false;
            _buttonLogin.Enabled = false;
            _buttonSend.Enabled = false;
            
            _serverAddress.Enabled = true;
            _serverPort.Enabled = true;
            _transport.Enabled = true;
            _message.Enabled = false;
            _login.Enabled = false;
        }
        
        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { HandleMessageReceived(e); }));
            }
            else
            {
                HandleMessageReceived(e);
            }
        }

        private void HandleConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { HandleConnectionStateChanged(e); }));
            }
            else
            {
                HandleConnectionStateChanged(e);
            }
        }
    }
}
