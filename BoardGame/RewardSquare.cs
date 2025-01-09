using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using game.BoardGame;

namespace game.RewardSquare
{
    public class RewardSquare : Square
    { 
        private int totalSquares;
        private int rewardSquaresCount;
        private HashSet<int> rewardSquares;
        private HashSet<int> emptySquares;
        public RewardSquare(int totalSquare, int rewardSquaresCount)
        {
            this.totalSquares = totalSquare;
            this.rewardSquaresCount = rewardSquaresCount;
            rewardSquares = new HashSet<int>();
            emptySquares = new HashSet<int>();
            GenerateRewardSquares();
        }
        private void GenerateRewardSquares()
        {
            Random random = new Random();
            while (rewardSquares.Count < rewardSquaresCount)
            {
                int randomSquare = random.Next(1, totalSquares + 1);
                rewardSquares.Add(randomSquare);
            }
        }
        private void GenerateEmptySquares()
        {
            for (int i = 1; i <= totalSquares; i++)
            {
                if (!rewardSquares.Contains(i))
                {
                    emptySquares.Add(i);
                }
            }
        }
        public bool IsRewardSquare(int square)
        {
            return rewardSquares.Contains(square);
        }
        public bool IsEmptySquare(int square)
        {
            return emptySquares.Contains(square);
        }
        public void DisplayRewardSquares()
        {
            Console.WriteLine("You get a reward!!!!");
            foreach (var square in rewardSquares)
            {
                Console.WriteLine(square);
            }
            Console.WriteLine("Ohh no is empty");
            foreach(var square in emptySquares)
            {
                Console.WriteLine(square);
            }
        }
    }
}
