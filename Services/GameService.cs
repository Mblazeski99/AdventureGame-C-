using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Entities;


namespace Services
{
    public class GameService
    {
        protected class Hero
        {
            public int CurrentPoint { get; set; }
            public int Health { get; set; }
            public int Armor { get; set; }
            public int Food { get; set; }

            public Hero(int health, int armor, int food)
            {
                CurrentPoint = 0;
                Health = health;
                Armor = armor;
                Food = food;
            }
        }

        private char answer;
        private List<Event> _events;
        private Random _rnd = new Random();
        private int _rolledDice;
        private Hero _jack = new Hero(60, 40, 20);

        public GameService(string serializedEvents)
        {
            _events = JsonConvert.DeserializeObject<List<Event>>(serializedEvents);
        }

        public void StartGame()
        {
            _jack.CurrentPoint = 0;
            _jack.Health = 60;
            _jack.Armor = 40;
            _jack.Food = 20;

            Console.WriteLine("Press Any key to start");
            Console.ReadKey();
            Console.WriteLine($@"Health: {_jack.Health}
Armor: {_jack.Armor}
Food: {_jack.Food}");

            while(_jack.CurrentPoint < 30)
            {
                RollDice();
            }
        }

        private void RollDice()
        {

            Console.WriteLine("Press any key to roll the dice!");
            if (Console.ReadLine() == "aspirin") _jack.Health = 999;
            _rolledDice = _rnd.Next(1, 4);            
            _jack.CurrentPoint += _rolledDice;
            if (_jack.CurrentPoint < 30) TriggerEvent(_events[_jack.CurrentPoint - 1]);
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("YOU HAVE WON!!!!");
                Console.WriteLine("Play Again? (y/n)");
                Console.ResetColor();
                answer = Console.ReadKey().KeyChar;
                if (answer == 'y' || answer == 'Y')
                {
                    Console.Clear();
                    StartGame();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("guess not");
                }
            }

        }

        private void TriggerEvent(Event ev)
        {           
            switch (ev.Type)
            {
                case (EventType.Neutral):
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case (EventType.Good):
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case (EventType.Bad):
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.WriteLine(ev.Title);
            Console.ResetColor();
            Console.WriteLine(ev.Description);            

            if(ev.HealthModifier < 0)
            {
                if (_jack.Armor > 0)
                {
                    ev.HealthModifier += 5;
                    _jack.Health += ev.HealthModifier;
                }
                else _jack.Health += ev.HealthModifier;
            } else _jack.Health += ev.HealthModifier;

            if (ev.ArmorModifier < 0)
            {
                if (_jack.Armor > 0) _jack.Armor += ev.ArmorModifier;
            }
            else _jack.Armor += ev.ArmorModifier;

            if (ev.FoodModifier < 0)
            {
                if (_jack.Food > 0) _jack.Food += ev.FoodModifier;
                else
                {
                    _jack.Food += ev.FoodModifier;
                    _jack.Health -= 5;
                }
            }
            else _jack.Food += ev.FoodModifier;

            if (_jack.Armor < 0) _jack.Armor = 0;
            if (_jack.Food < 0) _jack.Food = 0;

            Console.WriteLine($@"Curret Health: {_jack.Health}
Current Armor: {_jack.Armor}
Current Food: {_jack.Food}
Current Point: {_jack.CurrentPoint}");

            if (_jack.Health <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("YOU HAVE DIED!!!");
                Console.WriteLine("Play Again? (Y/N)");
                Console.ResetColor();
                answer = Console.ReadKey().KeyChar;
                if (answer == 'y' || answer == 'Y')
                {
                    Console.Clear();
                    StartGame();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("guess not");
                }
            }          
        }
    }
}