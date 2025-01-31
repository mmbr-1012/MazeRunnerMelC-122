using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Tokens.Habilities
{
    public class Hability
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public int CurrentCooldown { get; set; }
        public int Cost { get; set; } // Costo en salud o recursos

        public Hability(string name, string description, int cooldown, int cost)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            CurrentCooldown = 0;
            Cost = cost;
        }

        public bool CanUse()
        {
            return CurrentCooldown <= 0;
        }

        public void Use()
        {
            CurrentCooldown = Cooldown;
        }

        public void Update()
        {
            if (CurrentCooldown > 0)
                CurrentCooldown--;
        }
    }
}

/* Hability:
 * -Es una clase abstracta, porque en el juego deben existir varios tipos de habilidades, las cuales compartiran caracteristicas, pero cada una tendra una funcionalidad diferente. 
 * -La clase abstracta sirver para englobal todos los tipos de habilidades, pero a su vez que cada tipo de habilidad que herede de ella, pueda implementar su propia funcionalidad. 
 * -Para crear un tipo nuevo de habilidad, basta con crear una clase nueva y que herede de esta clase, tambien se debe añadir el tipo nuevo en el enumerable HabilityType, que engloba los tipos
 *  de habilidades. Este enumerable con los tipos de habilidades, puede ayudar a preguntar facilmente por el tipo de habilidad de la que estamos hablando, pero no es necesario.
 */