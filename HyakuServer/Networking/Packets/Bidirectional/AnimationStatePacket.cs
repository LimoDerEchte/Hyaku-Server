namespace HyakuServer.Networking.Packets.Bidirectional
{
    public class AnimationStatePacketC2S : ClientToServerPacket
    {
        public AnimationStatePacketC2S() : base(2) { }
        
        public override void handle(Packet packet, int clientId)
        {
            new AnimationStatePacketS2C(packet.ReadInt(), packet.ReadBool(), clientId).Send();
        }
    }
    
    public class AnimationStatePacketS2C : ServerToClientPacket
    {
        public int Owner;
        public int Anim;
        public bool Facing;
        
        public AnimationStatePacketS2C() : base(2) { }
        public AnimationStatePacketS2C(int anim, bool facing, int owner) : base(2)
        {
            Owner = owner;
            Anim = anim;
            Facing = facing;
        }
        
        public override void Send()
        {
            Packet.Write(Owner);
            Packet.Write(Anim);
            Packet.Write(Facing);
            PacketHandler.SendTcpDataToLobby(Owner, Owner, Packet);
        }
    }
}