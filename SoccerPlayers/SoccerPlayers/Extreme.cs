using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SoccerPlayers
{
    public class Extreme
    {

        public class Point { public double x, y; }
        void Go()
        {
            //read

            int time = ReadInt(), scoreWe = ReadInt(), scoreEnemy = ReadInt();
            Point ball = new Point { x = ReadDouble(), y = ReadDouble() };
            Point ballFinish = new SoccerPlayers.Extreme.Point { x = ReadDouble(), y = ReadDouble() };
            List<Point> we = new List<Point>(), enemy = new List<SoccerPlayers.Extreme.Point>();
            for (int i = 0; i < 5; i++)
            {
                we.Add(new Point { x = ReadDouble(), y = ReadDouble() });
            }
            for (int i = 0; i < 5; i++)
            {
                enemy.Add(new Point { x = ReadDouble(), y = ReadDouble() });
            }
            var aim = Enumerable.Range(1, 5).Select(x => new Point()).ToList();


            Point memoryPass = new Point();
            int memoryTime = ReadInt();
            if (memoryTime != -1)
            {
                memoryPass.x = ReadDouble();
                memoryPass.y = ReadDouble();

                if (PointNear(memoryPass, ballFinish) == false)
                    memoryTime = -1;
            }
            Point ballAim = new Point();


            


            bool weHaveBall = we.Any(x => x.x == ball.x && x.y == ball.y);
            if (weHaveBall)
            {
                memoryTime = -1;


                if (false)//too primitive
                {
                    RandomPass(time, ball, out memoryPass, out memoryTime, ref ballAim, aim);
                }
                else
                {
                    int ballOwner = we.FindIndex(x => x.x == ball.x && x.y == ball.y);


                    var kickMatrix = new List<Tuple<double, Point>>();
                    Point goalPossible =  FillSafeMatrixAndCheckGoal(kickMatrix, ball, we,enemy, ballOwner);
                    

                    if (goalPossible != null)
                    {
                        Pass(time, goalPossible, out memoryPass, out memoryTime, ref ballAim, aim,we);
                        SetPeopleAround(new SoccerPlayers.Extreme.Point { x = 0, y = 30 }, we, aim, 4);
                    }
                    else
                    {
                        Point bestPass = new Point { x = 100, y = 30 };

                        bool canGiveNicePass = AnalizeKickMatrix(kickMatrix,  we, ref bestPass);
                        var enemiesToBall = enemy.OrderBy(x => Dist(x, ball)).TakeWhile(x=> Dist(x, ball)<12).ToList();

                        if (canGiveNicePass)
                        {
                            //why not
                            Pass(time, bestPass, out memoryPass, out memoryTime, ref ballAim, aim,we);
                        }
                        else
                        {
                            if(enemiesToBall.Count == 1)
                            {
                                aim[ballOwner] = new Point
                                {
                                    x = ball.x + (ball.x - enemiesToBall[0].x) * 100,
                                    y = ball.y + (ball.y - enemiesToBall[0].y) * 100
                                };
                                SetPeopleAround(we[ballOwner], we, aim, ballOwner);
                                
                                if(Dist(enemiesToBall.First(),ball) < 5) //догоняет на нашей половине
                                {
                                    Pass(time, bestPass, out memoryPass, out memoryTime, ref ballAim, aim,we);
                                }
                            }
                            else if(enemiesToBall.Count >1)
                            {
                                //pass
                                Pass(time, bestPass, out memoryPass, out memoryTime, ref ballAim, aim,we);
                            }
                            else
                            {
                                aim[ballOwner] = new Point
                                {
                                    x = ball.x + 2,
                                    y = 30
                                };
                                SetPeopleAround(we[ballOwner], we, aim, ballOwner);
                            }
                        }
                    }
                }
            }
            else
            {
                bool enemyHasBall = enemy.Any(x => x.x == ball.x && x.y == ball.y); ;
                if (enemyHasBall)
                {
                    memoryTime = -1;
                    if (ball.x < 10)
                    {
                        //all to ball
                        aim = aim.Select(x => ball).ToList();
                    }
                    else
                    {
                        SetDefence(ball, we, enemy, aim);
                    }
                }
                else
                {
                    bool wasPass = memoryTime != -1 ;
                    if (wasPass)
                    {
                        List<int> indices = new List<int> { 0, 1, 2, 3, 4 };
                        int closest = indices.OrderBy(x => Dist(we[x], memoryPass)).First();
                        aim[closest] = memoryPass;
                        SetPeopleAround(memoryPass, we, aim, closest);
                    }
                    else
                    {
                        SetDefence(ball, we, enemy, aim);
                        //perexvat

                        var ourCross = Perexvat(ball, ballFinish, we,-1);
                        var enemyCross = Perexvat(ball, ballFinish, enemy,-1);

                        if (PointNear(ourCross.Item2, ballFinish) == false ||
                            ourCross.Item3 < enemyCross.Item3+2)
                        {
                            aim[ourCross.Item1] = ourCross.Item2;
                        }
                    }


                    memoryTime--;

                }
            }

            aim.ForEach(x => Write(x.x, x.y));
            if (ballAim.x != 0)
            {
                Write(ballAim.x, ballAim.y);
            }
            Write("memory", Math.Max(-1, memoryTime), memoryPass.x, memoryPass.y);
        }

        private bool AnalizeKickMatrix(List<Tuple<double, Point>> kickMatrix, List<Point> we, ref Point bestPass)
        {
            var withoutPerexvat = kickMatrix.Where(x => x.Item1 > 4).ToList();

            

             bestPass = withoutPerexvat.OrderByDescending(x => x.Item2.x).Select(x=>x.Item2).FirstOrDefault();

            if (bestPass == null) //all with perexvat
                bestPass = kickMatrix.OrderByDescending(x => x.Item1).Select(x => x.Item2).FirstOrDefault();
            else
                return true;

            //impossible
           // if (bestPass == null) //all with perexvat
            //    bestPass = kickMatrix.OrderByDescending(x => x.Item1).Select(x => x.Item2).FirstOrDefault();

            if (bestPass == null)
                bestPass = new Point { x = 100, y = 30 };

            return false;
        }

        private Point FillSafeMatrixAndCheckGoal(List<Tuple<double,Point>> kickMatrix, Point ball,List<Point> we,  List<Point> enemy, int ballOwner)
        {
            double dx = 100.0 / 20;
            double dy = 60.0 / 20;

            for(double x = 0; x < 105; x += dx)
            {
                for(double y = 0; y < 60; y += dy)
                {
                    if(x == 40 && y == 12)
                    {

                    }
                    var point = new Point { x = x, y = y };
                    if (Dist(ball, point) > 30 || Dist(ball, point) < 10)
                        continue;
                    var ourCross = Perexvat(ball, point, we, ballOwner);
                    
                    var enemyCross = Perexvat(ball, point, enemy,-1);
                    if (PointNear( enemyCross.Item2, point) == true && enemy.Min(xx=>Dist(xx, point)) > 10)
                        kickMatrix.Add(Tuple.Create( enemyCross.Item3- ourCross.Item3/1.25 , point));
                }
            }

            //var bestt = kickMatrix.FirstOrDefault(x => x.Item2.x > 96);
          //  if (bestt == null)
                return null;
          //  return bestt.Item2;
        }

        private  void RandomPass(int time, Point ball, out Point memoryPass, out int memoryTime, ref Point ballAim, List<Point> aim)
        {
            ballAim.y = ball.y + new Random().NextDouble() * 29;

            ballAim.x = ball.x + Math.Sqrt(30 * 30 - (ballAim.y - ball.y) * (ballAim.y - ball.y));
            memoryTime = time;
            memoryPass = ballAim;

            for (int i = 0; i < 5; i++)
                aim[i] = ballAim;
        }

        private  void Pass(int time, Point where, out Point memoryPass, out int memoryTime, ref Point ballAim, List<Point> aim, List<Point> we)
        {
            ballAim = where;
            memoryTime = time;
            memoryPass = ballAim;

            double bestDist = 100000;
            int bestIndex = -1;
            for (int i = 0; i < 5; i++)
                if(Dist(we[i], ballAim) < bestDist)
                {
                    bestDist = Dist(we[i], ballAim);
                    bestIndex = i;
                }

            aim[bestIndex] = ballAim;
            SetPeopleAround(ballAim, we, aim, bestIndex);
        }







        private void SetDefence(Point ball, List<Point> we, List<Point> enemy, List<Point> aim)
        {
            aim[0].x = ball.x - 10;
            aim[0].y = ball.y - 3;

            aim[1].x = ball.x - 10;
            aim[1].y = ball.y + 3;

            List<int> others = new List<int> { 2, 3, 4 };
            int closest = others.OrderBy(x => Dist(we[x], ball)).First();

            aim[closest] = ball;
            others.Remove(closest);

            List<int> enemies = new List<int> { 0, 1, 2, 3, 4 };
            var enemiesToBall = enemies.OrderBy(x => Dist(enemy[x], ball)).ToList();

            aim[others[0]] = new Point
            {
                x = (ball.x + enemy[enemiesToBall[1]].x) / 2,
                y = (ball.y + enemy[enemiesToBall[1]].y) / 2
            };
            aim[others[1]] = new Point
            {
                x = (ball.x + enemy[enemiesToBall[2]].x) / 2,
                y = (ball.y + enemy[enemiesToBall[2]].y) / 2
            };
        }

        bool PointNear(Point one, Point two)
        {
            return Dist(one, two) < 1;
        }

        void SetPeopleAround(Point center, List<Point> all, List<Point> aim, int ignoreIndex)
        {
            center = new Point { x = center.x, y = center.y };
            var rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                if (i == ignoreIndex)
                    continue;


                bool distok = Math.Abs(Dist(all[i], center) - 25) < 5;

                if (distok == false)
                {
                    if (Dist(all[i], center) > 25)
                        aim[i] = center;
                    else
                    {
                        aim[i] = center;
                        aim[i].x = all[i].x + (all[i].x - center.x) * 100;
                        aim[i].y = all[i].y + (all[i].y - center.y) * 100;
                    }
                }
                else
                {
                    double best = 0;
                    for (int j = 0; j < 100; j++)
                    {
                        double addx = rand.NextDouble() * 5 - 2.5;
                        double addy = rand.NextDouble() * 5 - 2.5;

                        var point = new Point { x = all[i].x + addx, y=all[i].y + addy };

                        var others = new List<Point>(all);
                        others.Remove(all[i]);

                        var nearest = others.Min(x => Dist(all[i], x));
                        if(nearest > best)
                        {
                            best = nearest;
                            aim[i] = point;
                        }
                    }
                }


            }

            //double total = 10000000;
            //List<int> best = new List<int>();
            //var rand = new Random();
            //for(int i = 0; i < 100;i++)
            //{
            //    indices = indices.OrderBy(x => rand.NextDouble()).ToList();

            //}
        }

        Tuple<int, Point, double> Perexvat(Point ball, Point ballFinish, List<Point> players, int ignoreIndex)
        {
            int n = 20;
            var answer = Tuple.Create(0, ballFinish, Dist(players[0], ballFinish));
            for (int part = 0; part <= n; part++)
            {

                Point middle = new Point
                {
                    x = ball.x + (ballFinish.x - ball.x) / n * part,
                    y = ball.y + (ballFinish.y - ball.y) / n * part,
                };
                bool ok = false;
                for (int i = 0; i < 5; i++)
                {
                    if (i == ignoreIndex && part < n)
                        continue;

                    var secondsBallToMiddle = Dist(middle, ball) / 6;
                    var secondsManToMiddle = Dist(middle, players[i]) / 2;

                    if (secondsManToMiddle -1 < secondsBallToMiddle || n == part)
                    {
                        ok = true;
                        //possible
                        if (answer.Item3 > Dist(middle, players[i]))
                        {
                            answer = Tuple.Create(i, middle, Dist(middle, players[i]));
                        }

                    }
                }
                if (ok)
                    break;
            }

            return answer;
        }

        double Dist(Point one, Point two)
        {
            return Math.Sqrt((one.x - two.x) * (one.x - two.x) + (one.y - two.y) * (one.y - two.y));
        }


        #region Main

        protected static TextReader reader;
        protected static TextWriter writer;
        public static void Main2()
        {
            if (Debugger.IsAttached)
            {
                reader = new StreamReader("..\\..\\input.txt");
                //reader = new StreamReader(Console.OpenStandardInput());
                //writer = Console.Out;
                writer = new StreamWriter("..\\..\\output.txt");
            }
            else
            {
                //     reader = new StreamReader(Console.OpenStandardInput());
                //     writer = new StreamWriter(Console.OpenStandardOutput());
                reader = new StreamReader("input.txt");
                //reader = new StreamReader(Console.OpenStandardInput());
                //writer = Console.Out;
                writer = new StreamWriter("output.txt");
            }
            try
            {
                //var thread = new Thread(new Solver().Solve, 1024 * 1024 * 128);
                //thread.Start();
                //thread.Join();
                new Extreme().Go();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
#if DEBUG
#else
            throw;
#endif
            }
            reader.Close();
            writer.Close();
        }

        #endregion

        #region Read / Write
        private static Queue<string> currentLineTokens = new Queue<string>();
        private static string[] ReadAndSplitLine() { return reader.ReadLine().Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries); }
        public static string ReadToken() { while (currentLineTokens.Count == 0) currentLineTokens = new Queue<string>(ReadAndSplitLine()); return currentLineTokens.Dequeue(); }
        public static int ReadInt() { return int.Parse(ReadToken()); }
        public static long ReadLong() { return long.Parse(ReadToken()); }
        public static double ReadDouble() { return double.Parse(ReadToken(), CultureInfo.InvariantCulture); }
        public static int[] ReadIntArray() { return ReadAndSplitLine().Select(int.Parse).ToArray(); }
        public static long[] ReadLongArray() { return ReadAndSplitLine().Select(long.Parse).ToArray(); }
        public static double[] ReadDoubleArray() { return ReadAndSplitLine().Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray(); }
        public static int[][] ReadIntMatrix(int numberOfRows) { int[][] matrix = new int[numberOfRows][]; for (int i = 0; i < numberOfRows; i++) matrix[i] = ReadIntArray(); return matrix; }
        public static int[][] ReadAndTransposeIntMatrix(int numberOfRows)
        {
            int[][] matrix = ReadIntMatrix(numberOfRows); int[][] ret = new int[matrix[0].Length][];
            for (int i = 0; i < ret.Length; i++) { ret[i] = new int[numberOfRows]; for (int j = 0; j < numberOfRows; j++) ret[i][j] = matrix[j][i]; }
            return ret;
        }
        public static string[] ReadLines(int quantity) { string[] lines = new string[quantity]; for (int i = 0; i < quantity; i++) lines[i] = reader.ReadLine().Trim(); return lines; }
        public static void WriteArray<T>(IEnumerable<T> array) { writer.WriteLine(string.Join(" ", array)); }
        public static void Write(params object[] array) { WriteArray(array); }
        public static void WriteLines<T>(IEnumerable<T> array) { foreach (var a in array) writer.WriteLine(a); }
        private class SDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        {
            public new TValue this[TKey key]
            {
                get { return ContainsKey(key) ? base[key] : default(TValue); }
                set { base[key] = value; }
            }
        }
        private static T[] Init<T>(int size) where T : new() { var ret = new T[size]; for (int i = 0; i < size; i++) ret[i] = new T(); return ret; }
        #endregion
    }
}