using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smolin_contest
{
    class Ball
    {
        public double FromX { get; private set; }
        public double FromY { get; private set; }
        public double ToX { get; private set; }
        public double ToY { get; private set; }

        public Ball(double fromX, double fromY, double toX, double toY)
        {
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
        }

        public double GetX(bool to)
        {
            return to ? ToX : FromX;
        }

        public double GetY(bool to)
        {
            return to ? ToY : FromY;
        }
    }
}
