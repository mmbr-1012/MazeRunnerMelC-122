using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Tokens.Habilities
{
    public abstract class Hability
    {
        readonly string name;

        public Hability(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

        public abstract HabilityType Type { get; }

        public abstract void active();
    }
    public class ImmunityHability : Hability
    {
        public ImmunityHability(string name) : base(name)
        {
        }

        public override HabilityType Type { get { return HabilityType.Immunity; } }

        public override void active()
        {
            throw new NotImplementedException();
        }
    }
     internal class JumpObstacleHability : Hability
    {
        public JumpObstacleHability(string name) : base(name)
        {
        }

        public override HabilityType Type { get { return HabilityType.JumpObstacle; } }

        public override void active()
        {
            throw new NotImplementedException();
        }
    }
}

/* Hability:
 * -Es una clase abstracta, porque en el juego deben existir varios tipos de habilidades, las cuales compartiran caracteristicas, pero cada una tendra una funcionalidad diferente. 
 * -La clase abstracta sirver para englobal todos los tipos de habilidades, pero a su vez que cada tipo de habilidad que herede de ella, pueda implementar su propia funcionalidad. 
 * -Para crear un tipo nuevo de habilidad, basta con crear una clase nueva y que herede de esta clase, tambien se debe añadir el tipo nuevo en el enumerable HabilityType, que engloba los tipos
 *  de habilidades. Este enumerable con los tipos de habilidades, puede ayudar a preguntar facilmente por el tipo de habilidad de la que estamos hablando, pero no es necesario.
 */