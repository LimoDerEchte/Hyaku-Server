namespace HyakuServer.Networking.Packets.ServerToClient
{
    public class DespawnPlayerPacket : ServerToClientPacket
    {
        public int ClientID;
        
        public DespawnPlayerPacket() : base(101) { }
        public DespawnPlayerPacket(int clientID) : base(101)
        {
            ClientID = clientID;
        }

        public override void Send()
        {
            Packet.Write(ClientID);
            PacketHandler.SendTcpDataToLobby(ClientID, ClientID, Packet);
        }
    }
}