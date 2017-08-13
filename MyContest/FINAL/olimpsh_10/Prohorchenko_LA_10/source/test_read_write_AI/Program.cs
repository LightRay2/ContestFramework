using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using System.Globalization;

class Test
{
    public static void Main()
    {
        try
        {
            #region Read
            DPoint[] playersFriendly = new DPoint[5];//me
            DPoint[] playersEnemy = new DPoint[5];  //enemy
            DPoint[] ball = new DPoint[3];  //where -> where to go
            string message = "";
            ReadFromFile(out playersFriendly, out playersEnemy, out ball, out message);
            #endregion
            //int n = 0;    //ball 0 == ball X


            DPoint[] playerGoTo = new DPoint[5];
            for (int i = 0; i < playerGoTo.Length; i++)
                playerGoTo[i] = playersFriendly[i];

            //who have the freaking ball
            int whoHaveABall = 0;

            //punch and other
            bool want_to_punch = false;
            DPoint where_to_punch = new DPoint(50, 30);

            for (int i = 0; i < playersFriendly.Length; i++)
            {
                if (playersFriendly[i].X == ball[0].X && playersFriendly[i].Y == ball[0].Y) whoHaveABall = 1;
                if (playersEnemy[i].X == ball[0].X && playersEnemy[i].Y == ball[0].Y) whoHaveABall = -1;
            }

            //1st
            #region AI



            if (ball[0].X <= 60 || ball[1].X <= 70)
            {
                #region Defensive

                //for (int i = 0; i < playersFriendly.Length; i++)
                if (whoHaveABall <= 0)
                {
                    //we do not have a ball on out side
                    if (ball[1].X < 37)
                        playerGoTo = gotoClosest(new DPoint[] { ball[0].Add(-10,0), ball[1],
                            new DPoint(Math.Max(5,ball[1].X-6), ball[1].Y/2 + ball[0].Y/2), 
                            new DPoint(Math.Max(20,ball[1].X+2), ball[1].Y ),
                            new DPoint(Math.Max(40,ball[0].X), ball[0].Y) }, playersFriendly);
                    else
                        playerGoTo = gotoClosest(new DPoint[] { ball[0], ball[0].Add(10,0),
                            new DPoint(Math.Max(10,ball[1].X), ball[1].Y/2 + ball[0].Y/2), new DPoint(ball[1].X, ball[1].Y),
                            new DPoint(ball[1].X/2+ball[0].X/2, ball[0].Y + 3) }, playersFriendly);

                    //break;
                }
                else
                {
                    playerGoTo = gotoClosest(new DPoint[] {
                        ball[0],
                        ball[0].Add(8,7),new DPoint(20,ball[0].Y/2+ball[1].Y/2),
                        new DPoint(ball[0].X+10, ball[0].Y/2 + ball[1].Y/2),
                        new DPoint(ball[1].X, ball[0].Y/2 + ball[1].Y/2),
                    }, playersFriendly);

                    //someone who carry a ball should go further
                    Random rnd = new Random();
                    for (int i = 0; i < playersFriendly.Length; i++)
                        if (ball[0].X == playersFriendly[i].X && ball[0].Y == playersFriendly[i].Y)
                        { playerGoTo[i] = playersFriendly[i].Add(30, rnd.Next(-5, 5)); }

                    //we have a ball on our side    --> vipinat ego nafig ottuda
                    if (CountEnemyes(ball[0], 15, playersEnemy) > 0) want_to_punch = true;

                    int pass_to = ClosestRighter(ball[0], playersFriendly);
                    if (pass_to < 0)
                    {
                        //не кому пнуть мяч
                        if (CountEnemyes(ball[0], 10, playersEnemy) > 2)
                        {
                            Random rnd2 = new Random(DateTime.Now.Millisecond);
                            where_to_punch = ball[0].Add(30, 100 * (rnd2.Next(0, 2) * 2 - 1));
                        }
                        else
                            where_to_punch = new DPoint(1000, ball[0].Y - 30);
                    }
                    else
                        where_to_punch = playersFriendly[pass_to].Add(35, rnd.Next(-4, 5));
                }


                #endregion
            }
            else
            {
                #region Offensive

                //if (whoHaveABall > 0)
                //{
                playerGoTo = gotoClosest(new DPoint[] {
                        new DPoint(Math.Max(70,ball[1].X-30),ball[0].Y/2+ball[1].Y/2),  //def
                        ball[0]
                        ,ball[1].Add(10,15), ball[1].Add(10,-15), ball[0].Add(-9,0)
                    }, playersFriendly);

                int leadplayer = -1;
                for (int i = 0; i < playersFriendly.Length; i++)
                    if (ball[0].X == playersFriendly[i].X && ball[0].Y == playersFriendly[i].Y)
                        leadplayer = i;
                //a player mate who carry a ball
                Random rnd = new Random(DateTime.Now.Millisecond);
                if (CountEnemyes(ball[0], 20, playersEnemy) < 1)//никого вокруг нет рядом
                {
                    if (leadplayer > -1)
                    {
                        if (CountEnemyes(ball[0], 20, 9, playersEnemy) < 1)//никого нет впереди --> просто ведешь мяч вперед
                            playerGoTo[leadplayer] = ball[0].Add(15, 0);
                        else//вести мяч с обмоткой
                            playerGoTo[leadplayer] = ball[0].Add(10, 0.5 * (rnd.Next(2) * 2 - 1) * (9 + rnd.Next(10)));
                    }
                    else
                    {
                        for (int i = 0; i < playersFriendly.Length; i++)
                            playerGoTo[i].Add(10, (playerGoTo[i].Y - 30) / 3);   //masive attack to the front and rassip
                    }
                }
                else
                {

                    if (CountEnemyes(ball[0], 50, 8, playersEnemy) < 1)
                    { if (rnd.Next(0, 100) < 20)want_to_punch = true; where_to_punch = ball[0].Add(100, 80); }//только вперед
                    else
                    {

                        want_to_punch = true;//придется бить
                        if (CountEnemyes(ball[0], 10, playersEnemy) > 1)//бить вперед и при этом в бок чтобы кучу противников еморализовать
                            where_to_punch = ball[0].Add(10, 20 * (rnd.Next(2) * 2 - 1));
                        if (CountEnemyes(ball[0], 10, playersEnemy) > 2)//немного отдать мяч назад или как пойдет
                            where_to_punch = playersFriendly[ClosestLeft(ball[0], playersFriendly)].Add(5, 0);
                    }
                }
                //}


                #endregion
                //}
            }
            //КОСТЫЛИИИ
            int lead = -1;
            for (int i = 0; i < playersFriendly.Length; i++)
                if (ball[0].X == playersFriendly[i].X && ball[0].Y == playersFriendly[i].Y)
                    lead = i;
            if (lead >= 0 && whoHaveABall > 0)
                want_to_punch = true;
            //надо было пасавать а он ничего не сделал
            //{ 
            if (lead >= 0)
            { want_to_punch = true; where_to_punch = playersFriendly[nextToYou(playersFriendly[lead], playersFriendly)].Add(5, 0); }
            //}
            if (ball[0].X > 90 || ball[1].X > 90) { playerGoTo[Righter(playersFriendly)] = ball[1]; want_to_punch = true; where_to_punch = ball[0].Add(1000, 1000); }

            #endregion

            //2nd
            #region AI new
            /*
        double[] summ = new double[] { 0, 0 };
        for (int i = 0; i < playersEnemy.Length; i++)
        { summ[0] += playersEnemy[i].X; summ[1] += playersEnemy[i].Y; }


        ball[2] = new DPoint(summ[0]/5, summ[1]/5);
        ball[2] = new DPoint(ball[2].X / 2 + ball[1].X / 2, ball[2].Y / 2 + ball[1].Y/ 2);
        if (whoHaveABall == 0)
        {
            if (ball[0].X > ball[1].X)  //fly from us
            {
                playerGoTo = gotoClosest(new DPoint[] { 
                                            ball[1],ball[1].Add(10,10),
                                            ball[1].Add(-10,10), ball[1].Add(-10,-10),
                                            ball[1].Add(10,-10)
                                            }, playersFriendly);
            }
            else//fly to us
            {
                playerGoTo = gotoClosest(new DPoint[] { 
                                            ball[2],ball[0].Add(20,20),
                                            ball[2].Add(-20,-20),
                                            ball[2].Add(20,-20),
                                            ball[2].Add(-20,20)
                                            }, playersFriendly);
            }
        }
        if (whoHaveABall == 1)
        {

        }
        if (whoHaveABall == -1)
        {
            playerGoTo = gotoClosest(new DPoint[] { 
                                            ball[2].Add(-10,0),ball[2].Add(-5,5),
                                            ball[2].Add(-9,9), ball[2].Add(-9,-9),
                                            ball[2].Add(10,0)
                                            }, playersFriendly);
        }
        */
            #endregion
            #region Write
            List<String> res = new List<string>();
            for (int i = 0; i < playerGoTo.Length; i++)
                res.Add(playerGoTo[i].ToString() + "");

            if (want_to_punch)
            {
                if ( (new Random()).Next(0, 100) < 40)
                //{
                    res.Add(/*where_to_punch*/whereToPass(ball[0], playersFriendly, playersEnemy).ToString());
                //}
                //else
                res.Add(where_to_punch.ToString());
            }

            Write(res);
            #endregion

        }
        catch (Exception e)
        {
            //
            Console.Write("something went wrong");
        }
    }


