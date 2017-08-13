using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace smolin_contest
{
    class Input
    {
        private char[] separator = new Char[] { ' ' };
        private const int LastLine = 12;

        public const double MaxX = 100;
        public const double MidY = 30;
        public const int MaxBots = 5;
        public const int MaxTicks = 300;

        public int Tick { get; private set; }
        public Ball Ball { get; private set; }
        public Player Me { get; private set; }
        public Player Enemy { get; private set; }
        public string Memory { get; private set; }

        public Input()
        {
            int count = 0;
            string[] line = new String[0];
            List<double> data = new List<double>();
            using (StreamReader reader = new StreamReader(new FileStream("input.txt", FileMode.Open, FileAccess.Read)))
            {
                while (count < LastLine + 1)
                {
                    if (count != LastLine)
                    {
                        line = reader.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        data = new List<double>();
                        foreach (string piece in line)
                            data.Add(Double.Parse(piece, CultureInfo.InvariantCulture));
                    }

                    switch (count)
                    {
                        case 0:
                            Tick = Int32.Parse(line[0]);
                            Me = new Player(Int32.Parse(line[1]));
                            Enemy = new Player(Int32.Parse(line[2]));
                            break;
                        case 1:
                            Ball = new Ball(data[0], data[1], data[2], data[3]);
                            break;
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                            Me.Bots.Add(new Bot(data[0], data[1], hasBall(data)));
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            Enemy.Bots.Add(new Bot(data[0], data[1], hasBall(data)));
                            break;
                        case LastLine:
                            Memory = reader.ReadLine();
                            break;
                    }
                    count++;
                }
            }
        }

        private bool hasBall(List<double> data)
        {
            return Ball.FromX == data[0] && Ball.FromY == data[1];
        }
    }
}
