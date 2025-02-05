using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Square
    {
        public SquareType Type { get; }
        public string Symbol { get; set;}
        public Player Player { get; set; } = null!;

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
            SquareType.Obstacle => "🌳",
            SquareType.Trap => new Random().Next(3) switch { 0 => "🍄", 1 => "🌿", _ => " " },
            _ => " "
        };
    }

    public enum SquareType { Empty, Wall, Obstacle, Trap }


}