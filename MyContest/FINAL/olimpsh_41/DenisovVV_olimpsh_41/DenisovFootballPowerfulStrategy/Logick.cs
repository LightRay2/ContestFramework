using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DenisovFootballPowerfulStrategy
{
    public class Logic
    {
        public static string[] Analize(Player[] players, Ball ball)
        {
            double[] OurRanges = new double[5];
            double[] AlienRanges = new double[5];

            for (int i = 0; i < 5; i++)
            {
                double range = Measure(players[i].current, ball.start);
                OurRanges[i] = range;
            }

            for (int i = 0; i < 5; i++)
            {
                double range = Measure(players[i].current, ball.start);
                AlienRanges[i] = range;
            }

            string[] result = new string[6];

            result[1] = GetDest(players[1], ball);
            result[3] = GetDest(players[3], ball);
            result[4] = GetDest(players[4], ball);


            string strike = "";
            string[] def = Defend(new Player[] { players[0], players[2] }, ball, out strike, players);

            result[0] = def[0];
            result[2] = def[1];


            if (strike.Length == 0)
            {
                #region Проверка на возможность нанести завершающий удар

                if (strike.Length == 0 && IsOwn(players[1], ball))
                {
                    if (TryStrikeBuild(players[1], new Points(100, players[1].current.Y), players))
                    {
                        strike = "100 " + players[1].current.Y;
                    }
                    else
                    for (int i = 0; i < 100; i++)
                        if (TryStrikeBuild(players[1], new Points(100, i), players))
                        {
                            strike = "100 " + i;
                            break;
                        }
                }
                if (strike.Length == 0 && IsOwn(players[3], ball))
                {
                    if (TryStrikeBuild(players[3], new Points(100, players[3].current.Y), players))
                    {
                        strike = "100 " + players[3].current.Y;
                    } else
                    for (int i = 0; i < 100; i++)
                        if (TryStrikeBuild(players[1], new Points(100, i), players))
                        {
                            strike = "100 " + i;
                            break;
                        }
                }
                if (strike.Length == 0 && IsOwn(players[4], ball))
                {
                    if (TryStrikeBuild(players[4], new Points(100, players[4].current.Y), players))
                    {
                        strike = "100 " + players[4].current.Y;
                    }
                    else
                    for (int i = 0; i < 100; i++)
                        if (TryStrikeBuild(players[1], new Points(100, i), players))
                        {
                            strike = "100 " + i;
                            break;
                        }
                }
                #endregion

                #region Проверка на возможность свободно двигаться
                if (strike.Length == 0 && players[1].current.X < ball.start.X && 
                    players[3].current.X < ball.start.X &&
                    players[4].current.X < ball.start.X)
                {
                    int step = 7;
                    if (IsOwn(players[1], ball)) {
                        if (TryStrikeBuild(players[1], new Points(players[1].current.X + step, players[1].current.Y + step), players))
                        {
                            result[1] = "" + (players[1].current.X + step) + ' ' + (players[1].current.Y + step);
                            result[3] = "" + (players[3].current.X + step) + ' ' + (players[1].current.Y);
                            result[4] = "" + (players[4].current.X + step) + ' ' + (players[1].current.Y - step);
                        }
                        else if (TryStrikeBuild(players[1], new Points(players[1].current.X - step, players[1].current.Y - step), players))
                        {
                            result[1] = "" + (players[1].current.X + step) + ' ' + (players[1].current.Y + step);
                            result[3] = "" + (players[3].current.X + step) + ' ' + (players[1].current.Y);
                            result[4] = "" + (players[4].current.X + step) + ' ' + (players[1].current.Y - step);
                        }
                    }
                    else
                        if (IsOwn(players[3], ball))
                        {
                            if (TryStrikeBuild(players[3], new Points(players[3].current.X + step, players[3].current.Y + step), players))
                            {
                                result[1] = "" + (players[1].current.X + step) + ' ' + (players[3].current.Y + step);
                                result[3] = "" + (players[3].current.X + step) + ' ' + (players[3].current.Y);
                                result[4] = "" + (players[4].current.X + step) + ' ' + (players[3].current.Y - step);
                            }
                            else if (TryStrikeBuild(players[3], new Points(players[3].current.X - step, players[3].current.Y - step), players))
                            {
                                result[1] = "" + (players[1].current.X + step) + ' ' + (players[3].current.Y + step);
                                result[3] = "" + (players[3].current.X + step) + ' ' + (players[3].current.Y);
                                result[4] = "" + (players[4].current.X + step) + ' ' + (players[3].current.Y - step);
                            }
                        }
                        else
                            if (IsOwn(players[4], ball))
                            {
                                if (TryStrikeBuild(players[4], new Points(players[4].current.X + step, players[4].current.Y + step), players))
                                {
                                    result[1] = "" + (players[1].current.X + step) + ' ' + (players[4].current.Y + step);
                                    result[3] = "" + (players[3].current.X + step) + ' ' + (players[4].current.Y);
                                    result[4] = "" + (players[4].current.X + step) + ' ' + (players[4].current.Y - step);
                                }
                                else if (TryStrikeBuild(players[4], new Points(players[4].current.X - step, players[4].current.Y - step), players))
                                {
                                    result[1] = "" + (players[1].current.X + step) + ' ' + (players[4].current.Y + step);
                                    result[3] = "" + (players[3].current.X + step) + ' ' + (players[4].current.Y);
                                    result[4] = "" + (players[4].current.X + step) + ' ' + (players[4].current.Y - step);
                                }
                            }
                            else
                            {
                                result[1] = "" + ball.start.X + ' ' + (ball.start.Y + step);
                                result[3] = "" + ball.start.X + ' ' + ball.start.Y;
                                result[4] = "" + ball.start.X + ' ' + (ball.start.Y - step);
                            }
                }
                #endregion

                #region Удар отчаяния
                if (strike.Length == 0)
                {
                    int step = 5;
                    if (IsOwn(players[1], ball))
                        strike = "" + (players[1].current.X + step*2) + ' ' + (players[1].current.Y + step/2);

                    if (IsOwn(players[3], ball))
                        strike = "" + (players[3].current.X + step * 2) + ' ' + (players[3].current.Y + step / 2);

                    if (IsOwn(players[4], ball))
                        strike = "" + (players[4].current.X + step * 2) + ' ' + (players[4].current.Y + step / 2);

                }
                #endregion
            }

            #region SPECIAL STRATEGY!!!1 (more efficiency with easy bot :( )
            /*
            if (IsOwn(players[1], ball))
            {
                if (players[1].current.Y < 50)
                {
                    result[1] = "" + players[1].current.X + ' ' + "52";
                    result[3] = "" + players[1].current.X + ' ' + "48";
                    result[4] = "" + players[1].current.X + ' ' + "44";
                }
                else
                {
                    result[1] = "" + (players[1].current.X+10) + ' ' + "52";
                    result[3] = "" + (players[1].current.X + 10) + ' ' + "48";
                    result[4] = "" + (players[1].current.X + 10) + ' ' + "44";
                }
            }
            else if (IsOwn(players[3], ball))
            {
                if (players[3].current.Y < 50)
                {
                    result[1] = "" + players[3].current.X + ' ' + "52";
                    result[3] = "" + players[3].current.X + ' ' + "48";
                    result[4] = "" + players[3].current.X + ' ' + "44";
                }
                else
                {
                    result[1] = "" + (players[3].current.X + 10) + ' ' + "52";
                    result[3] = "" + (players[3].current.X + 10) + ' ' + "48";
                    result[4] = "" + (players[3].current.X + 10) + ' ' + "44";
                }
            } else if (IsOwn(players[4], ball))
            {
                if (players[4].current.Y < 50)
                {
                    result[1] = "" + players[4].current.X + ' ' + "52";
                    result[3] = "" + players[4].current.X + ' ' + "48";
                    result[4] = "" + players[4].current.X + ' ' + "44";
                }
                else
                {
                    result[1] = "" + (players[4].current.X+10) + ' ' + "52";
                    result[3] = "" + (players[4].current.X + 10) + ' ' + "48";
                    result[4] = "" + (players[4].current.X + 10) + ' ' + "44";
                }
            } */

            #endregion

            result[5] = strike;


            return result;
        }

        public static bool IsOwn(Player p, Ball b)
        {
            return Math.Abs(p.current.X - b.start.X) < 2 && Math.Abs(p.current.Y - b.start.Y) < 2;
        }

        public static string GetDest(Player p, Ball b)
        {
            string res = "";
            double t1 = Math.Abs(b.start.X - b.dest.X);
            double t2 = Math.Abs(b.start.Y - b.dest.Y);
            double rb = Math.Sqrt(t1 * t1 + t2 * t2);
            if (rb < 6)
            {
                res = "" + b.dest.X + ' ' + b.dest.Y;
            }
            else
            {
                double k = rb / 6;
                res = "" + (b.start.X + Math.Abs(b.start.X - b.dest.X) / k).ToString("0.000") + ' '
                    + (b.start.Y + Math.Abs(b.start.Y - b.dest.Y) / k).ToString("0.000");
                res.Replace(',', '.');
            }

            return res;
        }

        public static string[] Defend(Player[] p, Ball b, out string strike, Player[] ourP)
        {
            strike = "";
            string[] result = new string[2];
            if (b.dest.X < 40)
            {
                if (b.dest.Y > 30)
                {
                    Player pl, an;
                    if (p[0].current.Y < 30)
                    {
                        pl = p[0];
                        an = p[1];
                    }
                    else
                    {
                        pl = p[1];
                        an = p[0];
                    }
                    result[0] = GetDest(pl, b);
                    result[1] = "" + result[0].Substring(0, result[0].IndexOf(' ')) + " 31";
                }
                else
                {
                    Player pl, an;
                    if (p[0].current.Y < 30)
                    {
                        pl = p[1];
                        an = p[0];
                    }
                    else
                    {
                        pl = p[0];
                        an = p[1];
                    }
                    result[0] = GetDest(pl, b);
                    result[1] = "" + result[0].Substring(0, result[0].IndexOf(' ')) + " 31";
                }
            }
            else
            {
                result[0] = "5 40";
                result[1] = "5 20";
            }
            #region Что-то с чем-то для защиты
            if (IsOwn(p[0], b))
            {
                Player destP = ourP[1];
                if (TryStrikeBuild(p[0], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                destP = ourP[2];
                if (TryStrikeBuild(p[0], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                destP = ourP[3];
                if (TryStrikeBuild(p[0], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                destP = ourP[4];
                if (TryStrikeBuild(p[0], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                else strike = "" + (p[0].current.X + 30) + ' ' + (p[0].current.Y + 30);
            }
            else if (Math.Abs(p[0].current.X - b.start.X) < 2 && Math.Abs(p[0].current.Y - b.start.Y) < 2)
            {
                Player destP = ourP[0];
                if (TryStrikeBuild(p[2], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                destP = ourP[1];
                if (TryStrikeBuild(p[2], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                destP = ourP[3];
                if (TryStrikeBuild(p[2], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                destP = ourP[4];
                if (TryStrikeBuild(p[2], destP.current, ourP))
                    strike = "" + destP.current.X + ' ' + destP.current.Y;
                else strike = "" + (p[2].current.X + 30) + ' ' + (p[2].current.Y + 30);
            }
            #endregion
            return result;
        }

        public static bool TryStrikeBuild(Player p1, Points p2, Player[] enemies)
        {
            double range = Measure(p1.current, p2);
            int k = (int)range / 6 + 1;
            if (k > 5)
                return false;

            bool iss = true;
            for (int j = 5; j < enemies.Length; j++)
            {
                for (int i = 1; i <= k; i++)
                {
                    Points future = getCoords(p1.current, p2, i * 6.0);
                    double rangeToEnemy = Measure(enemies[j].current, future);
                    if (Measure(p1.current, p2) > range || rangeToEnemy <= 2 * i)
                    {
                        iss = false;
                        break;
                    }
                }
                if (!iss)
                    return false;
            }
            return iss;
        }

        public static double Measure(Points p1, Points p2)
        {
            double res = 0;
            double dx = Math.Abs(p1.X - p2.X);
            double dy = Math.Abs(p1.Y - p2.Y);
            res = Math.Sqrt(dx * dx + dy * dy);
            return res;
        }

        public static Points getCoords(Points p1, Points p2, double range)
        {
            double maxRange = Measure(p1, p2);
            if (maxRange <= range)
                return p2;

            double k = range / maxRange;

            double dx = (p1.X - p2.X) * k;
            double dy = (p1.Y - p2.Y) * k;

            Points res = new Points(p1.X - dx, p1.Y - dy);

            return res;
        }
    }
}
