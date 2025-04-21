using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Square
    {
        public SquareType Type { get; }
        public string Symbol { get; set; }
        public Player Player { get; set; } = null!;
        private static Random _rand = new Random();

        public Square(SquareType type)
        {
            Type = type;
            Symbol = GetSymbol(type);
        }

        public Square(Player player)
        {
            Player = player;
            Symbol = player.Token.Symbol;
        }

        public string GetSymbol(SquareType type) => type switch
        {
            SquareType.Wall => "█",
            SquareType.Obstacle => "◙",  // Obstáculos como engranajes
            SquareType.Trap => _rand.Next(3) switch
            {
                0 => "T",  // Vortex temporal
                1 => "D",  // Envejecimiento
                _ => "C"    // Congelamiento
            },
            SquareType.Relic => "◍",    
            _ => " "
        };
    }

    public enum SquareType { Empty, Wall, Obstacle, Trap, Relic }


}