using System.IO;

namespace mrCorner
{
    class Action
    {
        public Point From;
        public Point To;

        public Action(Point from, Point to)
        {
            From = from;
            To = to;
        }

        public void WriteAsOutput()
        {
            File.WriteAllText("output.txt", string.Format("{0} {1} {2} {3}", From.Y, From.X, To.Y, To.X));
        }

        public override string ToString()
        {
            return string.Format("({0} {1})", From, To);
        }

        public int Inc()
        {
            return From.DistTo(Point.Get(9, 9)) - To.DistTo(Point.Get(9, 9));
        }
    }
}
