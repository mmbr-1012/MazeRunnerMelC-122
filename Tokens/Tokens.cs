using game.BoardGame;
using game.Tokens.Habilities;

namespace game
{
    public class Token: Square
    {
        readonly Hability hability;
        readonly int frozenTime;
        readonly int speed;
        bool isFrozen;
        readonly string image;

        public Token(Hability hability, int frozenTime, int speed, string image)
        {
            isFrozen = false;
            this.hability = hability;
            this.frozenTime = frozenTime;
            this.speed = speed;
            this.image = image;
        }

        public Hability Hability { get { return hability; } }

        public int FrozenTime { get { return frozenTime; } }

        public int Speed { get { return speed; } }
 
        public bool IsFrozen { get { return isFrozen; } set { isFrozen = value; } }

        public string Image { get { return image; } }
    }
}