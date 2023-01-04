
using System;
using HyakuServer.DataHandling;
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
            string modVersion = packet.ReadString();
            string username = packet.ReadString();
            string password = packet.ReadString();
            string lobby = packet.ReadString();

            if (modVersion == HyakuServer.ModVersion)
            {
                if (Lobby.NameRegex.IsMatch(lobby))
                {
                    if (Player.NameRegex.IsMatch(username))
                    {
                        Lobby l = !HyakuServer.Lobbies.ContainsKey(lobby) ? new Lobby(HyakuServer.Clients[clientId], password, lobby) : HyakuServer.Lobbies[lobby];
                        if (password.Equals(HyakuServer.Lobbies[lobby].Password))
                        {
                            if(l != null && l.Owner == clientId)
                                HyakuServer.Lobbies.Add(lobby, l);
                            else if (l != null) l.Clients.Add(HyakuServer.Clients[clientId]);
                            Console.WriteLine($"{HyakuServer.Clients[clientId].Tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {clientId} with username {username}");
                            if (clientId != assumedId)
                                Console.WriteLine($"[Warning] Player {username} (ID: {clientId}) has assumed the wrong client ID ({assumedId})!");
                            HyakuServer.Clients[clientId].SendIntoGame(username);
                        }
                        else
                            new KickPacket("Wrong Password", clientId).Send();
                    }
                    else
                        new KickPacket("Invalid Username", clientId).Send();
                }
                else
                    new KickPacket("Invalid Lobby Name", clientId).Send();
            }
            else
                new KickPacket("Mismatching Client Version", clientId).Send();
        }
    }
}
