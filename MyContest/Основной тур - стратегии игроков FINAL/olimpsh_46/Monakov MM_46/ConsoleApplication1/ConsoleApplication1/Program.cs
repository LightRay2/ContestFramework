using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;


namespace ConsoleApplication1
{
    class Program
    {
        static Point[] me = new Point[5];
        static Point[] opp = new Point[5];
        static Point ballStart = new Point();
        static Point ballEnd = new Point();
        static double[] dToMy = new double[5];
        static double distanceToGate = 50;
        static bool[] goodPassPossible = new bool[5];
        static double[] medistToBall = new double[5];
        static string str = "memory ";
        static double helpCos;
        static void readFromFile()
        {
            helpCos = Math.Sqrt(2) / 2;
            FileStream fs = new FileStream("input.txt", FileMode.Open);
            StreamReader rd = new StreamReader(fs);
            //прокидываем пока
            rd.ReadLine();
            //мяч 
            string[] str = rd.ReadLine().Split(' ');
            ballStart.x = double.Parse(str[0], CultureInfo.InvariantCulture);
            ballStart.y = double.Parse(str[1], CultureInfo.InvariantCulture);
            ballEnd.x = double.Parse(str[2], CultureInfo.InvariantCulture);
            ballEnd.y = double.Parse(str[3], CultureInfo.InvariantCulture);
            
            for(int i = 0; i < 5; i++)
            {
                str = rd.ReadLine().Split(' ');
                me[i] = new Point();
                me[i].x = double.Parse(str[0], CultureInfo.InvariantCulture);
                me[i].y = double.Parse(str[1], CultureInfo.InvariantCulture);
            }

            for (int i = 0; i < 5; i++)
            {
                str = rd.ReadLine().Split(' ');
                opp[i] = new Point();
                opp[i].x = double.Parse(str[0], CultureInfo.InvariantCulture);
                opp[i].y = double.Parse(str[1], CultureInfo.InvariantCulture);
            }

            //fix me 

            rd.Close();
        }

        static void WriteToFile()
        {
            FileStream file1 = new FileStream("output.txt", FileMode.Create); //создаем файловый поток
            StreamWriter writer = new StreamWriter(file1); //создаем «потоковый писатель» и связываем его с файловым потоком 

            for(int i = 0; i<5; i++)
            {
                writer.Write(me[i].x);
                writer.Write(' ');
                writer.WriteLine(me[i].y);
            }
            writer.Write(ballEnd.x);
            writer.Write(' ');
            writer.WriteLine(ballEnd.y);
            writer.WriteLine(str);
            writer.Close(); //закрываем поток. Не закрыв поток, в файл ничего не зап
        }

        static double countDistance(Point p1, Point p2)
        {
            double res =  Math.Sqrt(Math.Pow((p1.x - p2.x), 2) + Math.Pow((p1.y - p2.y), 2));
            return res;
        }

        static bool haveBall(out int who)
        {
            for (int i = 0; i < 5; i++)
            {
                 if (((me[i].x + 2 >= ballStart.x)&& (me[i].x - 2 < ballStart.x)) && ((me[i].y + 2 >= ballStart.y) && (me[i].y - 2 < ballStart.y)))
                {

                    who = i;
                    return true;
                }
            }
            who = - 1;
            return false;
        }

