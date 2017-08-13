using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace testBot
{

    class Player
    {
        public double X;
        public double Y;

        public Player(double x, double y)
        {
            X = x;
            Y = y;
        }


    }

    class Ball
    {
        public double X1;
        public double Y1;

        public double X2;
        public double Y2;

        public double lengh = 0;

        public bool udar = false;
        public Ball(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }
    }

    class Strategy
    {
        public void toBall(Player[] playersOur, Player[] playersEnemy, Ball ball)
        {
            bool rush = false;
            for (int i = 1; i < playersOur.Length; i++)
            {
                playersOur[0].X = 30;
                playersOur[0].Y = 30;

                if (ball.X1 < 50)
                {
                    playersOur[0].Y = ball.Y1;
                    playersOur[0].X = ball.X1;
                }
                else
                {
                    playersOur[0].X = 30;
                    playersOur[0].Y = ball.Y1;
                }
                playersOur[i].X = ball.X1;
                playersOur[i].Y = ball.Y1;
                if (playersOur[0].X == ball.X1 && playersOur[0].Y == ball.Y2)
                {
                    rush = true;
                    ball.udar = true;
                }
                if (playersOur[i].X == ball.X1 && playersOur[i].Y == ball.Y2)
                {
                    rush = true;
                }
                if (rush)
                {
                    for (int f = 1; f < 5; f++)
                    {
                        playersOur[f].X = ball.X1 + 10;
                        playersOur[f].Y = ball.Y1;

                        //if (playersOur[i].X < playersOur[f].X)
                        //{
                        //    ball.udar = true;
                        //    ball.Y2 = playersOur[f].X;
                        //}

                        //if() 
                    }
                }
                ball.lengh = Math.Sqrt(Math.Pow((100 - ball.X1), 2.0) + Math.Pow((ball.Y1 - ball.Y1), 2.0));
                if (ball.lengh < 30)
                    ball.udar = true;
                //if (playersOur[0].X > playersOur[i].X) 
                //{ 
                // ball.udar = true; 
                // ball.X2 = playersOur[0].X; 
                // ball.Y2 = playersOur[0].Y; 
                //} 
                //for (int k = 0; k < 5; k++) 
                //{ 
                // if (playersOur[k].X < playersOur[0].X) 
                // continue; 
                // else { ball.X2 = playersOur[0].X; ball.Y2 = playersOur[0].Y; ball.udar = true; } 
                //} 
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            FileStream fstream = new FileStream(@"input.txt", FileMode.OpenOrCreate);
            StreamReader str = new StreamReader(fstream);
            string textFromFile = "";
            textFromFile = str.ReadToEnd();
            string[] AllNumText = textFromFile.Split(new string[] { " ", "", ":", ";", "}", "{", "=", "(", ")", "\"", "\n", "\r", "\t" }, System.StringSplitOptions.RemoveEmptyEntries);

            double[] AllNum = new double[28];
            for (int i = 0; i < AllNumText.Length; i++)
            {
                AllNum[i] = Convert.ToDouble(AllNumText[i]);
            }

            //Создаем массивчик игроков 
            Player[] playersOur = new Player[5];
            for (int i = 0, count = 6; i < 5; i++)
            {
                playersOur[i] = new Player(AllNum[++count], AllNum[++count]);
            }

            Player[] playersEnemy = new Player[5];
            for (int i = 0, count = 16; i < 5; i++)
            {
                playersEnemy[i] = new Player(AllNum[++count], AllNum[++count]);
            }

            Ball ball = new Ball(AllNum[3], AllNum[4], AllNum[5], AllNum[6]);

            double Peredanoe = AllNum[27];

            Strategy strat = new Strategy();
            strat.toBall(playersOur, playersEnemy, ball);

            //Console.WriteLine("Текст из файла: {0}", textFromFile); 

            str.Close();
            fstream.Close();

            string outText = "";
            for (int i = 0; i < 5; i++)
            {
                outText += playersOur[i].X + " " + playersOur[i].Y + "\r\n";
            }
            if (ball.udar == true)
                outText += "100 " + ball.Y2;
            ball.udar = false;

            FileStream outputFile = new FileStream("output.txt", FileMode.OpenOrCreate);
            StreamWriter writeComand = new StreamWriter(outputFile);
            writeComand.WriteLine(outText);
            writeComand.Close();
            outputFile.Close();

            // Console.ReadLine(); 
        }

    }
}