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
        public static ConcurrentDictionary<int, GameState> runningGames =
            new ConcurrentDictionary<int, GameState>();

        public static ConcurrentDictionary<int, UploadingFileInfo> uploadingFiles = new ConcurrentDictionary<int, UploadingFileInfo>();

        public static void JoinGame(Client client, int gameId)
        {
            
        }

        public static void AddClientToGame(Client client, int gameId)
        {
            
        }
    }

    public class RoomState
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<GameState> games = new List<GameState>();
    }

    public class GameState
    {
        public int id { get; set; }
        public List<Client> players = new List<Client>();
        public DateTime DateStart { get; set; }
        public string Name { get; set; }
        public string GameInfo { get; set; }

    }

    public enum EClientState { none, watchGame }
    public class Client
    {
        public int id { get; set; }
        public int hasTurnCount { get; set; }

        EClientState state = EClientState.none;

        public string Name { get; set; }

        public bool IsAdmin { get; set; }
    }

}