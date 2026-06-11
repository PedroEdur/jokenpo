using System;
using RockPaperScissors.Network;

namespace Network
{
    [Serializable]
    public class NetworkMessage
    {
        public MessageType Type;
        public string SenderId;
        public string Payload;

        public NetworkMessage(MessageType type, string senderId, string payload)
        {
            Type = type;
            SenderId = senderId;
            Payload = payload;
        }
    }
}