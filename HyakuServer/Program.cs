using System;
using System.Threading;
using HyakuServer.DataHandling;

namespace HyakuServer
{
    internal class Program
    {
        public static Config Config;
        private static bool _isRunning = true;
        
        public static void Main()
        {
            Config = Config.LoadConfig();
            Console.WriteLine("Starting Hyaku Server...");
            HyakuServer.Init(Config.Port, Config.MaxPlayers, Config.Password);
            HyakuServer.Start();
            new Thread(MainThread).Start();
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {60} ticks per second.");
            DateTime nextLoop = DateTime.Now;

            while (_isRunning)
            {
                while (nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    nextLoop = nextLoop.AddMilliseconds(17); 

                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now); 
                    }
                }
            }
        }
    }
}