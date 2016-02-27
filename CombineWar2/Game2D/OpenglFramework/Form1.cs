using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game2D.Opengl;
using Game2D.Game;

namespace Game2D
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CGL.InitializeContexts();
            InputLanguage.CurrentInputLanguage
                = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var screen = Screen.AllScreens;
            Rectangle monitorSize = screen[0].WorkingArea;
            int h = monitorSize.Bottom - (this.Size.Height - this.ClientRectangle.Height);
            int w = Math.Min( h*4/3,
                monitorSize.Right - (this.Size.Width - this.ClientRectangle.Width)); 
            MainController _mainController = new MainController(new 
                Game2D.Game.Game(), w, h);
            _mainController.SetMainLoop();
        }


    }
}
