namespace Turncoat.Server
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        this.components = new System.ComponentModel.Container();
        System.Windows.Forms.Label label1;
        System.Windows.Forms.Label label2;
        System.Windows.Forms.Panel panel1;
        this.stepsLogListBox = new System.Windows.Forms.ListBox();
        this.startStopGameButton = new System.Windows.Forms.Button();
        this.player1GroupBox = new System.Windows.Forms.GroupBox();
        this.player1SelectProgramCheckBox = new System.Windows.Forms.CheckBox();
        this.player1SelectProgramTextBox = new System.Windows.Forms.TextBox();
        this.player1SelectProgramButton = new System.Windows.Forms.Button();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.player2GroupBox = new System.Windows.Forms.GroupBox();
        this.player2SelectProgramCheckBox = new System.Windows.Forms.CheckBox();
        this.player2SelectProgramTextBox = new System.Windows.Forms.TextBox();
        this.player2SelectProgramButton = new System.Windows.Forms.Button();
        this.player1FirstStepRadioButton = new System.Windows.Forms.RadioButton();
        this.player2FirstStepRadioButton = new System.Windows.Forms.RadioButton();
        this.player1SelectProgramOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.player2SelectProgramOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.stepTimeHScrollBar = new System.Windows.Forms.HScrollBar();
        this.stepTimeLabel = new System.Windows.Forms.Label();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.selectGameConfigTextBox = new System.Windows.Forms.TextBox();
        this.selectGameConfigButton = new System.Windows.Forms.Button();
        this.selectGameConfigOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
        this.startGameOptionsPanel = new System.Windows.Forms.Panel();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
        this.pauseGameButton = new System.Windows.Forms.Button();
        this.label3 = new System.Windows.Forms.Label();
        this.betweenStepTimeHScrollBar = new System.Windows.Forms.HScrollBar();
        this.fieldPanel = new Turncoat.Server.Controls.DoubleBufferedPanel();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        panel1 = new System.Windows.Forms.Panel();
        panel1.SuspendLayout();
        this.player1GroupBox.SuspendLayout();
        this.menuStrip1.SuspendLayout();
        this.player2GroupBox.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.startGameOptionsPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
        this.SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(3, 491);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(80, 13);
        label1.TabIndex = 5;
        label1.Text = "Первым ходит";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 21);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(80, 13);
        label2.TabIndex = 11;
        label2.Text = "Конфигурация";
        // 
        // panel1
        // 
        panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        panel1.BackColor = System.Drawing.SystemColors.Control;
        panel1.Controls.Add(this.stepsLogListBox);
        panel1.Controls.Add(this.fieldPanel);
        panel1.Location = new System.Drawing.Point(381, 31);
        panel1.MaximumSize = new System.Drawing.Size(1100, 1000);
        panel1.Name = "panel1";
        panel1.Size = new System.Drawing.Size(714, 595);
        panel1.TabIndex = 13;
        // 
        // stepsLogListBox
        // 
        this.stepsLogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.stepsLogListBox.BackColor = System.Drawing.Color.DarkGray;
        this.stepsLogListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        this.stepsLogListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.stepsLogListBox.FormattingEnabled = true;
        this.stepsLogListBox.ItemHeight = 21;
        this.stepsLogListBox.Location = new System.Drawing.Point(481, 0);
        this.stepsLogListBox.Name = "stepsLogListBox";
        this.stepsLogListBox.ScrollAlwaysVisible = true;
        this.stepsLogListBox.Size = new System.Drawing.Size(230, 571);
        this.stepsLogListBox.TabIndex = 13;
        this.stepsLogListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.stepsLogListBox_DrawItem);
        // 
        // startStopGameButton
        // 
        this.startStopGameButton.Location = new System.Drawing.Point(3, 603);
        this.startStopGameButton.Name = "startStopGameButton";
        this.startStopGameButton.Size = new System.Drawing.Size(104, 23);
        this.startStopGameButton.TabIndex = 0;
        this.startStopGameButton.Text = "Новая игра";
        this.startStopGameButton.UseVisualStyleBackColor = true;
        this.startStopGameButton.Click += new System.EventHandler(this.startStopGameButton_Click);
        // 
        // player1GroupBox
        // 
        this.player1GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.player1GroupBox.Controls.Add(this.player1SelectProgramCheckBox);
        this.player1GroupBox.Controls.Add(this.player1SelectProgramTextBox);
        this.player1GroupBox.Controls.Add(this.player1SelectProgramButton);
        this.player1GroupBox.Location = new System.Drawing.Point(3, 301);
        this.player1GroupBox.Name = "player1GroupBox";
        this.player1GroupBox.Size = new System.Drawing.Size(375, 79);
        this.player1GroupBox.TabIndex = 2;
        this.player1GroupBox.TabStop = false;
        this.player1GroupBox.Text = "Игрок 1";
        this.player1GroupBox.Paint += new System.Windows.Forms.PaintEventHandler(this.player1GroupBox_Paint);
        // 
        // player1SelectProgramCheckBox
        // 
        this.player1SelectProgramCheckBox.AutoSize = true;
        this.player1SelectProgramCheckBox.Location = new System.Drawing.Point(86, 23);
        this.player1SelectProgramCheckBox.Name = "player1SelectProgramCheckBox";
        this.player1SelectProgramCheckBox.Size = new System.Drawing.Size(85, 17);
        this.player1SelectProgramCheckBox.TabIndex = 8;
        this.player1SelectProgramCheckBox.Text = "Программа";
        this.player1SelectProgramCheckBox.UseVisualStyleBackColor = true;
        this.player1SelectProgramCheckBox.CheckedChanged += new System.EventHandler(this.player1ProgramCheckBox_CheckedChanged);
        // 
        // player1SelectProgramTextBox
        // 
        this.player1SelectProgramTextBox.Enabled = false;
        this.player1SelectProgramTextBox.Location = new System.Drawing.Point(86, 53);
        this.player1SelectProgramTextBox.Name = "player1SelectProgramTextBox";
        this.player1SelectProgramTextBox.Size = new System.Drawing.Size(283, 20);
        this.player1SelectProgramTextBox.TabIndex = 3;
        // 
        // player1SelectProgramButton
        // 
        this.player1SelectProgramButton.Enabled = false;
        this.player1SelectProgramButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.player1SelectProgramButton.Location = new System.Drawing.Point(184, 19);
        this.player1SelectProgramButton.Name = "player1SelectProgramButton";
        this.player1SelectProgramButton.Size = new System.Drawing.Size(30, 23);
        this.player1SelectProgramButton.TabIndex = 2;
        this.player1SelectProgramButton.Text = "...";
        this.player1SelectProgramButton.UseVisualStyleBackColor = true;
        this.player1SelectProgramButton.Click += new System.EventHandler(this.player1SelectProgramButton_Click);
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Size = new System.Drawing.Size(1107, 24);
        this.menuStrip1.TabIndex = 3;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // aboutToolStripMenuItem
        // 
        this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        this.aboutToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
        this.aboutToolStripMenuItem.Text = "О программе";
        this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
        // 
        // player2GroupBox
        // 
        this.player2GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.player2GroupBox.Controls.Add(this.player2SelectProgramCheckBox);
        this.player2GroupBox.Controls.Add(this.player2SelectProgramTextBox);
        this.player2GroupBox.Controls.Add(this.player2SelectProgramButton);
        this.player2GroupBox.Location = new System.Drawing.Point(3, 386);
        this.player2GroupBox.Name = "player2GroupBox";
        this.player2GroupBox.Size = new System.Drawing.Size(375, 83);
        this.player2GroupBox.TabIndex = 4;
        this.player2GroupBox.TabStop = false;
        this.player2GroupBox.Text = "Игрок 2";
        this.player2GroupBox.Paint += new System.Windows.Forms.PaintEventHandler(this.player2GroupBox_Paint);
        // 
        // player2SelectProgramCheckBox
        // 
        this.player2SelectProgramCheckBox.AutoSize = true;
        this.player2SelectProgramCheckBox.Location = new System.Drawing.Point(86, 18);
        this.player2SelectProgramCheckBox.Name = "player2SelectProgramCheckBox";
        this.player2SelectProgramCheckBox.Size = new System.Drawing.Size(85, 17);
        this.player2SelectProgramCheckBox.TabIndex = 9;
        this.player2SelectProgramCheckBox.Text = "Программа";
        this.player2SelectProgramCheckBox.UseVisualStyleBackColor = true;
        this.player2SelectProgramCheckBox.CheckStateChanged += new System.EventHandler(this.player2ProgramCheckBox_CheckedChanged);
        // 
        // player2SelectProgramTextBox
        // 
        this.player2SelectProgramTextBox.Enabled = false;
        this.player2SelectProgramTextBox.Location = new System.Drawing.Point(86, 43);
        this.player2SelectProgramTextBox.Name = "player2SelectProgramTextBox";
        this.player2SelectProgramTextBox.Size = new System.Drawing.Size(283, 20);
        this.player2SelectProgramTextBox.TabIndex = 3;
        // 
        // player2SelectProgramButton
        // 
        this.player2SelectProgramButton.Enabled = false;
        this.player2SelectProgramButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.player2SelectProgramButton.Location = new System.Drawing.Point(184, 14);
        this.player2SelectProgramButton.Name = "player2SelectProgramButton";
        this.player2SelectProgramButton.Size = new System.Drawing.Size(30, 23);
        this.player2SelectProgramButton.TabIndex = 2;
        this.player2SelectProgramButton.Text = "...";
        this.player2SelectProgramButton.UseVisualStyleBackColor = true;
        this.player2SelectProgramButton.Click += new System.EventHandler(this.player2SelectProgramButton_Click);
        // 
        // player1FirstStepRadioButton
        // 
        this.player1FirstStepRadioButton.AutoSize = true;
        this.player1FirstStepRadioButton.Checked = true;
        this.player1FirstStepRadioButton.Location = new System.Drawing.Point(89, 489);
        this.player1FirstStepRadioButton.Name = "player1FirstStepRadioButton";
        this.player1FirstStepRadioButton.Size = new System.Drawing.Size(65, 17);
        this.player1FirstStepRadioButton.TabIndex = 6;
        this.player1FirstStepRadioButton.TabStop = true;
        this.player1FirstStepRadioButton.Text = "Игрок 1";
        this.player1FirstStepRadioButton.UseVisualStyleBackColor = true;
        // 
        // player2FirstStepRadioButton
        // 
        this.player2FirstStepRadioButton.AutoSize = true;
        this.player2FirstStepRadioButton.Location = new System.Drawing.Point(160, 489);
        this.player2FirstStepRadioButton.Name = "player2FirstStepRadioButton";
        this.player2FirstStepRadioButton.Size = new System.Drawing.Size(65, 17);
        this.player2FirstStepRadioButton.TabIndex = 7;
        this.player2FirstStepRadioButton.Text = "Игрок 2";
        this.player2FirstStepRadioButton.UseVisualStyleBackColor = true;
        // 
        // player1SelectProgramOpenFileDialog
        // 
        this.player1SelectProgramOpenFileDialog.DefaultExt = "exe";
        this.player1SelectProgramOpenFileDialog.Filter = "Испольняемые файлы (*.exe)|*.exe";
        // 
        // player2SelectProgramOpenFileDialog
        // 
        this.player2SelectProgramOpenFileDialog.DefaultExt = "exe";
        this.player2SelectProgramOpenFileDialog.Filter = "Испольняемые файлы (*.exe)|*.exe";
        // 
        // stepTimeHScrollBar
        // 
        this.stepTimeHScrollBar.LargeChange = 4;
        this.stepTimeHScrollBar.Location = new System.Drawing.Point(172, 544);
        this.stepTimeHScrollBar.Maximum = 103;
        this.stepTimeHScrollBar.Minimum = 10;
        this.stepTimeHScrollBar.Name = "stepTimeHScrollBar";
        this.stepTimeHScrollBar.Size = new System.Drawing.Size(206, 16);
        this.stepTimeHScrollBar.TabIndex = 8;
        this.stepTimeHScrollBar.Value = 40;
        this.stepTimeHScrollBar.ValueChanged += new System.EventHandler(this.stepTimeHScrollBar_ValueChanged);
        // 
        // stepTimeLabel
        // 
        this.stepTimeLabel.AutoSize = true;
        this.stepTimeLabel.Location = new System.Drawing.Point(11, 547);
        this.stepTimeLabel.Name = "stepTimeLabel";
        this.stepTimeLabel.Size = new System.Drawing.Size(75, 13);
        this.stepTimeLabel.TabIndex = 9;
        this.stepTimeLabel.Text = "Время на ход";
        this.stepTimeLabel.Click += new System.EventHandler(this.stepTimeLabel_Click);
        // 
        // groupBox1
        // 
        this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.groupBox1.Controls.Add(label2);
        this.groupBox1.Controls.Add(this.selectGameConfigTextBox);
        this.groupBox1.Controls.Add(this.selectGameConfigButton);
        this.groupBox1.Location = new System.Drawing.Point(0, 4);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(375, 82);
        this.groupBox1.TabIndex = 10;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Игра";
        // 
        // selectGameConfigTextBox
        // 
        this.selectGameConfigTextBox.Location = new System.Drawing.Point(15, 56);
        this.selectGameConfigTextBox.Name = "selectGameConfigTextBox";
        this.selectGameConfigTextBox.Size = new System.Drawing.Size(354, 20);
        this.selectGameConfigTextBox.TabIndex = 3;
        this.selectGameConfigTextBox.TextChanged += new System.EventHandler(this.selectGameConfigTextBox_TextChanged);
        // 
        // selectGameConfigButton
        // 
        this.selectGameConfigButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.selectGameConfigButton.Location = new System.Drawing.Point(147, 21);
        this.selectGameConfigButton.Name = "selectGameConfigButton";
        this.selectGameConfigButton.Size = new System.Drawing.Size(30, 23);
        this.selectGameConfigButton.TabIndex = 2;
        this.selectGameConfigButton.Text = "...";
        this.selectGameConfigButton.UseVisualStyleBackColor = true;
        this.selectGameConfigButton.Click += new System.EventHandler(this.selectGameConfigButton_Click);
        // 
        // selectGameConfigOpenFileDialog
        // 
        this.selectGameConfigOpenFileDialog.DefaultExt = "cfg";
        this.selectGameConfigOpenFileDialog.Filter = "Конфигурационные файлы (*.cfg)|*.cfg|Текстовые файлы (*.txt)|*.txt|Все файлы (*.*" +
            ")|*.*";
        // 
        // startGameOptionsPanel
        // 
        this.startGameOptionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.startGameOptionsPanel.BackColor = System.Drawing.SystemColors.Control;
        this.startGameOptionsPanel.Controls.Add(this.textBox1);
        this.startGameOptionsPanel.Controls.Add(this.groupBox1);
        this.startGameOptionsPanel.Controls.Add(this.player1GroupBox);
        this.startGameOptionsPanel.Controls.Add(this.player2GroupBox);
        this.startGameOptionsPanel.Controls.Add(this.player2FirstStepRadioButton);
        this.startGameOptionsPanel.Controls.Add(label1);
        this.startGameOptionsPanel.Controls.Add(this.player1FirstStepRadioButton);
        this.startGameOptionsPanel.Location = new System.Drawing.Point(0, 27);
        this.startGameOptionsPanel.MaximumSize = new System.Drawing.Size(1100, 1000);
        this.startGameOptionsPanel.Name = "startGameOptionsPanel";
        this.startGameOptionsPanel.Size = new System.Drawing.Size(375, 514);
        this.startGameOptionsPanel.TabIndex = 11;
        // 
        // textBox1
        // 
        this.textBox1.Font = new System.Drawing.Font("Lucida Console", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        this.textBox1.Location = new System.Drawing.Point(9, 92);
        this.textBox1.Multiline = true;
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(363, 206);
        this.textBox1.TabIndex = 18;
        // 
        // pauseGameButton
        // 
        this.pauseGameButton.Location = new System.Drawing.Point(113, 603);
        this.pauseGameButton.Name = "pauseGameButton";
        this.pauseGameButton.Size = new System.Drawing.Size(104, 23);
        this.pauseGameButton.TabIndex = 17;
        this.pauseGameButton.Text = "Запустить";
        this.pauseGameButton.UseVisualStyleBackColor = true;
        this.pauseGameButton.Click += new System.EventHandler(this.pauseGameButton_Click);
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(11, 574);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(116, 13);
        this.label3.TabIndex = 19;
        this.label3.Text = "Время между ходами";
        // 
        // betweenStepTimeHScrollBar
        // 
        this.betweenStepTimeHScrollBar.LargeChange = 4;
        this.betweenStepTimeHScrollBar.Location = new System.Drawing.Point(172, 571);
        this.betweenStepTimeHScrollBar.Maximum = 93;
        this.betweenStepTimeHScrollBar.Minimum = 1;
        this.betweenStepTimeHScrollBar.Name = "betweenStepTimeHScrollBar";
        this.betweenStepTimeHScrollBar.Size = new System.Drawing.Size(206, 16);
        this.betweenStepTimeHScrollBar.TabIndex = 18;
        this.betweenStepTimeHScrollBar.Value = 2;
        // 
        // fieldPanel
        // 
        this.fieldPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.fieldPanel.Location = new System.Drawing.Point(3, 0);
        this.fieldPanel.Name = "fieldPanel";
        this.fieldPanel.Size = new System.Drawing.Size(458, 595);
        this.fieldPanel.TabIndex = 12;
        this.fieldPanel.Text = "doubleBufferedPanel1";
        this.fieldPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.fieldPanel_Paint);
        this.fieldPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fieldPanel_MouseDown);
        this.fieldPanel.Resize += new System.EventHandler(this.fieldPanel_Resize);
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.Control;
        this.ClientSize = new System.Drawing.Size(1107, 638);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.betweenStepTimeHScrollBar);
        this.Controls.Add(this.pauseGameButton);
        this.Controls.Add(panel1);
        this.Controls.Add(this.startGameOptionsPanel);
        this.Controls.Add(this.stepTimeLabel);
        this.Controls.Add(this.stepTimeHScrollBar);
        this.Controls.Add(this.startStopGameButton);
        this.Controls.Add(this.menuStrip1);
        this.DoubleBuffered = true;
        this.MainMenuStrip = this.menuStrip1;
        this.Name = "MainForm";
        this.Text = "Черные vs Белые";
        this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
        this.Load += new System.EventHandler(this.MainForm_Load);
        panel1.ResumeLayout(false);
        this.player1GroupBox.ResumeLayout(false);
        this.player1GroupBox.PerformLayout();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.player2GroupBox.ResumeLayout(false);
        this.player2GroupBox.PerformLayout();
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.startGameOptionsPanel.ResumeLayout(false);
        this.startGameOptionsPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button startStopGameButton;
    private System.Windows.Forms.GroupBox player1GroupBox;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.TextBox player1SelectProgramTextBox;
    private System.Windows.Forms.Button player1SelectProgramButton;
    private System.Windows.Forms.GroupBox player2GroupBox;
    private System.Windows.Forms.TextBox player2SelectProgramTextBox;
    private System.Windows.Forms.Button player2SelectProgramButton;
    private System.Windows.Forms.BindingSource bindingSource1;
    private System.Windows.Forms.RadioButton player1FirstStepRadioButton;
    private System.Windows.Forms.RadioButton player2FirstStepRadioButton;
    private System.Windows.Forms.CheckBox player1SelectProgramCheckBox;
    private System.Windows.Forms.CheckBox player2SelectProgramCheckBox;
    private System.Windows.Forms.OpenFileDialog player1SelectProgramOpenFileDialog;
    private System.Windows.Forms.OpenFileDialog player2SelectProgramOpenFileDialog;
    private System.Windows.Forms.HScrollBar stepTimeHScrollBar;
    private System.Windows.Forms.Label stepTimeLabel;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox selectGameConfigTextBox;
    private System.Windows.Forms.Button selectGameConfigButton;
    private System.Windows.Forms.OpenFileDialog selectGameConfigOpenFileDialog;
    private System.Windows.Forms.Panel startGameOptionsPanel;
    private Controls.DoubleBufferedPanel fieldPanel;
    private System.Windows.Forms.ListBox stepsLogListBox;
    private System.Windows.Forms.Button pauseGameButton;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.HScrollBar betweenStepTimeHScrollBar;
  }
}

