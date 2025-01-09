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

namespace game.BoardGame
{
    public class Board
    {
        public static void Main()
        {
            int width = 100;
            int height = 30;
            Board board = new Board(width, height);
        }
        private int[,] maze;
        private int width, height;
        private Random random = new Random();
        private (int, int) start;
        private (int, int) end;
        private (int, int) subend;
        private int cantPathsFound = 4;
        private List<(int dx, int dy)>directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
        private List<List<(int, int)>> paths = new List<List<(int, int)>>();

        public Board(int width, int height)
        {
            this.width = width;
            this.height = height;
            maze = new int[height, width];
            start = (1, 0);
            end = (28, width - 1);
            subend = (28,98);
            // end = (2, 2);
            
            FindPath();
           GenerateMaze();
           PrintBoard();
        }
        public void GenerateMaze()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[y, x] = 1; // 1 representa una pared
                }
            }

        // Empieza desde un punto aleatorio
            (int startX, int startY) = start;
            (int endX, int endY) = end;
            (int subendX, int subendY) = subend;
            maze[startX, startY] = 0; // 0 representa un camino
            maze[endX, endY] = 0;
            maze[subendX, subendY] = 0;
        // Usa el algoritmo de Recursive Backtracking
            CarveMaze(startX, startY);
        }

        private void CarveMaze(int x, int y)
        {
            List<(int dx, int dy)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
            Shuffle(directions);

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx*2 , ny = y + dy *2;
                // int nx = x + dx , ny = y + dy;
                
                if (IsInBounds(nx, ny) && maze[ny, nx] == 1 && nx != width -1 && ny != height - 1)
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
                    Console.Write(maze[y, x] == 1 ? "█" : " ");
                }
                Console.WriteLine();
            }
        }

        public void FindPath()
        {
            FindPaths(start, end, new List<(int, int)>(), 1);
        }
        
        private void FindPaths((int, int) start, (int, int) end, List<(int, int)> savedPath, int maxCantPath)
        {
             if(cantPathsFound >= maxCantPath) 
            {
                return;
            }

            if(start == end) 
            {
                paths.Add(savedPath);
                cantPathsFound += 1;
                return;
            }

            foreach(var (dx, dy) in directions) 
            {
                if(cantPathsFound == maxCantPath) break;

                (int startX, int startY) = start;
                (int endX, int endY) = end;
                (int subendX, int subendY) = subend;
                int x = dx + startX;
                int y = dy + startY;

                // if(IsInBounds(x, y))
                //     Console.WriteLine(maze[x,y] + "," + x + "," + y );
                if(IsInBounds(x, y) && maze[x,y] == 0 && x != height - 1 && y != width - 1) 
                {
                    maze[x, y] = 0;
                    savedPath.Add((x, y));
                    FindPaths((x, y), end, savedPath, maxCantPath);
                }
            }
        }
    }
}
