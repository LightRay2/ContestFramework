using System;
using System.IO;


class Task0
{
  public const string
    InputFileName = "input.txt",
    OutputFileName = "output.txt";


  static void Main(string[] args)
  {
    StreamReader input = new StreamReader(InputFileName);
    StreamWriter output = new StreamWriter(OutputFileName);

    string[] parts = input.ReadLine().Split();
    int a = int.Parse(parts[0]),
        b = int.Parse(parts[1]);

    int c = a + b;

    output.WriteLine(c);

    input.Close();
    output.Close();
  }
}
