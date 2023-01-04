
using System.Text.RegularExpressions;
using HyakuServer.Utility;

namespace HyakuServer.DataHandling
{
    public class Player
    {
        public static readonly Regex NameRegex = new Regex("^[a-zA-Z0-9_-]{3,16}$");
        
        public int id;
        public string username;
        public Texture2D Texture;

        public Player(int _id, string _username, Texture2D _texture)
        {
            id = _id;
            username = _username;
            Texture = _texture;
        }
    }
}