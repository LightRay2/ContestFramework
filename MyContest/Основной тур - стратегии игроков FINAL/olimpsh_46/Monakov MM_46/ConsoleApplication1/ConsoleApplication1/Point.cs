using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Point
    {
        public double x;
        public double y;
        public Point(double x1 = 0, double y1 = 0)
        {
            x = x1;
            y = y1;
        }
        public Point()
        {
            x = 0;
            y = 0;
        }
    }
}
