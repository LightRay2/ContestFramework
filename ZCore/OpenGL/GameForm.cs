using Framework.Opengl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Framework
{
    public partial class GameForm : Form
    {
        Func<IGetKeyboardState, Frame> _processMethod;
        public GameForm(Func<IGetKeyboardState, Frame> processMethod)
        {
            
            _processMethod = processMethod;
            InitializeComponent();
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

             GameController _mainController = new GameController(glControl1,
                _processMethod, this);
        }


    }
}
