using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OlferukPlaysFootball
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.SetupFrom("input.txt");
            game.MakeDecision();
            game.WriteDownDecision("output.txt");
        }
    }
}
