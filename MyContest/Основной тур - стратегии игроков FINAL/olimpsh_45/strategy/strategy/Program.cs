using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace strategy
{
    public class cell{
        public cell(double tx, double ty){
            x = tx;
            y = ty;
        }
        public double x;
        public double y;
        public void Print()
        {
            Console.WriteLine(Convert.ToString(x) + " " + Convert.ToString(y));
        }
    }
    class Program
    {
        //VAR
        public static int NumOfStep;
        public static cell BallCoord;
        public static cell BallTraject;
        public static List<cell> MyPlayers = new List<cell>();
        public static List<cell> EnemyPlayers = new List<cell>();
        public static string InputMemory;

        public static bool BallIsMine = false;

        public static List<cell> MyPlayersStep = new List<cell>();
        public static cell MyBallStep;
        //VAR
        public static double GetDistance(cell a, cell b)
        {
            return Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y),2));
        }
        public static void Load()
        {
            StreamReader sr = new StreamReader("input.txt");
            string s1 = sr.ReadLine();
            NumOfStep = Convert.ToInt32(s1.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0]);
            Console.WriteLine("NumOfStep: " + Convert.ToString(NumOfStep));
            string s2 = sr.ReadLine();
            double b_x = Double.Parse(s2.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0] , CultureInfo.InvariantCulture);
            double b_y = Double.Parse(s2.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture);
            double v_x = Double.Parse(s2.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2], CultureInfo.InvariantCulture);
            double v_y = Double.Parse(s2.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[3], CultureInfo.InvariantCulture);
            BallCoord = new cell(b_x, b_y);
            BallTraject = new cell(v_x, v_y);
            Console.WriteLine("BallCoord: " + Convert.ToString(b_x) + " " + Convert.ToString(b_y));
            Console.WriteLine("BallTraect: " + Convert.ToString(v_x) + " " + Convert.ToString(v_y));


            for (int i = 0; i < 5; i++)
            {
                string s3 = sr.ReadLine();
                double u_x = Double.Parse(s3.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.InvariantCulture);
                double u_y = Double.Parse(s3.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture);
                MyPlayers.Add(new cell(u_x, u_y));
                Console.WriteLine("My: " + Convert.ToString(u_x) + " " + Convert.ToString(u_y));
            }
            for (int i = 0; i < 5; i++)
            {
                string s3 = sr.ReadLine();
                double u_x = Double.Parse(s3.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.InvariantCulture);
                double u_y = Double.Parse(s3.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture);
                EnemyPlayers.Add(new cell(u_x, u_y));
                Console.WriteLine("Enemy: " + Convert.ToString(u_x) + " " + Convert.ToString(u_y));
            }
            string s4 = sr.ReadLine();
            if (s4 == "-1")
            {
                InputMemory = "";
            }
            else
            {
                for (int i = 7; i < s4.Length; i++)
                {
                    InputMemory += s4[i];
                }
            }
            sr.Close();
            Console.WriteLine(InputMemory);
        }
        public static void Scan()
        {
            
            foreach (cell a in MyPlayers)
            {
                if (a == BallCoord)
                {
                    BallIsMine = true;
                    Console.WriteLine("Ball is Mine");
                }
            }
            Console.WriteLine("...");
        }
        public static void Tactic1()
        {
            for (int i = 0; i < 5; i++)
            {
                MyPlayersStep.Add(BallTraject);
                
            }
            Random r = new Random();
            double ar = Convert.ToDouble(r.Next(0, 61));
            MyBallStep = new cell(100.0, ar);
        }
        public static void Tactic2()
        {
            for (int i = 0; i < 5; i++)
            {
                MyPlayersStep.Add(MyPlayers[i]);

            }
            cell Enemy1 = EnemyPlayers[0];
            
            foreach (cell a in EnemyPlayers)
            {
                if (a.x < Enemy1.x)
                {
                    Enemy1 = a;
                }
            }

            MyPlayersStep[0] = Enemy1;
            MyPlayersStep[1] = BallCoord;
            MyPlayersStep[2] = new cell(80, 30);
            MyPlayersStep[3] = BallCoord;
            MyPlayersStep[4] = BallTraject;
            //if (GetDistance(Enemy1, BallTraject) > GetDistance(MyPlayersStep[0], BallTraject) || GetDistance(Enemy1, BallTraject) < 30)
            //{
            //    MyPlayersStep[0] = BallCoord;
            //}
            if(GetDistance(MyPlayersStep[2], BallTraject) <= 40){
                MyPlayersStep[2] = BallTraject;
            }
            if (BallCoord.x <= 50)
            {
                Random r = new Random();
                double ar = Convert.ToDouble(r.Next(0, 61));
                MyBallStep = new cell(100.0, ar);
            }
            else
            {
                if (GetDistance(BallCoord, MyPlayersStep[2]) <= 1.9)
                {
                    MyBallStep = new cell(100.0, MyPlayers[2].y);
                }
                else
                {
                    Random r = new Random();
                    double ar = Convert.ToDouble(r.Next(0, 61));
                    MyBallStep = new cell(100.0, ar);
                }
                
            }
            
        }
        
        public static void Save()
        {
            StreamWriter sr = new StreamWriter("output.txt");
            foreach (cell a in MyPlayersStep)
            {
                sr.WriteLine(Convert.ToString(a.x) + " " + Convert.ToString(a.y));
            }
            sr.WriteLine((Convert.ToString(MyBallStep.x) + " " + Convert.ToString(MyBallStep.y)));
            sr.Close();
        }
        static void Main(string[] args)
        {
            Load();
            Scan();
            Tactic2();
            Save();
        }
    }
}
