
using System.Text.RegularExpressions;
using HyakuServer.Utility;

namespace HyakuServer.DataHandling
{
    public class Player
    {
        public static readonly Regex NameRegex = new Regex("^[a-zA-Z0-9_-]{3,16}$");
        
        public int Id;
        public string Username;
        public Texture2D Texture;

        public Player(int id, string username, Texture2D texture)
        {
            Id = id;
            Username = username;
            Texture = texture;
        }
    }
}