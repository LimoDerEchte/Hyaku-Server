
namespace HyakuServer.Networking.Packets
{
    public abstract class ClientToServerPacket
    {
        public int ID;

        public ClientToServerPacket()
        {
        }

        public ClientToServerPacket(int id)
        {
            ID = id;
        }

        public abstract void handle(Packet packet, int clientId);
    }
}