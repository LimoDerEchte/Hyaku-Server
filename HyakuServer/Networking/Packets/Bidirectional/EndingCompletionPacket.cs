using System;
using HyakuServer.DataHandling;

namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class EndingCompletionPacketC2S : ClientToServerPacket
    {
        public EndingCompletionPacketC2S() : base(10) { }
        
        public override void handle(Packet packet, int clientId)
        {
            if (Enum.TryParse(packet.ReadString(), out EndingTypes ending))
            {
                if (!HyakuServer.Save.UnlockedEndings.Contains(ending))
                {
                    HyakuServer.Save.AddEnding(ending);
                    new EndingCompletionPacketS2C(ending, clientId).Send();
                    Console.WriteLine("Player " + HyakuServer.Clients[clientId].Player.username + " unlocked ending " + ending.ToString());
                }
            }else
                Console.WriteLine("Player " + HyakuServer.Clients[clientId].Player.username + " tried to send invalid Ending!");
        }
    }
    
    public class EndingCompletionPacketS2C : ServerToClientPacket
    {
        public EndingTypes Ending;
        public int ClientID;
        
        public EndingCompletionPacketS2C() : base(10) { }
        public EndingCompletionPacketS2C(EndingTypes ending, int clientId) : base(10)
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