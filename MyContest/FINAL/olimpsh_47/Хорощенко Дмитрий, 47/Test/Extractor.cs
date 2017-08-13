using System;
using System.Threading;
using System.IO;
using System.Globalization;
//using Test.Program;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public static class TextFileWork
    {
        /*public Extractor()
        {

        }
        */
        static public void Input(string path, string InputLine)
        {
            StreamWriter strin = new StreamWriter(path);
            strin.WriteLine(InputLine);
            strin.Close();
        }

        static public void Extract(string path, ref MatchInfo mi)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            StreamReader strtx = new StreamReader(path);
            List<string> inpline = Program.UnparsedSplitLine(strtx);
            mi.ourteam = new List<Point>();
            mi.theirteam = new List<Point>();
            mi.roundnum = int.Parse(inpline[0]);
            mi.ourscore = int.Parse(inpline[1]);
            mi.theirscore = int.Parse(inpline[2]);
            inpline = Program.UnparsedSplitLine(strtx);
            mi.ballpos = new Point(double.Parse(inpline[0]), double.Parse(inpline[1]));
            mi.balldir = new Point(double.Parse(inpline[2]), double.Parse(inpline[3]));
            for (int i = 0; i < 5; i++)
            {
                inpline = Program.UnparsedSplitLine(strtx);
                Point tm = new Point(double.Parse(inpline[0]), double.Parse(inpline[1]));
                mi.ourteam.Add(tm);
            }
            for (int i = 0; i < 5; i++)
            {
                inpline = Program.UnparsedSplitLine(strtx);
                Point tm = new Point(double.Parse(inpline[0]), double.Parse(inpline[1]));
                mi.theirteam.Add(tm);
            }
            mi.comm = strtx.ReadLine();
            strtx.Close();
            /*Console.WriteLine(Convert.ToString(mi.roundnum));
            Console.WriteLine(mi.comm);
            Console.ReadKey();*/
        }
    }
}