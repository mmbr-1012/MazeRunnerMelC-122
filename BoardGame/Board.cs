using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using System.Runtime.CompilerServices;
using System.Data;
using static game.MazeGame;
using game.Program;
using game.Tokens;
using game.Gamers;
using game.Tokens.Habilities;
using Spectre.Console;

namespace game.BoardGame
{
    public class Board
    {
        private static int[,] maze = null!;
        private static int width, height;
        private static Random random = new Random();
        private static (int, int) start;
        private static (int, int) end;
        private static (int, int) subend;
        private static (int, int) playerPosition1;
        private static (int, int) playerPosition2;
        private static List<(int dx, int dy)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
        private List<List<(int, int)>> paths = new List<List<(int, int)>>();
        static Player[] players = new Player[2];
        static Player player = null!;
        private static string[] tokens = { "Heart", "Clover", "Star", "Diamond", "Moon", "Sun" };
        private static string[] symbols = { "❤️", "☘️", "⭐", "💎", "🌙", "☀️" };
        private static string playerSymbol = "*";
        private static int selectedTokenIndex = 0;
        private string[] obstacleSymbols = { "🌳", "🏔️", "🌲", "🏃", "🏃‍♂️" };
        private HashSet<(int, int)> occupiedPositions = new HashSet<(int, int)>();
        string HabilityName = null!;
        private static readonly bool nearEnd;
        private static readonly bool nearStart;
        private static readonly bool blocksPath;
        private static readonly List<Hability> heartHabilities = null!;
        private static readonly List<Hability> cloverHabilities = null!;
        private static readonly List<Hability> starHabilities = null!;
        private static readonly List<Hability> diamondHabilities = null!;
        private static readonly List<Hability> moonHabilities = null!;
        private static readonly List<Hability> sunHabilities = null!;
        private static readonly Hability hability = null!;
        private static Dictionary<string, Hability> Habilities = new Dictionary<string, Hability>();
        public Dictionary<string, List<Hability>> habilitiesBySymbol { get; private set; } = null!;
        public static bool hasShield { get; private set; }
        public static int shieldStrength { get; private set; }
        public static bool speedBoost { get; private set; }
        private Player _currentPlayer1;
        private Player _currentPlayer2;
        private static Player _player1 = null!;
        private static Player _player2 = null!;
        Player player1 = null!;
        Player player2 = null!;

        public Board(int boardWidth, int boardHeight, int x, int y, Player[] players)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            _player1 = players.Length > 0 ? players[0] : new Player("Jugador 1", "playerSymbol");
            _player2 = players.Length > 1 ? players[1] : new Player("Jugador 2", "playerSymbol");
            _currentPlayer1 = player1;
            _currentPlayer2 = player2;
            playerSymbol = _player1.Symbol;
            playerSymbol = _player2.Symbol;
            width = boardWidth;
            height = boardHeight;
            maze = new int[height, width];
            start = (1, 0);
            end = (28, width - 1);
            subend = (28, 118);
            playerPosition1 = (1, 0);
            playerPosition2 = (28, width - 1);
            GenerateMaze();
            GeneratePath();
            CreateObstacles();
            CreateTramps();
            ActiveTramp(x, y, player);
            InitializeHabilities();
            Console.WriteLine("\nPulsa una tecla para comenzar...");
            Console.ReadKey();
            Console.WriteLine("Vera este laberinto raro pero el problema es que esta embrujado a me dida que avance podra ver mejor los caminos");
            Player.PlayGame();
        }

