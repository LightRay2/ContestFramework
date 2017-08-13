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

        void Solve()
        {
            //code here. use Read...() and Write(...,...,...)
            double n = ReadDouble();
            double g1 = ReadDouble();
            double g2 = ReadDouble();
            double mx1 = ReadDouble();
            double my1 = ReadDouble();
            double mx2 = ReadDouble();
            double my2 = ReadDouble();
            double[,] a = new double[10, 2];
            for(int i = 0;i<10;i++)
                for (int j = 0; j < 2; j++)
                    a[i,j]= ReadDouble();
            string st = ReadToken();
            //if((mx2>=0)&&(my2>=0))
            double min = 1000;
           int mini = -1;
            for (int i = 0; i < 5; i++)
                if(Math.Sqrt((a[i, 0]- mx2) * (a[i, 0]- mx2 )+ (a[i, 1] - my2) * (a[i, 1] - my2))<min)
                {
                    min =Math.Round( Math.Sqrt((a[i, 0] - mx2) * (a[i, 0] - mx2) + (a[i, 1] - my2) * (a[i, 1] - my2)),3);
                    mini = i;
                }
            int r = 0;
            int l = 0;
            for (int i = 5; i < 10; i++)
                if ((my2 > a[i, 1]) && (a[i, 0] - mx2 >= 0))
                {
                    r++;
                }
                else
                    if ((my2 < a[i, 1]) && (a[i, 0] - mx2 >= 0))
                    l++;
            int k = 0;
            //int d = Convert.ToInt32(st);
            //if ((Math.Abs(r - l) <= 1)&&(st!="-1"))
            //    k = d;
            //else
            if (r >= l)
                k = 5;
            else
                k = -5;
            //st = k.ToString();
             double min2 = 1000;
            int mini2 = -1;
            for (int i = 1; i < 5; i++)
                if( (Math.Sqrt((a[i, 0] - mx2) * (a[i, 0] - mx2) + (a[i, 1] - my2) * (a[i, 1] - my2)) < min2)&&(mini!=i))
                {
                    min2 = Math.Round(Math.Sqrt((a[i, 0] - mx2) * (a[i, 0] - mx2) + (a[i, 1] - my2) * (a[i, 1] - my2)), 3);
                    mini2 = i;
                }
            double maxx = 0;
            int maxxi = 0;
            for (int i = 1; i < 5; i++)
            {
                if ((i != mini) && (i != mini2))
                    if (maxx < a[i, 0])
                    {
                        maxx = a[i, 0];
                        maxxi = i;
                    }
            }

            if(mini!=0)
            Write(5, my2);
            else
                Write(mx2, my2);
            for (int i = 1; i < 5; i++)
                if ((i == mini) || (i == mini2))
                    Write(mx2, my2);
                else
           if (i == maxxi)
                    if (maxx < 70) 
                        Write(a[i, 0] + 2.5, a[i, 1]);
                    else
                    {
                        Write(a[i, 0] - 2.5, a[i, 1]);
                    }
                else
              if (a[0, 1]> a[0, 1])
                {
                    Write(10, 20);
                }
                else
                {
                    Write(10, 40);
                }
            bool ok = true;
            double cit = 0;
            int citi = -1;
            if(a[mini,0]<70)
            for (int i = 1;i<5;i++)
            {
                if(i!=mini)
                {
                   if(( Math.Sqrt((a[i, 0] - mx2) * (a[i, 0] - mx2) + (a[i, 1] - my2) * (a[i, 1] - my2))<25)&&(a[i, 0] - mx2>cit))
                    {
                        int y = (int)my2;
                        bool f=true;
                        for (int x = (int)mx2; x< a[i, 0]&& y < a[i, 0] ;x++)
                        {
                            for(int j=5;j<10;j++)
                            {
                                if(((int)a[j,0]==x)&& ((int)a[j, 1] == y))
                                {
                                    f = false;
                                    break;
                                }
                            }
                            if (f == false)
                                break;
                        }
                        if (f)
                        {
                            cit = a[i, 0] - mx2;
                            citi = i;
                        }
                    }
                }
            }
            if(citi!=-1)
            {
                Write(a[citi, 0], a[citi, 1]);
                ok = false;
            }
            if(ok)
            if (mx2 <= 71)
            {
                if ((maxx > mx2+5) && (maxx > 50)&& 
                    (Math.Sqrt((maxx - mx2) * (maxx - mx2) + (a[maxxi, 1] - my2) * (a[maxxi, 1] - my2)) < 35))
                        Write(maxx, a[maxxi, 1]);
                else
                {
                    if (mini != 0)
                    {
                        if ((my2 + k > 0) && (my2 + k < 60) && (mx2 + 5 > 0))
                            Write(mx2 + 5, my2 + k);
                        else
                            if (mx2 + 5 > 0)
                            Write(mx2 + 5, my2);
                        else
                            Write(a[mini, 0] + 5, my2);
                    }
                    else
                        Write(a[mini2, 0] , a[mini2, 1]);
                }
            }
            else
                Write(mx2 + 30, my2);
            st = "memory " + st;
            Write(st);
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