using System.Collections.Generic;
using System.Text;

namespace game.Tokens
{
    public class Token
    {
        public string Name { get; private set; }
        public string Hability { get; private set; }

        public Token(string name, string hability)
        {
            Name = name;
            Hability = hability;
        }
    }

    public static class TokenManager
    {
        private static List<Token> tokens = new List<Token>();

        public static void InitializeTokens()
        {
            tokens.Add(new Token("Warrior", "Strength"));
            tokens.Add(new Token("Mage", "Magic"));
            tokens.Add(new Token("Rogue", "Stealth"));
            tokens.Add(new Token("Healer", "Healing"));
            tokens.Add(new Token("Berserker", "Fury"));
            tokens.Add(new Token("Guardian", "Defense"));
        }

        public static List<Token> GetTokens()
        {
            return tokens;
        }
    }
}