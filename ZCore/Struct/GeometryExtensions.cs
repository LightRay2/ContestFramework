using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public static class GeometryExtensions
    {
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
        
    }
}
