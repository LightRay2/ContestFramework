using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace smolin_contest
{
    class Program
    {

        static void Main(string[] args)
        {
            Input input = new Input();
            Process process = new Process(input);
            Output output = new Output(process.Bots, process.ToX, process.ToY);
        }
    }
}
