using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smolin_contest
{
    class Player
    {
        public List<Bot> Bots { get; private set; }
        public int Goals { get; private set; }
        public bool HasBall
        {
            get
            {
                foreach (Bot bot in Bots)
                    if (bot.HasBall)
                        return true;
                return false;
            }
        }

        public Player(int goals)
        {
            Bots = new List<Bot>();
            Goals = goals;
        }
    }
}
