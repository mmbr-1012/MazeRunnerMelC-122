using System.Threading.Tasks;
using game.BoardGame;
using game.Gamers;
using game.Tokens;

namespace game
{
    public class MazeGame
    {
        bool start;
        bool stop;
        private int[,] maze;
        readonly int currentTurn;
        readonly int countGamers;
        readonly int countTokens;
        readonly Board board;
        private readonly Token[] gameTokens;
        private List<Player> players;
        private (int, int) playerPosition;
        int steps;
        private int player;
        public MazeGame(int n, int countGamers, int countTokens, int width, int height, int totalSquares, int rewardSquaresCount, int emptySquares, (int, int) startPosition, int[,] maze)
        {
            start = false;
            stop = false;
            currentTurn = 0;
            board = new Board(0, 0);
            board.GenerateMaze();
            var mover = new Gamer;
            mover.MovePlayers(move, steps, player);
            this.countGamers = countGamers;
            this.countTokens = countTokens;
            this.players = new List<Player>();
            this.playerPosition = startPosition;
            this.maze = maze;
        }

        public class Player(int name, int newX, int newY)
        {
            int name = name;
            private int newX = newX;
            private int newY = newY;

            public int Name { get { return name; } }
            public int XPosition { get { return XPosition; } }
            public int YPosition { get { return YPosition; } }
        }
        public bool Start { get { return start; } }

        public bool Stop { get { return stop; } }

        public int CurrentTurn { get { return currentTurn; } }

        public int CountGamers { get { return countGamers; } }

        public int CountTokens { get { return countTokens; } }

        public int move { get { return move; } }

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



/* MazeGame:
 * -start: se pone en true al comenzar el juego. Solo vuelve a estar en false cuando se crea un juego nuevo
 * -stop: se pone en true cuando se para el juego. Se vuelve a poner en false cuando el usuario quita la pausa del juego
 */