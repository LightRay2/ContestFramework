using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  

namespace Game2D
{
    /// <summary>
    /// вектор с заданным началом, плюс реализованы вспомогательные функции
    /// </summary>
    struct Vector2
    {
        //координаты начала и вектор
        public double x, y, vx, vy ;

        //вспомогательные свойства
        public Point2 point { get { return new Point2(x, y); } }
        public Point2 vector { get { return new Point2(vx, vy); } }
        /// <summary>
        /// число от 0 до 6,28....
        /// </summary>
        public double angleRad { get { return (Math.Atan2(vy, vx)) /*+ Math.PI*2) % (Math.PI*2)*/; } }
        /// <summary>
        /// число от 0 до 360
        /// </summary>
        public double angleDeg { get { return angleRad / Math.PI * 180; } }

        //конструкторы
        /// <summary>
        /// x,y - начало
        /// vx,vy - сам вектор
        /// </summary>
        public Vector2(double x, double y, double vx, double vy)
        {
            this.vx = vx;
            this.vy = vy;
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// задаем начало и направление(в градусах). Вектор будет храниться в нормализованном виде
        /// </summary>
        public Vector2(double x, double y, double angleDeg=0)
        {
            this.x = x;
            this.y = y;
            double angleRad = angleDeg / 180 * Math.PI;
            this.vx = Math.Cos(angleRad);
            this.vy = Math.Sin(angleRad);
        }

        
        //вспомогательные функции
        public void Normalize(double k=1)
        {
            double m = Math.Sqrt(vx * vx + vy * vy);
            vx /= m; vx *= k;
            vy /= m; vy *= k;
        }
        /// <summary>
        /// в градусах
        /// </summary>
        public void Rotate(double angleDeg)
        {
            double len = this.Length();
            this = new Vector2(x, y, (this.angleDeg + angleDeg) % 360);
            vx *= len; vy *= len;
        }
        public double Length()
        {
            return Math.Sqrt(vx * vx + vy * vy);
        }
    }
}
