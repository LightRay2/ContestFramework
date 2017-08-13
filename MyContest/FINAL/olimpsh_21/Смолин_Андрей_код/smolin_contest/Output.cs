using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace smolin_contest
{
    class Output
    {
        public Output(List<Bot> bots, double toX, double toY, string memory = "")
        {
            using (StreamWriter writer = new StreamWriter(new FileStream("output.txt", FileMode.Create, FileAccess.Write)))
            {
                foreach (Bot bot in bots)
                    writer.WriteLine(String.Format("{0:0.##} {1:0.##}", bot.X, bot.Y));
                if (toX != -1)
                    writer.WriteLine(String.Format("{0:0.##} {1:0.##}", toX, toY));
                writer.WriteLine(memory);
            }
        }
    }
}
