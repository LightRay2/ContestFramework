using Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Client
{
    public class FormState : INotifyPropertyChanged, IParamsFromStartForm //todo вынести в хелпер
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static string saveLoadPath = FrameworkSettings.ForInnerUse.RoamingPathWithSlash + "Settings.xml";
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

            //тут можно установить начальные значения
            _serverAddress = "http://localhost:49972/";
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
        string _serverAddress;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set { _serverAddress = value;   Notify("ServerAddress"); }
        }


        string _serverLogin;
        public string ServerLogin
        {
            get { return _serverLogin; }
            set { _serverLogin = value;   Notify("ServerLogin"); }
        }

        string _fileToServer;
        public string FileToServer
        {
            get { return _fileToServer; }
            set { _fileToServer = value;  Notify("FileToServer"); }
        }

        string _serverPassword;
        public string ServerPassword
        {
            get { return _serverPassword; }
            set { _serverPassword = value;   Notify("ServerPassword"); }
        }

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

        ObservableCollection<object> _gameParamsList;
        [XmlIgnore]
        public ObservableCollection<object> GameParamsList
        {
            get
            {
                if (_gameParamsList == null)
                {
                    _gameParamsList = new ObservableCollection<object>();
                    _gameParamsList.CollectionChanged += (s, e) => Notify("GameParamsList");
                }
                return _gameParamsList;
            }
        }

        int _minTimePerMatch  =0;
        public int MinTimePerMatch
        {
            get { return _minTimePerMatch; }
            set { _minTimePerMatch = value;  Notify("MinTimePerMatch"); }
        }

        DateTime _matchDate = DateTime.Now;
        public DateTime MatchDate
        {
            get { return _matchDate; }
            set { _matchDate = value; Notify("MatchDate"); }
        }

        bool _runMatchesServerMode=false;
        public bool RunMatchesServerMode
        {
            get { return _runMatchesServerMode; }
            set { _runMatchesServerMode = value; Notify("RunMatchesServerMode"); }
        }

        ObservableCollection<ServerRoom> _serverRooms;
        [XmlIgnore]
        public ObservableCollection<ServerRoom> ServerRooms
        {
            get
            {
                if (_serverRooms == null)
                {
                    _serverRooms = new ObservableCollection<ServerRoom>();
                    _serverRooms.CollectionChanged += (s, e) => Notify("ServerRooms");
                }
                return _serverRooms;
            }
        }

        int _selectedRoom = 0;
        public int SelectedRoom
        {
            get { return _selectedRoom; }
            set { _selectedRoom = value; Notify("SelectedRoom"); }
        }
        //todo more programs

        string javaPath;
        public string JavaPath
        {
            get { return javaPath; }
            set { javaPath = value; if (!loading)  Notify("JavaPath"); }
        }

        int _randomSeed = new Random().Next();
        public int RandomSeed
        {
            get { return _randomSeed; }
            set { _randomSeed = value; if (!loading) Notify("RandomSeed"); }
        }

        double _FramesPerTurnMultiplier = 1.0;
        /// <summary>
        /// когда менем скорость, меняется и он
        /// </summary>
        public double FramesPerTurnMultiplier
        {
            get { return _FramesPerTurnMultiplier; }
            set { _FramesPerTurnMultiplier = value; if (!loading) Notify("FramesPerTurnMultiplier"); }
        }
        
    }
}
