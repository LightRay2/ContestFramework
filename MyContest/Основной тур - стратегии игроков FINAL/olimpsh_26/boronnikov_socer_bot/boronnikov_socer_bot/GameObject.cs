using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace boronnikov_socer_bot
{
    class GameObject
    {
        public double x;
        public double y;
        public GameObject(string s)
        {
            x = double.Parse(s.Split(' ')[0], CultureInfo.InvariantCulture);
            y = double.Parse(s.Split(' ')[1], CultureInfo.InvariantCulture);
        }
        public GameObject(double _x, double _y)
        {
            x = _x;
            y = _y;
        }
        public string pos
        {
            get
            {
                return Convert.ToString(x) + ' ' + Convert.ToString(y);
            }
        }
    }

    class Ball : GameObject
    {
        public Ball(string s)
            : base(s)
        {
        }
        public Ball(double _x, double _y)
            : base(_x, _y)
        {
        }
    }

    class Player : GameObject
    {
        public Player(string s)
            : base(s)
        {
        }
        public Player(double _x, double _y)
            : base(_x, _y)
        {
        }
        public bool haveBall(Ball b) { return b.x == x && b.y == y; }
    }
}
