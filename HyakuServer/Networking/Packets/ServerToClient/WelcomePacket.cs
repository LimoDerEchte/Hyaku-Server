
namespace HyakuServer.Networking.Packets.ServerToClient
{
    public class WelcomePacket : ServerToClientPacket
    {
        public int clientID;

        public WelcomePacket() : base(999)
        {
        }

        public WelcomePacket(int id) : base(999)
        {
            clientID = id;
        }

        public override void Send()
        {
            Packet.Write(clientID);
            PacketHandler.SendTcpData(clientID, Packet);
        }
    }
}