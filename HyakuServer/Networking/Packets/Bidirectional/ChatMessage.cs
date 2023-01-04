namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class ChatMessageC2S : ClientToServerPacket
    {
        public ChatMessageC2S() : base(14) { }
        
        public override void handle(Packet packet, int clientId)
        {
            string uname = HyakuServer.Clients[clientId].Player.username;
            new ChatMessageS2C($"<color=blue>{uname}:</color> <color=white>{packet.ReadString()}</color>").Send();
        }
    }
    
    public class ChatMessageS2C : ServerToClientPacket
    {
        public string Message;
        public int Client = -1;
        
        public ChatMessageS2C() : base(14) { }
        public ChatMessageS2C(string message) : base(14)
        {
            Message = message;
        }
        public ChatMessageS2C(int clientId, string message) : base(14)
        {
            Message = message;
            Client = clientId;
        }
        
        public override void Send()
        {
            Packet.Write(Message);
            if(Client < 0)
                PacketHandler.SendTcpDataToLobby(-Client - 1, Packet);
            else
                PacketHandler.SendTcpDataToLobby(Client, Client, Packet);
        }
    }
}