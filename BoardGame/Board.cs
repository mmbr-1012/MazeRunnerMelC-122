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
        private static (int, int) playerPosition;
        private List<(int dx, int dy)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
        private List<List<(int, int)>> paths = new List<List<(int, int)>>();
        static Player player = null!;
        private static string[] tokens = { "Heart", "Clover", "Star", "Diamond", "Moon", "Sun" };
        private static string[] symbols = { "❤️", "☘️", "⭐", "💎", "🌙", "☀️" };
        private static string playerSymbol = "*";
        private static int selectedTokenIndex = 0;
        private string[] obstacleSymbols = { "🌳", "🏔️", "🌲", "🏃", "🏃‍♂️" };
        private HashSet<(int, int)> occupiedPositions = new HashSet<(int, int)>();
        private readonly bool nearEnd;
        private readonly bool nearStart;
        private readonly bool blocksPath;
        private readonly List<Hability> heartHabilities = null!;
        private readonly List<Hability> cloverHabilities = null!;
        private readonly List<Hability> starHabilities = null!;
        private readonly List<Hability> diamondHabilities = null!;
        private readonly List<Hability> moonHabilities = null!;
        private readonly List<Hability> sunHabilities = null!;
        public static Dictionary<string, Hability> Habilities { get; set; } = null!;
        public Dictionary<string, List<Hability>> habilitiesBySymbol { get; private set; } = null!;
        public bool hasShield { get; private set; }
        public bool speedBoost { get; private set; }

        public Board(int boardWidth, int boardHeight, int x, int y)
        {
            width = boardWidth;
            height = boardHeight;
            maze = new int[height, width];
            start = (1, 0);
            end = (28, width - 1);
            subend = (28, 118);
            playerPosition = (1, 0);
            GenerateMaze();
            GeneratePath();
            CreateObstacles();
            CreateTramps();
            ActiveTramp(x, y, player);
            SelectPlayerSymbol();
            InitializeHabilities();
            Console.WriteLine("\nPulsa una tecla para comenzar...");
            Console.ReadKey();
            Player.PlayGame();
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
            (int playerPositionx, int playerPositiony) = playerPosition;
            maze[startX, startY] = 0; // 0 represents a path
            maze[endX, endY] = 0;
            maze[subendX, subendY] = 0;
            maze[playerPositionx, playerPositiony] = 11;
            CarveMaze(startX, startY);
        }

        private void CarveMaze(int x, int y)
        {
            List<(int dx, int dy)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
            Shuffle(directions);

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx * 2, ny = y + dy * 2;
                // Asegurar que no se modifique el borde del laberinto
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

        public static void SelectPlayerSymbol()
        {
            Console.Clear();
            Console.WriteLine("Elige tu símbolo:");

            // Mostrar todos los símbolos disponibles
            for (int i = 0; i < tokens.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {tokens[i]} {symbols[i]}");
            }

            // Obtener la selección del usuario
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int selection) &&
                    selection > 0 && selection <= tokens.Length)
                {
                    selectedTokenIndex = selection - 1;
                    playerSymbol = symbols[selectedTokenIndex];
                    break;
                }
                Console.WriteLine("Selección inválida. Intenta de nuevo:");
            }
        }
        public static void PrintBoard()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Posición actual: X=" + playerPosition.Item1 + ", Y=" + playerPosition.Item2);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(maze[y, x] == 1 ? "█" :
                                 maze[y, x] == 2 ? " " :
                                 maze[y, x] == 3 ? "🌳" :
                                 maze[y, x] == 0 ? " " :
                                 maze[y, x] == 11 ? playerSymbol :
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
            Console.WriteLine("\nUsa las flechas para moverte (↑↓←→)");
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
        public static void MovePlayer(ConsoleKeyInfo key)
        {
            int newX = playerPosition.Item1;
            int newY = playerPosition.Item2;

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    newX -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    newX += 1;
                    break;
                case ConsoleKey.LeftArrow:
                    newY -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    newY += 1;
                    break;
                default:
                    return; // Ignora otras teclas
            }

            // Validar si el movimiento es válido
            if (!IsValidMove(newX, newY))
                return;

            // Actualizar la posición del jugador en el laberinto
            maze[playerPosition.Item1, playerPosition.Item2] = 0; // Limpiar posición anterior
            playerPosition = (newX, newY);
            maze[playerPosition.Item1, playerPosition.Item2] = 11; // Marcar nueva posición

            // Verificar si pisó una trampa
            ActiveTramp(newX, newY, player);
            PrintBoard();
        }

        private static bool IsValidMove(int newX, int newY)
        {
            // Corregir orden: x es fila, y es columna
            if (!IsInBounds(newY, newX)) // newY = columna, newX = fila
                return false;

            if (maze[newX, newY] == 1 || maze[newX, newY] == 3)
                return false;

            return true;
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
            for (int i = 0; i < cantTramps; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(0, height);
                    y = random.Next(0, width);
                }
                while ((x, y) == playerPosition || maze[x, y] != 0); // Evitar posición del jugador
                maze[x, y] = random.Next(4, 12);
            }
        }

        private void CreateObstacles()
        {
            (int startX, int startY) = start;
            (int endX, int endY) = end;

            // Lista de símbolos bonitos para obstáculos
            string[] obstacleSymbols = { "🌳" };

            // Crear solo 5 obstáculos estratégicos
            int cantObstacles = 5;
            HashSet<(int, int)> occupiedPositions = new HashSet<(int, int)>();

            for (int i = 0; i < cantObstacles; i++)
            {
                int x, y;

                // Evitar la primera columna (entrada) y última columna (salida)
                do
                {
                    x = random.Next(1, height - 1); // Evitar primera y última fila
                    y = random.Next(1, width - 2);  // Evitar primera y última columna

                    // Verificar que no esté cerca de la entrada ni la salida
                    bool nearStart = Math.Abs(x - startX) < 3 && Math.Abs(y - startY) < 3;
                    bool nearEnd = Math.Abs(x - endX) < 3 && Math.Abs(y - endY) < 3;

                    // Verificar que no bloquee los caminos encontrados
                    bool blocksPath = false;
                    foreach (var path in paths)
                    {
                        if (path.Contains((x, y)))
                            blocksPath = true;
                    }

                    // Aceptar la posición solo si cumple todas las condiciones
                }
                while ((x == startX && y == startY) ||
                         (x == endX && y == endY) ||
                         maze[x, y] != 0 ||
                         occupiedPositions.Contains((x, y)) ||
                         nearStart ||
                         nearEnd ||
                         blocksPath);

                // Asignar un símbolo aleatorio del array
                maze[x, y] = 3;
                occupiedPositions.Add((x, y));
            }
        }
        private static void ActiveTramp(int x, int y, Player player)
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
                    Console.WriteLine("HHability Blocked!");
                    break;
            }
        }
        private bool IsValidPosition(int x, int y)
        {
            // Posiciones prohibidas: inicio, final, subfinal y jugador
            (int startX, int startY) = start;
            (int endX, int endY) = end;
            (int subendX, int subendY) = subend;
            (int playerX, int playerY) = playerPosition;

            bool isRestricted = (x == startX && y == startY) ||
                                (x == endX && y == endY) ||
                                (x == subendX && y == subendY) ||
                                (x == playerX && y == playerY);

            // Verificar que sea un camino transitable y no esté ocupado
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

            if (habilitiesBySymbol.ContainsKey(playerSymbol))
            {
                Habilities = new Dictionary<string, Hability>();
                foreach (var hability in habilitiesBySymbol[playerSymbol])
                {
                    Habilities.Add(hability.Name, hability);
                }
            }
            else
            {
                throw new KeyNotFoundException($"Símbolo del jugador '{playerSymbol}' no encontrado en habilitiesBySymbol.");
            }
        }
        public void UseHability(string HabilityName)
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

        private void ActivateHability(Hability Hability)
        {
            switch (Hability.Name)
            {
                case "Curación":
                    player.CurrentHealth = Math.Min(player.Health, player.CurrentHealth + 20);
                    break;
                case "Escudo":
                    hasShield = true;
                    break;
                case "Estrella Brillante":
                    RevealArea();
                    break;
                case "Teletransporte":
                    Teleport();
                    break;
                case "Luna Llena":
                    speedBoost = true;
                    break;
                case "Visión Nocturna":
                    RevealTraps();
                    break;
            }
        }

        private void RevealTraps()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Revelar trampas pero mantener obstáculos normales
                    if (maze[y, x] >= 4 && maze[y, x] <= 10)
                    {
                        maze[y, x] = 2; // Marcar como revelado
                    }
                }
            }

            Task.Delay(5000).ContinueWith(t =>
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (maze[y, x] == 2 && (y, x) != playerPosition)
                        {
                            if ((y, x) == start || (y, x) == end || (y, x) == subend)
                                maze[y, x] = 0;
                            else if (maze[y, x] == 2)
                                maze[y, x] = 3; // Volver a obstáculo
                        }
                    }
                }
                PrintBoard();
            });
        }

        private void Teleport()
        {
            int maxDistance = 3;
            int newX = playerPosition.Item1;
            int newY = playerPosition.Item2;

            for (int distance = maxDistance; distance > 0; distance--)
            {

                foreach (var (dx, dy) in directions)
                {
                    newX = playerPosition.Item1 + dx * distance;
                    newY = playerPosition.Item2 + dy * distance;

                    // Verificar si la posición es válida
                    if (newX >= 0 && newX < height && newY >= 0 && newY < width &&
                        maze[newX, newY] == 0)
                    {
                        // Actualizar posición
                        maze[playerPosition.Item1, playerPosition.Item2] = 0;
                        playerPosition = (newX, newY);
                        maze[newX, newY] = 11;

                        // Verificar trampas en la nueva posición
                        ActiveTramp(newX, newY, player);
                        PrintBoard();
                        return;
                    }
                }
            }
        }

        private void RevealArea()
        {
            int centerX = playerPosition.Item1;
            int centerY = playerPosition.Item2;
            int radius = 2; // Radio de visión

            // Crear un área circular alrededor del jugador
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    int newX = centerX + x;
                    int newY = centerY + y;

                    // Verificar que la posición esté dentro del laberinto
                    if (newX >= 0 && newX < height && newY >= 0 && newY < width)
                    {
                        // Revelar trampas y obstáculos
                        if (maze[newX, newY] >= 4 && maze[newX, newY] <= 10)
                        {
                            maze[newX, newY] = 2; // Marcar como revelado
                        }
                    }
                }
            }

            // Volver a ocultar después de un tiempo
            Task.Delay(3000).ContinueWith(t =>
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (maze[y, x] == 2 && (y, x) != playerPosition)
                        {
                            if ((y, x) == start || (y, x) == end || (y, x) == subend)
                                maze[y, x] = 0;
                            else if (maze[y, x] == 2)
                                maze[y, x] = 3; // Volver a obstáculo
                        }
                    }
                }
                PrintBoard();
            });
        }

        // public static void UpdateHabilities()
        // {
        //     foreach (var Hability in Habilities.Values)
        //     {
        //         Hability.Update();
        //     }
        // }
    }
}