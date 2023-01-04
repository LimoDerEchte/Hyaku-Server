
using System;
using System.Net.Sockets;
using HyakuServer.DataHandling;
using HyakuServer.Networking.Packets;
using HyakuServer.Networking.Packets.Bidirectional;
using HyakuServer.Networking.Packets.ServerToClient;
using HyakuServer.Utility;

namespace HyakuServer.Networking
{
    public class Client 
    {
        public const int DataBufferSize = 4096;

        public int ID;
        public Player Player;
        public TCP Tcp;
        public Lobby Lobby;
        
        public long LastPacket;

        public Client(int _clientId)
        {
            ID = _clientId;
            Tcp = new TCP(ID);
        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private Packet receivedData;
            private byte[] receiveBuffer;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = DataBufferSize;
                socket.SendBufferSize = DataBufferSize;

                stream = socket.GetStream();

                receivedData = new Packet();
                receiveBuffer = new byte[DataBufferSize];

                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

                new WelcomePacket(id).Send();
            }

            public void SendData(Packet _packet, AsyncCallback callback)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), callback, null); 
                    }
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
                    HyakuServer.Clients[id].Disconnect();
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        new KickPacket("Invalid Packet Received", id).Send();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);

                    receivedData.Reset(HandleData(_data)); 
                    stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"[Error] Error receiving TCP data: {_ex}");
                    new KickPacket("Serverside Error", id).Send();
                }
            }

            private bool HandleData(byte[] _data)
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true; 
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            HyakuServer.PacketHandler.Handle(_packet, id);
                        }
                    });

                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true; 
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true;
                }

                return false;
            }

            public void Disconnect()
            {
                if(socket != null && socket.Connected) socket.Close();
                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }
        
        public void SendIntoGame(string username)
        {
            if (username.Length < 3 || username.Length > 20)
            {
                new KickPacket("Invalid Username", ID).Send();
                return;
            }
            Player = new Player(ID, username, new Texture2D(0, 0));
            new InitiateSaveStatePacket(ID).Send();
            new ChatMessageS2C($"<color=lightblue>{username} joined.</color>").Send();
            foreach (Client client in HyakuServer.Clients.Values)
            {
                if (ID != client.ID)
                {
                    new SpawnPlayerPacket(Player.username, Player.id, Player.Texture, client.ID).Send();
                    if (client.Player != null)
                    {
                        new SpawnPlayerPacket(client.Player.username, client.Player.id, client.Player.Texture, ID).Send();
                    }
                }
            }
        }

        public void Disconnect()
        {
            Tcp.Disconnect();
            if (Player != null)
            {
                Console.WriteLine($"{Player.username} disconnected.");
                new ChatMessageS2C($"<color=lightblue>{Player.username} left.</color>").Send();
                new DespawnPlayerPacket(ID).Send();
                Player = null;
            }
        }
    }
}