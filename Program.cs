using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using game.Tramps;
using game.RewardSquare;
using System.Runtime.CompilerServices;
using System.Data;
using game.Gamers;
using static game.MazeGame;
using game.BoardGame;
using game.Tokens;

namespace game.Program
{
    public class Program
    {
        private static readonly Player players = null!;
        private static readonly int move;
        private static readonly int player;

        public static void Main(string[] args)
        {
            int width = 100;
            int height = 30;
            Board board = new Board(width, height);
            Console.WriteLine("Bienvenido al Laberinto de la muerte!");
            Console.WriteLine("Por favor introduzca su nombre:");
            string name = Console.ReadLine()!;
        }
    }
}