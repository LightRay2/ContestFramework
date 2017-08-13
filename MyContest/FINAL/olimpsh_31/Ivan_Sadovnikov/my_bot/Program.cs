using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace my_bot
{
    class Program
    {
        static int tick, my_gols, any_gols;
        static Ball ball;
        static Gamer[] my_team = new Gamer[5];
        static Gamer[] any_team = new Gamer[5];
        static Game game;
        static void Main(string[] args)
        {
            try
            {
                StreamReader reader = new StreamReader("input.txt");
                string[] b = reader.ReadLine().Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries);

                tick = int.Parse(b[0]);
                my_gols = int.Parse(b[1]);
                any_gols = int.Parse(b[2]);

                b = reader.ReadLine().Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries);

                ball = new Ball(new Position(b[0] + " " + b[1]), new Position(b[2] + " " + b[3]));

                string bb;
                for (int i = 0; i != 10; i++)
                {
                    bb = reader.ReadLine();
                    if (i < 5) my_team[i] = new Gamer(new Position(bb));
                    else any_team[i - 5] = new Gamer(new Position(bb));
                }

                int team = 0;
                try { team = int.Parse(reader.ReadLine()); }
                catch { }
                if ((my_team[0].position.x > 50 && team != 1) || team == 2)
                {
                    game = new Game(2);
                    game.my_gates = 100;
                    game.any_gates = 0;
                    game.min_action = 25;
                }
                else if ((my_team[0].position.x < 50 && team != 2) || team == 1)
                {
                    game = new Game(1);
                    game.my_gates = 0;
                    game.any_gates = 100;
                    game.min_action = 25;
                }
                for (int i = 0; i != 5; i++) my_team[i].next = game.start_positions[i];

                bool have_ball = false;
                int g = 0;
                for (int i = 0; i != 5; i++)
                {
                    if (my_team[i].position.x == ball.current.x && my_team[i].position.y == ball.current.y)
                    { have_ball = true; g = i; }
                }


                if (have_ball == false)
                {
                    double dist_for_ball = 1000;
                    int q = -5;
                    double buffer_dist;
                    for (int i = 0; i != 5; i++)
                    {
                        my_team[i].next = game.start_positions[i];
                        buffer_dist = Dist(ball.current, my_team[i].position);
                        if (buffer_dist < dist_for_ball)
                        {
                            dist_for_ball = buffer_dist;
                            q = i;
                        }
                    }

                    my_team[q].next = ball.current;
                }
                else
                {
                    bool check = false;

                    if (Math.Abs(game.any_gates - my_team[g].position.x) < game.min_action)
                    {
                        ball.Kick(new Position(game.any_gates, my_team[g].position.y));
                        check = true;
                    }
                    for (int i = 0; i != 5; i++)
                    {
                        if (check == true) break;
                        double he_dist = Dist(any_team[i].position, new Position(game.any_gates, any_team[i].position.y));
                        double my_dist = Dist(my_team[g].position, new Position(game.any_gates, my_team[g].position.y));
                        if ((Dist(my_team[g].position, any_team[i].position) < 15) && (he_dist < my_dist) && Math.Abs(my_team[g].position.y - any_team[i].position.y) < 10)
                        {
                            double dist;
                            for (int j = 0; j != 5; j++)
                            {
                                if (j == g) continue;
                                dist = Dist(my_team[j].position, new Position(game.any_gates, my_team[g].position.y));
                                if (dist <= my_dist && Dist(my_team[g].position, my_team[j].position) < 25)
                                {
                                    bool check_x = false;
                                    bool check_y = false;
                                    for (int q = 0; q != 5; q++)
                                    {
                                        if (game.team == 1)
                                        {
                                            if (my_team[g].position.x < any_team[q].position.x && any_team[q].position.x < my_team[j].position.x)
                                            {
                                                check_x = true;
                                                break;
                                            }
                                            if (my_team[g].position.y < any_team[q].position.y && any_team[q].position.y < my_team[j].position.y)
                                            {
                                                check_y = true;
                                                break;
                                            }
                                        }
                                        else if (game.team == 2)
                                        {
                                            if (my_team[g].position.x > any_team[q].position.x && any_team[q].position.x > my_team[j].position.x)
                                            {
                                                check_x = true;
                                                break;
                                            }
                                            if (my_team[g].position.y > any_team[q].position.y && any_team[q].position.y > my_team[j].position.y)
                                            {
                                                check_y = true;
                                                break;
                                            }
                                        }

                                        if (check_x == true && check_y == true)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            GiveBall(my_team[j]);
                                            check = true;
                                        }
                                    }
                                }
                            }
                            if (check == false)
                            {
                                GiveBall(g);
                            }
                        }
                    }
                    if (check != true)
                    {
                        for (int i = 0; i != 5; i++) my_team[i].next = new Position(game.any_gates, my_team[g].position.y);
                    }
                }

                Out();
            }
            catch
            {
            }
        }

        static public void GiveBall(Gamer g)
        {
            ball.Kick(g.position);
        }

        static public void GiveBall(Position pos)
        {
            ball.Kick(pos);
        }

        static public void GiveBall(int g)
        {
            g++;
            if (g > 4) g = 0;
            GiveBall(my_team[g]);
        }

        static public void Out()
        {
            StreamWriter wr = new StreamWriter("output.txt");
            string bbb = "";
            for (int i = 0; i != 5; i++)
            {
                bbb = my_team[i].next.x + " " + my_team[i].next.y;
                wr.WriteLine(bbb);
            }
            if (ball.kicked == true) wr.WriteLine(ball.next.x + " " + ball.next.y);
            wr.WriteLine("memory " + game.team);
            wr.Close();
        }

        static public double Dist(Position one, Position two)
        {
            double dist = Math.Sqrt(Math.Pow(one.x - two.x, 2) + Math.Pow(one.y - two.y, 2));
            return dist;
        }
    }
}
