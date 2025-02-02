using game.BoardGame;
using game.Gamers;

namespace game.Tramps
{
    public class Trap
    {
        public int Type { get; }
        public string Name { get; }
        public int Damage { get; }
        public string Message { get; }

        public Trap(int type, string name, int damage, string message)
        {
            Type = type;
            Name = name;
            Damage = damage;
            Message = message;
        }

        public void Activate(Player player)
        {
            player.TakeDamage(Damage);
            Console.WriteLine(Message);
        }
    }
    public abstract class Tramp(TrampType type)
    {
        protected readonly TrampType type = type;

        public abstract TrampType Type { get; }

        public abstract void Active();
    }
    public class Pitfall : Tramp
    {
        public Pitfall() : base(TrampType.Pitfall) { }

        public override TrampType Type => TrampType.Pitfall;

        public override void Active()
        {
            Console.WriteLine("You fell into a pitfall!");
        }
    }
    public class BlockHabilityTramp : Tramp
    {
        public BlockHabilityTramp() : base(TrampType.BlockHabilityTramp) { }

        public override TrampType Type => TrampType.BlockHabilityTramp;

        public override void Active()
        {
            Console.WriteLine("Hability Blocked!");
        }
    }

    public class Spike : Tramp
    {
        public Spike() : base(TrampType.Spike) { }

        public override TrampType Type => TrampType.Spike;

        public override void Active()
        {
            Console.WriteLine("You stepped on a spike!");
        }
    }

    public class Fire : Tramp
    {
        public Fire() : base(TrampType.Fire) { }

        public override TrampType Type => TrampType.Fire;

        public override void Active()
        {
            Console.WriteLine("You got burned by fire!");
        }
    }
    public class Poison : Tramp
    {
        public Poison() : base(TrampType.Poison) { }

        public override TrampType Type => TrampType.Poison;

        public override void Active()
        {
            Console.WriteLine("You drink Poison!");
        }
    }
    public class Thorns : Tramp
    {
        public Thorns() : base(TrampType.Thorns) { }

        public override TrampType Type => TrampType.Thorns;

        public override void Active()
        {
            Console.WriteLine("You just fell into Thorns!");
        }
    }
    public class Hole : Tramp
    {
        public Hole() : base(TrampType.Hole) { }

        public override TrampType Type => TrampType.Hole;

        public override void Active()
        {
            Console.WriteLine("You just fell into a Hole!");
        }
    }

    public class Obstacle
    {
        public string Symbol { get; }
        public bool IsBlocking { get; }
        public string Name { get; }

        public Obstacle(string symbol, bool isBlocking, string name)
        {
            Symbol = symbol;
            IsBlocking = isBlocking;
            Name = name;
        }
    }
}