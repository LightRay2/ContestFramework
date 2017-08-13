using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Task1
{
    class Program
    {

        public struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X, Y;

            public Point Add(int x, int y)
            {
                return new Point(X + x, Y + y);
            }
            public override string ToString()
            {
                return string.Format("{0}{1}, {2}{3}", "{", X, Y, "}");
            }
            public static bool operator !=(Point a, Point b)
            {
                return a.X != b.X || a.Y != b.Y;
            }
            public static bool operator ==(Point a, Point b)
            {
                return a.X == b.X && a.Y == b.Y;
            }
        }
        public class Step
        {
            public Step(Point src, Point dst)
            {
                source = src;
                destination = dst;
            }

            public Point source, destination;
            public double estimation;
            public string Serialize()
            {
                return string.Format("{0} {1} {2} {3}", source.X, source.Y, destination.X, destination.Y);
            }
            public override string ToString()
            {
                return string.Format("{0} -> {1}", source, destination);
            }
        }
        public enum PlayerSide
        {
            PlayerA = 1,
            PlayerB = 2,
            PlayerC = 3,
            PlayerD = 4,
        }
        public enum TileState
        {
            None = 0,
            PlayerA = 1,
            PlayerB = 2,
            PlayerC = 3,
            PlayerD = 4,
        }
        public class GameState
        {
            private GameState()
            {
                Tiles = new TileState[10, 10];
            }

            public int StepNumber { get; set; }
            public TileState[,] Tiles { get; private set; }

            public TileState this[Point position]
            {
                get
                {
                    return Tiles[position.X, position.Y];
                }
                set
                {
                    Tiles[position.X, position.Y] = value;
                }
            }

            public static GameState Parse(string value)
            {
                var x = new GameState();
                string[] lns = value.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                x.StepNumber = int.Parse(lns[0]);
                string[][] tls = lns.Skip(1).Take(10).Select((_) => _.Split(' ')).ToArray();
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        x.Tiles[i, j] = (TileState)int.Parse(tls[j][i]);

                return x;
            }

            private IEnumerable<Point> EnumeratePositions()
            {
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 10; j++)
                        yield return new Point(i, j);
            }

            private IEnumerable<Step> EnumJumps(Point s, Point tl, int l, HashSet<Point> was)
            {
                l++;
                if (l > 50)
                    yield break;

                if (tl.X + 2 < 10 && this[tl.Add(2, 0)] == TileState.None && this[tl.Add(1, 0)] != TileState.None)
                {
                    Point p = tl.Add(2, 0);
                    if (!was.Contains(p))
                    {
                        was.Add(p);
                        yield return new Step(s, p);
                        foreach (var v in EnumJumps(s, p, l, was))
                            yield return v;
                    }
                }
                if (tl.Y + 2 < 10 && this[tl.Add(0, 2)] == TileState.None && this[tl.Add(0, 1)] != TileState.None)
                {
                    Point p = tl.Add(0, 2);
                    if (!was.Contains(p))
                    {
                        was.Add(p);
                        yield return new Step(s, p);
                        foreach (var v in EnumJumps(s, p, l, was))
                            yield return v;
                    }
                }

                if (tl.X - 2 >= 0 && this[tl.Add(-2, 0)] == TileState.None && this[tl.Add(-1, 0)] != TileState.None)
                {
                    Point p = tl.Add(-2, 0);
                    if (!was.Contains(p))
                    {
                        was.Add(p);
                        yield return new Step(s, p);
                        foreach (var v in EnumJumps(s, p, l, was))
                            yield return v;
                    }
                }
                if (tl.Y - 2 >= 0 && this[tl.Add(0, -2)] == TileState.None && this[tl.Add(0, -1)] != TileState.None)
                {
                    Point p = tl.Add(0, -2);
                    if (!was.Contains(p))
                    {
                        was.Add(p);
                        yield return new Step(s, p);
                        foreach (var v in EnumJumps(s, p, l, was))
                            yield return v;
                    }
                }
            }

            private IEnumerable<Point> EnumFriends(PlayerSide side)
            {
                return EnumeratePositions().Where((_) => (PlayerSide)this[_] == side);
            }

            public IEnumerable<Step> EnumerateSteps(PlayerSide side)
            {
                var t = EnumFriends(side);
                foreach (var tl in t)
                {
                    if (tl.X + 1 < 10 && this[tl.Add(1, 0)] == TileState.None)
                        yield return new Step(tl, tl.Add(1, 0));
                    if (tl.Y + 1 < 10 && this[tl.Add(0, 1)] == TileState.None)
                        yield return new Step(tl, tl.Add(0, 1));
                    //if (x.X + 1 < 10 && x.Y + 1 < 10 && this[x.Add(1, 1)] == TileState.None)
                    //    yield return new Step(x, x.Add(1, 1));

                    if (tl.X - 1 >= 0 && this[tl.Add(-1, 0)] == TileState.None)
                        yield return new Step(tl, tl.Add(-1, 0));
                    if (tl.Y - 1 >= 0 && this[tl.Add(0, -1)] == TileState.None)
                        yield return new Step(tl, tl.Add(0, -1));

                    foreach (var v in EnumJumps(tl, tl, 0, new HashSet<Point>()))
                        yield return v;
                    //if (x.X - 1 >= 0 && x.Y - 1 >= 0 && this[x.Add(-1, -1)] == TileState.None)
                    //    yield return new Step(x, x.Add(-1, -1));

                }
            }

            public GameState Clone()
            {
                GameState gs = new GameState();
                gs.StepNumber = StepNumber;
                gs.Tiles = (TileState[,])Tiles.Clone();
                return gs;
            }

            public void Append(Step s)
            {
                var x = this[s.source];
                this[s.source] = TileState.None;
                this[s.destination] = x;
            }

            public double Estimate(PlayerSide side)
            {
                double r = 0.0;

                double badness = 0.0;
                var fr = EnumFriends(side);
                foreach (var f in fr)
                {
                    badness += Math.Abs(f.X - 9) + Math.Abs(f.Y - 9);
                    if (f.X > 5 && f.Y > 5)
                        badness -= 5.0;
                }

                r -= badness;

                if (fr.Select((_) => _.X < 6 && _.Y < 6).Count() == 0)
                    r += 10000.0;

                return r;
            }

            private double EstimateDeep(PlayerSide side, Step s, int level)
            {
                level++;
                if (level > 3)
                    return 0.0;
                GameState gs = Clone();
                gs.Append(s);
                double r = gs.Estimate(side);
                double best = double.MinValue;
                foreach (var x in gs.EnumerateSteps(side).Distinct())
                {
                    x.estimation = gs.EstimateDeep(side, x, level);
                    if (x.estimation > best)
                        best = x.estimation;
                }
                return r + best / (2.0 * level);
            }

            public Step FindStep(PlayerSide side)
            {
                Step best = new Step(new Point(), new Point());
                best.estimation = double.MinValue;
                foreach (var x in EnumerateSteps(PlayerSide.PlayerA).Distinct())
                {
                    x.estimation = EstimateDeep(side, x, 0);
                    if (x.estimation > best.estimation)
                        best = x;
                }
                return best;
            }
        }

        private class StepEqualityComparer : IEqualityComparer<Step>
        {
            bool IEqualityComparer<Step>.Equals(Step x, Step y)
            {
                return x.destination == y.destination && x.source == y.source;
            }

            int IEqualityComparer<Step>.GetHashCode(Step obj)
            {
                return (obj.destination.GetHashCode() + obj.source.GetHashCode());
            }
        }

        static void Main(string[] args)
        {
            var st = GameState.Parse(File.ReadAllText("input.txt"));

            Step step = st.FindStep(PlayerSide.PlayerA);

            File.WriteAllText("output.txt", step.Serialize());
        }
    }
}
