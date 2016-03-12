using Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server
{
    public class Manager
    {
        public static ConcurrentDictionary<string, Client> ClientList = new ConcurrentDictionary<string, Client>();
        public static ConcurrentDictionary<int, RunningGame> runningGames =
            new ConcurrentDictionary<int, RunningGame>();
        //todo повторное подключение = ошибка словаря?
        /// <summary>
        /// id игры
        /// вынимать отсюда только после переключения состоние игра на Старт
        /// </summary>
        public static ConcurrentDictionary<Client, int> clientListWaitingForGameStart =
            new ConcurrentDictionary<Client, int>(); //todo что делать, если игрок перезашел и уже не ждет игру?

        public static ConcurrentDictionary<int, UploadingFileInfo> uploadingFiles = new ConcurrentDictionary<int, UploadingFileInfo>();

        public static Microsoft.AspNet.SignalR.IHubContext hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<MainHub>();

        
    }

    

    public enum EClientState { none, watchGame }
    public class Client
    {
        public int id { get; set; }
        public int hasTurnCount { get; set; }

        EClientState state = EClientState.none;

        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public string connectionId { get; set; }
    }

}