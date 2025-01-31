using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using game.RewardSquare;
using game.BoardGame;
using game.Tokens.Habilities;

namespace game.Gamers
{
    public class Player
    {
        private static int HabilityIndex;
        public string Name { get; set; }
        public int Life { get; set; }
        public int Position { get; set; }
        public string Symbol { get; set; }
        public int Health { get; set; }
        public static Dictionary<string, Hability> Habilities { get; set; } = null!;
        public int CurrentHealth { get; set; }

        public Player(string name, int life, string symbol)
        {
            Name = name;
            Life = life;
            Symbol = symbol;
            Position = 0;
            Health = 100;
            CurrentHealth = Health;
            Habilities = new Dictionary<string, Hability>();
        }
        public static void PlayGame()
        {
            Board.PrintBoard();
            // Board.UpdateHabilities();
            Console.WriteLine("Usa las flechas para moverte. Presiona ESC para salir.");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;

                    if (keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        // Mostrar menú de habilidades
                        ShowHabilitiesMenu();
                        continue;
                    }

                    Board.MovePlayer(keyInfo);
                }

                Thread.Sleep(100); // Pequeña pausa para controlar la velocidad de actualización
            }
        }
        private static void ShowHabilitiesMenu()
        {
            Console.Clear();
            Console.WriteLine("Habilidades disponibles:");
            int index = 1;
            foreach (var hability in Habilities.Values)
            {
                Console.WriteLine($"{index}. {hability.Name} - {hability.Description}");
                Console.WriteLine($"   Cooldown: {hability.CurrentCooldown}/{hability.Cooldown}");
                Console.WriteLine($"   Costo: {hability.Cost} salud");
                index++;
            }

            Console.WriteLine("\nPresiona el número de la habilidad o ESC para cancelar");
            var key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Escape)
            {
                int HabilityIndex;
                if (int.TryParse(key.KeyChar.ToString(), out HabilityIndex) &&
                    HabilityIndex > 0 && HabilityIndex <= Habilities.Count)
                {
                    var ability = Habilities.Values.ElementAt(HabilityIndex - 1);
                    // Board.UseHability(hability.Name);
                }
            }
        }
    }
}
