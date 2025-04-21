using System;
using System.Collections.Generic;
using System.Threading;
using Spectre.Console;

namespace BoardGame
{
    public class Game
    {
        private Board board = null!;
        private List<Player> players = new List<Player>();
        private const int BoardWidth = 51;
        private const int BoardHeight = 25;
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
        new Token("Guerrero del Tiempo", new Ability("Escudo Temporal", 5, 2)),
        new Token("Mago Cronomántico", new Ability("Retroceso", 6, 1)),
        new Token("Ladrón de Reliquias", new Ability("Velocidad Cuántica", 4, 3)),
        new Token("Arquero Ancestral", new Ability("Visión Futura", 5, 2)),
        new Token("Necrótico Eterno", new Ability("Resurrección", 10, 1)),
        new Token("Berserker Temporal", new Ability("Distorsión", 3, 1))
    };

            for (int i = 1; i <= 2; i++)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                    new Panel("[bold gold1]El Templo del Tiempo Perdido[/]\n\n" +
                        "Hace siglos, un reloj místico controlaba el flujo del tiempo...\n" +
                        "Tras su destrucción, sus [cyan]◍ fragmentos[/] quedaron esparcidos en este laberinto.\n" +
                        "¡Recógelos y llévalos al [yellow]altar central[/] para restaurar el equilibrio temporal!⏳\n")
                        .BorderColor(Color.Yellow)
                    );

                var token = AnsiConsole.Prompt(
                    new SelectionPrompt<Token>()
                        .Title($"Jugador {i}, elige tu avatar:")
                        .PageSize(6)
                        .UseConverter(t => $"{t.Symbol} {t.Name} - [grey]{t.Ability.Description}[/]")
                        .AddChoices(tokens)
                );

                tokens.Remove(token);
                string name = AnsiConsole.Ask<string>($"[bold]Nombre del Jugador {i}:[/]");
                players.Add(new Player(name, token, Lives));
            }
        }

        private void RunGameLoop()
        {
            while (true)
            {
                foreach (var player in players)
                {
                    board.PrintBoard();
                    Console.WriteLine("El primer jugador se mostrara en la parte superior izquiera y el segundo jugador en la esquina inferior derecha");
                    Console.WriteLine("Les deseo suerte y que logren salir con vida de este laberinto ♫");
                    Console.WriteLine($"{player.Name} es tu turno | Vidas: {player.Lives}");
                    Console.WriteLine("Puedes moverte con las flechas/WASD. Usa la habilidad con la barra espaciadora");

                    var key = Console.ReadKey(true);
                    board.MovePlayer(player);

                    if (CheckVictory(player))
                    {
                        Console.WriteLine($"¡{player.Name} ha ganado!");
                        return;
                    }

                    if (player.Lives <= 0)
                        return;

                    player.Token.Ability.UpdateCooldown();
                    player.PhaseActive = false;

                    if (player.MovesBlocked > 0)
                    {
                        player.MovesBlocked--;
                        Console.WriteLine($"{player.Name} está inmovilizado. Turnos restantes: {player.MovesBlocked}");
                        continue; // Saltar turno
                    }

                    if (player.ExtraMoves > 0)
                    {
                        player.ExtraMoves--;
                        board.PrintBoard();
                        Console.WriteLine("¡Movimiento extra disponible!");
                        Console.ReadKey(true);
                        board.MovePlayer(player);
                    }

                    if (player.Lives <= 0)
                    {
                        Console.WriteLine($"¡{player.Name} ha sido eliminado!");
                        return; // Terminar el juego al instante.
                    }
                }
            }
        }
        private bool CheckVictory(Player player)
        {
            bool inAltar = player.Position == board.AltarCentral;
            bool hasAllFragments = player.CollectedFragments >= 3;

            if (inAltar && hasAllFragments)
            {
                AnsiConsole.MarkupLine($"[bold gold1]¡{player.Name} colocó los fragmentos en el altar![/]");
                AnsiConsole.MarkupLine("[yellow]El reloj ancestral cobra vida... ¡EL TIEMPO HA SIDO RESTAURADO![/]");
                return true;
            }

            return inAltar && hasAllFragments;
        }
    }
}