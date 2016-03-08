using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Framework;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class MainHub : Hub
    {
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
                return ;

            Clients.Caller.authorizeResult(Manager.ClientList[Context.ConnectionId].id);
        }

        public RoomState GetRoomState(int roomId)
        {
            if (Manager.ClientList[Context.ConnectionId] == null)
                return null;
            var roomState = new RoomState
            {
                name = "Основная комната"
            };
            return roomState;
        }

        public void StartUploadingAndGetId(string fileName, int partCount)
        {
             var client=Manager.ClientList[Context.ConnectionId];
            if(client == null)
                return ;

            var uploadingFileInfo = new UploadingFileInfo(partCount) { fileName = fileName };
            Manager.uploadingFiles.TryAdd(uploadingFileInfo.id, uploadingFileInfo);

            Clients.Caller.fileId(uploadingFileInfo.id); 
        }

        public void LoadFilePart(int id, int partNumber, byte[] filePart)
        {
            var client=Manager.ClientList[Context.ConnectionId];
            if(client == null)
                return ;

            var file = Manager.uploadingFiles[id];
            if(file != null){
                bool finished = file.AddPartAndCheckFinish(filePart,partNumber);
                if(finished){
                    string startupPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    string directory = Path.Combine(startupPath, "programs");
                    if(!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                    
                    using(var db = new MainContext()){
                        int programNum = db.ServerPlayer.Count()+1;
                        string filePhysicalName = Path.Combine(directory, "file"+programNum.ToString().PadLeft(4,'0') + file.fileName);
                        File.WriteAllBytes(filePhysicalName, file.bytes.SelectMany(x=>x).ToArray());
                        
                        var serverPlayer = new ServerPlayer{
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

        public void ConnectToGame(int gameId)
        {

        }
    }

    
}