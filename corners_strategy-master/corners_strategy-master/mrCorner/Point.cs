using System;
using System.Collections.Generic;

namespace mrCorner
{
    class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private static Dictionary<int, Point> Cache = new Dictionary<int, Point>();

        private Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Get(int x, int y)
        {
            int hash = x * 1000 + y;

            Point point;

            bool isCached = Cache.TryGetValue(hash, out point);

            if (isCached)
            {
                return point;
            }

            point = new Point(x, y);
            Cache.Add(hash, point);
            return point;
        }

        public bool Exists => X > -1 && X < 10 && Y > -1 && Y < 10;

        public Point Up => Get(X - 1, Y);
        public Point Down => Get(X + 1, Y);
        public Point Left => Get(X, Y - 1);
        public Point Right => Get(X, Y + 1);

        public int DistTo(Point other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        public override string ToString()
        {
            return string.Format("[{0} {1}]", X, Y);
        }
    }
}
