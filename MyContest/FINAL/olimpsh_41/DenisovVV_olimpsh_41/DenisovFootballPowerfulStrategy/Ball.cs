using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DenisovFootballPowerfulStrategy
{
    public class Ball
    {
        public Points start { get; set; }
        public Points dest { get; set; }

        public Ball(Points start, Points dest)
        {
            this.start = start;
            this.dest = dest;
        }
    }
}
