using game.BoardGame;
using game.Tokens.Habilities;


namespace game.Tokens
{
    public class Token
    {
        public Token[] Tokens { get { return tokens; } }
        private readonly Token[] gameTokens = null!;
        private readonly Token[] tokens = null!;
        public Token[] GameTokens { get { return gameTokens; } }
        public string Name { get; set; }
        public string Hability { get; set; }
        public bool Selected { get; private set; }
        public Token(string hability, string name, string gameToken, Token[] tokens)
        {
            Hability = hability;
            Name = name;
            Selected = false;
        }

        public void Select()
        {
            Selected = true;
        }
    }
}