using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.Gamers;

namespace game.Tokens.Habilities
{
    public class Hability
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public int CurrentCooldown { get; set; }
        public int Cost { get; set; }
        public static bool IsBlocked { get; private set; }
        public Hability(string name, string description, int cooldown, int cost)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            CurrentCooldown = 0;
            Cost = cost;
        }

        public void Use(Player player)
        {
            if (!IsBlocked && player.CurrentHealth >= Cost)
            {
                player.CurrentHealth -= Cost;
                Execute(player);
            }
        }
        protected virtual void Execute(Player player)
        {
            Console.WriteLine($"{player.Name} uses {Name}");
        }
        public static void Block()
        {
            IsBlocked = true;
        }

        public void Unblock()
        {
            IsBlocked = false;
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
    public class HealingAbility : Hability
    {
        private readonly int healingAmount;

        public HealingAbility(string name, string description, int cost, int healingAmount, int cooldown)
            : base(name, description, cost, cooldown)
        {
            this.healingAmount = healingAmount;
        }

        protected override void Execute(Player player)
        {
            int finalHealth = Math.Min(player.Health, player.CurrentHealth + healingAmount);
            player.CurrentHealth = finalHealth;
            Console.WriteLine($"{player.Name} heals for {healingAmount} health points");
        }
    }
    public class ShieldAbility : Hability
    {
        private readonly int shieldStrength;

        public ShieldAbility(string name, string description, int cost, int shieldStrength, int cooldown)
            : base(name, description, cost, cooldown)
        {
            this.shieldStrength = shieldStrength;
        }

        protected override void Execute(Player player)
        {
            player.AddShield(shieldStrength);
            Console.WriteLine($"{player.Name} gains a shield of {shieldStrength} points");
        }
    }
    public class DamageAbility : Hability
    {
        private readonly int damage;
        private readonly int range;

        public DamageAbility(string name, string description, int cost, int damage, int range, int cooldown)
            : base(name, description, cost, cooldown)
        {
            this.damage = damage;
            this.range = range;
        }

        protected override void Execute(Player player)
        {
            var affectedPlayers = GetPlayersInRange(player, range);
            foreach (var target in affectedPlayers)
            {
                target.TakeDamage(damage);
                Console.WriteLine($"{player.Name} attacks {target.Name} for {damage} damage");
            }
        }

        private List<Player> GetPlayersInRange(Player source, int range)
        {
            // Implementar lógica para encontrar jugadores en rango
            return new List<Player>();
        }
    }
}