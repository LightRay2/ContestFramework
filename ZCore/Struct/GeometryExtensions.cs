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

        
    }
}
