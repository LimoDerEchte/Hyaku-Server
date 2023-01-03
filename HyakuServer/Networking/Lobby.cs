using System.Collections.Generic;

namespace HyakuServer.Networking
{
    public class Lobby
    {
        public int Owner;
        public List<Client> Clients;

        public Lobby(Client owner)
        {
            Clients = new List<Client> { owner };
            Owner = owner.ID;
        }
    }
}