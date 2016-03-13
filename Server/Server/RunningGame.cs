using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR;
using Client;
using System.Threading;
using Newtonsoft.Json;
namespace Server
{
    public class RunningGame
    {
        public List<Client> watcherList = new List<Client>();
         int gameId;
        ConcurrentDictionary<int, object> roundList = new ConcurrentDictionary<int, object>();

        int roundNumber = 0;
        object _locker = new object();

        public void RunGame(ServerGame game)
        {
             this.gameId = game.Id;//todo тут надо красиво вынести, раз уж используем клиента
            
            new Thread(new ThreadStart(()=>{

                string error = GameCore<State, Turn, Round, Player>.RunOnServerOrGetError(new Game(), game.StartSettings, RoundPlayed);

                if(error == "Ok")
                    this.GamePlayed();
                else
                {
                    //todo что-то делать, если игра на сервере не сыграла (вылетела с ошибкой)
                }
            })).Start();
        
        }

        public void RoundPlayed(object round, string shortStateInfo)
        {
            //важно именно снаружи заблокировать, если идет пересылка первых ходов только что подключившемуся, пусть она завершится, и уже затем ему добавится новый ход
             lock(_locker)
             {
                roundList.TryAdd(roundNumber, round);

                using (var db = new MainContext())
                {
                    var game = db.ServerGame.Find(gameId);
                    if (game != null)
                    {
                        game.JsonCurrentGameInfo = shortStateInfo;
                        db.SaveChanges();
                        Manager.hubContext.Clients.Clients(watcherList.Select(x => x.connectionId).ToList()).roundPlayed(gameId, roundNumber, round);
                    }
                }


                roundNumber++;
            }
        }

        public void GamePlayed()
        {
            lock(_locker)
             {
                RunningGame runningGame;
                Manager.runningGames.TryRemove(gameId, out runningGame);
                using(var db = new MainContext())
                {
                    var game = db.ServerGame.Find(gameId);
                    if (game != null)
                    {
                        game.state = EServerGameState.finish;
                        game.JsonGameData = JsonConvert.SerializeObject(this.roundList.ToList().OrderBy(x=>x.Key).Select(x=>x.Value).ToList());
                        db.SaveChanges();
                    }

                }
            }
        }

    
        public void AddWatcher(Client client)
        {
            lock (_locker)
            {
                watcherList.Add(client);
            }
        }}
}