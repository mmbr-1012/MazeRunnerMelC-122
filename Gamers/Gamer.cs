using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using game.BoardGame;
using game.Tokens.Habilities;

namespace game.Gamers
{
    public class Player
    {
        public string Name { get; set; }
        public int Lives { get; set; }
        public (int X, int Y) Position { get; set; }
        public string Symbol { get; set; }
        public int Health { get; set; }
        public static Dictionary<string, Hability> Habilities { get; set; } = new Dictionary<string, Hability>();
        public int CurrentHealth { get; set; }
        public event Action<Player> OnDeath = null!;
        public int Shield { get; set; }
        (int, int) startPosition ;
        private static Player player = null!;

        public Player(string name, string symbol)
        {
            Name = name;
            Lives = 3;
            Symbol = symbol;
            Position = startPosition;
            Health = 100;
            Shield = 0;
            CurrentHealth = Health;
            Habilities = new Dictionary<string, Hability>();
        }
        public void LoseLife()
        {
            if (Lives > 0)
            {
                Lives--;
                Console.WriteLine($"{Name} perdió una vida! Vidas restantes: {Lives}");
                if (Lives <= 0)
                {
                    Console.WriteLine($"{Name} ha sido eliminado!");
                    Die();
                }
            }
        }

        public static void PlayGame()
        {
            Board.PrintBoard();
            Board.UpdateHabilities();
            Console.WriteLine("Presiona ESC para salir.");
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Escape)
                        break;
                    Board.MovePlayer(keyInfo, player);
                }

                Thread.Sleep(100);
            }
        }
        
        public void TakeDamage(int damage)
        {
            if (Shield > 0)
            {
                Shield -= damage;
                if (Shield < 0)
                {
                    CurrentHealth += Shield;
                    Shield = 0;
                }
            }
            else
            {
                CurrentHealth -= damage;
                if (CurrentHealth < 0)
                {
                    CurrentHealth = 0;
                }
            }
        }

        private void Die()
        {
            Console.WriteLine($"{Name} has died!");
            OnDeath?.Invoke(this);
        }

        public void BlockAbilities()
        {
            foreach (var hability in Habilities.Values)
            {
                Hability.Block();
            }
        }

        internal void AddShield(int shieldStrength)
        {
            Shield += shieldStrength;
        }
        private List<Player> GetPlayersInRange(Player source, int range)
        {
            List<Player> playersInRange = new List<Player>();
            return playersInRange;
        }
    }
    public static class GameManager
    {
        private static readonly Player[] players;
        private static Player _currentPlayer = null!;

        public static Player player { get; private set; } = null!;

        public static void StartGame(Board board, Player player1, Player player2)
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Tab)
                    {
                        _currentPlayer = player1;
                        Console.WriteLine($"Turno de: {_currentPlayer.Name}");
                        continue;
                    }
                    Board.MovePlayer(key, player);
                    Board.PrintBoard();

                    if (player1.Lives <= 0 || player2.Lives <= 0)
                    {
                        Console.WriteLine("¿Reiniciar? (R)");
                        if (Console.ReadKey().Key == ConsoleKey.R)
                        {
                            player1.Lives = 5;
                            player2.Lives = 5;
                            board = new Board(0, 0, 0, 0, players);
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}