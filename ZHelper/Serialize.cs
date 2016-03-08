using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ZHelper
{
    public class Serialize
    {
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
                returned = (T)serializer.Deserialize(reader);
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
