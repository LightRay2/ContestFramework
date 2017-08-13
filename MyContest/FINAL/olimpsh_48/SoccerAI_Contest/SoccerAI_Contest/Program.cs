using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace SoccerAI_Contest
{
    class Program
    {        
        static void Main(string[] args)
        {
            FileStream aFile = new FileStream("input.txt", FileMode.Open);
            StreamReader f = new StreamReader(aFile);
            string s = f.ReadLine();
            String[] subs = s.Split(' ');
            double turn = double.Parse(subs[0], CultureInfo.InvariantCulture);
            double myScore = double.Parse(subs[1], CultureInfo.InvariantCulture);
            double opponentScore = double.Parse(subs[2], CultureInfo.InvariantCulture);
            s = f.ReadLine();
            subs = s.Split(' ');
            double ballX = double.Parse(subs[0], CultureInfo.InvariantCulture);
            double ballY = double.Parse(subs[1], CultureInfo.InvariantCulture);
            double dirX = double.Parse(subs[2], CultureInfo.InvariantCulture);
            double dirY = double.Parse(subs[3], CultureInfo.InvariantCulture);
            double[,] players = new double[10, 2];
            for (int i = 0; i < 10; i++)
            {
                s = f.ReadLine();
                subs = s.Split(' ');
                players[i, 0] = double.Parse(subs[0], CultureInfo.InvariantCulture);
                players[i, 1] = double.Parse(subs[1], CultureInfo.InvariantCulture);
            }
            string info = f.ReadLine();            
            f.Close();
            aFile.Close();
            FileStream aFile2 = new FileStream("output.txt", FileMode.Create);
            StreamWriter f2 = new StreamWriter(aFile2);
            if (enemyTeamHasTheBall(players, ballX, ballY))
            {
                f2.WriteLine((ballX-3).ToString() + ' ' + ballY.ToString());
                f2.WriteLine((ballX-3).ToString() + ' ' + ballY.ToString());
                f2.WriteLine((ballX-3).ToString() + ' ' + ballY.ToString());
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());                
            }
            else if (myTeamHasTheBall(players, ballX, ballY))
            {
                int index = findClosestAlly(players, ballX, ballY);
                if (index == -1)
                {
                    for (int i = 0; i < 5; i++)
                        f2.WriteLine((players[i, 0] + 5).ToString() + ' ' + players[i, 1].ToString());
                    if (ballX > 70)
                        f2.WriteLine("100 " + ballY.ToString());
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                        if (i == index)
                            f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                        else if (i == 0)
                            f2.WriteLine((ballX - 15).ToString() + ' ' + (ballY-10).ToString());                        
                        else
                            f2.WriteLine((players[i, 0] + 5).ToString() + ' ' + players[i, 1].ToString());
                    f2.WriteLine(players[index, 0].ToString() + ' ' + players[index, 1].ToString());
                }
            }
            else
            {
                f2.WriteLine((ballX - 3).ToString() + ' ' + ballY.ToString());
                f2.WriteLine((ballX - 3).ToString() + ' ' + ballY.ToString());
                f2.WriteLine((ballX - 3).ToString() + ' ' + ballY.ToString());
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());                
            }
            /*if (myTeamHasTheBall(players, ballX, ballY, info))
            {
                if (ballX < 50)
                {
                    f2.WriteLine("30 10");
                    f2.WriteLine("30 30");
                    f2.WriteLine("30 50");
                    f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                    f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                    if (ballY < 30)
                        f2.WriteLine((ballX + 100).ToString() + ' ' + (ballY - 100).ToString());                    
                    else
                        f2.WriteLine((ballX + 100).ToString() + ' ' + (ballY + 100).ToString());
                }
                else
                {
                    f2.WriteLine("50 10");
                    f2.WriteLine("50 30");
                    f2.WriteLine("50 50");
                    f2.WriteLine((players[3, 0] + 5).ToString() + ' ' + players[3, 1].ToString());
                    f2.WriteLine((players[4, 0] + 5).ToString() + ' ' + players[4, 1].ToString());
                    if (ballX > 70)
                        f2.WriteLine("100 " + ballY.ToString());
                }
                    
                f2.WriteLine("memory me");
            }

            else if (enemyTeamHasTheBall(players, ballX, ballY, info))
            {
                double pos;
                int index = findClosestDefender(players, ballX, ballY);
                for (int i = 0; i < 3; i++)
                    if (i == index)
                        f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                    else if ((i == 0) || (index == 0) && (i == 1))
                    {
                        pos = findDefPosition1(ballY);
                        double defX = (ballX < 20) ? 5 : (ballX - 15);
                        f2.WriteLine((defX.ToString() + ' ' + pos.ToString()));
                    }
                    else
                    {
                        pos = findDefPosition2(ballY);
                        double defX = (ballX < 20) ? 5 : (ballX - 15);
                        f2.WriteLine((defX.ToString() + ' ' + pos.ToString()));
                    }
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                f2.WriteLine("memory enemy");
            }

            else
            {                
                double pos;                
                int index = findClosestDefender(players, ballX, ballY);
                for (int i = 0; i < 3; i++)
                    if (i == index)
                        f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                    else if ((i == 0) || (index == 0) && (i == 1))
                    {
                        pos = findDefPosition1(ballY);
                        double defX = (ballX < 20) ? 5 : (ballX - 15);
                        f2.WriteLine((defX.ToString() + ' ' + pos.ToString()));
                    }
                    else
                    {
                        pos = findDefPosition2(ballY);
                        double defX = (ballX < 20) ? 5 : (ballX - 15);
                        f2.WriteLine((defX.ToString() + ' ' + pos.ToString()));
                    }
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());
                f2.WriteLine(ballX.ToString() + ' ' + ballY.ToString());                              
            }*/
            f2.Close();
            aFile2.Close();
        }

        static bool myTeamHasTheBall(double[,] players, double ballX, double ballY)
        {
            for (int i = 0; i < 5; i++)
                if ((Math.Abs(players[i, 0] - ballX) < 1) && (Math.Abs(players[i, 1] - ballY) < 1))
                    return true;
                return false;
        }
        static bool enemyTeamHasTheBall(double[,] players, double ballX, double ballY)
        {
            for (int i = 5; i < 10; i++)
                if ((Math.Abs(players[i, 0] - ballX) < 1) && (Math.Abs(players[i, 1] - ballY) < 1))
                    return true;
            return false;
        }
        static int findClosestAlly(double[,] players, double ballX, double ballY)
        {
            int index = -1;
            double min = 200;
            for (int i = 0; i < 5; i++)
            {
                double dist = Math.Sqrt(Math.Pow(players[i, 0] - ballX, 2) + Math.Pow(players[i, 1] - ballY, 2));
                if ((dist < min) && (dist > 3) && (players[i,0] > ballX))
                {
                    min = dist;
                    index = i;
                }
            }
            return index;
        }
        /*static bool ballOnMySide(double ballX)
        {
            if (ballX < 50)
                return true;
            return false;
        }
        static int findClosestDefender(double[,] players, double ballX, double ballY)
        {
            int index = -1;
            double min = 200;
            for (int i = 0; i < 3; i++)
            {
                double dist = Math.Sqrt(Math.Pow(players[i, 0] - ballX, 2) + Math.Pow(players[i, 1] - ballY, 2));
                if (dist < min)
                {
                    min = dist;
                    index = i;
                }
            }
            return index;
        }
        static double findDefPosition1(double ballY)
        {
            double pos = 20 + (ballY - 30) / 1.5;
            return pos;
        }

        static double findDefPosition2(double ballY)
        {
            double pos = 40 + (ballY - 30) / 1.5;
            return pos;
        }*/
    }
}
