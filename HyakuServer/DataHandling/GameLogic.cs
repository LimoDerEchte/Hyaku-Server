using System;
using HyakuServer.Networking;
using HyakuServer.Networking.Packets.Bidirectional;
using HyakuServer.Networking.Packets.ServerToClient;
using HyakuServer.Utility;

namespace HyakuServer.DataHandling
{
    public class GameLogic
    {
        public static int KeepAliveCooldown = 60;
        public static int SaveCooldown = 18000;
        
        public static void Update()
        {
            ThreadManager.Update();
            KeepAliveCooldown--;
            SaveCooldown--;
            if (KeepAliveCooldown <= 0)
            {
                KeepAliveCooldown = 60;
                new KeepAlivePacketS2C().Send();
                long currentTime = DateTime.UtcNow.Ticks;
                foreach (Client client in HyakuServer.Clients.Values)
                {
                    if (client.Tcp.socket != null)
                    {
                        long offset = currentTime - client.LastPacket;
                        if (new TimeSpan(offset).TotalSeconds > 3)
                        {
                            new KickPacket("Timed Out", client.ID).Send();
                        }
                    }
                }
            }

            if (SaveCooldown <= 0)
            {
                SaveCooldown = 18000;
                HyakuServer.Save.SaveSaveState();
            }
        }
    }
}