using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Token
    {
        public string Name { get; }
        public Ability Ability { get; }
        public string Symbol { get; }

        public Token(string name, Ability ability)
        {
            Name = name;
            Ability = ability;
            Symbol = name[..2].ToUpper();
        }
    }
}