using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using HyakuServer.DataHandling;
using HyakuServer.Networking;
using HyakuServer.Networking.Packets;
using HyakuServer.Utility;

namespace HyakuServer
{
    public class HyakuServer
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        public static string Password { get; private set; }
        public static SaveState Save { get; private set; }
        public static readonly Dictionary<int, Client> Clients = new Dictionary<int, Client>();
        public static readonly Dictionary<string, Lobby> Lobbies = new Dictionary<string, Lobby>();
        public static readonly string ModVersion = "0.1.3"; 

        private static TcpListener _tcpListener;
        public static PacketHandler PacketHandler;
        
        public static void Init(int port, int maxPlayers, string password)
        {
            PacketHandler = new PacketHandler();
            PacketHandler.Init();
            Port = port;
            if(password != "") Password = Hashing.GetHashString(password);
            MaxPlayers = maxPlayers;
            Save = SaveState.LoadSaveState();
            for (int i = 1; i <= MaxPlayers; i++)
            {
                Clients[i] = new Client(i);
            }
        }
        
        public static void Start()
        {
            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            _tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            Console.WriteLine($"Server started on port {Port}");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = _tcpListener.EndAcceptTcpClient(_result);
            _tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (Clients[i].Tcp.socket == null)
                {
                    Clients[i].LastPacket = DateTime.UtcNow.Ticks;
                    Clients[i].Tcp.Connect(_client);
                    return;
                }
            }
            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }
    }
}
