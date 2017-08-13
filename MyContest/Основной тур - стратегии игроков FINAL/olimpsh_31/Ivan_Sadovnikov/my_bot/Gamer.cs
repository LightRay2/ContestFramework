using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace my_bot
{
    class Gamer
    {
        public Position position;
        public Position next;
        public bool have_ball;

        public Gamer(Position pos)
        {
            position = pos;
            have_ball = false;
        }
    }
}
