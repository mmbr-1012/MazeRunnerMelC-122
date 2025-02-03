using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Board
    {
        public (int, int) Player1Start { get; } = (1, 1);
        public (int, int) Player2Start { get; }
        private Square[,,] maze;
        private int width;
        private int height;

        public Board(int height, int width, List<Player> players)
        {
            this.width = width;
            this.height = height;
            Player2Start = (height - 2, width - 2);
            maze = new Square[height, width, 2]; 
            
            InitializeBoard();
            PlacePlayers(players);
        }

        private void InitializeBoard()
        {
            var rand = new Random();
            
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    maze[i, j, 0] = new Square(i == 0 || i == height - 1 || j == 0 || j == width - 1 ? 
                                             SquareType.Wall : SquareType.Empty);

            AddFeatures(SquareType.Obstacle, 20);
            AddFeatures(SquareType.Trap, 15);
            
            EnsureConnectivity();
        }

        private void AddFeatures(SquareType type, int quantity)
        {
            var rand = new Random();
            for (int i = 0; i < quantity; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(1, height - 1);
                    y = rand.Next(1, width - 1);
                } while (maze[x, y, 0].Type != SquareType.Empty);
                
                maze[x, y, 0] = new Square(type);
            }
        }

        private void EnsureConnectivity()
        {
            bool[,] visited = new bool[height, width];
            FloodFill(Player1Start.Item1, Player1Start.Item2, visited);
            
            for (int i = 1; i < height - 1; i++)
                for (int j = 1; j < width - 1; j++)
                    if (!visited[i, j] && maze[i, j, 0].Type == SquareType.Empty)
                        maze[i, j, 0] = new Square(SquareType.Empty);
        }

        private void FloodFill(int x, int y, bool[,] visited)
        {
            if (x < 1 || x >= height - 1 || y < 1 || y >= width - 1 || visited[x, y] || 
                maze[x, y, 0].Type == SquareType.Wall || maze[x, y, 0].Type == SquareType.Obstacle)
                return;

            visited[x, y] = true;
            FloodFill(x + 1, y, visited);
            FloodFill(x - 1, y, visited);
            FloodFill(x, y + 1, visited);
            FloodFill(x, y - 1, visited);
        }

        private void PlacePlayers(List<Player> players)
        {
            players[0].Position = Player1Start;
            players[1].Position = Player2Start;
            UpdatePlayerLayer(players[0]);
            UpdatePlayerLayer(players[1]);
        }

        public void MovePlayer(ConsoleKeyInfo key, Player player)
        {
            var (dx, dy) = (0, 0);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.W: dx = -1; break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S: dx = 1; break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A: dy = -1; break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.D: dy = 1; break;
                case ConsoleKey.Spacebar: UseAbility(player); break;
            }

            var newPos = (player.Position.Item1 + dx, player.Position.Item2 + dy);
            if (IsValidMove(newPos))
            {
                ClearPlayerPosition(player);
                player.Position = newPos;
                UpdatePlayerLayer(player);
                CheckTrap(player);
            }
        }

        private void UseAbility(Player player)
        {
            if (player.Token.Ability.TryUse())
            {
                Console.WriteLine($"{player.Name} used {player.Token.Ability.Name}!");

            }
        }

        private void CheckTrap(Player player)
        {
            var square = maze[player.Position.Item1, player.Position.Item2, 0];
            if (square.Type == SquareType.Trap)
            {
                player.Lives--;
                Console.WriteLine($"¡{player.Name} activó una trampa! Vidas restantes: {player.Lives}");
            }
        }

        private bool IsValidMove((int, int) pos)
        {
            return pos.Item1 > 0 && pos.Item1 < height - 1 &&
                   pos.Item2 > 0 && pos.Item2 < width - 1 &&
                   maze[pos.Item1, pos.Item2, 0].Type != SquareType.Wall &&
                   maze[pos.Item1, pos.Item2, 0].Type != SquareType.Obstacle;
        }

        private void UpdatePlayerLayer(Player player)
        {
            maze[player.Position.Item1, player.Position.Item2, 1] = new Square(player);
        }

        private void ClearPlayerPosition(Player player)
        {
            maze[player.Position.Item1, player.Position.Item2, 1] = null!;
        }

        public void PrintBoard()
        {
            Console.Clear();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var playerSquare = maze[i, j, 1];
                    var terrainSquare = maze[i, j, 0];
                    
                    Console.Write(playerSquare?.Player.Token.Symbol ?? terrainSquare.Symbol);
                }
                Console.WriteLine();
            }
        }
    }
}