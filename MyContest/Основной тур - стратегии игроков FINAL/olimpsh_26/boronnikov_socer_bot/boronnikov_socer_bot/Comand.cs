using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace boronnikov_socer_bot
{
    class Comand
    {
        public Player[] players;
        public Ball ball;
        public Comand(Player[] pl, Ball b)
        {
            players = pl;
            ball = b;
        }
        public bool weHaveBall()
        {
            for (int i = 0; i < 5; i++)
            {
                if (players[i].haveBall(ball))
                {
                    return true;
                }
            }
            return false;
        }
        public void goLeft()
        {
            for (int i = 0; i < 5; i++)
            {
                players[i].x += 5;
            }
        }
        public void goToBall()
        {
            for (int i = 0; i < 5; i++)
            {
                players[i].x += ball.x;
                players[i].y += ball.y;
            }
        }
    }
}
