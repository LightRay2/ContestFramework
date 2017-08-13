using System;
using System.IO;


public class Random8x8
{
  public static void Main(string[] args) {
    Random rnd = new Random();
    int r = rnd.Next(8),
        c = rnd.Next(8);
    File.WriteAllText("output.txt", string.Format("{0} {1}{2}", r + 1, c + 1, Environment.NewLine));
  }
}
