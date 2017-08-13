using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Globalization;
//using System.*;

namespace Test
{
    public struct Point
        {
            public double X { get { return _x; } }
            public double Y { get { return _y; } }    
            double _x;
            double _y;
            public Point(double x, double y)
            {
                this._x = x;
                this._y = y;
            }
            public static bool operator ==(Point p1, Point p2)
            {
                if (Math.Abs(p1.X - p2.X) < 0.1 && Math.Abs(p1.Y - p2.Y) < 0.1)
                    return true;
                return false;
            }

            public static bool operator !=(Point p1, Point p2)
            {
                if (Math.Abs(p1.X - p2.X) < 0.1 && Math.Abs(p1.Y - p2.Y) < 0.1)
                    return false;
                return true;
            }
        }

    public struct MatchInfo
    {
        public List<Point> ourteam;
        public List<Point> theirteam;
        public int roundnum;
        public int ourscore;
        public int theirscore;
        public string comm;
        public Point ballpos;
        public Point balldir;
        public MatchInfo(bool t)
        {
            ourteam = new List<Point>();
            theirteam = new List<Point>();
            ballpos = new Point();
            balldir = new Point();
            roundnum = 0;
            ourscore = 0;
            theirscore = 0;
            comm = "";
        }
    }

    class Program
    {
        const double v = 2;
        const double vball = 2.5;
        const double ball = 6;
        const double balldist = 30;
        const double Pi = 3.14159605;
        static string ballkick = "";

