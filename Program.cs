using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game game = new Game();
            game.Start();
        }
    }
}