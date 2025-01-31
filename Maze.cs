﻿using System.Threading.Tasks;
using game.BoardGame;
using game.Gamers;
using game.Tokens;

namespace game
{
    public class MazeGame
    {
        bool start;
        bool stop;
        private static int[,] maze = null!;
        readonly int currentTurn;
        readonly int countGamers;
        readonly int countTokens;
        readonly Board board;
        private const int MaxPlayers = 6;
        private static List<Player> players = new List<Player>();
        private List<Token> tokens = new List<Token>();
        private static Random random = new Random();
        static string name = null!; 
        static string symbol = null!;

        public MazeGame(int n, int countGamers, int countTokens, int width, int height, int totalSquares, int rewardSquaresCount, int emptySquares, (int, int) startPosition, int[,] Mazemaze)
        {
            start = false;
            stop = false;
            currentTurn = 0;
            board = new Board(0, 0, 0 ,0);
            board.GenerateMaze();
            this.countGamers = countGamers;
            this.countTokens = countTokens;
            maze = Mazemaze;
            TokenManager.InitializeTokens();
        }

        public static void AddPlayer(string playerName, string playerSymbol)
        {
            if (players.Count < MaxPlayers)
            {
                players.Add(new Player(playerName, playerSymbol)); // Pass player name and symbol
            }
            else
            {
                Console.WriteLine("Maximum number of players reached.");
            }
        }

        public static void PlayerTurn(Player player)
        {
            // Simulate player movement and trap/reward interaction
            int outcome = random.Next(1, 11); // Random number between 1 and 10

            if (outcome <= 3) // 30% chance of hitting a trap
            {
                player.Lives--;
                Console.WriteLine($"{player.Name} hit a trap! Lives left: {player.Lives}");
            }
            else if (outcome <= 6) // 30% chance of hitting a reward
            {
                player.Lives++;
                Console.WriteLine($"{player.Name} found a reward! Lives now: {player.Lives}");
            }
            else
            {
                Console.WriteLine($"{player.Name} moved safely.");
            }
        }

        public static void StartGame()
        {
            foreach (var player in players)
            {
                PlayerTurn(player);
            }
        }

        public bool Start { get { return start; } }
        public bool Stop { get { return stop; } }
        public int CurrentTurn { get { return currentTurn; } }
        public int CountGamers { get { return countGamers; } }
        public int CountTokens { get { return countTokens; } }

        public void run()
        {
            start = true;
        }

        public void pause()
        {
            stop = true;
        }

        public void NextTurn() { }
    }
}
