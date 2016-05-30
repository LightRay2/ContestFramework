using Framework;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace Client
{
    public partial class FormMain : Form
    {
        public static string RoamingPath = null;
        FormMainSettings settings;
        public FormMain()
        {

            
            settings = FormMainSettings.LoadOrCreate();

            InitializeComponent();

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            edtServerAddress.DataBindings.Add("Text", settings, "ServerAddress");
            edtAddressOne.DataBindings.Add("Text", settings, "FirstProgram");
            edtAddressTwo.DataBindings.Add("Text", settings, "SecondProgram");
            checkHumanOne.DataBindings.Add("Checked", settings, "FirstIsHuman");
            checkHumanTwo.DataBindings.Add("Checked", settings, "SecondIsHuman");
            edtFileToServer.DataBindings.Add("Text", settings, "FileToServer");
            btnRefreshServer_Click(null, null);

            

            grd.DataSource = gameList;
        }

        void SetAdminButtonsVisibility(bool visible)
        {
            btnAddGames.Visible = btnDeleteGame.Visible = visible;
        }

        

        private void btnAddressOne_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //todo full form recreate
                settings.FirstProgram = openFileDialog1.FileName;
            }
        }

        private void btnAddressTwo_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                settings.SecondProgram = openFileDialog1.FileName;
            }
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            string temp = settings.FirstProgram;
            settings.FirstProgram = settings.SecondProgram;
            settings.SecondProgram = temp;
            bool tempBool = settings.FirstIsHuman;
            settings.FirstIsHuman = settings.SecondIsHuman;
            settings.SecondIsHuman = tempBool;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //GameCore<State, Turn, Round, Player>.TryRunAsSingleton(
            //    new Game(),
            //    new List<FormMainSettings> { settings });
            
        }

        public static IHubProxy hubProxy;
        int myIdOnServer = -1;
        bool ImAdmin = false;
        int uploadingFileIdOnServer = -1;
        byte[][] currentFile;
        int currentFilePart = 0;
        List<ServerGame> gameList;
        List<ServerPlayer> playerList;
        bool connectedToServer = false;
        public void ConnectToServer()
        {
            HubConnection hubConnection;

            hubConnection = new HubConnection(settings.ServerAddress);
            hubProxy = hubConnection.CreateHubProxy("MainHub");
            hubProxy.On("hello", () => Invoke(new Action(() => this.Text = "Success")));
            hubProxy.On("authorizeResult", new Action<int, bool>((myId, isAdmin) => { this.myIdOnServer = (int)myId; ImAdmin = isAdmin; this.Invoke(new Action(()=> SetAdminButtonsVisibility(isAdmin) ));}));
            hubProxy.On("fileId", (myFileId) => UploadFileToServer((int)myFileId));
            hubProxy.On("message", (text) => AddServerMessage(text));
            hubProxy.On("setRoomState", (x)=>{
                gameList = JsonConvert.DeserializeObject<List<ServerGame>>(x.gameList.ToString());
                playerList = JsonConvert.DeserializeObject<List<ServerPlayer>>(x.playerList.ToString());
                this.Invoke(new Action(() => this.RefreshGameFrid()));
            });
            hubProxy.On("roundFinished", new Action<int, int, dynamic>((gameId, roundNumber, x) => RoundFinished(gameId, roundNumber,JsonConvert.DeserializeObject<object>(x.ToString()))));
            hubProxy.On("runGameFromServer", (game) => RunGameFromServer(JsonConvert.DeserializeObject<ServerGame>(game.ToString())));
            //если тут вылез эксепшн, вероятно, Core.Config.serverAddress некорректен
            try
            {
                connectedToServer = true;
                hubConnection.Start().Wait();
            }
            catch (Exception ex)
            {
               // ZHelper.Log.LogOrThrow(ex);
            }
           // hubProxy.Invoke("Hello");

        }


        public void AddServerMessage(string text)
        {
            text = DateTime.Now.ToShortTimeString() + ": " + text;
            edtServerMessages.Invoke(new Action(() => 
                this.edtServerMessages.Text = text+ Environment.NewLine+ this.edtServerMessages.Text ));
        }
       

        private void btnRefreshServer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(settings.ServerLogin) || string.IsNullOrEmpty(settings.ServerPassword))
            {
                btmChangeLoginAndPassword_Click(null, null);
            }
            ConnectToServer();
            if(connectedToServer )
                hubProxy.Invoke("AuthorizeAndGetMyId", settings.ServerLogin, settings.ServerPassword );
        }

        

        private void btnFileToServerDialog_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                settings.FileToServer = openFileDialog1.FileName;
            }

        }

        private void UploadFileToServer(int myFielId)
        {
            this.uploadingFileIdOnServer = myFielId;
            for (int i = 0; i < currentFile.Length; i++)
            {
                if (connectedToServer)
                    hubProxy.Invoke("LoadFilePart", myFielId, i, currentFile[i]);
            }
        }

        private void btnUploadFileToServer_Click(object sender, EventArgs e)
        {
            if (myIdOnServer == -1)
            {
                MessageBox.Show("Сначала нажмите обновить");
                return;
            }

            if (File.Exists(settings.FileToServer))
            {
                currentFilePart = 0;
                var allBytes = File.ReadAllBytes(settings.FileToServer);
                int partCount = (int)Math.Ceiling((double)allBytes.Length / FrameworkSettings.InnerSettings.FileUploadBufferSize - 0.000000000001);
                currentFile = new byte[partCount][];
                for (int i = 0; i < allBytes.Length; i += FrameworkSettings.InnerSettings.FileUploadBufferSize)
                {
                    int partNumber = i / FrameworkSettings.InnerSettings.FileUploadBufferSize;
                    int size = Math.Min(FrameworkSettings.InnerSettings.FileUploadBufferSize, allBytes.Length - i);
                    currentFile[partNumber] = new byte[size];
                    Array.Copy(allBytes, i, currentFile[partNumber], 0, size);

                }

                if (connectedToServer)
                    hubProxy.Invoke("StartUploadingAndGetId", Path.GetFileName(settings.FileToServer), partCount);
            }
            else
                MessageBox.Show("Файл не найден");
        }

        private void btmChangeLoginAndPassword_Click(object sender, EventArgs e)
        {
            if (frmEnterLoginAndPassword.e.ShowDialog(settings.ServerLogin, settings.ServerPassword) == System.Windows.Forms.DialogResult.OK)
            {
                settings.ServerLogin = frmEnterLoginAndPassword.e.login;
                settings.ServerPassword = frmEnterLoginAndPassword.e.password;
            }
        }

        private void btnAddGames_Click(object sender, EventArgs e)
        {
            frmCreateGamesOnServer.e.ShowDialog(playerList.Select(x => Tuple.Create(x.Id, x.fileName, x)).ToList(), this);
        }

        private void edtRefreshRoom_Click(object sender, EventArgs e)
        {
            if (connectedToServer)
                hubProxy.Invoke("GetRoomState", 0);
            AddServerMessage("Обновление...");
           
        }

        void RefreshGameFrid()
        {
            grd.DataSource = gameList;
            grd.Refresh();


        }

        private void btnConnectToGame_Click(object sender, EventArgs e)
        {
            if(grd.SelectedRows.Count == 0)
                return;
            int id = (grd.SelectedRows[0].DataBoundItem as ServerGame).Id;
            serverGameId = id;
            roundsFromServer.Clear();
            if (connectedToServer)
                hubProxy.Invoke("ConnectToGame", id);
            AddServerMessage("Подключение к игре ...");
        }

        public void RunGameFromServer(ServerGame game)
        {
            game.StartSettings.JavaPath = settings.JavaPath;//todo javapath on client if needed !
            //GameCore<State, Turn, Round, Player>.TryRunAsSingleton(new Game(),new List<FormMainSettings>{ game.StartSettings}, roundsFromServer);
        }

        //todo remove played games
        int serverGameId = -1;
        ConcurrentDictionary<int, object> roundsFromServer = new ConcurrentDictionary<int, object>();
        void RoundFinished(int gameId, int roundNumber, object round)
        {
            if (gameId != serverGameId)
                return;
            roundsFromServer.TryAdd(roundNumber, round);
        }

        private void btnDeleteGame_Click(object sender, EventArgs e)
        {
            int id = (grd.SelectedRows[0].DataBoundItem as ServerGame).Id;
            if (connectedToServer)
                hubProxy.Invoke("RemoveGame", id);
        }

        
        
    }
}
