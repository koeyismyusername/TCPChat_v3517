using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.IO;

using TCPChatServer.Models;
using Newtonsoft.Json;

namespace TCPChatServer
{
    public partial class ChatServerForm : Form
    {
        private TcpListener _listener;
        private bool _isRunning;

        private Dictionary<long, List<TcpClient>> _rooms;
        private Dictionary<TcpClient, long> _clientRoomIdMap;

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
            _rooms = new Dictionary<long, List<TcpClient>>();
            _clientRoomIdMap = new Dictionary<TcpClient, long>();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lBoxMessages.Items.Clear();
            _listener.Start();
            IsRunning = true;

            AddToLBoxMessages("서버가 준비되었습니다.");

            new Thread(AcceptClient).Start();
        }

        private void AddToLBoxMessages(string message)
        {
            if (lBoxMessages.InvokeRequired) lBoxMessages.Invoke(new MethodInvoker(() => lBoxMessages.Items.Add(message)));
            else lBoxMessages.Items.Add(message);
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
            client.GetStream().BeginRead(contentLengthBytes, 0, contentLengthBytes.Length, OnContentLengthReadComplete, new DefaultReadState(client, contentLengthBytes));
        }

        private void OnContentLengthReadComplete(IAsyncResult ar)
        {
            DefaultReadState state = (DefaultReadState)ar.AsyncState;
            if (state is null) return;

            byte[] contentLengthBytes = state.Bytes;
            TcpClient client = state.Client;

            if (client is null) return;
            
            try
            {
                int read = client.GetStream().EndRead(ar);
                if (read is 0)
                {
                    RemoveClient(client);
                    return;
                }
            }
            catch (InvalidOperationException e)
            {
                RemoveClient(client);
                return;
            }
            catch (IOException e)
            {
                //Console.WriteLine("소켓 연결이 끊어짐");
                RemoveClient(client);

                return;
            }

            int contentLength = BitConverter.ToInt32(contentLengthBytes, 0);
            byte[] contentBytes = new byte[contentLength];
            client.GetStream().BeginRead(contentBytes, 0, contentBytes.Length, OnContentReadComplete, new DefaultReadState(client, contentBytes));
        }

        private void OnContentReadComplete(IAsyncResult ar)
        {
            DefaultReadState state = (DefaultReadState)ar.AsyncState;

            if (state is null) return;
            byte[] contentBytes = state.Bytes;
            TcpClient client = state.Client;

            string json = Encoding.UTF8.GetString(contentBytes);
            Chat chat = JsonConvert.DeserializeObject<Chat>(json);

            if (client is null) return;
            int read = client.GetStream().EndRead(ar);
            if (read is 0) return;

            new Thread(RouteChat).Start(new ClientChatPair(client, chat));
            Listen(client);
        }

        private void RouteChat(object clientChatPairObj)
        {
            ClientChatPair pair = (ClientChatPair)clientChatPairObj;

            Chat chat = pair.Chat;
            TcpClient client = pair.Client;

            switch (chat.State)
            {
                case ChatState.Connect:
                    AddClient(chat.RoomId, pair.Client);
                    SendToRoom(chat);
                    AddToLBoxMessages($"{chat.Username} 접속");
                    break;
                case ChatState.Message:
                    SendToRoom(chat);
                    AddToLBoxMessages($"{chat.Username}: {chat.Message}");
                    break;
                case ChatState.Disconnect:
                    RemoveClient(client);
                    SendToRoom(chat);
                    AddToLBoxMessages($"{chat.Username} 종료");
                    break;
                default:
                    break;
            }
        }

        private void SendToRoom(Chat chat)
        {
            _rooms.TryGetValue(chat.RoomId, out List<TcpClient> clients);
            if (clients is null) return;

            string json = JsonConvert.SerializeObject(chat);
            byte[] contentBytes = Encoding.UTF8.GetBytes(json);
            byte[] contentLengthBytes = BitConverter.GetBytes(contentBytes.Length);
            byte[] combinedBytes = new byte[contentLengthBytes.Length + contentBytes.Length];
            Array.Copy(contentLengthBytes, 0, combinedBytes, 0, contentLengthBytes.Length);
            Array.Copy(contentBytes, 0, combinedBytes, contentLengthBytes.Length, contentBytes.Length);

            clients.ForEach(c => c.GetStream().BeginWrite(combinedBytes, 0, combinedBytes.Length, OnChatWriteComplete, c));
        }

        private void OnChatWriteComplete(IAsyncResult ar)
        {
            TcpClient client = (TcpClient)ar.AsyncState;
            if (client is null) return;

            client.GetStream().EndWrite(ar);
        }

        private void AddClient(long roomId, TcpClient client)
        {
            _clientRoomIdMap.Add(client, roomId);
            _rooms.TryGetValue(roomId, out List<TcpClient> clients);

            if (clients is null) _rooms.Add(roomId, new List<TcpClient> { client });
            else clients.Add(client);
        }

        private void RemoveClient(TcpClient client)
        {
            _clientRoomIdMap.TryGetValue(client, out long roomId);

            if (roomId is 0) return;
            _clientRoomIdMap.Remove(client);
            _rooms[roomId].Remove(client);
        }

        private void RemoveClient(long key, TcpClient client)
        {
            _rooms.TryGetValue(key, out List<TcpClient> clients);

            if (clients is null) return;
            clients.Remove(client);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            CloseServer();
            AddToLBoxMessages("서버가 종료되었습니다.");
        }

        private void CloseServer()
        {
            IsRunning = false;
            
            foreach(var room in _rooms.Values)
            {
                room.ForEach(c => c.Close());
            }

            _rooms.Clear();
            _clientRoomIdMap.Clear();

            _listener.Stop();
        }
    }
}
