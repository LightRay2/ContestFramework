using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace my_bot
{
    class Game
    {
        public int team;
        public double min_action, my_gates, any_gates;
        public Position[] start_positions = new Position[5];

        public Game(int t)
        {
            team = t;
            if (team == 1)
            {
                start_positions[0] = new Position(20.0, 10.0);
                start_positions[1] = new Position(20.0, 30.0);
                start_positions[2] = new Position(20.0, 50.0);
                start_positions[3] = new Position(42.0, 21.0);
                start_positions[4] = new Position(42.0, 38.0);
            }
            else
            {
                start_positions[0] = new Position(80.0, 10.0);
                start_positions[1] = new Position(80.0, 30.0);
                start_positions[2] = new Position(80.0, 50.0);
                start_positions[3] = new Position(56.0, 21.0);
                start_positions[4] = new Position(56.0, 38.0);
            }

            /*if (team == 1)
            {
                start_positions[0] = new Position(20.0, 5.0);
                start_positions[1] = new Position(30.0, 25.0);
                start_positions[2] = new Position(53.0, 50.0);
                start_positions[3] = new Position(68.0, 30.0);
                start_positions[4] = new Position(87.0, 5.0);
            }
            else
            {
                start_positions[0] = new Position(80.0, 5.0);
                start_positions[1] = new Position(70.0, 25.0);
                start_positions[2] = new Position(53.0, 50.0);
                start_positions[3] = new Position(32.0, 30.0);
                start_positions[4] = new Position(13.0, 5.0);
            }*/
        }
    }
}
