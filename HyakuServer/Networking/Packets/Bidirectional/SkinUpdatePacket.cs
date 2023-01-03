using HyakuServer.Utility;

namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class SkinUpdatePacketC2S : ClientToServerPacket
    {
        public SkinUpdatePacketC2S() : base(3) { }
        
        public override void handle(Packet packet, int clientId)
        {
            Texture2D tex = packet.ReadTexture2D();
            HyakuServer.Clients[clientId].Player.Texture = tex;
            new SkinUpdatePacketS2C(tex, clientId).Send();
        }
    }
    
    public class SkinUpdatePacketS2C : ServerToClientPacket
    {
        public Texture2D Tex;
        public int ClientId;
        
        public SkinUpdatePacketS2C() : base(3) { }
        public SkinUpdatePacketS2C(Texture2D tex, int clientId) : base(3)
        {
            Tex = tex;
            ClientId = clientId;
        }
        
        public override void Send()
        {
            Packet.Write(ClientId);
            Packet.Write(Tex);
            PacketHandler.SendTcpDataToAll(ClientId, Packet);
        }
    }
}