using System.Numerics;

namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class MovementPacketC2S : ClientToServerPacket
    {
        public MovementPacketC2S() : base(1) { }

        public override void handle(Packet packet, int clientId)
        {
            new MovementPacketS2C(packet.ReadVector3(), clientId).Send();
        }
    }
    
    public class MovementPacketS2C : ServerToClientPacket
    {
        public Vector3 Position;
        public int OwnerID;
        
        public MovementPacketS2C() : base(1) { }
        public MovementPacketS2C(Vector3 position, int ownerID) : base(1)
        {
            Position = position;
            OwnerID = ownerID;
        }
        
        public override void Send()
        {
            Packet.Write(OwnerID);
            Packet.Write(Position);
            PacketHandler.SendTcpDataToLobby(OwnerID, OwnerID, Packet);
        }
    }
}