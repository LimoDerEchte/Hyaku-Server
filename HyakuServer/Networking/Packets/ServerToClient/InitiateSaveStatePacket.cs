using HyakuServer.DataHandling;

namespace HyakuServer.Networking.Packets.ServerToClient
{
    public class InitiateSaveStatePacket : ServerToClientPacket
    {
        public int ClientID;
        
        public InitiateSaveStatePacket() : base(99) { }
        public InitiateSaveStatePacket(int clientID) : base(99)
        {
            ClientID = clientID;
        }
        
        public override void Send()
        {
            Packet.Write((int)HyakuServer.Save.UnlockedEndings.Count);
            foreach (EndingTypes ending in HyakuServer.Save.UnlockedEndings)
            {
                Packet.Write(ending.ToString());
            }
            Packet.Write((int)HyakuServer.Save.UnlockedHints.Count);
            foreach (EndingTypes ending in HyakuServer.Save.UnlockedHints)
            {
                Packet.Write(ending.ToString());
            }
            PacketHandler.SendTcpData(ClientID, Packet);
        }
    }
}