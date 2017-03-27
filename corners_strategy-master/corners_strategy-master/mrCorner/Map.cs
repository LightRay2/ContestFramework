using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mrCorner
{
    class Map
    {
        private int[,] map;

        public int this[Point p]
        {
            get
            {
                return map[p.X, p.Y];
            }
            set
            {
                map[p.X, p.Y] = value;
            }
        }

        public Map()
        {
            map = new int[10, 10];
        }

        public void ReadInput()
        {
            string[] lines = File.ReadAllLines("input.txt").Skip(1).ToArray();

            for (int i = 0; i < 10; i++)
            {
                string[] rows = lines[i].Split(' ');

                for (int j = 0; j < 10; j++)
                {
                    map[i, j] = int.Parse(rows[j]);
                }
            }
        }

        public List<Action> PossibleActionsSide(int side)
        {
            List<Action> res = new List<Action>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (map[i, j] == 1)
                    {
                        Point p = Point.Get(i, j);

                        PossibleDestinationsPosHop(p)
                            .Select(d => new Action(p, d))
                            .ToList()
                            .ForEach(res.Add);

                        PossibleDestinationsPosMove(p)
                            .Select(d => new Action(p, d))
                            .ToList()
                            .ForEach(res.Add);
                    }
                }
            }

            return res;
        }

        public List<Point> PossibleDestinationsPosMove(Point p)
        {
            List<Point> targets = new List<Point>();

            if (p.Up.Exists && this[p.Up] == 0) targets.Add(p.Up);
            if (p.Left.Exists && this[p.Left] == 0) targets.Add(p.Left);
            if (p.Right.Exists && this[p.Right] == 0) targets.Add(p.Right);
            if (p.Down.Exists && this[p.Down] == 0) targets.Add(p.Down);

            return targets;
        }

        public List<Point> PossibleDestinationsPosHop(Point p)
        {
            List<Point> targets = new List<Point>();

            if (p.Up.Exists && this[p.Up] != 0 && p.Up.Up.Exists && this[p.Up.Up] == 0) targets.Add(p.Up.Up);
            if (p.Left.Exists && this[p.Left] != 0 && p.Left.Left.Exists && this[p.Left.Left] == 0) targets.Add(p.Left.Left);
            if (p.Right.Exists && this[p.Right] != 0 && p.Right.Right.Exists && this[p.Right.Right] == 0) targets.Add(p.Right.Right);
            if (p.Down.Exists && this[p.Down] != 0 && p.Down.Down.Exists && this[p.Down.Down] == 0) targets.Add(p.Down.Down);

            foreach (var target in targets.Select(t => t).ToList())
            {
                this[target] = this[p];
                this[p] = -1;

                targets.AddRange(PossibleDestinationsPosHop(target));

                this[p] = this[target];
                this[target] = 0;
            }

            return targets;
        }

        public int EstimateStateEuristic()
        {
            int score = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (map[i, j] == 1)
                    {
                        score += i + j;

                        if (i > 5 && j > 5)
                        {
                            score += 5;
                        }
                    }
                }
            }

            return score;
        }

        public void ApplyAction(Action action)
        {
            this[action.To] = this[action.From];
            this[action.From] = 0;
        }

        public void RevertAction(Action action)
        {
            this[action.From] = this[action.To];
            this[action.To] = 0;
        }
    }
}
