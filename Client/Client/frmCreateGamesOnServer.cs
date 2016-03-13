using Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class frmCreateGamesOnServer : Form
    {
        static frmCreateGamesOnServer _instance;
        public static frmCreateGamesOnServer e
        {
            get
            {
                if (_instance == null)
                    _instance = new frmCreateGamesOnServer();
                return _instance;
            }

        }
         frmCreateGamesOnServer()
        {
            InitializeComponent();
        }

        FormMain formMain;
        List<int> playerIds;
        List<ServerPlayer> serverPlayers;
        public void ShowDialog(List<Tuple<int, string, ServerPlayer>> players, FormMain formMain)
        {
            this.formMain = formMain;
            edtTime.Text = DateTime.Now.ToString();
            edtPlayers.Items.Clear();
            players.ForEach(x => edtPlayers.Items.Add(x.Item2, false));
            playerIds = players.Select(x => x.Item1).ToList();
            serverPlayers = players.Select(x=>x.Item3).ToList();
            this.ShowDialog();
        }

        private void btnAddGame_Click(object sender, EventArgs e)
        {
            
            List<int> selected = new List<int>();
            foreach(var item in edtPlayers.CheckedIndices)
                selected.Add(playerIds[ (int)item]);
            
            if(selected.Count != 2){
                MessageBox.Show("Для этой игры нужно 2 игрока");

            }
            if(selected.Count == 0)
                return;
            DateTime date;
            if (!DateTime.TryParse(edtTime.Text, out date))
                return;

            FormMain.hubProxy.Invoke("AddGame", selected, date, 
                new FormMainSettings{
                    FirstProgram = serverPlayers.First(x=>x.Id == selected[0]).physicalFileName,
                    SecondProgram = serverPlayers.First(x=>x.Id == selected[1]).physicalFileName    
                }
            );
            formMain.AddServerMessage("Запрос на добавление игры...");
        }
    }
}
