using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Services;
using System.IO;

namespace AdventureGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string eventsPath = @"C:\Users\code\AdventureGame\Services\events.json";
            string serializedEvents;
            using (StreamReader sr = new StreamReader(eventsPath))
            {
                serializedEvents = sr.ReadToEnd();
            }

            GameService gs = new GameService(serializedEvents);
            gs.StartGame();
            

            
            Console.ReadKey();
        }
    }
}
