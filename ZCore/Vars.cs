using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework
{
    public class Vars
    {

        public static int FileUploadBufferSize = 4096;
        public static string RoamingPath;
        public static int FramesPerTurn=50;
        static Vars()
        {
            string cur = "ContestAI";            //todo check if exists
            RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + cur + "/";
            if (!Directory.Exists(RoamingPath))
                Directory.CreateDirectory(RoamingPath);

        }

    }
}
