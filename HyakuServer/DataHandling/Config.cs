using System;
using System.IO;
using Newtonsoft.Json;

namespace HyakuServer.DataHandling
{
    public class Config
    {
        public int Port = 26950;
        public int QueryPort = 26951;
        public int MaxPlayers = 10;
        public string Password = "";
        public bool UseLobbies = true;

        public static Config LoadConfig()
        {
            Config saveState = new Config();
            string path = AppDomain.CurrentDomain.BaseDirectory + "Config.json";
            if (File.Exists(path))
            {
                StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();
                r.Close();
                JsonConvert.PopulateObject(json, saveState);
                Console.WriteLine("Loaded Config.json");
            }
            else
                GenerateConfig();
            return saveState;
        }

        private static readonly string FileContent =
            "{\n" +
            "  \"Port\": 26950,\n" +
            "  \"QueryPort\": 26951,\n\n" +
            "  \"MaxPlayers\": 10,\n" +
            "  \"Password\": \"\",\n\n" +
            "  \"UseLobbies\": false\n" +
            "}";

        public static void GenerateConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Config.json";
            StreamWriter w = new StreamWriter(path);
            w.Write(FileContent);
            w.Close();
            Console.WriteLine("Generated Config.json");
        }
    }
}