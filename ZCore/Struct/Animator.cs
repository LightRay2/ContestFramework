using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class Animator<T>
    {
        public T start, finish;
        public double startTime, duration;
        public double finishTime { get { return startTime + duration; } set { duration = value - startTime; } }
        public Func<T, T, double, T> swingFunction;

        //todo откомментировать
        public Animator(Func<T, T, double, T> swingFunction, T start, T finish, double duration, double startTime = 0)
        {
            this.swingFunction = swingFunction;
            this.start = start;
            this.duration = duration;
            this.finish = finish;
            this.startTime = startTime;
        }

        public T Get(double currentTime)
        {
            double stage = (double)(currentTime - startTime) / (duration);
            return swingFunction(start, finish, stage);
          //  return (dynamic)left.Evaluate(context) + (dynamic)right.Evaluate(context);
        }


        
    }

    public class Animator
    {
        //todo добавить swing функций
        /// <summary>
        /// метод для вычисления плавного ускорения, стартовой точке соответствует stage = 0, финишной - stage = 1
        /// </summary>
        public static double SinSwingRefined(double start, double finish, double stage)
        {
            var append = finish - start;
            double term, k = 1.8, acc = 0.15;
            if (stage < acc) term = (-Math.Cos(stage / k * Math.PI) / 2) + 0.5;
            else if (stage < 1 - acc) term = ((-Math.Cos(((stage - 0.5) / ((0.5 - acc) / (0.5 - acc / k)) + 0.5) * Math.PI) / 2) + 0.5);
            else term = ((-Math.Cos(((stage - 1) / k + 1) * Math.PI) / 2) + 0.5);
            return start + append * term;
        }
        //просто копипаст
        public static Vector2d SinSwingRefined(Vector2d start, Vector2d finish, double stage)
        {
            var append = finish - start;
            double term, k = 1.8, acc = 0.15;
            if (stage < acc) term = (-Math.Cos(stage / k * Math.PI) / 2) + 0.5;
            else if (stage < 1 - acc) term = ((-Math.Cos(((stage - 0.5) / ((0.5 - acc) / (0.5 - acc / k)) + 0.5) * Math.PI) / 2) + 0.5);
            else term = ((-Math.Cos(((stage - 1) / k + 1) * Math.PI) / 2) + 0.5);
            return start + append * term;
        }
    }
}
