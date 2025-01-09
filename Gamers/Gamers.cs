using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.Tokens;

namespace game.Gamers
{
    public class Gamer
    {
        readonly string name;
        int score;
        readonly Token[] tokens;
        readonly Task[] tasksToWind;

        public Gamer(string name, Token[] tokens, Task[] tasksToWind)
        {
            score = 0;
            this.name = name;
            this.tokens = tokens;
            this.tasksToWind = tasksToWind;
        }

        public string Name { get { return name; } }

        public int Score { get { return score; } }

        public Token[] Tokens { get { return tokens; } }

        public Task[] TasksToWind { get { return tasksToWind; } }

        public int IncrementScore(int points)
        {
            score += points;
            return score;
        }

        public int DecrementScore(int points)
        {
            score -= points;
            return score;
        }
    }
}