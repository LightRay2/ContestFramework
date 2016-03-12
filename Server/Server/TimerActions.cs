using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Server
{
    public class TimerActions
    {
        static bool processing = false;
        public static Timer timer = new Timer();
        public static void Start()
        {
            timer.Interval = 32;
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (processing)
                return;
            processing = true;
            
            List<ServerGame> gamesToStart = new List<ServerGame>();
            using (var db = new MainContext())
            {
                var time = DateTime.Now;
                gamesToStart = db.ServerGame.Where(x => x.state == EServerGameState.waitForStart && x.DateStart >= time).ToList();
                
                //сначала запускаем, потом меняем состояние
                foreach (var game in gamesToStart)
                {
                    var runningGame = new RunningGame();
                    Manager.runningGames.TryAdd(game.Id, runningGame);
                    runningGame.RunGame(game);
                }
                
                gamesToStart.ForEach(x => x.state = EServerGameState.running);

                db.SaveChanges();
                //важно, чтоб вынимались после сохранения
                //todo сохранять в апп дата
                var waitingList = Manager.clientListWaitingForGameStart.ToList();

                foreach(var waitingClient in waitingList){
                   RunningGame  runningGame;
                    if(Manager.runningGames.TryGetValue(waitingClient.Value, out runningGame)){
                        runningGame.AddWatcher(waitingClient.Key);
                        int tmp;
                        Manager.clientListWaitingForGameStart.TryRemove(waitingClient.Key, out tmp);
                    }
                }
            }

            

            processing = false;
        }

       
    }
}