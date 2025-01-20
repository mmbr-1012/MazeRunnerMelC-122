using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Tramps
{
    public class BlockHabilityTramp(TrampType Type) : Tramp(TrampType.BlockHabilityTramp)
    {
        public override TrampType Type => TrampType.BlockHabilityTramp;

        public override void Active()
        {
            Console.WriteLine("Ability Blocked!");
        }
    }
}