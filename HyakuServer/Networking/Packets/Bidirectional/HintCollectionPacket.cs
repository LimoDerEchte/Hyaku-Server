using System;
using HyakuServer.DataHandling;

namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class HintCollectionPacketC2S : ClientToServerPacket
    {
        public HintCollectionPacketC2S() : base(11) { }
        
        public override void handle(Packet packet, int clientId)
        {
            if (Enum.TryParse(packet.ReadString(), out EndingTypes ending))
            {
                if (!HyakuServer.Save.UnlockedHints.Contains(ending))
                {
                    HyakuServer.Save.AddHint(ending);
                    new HintCollectionPacketS2C(ending, clientId).Send();
                    Console.WriteLine("Player " + HyakuServer.Clients[clientId].Player.username + " unlocked hint " + ending.ToString());
                }
            }else
                Console.WriteLine("Player " + HyakuServer.Clients[clientId].Player.username + " tried to send invalid hint!");
        }
    }
    
    public class HintCollectionPacketS2C : ServerToClientPacket
    {
        public EndingTypes Ending;
        public int ClientID;
        
        public HintCollectionPacketS2C() : base(11) { }
        public HintCollectionPacketS2C(EndingTypes ending, int clientId) : base(11)
        {
            Ending = ending;
            ClientID = clientId;
        }
        
        public override void Send()
        {
            Packet.Write(Ending.ToString());
            PacketHandler.SendTcpDataToAll(ClientID, Packet);
        }
    }
}