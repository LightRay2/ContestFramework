using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
namespace SoccerPlayers
{
    internal class Normal12
    {
        public Normal12()
        {
        }

        internal void GoPredefinedAttackAndSimpleDefence()
        {
            using(var reader = new StringReader(File.ReadAllText("input.txt")))
            {
                var erwerqwer = reader.ReadLine();
                var ss = reader.ReadLine().Split(' ');
                double ballx = double.Parse(ss[0], CultureInfo.InvariantCulture);
                double bally = double.Parse(ss[1], CultureInfo.InvariantCulture);

                bool weHaveBall = false;
                var xx = new List<double>();
                var yy = new List<double>();
                for(int i = 0; i < 10; i++)
                {
                    var s = reader.ReadLine().Split(' ') ;
                    xx.Add(double.Parse(s[0], CultureInfo.InvariantCulture));
                    yy.Add(double.Parse(s[1], CultureInfo.InvariantCulture));

                    if (i < 5 && xx.Last() == ballx && yy.Last() == bally)
                        weHaveBall = true;
                }

                if(weHaveBall == false)
                {
                    var output = new List<string>();
                    for(int i = 0; i < 5; i++)
                    {
                        output.Add(string.Format("{0} {1}", ballx, bally));
                    }
                    File.WriteAllLines("output.txt",output);
                }
                else
                {
                    if(bally < 5)
                    {
                        var output = new List<string>();
                        for (int i = 0; i < 5; i++)
                        {
                            output.Add(string.Format("{0} {1}", xx[i] + 10, 5));
                        }
                        output.Add(string.Format("{0} {1}", ballx + 100, 0));
                        File.WriteAllLines("output.txt",output);

                        
                    }
                    else
                    {
                        var output = new List<string>();
                        for (int i = 0; i < 5; i++)
                        {
                            output.Add(string.Format("{0} {1}", xx[i] , 0));
                        }
                        File.WriteAllLines("output.txt", output);
                    }
                }

            }
        }
    }
}