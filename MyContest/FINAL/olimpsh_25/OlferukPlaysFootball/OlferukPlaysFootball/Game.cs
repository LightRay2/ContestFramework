using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace OlferukPlaysFootball
{
    public class Game
    {
        public int Tick { get; set; }

        public int OurScore { get; set; }
        public int TheirScore { get; set; }

        public bool HasBall { get; set; }

        public Vector2 BallPosition { get; set; }
        public Vector2 BallDirection { get; set; }

        public List<Vector2> Players { get; set; }
        public List<Vector2> Foes { get; set; }

        public List<Vector2> NewPositions { get; set; }

        public string Memory { get; set; }

        public bool Attacking { get; set; }
        public Vector2 Attack { get; set; }

        private Random Random { get; set; }

        public Game()
        {
            Tick = 0;
            OurScore = 0;
            TheirScore = 0;

            Attacking = false;
            Attack = new Vector2();

            Random = new Random();

            BallDirection = new Vector2();
            BallPosition = new Vector2();

            Players = new List<Vector2>(5);
            Foes = new List<Vector2>(5);
            NewPositions = new List<Vector2>(5);

            Enumerable.Range(0, 5).ToList().ForEach(x => Players.Add(new Vector2()));
            Enumerable.Range(0, 5).ToList().ForEach(x => Foes.Add(new Vector2()));
            Enumerable.Range(0, 5).ToList().ForEach(x => NewPositions.Add(new Vector2()));
        }

        private void ParseHeader(string header)
        {
            var l = header.Split(' ').Select(x => Int32.Parse(x)).ToList();
            Tick = l[0];
            OurScore = l[1];
            TheirScore = l[2];
        }

        private void ParseBall(string ball)
        {
            var list = ball.Split(' ').Select(x => Double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            BallPosition.X = list[0];
            BallPosition.Y = list[1];

            BallDirection.X = list[2];
            BallDirection.Y = list[3];
        }

        private void ParsePlayer(List<Vector2> players, int index, string coords)
        {
            var list = coords.Split(' ').Select(x => Double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            players[index] = new Vector2(list[0], list[1]);
        }

        public void SetupFrom(string path)
        {
            using (StreamReader reader = new StreamReader("input.txt"))
            {
                ParseHeader(reader.ReadLine());
                ParseBall(reader.ReadLine());
                Enumerable.Range(0, 5).ToList().ForEach(index => ParsePlayer(Players, index, reader.ReadLine()));
                Enumerable.Range(0, 5).ToList().ForEach(index => ParsePlayer(Foes, index, reader.ReadLine()));
                Memory = reader.ReadLine();
            }

            HasBall = Players.Any(player => player.IsClose(BallPosition));
        }

        private bool NoEnemiesBetween(Vector2 a, Vector2 b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            double steps = 20;

            var DX = Enumerable.Range(0, 21).Select(step => dx/steps*step).ToList();
            var DY = Enumerable.Range(0, 21).Select(step => dy/steps*step).ToList();

            for (int i = 0; i < 5; ++i)
            {
                var foe = Foes[i];
                for (int k = 0; k <= 20; ++k) {
                    if (foe.Distance(new Vector2(a.X + DX[k], a.Y + DY[k])) < 5) {
                        return true;
                    }
                }
            }
            return false;
        }

        private int IndexOfClosest(Vector2 pos)
        {
            double min = 10000000;
            var min_i = -1;
            for (int i = 0; i < 5; ++i)
            {
                var d = Players[i].Distance(pos);
                if (d < min)
                {
                    min = d;
                    min_i = i;
                }
            }
            return min_i;
        }

        public double Average(IEnumerable<double> list)
        {
            double sum = 0;
            double cnt = 0;
            foreach (var x in list)
            {
                sum += x;
                cnt += 1;
            }
            return sum / cnt;
        }

        private Vector2 NextBuddy(Vector2 myPos)
        {
            var copy = new List<Vector2>(Players.Clone());
            foreach (var p in copy.OrderBy(x => -x.X).Where(x => x.X > myPos.X + 0.1))
            {
                if (p.IsClose(myPos))
                {
                    continue;
                }
                if (NoEnemiesBetween(p, myPos)) 
                {
                    return p;
                }
            }
            return null;
        }

        public void MakeDecision()
        {
            var copy = new List<Vector2>(Players.Clone());
            var bros = copy.OrderBy(x => x.Distance(BallPosition)).Select(IndexOfClosest).ToList();

            var closest = bros[0];
            var atk1 = bros[1];
            var atk2 = bros[2];
            var def1 = bros[3];
            var def2 = bros[4];

            if (!HasBall) // SEEK
            {
                NewPositions[closest] = new Vector2(BallPosition);

                NewPositions[atk1] = new Vector2(NewPositions[closest]);
                NewPositions[atk2] = new Vector2(NewPositions[closest]);

                NewPositions[def1] = new Vector2(25, 20);
                NewPositions[def2] = new Vector2(25, 45);
            }
            else // ATTACK
            {
                if (Players[closest].X <= 60)
                {
                    var next = NextBuddy(Players[closest]);
                    if (next != null)
                    {
                        Attacking = true;
                        Attack = new Vector2(next);
                    }
                }

                if (Players[closest].X > 83)
                {
                    var shift = Random.Next(15) - 8;
                    Attacking = true;
                    Attack = new Vector2(100, NewPositions[closest].Y + shift);
                }

                NewPositions[closest] = new Vector2(100, Players[closest].Y);

                NewPositions[atk1] = new Vector2(NewPositions[closest].AverageWith(new Vector2(100, Players[atk1].Y)));
                NewPositions[atk2] = new Vector2(NewPositions[closest].AverageWith(new Vector2(100, Players[atk2].Y)));

                NewPositions[def1] = new Vector2(50, Players[def1].Y);
                NewPositions[def2] = new Vector2(50, Players[def2].Y);
            }
        }

        public void WriteDownDecision(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                NewPositions.ForEach(x => writer.WriteLine(x.ToString()));
                if (Attacking)
                {
                    writer.WriteLine(Attack.ToString());
                }
            }
        }
    }
}
