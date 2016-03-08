using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Framework.Opengl;

namespace Framework
{
    public partial class Form1 : Form
    {
        Func<IGetKeyboardState, Frame> _processMethod;
        public Form1(Func<IGetKeyboardState, Frame> processMethod)
        {
            _processMethod = processMethod;
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
            MainController _mainController = new MainController(
                _processMethod, w, h);
            _mainController.SetMainLoop();
        }


    }
}