    #region try1 functions
    /// <summary>
    /// Возвращает индекс самого близкоого к точке игрока
    /// </summary>
    /// <param name="to">Точка</param>
    /// <param name="all">Игроки</param>
    /// <returns>Индекс в посланном массиве</returns>
    private static int closest(DPoint to, params DPoint[] all)
    {
        double[] dist = new double[all.Length];
        for (int i = 0; i < all.Length; i++)
        { dist[i] = Math.Sqrt(Math.Pow(all[i].X - to.X, 2) + Math.Pow(all[i].Y - to.Y, 2)); }

        double min_dist = 200; int min_index = -1;
        for (int i = 0; i < all.Length; i++)
            if (dist[i] < min_dist) { min_dist = dist[i]; min_index = i; }
        return min_index;
    }

    /// <summary>
    /// Распихивает игроков по точкам по принципу - кому куда быстрее
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    private static DPoint[] gotoClosest(DPoint[] to, DPoint[] from)
    {
        DPoint[] res = new DPoint[to.Length];
        DPoint[] tosorted = new DPoint[to.Length];

        for (int i = 0; i < to.Length; i++)
        {
            DPoint max = new DPoint(0, 0); int ind_max = 0;
            for (int j = i; j < to.Length; j++) if (max.X < to[j].X) { ind_max = j; max = to[j]; }

            tosorted[tosorted.Length - i - 1] = to[ind_max];
            DPoint temp = to[ind_max];
            to[ind_max] = to[i];
            to[i] = temp;
        }


        for (int i = 0; i < to.Length; i++)
        {
            int lucker = closest(to[i], from);    //the slosest to point n.I
            if (lucker == -1) continue;
            res[lucker] = to[i];
            from[lucker].X = 1000;  //distant it from all other
        }

        return res;
    }


