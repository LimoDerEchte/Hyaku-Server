using System;

namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class KeepAlivePacketC2S : ClientToServerPacket
    {
        public KeepAlivePacketC2S() : base(-1) { }
        
        public override void handle(Packet packet, int clientId)
        {
            HyakuServer.Clients[clientId].LastPacket = DateTime.UtcNow.Ticks;
        }
    }
    
    public class KeepAlivePacketS2C : ServerToClientPacket
    {
        public KeepAlivePacketS2C() : base(-1) { }

        public override void Send()
        {
            PacketHandler.SendTcpDataToAll(Packet);
        }
    }
}