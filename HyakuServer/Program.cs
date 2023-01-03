using System;
using System.Threading;
using HyakuServer.DataHandling;

namespace HyakuServer
{
    internal class Program
    {
        public static Config Config;
        private static bool isRunning = true;
        
        public static void Main(string[] args)
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
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(17); 

                    if (_nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(_nextLoop - DateTime.Now); 
                    }
                }
            }
        }
    }
}