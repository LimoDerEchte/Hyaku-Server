
using System;
using HyakuServer.Networking.Packets.ServerToClient;

namespace HyakuServer.Networking.Packets.ClientToServer
{
    public class WelcomePacket : ClientToServerPacket
    {
        public WelcomePacket(): base(999)
        {
        }

        public override void handle(Packet packet, int clientId)
        {
            int assumedId = packet.ReadInt();
            string username = packet.ReadString();
            string password = packet.ReadString();
            string modVersion = packet.ReadString();

            if (modVersion == HyakuServer.ModVersion)
            {
                if (HyakuServer.Password == null || password.Equals(HyakuServer.Password))
                {
                    Console.WriteLine(
                        $"{HyakuServer.Clients[clientId].Tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {clientId} with username {username}");
                    if (clientId != assumedId)
                        Console.WriteLine($"[Warning] Player {username} (ID: {clientId}) has assumed the wrong client ID ({assumedId})!");
                    HyakuServer.Clients[clientId].SendIntoGame(username);
                }
                else
                    new KickPacket("Wrong Password", clientId).Send();
            }
            else
                new KickPacket("Mismatching Client Version", clientId).Send();
        }
    }
}
