using Framework;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            GameCore<State, Turn, Round, Player>.TryRunAsSingleton(
                new Game(),
                new List<FormMainSettings> { settings });
            
        }

        IHubProxy hubProxy;
        int myIdOnServer = -1;
        int myFielId = -1;
        byte[][] currentFile;
        int currentFilePart = 0;
        public void ConnectToServer()
        {
            HubConnection hubConnection;

            hubConnection = new HubConnection(settings.ServerAddress);
            hubProxy = hubConnection.CreateHubProxy("MainHub");
            hubProxy.On("hello", () => Invoke(new Action(() => this.Text = "Success")));
            hubProxy.On("authorizeResult", (myId) => this.myIdOnServer = myId);
            hubProxy.On("fileId", (myFileId) => UploadFileToServer(myFielId));
            hubProxy.On("message", (text) => Invoke(new Action(() => this.edtServerMessages.Items.Add(text))));
            
            //если тут вылез эксепшн, вероятно, Core.Config.serverAddress некорректен
            hubConnection.Start().Wait();
           // hubProxy.Invoke("Hello");

        }

       

        private void btnRefreshServer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(settings.ServerLogin) || string.IsNullOrEmpty(settings.ServerPassword))
            {
                btmChangeLoginAndPassword_Click(null, null);
            }
            ConnectToServer();
            hubProxy.Invoke("AuthorizeAndGetMyId", settings.ServerLogin, settings.ServerPassword );
        }

        

        private void btnFileToServerDialog_Click(object sender, EventArgs e)
        {
            

        }

        private void UploadFileToServer(int myFielId)
        {
            this.myFielId = myFielId;
            for (int i = 0; i < currentFile.Length; i++)
            {
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
                int partCount = (int)Math.Ceiling((double)allBytes.Length / Vars.FileUploadBufferSize - 0.000000000001);
                currentFile = new byte[partCount][];
                for (int i = 0; i < allBytes.Length; i += Vars.FileUploadBufferSize)
                {
                    int partNumber = i / Vars.FileUploadBufferSize;
                    int size = Math.Min(Vars.FileUploadBufferSize, allBytes.Length - i);
                    currentFile[partNumber] = new byte[size];
                    Array.Copy(allBytes, i, currentFile[partNumber], 0, size);

                }

                hubProxy.Invoke("StartUploadingAndGetId", Path.GetFileName(settings.FileToServer), partCount);
            }
            else
                MessageBox.Show("Файл не найден");
        }

        private void btmChangeLoginAndPassword_Click(object sender, EventArgs e)
        {
            if (frmEnterLoginAndPassword.e.ShowDialog(settings.ServerLogin, settings.ServerPassword) == System.Windows.Forms.DialogResult.OK)
            {
                settings.ServerLogin = frmEnterLoginAndPassword.login;
                settings.ServerPassword = frmEnterLoginAndPassword.password;
            }
        }

        

        
    }
}
