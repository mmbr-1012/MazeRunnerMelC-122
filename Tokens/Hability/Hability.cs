using System;
using System.Collections.Generic;
using System.Threading;

namespace BoardGame
{
    public class Ability
    {
        public string Name { get; }
        public int Cooldown { get; }
        public int Duration { get; }
        public string Description => $"{Name} (CD: {Cooldown}, Dur: {Duration})";
        private int currentCooldown;

        public Ability(string name, int cooldown, int duration)
        {
            Name = name;
            Cooldown = cooldown;
            Duration = duration;
        }

        public bool TryUse()
        {
            if (currentCooldown > 0) return false;
            currentCooldown = Cooldown;
            return true;
        }

        public void UpdateCooldown() => currentCooldown = Math.Max(0, currentCooldown - 1);
    }
}