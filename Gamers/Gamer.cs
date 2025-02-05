using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Player
    {
        public string Name { get; }
        public Token Token { get; }
        public int Lives { get; set; }
        public (int, int) Position { get; set; }
        public bool ShieldActive { get; set; }
        public int ExtraMoves { get; set; }
        public bool PhaseActive { get; set; }
        public bool HasRevive { get; set; }

        public Player(string name, Token token, int lives)
        {
            Name = name;
            Token = token;
            Lives = lives;
        }
    }
}