    /// <summary>
    /// the most right player on a field
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    private static int Righter(params DPoint[] who)
    {
        double maxX = 0; int max_ind = 0;
        for (int i = 0; i < who.Length; i++)
            if (maxX < who[i].X) { maxX = who[i].X; max_ind = i; }

        return max_ind;
    }


    private static int ClosestRighter(DPoint from, params DPoint[] who)
    {
        double minimal = 100; int max_ind = 0;
        for (int i = 0; i < who.Length; i++)
            if (who[i].X > from.X + 10 && who[i].X > 5 && who[i].X - from.X < minimal) { minimal = who[i].X - from.X; max_ind = i; }

        return max_ind;
    }


    private static int ClosestLeft(DPoint from, params DPoint[] who)
    {
        double max = 0; int max_ind = 0;
        for (int i = 0; i < who.Length; i++)
            if (who[i].X > max && who[i].X <= from.X) { max = who[i].X; max_ind = i; }

        return max_ind;
    }

    private static int CountEnemyes(DPoint where, double rad, DPoint[] enemyes)
    {
        int res = 0;
        for (int i = 0; i < enemyes.Length; i++)
            if (Math.Sqrt(Math.Pow(enemyes[i].X - where.X, 2) + Math.Pow(enemyes[i].Y - where.Y, 2)) < rad) res++;
        return res;
    }

