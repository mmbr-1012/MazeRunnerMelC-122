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
using game.Gamers;

namespace game.BoardGame
{
    public class Board
    {
        private static int[,] maze = null!;
        private static int width, height; // Removed static modifier
        private Random random = new Random();
        private (int, int) start;
        private (int, int) end;
        private (int, int) subend;
        private int maxCantPath = 4;
        private List<(int dx, int dy)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
        private List<List<(int, int)>> paths = new List<List<(int, int)>>();
        Player player = null!;

        public Board(int boardWidth, int boardHeight, int x, int y)
        {
            width = boardWidth;
            height = boardHeight;
            maze = new int[height, width];
            start = (1, 0);
            end = (28, width - 1);
            subend = (28, 118);
            FindPath();
            GenerateMaze();
            CreateObstacles();
            CreateTramps(); 
            ActiveTramp(x, y, player);
            PrintBoard();
        }

        public void GenerateMaze()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[y, x] = 1; // 1 represents a wall
                }
            }

            (int startX, int startY) = start;
            (int endX, int endY) = end;
            (int subendX, int subendY) = subend;
            maze[startX, startY] = 0; // 0 represents a path
            maze[endX, endY] = 0;
            maze[subendX, subendY] = 0;
            CarveMaze(startX, startY);
        }

        private void CarveMaze(int x, int y)
        {
            List<(int dx, int dy)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
            Shuffle(directions);

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx * 2, ny = y + dy * 2;
                if (IsInBounds(nx, ny) && maze[ny, nx] == 1 && nx != width - 1 && ny != height - 1)
                {
                    maze[ny - dy, nx - dx] = 0;
                    maze[ny, nx] = 0;
                    CarveMaze(nx, ny);
                }
            }
        }

        private void Shuffle(List<(int, int)> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x > 0 && x < width && y > 0 && y < height;
        }

        public void PrintBoard()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(maze[y, x] == 1 ? "█" :  maze[y, x] == 2? "X" : maze[y, x] == 3? "▬" : maze[y,x] == 0? " " : " ");
                    
                }
                Console.WriteLine();
            }
        }

        public void FindPath()
        {
            FindPaths(start, end, new List<(int, int)>(), 1);
        }

        private void FindPaths((int, int) start, (int, int) end, List<(int, int)> savedPath, int cantPathsFound)
        {
            if (cantPathsFound >= maxCantPath)
            {
                return;
            }

            if (start == end)
            {
                paths.Add(savedPath);
                cantPathsFound += 1;
                return;
            }

            foreach (var (dx, dy) in directions)
            {
                if (cantPathsFound == maxCantPath) break;

                (int startX, int startY) = start;
                (int endX, int endY) = end;
                (int subendX, int subendY) = subend;
                int x = dx + startX;
                int y = dy + startY;

                if (IsInBounds(x, y) && maze[x, y] == 0 && x != height - 1 && y != width - 1)
                {
                    maze[x, y] = 2;
                    savedPath.Add((x, y));
                    FindPaths((x, y), end, savedPath, maxCantPath);
                }
            }
        }
        private void CreateTramps()
        {
            (int startX, int startY) = start;
            (int endX, int endY) = end;
            int cantTramps = random.Next(20, 50);

            for (int i = 0; i < cantTramps; i++)
            {
                int x = random.Next(0, height);
                int y = random.Next(0, width);

                while ((x == startX && y == startY) || (x == endX && y == endY) || maze[x, y] != 0)
                {
                    x = random.Next(0, height);
                    y = random.Next(0, width);
                }

                maze[x, y] = random.Next(4, 12);
            }
        }

        private void CreateObstacles()
        {
            (int startX, int startY) = start;
            (int endX, int endY) = end;
            int cantObstacles = random.Next(50, 100);
            HashSet<(int, int)> occupiedPositions = new HashSet<(int, int)>();
            for (int i = 0; i < cantObstacles; i++)
            {
                int x = random.Next(0, height);
                int y = random.Next(0, width);

                while ((x == startX && y == startY) || (x == endX && y == endY) || maze[x, y] != 0 || occupiedPositions.Contains((x, y)))
                {
                    x = random.Next(0, height);
                    y = random.Next(0, width);
                }
                maze[x, y] = 3;
                occupiedPositions.Add((x, y));
            }
        }
        private void ActiveTramp(int x, int y, Player player)
        {
            int squareType = maze[x, y];

            switch (squareType)
            {
                case 4:
                    Console.WriteLine("You fell into a pitfall!");
                    break;
                case 5:
                    Console.WriteLine("You stepped on a spike!");
                    break;
                case 6:
                    Console.WriteLine("You got burned by fire!");
                    //Ejemplo de ejecucion con el player: player.Points -= 10 (decrementar sus puntos en 10, por caer en este tipo de trampa)
                    break;
                case 7:
                    Console.WriteLine("You drink Poison!");
                    break;
                case 8:
                    Console.WriteLine("You just fell into Thorns!");
                    break;
                case 9:
                    Console.WriteLine("You just fell into a Hole!");
                    break;
                case 10:
                    Console.WriteLine("Hability Blocked!");
                    break;
            }
        }
    }
}