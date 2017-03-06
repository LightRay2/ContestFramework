using Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MyContest
{
    public class FormState : INotifyPropertyChanged //todo вынести в хелпер
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static string saveLoadPath = FrameworkSettings.InnerSettings.RoamingPath + "GameSettings.xml";
        bool loading = true;
        public bool SaveToFile = true;

        static bool legalCreation = false; //чтобы не ошиблись и не вызвали конструктор напрямую
        /// <summary>
        /// не нужно вызывать, используйте static LoadOrCreate
        /// </summary>
        public FormState()
        {
            if (legalCreation == false)
                throw new Exception("Используйте для создания LoadOrCreate");
            legalCreation = false;
        }
        public static FormState LoadOrCreate()
        {
            legalCreation = true;
            var loadedSettings = Serialize.TryReadFromXmlFile<FormState>(saveLoadPath);
            if (loadedSettings == null)
                loadedSettings = new FormState();
            loadedSettings.loading = false;

            return loadedSettings;
        }

        public FormState DeepClone()
        {
            return Serialize.TryDeepClone(this);
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
                bool success = Serialize.TryWriteToXmlFile<FormState>(saveLoadPath, this);
                Log.CheckIfDebug(success);
            }
        }

        // ================сами свойства===========================
       

        ObservableCollection<string> _programAddressesAll;
        public ObservableCollection<string> ProgramAddressesAll
        {
            get { 
                if (_programAddressesAll == null) 
                { 
                    _programAddressesAll = new ObservableCollection<string>();
                    _programAddressesAll.CollectionChanged += (s, e) => Notify("ProgramAddressesAll"); 
                } 
                return _programAddressesAll; }
        }

        ObservableCollection<int> _programAddressesInMatch;
        public ObservableCollection<int> ProgramAddressesInMatch
        {
            get
            {
                if (_programAddressesInMatch == null)
                {
                    _programAddressesInMatch = new ObservableCollection<int>();
                    _programAddressesInMatch.CollectionChanged += (s, e) => Notify("ProgramAddressesInMatch");
                }
                return _programAddressesInMatch;
            }
        }

        string javaPath;
        public string JavaPath
        {
            get { return javaPath; }
            set { javaPath = value; if (!loading)  Notify("JavaPath"); }
        }



    }
}
