using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Game
    {
        private Board board = null!;
        private List<Player> players = new List<Player>();
        private const int BoardSize = 30;
        private const int Lives = 3;

        public void Start()
        {
            InitializePlayers();
            board = new Board(BoardSize, BoardSize, players);
            RunGameLoop();
        }

        private void InitializePlayers()
        {
            var tokens = new List<Token> {
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
                    board.PrintBoard();
                    Console.WriteLine($"{player.Name}'s turn | Lives: {player.Lives}");
                    Console.WriteLine("Move with arrows/WASD, Use ability with Space");
                    
                    var key = Console.ReadKey(true);
                    board.MovePlayer(key, player);
                    
                    if (CheckVictory(player))
                    {
                        Console.WriteLine($"¡{player.Name} ha ganado!");
                        return;
                    }
                    
                    if (player.Lives <= 0)
                    {
                        Console.WriteLine($"¡{player.Name} ha sido eliminado!");
                        return;
                    }
                }
            }
        }

        private bool CheckVictory(Player player)
        {
            var target = players[0] == player ? board.Player2Start : board.Player1Start;
            return player.Position == target;
        }
    }
}