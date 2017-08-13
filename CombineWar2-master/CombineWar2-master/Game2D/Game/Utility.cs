using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Game2D.Game
{
    static class Utility
    {
        public static double eps = 1e-12;

        public static double MinAbs(double a, double b)
        {
            if (doubleLess(Math.Abs(a), Math.Abs(b)))
                return Math.Abs(a);
            return Math.Abs(b);
        }

        public static double GetFractionalPart(double number)
        {
            return (number - Math.Truncate(number));
        }

        public static double LimitAngle(double angle)
        {
            if (angle > 180)
                angle -= 360;
            if (angle < -180)
                angle += 360;
            return angle;
        }

        public static bool doubleEqual(double a, double b)
        {
            return Math.Abs(a - b) < eps;
        }

        public static bool doubleLessOrEqual(double a, double b)
        {
            return a < b || doubleEqual(a, b);
        }

        public static bool doubleLess (double a, double b)
        {
            return a < b && !doubleEqual(a, b);
        }

        public static bool doubleGreaterOrEqual(double a, double b)
        {
            return a > b || doubleEqual(a, b);
        }

        public static bool doubleGreater(double a, double b)
        {
            return a > b && !doubleEqual(a, b);
        }

        public static bool inClosedInterval(double n, double l, double r)
        {
            return doubleGreaterOrEqual(n, l) && doubleLessOrEqual(n, r);
        }

        public static bool inOpenInterval(double n, double l, double r)
        {
            return doubleGreater(n, l) && doubleLess(n, r);
        }

        public static bool TryWriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            bool ok = true;
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            catch (Exception ex)
            {
                ok = false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
            return ok;
        }

        public static T TryReadFromXmlFile<T>(string filePath) where T : new()
        {
            bool ok = true;
            T returned = default(T);
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                returned= (T)serializer.Deserialize(reader);
            }
            catch
            {
                ok = false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            if (!ok)
                return default(T);
            else
                return returned;
        }
    }
}
