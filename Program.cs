using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using game.Gamers;
using static game.MazeGame;
using game.BoardGame;
using game.Tokens;
using Spectre.Console;

namespace game.Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int width = 120;
            int height = 30;
            Board board = new Board(width, height,0,0);
            Player.PlayGame();
        }
    }
}