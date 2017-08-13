using System;

public static class Extractor
{
	public Extractor()
	{

	}

    static void Extract(string path)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        StreamReader strtx = new StreamReader(path);
        List<string> inpline = UnparsedSplitLine(strtx);
        List<Point> ourteam = new List<Point>();
        List<Point> theirteam = new List<Point>();
        int roundnum = int.Parse(inpline[0]);
        int ourscore = int.Parse(inpline[1]);
        int thscore = int.Parse(inpline[2]);
        inpline = UnparsedSplitLine(strtx);
        Point ball = new Point(double.Parse(inpline[0]), double.Parse(inpline[1]));
        Point ballto = new Point(double.Parse(inpline[2]), double.Parse(inpline[3]));
        for (int i = 0; i < 5; i++)
        {
            inpline = UnparsedSplitLine(strtx);
            Point tm = new Point(double.Parse(inpline[0]), double.Parse(inpline[1]));
            ourteam.Add(tm);
        }
        for (int i = 0; i < 5; i++)
        {
            inpline = UnparsedSplitLine(strtx);
            Point tm = new Point(double.Parse(inpline[0]), double.Parse(inpline[1]));
            theirteam.Add(tm);
        }
        string comm = strtx.ReadLine();
        strtx.Close();
        Console.WriteLine(Convert.ToString(roundnum));
        Console.WriteLine(comm);
        Console.ReadKey();
    }
}
