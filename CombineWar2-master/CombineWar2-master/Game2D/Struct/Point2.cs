using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2D
{
    /// <summary>
    /// точка или вектор, есть нормализация и расстояние
    /// </summary>
    struct Point2
    {
        public double x, y;
        public Point2(double x, double y)
        {
            this.x = x; this.y = y;
        }
        public void Normalize(double k)
        {
            double m = Math.Sqrt(x * x + y * y);
            x /= m; x *= k;
            y /= m; y *= k;
        }

        public static Point2 operator+(Point2 p1, Point2 p2)
        {
                return new Point2(p1.x + p2.x, p1.y + p2.y);
        }

        public static Point2 operator -(Point2 p1, Point2 p2)
        {
                return new Point2(p1.x - p2.x, p1.y - p2.y);
        }


        public static Point2 operator -(Point2 p)
        {
                return new Point2(-p.x, -p.y);
        }

        public static Point2 operator *(Point2 p, double k) 
        {
                return new Point2(p.x * k, p.y * k);
        }

        public static Point2 operator /(Point2 p, double k) 
        {
                return new Point2(p.x / k, p.y / k);
        }

        public static double operator %(Point2 p1, Point2 p2) // scalar product
        {
                return p1.x * p2.x + p1.y * p2.y;
        }

        public static double operator *(Point2 p1, Point2 p2) // vector product
        {
                return p1.x * p2.y - p1.y * p2.x;
        }


        public double Length()
        {
            return Math.Sqrt(x * x + y * y);
        }
        public double DistTo(Point2 to)
        {
            return Math.Sqrt((x - to.x) * (x - to.x) + (y - to.y) * (y - to.y));
        }

        public double getAngle(Point2 u)
        {
                Point2 v = this;
                return Math.Atan2(v * u, v % u) / Math.PI * 180;
        }


        /// <summary>
        /// Угол в градусах
        /// </summary>
        public double angleTo(Point2 to)
        {
           /* double angle = Math.Acos((x * to.x + y * to.y) / (Length() * to.Length())) / Math.PI * 180;
            if (PointRelativelyVector(to) > 0)
                angle = 360 - angle;*/
            double angle = Math.Atan((to.y - y) / (to.x - x)) / Math.PI * 180;

            return angle;
        }

        //внутренняя вспомогательная
        //1- точка слева от вектора, -1 - точка справа от вектора, 0 - на прямой вектора
        int PointRelativelyVector(Point2 to)
        {
            double s = x * (to.y - y) - y * (to.x - x);
            if (s > 0) return 1;
            else if (s < 0) return -1;
            else return 0;
        }

    }
}
