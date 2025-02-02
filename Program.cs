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
        static string[] symbols = { "❤️", "☘️", "⭐", "💎", "🌙", "☀️" };
        private static Player player1;
        private static Player player2;
        public static void Main(string[] args)
        {
            int width = 120;
            int height = 30;
            Console.WriteLine("¿Cuántos jugadores van a jugar? (1/2)");
            int numberOfPlayers;
            while (!int.TryParse(Console.ReadLine(), out numberOfPlayers) || (numberOfPlayers < 1 || numberOfPlayers > 2))
            {
                Console.WriteLine("¡Opción inválida! Ingresa 1 o 2:");
            }

            string[] symbols = { "❤️", "☘️", "⭐", "💎", "🌙", "☀️" };
            player1 = new Player("Jugador 1", symbols[0]);
            player2 = new Player("Jugador 2", symbols[1]);
            List<Player> players = new List<Player>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                Console.WriteLine($"Jugador {i + 1}, elige tu símbolo (1-6):");
                for (int j = 0; j < symbols.Length; j++)
                {
                    Console.WriteLine($"{j + 1}. {symbols[j]}");
                }

                int symbolChoice;
                while (!int.TryParse(Console.ReadLine(), out symbolChoice) || symbolChoice < 1 || symbolChoice > 7)
                {
                    Console.WriteLine("¡Opción inválida! Elige un número del 1 al 7:");
                }

                players.Add(new Player($"Jugador {i + 1}", symbols[symbolChoice - 1]));
            }
            Board board = new Board(width, height, 0, 0, players.ToArray());
            Player player = new Player("Jugador", symbols[0]);
            GameManager.StartGame(board, player1, player2);
        }
    }
}