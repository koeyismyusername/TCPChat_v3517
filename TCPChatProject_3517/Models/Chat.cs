using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCPChatProject_3517.Models
{
    public class Chat
    {
        public long RoomId { get; private set; }
        public string Username { get; private set; }
        public string Message { get; private set; }
        public ChatState State { get; private set; }

        public Chat(long roomId, string username, string message, ChatState state)
        {
            RoomId = roomId;
            Username = username;
            Message = message;
            State = state;
        }
    }
}
