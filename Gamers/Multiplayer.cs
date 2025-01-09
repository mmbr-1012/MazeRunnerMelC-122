using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.MultiplayerGame
{
    public class Player
    {
        int name;
        private int newX;
        private int newY;

        public Player(int name, int newX, int newY)
        {
            this.name = name;
            this.newX = newX;
            this.newY = newY;
        }

        public int Name { get {return name;}}
        public int XPosition { get; }
        public int YPosition { get; }
    }
    public class GameBoard
    {
        private int width;
        private int height;
        private bool[,] maze; //no
        private List<Player>players; //no
        public GameBoard(int width, int height)
        {
            this.width = width;
            this.height = height;
            maze = new bool[width, height];
            players = new List<Player>();
        }
        public void AddPlayer(Player player)
        {
            players.Add(player);
        }
        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }
        public void UpdatePlayerPosition(Player player, int newX, int newY)
        {
            if (IsValidMove(player.XPosition, player.YPosition, newX, newY))
            {
                players[players.IndexOf(player)] = new Player(player.Name, newX, newY);
            }
        }

        private bool IsValidMove(int startX, int startY, int endX, int endY)
        {
            return IsInBounds(endX, endY) && !maze[endX, endY];
        }
        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }

        public void CheckCollisions()
        {
            // Verifica si algún jugador choca con otro o con un obstáculo
        }

        public void PrintBoard()
        {
            char[,] display = new char[width, height];
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    display[x, y] = '█';
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!maze[x,y])
                    {
                        display[x,y] = ' ';
                    }
                }
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(display[x, y]);
                }
                Console.WriteLine();
            }
        }
    }

}


