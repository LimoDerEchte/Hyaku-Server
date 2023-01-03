using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace HyakuServer.DataHandling
{
    public class SaveState
    {
        public readonly List<EndingTypes> UnlockedEndings;
        public readonly List<EndingTypes> UnlockedHints;

        private SaveState()
        {
            UnlockedEndings = new List<EndingTypes>();
            UnlockedHints = new List<EndingTypes>();
        }

        public static SaveState LoadSaveState()
        {
            SaveState saveState = new SaveState();
            string path = AppDomain.CurrentDomain.BaseDirectory + "SaveState.json";
            if (File.Exists(path))
            {
                StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();
                /*string line = r.ReadLine();
                while (line != null)
                {
                    json += line;
                    line = r.ReadLine();
                }*/
                r.Close();
                JsonConvert.PopulateObject(json, saveState);
                Console.WriteLine("Loaded " + saveState.UnlockedEndings.Count + " unlocked Endings");
            }
            return saveState;
        }

        public void SaveSaveState()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "SaveState.json";
            string json = JsonConvert.SerializeObject(this);
            StreamWriter w = new StreamWriter(path);
            w.Write(json);
            w.Close();
            Console.WriteLine("Saved the current Game");
        }

        public void AddEnding(EndingTypes ending)
        {
            if (UnlockedHints.Contains(ending))
                UnlockedHints.Remove(ending);
            UnlockedEndings.Add(ending);
        }

        public void AddHint(EndingTypes ending)
        {
            if(!(UnlockedEndings.Contains(ending) || UnlockedHints.Contains(ending)))
                UnlockedHints.Add(ending);
        }
    }
}