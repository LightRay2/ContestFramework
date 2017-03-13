using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework
{
    public class FrameworkSettings
    {
        /// <summary>
        /// внутренние настройки, влияющие на способ работы фреймворка (вряд ли придется их менять)
        /// </summary>
        public static InnerSetting InnerSettings = new InnerSetting();
        public class InnerSetting
        {
            public int FileUploadBufferSize = 4096;
            public string RoamingPath;
            /// <summary>
            /// для дебага игры подходит
            /// </summary>
            public bool RunGameImmediately = false;
        }

        /// <summary>
        /// 0 значит 
        /// </summary>
        public static int PlayersPerGame = 0;
        public static int FramesPerTurn=50;

        public static bool AllowFastGameInBackgroundThread { get; set; }

        static FrameworkSettings()
        {
            string cur = "ContestAI";            //todo check if exists
            InnerSettings.RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + cur + "/";
            if (!Directory.Exists(InnerSettings.RoamingPath))
                Directory.CreateDirectory(InnerSettings.RoamingPath);

        }

    }
}
