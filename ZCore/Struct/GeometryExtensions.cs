using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class GeometryExtensions
    {
        /// <summary>
        /// не важно, больше Х чем Y или нет
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double ToRange(this double a,  double x,  double y)
        {
            if (a == double.NaN)
                return x;
            if (x > y)
            {
                double p = x; x = y; y = p;
            }
            if (a < x)
                return x;
                
            if (a > y)
                return y;

            return a;
        }

        /// <summary>
        /// x,y в любом порядке
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="strictly"></param>
        /// <returns></returns>
        public static bool IsInRange(this double a, double x, double y, bool strictly)
        {
            if(x > y){var p = x; x = y; y = p;}
            if (a.Equal(x) || a.Equal(y))
                return strictly == false;
            return a > x && a < y;
        }
        public static Vector2d MultEach(this Vector2d one, Vector2d other)
        {
            return new Vector2d(one.X * other.X, one.Y * other.Y);
        }

        public static Vector2d DivEach(this Vector2d one, Vector2d other)
        {
            return new Vector2d(one.X / other.X, one.Y / other.Y);
        }

        public static double RoundTo14SignificantDigits(this double d)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, 14);
        }

        public static Vector2d RotateRad(this Vector2d v, double angleRad)
        {
            return Vector2d.Transform(v, Quaterniond.FromAxisAngle(new Vector3d(0, 0, 1), angleRad));
        }
        public static Vector2d RotateDeg(this Vector2d v, double angleDeg)
        {
            return Vector2d.Transform(v, Quaterniond.FromAxisAngle(new Vector3d(0, 0, 1), angleDeg / 180 * Math.PI));
        }


        /// <summary>
        /// по часовой, может быть больше или меньше оборота
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double AngleRad(this Vector2d v)
        {
            return Math.Atan2(v.X, v.Y);
        }
        /// <summary>
        /// по часовой, может быть больше или меньше оборота
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double AngleDeg(this Vector2d v)
        {
            return v.AngleRad() / Math.PI * 180;
        }
        /// <summary>
        /// по часовой, , может быть больше или меньше оборота 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double AngleRad(this Vector2d v, Vector2d other)
        {
            return Math.Atan2(other.X, other.Y) - Math.Atan2(v.X, v.Y);
        }
        /// <summary>
        /// по часовой, , может быть больше или меньше оборота 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static double AngleDeg(this Vector2d v, Vector2d other)
        {
            return v.AngleRad(other) / Math.PI * 180;
        }
        
    }
}
