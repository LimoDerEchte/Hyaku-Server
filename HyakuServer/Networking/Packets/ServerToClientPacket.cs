namespace HyakuServer.Networking.Packets
{
    public abstract class ServerToClientPacket
    {
        public Packet Packet;
        public int ID;

        public ServerToClientPacket()
        {
        }

        public ServerToClientPacket(int id)
        {
            ID = id;
            Packet = new Packet(ID);
        }

        public abstract void Send();
    }
}
