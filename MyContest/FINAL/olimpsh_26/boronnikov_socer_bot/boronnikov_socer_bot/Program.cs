using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace boronnikov_socer_bot
{
    class Program
    {
        static Comand me;
        static void read()
        {
            Ball ball;
            Player[] players = new Player[10];
            FileStream inputFile = new FileStream("input.txt", FileMode.OpenOrCreate);
            StreamReader readWorld = new StreamReader(inputFile);
            string count = readWorld.ReadLine();
            ball = new Ball(readWorld.ReadLine());
            for (int i = 0; i < 10; i++)
            {
                string pl = readWorld.ReadLine();
                players[i] = new Player(pl);
            }
            //чтение memory
            /*for (int i = 0; i < 5; i++)
            {
                string pl = readWorld.ReadLine();
                players[i].setActionPoint(plAct);
            }*/
            readWorld.Close();
            inputFile.Close();
        }

        static void write()
        {
            FileStream outputFile = new FileStream("output.txt", FileMode.OpenOrCreate);
            StreamWriter writeComand = new StreamWriter(outputFile);
            //атаки команды
            for (int i = 0; i < 5; i++)
            {
                //strategy.myTeam[i].setActionPoint = new Point(50, 50);
                writeComand.WriteLine(me.ball.pos);
            }
            writeComand.WriteLine();
            //writeComand.WriteLine(strategy._ball.getPoint.X + ' ' + strategy._ball.getPoint.Y);
            writeComand.WriteLine("memory ");
            for (int i = 0; i < 5; i++)
            {
                //strategy.myTeam[i].setActionPoint = new Point(50, 50);
                writeComand.WriteLine("50 50");
            }
            writeComand.Close();
            outputFile.Close();
        }

        static void Action()
        {
            for (int i = 0; i < 5; i++)
            {
                if (me.weHaveBall())
                {
                    me.goLeft();
                }
                else
                {
                    me.goToBall();
                }
            }
        }

        static void allTeam()
        {

        }

        static double redVall(string s, int n)
        {
            return double.Parse(s.Split(' ')[n], CultureInfo.InvariantCulture);
        }

        static void Main(string[] args)//100x60
        {
            //считываем
            FileStream inputFile = new FileStream("input.txt", FileMode.OpenOrCreate);
            StreamReader readWorld = new StreamReader(inputFile);

            string count = readWorld.ReadLine();
            string ball = readWorld.ReadLine();
            string[] players = new string[10];

            for (int i = 0; i < 10; i++)
            {
                players[i] = "0 0";// readWorld.ReadLine();//в players - координаты игроков
            }

            //чтение memory
            string starBallPos = readWorld.ReadLine();
            for (int i = 0; i < 5; i++)
            {
                string pl = readWorld.ReadLine();
                //players[i].setActionPoint(plAct);
            }
            readWorld.Close();
            inputFile.Close();

            int paser = 2; //id пасующего
            int oldPaser = 0;
            bool pas = false;
            int idPas = -1;
            for (int i = 0; i < 5; i++)
            {
                if (redVall(players[i], 0) == redVall(ball, 0))
                {
                    if (redVall(players[i], 1) == redVall(ball, 1))
                    {
                        // наша команда владеет мячом

                        //вычисляем близость мяча к защите 3 & 4
                        for (int z = 0; z < 2; z++)
                        {
                            //if (Math.Pow(redVall(ball, 0) - redVall(players[z], 0), 2) + Math.Pow(redVall(ball, 1) - redVall(players[z], 1), 2) > 25)
                            {
                                players[z] = Convert.ToString(redVall(ball, 0) - 5) + ' ' + Convert.ToString(redVall(ball, 1) - 5);
                            }
                            /*else
                            {
                                players[z] = ball;
                            }*/
                        }
                        for (int z = 2; z < 5; z++)
                        {
                            Random r = new Random();
                            players[z] = Convert.ToString(redVall(players[z], 0) + 5) + ' ' + Convert.ToString(redVall(players[i], 0) + r.Next(0,60));
                        }
                        //нападающие
                        for (int z = 2; z < 5; z++)
                        {
                            if (z != paser) // если не пасер
                            {
                                if (players[z] == ball) //если схватил мяч
                                {
                                    ball = players[paser]; //посылаем мяч пасующему
                                    idPas = paser;
                                    oldPaser = z;
                                    pas = true;
                                }
                                else
                                {
                                    players[z] = ball;
                                }
                            }
                        }

                        if (players[paser] == ball)
                        {
                            if (paser != 3)
                                paser = 3;
                            else
                                paser = 4;
                        }
                        if (!pas)
                        {//если паса нет
                            if (redVall(ball, 1) > 30)
                                players[paser] = Convert.ToString(redVall(ball, 0) - 20) + ' ' + Convert.ToString(redVall(ball, 1) - 30);
                            else
                                players[paser] = Convert.ToString(redVall(ball, 0) - 20) + ' ' + Convert.ToString(redVall(ball, 1) + 30);
                        }
                        else
                        {//если дали пас
                            if (players[paser] == ball)
                            {//если пасующий схватил мяч
                                paser = oldPaser;
                                pas = false;
                            }
                            else
                            {
                                players[paser] = ball;
                            }
                        }
                        for (int z = 0; z < 5; z++)
                        {
                            if (z != idPas)
                            {
                                Random r = new Random();
                                players[z] = Convert.ToString(redVall(players[z], 0) - 20) + ' ' + Convert.ToString(redVall(players[z], 1) + r.Next(0,60));
                            }
                        }
                    }
                }
                else//когда команда не владеет мячём
                {
                    /*for (int z = 0; z < 2; z++)
                    {
                        if (Math.Pow(redVall(ball, 0) - redVall(players[z], 0), 2) + Math.Pow(redVall(ball, 1) - redVall(players[z], 1), 2) > 25)
                        {
                            players[z] = Convert.ToString(redVall(ball, 0) - 5) + ' ' + 15;
                        }
                        else
                        {
                            players[z] = ball;
                        }
                    }*/
                    for (int z = 0; z < 5; z++)
                    {
                        players[z] = ball;
                    }
                }
            }

            for (int z = 0; z < 5; z++)
            {

            }


            //записываем
            FileStream outputFile = new FileStream("output.txt", FileMode.OpenOrCreate);
            StreamWriter writeComand = new StreamWriter(outputFile);
            //атаки команды
            for (int i = 0; i < 5; i++)
            {
                //strategy.myTeam[i].setActionPoint = new Point(50, 50);
                writeComand.WriteLine(players[i]);
            }
            if (idPas > -1)
                writeComand.WriteLine(players[idPas]);
            else
                writeComand.WriteLine("100 30");
            //writeComand.WriteLine(strategy._ball.getPoint.X + ' ' + strategy._ball.getPoint.Y);
            writeComand.WriteLine("memory ");
            writeComand.WriteLine(ball);
            for (int i = 0; i < 5; i++)
            {
                //strategy.myTeam[i].setActionPoint = new Point(50, 50);
                writeComand.WriteLine("50 50");
            }
            writeComand.Close();
            outputFile.Close();

        }

        class World
        {
            public string b;
            public string[] p;
            public World(string ball, string[] pl)
            {
                b = ball;
                p = pl;
            }
        }

        static void StrRead()
        {
        }

    }
}
