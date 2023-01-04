using System.Collections.Generic;

namespace HyakuServer.Networking
{
    public class Lobby
    {
        public int Owner;
        public List<Client> Clients;
        public string Password;

        public Lobby(Client owner, string password)
        {
            Clients = new List<Client> { owner };
            Owner = owner.ID;
            Password = password;
        }
    }
}