        public void GenerateMaze()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[y, x] = 1;
                }
            }

            (int startX, int startY) = start;
            (int endX, int endY) = end;
            (int subendX, int subendY) = subend;
            (int playerPosition1x, int playerPosition1y) = playerPosition1;
            (int playerPosition2x, int playerPosition2y) = playerPosition2;
            maze[startX, startY] = 0;
            maze[endX, endY] = 0;
            maze[subendX, subendY] = 0;
            maze[playerPosition1x, playerPosition1y] = 11;
            maze[playerPosition2x, playerPosition2y] = 12;
            CarveMaze(startX, startY);
        }

        private void CarveMaze(int x, int y)
        {
            List<(int dx, int dy)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
            Shuffle(directions);

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx * 2, ny = y + dy * 2;

                if (IsInBounds(nx, ny) && maze[ny, nx] == 1 && nx < width - 1 && ny < height - 1)
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

        private static bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
        public static void PrintBoard()
        {
            Console.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(maze[y, x] == 1 ? "█" :
                                maze[y, x] == 2 ? " " :
                                maze[y, x] == 3 ? "🌳" :
                                maze[y, x] == 0 ? " " :
                                maze[y, x] == 11 ? playerSymbol :
                                maze[y, x] == 12 ? playerSymbol :
                                maze[y, x] == 4 ? "🕳 " :
                                maze[y, x] == 5 ? "🔺 " :
                                maze[y, x] == 6 ? "🔥" :
                                maze[y, x] == 7 ? "☠ " :
                                maze[y, x] == 8 ? "🌀" :
                                maze[y, x] == 9 ? "💀 " :
                                maze[y, x] == 10 ? "⛔" : " ");
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(playerPosition1.Item2, playerPosition1.Item1);
            Console.Write(_player1.Symbol);
            Console.SetCursorPosition(playerPosition2.Item2, playerPosition2.Item1);
            Console.Write(_player2.Symbol);

            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine($"Vidas: {_player1.Name} ({_player1.Lives}) | {_player2.Name} ({_player2.Lives})");
            Console.WriteLine("Usa las flechas (↑↓←→) o WASD para moverte");
        }

        public void GeneratePath()
        {
            maze[28, width - 1] = 2;
            maze[28, 117] = 2;
            for (int i = 1; i <= 4; i++) maze[i, 1] = 2;
            for (int i = 1; i <= 6; i++) maze[4, i] = 2;
            for (int i = 4; i <= 7; i++) maze[i, 6] = 2;
            for (int i = 7; i <= 12; i++) maze[7, i] = 2;
            for (int i = 7; i >= 1; i--) maze[i, 12] = 2;
            for (int i = 12; i <= 20; i++) maze[1, i] = 2;
            for (int i = 1; i <= 13; i++) maze[i, 20] = 2;
            for (int i = 20; i >= 4; i--) maze[13, i] = 2;
            for (int i = 13; i <= 18; i++) maze[i, 4] = 2;
            for (int i = 4; i <= 28; i++) maze[18, i] = 2;
            for (int i = 18; i >= 14; i--) maze[i, 28] = 2;
            for (int i = 28; i <= 33; i++) maze[14, i] = 2;
            for (int i = 14; i <= height - 3; i++) maze[i, 33] = 2;
            for (int i = 33; i <= 36; i++) maze[height - 3, i] = 2;
            for (int i = height - 3; i >= height - 6; i--) maze[i, 36] = 2;
            for (int i = 36; i <= 42; i++) maze[height - 6, i] = 2;
            for (int i = height - 6; i <= height - 3; i++) maze[i, 42] = 2;
            for (int i = 42; i <= 46; i++) maze[height - 3, i] = 2;
            for (int i = height - 3; i >= 15; i--) maze[i, 46] = 2;
            for (int i = 46; i <= 50; i++) maze[15, i] = 2;
            for (int i = 15; i <= 20; i++) maze[i, 50] = 2;
            for (int i = 50; i <= 65; i++) maze[20, i] = 2;
            for (int i = 20; i >= 11; i--) maze[i, 65] = 2;
            for (int i = 65; i <= 75; i++) maze[11, i] = 2;
            for (int i = 11; i <= 16; i++) maze[i, 75] = 2;
            for (int i = 75; i <= 90; i++) maze[16, i] = 2;
            for (int i = 16; i <= 25; i++) maze[i, 90] = 2;
            for (int i = 90; i <= 97; i++) maze[25, i] = 2;
            for (int i = 25; i >= 20; i--) maze[i, 97] = 2;
            for (int i = 97; i <= 110; i++) maze[20, i] = 2;
            for (int i = 20; i <= height - 8; i++) maze[i, 110] = 2;
            for (int i = 110; i <= 113; i++) maze[height - 8, i] = 2;
            for (int i = height - 8; i <= height - 3; i++) maze[i, 113] = 2;
            for (int i = 113; i <= 117; i++) maze[height - 3, i] = 2;

            for (int i = 0; i <= width - 1; i++)
            {
                if (maze[0, i] != 1) maze[0, i] = 1;
            }

        }
        public static void MovePlayer(ConsoleKeyInfo key, Player player)
        {
            int deltaX = 0, deltaY = 0;
            if (player == _player1 || player == _player2)
            {
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W: deltaX = -1; break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S: deltaX = 1; break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A: deltaY = -1; break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D: deltaY = 1; break;
                    default: return;
                }
            }

            int newX = (player == _player1) ? playerPosition1.Item1 + deltaX : playerPosition2.Item1 + deltaX;
            int newY = (player == _player1) ? playerPosition1.Item2 + deltaY : playerPosition2.Item2 + deltaY;

            if (!IsValidMove(newX, newY)) return;

            if (player == _player1)
            {
                maze[playerPosition1.Item1, playerPosition1.Item2] = 0;
                playerPosition1 = (newX, newY);
                maze[newX, newY] = 11;
            }
            else
            {
                maze[playerPosition2.Item1, playerPosition2.Item2] = 0;
                playerPosition2 = (newX, newY);
                maze[newX, newY] = 12;
            }
            // CheckTrap(player);
            ActiveTramp(newX, newY, player);
            PrintBoard();
        }
        private static void CheckTrap(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player), "Player object cannot be null");
            }
            int cellValue = maze[player.Position.X, player.Position.Y];
            if (cellValue >= 4 && cellValue <= 10)
            {
                int damage = hasShield ? 1 : 2;

                player.LoseLife();
                ActiveTramp(player.Position.X, player.Position.Y, player);

                if (hasShield)
                {
                    hasShield = false;
                    shieldStrength = 0;
                }
            }
        }
        private static bool IsValidMove(int newX, int newY)
        {
            if (!IsInBounds(newY, newX))
                return false;

            if (maze[newX, newY] == 1 || maze[newX, newY] == 3)
                return false;

            return true;
        }
        private static void CreateTramps()
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
            for (int i = 0; i < cantTramps; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(0, height);
                    y = random.Next(0, width);
                }
                while ((x, y) == playerPosition1 || maze[x, y] != 0);
                maze[x, y] = random.Next(4, 12);
            }
        }

        private void CreateObstacles()
        {
            int cantObstacles = 5;
            HashSet<(int, int)> occupiedPositions = new HashSet<(int, int)>();

            for (int i = 0; i < cantObstacles; i++)
            {
                int x, y;
                bool isValidPosition;

                do
                {
                    x = random.Next(1, height - 1);
                    y = random.Next(1, width - 2);

                    // Verificar que no esté cerca de inicio o fin
                    bool nearStart = Math.Abs(x - start.Item1) < 3 && Math.Abs(y - start.Item2) < 3;
                    bool nearEnd = Math.Abs(x - end.Item1) < 3 && Math.Abs(y - end.Item2) < 3;

                    isValidPosition = !nearStart && !nearEnd &&
                                     maze[x, y] == 0 &&
                                     !occupiedPositions.Contains((x, y));
                } while (!isValidPosition);

                maze[x, y] = 3;  // '@'
                occupiedPositions.Add((x, y));
            }
        }
        private static void ActiveTramp(int x, int y, Player player)
        {
            int squareType = maze[x, y];
            if (squareType is < 4 or > 10) return;

            player.LoseLife();
            Console.WriteLine($"¡{player.Name} activó una trampa!");

            string[] trapMessages = new[]
            {
                "¡Has caído en un pozo!",
                "¡Te has pinchado con una espina!",
                "¡Has sido quemado por fuego!",
                "¡Has bebido veneno!",
                "¡Te has pinchado con espinos!",
                "¡Has caído en un agujero!",
                "¡Tu habilidad ha sido bloqueada!"
            };
            if (squareType - 4 < trapMessages.Length)
            {
                Console.WriteLine(trapMessages[squareType - 4]);
            }
        }
        private bool IsValidPosition(int x, int y)
        {
            (int startX, int startY) = start;
            (int endX, int endY) = end;
            (int subendX, int subendY) = subend;
            (int playerX, int playerY) = playerPosition1;

            bool isRestricted = (x == startX && y == startY) ||
                                (x == endX && y == endY) ||
                                (x == subendX && y == subendY) ||
                                (x == playerX && y == playerY);

            return maze[x, y] == 0 && !isRestricted && !occupiedPositions.Contains((x, y));
        }
        private void InitializeHabilities()
        {
            var heartHabilities = new List<Hability>
            {
                new Hability("Curación", "Restaura 20 puntos de salud", 3, 10),
                new Hability("Escudo", "Bloquea el próximo daño de trampa", 5, 15)
            };

            var cloverHabilities = new List<Hability>
            {
                new Hability("Suerte", "Evita el próximo daño de trampa", 4, 12),
                new Hability("Regeneración", "Recupera 15 puntos de salud cada turno", 2, 8)
            };

            var starHabilities = new List<Hability>
            {
                new Hability("Estrella Brillante", "Ilumina el área alrededor", 4, 12),
                new Hability("Teletransporte", "Mueve al jugador 3 casillas", 6, 25)
            };

            var diamondHabilities = new List<Hability>
            {
                new Hability("Brillo", "Revela trampas en un área grande", 5, 20),
                new Hability("Protección", "Reduce el daño de trampas a la mitad", 3, 15)
            };

            var moonHabilities = new List<Hability>
            {
                new Hability("Luna Llena", "Aumenta velocidad temporalmente", 5, 15),
                new Hability("Visión Nocturna", "Revela trampas cercanas", 4, 10)
            };

            var sunHabilities = new List<Hability>
            {
                new Hability("Luz Solar", "Destruye obstáculos adyacentes", 6, 25),
                new Hability("Curación Solar", "Restaura salud a todos los jugadores", 8, 30)
            };

            habilitiesBySymbol = new Dictionary<string, List<Hability>>
            {
            { "❤️", heartHabilities },
            { "☘️", cloverHabilities },
            { "⭐", starHabilities },
            { "💎", diamondHabilities },
            { "🌙", moonHabilities },
            { "☀️", sunHabilities }
        };
            string currentPlayerSymbol = _player1.Symbol;
            if (habilitiesBySymbol.ContainsKey(playerSymbol))
            {
                Habilities = new Dictionary<string, Hability>();
                foreach (var hability in habilitiesBySymbol[currentPlayerSymbol])
                {
                    Habilities.Add(hability.Name, hability);
                }
            }
            else
            {
                throw new KeyNotFoundException($"Símbolo del jugador '{playerSymbol}' no encontrado en habilitiesBySymbol.");
            }
        }
        public static void UseHability(string HabilityName)
        {
            if (Habilities.TryGetValue(HabilityName, out var Hability) && Hability.CanUse())
            {
                if (player.CurrentHealth >= Hability.Cost)
                {
                    player.CurrentHealth -= Hability.Cost;
                    Hability.Use();
                    ActivateHability(Hability);
                }
                else
                {
                    Console.WriteLine("No tienes suficiente salud para usar esta habilidad");
                }
            }
            else
            {
                Console.WriteLine("No puedes usar esta habilidad ahora");
            }
        }

        private static void ActivateHability(Hability hability)
        {
            if (hability == null) return;

            if (player.CurrentHealth >= hability.Cost)
            {
                switch (hability.Name)
                {
                    case "Curación":
                        player.CurrentHealth = Math.Min(player.Health, player.CurrentHealth + 20);
                        break;
                    case "Escudo":
                        hasShield = true;
                        shieldStrength = 10;
                        break;
                    case "Estrella Brillante":
                        RevealArea();
                        break;
                    case "Teletransporte":
                        Teleport();
                        break;
                    case "Luna Llena":
                        speedBoost = true;
                        Task.Delay(5000).ContinueWith(t =>
                        {
                            speedBoost = false;
                        });
                        break;
                    case "Visión Nocturna":
                        RevealTraps();
                        break;
                }

            }
            else
            {
                Console.WriteLine("La habilidad no existe");
            }

        }

        private static void RevealTraps()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (maze[y, x] >= 4 && maze[y, x] <= 10)
                    {
                        maze[y, x] = 2;
                    }
                }
            }

            Task.Delay(5000).ContinueWith(t =>
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (maze[y, x] == 2 && (y, x) != playerPosition1)
                        {
                            if ((y, x) == start || (y, x) == end || (y, x) == subend)
                                maze[y, x] = 0;
                            else if (maze[y, x] == 2)
                                maze[y, x] = 3;
                        }
                    }
                }
                PrintBoard();
            });
        }

        private static void Teleport()
        {
            int maxDistance = 3;
            int newX = playerPosition1.Item1 + 3;
            int newY = playerPosition1.Item2;

            for (int distance = maxDistance; distance > 0; distance--)
            {

                foreach (var (dx, dy) in directions)
                {
                    newX = playerPosition1.Item1 + dx * distance;
                    newY = playerPosition1.Item2 + dy * distance;

                    if (newX >= 0 && newX < height && newY >= 0 && newY < width &&
                        maze[newX, newY] == 0)
                    {
                        maze[playerPosition1.Item1, playerPosition1.Item2] = 0;
                        playerPosition1 = (newX, newY);
                        maze[newX, newY] = 11;
                        ActiveTramp(newX, newY, player);
                        PrintBoard();
                        return;
                    }
                }
            }
        }

        private static void RevealArea()
        {
            int centerX = playerPosition1.Item1;
            int centerY = playerPosition1.Item2;
            int radius = 2;

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    int newX = centerX + x;
                    int newY = centerY + y;

                    if (newX >= 0 && newX < height && newY >= 0 && newY < width)
                    {
                        if (maze[newX, newY] >= 4 && maze[newX, newY] <= 10)
                        {
                            maze[newX, newY] = 2;
                        }
                    }
                }
            }

            Task.Delay(3000).ContinueWith(t =>
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (maze[y, x] == 2 && (y, x) != playerPosition1)
                        {
                            if ((y, x) == start || (y, x) == end || (y, x) == subend)
                                maze[y, x] = 0;
                            else if (maze[y, x] == 2)
                                maze[y, x] = 3;
                        }
                    }
                }
                PrintBoard();
            });
        }

        public static void UpdateHabilities()
        {
            foreach (var Hability in Habilities.Values)
            {
                Hability.Update();
            }
        }
    }
}