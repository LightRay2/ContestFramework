using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace my_bot
{
    class Position
    {
        public double x, y;

        public Position(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public Position(String pos)
        {
            pos = pos.Replace('.', ',');
            string[] b = pos.Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries);
            x = double.Parse(b[0]);
            y = double.Parse(b[1]);
        }

        public string GetString()
        {
            return x + " " + y;
        }
    }
}