        static void countdistanceToMyTeam(int who)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i != who)
                {
                   dToMy[i] = countDistance(me[who], me[i]);
                } else
                {
                    dToMy[i] = 0;
                }
            }
        }


        static void countdistanceToBall()
        {
            for (int i = 0; i < 5; i++)
            {
                medistToBall[i] = countDistance(me[i], ballEnd);
            }
        }

        static double countDistanceToGoal()
        {
            distanceToGate = (100 - ballStart.x);
            return distanceToGate;
        }
        static bool upper()
        {
            return ballStart.y <= 30;
        }

        static void findgoodPass()
        {
            for (int i = 0; i < 5; i++)
            {
                if (dToMy[i] != 0)
                {
                    if (i < 28)
                    {
                        goodPassPossible[i] = true;
                    }
                    else
                    {
                        goodPassPossible[i] = false;
                    }
                }
            }
        }

        static bool wayFree(double y)
        {
            double up = y - 2;
            double bottom = y + 2;
            foreach( var i in opp)
            {
                if ((i.y <= up) && (i.y >= bottom))
                {
                    return false;
                }
            }
            return true;
        }

        static void finishGame()
        {
        
        }


        static void findRunners(out int[] runners,out int[] defenders)
        {
            runners = new int[3];
            defenders = new int[2];
            double[] tmp = new double[5];
            for (int i = 0; i < 5; i++)
            {
                tmp[i] = medistToBall[i];
            }
            Array.Sort(tmp);
            int k = 0;
            int l = 0;
            for(int i = 0; i<5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tmp[i] == medistToBall[j])
                    {
                        if (i > 2)
                        {
                            defenders[l] = j;
                            l++;
                        }
                        else
                        {
                            runners[k] = j;
                            k++;
                        }
                        break;
                    }
                }
            }
        }

        static void finishGameForRunners(int[] runners, int[] defenders)
        {
            Random rand = new Random();
            foreach(var r in runners)
            {
                me[r].x = ballEnd.x + rand.Next(-5, 5);
                me[r].y = ballEnd.y + rand.Next(-5, 5);
            }

           
             me[defenders[0]].x = 25;
            me[defenders[0]].y = 20;
             me[defenders[1]].x = 25;
            me[defenders[1]].y = 40;
        }
        
        static int findClost()
        {
            double a,tmp = 100;
            int clo = 0;
            for(int i = 0; i < 5; i++)
            { 
                    a = countDistance(me[i],ballStart);
                    if (a < tmp)
                    {
                        clo = i;
                         tmp = a;
                    }
            }
            if (tmp < 2) {
                return clo;
            } else
            {
                return -1;
            }
            
        } 

        static bool enemyClose(int toWho, ref double cos)
        {
            bool close = false;
            double tmp, costmp;
            for (int i = 0; i < 5; i++)
            {
                tmp = countDistance(me[toWho], opp[i]);
                if ( tmp <= 6)
                {
                    close = true;
                    if (me[toWho].x < opp[i].x) {
                        costmp = (opp[i].x - me[toWho].x) / tmp;
                        if ((costmp > cos)&&(costmp > helpCos) )
                        {
                            cos = costmp;
                        }
                    }  
                }   
            }
            return close;
        }

        static void Main(string[] args)
        {
            try
            {
                readFromFile();
                Random rand = new Random();
                int who = findClost();
                if (who != -1)
                {
                    //have ball strategy
                    for (int j = 0; j < 5; j++)
                    {
                        me[j].x += 10;
                    }
                    if (countDistanceToGoal() <= 30)
                    {
                        str += "dist < 30  ";
                        if (wayFree(ballStart.y))
                        {
                            str += "way was free ";
                            ballEnd.x = 130;
                            ballEnd.y = ballStart.y;
                            WriteToFile();
                        }
                        else
                        {
                            ballEnd.x += 30;
                            ballEnd.y += rand.Next(-30, 30);
                            WriteToFile();
                        }
                    }
                    else
                    {
                        if (wayFree(ballStart.y))
                        {
                            ballEnd.x += 100;
                        }
                        else
                        {
                            ballEnd.x += 100;
                            ballEnd.y += rand.Next(-30, 30);
                        }
                    }
                }
                else
                {
                    countdistanceToBall();
                    int[] runners = new int[3];
                    int[] defenders = new int[2];
                    findRunners(out runners, out defenders);
                    finishGameForRunners(runners, defenders);
                    ballEnd.x += 100;
                    ballEnd.y += rand.Next(-30, 30);
                    WriteToFile();
                }
            } catch (Exception e)
            {

            }
            
        }
    }
}
