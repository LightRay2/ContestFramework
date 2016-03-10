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
        public void ShowDialog(List<Tuple<int, string>> players, FormMain formMain)
        {
            this.formMain = formMain;
            edtTime.Text = DateTime.Now.ToString();
            edtPlayers.Items.Clear();
            players.ForEach(x => edtPlayers.Items.Add(x.Item2, false));
            playerIds = players.Select(x => x.Item1).ToList();
            this.ShowDialog();
        }

        private void btnAddGame_Click(object sender, EventArgs e)
        {
            List<int> selected = new List<int>();
            foreach(var item in edtPlayers.CheckedIndices)
                selected.Add(playerIds[ (int)item]);
            if(selected.Count == 0)
                return;
            DateTime date;
            if (!DateTime.TryParse(edtTime.Text, out date))
                return;

            FormMain.hubProxy.Invoke("AddGame", selected, date);
            formMain.AddServerMessage("Запрос на добавление игры...");
        }
    }
}
