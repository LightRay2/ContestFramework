using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoccerPlayers
{
    internal class Easy2
    {
        public Easy2()
        {
        }

        internal void GoFullRandomMoves()
        {
            var sb = new StringBuilder();
            var rand = new Random();
            for(int  i = 0; i < 5; i++)
            {
                sb.AppendLine(string.Format("{0} {1}", rand.NextDouble() * 100, rand.NextDouble() * 50));

            }
            File.WriteAllText("output.txt", sb.ToString());
        }
    }
}