using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DenisovFootballPowerfulStrategy
{
    public class Solver
    {
        private static bool IsFirst;
        void Solve()
        {
            string[] coords = ReadLines(12);
            string[] ballCoords = coords[1].Split(' ');
            Ball b = new Ball(new Points(double.Parse(ballCoords[0], CultureInfo.InvariantCulture),
                double.Parse(ballCoords[1], CultureInfo.InvariantCulture)),
                new Points(double.Parse(ballCoords[2], CultureInfo.InvariantCulture),
                double.Parse(ballCoords[3], CultureInfo.InvariantCulture)));
            Player[] pls = new Player[10];
            for (int i = 2; i < 12; i++)
            {
                string[] crd = coords[i].Split(' ');
                pls[i-2] = new Player(new Points(double.Parse(crd[0], CultureInfo.InvariantCulture),
                    double.Parse(crd[1], CultureInfo.InvariantCulture)), (i < 7) ? true : false);
            }

            /*if (int.Parse(coords[0].Substring(0, coords[0].IndexOf(' '))) % 2 != 0)
            {
                b.start.X = 100 - b.start.X;
                //b.start.Y = 100 - b.start.Y;
                b.dest.X = 100 - b.dest.X;
                //b.dest.Y = 100 - b.dest.Y;

                for (int i = 0; i < 10; i++)
                {
                    pls[i].current.X = 100 - pls[i].current.X;
                    //pls[i].current.Y = 100 - pls[i].current.Y;
                }
            }*/

            string[] res = Logic.Analize(pls, b);
            /*if (int.Parse(coords[0].Substring(0, coords[0].IndexOf(' '))) % 2 != 0)
            for (int i = 0; i < res.Length; i++)
            {
                string s = res[i];
                if (s.Length == 0)
                    continue;
                double intt = double.Parse(s.Substring(0, s.IndexOf(' ')));
                res[i] = "" + (100-intt) + res[i].Substring(res[i].IndexOf(' '));
            }*/
            WriteLines<string>(res);
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


            new Solver().Solve();

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
