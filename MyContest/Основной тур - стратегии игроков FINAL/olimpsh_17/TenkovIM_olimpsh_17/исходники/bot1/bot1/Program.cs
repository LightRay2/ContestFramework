using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Threading;

namespace bot1
{
    class Program
    {
        class Player {
            public double x1 { get; set; }
            public double y1 { get; set; }
            public double x2 { get; set; }
            public double y2 { get; set; }
            public int number { get; set; }
            public Player()
            {
                x1 = 0;
                y1 = 0;
                x2 = 0;
                y2 = 0;
                number = 0;
            }

        }
        
        class Ball
        {
            public double x1 { get; set; }
            public double y1 { get; set; }
            public double x2 { get; set; }
            public double y2 { get; set; }
        }

        static double parse(string s)
        {
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        static void Main(string[] args)
        {
            string[] info = new string[3];
            string[] myposition= new string[5];
            string[] enemyPosition = new string[5];
            string[] ballPosition = new string[4];
            string[] playerPosition = new string[2];
            string plan;
            string enter = Environment.NewLine;
            Ball ball = new Ball();
            Player[] myTeam = new Player[5] { new Player(), new Player(), new Player(), new Player(), new Player() } ;
            Player[] enemyTeam = new Player[5] { new Player(), new Player(), new Player(), new Player(), new Player() }; ;
        
            int i;

            #region read
            try
            {
                    var input = File.ReadAllLines("input.txt");
                    info = input[0].Split();

                    ballPosition = input[1].Split();

                    ball.x1 = parse(ballPosition[0]);
                    ball.y1 = parse(ballPosition[1]);
                    ball.x2 = parse(ballPosition[2]);
                    ball.y2 = parse(ballPosition[3]);

                    for (i = 0; i < 5; i++)
                    {
                        myposition[i] = input[i + 2];
                        playerPosition = myposition[i].Split();
                        myTeam[i].number = i;
                        myTeam[i].x1 = parse(playerPosition[0]);
                        myTeam[i].y1 = parse(playerPosition[1]);
                    }
                    for (i = 0; i<5; i++)
                    {
                        enemyPosition[i] = input[i + 7];
                        playerPosition = enemyPosition[i].Split();
                    enemyTeam[i].number = i;
                        enemyTeam[i].x1 = parse(playerPosition[0]);
                        enemyTeam[i].y1 = parse(playerPosition[1]);
                    }
                    plan = input[12];
                }
               catch(Exception exс)
                {
                   File.WriteAllText("output.txt", exс.Message);
                }
            #endregion read
            try
            {
                strategy(myTeam, enemyTeam, ball);
                writePlans(myTeam, ball);
            }
            catch (Exception ec)
            {
                File.WriteAllText("output.txt", ec.Message);
            }         
        }

        static void allTeamGo(Player p, Player[] team)
        {
            for (int i=1; i<team.Length; i++)
            {
                if (  team[i].x1 - p.x1 < 5)
                    team[i].x2 = team[i].x1 + 3;
                else
                    team[i].x2 = team[i].x1 - 3;
                team[i].y2 = team[i].y1;
            }
        }
        static void allTeamBack(Player p, Player[] team)
        {
            for (int i = 1; i < team.Length; i++)
            {
                team[i].x2 = team[i].x1-3;
                team[i].y2 = team[i].y1;
            }
        }
        static double distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2,2));
        }

        static Player teamHasBall(Player[] team, Ball ball)
        {
            for (int i = 0; i < 5; i++)
            {
                if (Math.Abs(team[i].x1 - ball.x2)<2  && Math.Abs(team[i].y1 - ball.y2) < 2)
                {
                    return team[i];
                }
            }
            return null;            
        }

        static Player nearestFriend(Player p, Player[] team)
        {
            int result = 0;
            double min = 100;
            for (int k=0; k<team.Length; k++)
            {
                if (k != p.number)
                {
                    var d = distance(p.x1, p.y1, team[k].x1, team[k].y1);
                    if (d < min)
                    {
                        min = d;
                        result = k;
                    }
                }
            }
            return team[result];           
        }

        static Player nearestEnemy(Player p, Player[] team)
        {
            int result = 0;
            double min = 100;
            for (int k = 0; k < team.Length; k++)
            {
                var d = distance(p.x1, p.y1, team[k].x1, team[k].y1);
                if (d < min)
                {
                    min = d;
                    result = k;
                }
            }
            return team[result];
        }

        static Player nearBall(Ball b, Player[] team)
        {
            int result = 0;
            double min = 100;
            for (int k = 0; k < team.Length; k++)
            {
                var d = distance(b.x2, b.y2, team[k].x1, team[k].y1);
                if (d < min)
                {
                    min = d;
                    result = k;
                }
            }
            return team[result];
        }

        static bool nearGate(Player p)
        {
            return (p.x1 + 30 > 100);
        }
        static void strategy(Player[] team, Player[] enemy, Ball ball)
        {
            var withBall = teamHasBall(team, ball);
            for(int i = 0; i< team.Length; i++)
            {
                team[i].x2 = team[i].x1;
                team[i].y2 = team[i].y1;
            }
            if (withBall != null) //есть мяч у меня
            {
                if (withBall.number == 0)
                {
                    Player nearFriend = nearestFriend(withBall, team);
                    ball.x2 = nearFriend.x1;
                    ball.y2 = nearFriend.y1;
                }
                else
                {
                    if (nearGate(withBall)) //игрок с мячом рядом с воротами
                    {
                        withBall.x2 += 2.5;
                        ball.x2 += 30;
                        ball.y2 = ball.y1 + 5;
                    }
                    else
                    {
                        Player nearEnemy = nearestEnemy(withBall, enemy);
                        var distanceToEnemy = distance(withBall.x1, withBall.y1, nearEnemy.x1, nearEnemy.y1);
                        Player nearFriend = nearestFriend(withBall, team);
                        var distanceToFriend = distance(withBall.x1, withBall.y1, nearFriend.x1, nearFriend.y1);

                        if ((distanceToEnemy < distanceToFriend) && (distanceToEnemy < 6))
                        {
                            //отдать пас
                            ball.x2 = nearFriend.x1;
                            ball.y2 = nearFriend.y1;
                        }
                        else
                        {
                            //бежим, не пинаем

                            allTeamGo(withBall, team);
                            ball.x2 = withBall.x2;
                            ball.y2 = withBall.y2;
                        }
                    }
                }
            }
            else
            {
                var enemyWithBall = teamHasBall(enemy, ball);
                if (enemyWithBall != null) //мяч у врага
                {


                    Player nearFriend = nearestEnemy(enemyWithBall, team);
                    //отступить
                    allTeamBack(nearFriend, team);
                    //отобрать
                    nearFriend.x2 = ball.x2;
                    nearFriend.y2 = ball.y2;
                    team[4].y2 = enemyWithBall.y2;
                }
                else //мяч ни у кого
                {
                    Player nearEnemy = nearBall(ball, enemy);
                    var distanceFromEnemy = distance(ball.x2, ball.y2, nearEnemy.x1, nearEnemy.y1);
                    Player nearFriend = nearBall(ball, team);
                    var distanceFromFriend = distance(ball.x2, ball.y2, nearFriend.x1, nearFriend.y1);

                    if (distanceFromEnemy < distanceFromFriend) //первый добежит враг
                    {
                        //отступить
                        allTeamBack(nearFriend, team);
                        //отобрать
                        nearFriend.x2 = ball.x2 - 0.5;
                        nearFriend.y2 = ball.y2;
                    }
                    else
                    {
                        //бежим
                        allTeamGo(nearFriend, team);
                        nearFriend.x2 = ball.x2;
                        nearFriend.y2 = ball.y2;
                    }
                }
            }
            team[0].y2 = ball.y2;
            team[0].x2 = 5;
        }

        static void writePlans(Player[] team, Ball ball)
        {
            string output = "";
            string enter = Environment.NewLine;
            for (int i = 0; i < 5; i++)
            {
                output += String.Format(CultureInfo.InvariantCulture, "{0:0.000} {1:0.000} {2}", team[i].x2, team[i].y2, enter);
            }
                output += String.Format(CultureInfo.InvariantCulture, "{0:0.000} {1:0.000} {2}", ball.x2, ball.y2, enter);
            File.WriteAllText("output.txt", output);
        }


    }
}
