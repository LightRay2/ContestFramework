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
        Func<GlInput, Frame> _processMethod;
        public GameForm(Func<GlInput, Frame> processMethod)
        {
            
            _processMethod = processMethod;
            
            InitializeComponent();
            if (!DesignMode)
            {
                this.glControl1 = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 8, 4), 1, 0, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);
                this.glControl1.BackColor = System.Drawing.Color.Black;
                this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
               // this.glControl1.Location = new System.Drawing.Point(0, 25);
               // this.glControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
                this.glControl1.Name = "glControl1";
              //  this.glControl1.Size = new System.Drawing.Size(928, 675);
                this.glControl1.TabIndex = 0;
                this.glControl1.VSync = false;
                this.Controls.Add(this.glControl1);
            }
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
