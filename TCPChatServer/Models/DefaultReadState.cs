using System.Net.Sockets;

namespace TCPChatServer.Models
{
    public class DefaultReadState
    {
        public byte[] Bytes { get; private set; }
        public TcpClient Client { get; private set; }

        public DefaultReadState(TcpClient client, byte[] bytes)
        {
            Bytes = bytes;
            Client = client;
        }
    }
}
