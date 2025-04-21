using System;
using System.Collections.Generic;
using System.Threading;
using Spectre.Console;

namespace BoardGame
{
    public class Board
    {
        public (int, int) Player1Start { get; } = (1, 1);
        public (int, int) Player2Start { get; } = (23, 49);
        private Square[,,] maze;
        private int width;
        private int height;
        public (int, int) AltarCentral { get; set; }
        private List<(int, int)> fragmentPositions = new List<(int, int)>();
        private readonly int players;

        public Board(int height, int width, List<Player> players)
        {
            maze = new Square[height, width, 2];
            this.height = height;
            this.width = width;

            InitializeBoard();
            IsBoardConnected();
            PlacePlayers(players);
            PlaceFragments(3);
        }

        private void PlaceFragments(int quantity)
        {
            var rand = new Random();
            for (int i = 0; i < quantity; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(1, Math.Max(2, height - 1));
                    y = rand.Next(1, Math.Max(2, width - 1));
                } while (maze[x, y, 0].Type != SquareType.Empty || IsNearStart((x, y), 5));

                fragmentPositions.Add((x, y));
                maze[x, y, 0].Symbol = "◍"; // Símbolo del fragmento
            }
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[i, j, 0] = new Square(SquareType.Wall); // Inicializar como muro
                }
            }
            GenerateMaze(); // Generar caminos
            CreateGuaranteedPaths();
            AddFeatures(SquareType.Obstacle, 20); // 20 obstáculos
            AddFeatures(SquareType.Trap, 10); // 10 trampas aleatorias adicionales
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
            }

            // Asegurar conexiones adicionales
            CreateGuaranteedPaths();
            AddConnectionPaths();
        }

        private void AddConnectionPaths()
        {
            var rand = new Random();

            // Crear conexiones adicionales entre áreas aleatorias
            for (int i = 0; i < 8; i++)
            {
                int x1 = rand.Next(1, height - 2);
                int y1 = rand.Next(1, width - 2);
                int x2 = rand.Next(1, height - 2);
                int y2 = rand.Next(1, width - 2);

                if (maze[x1, y1, 0].Type == SquareType.Empty &&
                    maze[x2, y2, 0].Type == SquareType.Empty)
                {
                    ConnectPoints((x1, y1), (x2, y2));
                }
            }
        }

        private void CreateGuaranteedPaths()
        {
            var rand = new Random();

            for (int i = 0; i < 4; i++)
            {
                CreateRandomPath(Player1Start, (height / 2, width / 2), i);
                CreateRandomPath(Player2Start, (height / 2, width / 2), i);
            }
        }

        private void CreateRandomPath((int x, int y) start, (int x, int y) end, int pathNumber)
        {
            int centerX = start.x;
            int centerY = start.y;
            var rand = new Random();

            // Ajustar el camino según el número para crear rutas diferentes
            int offsetX = (pathNumber - 1) * 3; 

            for (int j = 0; j < 15; j++)
            {
                int dirX = rand.Next(-1, 2);
                int dirY = rand.Next(-1, 2);

                if (centerX > 1 && centerX < height - 2 && centerY > 1 && centerY < width - 2)
                {
                    maze[centerX, centerY, 0] = new Square(SquareType.Empty);

                    // Ajustar la dirección según el camino
                    centerX += dirX;
                    centerY += dirY + offsetX;
                }
            }

            // Conectar el final del camino al punto central
            ConnectPoints((centerX, centerY), end);
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
                bool validPosition;
                do
                {
                    x = rand.Next(1, Math.Max(2, height - 1));
                    y = rand.Next(1, Math.Max(2, width - 1));

                    // Verificar que no está en el camino principal
                    validPosition = maze[x, y, 0].Type == SquareType.Empty
                                  && !IsNearStart((x, y), 5)
                                  && !IsMainPath((x, y))
                                  && !IsOnPlayerPath((x, y));
                } while (!validPosition);

                maze[x, y, 0] = new Square(type);
            }
        }

        private bool IsOnPlayerPath((int x, int y) pos)
        {
            // Verificar si está en el camino entre el jugador 1 y el centro
            if (IsBetweenPoints(pos, Player1Start, (height / 2, width / 2)))
                return true;

            // Verificar si está en el camino entre el jugador 2 y el centro
            if (IsBetweenPoints(pos, Player2Start, (height / 2, width / 2)))
                return true;

            return false;
        }

        private bool IsBetweenPoints((int x, int y) pos, (int x, int y) start, (int x, int y) end)
        {
            // Verificar si el punto está en el camino recto entre start y end
            int minX = Math.Min(start.Item1, end.Item1);
            int maxX = Math.Max(start.Item1, end.Item1);
            int minY = Math.Min(start.Item2, end.Item2);
            int maxY = Math.Max(start.Item2, end.Item2);

            if (pos.Item1 >= minX && pos.Item1 <= maxX && pos.Item2 >= minY && pos.Item2 <= maxY)
            {
                // Verificar si está exactamente en la línea
                if (start.Item1 == end.Item1) // camino vertical
                    return pos.Item1 == start.Item1;
                else if (start.Item2 == end.Item2) // camino horizontal
                    return pos.Item2 == start.Item2;
                else // camino diagonal
                    return Math.Abs((pos.Item2 - start.Item2) - (pos.Item1 - start.Item1)) <= 1;
            }
            return false;
        }

        private bool IsBoardConnected()
        {
            bool[,] visited = new bool[height, width];
            Queue<(int x, int y)> queue = new Queue<(int, int)>();

            // Iniciar BFS desde ambas posiciones de jugador
            queue.Enqueue(Player2Start);
            visited[Player2Start.Item1, Player2Start.Item2] = true;

            int[] dirX = { -1, 1, 0, 0 };
            int[] dirY = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    int newX = x + dirX[i];
                    int newY = y + dirY[i];

                    // Verificar límites del tablero
                    if (newX > 0 && newX < height - 1 && newY > 0 && newY < width - 1)
                    {
                        Square cell = maze[newX, newY, 0];

                        // Celdas transitables: vacías o trampas (ignorar obstáculos/muros)
                        if (!visited[newX, newY] &&
                           (cell.Type == SquareType.Empty || cell.Type == SquareType.Trap))
                        {
                            visited[newX, newY] = true;
                            queue.Enqueue((newX, newY));
                        }
                    }
                }
            }

            // Verificar que todas las celdas transitables están conectadas
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    Square cell = maze[i, j, 0];
                    if ((cell.Type == SquareType.Empty || cell.Type == SquareType.Trap) && !visited[i, j])
                    {
                        return false; // Celda inalcanzable
                    }
                }
            }
            return true;
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
            PrintBoard();
        }

        public void MovePlayer(Player player)
        {
            var (dx, dy) = (0, 0);
            int movesLeft = player.MoveSpeed;
            while (movesLeft > 0)
            {
                var key = Console.ReadKey(true);
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
                movesLeft--;
            }

            var newPos = (player.Position.Item1 + dx, player.Position.Item2 + dy);

            if (IsValidMove(newPos, player))
            {
                ClearPlayerPosition(player);
                player.Position = newPos;
                UpdatePlayerLayer(player);
                CheckTrap(player);
                CheckFragment(player);
            }
        }

        private void CheckFragment(Player player)
        {
            var pos = player.Position;
            if (fragmentPositions.Contains(pos))
            {
                player.CollectedFragments++;
                fragmentPositions.Remove(pos);
                AnsiConsole.MarkupLine($"[bold cyan]¡{player.Name} sintió el latir del tiempo![/]");
                AnsiConsole.MarkupLine($"[grey]Fragmentos recolectados: {player.CollectedFragments}/3[/]");
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

                    case "Berserker":
                        player.MoveSpeed += 2; // Aumenta la velocidad de movimiento
                        Console.WriteLine($"¡{player.Name} entra en furia! +2 movimientos por turno.");
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
                string trapSymbol = square.Symbol;
                if (player.ShieldActive)
                {
                    player.ShieldActive = false;
                    Console.WriteLine("¡Escudo bloqueó la trampa!");
                }
                else
                {
                    switch (trapSymbol)
                    {
                        case "D": // Trampa de daño
                            player.Lives--;
                            Console.WriteLine($"¡{player.Name} perdió 1 vida! Vidas restantes: {player.Lives}");
                            break;
                        case "T": // Teletransporte
                            TeleportPlayer(player);
                            Console.WriteLine($"¡{player.Name} fue teletransportado!");
                            break;
                        case "C": // Congelamiento
                            player.ExtraMoves = -2; // Bloquea movimientos por 2 turnos
                            Console.WriteLine($"¡{player.Name} no puede moverse por 2 turnos!");
                            break;
                    }
                }
            }
        }

        // Método para teletransportar a una posición aleatoria
        private void TeleportPlayer(Player player)
        {
            var rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(1, height - 1);
                y = rand.Next(1, width - 1);
            } while (maze[x, y, 0].Type == SquareType.Wall || maze[x, y, 0].Type == SquareType.Obstacle);

            ClearPlayerPosition(player);
            player.Position = (x, y);
            UpdatePlayerLayer(player);
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
            maze[player.Position.Item1, player.Position.Item2, 1] = new Square(SquareType.Empty);
        }

        public void PrintBoard()
        {
            Console.Clear();

            // Primero imprimir capa base (terreno)
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var terrainLayer = maze[i, j, 0];

                    switch (terrainLayer.Symbol)
                    {
                        case "T": // Trampa teletransporte
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case "D": // Trampa daño
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "C": // Trampa congelamiento
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        default: // Vacío
                            Console.ResetColor();
                            break;
                    }

                    // Imprimir símbolo del terreno o muro
                    Console.Write(terrainLayer.Symbol);
                }
                Console.WriteLine();
            }

            // Segundo imprimir capa de jugadores sobre el laberinto
            int consoleY = Console.CursorTop - height;

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, consoleY + i);

                for (int j = 0; j < width; j++)
                {
                    var playerLayer = maze[i, j, 1];

                    if (playerLayer?.Player != null)
                    {
                        Console.SetCursorPosition(j, consoleY + i);
                        Console.Write(playerLayer.Player.Token.Symbol);
                    }
                }
            }

            // Imprimir borde inferior
            Console.SetCursorPosition(0, consoleY + height);
            for (int i = 0; i < width; i++)
            {
                Console.Write("─");
            }
        }
    }
}