        static double Dist(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        static bool IsEnemys(MatchInfo mi)
        {
            for (int i = 0; i < 5; i++)
                if (mi.theirteam[i] == mi.ballpos) return true;
            return false;
        }

        static bool IsOurs(MatchInfo mi)
        {
            for (int i = 0; i < 5; i++)
                if (mi.ourteam[i] == mi.ballpos) return true;
            return false;
        }

        static bool IsOurs(MatchInfo mi, int pl)
        {
            if (mi.ourteam[pl] == mi.ballpos) return true;
            return false;
        }

        static string Pass(MatchInfo mi, int pl)
        {
            if (IsOurs(mi))
            {
                return string.Format(@"{0} {1}
", mi.ourteam[pl].X, mi.ourteam[pl].Y);
                //mi.balldir = mi.ourteam[pl];
            }
            else
            { return ""; }
        }


        /*static Point Strat()
        {

        }*/

        static bool NoCrossing(MatchInfo mi, int pl, Point p, double speed)
        {
            int iter = (int)(Math.Min(Dist(p, mi.ourteam[pl]), balldist)/speed);
            for (int i = 0; i < mi.theirteam.Count; i++)
                for (int j = 0; j < Math.Min(Dist(p, mi.ourteam[pl]), balldist); j++)
                    if (Dist(mi.theirteam[i], new Point(mi.ourteam[pl].X + (j + 1) / (iter + 1) * (p.X - mi.ourteam[pl].X), mi.ourteam[pl].Y + (j + 1) / (iter + 1) * (p.Y - mi.ourteam[pl].Y))) < v * j)
                        return false;
            return true;
        }

        static string Evade(MatchInfo mi, int pl)
        {
            double gx = mi.ourteam[pl].X + 10; double gy = mi.ourteam[pl].Y;
            if (NoCrossing(mi, pl, new Point(gx, gy), vball))
                return String.Format(@" {0} {1}
", gx, gy);
            bool conf = true;
            for (int i = 0; i < mi.theirteam.Count; i++)
            {
                if (mi.theirteam[i].X - mi.ourteam[pl].X < 2.5 && mi.theirteam[i].X - mi.ourteam[pl].X > -1 && Math.Abs(mi.ourteam[pl].Y - mi.theirteam[i].Y) < 30)
                    for (double j = 0; j < (Pi / 2) && conf; j += 0.1)
                    {
                        conf = true;
                        if (NoCrossing(mi, pl, new Point(mi.ourteam[pl].X+2.5*Math.Cos(j), mi.ourteam[pl].Y+2.5*Math.Sin(j)), vball))
                        {
                            if (mi.ourteam[pl].X + 2.5 * Math.Cos(j) < gx) { gx = mi.ourteam[pl].X + 2.5 * Math.Cos(j); gy = mi.ourteam[pl].X + 2.5 * Math.Sin(j); }
                            conf = false;
                        }
                        if (NoCrossing(mi, pl, new Point(mi.ourteam[pl].X + 2.5 * Math.Cos(j), mi.ourteam[pl].Y - 2.5 * Math.Sin(j)), vball))
                        {
                            if (mi.ourteam[pl].X + 2.5 * Math.Cos(j) < gx) { gx = mi.ourteam[pl].X + 2.5 * Math.Cos(j); gy = mi.ourteam[pl].X - 2.5 * Math.Sin(j); }
                            conf = false;
                        }
                    }
                if (conf)
                {
                    ballkick = PassClosest(mi, pl);
                    return String.Format(@"{0} {1}
", mi.ourteam[pl].X + vball, mi.ourteam[pl].Y);
                }
            }
            return String.Format(@"{0} {1}
", gx, gy);
        }

        static string PassClosest(MatchInfo mi, int pl)
        {
            double mindist = 200;
            int passto = (pl+1)%5;
            bool found = false;
            for (int i = 0; i < mi.ourteam.Count; i++)
            {
                if (i!=pl && NoCrossing(mi, pl, mi.ourteam[i], ball) && Dist(mi.ourteam[pl], mi.ourteam[i]) < mindist)
                {
                    mindist = Dist(mi.ourteam[pl], mi.ourteam[i]);
                    passto = i;
                    found = true;
                }
                               
            }
            /*if (found)*/ return Pass(mi, passto);
            //else return "";
        }

        static bool IsPointToGoal(int pl, MatchInfo mi)
        {
            Point goal;
            for (int i = 1; i < 60; i++ )
            {
                goal = new Point(100, i);
                if ((Dist(goal, mi.ourteam[pl]) <= 30) && (NoCrossing(mi, pl, goal, ball)))
                    return true;
            }
            return false;
        }

        static Point PointToGoal(int pl, MatchInfo mi)
        {
            Point goal;
            for (int i = 1; i < 60; i++)
            {
                goal = new Point(100, i);
                if ((Dist(goal, mi.ourteam[pl]) <= 30) && (NoCrossing(mi, pl, goal, ball)))
                    return goal;
                
            }
            return new Point(-1, 200);
        }

        static void OutputGen(StringBuilder str, MatchInfo mi)
        {
            double delx = mi.balldir.X - mi.ballpos.X;
            double dely = mi.balldir.Y - mi.ballpos.Y;
            double balldist = Math.Sqrt(delx * delx + dely * dely);
            //string ballkick = "";
            Point ballnext;
            //if (balldist == 0) 
            ballnext = mi.ballpos;
            //else ballnext = new Point(mi.balldir.X+ball*delx/balldist, mi.balldir.Y+ball*delx/balldist);
            for (int i = 0; i < 5; i++)
            {
                if (!IsOurs(mi, i))
                {
                    if (!IsOurs(mi))
                    str.Append(string.Format(@"{0} {1}
", Convert.ToString(ballnext.X), Convert.ToString(ballnext.Y)));
                    else
                        if (ballnext.Y > mi.ourteam[i].Y)
                            str.Append(string.Format(@"{0} {1}
", mi.ourteam[i].X + vball, ballnext.Y + 5));
                        else
	{
                            str.Append(string.Format(@"{0} {1}
", mi.ourteam[i].X + vball, ballnext.Y - 5));
	}
                }
                else
                {
                    if (IsPointToGoal(i, mi))
                    {
                        Point goal = PointToGoal(i, mi);
                        ballkick = string.Format(@"{0} {1}
", goal.X, goal.Y);
                        str.Append(ballkick);
                    }
                    else
                    {
                        str.Append(Evade(mi, i));
                    }
                }
            }
            str.Append(ballkick);
            str.Append("LEROOOOOOOOOOOY JENKINS!");
        }

        static public List<string> UnparsedSplitLine(StreamReader strtx)
        {
            string str = strtx.ReadLine();
            return str.Split(' ').ToList<string>();
        }

        static void Main(string[] args)
        {
            MatchInfo mi = new MatchInfo(true);
            TextFileWork.Extract("input.txt", ref mi);
            StringBuilder outtextgen = new StringBuilder();
            OutputGen(outtextgen, mi);
            TextFileWork.Input("output.txt", outtextgen.ToString());
        }
    }
}
