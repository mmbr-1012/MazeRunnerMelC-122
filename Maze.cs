using System.Threading.Tasks;
using game.BoardGame;
using game.Gamers;
using game.MultiplayerGame;

namespace game
{
    public class MazeGame
    {
        bool start;
        bool stop;
        readonly int currentTurn;
        readonly int countGamers;
        readonly int countTokens;
        readonly Board board;
        readonly Token[] gameTokens;
        private List<Player> players;

        public MazeGame(int n, int countGamers, int countTokens, Token[] gameTokens, int width, int height, int totalSquares, int rewardSquaresCount, int emptySquares)
        {
            start  = false;
            stop = false;
            currentTurn = 0;
            board = new Board(0,0);
            board.GenerateMaze();
            this.countGamers = countGamers;
            this.countTokens = countTokens;
            this.gameTokens = gameTokens;
            this.players = new List<Player>();
        }

        public bool Start {  get { return start; } }

        public bool Stop { get { return stop; } }  

        public int CurrentTurn { get { return currentTurn; } }

        public int CountGamers { get { return countGamers; } }

        public int CountTokens { get {  return countTokens; } }

        public List<Player> Players { get {  return players; } }

        public Token[] GameTokens { get { return gameTokens; } }

        public void run ()
        {
            start = true;
        }

        public void pause ()
        {
            stop = true;
        }
        
        public void NextTurn () { }
    }
}



/* MazeGame:
 * -start: se pone en true al comenzar el juego. Solo vuelve a estar en false cuando se crea un juego nuevo
 * -stop: se pone en true cuando se para el juego. Se vuelve a poner en false cuando el usuario quita la pausa del juego
 */