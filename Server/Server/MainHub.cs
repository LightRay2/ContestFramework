using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Framework;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;

namespace Server
{
    public class MainHub : Hub
    {
        static object _locker = new object();
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }


        public void Hello()
        {
            Clients.All.hello();
        }

        public void AuthorizeAndGetMyId(string name, string password)
        {
            bool success = AuthenticationHandler.Authorize(Context.ConnectionId, name, password);
            if (!success)
                return;

            Clients.Caller.authorizeResult(Manager.ClientList[Context.ConnectionId].id, Manager.ClientList[Context.ConnectionId].IsAdmin);
            Clients.Caller.message("Соединение установлено");
        }

        public void GetRoomState(int roomId)
        {
            if (Manager.ClientList[Context.ConnectionId] == null)
                return ;
            using(var db = new MainContext()){
                var roomState=  new
                {
                    playerList = db.ServerPlayer.ToList(),
                    gameList = db.ServerGame.OrderByDescending(x => x.DateStart).ToList()
                };
               roomState.gameList.ForEach(x=>x.players = x.ServerGameServerPlayer.Select(y=>y.ServerPlayer).ToList());
                Clients.Caller.setRoomState(roomState);
                Clients.Caller.message("Состояние комнаты обновлено");
            }
        }

        public void StartUploadingAndGetId(string fileName, int partCount)
        {
            var client = Manager.ClientList[Context.ConnectionId];
            if (client == null)
                return;

            var uploadingFileInfo = new UploadingFileInfo(partCount) { fileName = fileName };
            Manager.uploadingFiles.TryAdd(uploadingFileInfo.id, uploadingFileInfo);

            Clients.Caller.fileId(uploadingFileInfo.id);
        }

        public void LoadFilePart(int id, int partNumber, byte[] filePart)
        {
            var client = Manager.ClientList[Context.ConnectionId];
            if (client == null)
                return;

            lock (_locker)
            {
                var file = Manager.uploadingFiles[id];
                if (file != null)
                {

                    bool finished = file.AddPartAndCheckFinish(filePart, partNumber);

                    if (finished)
                    {
                        string startupPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string directory = Path.Combine(startupPath, "programs");
                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        using (var db = new MainContext())
                        {
                            //todo remove old?
                            int programNum = db.ServerPlayer.Count() ;
                            string filePhysicalName = Path.Combine(directory, "file" + programNum.ToString().PadLeft(4, '0') + file.fileName);
                            File.WriteAllBytes(filePhysicalName, file.bytes.SelectMany(x => x).ToArray());

                            var serverPlayer = new ServerPlayer
                            {
                                fileName = file.fileName,
                                physicalFileName = filePhysicalName,
                                ServerUserId = client.id
                            };
                            db.ServerPlayer.Add(serverPlayer);
                            db.SaveChanges();

                            Clients.Caller.message(string.Format("Файл {0} загружен", file.fileName));
                        }
                    }
                }
            }
        }

        public void AddGame(List<int> playerIdList, DateTime date)
        {
            var client = Manager.ClientList[Context.ConnectionId];
            if (client == null && client.IsAdmin == false)
                return;

            using (var db = new MainContext())
            {
               
                var serverGame= new ServerGame{
                    DateStart = date,
                     Name = "игра",
                      RoomId = -1
                };
                
              //  db.ServerGame.Add(serverGame);
                int num = 0;
                foreach (var id in playerIdList)
                {
                    var player = db.ServerPlayer.Find(id);
                    if (player != null)
                    {
                        db.ServerGameServerPlayer.Add(new ServerGameServerPlayer
                        {
                            Number = num,
                            ServerGame = serverGame,
                            ServerPlayer =player
                        });
                        num++;
                    }

                }
                db.SaveChanges();
            }
            Clients.Caller.message("Игра добавлена");
        }

        public void ConnectToGame(int gameId)
        {
            var client = Manager.ClientList[Context.ConnectionId];
            if (client == null && client.IsAdmin == false)
                return;

            Manager.AddClientToGame(client, gameId);
            Clients.Caller.message("Вы присоединились к игре");
        }
    }


}