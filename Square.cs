using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Square
    {
        public SquareType Type { get; }
        public string Symbol { get; }
        public Player Player { get; set; }

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

        private string GetSymbol(SquareType type) => type switch
        {
            SquareType.Wall => "██",
            SquareType.Obstacle => "▓▓",
            SquareType.Trap => GetRandomTrapSymbol(),
            _ => "  "
        };

        private string GetRandomTrapSymbol() => new[] { "💀", "🔥", "⚠️" }[new Random().Next(3)];
    }
    public enum SquareType { Empty, Wall, Obstacle, Trap }
}