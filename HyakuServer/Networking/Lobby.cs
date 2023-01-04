using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HyakuServer.Networking
{
    public class Lobby
    {
        public static readonly Regex NameRegex = new Regex("^[a-zA-Z0-9_-]{2,32}$");
        
        public int Owner;
        public readonly List<Client> Clients;
        public readonly string Password;
        public readonly string Name;

        public Lobby(Client owner, string password, string name)
        {
            Clients = new List<Client> { owner };
            Owner = owner.ID;
            Password = password;
            Name = name;
        }
    }
}