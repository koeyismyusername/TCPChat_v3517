using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;

namespace TCPChatServer
{
    public partial class ChatServerForm : Form
    {
        private TcpListener _listener;
        private bool _isRunning;

        private Dictionary<long, List<TcpClient>> _clientsDict;

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                _isRunning = value;

                if (btnStart.InvokeRequired) btnStart.Invoke(new MethodInvoker(() => { btnStart.Enabled = !value; }));
                else btnStart.Enabled = !value;

                if (btnExit.InvokeRequired) btnExit.Invoke(new MethodInvoker(() => { btnExit.Enabled = value; }));
                else btnExit.Enabled = value;
            }
        }

        public ChatServerForm()
        {
            InitializeComponent();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            IsRunning = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _listener.Start();
            IsRunning = true;

            new Thread(AcceptClient).Start();
        }

        private void AcceptClient()
        {
            if (!IsRunning) return;
            _listener.BeginAcceptTcpClient(OnAcceptTcpClientComplete, null);
        }

        private void OnAcceptTcpClientComplete(IAsyncResult ar)
        {
            if (!IsRunning) return;

            TcpClient client = _listener.EndAcceptTcpClient(ar);

            new Thread(Listen).Start(client);
            AcceptClient();
        }

        private void Listen(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;

            if (client is null) return;

            byte[] contentLengthBytes = new byte[4];
            client.GetStream().BeginRead(contentLengthBytes, 0, contentLengthBytes.Length, OnContentLengthReadComplete, new ContentLengthReadState(client, contentLengthBytes));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CloseServer();
        }

        private void CloseServer()
        {
            IsRunning = false;
            _listener.Stop();
        }
    }
}
