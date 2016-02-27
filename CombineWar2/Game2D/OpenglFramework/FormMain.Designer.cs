namespace MagicStorm
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.cbPlayer1 = new System.Windows.Forms.CheckBox();
            this.edtPlayer1 = new System.Windows.Forms.TextBox();
            this.edtPlayer2 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnPlayer1 = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.cbPlayer2 = new System.Windows.Forms.CheckBox();
            this.btnPlayer2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.управлениеСКлавиатурыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnAddReplay = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(751, 371);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(355, 71);
            this.button1.TabIndex = 0;
            this.button1.Text = "Запустить игру";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbPlayer1
            // 
            this.cbPlayer1.AutoSize = true;
            this.cbPlayer1.Location = new System.Drawing.Point(33, 131);
            this.cbPlayer1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbPlayer1.Name = "cbPlayer1";
            this.cbPlayer1.Size = new System.Drawing.Size(94, 24);
            this.cbPlayer1.TabIndex = 1;
            this.cbPlayer1.Text = "Человек";
            this.cbPlayer1.UseVisualStyleBackColor = true;
            this.cbPlayer1.CheckedChanged += new System.EventHandler(this.cbPlayer1_CheckedChanged);
            // 
            // edtPlayer1
            // 
            this.edtPlayer1.Location = new System.Drawing.Point(177, 129);
            this.edtPlayer1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.edtPlayer1.Name = "edtPlayer1";
            this.edtPlayer1.Size = new System.Drawing.Size(857, 26);
            this.edtPlayer1.TabIndex = 3;
            // 
            // edtPlayer2
            // 
            this.edtPlayer2.Location = new System.Drawing.Point(177, 252);
            this.edtPlayer2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.edtPlayer2.Name = "edtPlayer2";
            this.edtPlayer2.Size = new System.Drawing.Size(857, 26);
            this.edtPlayer2.TabIndex = 4;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Исполняемые файлы|*.exe;*.jar";
            // 
            // btnPlayer1
            // 
            this.btnPlayer1.Location = new System.Drawing.Point(1061, 128);
            this.btnPlayer1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnPlayer1.Name = "btnPlayer1";
            this.btnPlayer1.Size = new System.Drawing.Size(44, 36);
            this.btnPlayer1.TabIndex = 5;
            this.btnPlayer1.Text = "...";
            this.btnPlayer1.UseVisualStyleBackColor = true;
            this.btnPlayer1.Click += new System.EventHandler(this.btnPlayer1_Click);
            // 
            // btnExchange
            // 
            this.btnExchange.Image = global::Game2D.Properties.Resources.icon0;
            this.btnExchange.Location = new System.Drawing.Point(468, 181);
            this.btnExchange.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(59, 49);
            this.btnExchange.TabIndex = 7;
            this.btnExchange.UseVisualStyleBackColor = true;
            this.btnExchange.Click += new System.EventHandler(this.btnExchange_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(290, 334);
            this.lblTime.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(50, 20);
            this.lblTime.TabIndex = 8;
            this.lblTime.Text = "100%";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 72);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Выберите игроков или";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(42, 375);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.trackBar1.Maximum = 20;
            this.trackBar1.Minimum = 3;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(322, 45);
            this.trackBar1.TabIndex = 13;
            this.trackBar1.Value = 10;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 334);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Время анимации:";
            // 
            // cbPlayer2
            // 
            this.cbPlayer2.AutoSize = true;
            this.cbPlayer2.Location = new System.Drawing.Point(33, 254);
            this.cbPlayer2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbPlayer2.Name = "cbPlayer2";
            this.cbPlayer2.Size = new System.Drawing.Size(94, 24);
            this.cbPlayer2.TabIndex = 15;
            this.cbPlayer2.Text = "Человек";
            this.cbPlayer2.UseVisualStyleBackColor = true;
            this.cbPlayer2.CheckedChanged += new System.EventHandler(this.cbPlayer2_CheckedChanged);
            // 
            // btnPlayer2
            // 
            this.btnPlayer2.Location = new System.Drawing.Point(1061, 251);
            this.btnPlayer2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnPlayer2.Name = "btnPlayer2";
            this.btnPlayer2.Size = new System.Drawing.Size(44, 36);
            this.btnPlayer2.TabIndex = 16;
            this.btnPlayer2.Text = "...";
            this.btnPlayer2.UseVisualStyleBackColor = true;
            this.btnPlayer2.Click += new System.EventHandler(this.btnPlayer2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(751, 325);
            this.button3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(302, 38);
            this.button3.TabIndex = 22;
            this.button3.Text = "Добавить в список игр (0)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1063, 325);
            this.button4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(43, 38);
            this.button4.TabIndex = 23;
            this.button4.Text = "-";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.управлениеСКлавиатурыToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1144, 29);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // управлениеСКлавиатурыToolStripMenuItem
            // 
            this.управлениеСКлавиатурыToolStripMenuItem.Name = "управлениеСКлавиатурыToolStripMenuItem";
            this.управлениеСКлавиатурыToolStripMenuItem.Size = new System.Drawing.Size(215, 25);
            this.управлениеСКлавиатурыToolStripMenuItem.Text = "Управление с клавиатуры...";
            this.управлениеСКлавиатурыToolStripMenuItem.Click += new System.EventHandler(this.управлениеСКлавиатурыToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.оПрограммеToolStripMenuItem.Text = "О программе...";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // btnAddReplay
            // 
            this.btnAddReplay.Location = new System.Drawing.Point(220, 68);
            this.btnAddReplay.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAddReplay.Name = "btnAddReplay";
            this.btnAddReplay.Size = new System.Drawing.Size(302, 29);
            this.btnAddReplay.TabIndex = 26;
            this.btnAddReplay.Text = "Добавьте файл с повтором игры";
            this.btnAddReplay.UseVisualStyleBackColor = true;
            this.btnAddReplay.Click += new System.EventHandler(this.btnAddReplay_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1144, 469);
            this.Controls.Add(this.btnAddReplay);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnPlayer2);
            this.Controls.Add(this.cbPlayer2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.btnExchange);
            this.Controls.Add(this.btnPlayer1);
            this.Controls.Add(this.edtPlayer2);
            this.Controls.Add(this.edtPlayer1);
            this.Controls.Add(this.cbPlayer1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FormMain";
            this.Text = "ContestAI";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbPlayer1;
        private System.Windows.Forms.TextBox edtPlayer1;
        private System.Windows.Forms.TextBox edtPlayer2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnPlayer1;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbPlayer2;
        private System.Windows.Forms.Button btnPlayer2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem управлениеСКлавиатурыToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Button btnAddReplay;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
    }
}