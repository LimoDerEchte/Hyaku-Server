using System;
using System.Collections.Generic;
using HyakuServer.Utility;

namespace HyakuServer.Networking.Packets
{
    public class PacketHandler
    {
        private Dictionary<int, ClientToServerPacket> clientBound;
        private Dictionary<int, ServerToClientPacket> serverBound;

        public void Init()
        {
            clientBound = new Dictionary<int, ClientToServerPacket>();
            foreach (ClientToServerPacket packet in ReflectiveEnumerator.GetEnumerableOfType<ClientToServerPacket>())
            {
                clientBound.Add(packet.ID, packet);
            }
            serverBound = new Dictionary<int, ServerToClientPacket>();
            foreach (ServerToClientPacket packet in ReflectiveEnumerator.GetEnumerableOfType<ServerToClientPacket>())
            {
                serverBound.Add(packet.ID, packet);
            }
            Console.WriteLine($"Loaded {clientBound.Count} Client to Server and {serverBound.Count} Server to Client Packets");
            Console.WriteLine(String.Join(", ", clientBound.Keys) + "  <>  " + String.Join(", ", serverBound.Keys));
        }

        public void Handle(Packet packet, int clientId)
        {
            int packetId = packet.ReadInt();
            if(clientBound.ContainsKey(packetId))
                clientBound[packetId].handle(packet, clientId);
            else
                Console.WriteLine("[Warning] Invalid Packet ID from " + clientId + " (Packet ID: " + packetId + ")");
        }
        
        public static void SendTcpData(int toClient, Packet packet)
        {
            SendTcpData(toClient, packet, null);
        }
        
        public static void SendTcpData(int toClient, Packet packet, AsyncCallback callback)
        {
            if (HyakuServer.Clients[toClient].Tcp.socket == null) return;
            packet.WriteLength();
            HyakuServer.Clients[toClient].Tcp.SendData(packet, callback);
        }

        public static void SendTcpDataToLobby(int client, Packet packet)
        {
            if (HyakuServer.Clients[client].Lobby == null) return;
            packet.WriteLength();
            foreach (Client c in HyakuServer.Clients[client].Lobby.Clients)
            {
                c.Tcp.SendData(packet, null);
            }
        }
        
        public static void SendTcpDataToLobby(int exceptClient, int client, Packet packet)
        {
            if (HyakuServer.Clients[client].Lobby == null) return;
            packet.WriteLength();
            foreach (Client c in HyakuServer.Clients[client].Lobby.Clients)
            {
                if(c.ID != exceptClient)
                    c.Tcp.SendData(packet, null);
            }
        }

        public static void SendTcpDataToAll(Packet packet)
        {
            packet.WriteLength();
            foreach (Client c in HyakuServer.Clients.Values)
            {
                c.Tcp.SendData(packet, null);
            }
        }

        public static void SendTcpDataToAll(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            foreach (Client c in HyakuServer.Clients.Values)
            {
                if (c.ID != exceptClient)
                    c.Tcp.SendData(packet, null);
            }
        }
    }
}