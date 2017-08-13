﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlshchnkLx_bot
{
    class Program
    {
        const uint countPlayersInTeam = 5;

        static int[] stat = new int[3];
        static double[,] myTeam = new double[countPlayersInTeam, 2];
        static double[,] nMyTeam = new double[countPlayersInTeam, 2];
        static double[,] ball = new double[3, 2];

        static ArrayList memory;

        static string addMemory = "memory ";

        static void SaveResult()
        {
            using (StreamWriter sw = new StreamWriter("output.txt", false, Encoding.Default))
            {
                for (int i = 0; i < 5; i++)
                {
                    sw.Write(myTeam[i, 0].ToString().Replace(",", "."));

                    sw.Write(" ");

                    sw.WriteLine(myTeam[i, 1].ToString().Replace(",", "."));
                }

                if (ball[2, 0] != 0 && ball[2, 1] != 0)
                {
                    sw.Write(ball[2, 0].ToString().Replace(",", "."));

                    sw.Write(" ");

                    sw.WriteLine(ball[2, 1].ToString().Replace(",", "."));
                }


                sw.Write(addMemory);
            }
        }

        static double Length(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }

        static int BelongingBall(int ballID)
        {
            double my = 9999;
            double nMy = 9999;

            for (int i = 0; i < 5; i++)
            {
                double temp1 = Length(myTeam[i, 0], myTeam[i, 1], ball[ballID, 0], ball[ballID, 1]);
                double temp2 = Length(nMyTeam[i, 0], nMyTeam[i, 1], ball[ballID, 0], ball[ballID, 1]);

                if (temp1 < my)
                    my = temp1;

                if (temp2 < nMy)
                    nMy = temp2;
            }

            double eps = 1;
            if (my > eps && nMy > eps)
                return 0;
            else
            {
                if (my < nMy)
                    return 1;
                else
                    return -1;
            }
        }

        static int BallRegionTeam()
        {
            double x = (ball[0, 0] + ball[1, 0]) / 2.0;

            if (x < 40)
                return 1;
            if (x > 60)
                return -1;

            return 0;
        }

        static void RealEndPosBall()
        {
            try
            {
                double currentLength = Length(ball[0, 0], ball[0, 1], ball[1, 0], ball[1, 1]);
                if (currentLength > 30)
                {
                    double x = ((ball[1, 0] - ball[0, 0]) / currentLength) * 30 + ball[0, 0];
                    double y = ((ball[1, 1] - ball[0, 1]) / currentLength) * 30 + ball[0, 1];

                    ball[1, 0] = x;
                    ball[1, 1] = y;
                }
            }catch
            {
                double currentLength = Length(ball[0, 0], ball[0, 1], ball[1, 0], ball[1, 1]);
                if (currentLength > 30)
                {
                    double x = ((ball[1, 0] - ball[0, 0]) / currentLength) * 30 + ball[0, 0];
                    double y = ((ball[1, 1] - ball[0, 1]) / currentLength) * 30 + ball[0, 1];

                    ball[1, 0] = x;
                    ball[1, 1] = y;
                }
            }
        }

        static bool ClearTeamLine(int P1, double XX, double YY)
        {
            try
            {
                double length = Length(myTeam[P1, 0], myTeam[P1, 1], XX, YY);
                double x = (XX - myTeam[P1, 0]) / length;
                double y = (YY - myTeam[P1, 1]) / length;

                for (double s = 1.5; s < length; s++)
                {
                    double X = myTeam[P1, 0] + x * s;
                    double Y = myTeam[P1, 1] + y * s;

                    for (int i = 0; i < 5; i++)
                    {
                        if (Length(X, Y, nMyTeam[i, 0], nMyTeam[i, 1]) < 2)
                        {
                            return false;
                        }
                    }
                }

                
            }catch
            {

            }
            return true;
        }

        static int PlayersBall()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Length(myTeam[i, 0], myTeam[i, 1], ball[0, 0], ball[0, 1]) < 10)
                        return i;
                }
                
            }catch
            {

            }
            return -1;
        }
        static void JustGetTheBall()
        {
            try
            {
                double[] LengthPlayerBall0 = new double[5];

                for (int i = 0; i < 5; i++)
                {
                    LengthPlayerBall0[i] = Length(myTeam[i, 0], myTeam[i, 1], ball[0, 0], ball[0, 1]);
                }

                double[] LengthPlayerBall1 = new double[5];

                for (int i = 0; i < 5; i++)
                {
                    LengthPlayerBall1[i] = Length(myTeam[i, 0], myTeam[i, 1], ball[1, 0], ball[1, 1]);
                }

                for (int i = 0; i < 5; i++)
                {
                    if (LengthPlayerBall0[i] < LengthPlayerBall1[i])
                    {
                        myTeam[i, 0] = ball[0, 0];
                        myTeam[i, 1] = ball[0, 1];
                    }
                    else
                    {
                        myTeam[i, 0] = ball[1, 0];
                        myTeam[i, 1] = ball[1, 1];
                    }
                }
            }
            catch
            {

            }
        }

        static void KickBallToOpponent()
        {
            try
            {
                int temp1 = PlayersBall();

                if (temp1 != -1)
                {

                    if (myTeam[temp1, 0] >= 75)
                    {
                        for (int i = 0; i <= 30; i++)
                        {
                            if (ClearTeamLine(temp1, 100, 30 - i))
                            {
                                ball[2, 0] = 100;
                                ball[2, 1] = 30 - i;
                                return;
                            }
                            if (ClearTeamLine(temp1, 100, 30 + i))
                            {
                                ball[2, 0] = 100;
                                ball[2, 1] = 30 + i;
                                return;
                            }
                        }
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        if (temp1 != i)
                        {
                            if (myTeam[i, 0] > myTeam[temp1, 0])
                            {
                                if (ClearTeamLine(temp1, myTeam[i, 0], myTeam[i, 1]))
                                {
                                    ball[2, 0] = myTeam[i, 0];
                                    ball[2, 1] = myTeam[i, 1];
                                    return;
                                }
                            }
                        }
                    }
                }

                Random R = new Random();


                ball[2, 0] = 100;
                ball[2, 1] = R.NextDouble() * 60;
            }catch
            {

            }

        }

        static void BehaviorPlayers()
        {
            try
            {
                Random R = new Random();

                for (int i = 0; i < 2; i++)
                {
                    myTeam[i, 0] = 25 + R.NextDouble() * 20 - 10;

                    myTeam[i, 1] = i * 30 + 15 + R.NextDouble() * 10 - 5;
                }
                for (int i = 2; i < 4; i++)
                {
                    myTeam[i, 0] += 3;

                }

                myTeam[4, 0] = 50 + R.NextDouble() * 20 - 10;
                myTeam[4, 1] = 30 + R.NextDouble() * 10 - 5;
            }
            catch
            {

            }
        }
        static void StrategyManager()
        {
            try
            {
                RealEndPosBall();
                int ballRegion = BallRegionTeam();
                int[] belBall = new[] { BelongingBall(0), BelongingBall(1) };

                if (ballRegion == -1)
                {
                    if (belBall[0] != 1)
                    {
                        JustGetTheBall();
                    }
                    else
                    {
                        BehaviorPlayers();
                    }

                    KickBallToOpponent();
                }
                else
                {
                    if (belBall[0] != 1)
                    {
                        JustGetTheBall();
                    }
                    else
                    {
                        BehaviorPlayers();
                    }

                    KickBallToOpponent();
                }
            }
            catch
            {

            }
        }

        static void Main(string[] args)
        {
            try
            {
                FileSystem.FileTable file = new FileSystem.FileTable();

                file.Path = "input.txt";
                file.load_file(' ');

                for (int i = 0; i < file.GetElemCount; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            stat[j] = Convert.ToInt32(file.get_data_from_table(i, j));
                        }
                    }

                    if (i == 1)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            ball[0, j] = Convert.ToDouble(file.get_data_from_table(i, j).Replace(".", ","));
                        }

                        for (int j = 0; j < 2; j++)
                        {
                            ball[1, j] = Convert.ToDouble(file.get_data_from_table(i, j + 2).Replace(".", ","));
                        }
                    }

                    if (2 <= i && i <= 6)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            myTeam[i - 2, j] = Convert.ToDouble(file.get_data_from_table(i, j).Replace(".", ","));
                        }
                    }

                    if (7 <= i && i <= 11)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            nMyTeam[i - 7, j] = Convert.ToDouble(file.get_data_from_table(i, j).Replace(".", ","));
                        }
                    }

                    if (i == 12)
                    {
                        memory = file.get_data_from_table(i);
                    }
                }

                StrategyManager();
                SaveResult();
            }
            catch
            {

            }
           
        }
    }
}
