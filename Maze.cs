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

        public MazeGame(int n, int countGamers, int countTokens, int width, int height, int totalSquares, int rewardSquaresCount, int emptySquares, (int, int) startPosition, int[,] Mazemaze, int finishLine)
        {
            start = false;
            stop = false;
            currentTurn = 0;
            board = new Board(0, 0, 0, 0);
            board.GenerateMaze();
            players = new List<Player>();
            this.countGamers = countGamers;
            this.countTokens = countTokens;
            maze = Mazemaze;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
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
