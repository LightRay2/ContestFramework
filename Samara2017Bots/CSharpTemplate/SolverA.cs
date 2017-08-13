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

        class Point
        {
            public double x, y;
            public double vectorx, vectory;
            public int hp;
            public int timeToGo;
        }
        double Dist(Point one, Point two) { return Math.Sqrt((one.x - two.x) * (one.x - two.x) + (one.y - two.y) * (one.y - two.y)); }
        void Solve()
        {
            //round number
            //player count 
            //player positions, player looks at, hp, time to go,      
            //wall count
            //wall center positions
            //shell count
            //shell positions, finishPositions

            var time = ReadInt();
            var players = Enumerable.Repeat(new Point(),ReadInt()).ToList();
            var our = players[0];
            players.ForEach(p => { p.x = ReadDouble(); p.y = ReadDouble(); p.vectorx = ReadDouble(); p.vectory = ReadDouble(); p.hp = ReadInt(); p.timeToGo = ReadInt(); });
            var walls = Enumerable.Repeat(new Point(), ReadInt()).ToList();
            walls.ForEach(w => { w.x = ReadDouble(); w.y = ReadDouble(); });
            var shells = Enumerable.Repeat(new Point(), ReadInt()).ToList();
            shells.ForEach(s => { s.x = ReadDouble(); s.y = ReadDouble(); s.vectorx = ReadDouble(); s.vectory = ReadDouble(); });

            if (time / 5 == 0)
                Write(0 , our.x, our.y - 100, 1);
            else if (time / 5 == 1)
                Write(1, our.x, our.y - 100, 1);
            else if (time / 5 == 2)
                Write(2, our.x, our.y - 100, 1);
            else if (time / 5 == 3)
                Write(3, our.x, our.y - 100, 1);
            else
                Write(0, 500, 375 , 1);

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