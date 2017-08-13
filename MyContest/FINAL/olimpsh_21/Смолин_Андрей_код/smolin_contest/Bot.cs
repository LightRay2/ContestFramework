using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smolin_contest
{
    class Bot
    {
        public double X { get; set; }
        public double Y { get; set; }
        public bool HasBall { get; private set; }

        public Bot(double x, double y, bool hasBall = false)
        {
            X = x;
            Y = y;
            HasBall = hasBall;
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", X, Y);
        }
    }
}