    private static int CountEnemyes(DPoint where, double X, double Y, DPoint[] enemyes)
    {
        int res = 0;
        for (int i = 0; i < enemyes.Length; i++)
            if (enemyes[i].X > where.X && enemyes[i].X < where.X + X &&
                enemyes[i].Y > where.Y - Y / 2 && enemyes[i].Y < where.Y + Y / 2) res++;
        return res;
    }

    #endregion

    private static int nextToYou(DPoint from, DPoint[] players)
    {
        int ind = 0; double min = 100;
        for (int i = 0; i < players.Length; i++)
            if (players[i].X != from.X && players[i].Y != from.Y && players[i].X > from.X + 5 && players[i].X < min)
            { min = players[i].X; ind = i; }

        return ind;
    }

    private static DPoint whereToPass(DPoint from, DPoint[] friends, DPoint[] enemyes)
    {
        List<DPoint> enemy = enemyes.ToList();
        int j = 0;
        for (int i = 0; i < 5; i++)
        {
            if (Math.Sqrt(Math.Pow(from.X - enemy[j].X, 2) + Math.Pow(from.Y - enemy[j].Y, 2)) < 30)
                j++;
            else
                enemy.RemoveAt(j);
        }
        double[] summ = new double[] { 0, 0 };
        for (int i = 0; i < enemy.Count; i++)
        {
            double Long = Math.Sqrt(Math.Pow(from.X - enemy[i].X, 2) + Math.Pow(from.Y - enemy[i].Y, 2));
            summ[0] += enemy[i].X / Long; summ[1] += enemy[i].Y / Long;
        }
        summ[0] /= enemy.Count; summ[1] /= enemy.Count; //единичный вектор суммирубщего направления массы врагов
        summ[0]++;

        DPoint closestShit = enemyes[Math.Max(0, closest(from, enemyes))];
        double range = Math.Sqrt(Math.Pow(from.X - closestShit.X, 2) + Math.Pow(from.Y - closestShit.Y, 2)) - 2;

        return from.Add(Math.Abs(summ[0] * range / 2), summ[1] * range / 2);
    }

    #region Misc
    private static void Write(List<String> strings)
    {
        using (StreamWriter sw = new StreamWriter("output.txt"))
        {
            for (int i = 0; i < strings.Count; i++)
                sw.WriteLine(strings[i]);
            Console.WriteLine("Success writing;");
        }
    }

    private static List<String> Read()
    {
        try
        {   //open stream
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                String line; List<String> res = new List<string>();
                do
                {
                    line = sr.ReadLine();
                    Console.WriteLine(line);
                    res.Add(line);
                } while (line != null);

                //success
                Console.WriteLine("Success reading;");
                return res;
            }
        }
        //excecption catching
        catch (Exception e)
        {
            //catched
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
        //return zero list
        return new List<string>();
    }

    private struct DPoint
    {
        public double X;
        public double Y;
        public DPoint(string[] arr)
        {
            this.X = double.Parse(arr[0], CultureInfo.InvariantCulture);
            this.Y = double.Parse(arr[1], CultureInfo.InvariantCulture);
        }

        public DPoint(double X, double Y)
        {
            this.X = X; this.Y = Y;
        }
        public override string ToString()
        {
            return String.Format("{0} {1}", X, Y);
        }

        public DPoint Add(double Xadd, double Yadd)
        {
            return new DPoint(X + Xadd, Y + Yadd);
        }
    }

    private static void ReadFromFile(out DPoint[] me, out DPoint[] en, out DPoint[] bal, out string mes)
    {
        me = new DPoint[5]; en = new DPoint[5]; bal = new DPoint[3];

        List<String> input = Read();
        string[] ballCoords = input[1].Split(' ');
        for (int i = 0; i < 2; i++)
            bal[i] = new DPoint(new string[2] { ballCoords[2 * i], ballCoords[2 * i + 1] });
        for (int i = 0; i < 5; i++)
            me[i] = new DPoint(input[2 + i].Split(' '));
        for (int i = 0; i < 5; i++)
            en[i] = new DPoint(input[7 + i].Split(' '));

        if (input.Count >= 13) mes = input[12]; else mes = null;
    }
    #endregion
}