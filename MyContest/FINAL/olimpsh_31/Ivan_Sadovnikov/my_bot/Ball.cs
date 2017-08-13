using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace my_bot
{
    class Ball
    {
        public Position current;
        public Position next;
        public bool kicked;

        public Ball(Position _current, Position _next)
        {
            current = _current;
            next = _next;
            kicked = false;
        }

        public void Kick(Position pos)
        {
            next = pos;
            kicked = true;
        }
    }
}
