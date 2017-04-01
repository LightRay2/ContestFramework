using System;
using System.Collections.Generic;

namespace mrCorner
{
    class Strategy
    {
        private Map Map { get; set; } = new Map();

        private const int MaxDepth = 3;
        private const int Difficulty = 100;
        private const int MaxDifficulty = 100;

        public Strategy()
        {
        }

        public void Run()
        {
            Map.ReadInput();
            Random rand = new Random();

            Action bestAct;
            EstimateActionDeep(out bestAct);

            List<Action> allActs = Map.PossibleActionsSide(1);
            Action randAct = allActs[rand.Next(allActs.Count)];

            Action resAct = rand.Next(MaxDifficulty) < Difficulty ? bestAct : randAct;
            resAct.WriteAsOutput();
        }

        private int EstimateActionDeep(out Action action, int depth = 0)
        {
            if (depth == MaxDepth)
            {
                action = null;
                return Map.EstimateStateEuristic();
            }

            List<Action> possibleActs = Map.PossibleActionsSide(1);

            int bestScore = 0;
            List<Action> bestActs = new List<Action>();

            foreach (var act in possibleActs)
            {
                Map.ApplyAction(act);

                Action tmp;
                int score = EstimateActionDeep(out tmp, depth + 1);

                if (depth == 0 && act.Inc() >= 6)
                {
                    score += 20;
                }

                Map.RevertAction(act);

                if (score >= bestScore)
                {
                    if (score > bestScore)
                    {
                        bestActs.Clear();
                    }

                    bestActs.Add(act);
                    bestScore = score;
                }
            }

            action = bestActs[(new Random()).Next(bestActs.Count)];
            return bestScore;
        }
    }
}
