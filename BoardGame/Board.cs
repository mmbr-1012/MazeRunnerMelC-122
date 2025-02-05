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
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[i, j, 0] ??= new Square(SquareType.Wall);
                }
            }

            GenerateMaze();

            AddFeatures(SquareType.Obstacle, 150);
            AddFeatures(SquareType.Trap, 100);
        }

        private void GenerateMaze()
        {
            var rand = new Random();
            Stack<(int x, int y)> stack = new Stack<(int, int)>();

            stack.Push(Player1Start);
            maze[Player1Start.Item1, Player1Start.Item2, 0] = new Square(SquareType.Empty);

            int[] dirX = { -2, 2, 0, 0 };
            int[] dirY = { 0, 0, -2, 2 };

            while (stack.Count > 0)
            {
                var (x, y) = stack.Pop();

                var directions = new List<int> { 0, 1, 2, 3 };
                Shuffle(rand, directions);

                foreach (int dir in directions)
                {
                    int newX = x + dirX[dir];
                    int newY = y + dirY[dir];

                    if (newX > 0 && newX < height - 1 &&
                        newY > 0 && newY < width - 1 &&
                        maze[newX, newY, 0].Type == SquareType.Wall)
                    {
                        int midX = x + dirX[dir] / 2;
                        int midY = y + dirY[dir] / 2;
                        maze[midX, midY, 0] = new Square(SquareType.Empty);
                        maze[newX, newY, 0] = new Square(SquareType.Empty);
                        stack.Push((newX, newY));
                    }
                }
                maze[Player1Start.Item1, Player1Start.Item2, 0] = new Square(SquareType.Empty);
                maze[Player2Start.Item1, Player2Start.Item2, 0] = new Square(SquareType.Empty);

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int X = Player2Start.Item1 + i;
                        int Y = Player2Start.Item2 + j;
                        if (X > 0 && X < height - 1 && Y > 0 && Y < width - 1)
                        {
                            maze[X, Y, 0] = new Square(SquareType.Empty);
                        }
                    }
                }
            }

            CreateGuaranteedPaths();
        }

        private void CreateGuaranteedPaths()
        {
            var rand = new Random();

            for (int i = 0; i < 4; i++)
            {
                int centerX = height / 2;
                int centerY = width / 2;

                for (int j = 0; j < 15; j++)
                {
                    int dirX = rand.Next(-1, 2);
                    int dirY = rand.Next(-1, 2);

                    if (centerX > 1 && centerX < height - 2 && centerY > 1 && centerY < width - 2)
                    {
                        maze[centerX, centerY, 0] = new Square(SquareType.Empty);
                        centerX += dirX;
                        centerY += dirY;
                    }
                }
            }

            ConnectPoints(Player1Start, (height / 2, width / 2));
            ConnectPoints(Player2Start, (height / 2, width / 2));
        }

        private void ConnectPoints((int x, int y) start, (int x, int y) end)
        {
            int x = start.x;
            int y = start.y;

            while (x != end.x || y != end.y)
            {
                if (x < end.x) x++;
                else if (x > end.x) x--;

                if (y < end.y) y++;
                else if (y > end.y) y--;

                maze[x, y, 0] = new Square(SquareType.Empty);
            }
        }

        private void Shuffle(Random rand, List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void AddFeatures(SquareType type, int quantity)
        {
            var rand = new Random();
            for (int i = 0; i < quantity; i++)
            {
                int x, y;
                int attempts = 0;
                bool validPosition = false;

                do
                {
                    x = rand.Next(1, height - 1);
                    y = rand.Next(1, width - 1);

                    validPosition = maze[x, y, 0].Type == SquareType.Empty && !IsNearStart((x, y), 10) && !IsMainPath((x, y));

                    attempts++;
                } while (!validPosition && attempts < 100);

                if (validPosition)
                {
                    maze[x, y, 0] = new Square(type);
                }
            }
        }

        private bool IsMainPath((int x, int y) pos)
        {
            int centerX = height / 2;
            int centerY = width / 2;

            return Math.Abs(pos.x - centerX) < 5 && Math.Abs(pos.y - centerY) < 5;
        }

        private bool IsNearStart((int x, int y) pos, int radius)
        {
            return (Math.Abs(pos.x - Player1Start.Item1) < radius &&
                    Math.Abs(pos.y - Player1Start.Item2) < radius) ||
                   (Math.Abs(pos.x - Player2Start.Item1) < radius &&
                    Math.Abs(pos.y - Player2Start.Item2) < radius);
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

            if (IsValidMove(newPos, player))
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
                Console.WriteLine($"{player.Name} usó {player.Token.Ability.Name}!");

                switch (player.Token.Name)
                {
                    case "Warrior":
                        player.ShieldActive = true;
                        Console.WriteLine("¡Escudo activado! La próxima trampa será ignorada.");
                        break;

                    case "Mage":
                        player.Lives = Math.Min(player.Lives + 1, 3);
                        Console.WriteLine("¡+1 vida! Vidas actuales: " + player.Lives);
                        break;

                    case "Rogue":
                        player.ExtraMoves = 1;
                        Console.WriteLine("¡Movimiento rápido! Podrás moverte de nuevo.");
                        break;

                    case "Archer":
                        player.PhaseActive = true;
                        Console.WriteLine("¡Visión de águila! Ignora obstáculos por 1 turno.");
                        break;

                    case "Necro":
                        player.HasRevive = true;
                        Console.WriteLine("¡Pacto oscuro! Revivirás una vez si pierdes todas las vidas.");
                        break;
                }

                UpdateAbilityEffects(player);
            }
            else
            {
                Console.WriteLine($"¡{player.Token.Ability.Name} no está listo! Turnos restantes: {player.Token.Ability.currentCooldown}");
            }
        }

        private void UpdateAbilityEffects(Player player)
        {
            if (player.PhaseActive || player.ShieldActive)
            {
                int x = player.Position.Item1;
                int y = player.Position.Item2;

                var original = maze[x, y, 1];

                maze[x, y, 1] = new Square(player)
                {
                    Symbol = player.PhaseActive ? "🌀" : "🛡"
                };

                PrintBoard();
                Thread.Sleep(500);

                maze[x, y, 1] = original;
            }
        }

        private void CheckTrap(Player player)
        {
            var square = maze[player.Position.Item1, player.Position.Item2, 0];

            if (square.Type == SquareType.Trap)
            {
                if (player.ShieldActive)
                {
                    player.ShieldActive = false;
                    Console.WriteLine("¡Escudo bloqueó la trampa!");
                }
                else
                {
                    player.Lives--;
                    Console.WriteLine($"¡{player.Name} cayó en una trampa! Vidas: {player.Lives}");

                    if (player.Lives <= 0 && player.HasRevive)
                    {
                        player.Lives = 1;
                        player.HasRevive = false;
                        Console.WriteLine("¡Pacto oscuro activado! Revivido con 1 vida.");
                    }
                }
            }
        }

        private bool IsValidMove((int, int) pos, Player currentPlayer)
        {
            if (pos.Item1 <= 0 || pos.Item1 >= height - 1 || pos.Item2 <= 0 || pos.Item2 >= width - 1)
            {
                return false;
            }

            var cell = maze[pos.Item1, pos.Item2, 0];
            return cell.Type != SquareType.Wall && (currentPlayer.PhaseActive || cell.Type != SquareType.Obstacle);
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
                    Console.Write(playerSquare != null ? playerSquare.Player.Token.Symbol : terrainSquare.Symbol);
                }
                Console.WriteLine();
            }
        }
    }
}