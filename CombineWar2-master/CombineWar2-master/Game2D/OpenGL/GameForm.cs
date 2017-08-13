
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Game2D
{
    public partial class GameForm : Form
    {
        public static bool UserWantsToClose = false;
        public static bool GameInSecondThreadIsRunning = false;
        public GameForm()
        {
            
            InitializeComponent();
        }

        //чтобы стрелки работали
        protected override bool ProcessDialogKey(Keys keyData)
        {
            return false;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            var screen = Screen.AllScreens;
            Rectangle monitorSize = screen[0].WorkingArea;
            int h = monitorSize.Bottom ;
            int w = Math.Min(h * 4 / 3,
                monitorSize.Right );

            this.Location = new Point();
            this.Size = new Size(w, h);

             GameController _mainController = new GameController(glControl1, this);
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!UserWantsToClose)
            {
                UserWantsToClose = true;
                e.Cancel = true;
            }
        }


    }
}
