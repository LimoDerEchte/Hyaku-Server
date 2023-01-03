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
            Console.WriteLine(String.Join(", ", clientBound.Keys) + "  |  " + String.Join(", ", serverBound.Keys));
        }

        public void Handle(Packet packet, int clientId)
        {
            int packetId = packet.ReadInt();
            try
            {
                clientBound[packetId].handle(packet, clientId);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("[Warning] Invalid Packet ID from " + clientId + " (Packet ID: " + packetId + ")");
            }
        }
        
        public static void SendTcpData(int toClient, Packet packet)
        {
            SendTcpData(toClient, packet, null);
        }
        
        public static void SendTcpData(int toClient, Packet packet, AsyncCallback callback)
        {
            packet.WriteLength();
            HyakuServer.Clients[toClient].Tcp.SendData(packet, callback);
        }

        public static void SendTcpDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= HyakuServer.MaxPlayers; i++)
            {
                HyakuServer.Clients[i].Tcp.SendData(packet, null);
            }
        }

        public static void SendTcpDataToAll(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= HyakuServer.MaxPlayers; i++)
            {
                if (i != exceptClient)
                {
                    HyakuServer.Clients[i].Tcp.SendData(packet, null);
                }
            }
        }
    }
}