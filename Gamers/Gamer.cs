using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using game.RewardSquare;

namespace game.Gamers
{
    public class Gamer
    {
        private static List<Player> players = new List<Player>();
        private static int currentPlayerIndex;

        public Gamer()
        {
            currentPlayerIndex = 0;
        }

    }

    public class Player
    {
        private static int[,] maze = null!;
        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public int Lives { get; set; }
        private static int playerX;
        private static int playerY;

        public Player(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
            Lives = 3; // Initialize lives
            playerX = 1; // Starting X position
            playerY = 0; // Starting Y position
        }

        public static void MovePlayer(string direction)
        {
            switch (direction)
            {
                case "w":
                    if (IsValidMove(playerX - 1, playerY))
                        playerX--;
                    break;
                case "s":
                    if (IsValidMove(playerX + 1, playerY))
                        playerX++;
                    break;
                case "a":
                    if (IsValidMove(playerX, playerY - 1))
                        playerY--;
                    break;
                case "d":
                    if (IsValidMove(playerX, playerY + 1))
                        playerY++;
                    break;
                default:
                    Console.WriteLine("Invalid direction. Use 'W', 'S', 'A', or 'D'.");
                    break;
            }
            ConsoleKeyInfo tecla = Console.ReadKey();
        }
        private static bool IsValidMove(int x, int y)
        {
            return x >= 0 && x < maze.GetLength(0) && y >= 0 && y < maze.GetLength(1) && maze[x, y] != '#';
        }
    }
}
