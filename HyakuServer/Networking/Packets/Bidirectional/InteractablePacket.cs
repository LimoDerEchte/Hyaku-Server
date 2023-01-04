using System.Numerics;

namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class InteractablePacketC2S : ClientToServerPacket
    {
        public InteractablePacketC2S() : base(12) { }
        
        public override void handle(Packet packet, int clientId)
        {
            new InteractablePacketS2C(packet.ReadVector3(), packet.ReadInt(), packet.ReadInt(), clientId).Send();
        }
    }
    
    public class InteractablePacketS2C : ServerToClientPacket
    {
        public Vector3 Pos;
        public int Type, State;
        public int ClientID;

        public InteractablePacketS2C() : base(12) { }
        public InteractablePacketS2C(Vector3 position, int type, int state, int clientID) : base(12)
        {
            Pos = position;
            Type = type;
            State = state;
            ClientID = clientID;
        }
        
        public override void Send()
        {
            Packet.Write(Pos);
            Packet.Write(Type);
            Packet.Write(State);
            PacketHandler.SendTcpDataToLobby(ClientID, ClientID, Packet);
        }
    }
}