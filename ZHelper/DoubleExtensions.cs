using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DoubleExtensions
    {
        public static double eps = 1e-12;

        //public static double MinAbs(double a, double b)
        //{
        //    if (doubleLess(Math.Abs(a), Math.Abs(b)))
        //        return Math.Abs(a);
        //    return Math.Abs(b);
        //}

        //public static double GetFractionalPart(double number)
        //{
        //    return (number - Math.Truncate(number));
        //}

        //public static double LimitAngle(double angle)
        //{
        //    if (angle > 180)
        //        angle -= 360;
        //    if (angle < -180)
        //        angle += 360;
        //    return angle;
        //}

        public static bool Equal(this double a, double b)
        {
            return Math.Abs(a - b) < eps;
        }

        //public static bool LessOrEqual(this double a, double b)
        //{
        //    return a < b || doubleEqual(a, b);
        //}

        //public static bool doubleLess(double a, double b)
        //{
        //    return a < b && !doubleEqual(a, b);
        //}

        //public static bool doubleGreaterOrEqual(double a, double b)
        //{
        //    return a > b || doubleEqual(a, b);
        //}

        //public static bool doubleGreater(double a, double b)
        //{
        //    return a > b && !doubleEqual(a, b);
        //}

        //public static bool inClosedInterval(double n, double l, double r)
        //{
        //    return doubleGreaterOrEqual(n, l) && doubleLessOrEqual(n, r);
        //}

        //public static bool inOpenInterval(double n, double l, double r)
        //{
        //    return doubleGreater(n, l) && doubleLess(n, r);
        //}
    }
}
