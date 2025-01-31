using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using game.RewardSquare;
using game.Gamers;
using static game.MazeGame;
using game.BoardGame;
using game.Tokens;
using Spectre.Console;

namespace game.Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int width = 120;
            int height = 30;
            string direction = null!;
            string[] tokens = { "Heart", "Clover", "Star", "Diamond", "Moon", "Sun" };
            string[] symbols = { "❤️", "☘️", "⭐", "💎", "🌙", "☀️" };

            Console.WriteLine("Enter your name:");
            string playerName = Console.ReadLine()!;

            Console.WriteLine("Select a symbol for your character:");
            for (int i = 0; i < tokens.Length; i++)
            {
                Console.WriteLine($"{tokens[i]} : {symbols[i]}");
            }
            string selectedSymbol = Console.ReadLine()!;

            Board board = new Board(width, height,0,0);
            MazeGame.StartGame();
            Player.MovePlayer(direction);
        }
    }
}