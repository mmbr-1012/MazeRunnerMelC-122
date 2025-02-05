using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Game
    {
        private Board board = null!;
        private List<Player> players = new List<Player>();
        private const int BoardWidth = 121;
        private const int BoardHeight = 31;
        private const int Lives = 3;

        public void Start()
        {
            InitializePlayers();
            board = new Board(BoardHeight, BoardWidth, players);
            RunGameLoop();
        }

        private void InitializePlayers()
        {
            var tokens = new List<Token>
            {
                new Token("Warrior", new Ability("Shield", 5, 2)),
                new Token("Mage", new Ability("Heal", 6, 1)),
                new Token("Rogue", new Ability("Speed", 4, 3)),
                new Token("Archer", new Ability("Range", 5, 2)),
                new Token("Necro", new Ability("Revive", 10, 1))
            };

            for (int i = 1; i <= 2; i++)
            {
                Console.WriteLine($"Player {i}, choose your token:");
                for (int j = 0; j < tokens.Count; j++)
                {
                    Console.WriteLine($"{j + 1}. {tokens[j].Name} - {tokens[j].Ability.Description}");
                }

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > tokens.Count)
                {
                    Console.WriteLine("Invalid choice!");
                }

                Console.WriteLine($"Player {i} name:");
                string name = Console.ReadLine()!;

                players.Add(new Player(name, tokens[choice - 1], Lives));
            }
        }

        private void RunGameLoop()
        {
            while (true)
            {
                foreach (var player in players)
                {
                    Console.WriteLine("Como puede apreciar este laberinto se ve raro, pero no se preocupe es solo que esta embrujado por un antiguo Hechicero malvado");
                    Console.WriteLine("El primer jugador se mostrara en la parte superior izquiera y el segundo jugador en la esquina inferior derecha");
                    Console.WriteLine("Les deseo suerte y que logren salir con vida de este laberinto ♫");
                    Console.WriteLine($"{player.Name}'s turn | Lives: {player.Lives}");
                    Console.WriteLine("Move with arrows/WASD, Use ability with Space");
                    board.PrintBoard();

                    var key = Console.ReadKey(true);
                    board.MovePlayer(key, player);

                    if (CheckVictory(player))
                    {
                        Console.WriteLine($"¡{player.Name} ha ganado!");
                        return;
                    }

                    if (player.Lives <= 0)
                        return;

                    player.Token.Ability.UpdateCooldown();
                    player.PhaseActive = false;

                    if (player.ExtraMoves > 0)
                    {
                        player.ExtraMoves--;
                        board.PrintBoard();
                        Console.WriteLine("¡Movimiento extra disponible!");
                        Console.ReadKey(true);
                        board.MovePlayer(key, player);
                    }
                }
            }
        }
        private bool CheckVictory(Player player)
        {
            var target = players[0] == player ? board.Player2Start : board.Player1Start;

            return Math.Abs(player.Position.Item1 - target.Item1) <= 1 &&
                   Math.Abs(player.Position.Item2 - target.Item2) <= 1;
        }
    }
}