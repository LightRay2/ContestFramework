using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace test
{
    class Program
    {

        public struct Point
        {
            public double x;
            public double y;
            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }
        }
        public struct Plaer
        {
            public Point cordenat;
            public int index;
            public Plaer(Point cordenat, int index)
            {
                this.cordenat = cordenat;
                this.index = index;
            }

        }
        int time;
        int my_goll;
        int bad_goll;
        bool chec_udar = false;
        bool my_ball = false;

        bool go_end = false;


        Point PudarU = new Point(0, 10);
        Point PudarD = new Point(0, 50);


        Point ball_start;
        Point ball_end;
        Point udar;
        Point[] old_my_plaer = new Point[5];
        Point[] old_bed_plaer = new Point[5];
        Point[] my_plaer = new Point[5];

        public void chekball()
        {
            if (ball_end.x == ball_start.x && ball_end.y == ball_start.y)
                go_end = true;
        }

        public void readInfo()
        {
            StreamReader str_read = new StreamReader("input.txt");

            string inp = str_read.ReadLine();
            string[] input = inp.Split(' ');
            time = int.Parse(input[0]);
            my_goll = int.Parse(input[1]);
            bad_goll = int.Parse(input[2]);

            inp = str_read.ReadLine();
            input = inp.Split(' ');
            ball_start = new Point(double.Parse(input[0], CultureInfo.InvariantCulture), double.Parse(input[1], CultureInfo.InvariantCulture));
            ball_end = new Point(double.Parse(input[2], CultureInfo.InvariantCulture), double.Parse(input[3], CultureInfo.InvariantCulture));

            for (var i = 0; i < old_my_plaer.Length; i++)
            {
                inp = str_read.ReadLine();
                input = inp.Split(' ');
                old_my_plaer[i] = new Point(double.Parse(input[0], CultureInfo.InvariantCulture), double.Parse(input[1], CultureInfo.InvariantCulture));
            }

            for (var i = 0; i < old_bed_plaer.Length; i++)
            {
                inp = str_read.ReadLine();
                input = inp.Split(' ');
                old_bed_plaer[i] = new Point(double.Parse(input[0], CultureInfo.InvariantCulture), double.Parse(input[1], CultureInfo.InvariantCulture));
            }
        }

        public void output()
        {
            StreamWriter str_write = new StreamWriter("output.txt");
            for (var i = 0; i < my_plaer.Length; i++)
                str_write.WriteLine("{0} {1}", my_plaer[i].x, my_plaer[i].y);
            if (chec_udar)
                str_write.WriteLine("{0} {1}", udar.x, udar.y);
            str_write.Close();
        }

        public bool proverka_ter()
        {
            for (var i = 0; i < old_bed_plaer.Length; i++)
                if (old_bed_plaer[i].x < 51)
                    return true;
            return false;
        }

        public void zashita()
        {
            if (proverka_ter() && ball_start.x < 50)
            {
                if (go_end)
                {
                    my_plaer[0] = ball_end;
                    my_plaer[1] = ball_end;
                }
                else
                {
                    my_plaer[0] = ball_start;
                    my_plaer[1] = ball_start;
                }
            }
            else
            {
                my_plaer[0] = new Point(20, 10);
                my_plaer[1] = new Point(20, 50);
            }
        }

        public int chek_myball()
        {
            for (var i = 0; i < my_plaer.Length; i++)
            {
                if (old_my_plaer[i].x == ball_start.x && old_my_plaer[i].y == ball_start.y)
                {
                    my_ball = true;
                    return i;
                }
            }
            return 0;

        }

        public Point bliz_my_plaer(Point start)
        {
            double[] len = new double[3];
            for (var i = 2; i < old_my_plaer.Length; i++)
            {
                len[i - 2] = range(old_my_plaer[i], start);
            }

            int index = 0;
            for (var i = 0; i < len.Length - 1; i++)
            {
                if (len[i] > len[i + 1])
                    index = i + 1;
            }
            return old_my_plaer[index + 2];
        }
        Random ran = new Random();
        public void vorota()
        {
            if ((chek_myball() == 0) || (chek_myball() == 1))
            {
                chec_udar = true;
                //udar = bliz_my_plaer(old_my_plaer[chek_myball()]);
                udar = new Point(100, ran.Next(0, 50));
            }
            for (var i = 2; i < my_plaer.Length; i++)
                my_plaer[i] = new Point(100, my_plaer[chek_myball()].y);
            offset(my_plaer[chek_myball()]);
        }

        void my_plare_go_ball()
        {
            for (var i = 2; i < my_plaer.Length; i++)
            {
                my_plaer[i] = go_end ? ball_end : ball_start;
            }
            zashita();
        }

        void pendel()
        {
            //if (my_ball) 
            //{
            //    chec_udar = true;
            //    udar = new Point(15+old_my_plaer[chek_myball()].x, old_my_plaer[chek_myball()].y);
            //}
            if (my_ball && old_my_plaer[chek_myball()].x > 75)
            {
                chec_udar = true;
                int ofset = 5;
                if (old_my_plaer[chek_myball()].y < 30)
                    ofset = -ofset;
                udar = new Point(100, old_my_plaer[chek_myball()].y + ofset);
            }
            //if (pendel_up() != -1) 
            //{
            //    chec_udar = true;
            //    udar =my_plaer[pendel_up()];
            //}
        }

        public double range(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p1.x - p2.x), 2) + Math.Pow((p1.y - p2.y), 2));
        }

        //public int pendel_up()
        //{
        //    int old_index = chek_myball();
        //    List<Plaer> pl = new List<Plaer>();
        //    if (my_ball)
        //        for (var i = 0; i < old_my_plaer.Length; i++)
        //            if (old_my_plaer[i].x > old_my_plaer[old_index].x)
        //                pl.Add(new Plaer(old_my_plaer[i], i));

        //    if (pl.Count > 0)
        //    {

        //        Plaer[] plaer_index_renge = new Plaer[pl.Count];
        //        for (var i = 0; i < pl.Count; i++)
        //            plaer_index_renge[i].index = pl[i].index;

        //        for (var i = 0; i < pl.Count; i++)
        //        {
        //            plaer_index_renge[i].cordenat.x = range(pl[i].cordenat, old_my_plaer[old_index]);
        //        }
        //        int index = 0;
        //        for (var i = 0; i < plaer_index_renge.Length - 1; i++)
        //        {
        //            if (plaer_index_renge[i].cordenat.x > plaer_index_renge[i + 1].cordenat.x)
        //            {
        //                index = plaer_index_renge[i + 1].index;
        //            }
        //        }
        //        for (var i = 0; i < plaer_index_renge.Length; i++) 
        //        {
        //            if (plaer_index_renge[i].index == index)
        //                return i;
        //        }
        //        return -1;
        //    }
        //    else
        //        return -1;
        //}
        void attak()
        {
            if (my_ball)
            {
                my_plaer[4] = new Point(70, 50);
                vorota();
            }
        }
        void offset(Point centr)
        {
            Point[] mrC = new Point[3];
            mrC[0] = new Point(-10, 10);
            mrC[0] = new Point(0, 0);
            mrC[0] = new Point(10, -10);

            int[] chec = new int[2];
            for (var i = 2; i < my_plaer.Length; i++)
            {
                if ((my_plaer[i].x == centr.x) && (my_plaer[i].y == centr.y))
                    continue;

                my_plaer[i] = new Point(centr.x + 20, centr.y + 20);
            }

        }
        void logics()
        {
            readInfo();
            chekball();
            chek_myball();

            zashita();
            if (my_ball)
                vorota();
            else
                my_plare_go_ball();
            //attak();
            pendel();
            output();
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.logics();
        }
    }
}
