using System.Numerics;
using HyakuServer.Utility;

namespace HyakuServer.Networking.Packets.ServerToClient
{
    public class SpawnPlayerPacket : ServerToClientPacket
    {
        public Vector3 Position;
        public string Username;
        public int ClientID;
        public Texture2D Texture2D;
        public int ToID;

        public SpawnPlayerPacket() : base(100) { }
        public SpawnPlayerPacket(string username, int clientID, Texture2D texture2D, int toID) : base(100)
        {
            Position = new Vector3(0F, 0F, 0F);
            Username = username;
            ClientID = clientID;
            Texture2D = texture2D;
            ToID = toID;
        }
        
        public override void Send()
        {
            Packet.Write(Position);
            Packet.Write(Username);
            Packet.Write(Texture2D);
            Packet.Write(ClientID);
            PacketHandler.SendTcpData(ToID, Packet);
        }
    }
}