using System.Net.Sockets;

namespace TCPChatServer.Models
{
    public class ClientChatPair
    {
        public TcpClient Client { get; private set; }
        public Chat Chat { get; private set; }

        public ClientChatPair(TcpClient client, Chat chat)
        {
            Client = client;
            Chat = chat;
        }
    }
}
