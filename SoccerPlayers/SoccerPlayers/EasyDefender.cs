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
    public class EasyDefender
    {

        public class Point { public double x, y; }
        void Go()
        {
            int time = ReadInt(), scoreWe = ReadInt(), scoreEnemy = ReadInt();
            Point ball = new Point { x = ReadDouble(), y = ReadDouble() };
            Point ballFinish = new Point { x = ReadDouble(), y = ReadDouble() };
            List<Point> we = new List<Point>(), enemy = new List<Point>();
            for (int i = 0; i < 5; i++)
            {
                we.Add(new Point { x = ReadDouble(), y = ReadDouble() });
            }
            for (int i = 0; i < 5; i++)
            {
                enemy.Add(new Point { x = ReadDouble(), y = ReadDouble() });
            }
            var aim = Enumerable.Range(1, 5).Select(x => new Point()).ToList();

            Point ballAim = new Point();




            aim = we.Select(xx => new Point { x = xx.x, y = xx.y }).ToList();
            bool weHaveBall = we.Any(x => x.x == ball.x && x.y == ball.y);

            var rand = new Random();
            double yyy = rand.NextDouble() * 60-30;
            double xxx = Math.Sqrt(30 * 30 - yyy * yyy);
            ballAim = new Point { x = ball.x + xxx, y = ball.y + yyy };

            aim = enemy.Select(man => new Point { x = man.x - 5, y = man.y }).ToList();

            for (int i = 0; i < 5; i++)
            {
                if (Dist(we[i], ball) < 10)
                    aim[i] = ball;
            }
            

            aim.ForEach(x => Write(x.x, x.y));
            if (ballAim.x != 0)
            {
                Write(ballAim.x, ballAim.y);
            }

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
                new EasyDefender().Go();
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