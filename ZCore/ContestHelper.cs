using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    static class ContestHelper
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static List<Tuple<T,T>> RobinRoundTournament<T>(List<T> ListTeam) where T: class 
        {
            List<Tuple<T, T>> pairs = new List<Tuple<T,T>>(),
                res = new List<Tuple<T,T>>();
            for (int i = 0; i < ListTeam.Count - 1; i++)
            {
                for (int j = i + 1; j < ListTeam.Count; j++)
                {
                    pairs.Add(Tuple.Create(ListTeam[i], ListTeam[j]));
                }
            }
            pairs.Shuffle();

             //порядок
            foreach (var pair in pairs)
            {
                var one = pair.Item1;
                
                //последняя добавленная игра с участием первой команды
                var lastGame = res.LastOrDefault(x => x.Item1 == one || x.Item2 == one);
                if (lastGame == null || lastGame.Item2 == one)
                    res.Add(pair);
                else
                    res.Add(Tuple.Create(pair.Item2, pair.Item1));
            }

            return res;
        }
    }
}
