using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharpTemplate
{
    public class SolverA
    {
        struct pair
        {
            public double x;
            public double y;
            public double xv;
            public double yv;

        }

        struct pairr
        {
            public double x;
            public double y;
        }
        struct bal
        {
            public double x;
            public double y;
            public int rol;
        }

        pairr[] topPos = new pairr[3];
        pairr[] botPos = new pairr[3];

        int ENgols;
        int memory;

        int golkeep;//у ворот
        int attaker1;//по всему полю
        int attaker2;//по всему полю
        int farer;//у чужих ворот
        int closer;// у своих ворот

        bool top;

        int clos(bal[] data, double x, double y, int[] whithout)
        {
            int result = -1;
            double min = 1000000;
            for (int i = 0; i < 5; i++)
            {
                int k = -1;
                for (int j = 0; j < 4; j++)
                    if (i == whithout[j])
                        k = j;
                if (k != -1 && i == whithout[k])
                    continue;
                double value = Math.Sqrt((data[i].x - x) * (data[i].x - x) + (data[i].y - y) * (data[i].y - y));
                if (min > value)
                {
                    min = value;
                    result = i;
                }
            }
            return result;
        }
        int clos(bal[] data, double x, double y)
        {
            int result = -1;
            double min = 1000000;
            for (int i = 0; i < 5; i++)
            {
                double value = Math.Sqrt((data[i].x - x) * (data[i].x - x) + (data[i].y - y) * (data[i].y - y));
                if (min > value)
                {
                    min = value;
                    result = i;
                }
            }
            return result;
        }
        bool isBallOur(int i, bal[] data, pair ball)
        {
            return (data[i].x == ball.x && data[i].y == ball.y);
        }
        bool isBallnear(int i, bal[] data, pair ball)
        {
            double d = Math.Sqrt((data[i].x - ball.x) * (data[i].x - ball.x) + (data[i].y - ball.y) * (data[i].y - ball.y));
            return (Math.Abs(d) <= 7 && Math.Abs(d) >= 0.1);
        }
        void goToPos(bal[] mine, bal[] wher, pair ball)//определяет куда бежать
        {
            int d;
            pairr[] poss;

            if (ENgols % 2 == 0)
            {
                poss = topPos;
                d = 5;
            }
            else
            {
                poss = botPos;
                d = -5;
            }

            int[] whithout = new int[4];
            whithout[0] = -1; whithout[1] = -1; whithout[2] = -1; whithout[3] = -1;

            //******************************************

            farer = clos(mine, poss[1].x, poss[1].y, whithout);
            whithout[0] = farer;
            wher[farer].x = poss[1].x;
            wher[farer].y = poss[1].y;
            if (isBallnear(farer, mine, ball))
            {
                wher[farer].x = ball.x;
                wher[farer].y = ball.y;
            }

            //******************************************

            closer = clos(mine, poss[2].x, poss[2].y, whithout);
            whithout[1] = closer;
            wher[closer].x = poss[2].x;
            wher[closer].y = poss[2].y;

            if (isBallnear(closer, mine, ball))
            {
                wher[closer].x = ball.x;
                wher[closer].y = ball.y;
            }

            //******************************************

            attaker1 = clos(mine, ball.x, ball.y, whithout);
            whithout[2] = attaker1;
            wher[attaker1].x = ball.x;
            wher[attaker1].y = ball.y;

            //******************************************

            golkeep = clos(mine, poss[0].x, poss[0].y, whithout);
            whithout[3] = golkeep;
            wher[golkeep].x = poss[0].x;
            wher[golkeep].y = ball.y;

            if (isBallnear(golkeep, mine, ball))
            {
                wher[golkeep].x = ball.x;
                wher[golkeep].y = ball.y;
            }

            //******************************************

            attaker2 = clos(mine, ball.x, ball.y, whithout);
            wher[attaker2].x = wher[attaker1].x;
            wher[attaker2].y = wher[attaker1].y + d;
        }

        void Solve()
        {
            //заполняем позиции
            topPos[0].x = 15; topPos[1].x = 71; topPos[2].x = 42;
            topPos[0].y = 30; topPos[1].y = 10; topPos[2].y = 10;

            botPos[0].x = 15; botPos[1].x = 71; botPos[2].x = 42;
            botPos[0].y = 30; botPos[1].y = 50; botPos[2].y = 50;

            int[,] beetveners = new int[5, 5];

            int tick = ReadInt();
            int MYgols = ReadInt();
            ENgols = ReadInt();

            bal[] mine = new bal[5];
            bal[] wher = new bal[6];
            bal[] enem = new bal[5];

            pair Ball;
            Ball.x = ReadDouble();
            Ball.y = ReadDouble();
            Ball.xv = ReadDouble();
            Ball.yv = ReadDouble();


            for (int i = 0; i < 5; i++)
            {
                mine[i].x = ReadDouble();
                mine[i].y = ReadDouble();
            }

            for (int i = 0; i < 5; i++)
            {
                enem[i].x = ReadDouble();
                enem[i].y = ReadDouble();
            }

            memory = ReadInt();
            pairr[] toPas = new pairr[5];
            goToPos(mine, wher, Ball);

            for (int i = 0; i < 5; i++)
            {
                //если мяч близко               


                //если мяч у нас 
                // if (isBallOur(i, mine, Ball))
                //{
                if (i == golkeep)//мяч у "вратаря"
                {
                    int[] whithout = new int[4];
                    whithout[0] = golkeep; whithout[1] = farer; whithout[2] = -1; whithout[3] = -1;
                    int k = clos(mine, mine[golkeep].x, mine[golkeep].y, whithout);
                    toPas[i].x = wher[k].x;
                    toPas[i].y = wher[k].y;
                    //wher[5].x = wher[attaker1].x;
                    //wher[5].y = wher[attaker1].y;
                }
                else if (i == closer)
                {
                    double K = Math.Sqrt((mine[farer].x - mine[closer].x) * (mine[farer].x - mine[closer].x) + (mine[farer].y - mine[closer].y) * (mine[farer].y - mine[closer].y));
                    if (K <= 30)
                    {
                        toPas[i].x = wher[farer].x;
                        toPas[i].y = wher[farer].y;
                    }
                    else
                        toPas[i].x = -1;


                }
                else if (i == farer)//удар по воротам
                {
                    if (mine[farer].x >= 70)//иначе не долетит
                    {
                        toPas[i].x = 100;
                        toPas[i].y = wher[farer].y;
                    }
                    else
                    {
                        toPas[i].x = -1;
                        toPas[i].y = wher[farer].y;
                    }

                }
                else if (i == attaker1)//
                {
                    toPas[i].x = wher[closer].x;
                    toPas[i].y = wher[closer].y;
                }
                else if (i == attaker2)//кто ближе farer или closer
                {
                    int[] whithout = new int[5];
                    whithout[0] = attaker1; whithout[1] = attaker2; whithout[2] = golkeep;
                    int k = clos(mine, mine[attaker2].x, mine[attaker2].y, whithout);
                    toPas[i].x = wher[k].x;
                    toPas[i].y = wher[k].y;
                }

                // }

            }
            for (int i = 0; i < 5; i++)
            {
                Write(wher[i].x, wher[i].y);
            }
            int t = clos(mine, Ball.x, Ball.y);
            if (toPas[t].x != -1)
                Write(toPas[t].x, toPas[t].y);
        }



        #region Main

        protected static TextReader reader;
        protected static TextWriter writer;
        public static void Run()
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

            new SolverA().Solve();
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