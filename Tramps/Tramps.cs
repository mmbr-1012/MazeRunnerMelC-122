using game.BoardGame;

namespace game.Tramps   
{
    public abstract class Tramp(TrampType type) : Square
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
}