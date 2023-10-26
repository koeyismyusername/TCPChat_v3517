using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

using Newtonsoft.Json;
using TCPChatProject_3517.Models;

namespace TCPChatProject_3517
{
    public partial class ChatClientForm : Form
    {
        private TcpClient _client;

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                _isRunning = value;

                if (btnConnect.InvokeRequired) btnConnect.Invoke(new MethodInvoker(() => { btnConnect.Enabled = !value; }));
                else btnConnect.Enabled = !value;

                if (btnExit.InvokeRequired) btnExit.Invoke(new MethodInvoker(() => { btnExit.Enabled = value; }));
                else btnExit.Enabled = value;

                if (btnSend.InvokeRequired) btnExit.Invoke(new MethodInvoker(() => { btnSend.Enabled = value; }));
                btnSend.Enabled = value;

                if (numRoomId.InvokeRequired) btnExit.Invoke(new MethodInvoker(() => { numRoomId.Enabled = !value; }));
                numRoomId.Enabled = !value;

                if (tBoxUsername.InvokeRequired) btnExit.Invoke(new MethodInvoker(() => { tBoxUsername.Enabled = !value; }));
                tBoxUsername.Enabled = !value;

                if (tBoxMessage.InvokeRequired) btnExit.Invoke(new MethodInvoker(() => { tBoxMessage.Enabled = value; }));
                tBoxMessage.Enabled = value;
            }
        }

        public long RoomId{ get => (long)numRoomId.Value; }
        public string Username { get => tBoxUsername.Text; }
        public string Message { get => tBoxMessage.Text; }

        public ChatClientForm()
        {
            InitializeComponent();
            IsRunning = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SendChat(new Chat(RoomId, Username, Message, ChatState.Disconnect));
            CloseClient();
        }

        private void CloseClient()
        {
            if (_client != null) _client.Close();
            _client = null;
            IsRunning = false;

            SendChat(new Chat(RoomId, Username, Message, ChatState.Disconnect));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            lBoxMessages.Items.Clear();
            _client = new TcpClient();
            _client.BeginConnect("127.0.0.1", 8080, OnConnectComplete, null);
        }

        private void OnConnectComplete(IAsyncResult ar)
        {
            try
            {
                _client.EndConnect(ar);
                IsRunning = true;
                SendChat(new Chat(RoomId, Username, Message, ChatState.Connect));

                new Thread(Listen).Start();
                return;
            }
            catch (SocketException e)
            {
                AddToLBoxMessages("서버가 준비되지 않았습니다.");
            }

            CloseClient();
        }

        private void SendChat(Chat chat)
        {
            if (!IsRunning) return;

            string json = JsonConvert.SerializeObject(chat);
            byte[] contentBytes = Encoding.UTF8.GetBytes(json);
            byte[] contentLengthBytes = BitConverter.GetBytes(contentBytes.Length);
            byte[] combinedBytes = new byte[contentLengthBytes.Length + contentBytes.Length];

            int startIndex = 0;
            Array.Copy(contentLengthBytes, 0, combinedBytes, startIndex, contentLengthBytes.Length);

            startIndex += contentLengthBytes.Length;
            Array.Copy(contentBytes, 0, combinedBytes, startIndex, contentBytes.Length);

            _client.GetStream().BeginWrite(combinedBytes, 0, combinedBytes.Length, OnChatWriteComplete, null);
            //AddToLBoxMessages("메세지 전송");
        }

        private void Listen()
        {
            if (!IsRunning) return;

            byte[] contentLengthBytes = new byte[4];
            _client.GetStream().BeginRead(contentLengthBytes, 0, contentLengthBytes.Length, OnContentLengthReadComplete, contentLengthBytes);
        }

        private void OnContentLengthReadComplete(IAsyncResult ar)
        {
            if (!IsRunning) return;

            try
            {
                int read = _client.GetStream().EndRead(ar);
                if (read is 0)
                {
                    CloseClient();
                    return;
                }
            }
            catch (IOException e)
            {
                CloseClient();
                return;
            }
            catch (InvalidOperationException)
            {
                CloseClient();
                return;
            }
            

            byte[] contentLengthBytes = (byte[])ar.AsyncState;
            int contentLength = BitConverter.ToInt32(contentLengthBytes, 0);

            byte[] contentBytes = new byte[contentLength];
            _client.GetStream().BeginRead(contentBytes, 0, contentBytes.Length, OnContentReadComplete, contentBytes);
        }

        private void OnContentReadComplete(IAsyncResult ar)
        {
            if (!IsRunning) return;

            byte[] contentBytes = (byte[])ar.AsyncState;
            string json = Encoding.UTF8.GetString(contentBytes);
            Chat chat = JsonConvert.DeserializeObject<Chat>(json);

            new Thread(ShowChat).Start(chat);
            new Thread(Listen).Start();
        }

        private void ShowChat(object chatObj)
        {
            if (!IsRunning) return;

            Chat chat = (Chat)chatObj;

            switch (chat.State)
            {
                case ChatState.Connect:
                    AddToLBoxMessages($"{chat.Username}님이 입장했습니다.");
                    break;
                case ChatState.Message:
                    AddToLBoxMessages($"{chat.Username}: {chat.Message}");
                    break;
                case ChatState.Disconnect:
                    AddToLBoxMessages($"{chat.Username}님이 나갔습니다.");
                    break;
                default:
                    break;
            }
        }

        private void AddToLBoxMessages(string message)
        {
            if (lBoxMessages.InvokeRequired) lBoxMessages.Invoke(new MethodInvoker(() => lBoxMessages.Items.Add(message)));
            else lBoxMessages.Items.Add(message);
        }

        private void tBoxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsRunning) return;

            if (e.KeyCode == Keys.Enter) btnSend_Click(sender, e);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!IsRunning) return;

            Chat messageChat = new Chat(RoomId, Username, Message, ChatState.Message);
            string json = JsonConvert.SerializeObject(messageChat);
            byte[] chatBytes = Encoding.UTF8.GetBytes(json);
            byte[] lengthBytes = BitConverter.GetBytes(chatBytes.Length);
            byte[] combinedBytes = new byte[chatBytes.Length + 4];
            Array.Copy(lengthBytes, 0, combinedBytes, 0, 4);
            Array.Copy(chatBytes, 0, combinedBytes, 4, chatBytes.Length);

            _client.GetStream().BeginWrite(combinedBytes, 0, combinedBytes.Length, OnChatWriteComplete, null);
        }

        private void OnChatWriteComplete(IAsyncResult ar)
        {
            if (!IsRunning) return;

            _client.GetStream().EndWrite(ar);

            if (tBoxMessage.InvokeRequired) tBoxMessage.Invoke(new MethodInvoker(() => tBoxMessage.Clear()));
            else tBoxMessage.Clear();
        }
    }
}
