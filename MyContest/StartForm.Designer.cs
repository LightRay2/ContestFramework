namespace MyContest
{
    partial class StartForm
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLocalGames = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPlayers = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddHuman = new System.Windows.Forms.Button();
            this.btnAddProgram = new System.Windows.Forms.Button();
            this.labelScoreWithBots = new System.Windows.Forms.Label();
            this.edtFixedRandomSeed = new System.Windows.Forms.NumericUpDown();
            this.edtUseFixedRandomSeed = new System.Windows.Forms.CheckBox();
            this.btnChangePythonPath = new System.Windows.Forms.Button();
            this.btnChangeJavaPath = new System.Windows.Forms.Button();
            this.btnChangeOrder = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRun = new System.Windows.Forms.Button();
            this.panelPlayersInMatch = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.tabReplays = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRunReplay = new System.Windows.Forms.Button();
            this.btnSelectReplayFolder = new System.Windows.Forms.Button();
            this.edtSaveReplays = new System.Windows.Forms.CheckBox();
            this.edtReplayFolder = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabHelp = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabLocalGames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtFixedRandomSeed)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabReplays.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabHelp.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLocalGames);
            this.tabControl1.Controls.Add(this.tabReplays);
            this.tabControl1.Controls.Add(this.tabHelp);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(906, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // tabLocalGames
            // 
            this.tabLocalGames.AutoScroll = true;
            this.tabLocalGames.BackColor = System.Drawing.Color.Transparent;
            this.tabLocalGames.Controls.Add(this.splitContainer1);
            this.tabLocalGames.Location = new System.Drawing.Point(4, 27);
            this.tabLocalGames.Margin = new System.Windows.Forms.Padding(4);
            this.tabLocalGames.Name = "tabLocalGames";
            this.tabLocalGames.Padding = new System.Windows.Forms.Padding(4);
            this.tabLocalGames.Size = new System.Drawing.Size(898, 518);
            this.tabLocalGames.TabIndex = 0;
            this.tabLocalGames.Text = "Запустить игру";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddHuman);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddProgram);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.labelScoreWithBots);
            this.splitContainer1.Panel2.Controls.Add(this.edtFixedRandomSeed);
            this.splitContainer1.Panel2.Controls.Add(this.edtUseFixedRandomSeed);
            this.splitContainer1.Panel2.Controls.Add(this.btnChangePythonPath);
            this.splitContainer1.Panel2.Controls.Add(this.btnChangeJavaPath);
            this.splitContainer1.Panel2.Controls.Add(this.btnChangeOrder);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panelPlayersInMatch);
            this.splitContainer1.Panel2.Controls.Add(this.btnClearSelection);
            this.splitContainer1.Size = new System.Drawing.Size(890, 510);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelPlayers);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 444);
            this.panel1.TabIndex = 2;
            // 
            // panelPlayers
            // 
            this.panelPlayers.AutoScroll = true;
            this.panelPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayers.Location = new System.Drawing.Point(0, 18);
            this.panelPlayers.Name = "panelPlayers";
            this.panelPlayers.Size = new System.Drawing.Size(276, 426);
            this.panelPlayers.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "             ";
            // 
            // btnAddHuman
            // 
            this.btnAddHuman.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddHuman.Location = new System.Drawing.Point(0, 31);
            this.btnAddHuman.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.btnAddHuman.Name = "btnAddHuman";
            this.btnAddHuman.Size = new System.Drawing.Size(276, 31);
            this.btnAddHuman.TabIndex = 1;
            this.btnAddHuman.Text = "Добавить человека";
            this.btnAddHuman.UseVisualStyleBackColor = true;
            this.btnAddHuman.Visible = false;
            this.btnAddHuman.Click += new System.EventHandler(this.btnAddHuman_Click);
            // 
            // btnAddProgram
            // 
            this.btnAddProgram.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddProgram.Location = new System.Drawing.Point(0, 0);
            this.btnAddProgram.Name = "btnAddProgram";
            this.btnAddProgram.Size = new System.Drawing.Size(276, 31);
            this.btnAddProgram.TabIndex = 0;
            this.btnAddProgram.Text = "Добавить программу...";
            this.btnAddProgram.UseVisualStyleBackColor = true;
            this.btnAddProgram.Click += new System.EventHandler(this.btnAddProgram_Click);
            // 
            // labelScoreWithBots
            // 
            this.labelScoreWithBots.AutoSize = true;
            this.labelScoreWithBots.Location = new System.Drawing.Point(10, 259);
            this.labelScoreWithBots.Name = "labelScoreWithBots";
            this.labelScoreWithBots.Size = new System.Drawing.Size(28, 18);
            this.labelScoreWithBots.TabIndex = 9;
            this.labelScoreWithBots.Text = "----";
            // 
            // edtFixedRandomSeed
            // 
            this.edtFixedRandomSeed.Location = new System.Drawing.Point(349, 221);
            this.edtFixedRandomSeed.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.edtFixedRandomSeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.edtFixedRandomSeed.Name = "edtFixedRandomSeed";
            this.edtFixedRandomSeed.Size = new System.Drawing.Size(120, 24);
            this.edtFixedRandomSeed.TabIndex = 8;
            this.edtFixedRandomSeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // edtUseFixedRandomSeed
            // 
            this.edtUseFixedRandomSeed.AutoSize = true;
            this.edtUseFixedRandomSeed.Location = new System.Drawing.Point(3, 223);
            this.edtUseFixedRandomSeed.Name = "edtUseFixedRandomSeed";
            this.edtUseFixedRandomSeed.Size = new System.Drawing.Size(340, 22);
            this.edtUseFixedRandomSeed.TabIndex = 7;
            this.edtUseFixedRandomSeed.Text = "Использовать фиксированный random seed:";
            this.edtUseFixedRandomSeed.UseVisualStyleBackColor = true;
            // 
            // btnChangePythonPath
            // 
            this.btnChangePythonPath.Location = new System.Drawing.Point(13, 449);
            this.btnChangePythonPath.Name = "btnChangePythonPath";
            this.btnChangePythonPath.Size = new System.Drawing.Size(226, 35);
            this.btnChangePythonPath.TabIndex = 6;
            this.btnChangePythonPath.Text = "Изменить путь до python.exe";
            this.btnChangePythonPath.UseVisualStyleBackColor = true;
            this.btnChangePythonPath.Visible = false;
            this.btnChangePythonPath.Click += new System.EventHandler(this.btnChangePythonPath_Click);
            // 
            // btnChangeJavaPath
            // 
            this.btnChangeJavaPath.Location = new System.Drawing.Point(3, 166);
            this.btnChangeJavaPath.Name = "btnChangeJavaPath";
            this.btnChangeJavaPath.Size = new System.Drawing.Size(186, 35);
            this.btnChangeJavaPath.TabIndex = 2;
            this.btnChangeJavaPath.Text = "Изменить путь до Java";
            this.btnChangeJavaPath.UseVisualStyleBackColor = true;
            this.btnChangeJavaPath.Visible = false;
            this.btnChangeJavaPath.Click += new System.EventHandler(this.btnChangeJavaPath_Click);
            // 
            // btnChangeOrder
            // 
            this.btnChangeOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeOrder.Location = new System.Drawing.Point(167, 166);
            this.btnChangeOrder.Name = "btnChangeOrder";
            this.btnChangeOrder.Size = new System.Drawing.Size(192, 35);
            this.btnChangeOrder.TabIndex = 4;
            this.btnChangeOrder.Text = "Изменить порядок";
            this.btnChangeOrder.UseVisualStyleBackColor = true;
            this.btnChangeOrder.Click += new System.EventHandler(this.btnChangeOrder_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.btnRun);
            this.panel2.Location = new System.Drawing.Point(306, 419);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(248, 69);
            this.panel2.TabIndex = 3;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(3, 3);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(242, 62);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "НАЧАТЬ МАТЧ";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // panelPlayersInMatch
            // 
            this.panelPlayersInMatch.AutoScroll = true;
            this.panelPlayersInMatch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPlayersInMatch.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelPlayersInMatch.Location = new System.Drawing.Point(0, 0);
            this.panelPlayersInMatch.Name = "panelPlayersInMatch";
            this.panelPlayersInMatch.Size = new System.Drawing.Size(598, 160);
            this.panelPlayersInMatch.TabIndex = 0;
            this.panelPlayersInMatch.WrapContents = false;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSelection.Location = new System.Drawing.Point(365, 166);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(189, 35);
            this.btnClearSelection.TabIndex = 5;
            this.btnClearSelection.Text = "Очистить выбор";
            this.btnClearSelection.UseVisualStyleBackColor = true;
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // tabReplays
            // 
            this.tabReplays.Controls.Add(this.panel4);
            this.tabReplays.Location = new System.Drawing.Point(4, 27);
            this.tabReplays.Name = "tabReplays";
            this.tabReplays.Size = new System.Drawing.Size(898, 518);
            this.tabReplays.TabIndex = 5;
            this.tabReplays.Text = "Повторы матчей";
            this.tabReplays.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.btnRunReplay);
            this.panel4.Controls.Add(this.btnSelectReplayFolder);
            this.panel4.Controls.Add(this.edtSaveReplays);
            this.panel4.Controls.Add(this.edtReplayFolder);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(898, 518);
            this.panel4.TabIndex = 1;
            // 
            // btnRunReplay
            // 
            this.btnRunReplay.Location = new System.Drawing.Point(597, 424);
            this.btnRunReplay.Name = "btnRunReplay";
            this.btnRunReplay.Size = new System.Drawing.Size(283, 77);
            this.btnRunReplay.TabIndex = 12;
            this.btnRunReplay.Text = "Запустить повтор матча...";
            this.btnRunReplay.UseVisualStyleBackColor = true;
            this.btnRunReplay.Click += new System.EventHandler(this.btnRunReplay_Click);
            // 
            // btnSelectReplayFolder
            // 
            this.btnSelectReplayFolder.Location = new System.Drawing.Point(830, 51);
            this.btnSelectReplayFolder.Name = "btnSelectReplayFolder";
            this.btnSelectReplayFolder.Size = new System.Drawing.Size(33, 23);
            this.btnSelectReplayFolder.TabIndex = 11;
            this.btnSelectReplayFolder.Text = "...";
            this.btnSelectReplayFolder.UseVisualStyleBackColor = true;
            this.btnSelectReplayFolder.Click += new System.EventHandler(this.btnSelectReplayFolder_Click);
            // 
            // edtSaveReplays
            // 
            this.edtSaveReplays.AutoSize = true;
            this.edtSaveReplays.Location = new System.Drawing.Point(12, 50);
            this.edtSaveReplays.Name = "edtSaveReplays";
            this.edtSaveReplays.Size = new System.Drawing.Size(224, 22);
            this.edtSaveReplays.TabIndex = 10;
            this.edtSaveReplays.Text = "Сохранять повторы в папку:";
            this.edtSaveReplays.UseVisualStyleBackColor = true;
            // 
            // edtReplayFolder
            // 
            this.edtReplayFolder.Location = new System.Drawing.Point(242, 50);
            this.edtReplayFolder.Name = "edtReplayFolder";
            this.edtReplayFolder.Size = new System.Drawing.Size(582, 24);
            this.edtReplayFolder.TabIndex = 8;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(856, 18);
            this.label15.TabIndex = 0;
            this.label15.Text = "На этой вкладке вы можете включить автоматическое сохранение повторов игр или зап" +
    "устить уже сохраненный повтор";
            // 
            // tabHelp
            // 
            this.tabHelp.Controls.Add(this.panel3);
            this.tabHelp.Location = new System.Drawing.Point(4, 27);
            this.tabHelp.Name = "tabHelp";
            this.tabHelp.Size = new System.Drawing.Size(898, 518);
            this.tabHelp.TabIndex = 4;
            this.tabHelp.Text = "Помощь";
            this.tabHelp.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(898, 518);
            this.panel3.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 236);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(798, 18);
            this.label9.TabIndex = 7;
            this.label9.Text = "Кликните меню \"Помощь\" в игровом окне, чтобы ознакомиться с вспомогательными возм" +
    "ожностями системы.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(416, 18);
            this.label8.TabIndex = 6;
            this.label8.Text = "Нажмите кнопку НАЧАТЬ МАТЧ. Откроется игровое окно.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(385, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 18);
            this.label7.TabIndex = 5;
            this.label7.Text = "Игровое окно";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(385, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 18);
            this.label6.TabIndex = 4;
            this.label6.Text = "Добавление игроков";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(385, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "Запуск матча";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(825, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Кликните на добавленных игроков, чтобы выбрать их для участия в матче. Кликните п" +
    "овторно для отмены выбора.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(481, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Используйте левую панель, чтобы добавить игроков в программу. ";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 549);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(906, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(366, 455);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(529, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "R3DS PROGRAMMING CONTEST проводится компанией russian3dscanner.com";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(366, 484);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(526, 17);
            this.label10.TabIndex = 9;
            this.label10.Text = "Разработчик тестирующей системы: Землянухин Михаил (abc-0-4@yandex.ru)";
            // 
            // StartForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(906, 571);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StartForm";
            this.Text = "ContestAI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StartForm_FormClosed);
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StartForm_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabLocalGames.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edtFixedRandomSeed)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tabReplays.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabHelp.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLocalGames;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnAddProgram;
        private System.Windows.Forms.Button btnAddHuman;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel panelPlayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel panelPlayersInMatch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnChangeOrder;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.TabPage tabHelp;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChangeJavaPath;
        private System.Windows.Forms.Button btnChangePythonPath;
        private System.Windows.Forms.TabPage tabReplays;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnRunReplay;
        private System.Windows.Forms.Button btnSelectReplayFolder;
        private System.Windows.Forms.CheckBox edtSaveReplays;
        private System.Windows.Forms.TextBox edtReplayFolder;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown edtFixedRandomSeed;
        private System.Windows.Forms.CheckBox edtUseFixedRandomSeed;
        private System.Windows.Forms.Label labelScoreWithBots;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
    }
}