using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
namespace SoccerPlayers
{
    internal class Hard30
    {
        public Hard30()
        {
        }

        internal void GoGoodDefence()
        {
            var reader = new StringReader(File.ReadAllText("input.txt"));
            Point ball = new Point();
            Point ballEnd = new Point();
            List<Point> our = new List<Point>();
            var enemy = new List<Point>();
            {
                var ss = reader.ReadLine().Split(' ');
                ball.x = double.Parse(ss[0], CultureInfo.InvariantCulture);
                ball.y = double.Parse(ss[1], CultureInfo.InvariantCulture);
                ballEnd.x = double.Parse(ss[2], CultureInfo.InvariantCulture);
                ballEnd.y = double.Parse(ss[3], CultureInfo.InvariantCulture);
            }
            {
                for(int i = 0; i < 10; i++)
                {
                    var ss = reader.ReadLine().Split(' ');
                    Point p = new SoccerPlayers.Hard30.Point { x = double.Parse(ss[0], CultureInfo.InvariantCulture), y = double.Parse(ss[1], CultureInfo.InvariantCulture) };
                    p.index = i % 5;
                    if (i < 5)
                        our.Add(p);
                    else
                        enemy.Add(p);
                }
                 
            }

            bool weHaveBall = our.Any(x => x.x == ball.x && x.y == ball.y);
            if (weHaveBall == false) {
                var ourInter = canInterrupt(our, ball, ballEnd);
                var enemyInter = canInterrupt(enemy, ball, ballEnd);
                if (ourInter.Item2 / 1.3 < enemyInter.Item2)
                {

                        for (int i = 0; i < 5; i++)
                    {
                        our[i].aimx = ourInter.Item1.x;
                        our[i].aimy = 5 + i * 8; 
                    }
                    if (ourInter.Item2 < double.MaxValue / 100)
                    {
                        our[ourInter.Item3].aimx = ourInter.Item1.x;
                        our[ourInter.Item3].aimy = ourInter.Item1.y;
                    }
                   
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        our[i].aimx = ball.x - 10;
                        our[i].aimy = 5 + i * 8;
                    }
                }
            }
            else
            {
                var rand = new Random();
                var closest = enemy.Min(x => dist(x, ball));
                for (int i = 0; i < 5; i++)
                {
                    our[i].aimx = our[i].x + 100;
                    our[i].aimy = our[i].y;
                }
                if (closest > 5)
                {
                    var res2 = new List<string>();
                    for (int i = 0; i < 5; i++)
                    {
                        res2.Add(string.Format("{0} {1}", our[i].aimx, our[i].aimy));
                    }
                    res2.Add(string.Format("{0} {1}", ball.x + 20, 5 + rand.NextDouble() * 40));
                    File.WriteAllLines("output.txt", res2);

                    return;
                }
            }


            var res = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                res.Add(string.Format("{0} {1}", our[i].aimx, our[i].aimy));
            }
            File.WriteAllLines("output.txt",res);
        }

        public class Man
        {
            public double x, y;

        }

        public class Point
        {
            public double x, y;
            public double distToBall;
            public int index;
            public double aimx, aimy;
        }

        double faster = 10.0 / 3.0;
        public Tuple<Point , double, int> canInterrupt(List<Point> points, Point ballStart, Point ballFinish)
        {
            Point dif = new Point(); dif.x = (ballFinish.x - ballStart.x) / 10;
            dif.y = (ballFinish.y - ballStart.y) / 10;

            int interrupter = -1;
            double bestDist = double.MaxValue;
            Point go = new Point();
            for(int i = 0; i <= 10; i++)
            {
                Point to = new Point{  x = ballStart.x + dif.x * i, y = ballStart.y+ dif.y * i};
                var sorted = points.OrderBy(p =>  (dist(p, to)*faster <= dist(ballStart, to))? dist(p, to) : double.MaxValue ).ToList();

                sorted[0].distToBall = dist(sorted[0], to);// (dist(sorted[0], to) * faster <= dist(ballStart, to)) ? dist(sorted[0], to) : double.MaxValue;
                if (sorted[0].distToBall < bestDist)
                {
                    bestDist = sorted[0].distToBall;
                    interrupter = sorted[0].index;
                    go = to;
                }

                //if(i == 10 )
                //{
                //    if(dist(sorted[0], to) < bestDist)
                //    {
                //        bestDist = dist(sorted[0], to);
                //        interrupter = sorted[0].index;
                //        go = to;
                //    }
                //}
            }
            


            return Tuple.Create(go, bestDist, interrupter);
        }

        

        public double dist(Point a, Point b) { return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y)); }
    }
}