using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OlferukPlaysFootball
{
    public class Vector2: ICloneable
    {
        private double eps = 1e-6;

        public double X { get; set; }
        public double Y { get; set; }

        public Vector2()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(Vector2 another)
        {
            this.X = another.X;
            this.Y = another.Y;
        }

        public bool IsClose(Vector2 other)
        {
            return Math.Abs(X - other.X) < eps && Math.Abs(Y - other.Y) < eps;
        }

        public Vector2 AverageWith(Vector2 other)
        {
            return new Vector2((X + other.X) / 2.0, (Y + other.Y) / 2.0);
        }

        public double Distance(Vector2 other)
        {
            return Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y)); 
        }

        public override string ToString()
        {
            return String.Format("{0:0.000} {1:0.000}", X, Y);
        }

        public object Clone()
        {
            return new Vector2(X, Y);
        }
    }
}

static class Extensions
{
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }
}