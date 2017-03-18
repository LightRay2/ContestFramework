using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Framework
{
    /// <summary>
    /// имеются в виду настройки главной формы
    /// </summary>
    public class FormMainSettings : INotifyPropertyChanged //todo вынести в хелпер
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static string saveLoadPath = FrameworkSettings.ForInnerUse.RoamingPathWithSlash + "MainFormSettings.xml";
        bool loading = true;
        public bool SaveToFile = true;
        /// <summary>
        /// не нужно вызывать, лучше static LoadOrCreate
        /// </summary>
        public FormMainSettings()
        {
            
            //тут можно установить начальные значения
            serverAddress = "http://localhost:49972/";
        }
        public static FormMainSettings LoadOrCreate()
        {
            var loadedSettings = Serialize.TryReadFromXmlFile<FormMainSettings>(saveLoadPath);
            if (loadedSettings == null)
                loadedSettings = new FormMainSettings();
            loadedSettings.loading = false;

            //тут можно что то заполнить, если заполнено неверно

            return loadedSettings;
        }
        
        private void Notify(String info)
        {
            if (loading)
                return;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
            if (SaveToFile)
            {
                bool success = Serialize.TryWriteToXmlFile<FormMainSettings>(saveLoadPath, this);
                Log.CheckIfDebug(success);
            }
        }

        // ================сами свойства===========================
        string serverAddress;
        public string ServerAddress { 
            get { return serverAddress; }
            set { serverAddress = value; if (!loading)  Notify("ServerAddress"); }
        }


        string serverLogin;
        public string ServerLogin
        {
            get { return serverLogin; }
            set { serverLogin = value; if (!loading)  Notify("ServerLogin"); }
        }

        string fileToServer;
        public string FileToServer
        {
            get { return fileToServer; }
            set { fileToServer = value; if (!loading)  Notify("FileToServer"); }
        }

        string serverPassword;
        public string ServerPassword
        {
            get { return serverPassword; }
            set { serverPassword = value; if (!loading)  Notify("ServerPassword"); }
        }


        string firstProgram;
        public string FirstProgram { 
            get { return firstProgram; }
            set { firstProgram = value; if (!loading)  Notify("FirstProgram"); }
        }

        string secondProgram;
        public string SecondProgram { 
            get { return secondProgram; }
            set { secondProgram = value; if (!loading)  Notify("SecondProgram"); }
        }

        //todo more programs

        string javaPath;
        public string JavaPath { 
            get { return javaPath; }
            set { javaPath = value; if (!loading)  Notify("JavaPath"); }
        }

        bool firstIsHuman;
        public bool FirstIsHuman
        {
            get { return firstIsHuman; }
            set { firstIsHuman = value; if (!loading)  Notify("FirstIsHuman"); }
        }

        bool secondIsHuman;
        public bool SecondIsHuman
        {
            get { return secondIsHuman; }
            set { secondIsHuman = value; if (!loading)  Notify("SecondIsHuman"); }
        }
      
    }
}
