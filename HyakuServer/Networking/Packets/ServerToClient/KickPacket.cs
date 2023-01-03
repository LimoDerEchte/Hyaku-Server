using System;

namespace HyakuServer.Networking.Packets.ServerToClient
{
    public class KickPacket : ServerToClientPacket
    {
        public string Reason;
        public int ClientID;
        
        public KickPacket() : base(-999) { }
        public KickPacket(string reason, int clientID) : base(-999)
        {
            Reason = reason;
            ClientID = clientID;
        }

        public override void Send()
        {
            Packet.Write(Reason);
            PacketHandler.SendTcpData(ClientID, Packet, _ => HyakuServer.Clients[ClientID].Disconnect());
            Console.WriteLine($"User (ID: {ClientID}) got kicked: {Reason}");
        }
    }
}