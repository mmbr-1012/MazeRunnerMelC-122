using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using game.RewardSquare;
using System.Runtime.CompilerServices;
using System.Data;
using static game.MazeGame;
using game.Program;
using game.Tokens;

namespace game.Gamers
{
    public class Gamer(string name)
    {
        int mover = 0;
        private int[,] maze = null!;
        string name = name;
        int score = 0;
        Token selectedToken = null!;
        int position;
        public static List<Gamer> gamers = null!;
        private List<Player> players = null!;
        private (int, int) playerPosition;
        private static readonly int tokenIndex;
        private static readonly Token[] tokens = null!;

        public Gamer(int width, int height)
        {
            this.score = 0;
            this.selectedToken = null!;
            this.position = 0;
        }

        public string Name { get { return name; } }
        public int Score { get { return score; } }
        public Token[] Tokens { get; } = tokens;
        public Token[] SelectedToken { get { return SelectedToken; } }
        public int Position { get { return position; } }
        public List<Player> Players { get { return players; } }
        public void MovePlayers(int player, int steps, int move)
        {
            int newx = playerPosition.Item1;
            int newy = playerPosition.Item2;
            switch (steps)
            {
                case 'w': // Arriba
                newx--;
                break;
                case 's': // Abajo
                newx++;
                break;
                case 'a': // Izquierda
                newy--;
                break;
                case 'd': // Derecha
                newy++;
                break;
                default:
                Console.WriteLine("Movimiento inválido");
                return;
            }
            if (IsValidMove(newx, newy))
            {
                playerPosition = (newx, newy);
            }
            else
            {
                Console.WriteLine("No puede atravesar paredes");
            }
        }
        public void AddPlayer(Player player)
        {
            players.Add(player);
            Console.WriteLine($"Gamer {player.Name} added to the board.");
        }
        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }
        private bool IsValidMove(int x, int y)
        {
            return x >= 0 && x < maze.GetLength(0) && y >= 0 && y < maze.GetLength(1);
        }
        public void PrintTokens()
        {
            foreach (var item in Tokens)
            {
                Console.WriteLine($"tokens.IndexOf(item), item.name");
            }
            SelectToken(tokenIndex);
        }
        public static void PrintMenu()
        {
            Console.WriteLine("1. Start");
            Console.WriteLine("2. Exit");
        }
        public static void Start()
        {
            PrintMenu();
        }
        public void SelectToken(int tokenIndex)
        {
            if (tokenIndex >= 0 && tokenIndex < Tokens.Length)
            {
                selectedToken = Tokens[tokenIndex];
            }
            Console.WriteLine("Escoja una ficha:");
        }
        internal void Move(int steps)
        {
            position += steps;
        }

        public int IncrementScore(int points)
        {
            score += points;
            return score;
        }

        public int DecrementScore(int points)
        {
            score -= points;
            return score;
        }

    }
}