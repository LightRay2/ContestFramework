namespace Client
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
            this.panelMatchTimeOnServer = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.edtMinTimePerMatch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.edtMatchDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChangeOrder = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRemoveFromGameList = new System.Windows.Forms.Button();
            this.btnAddToGameList = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.panelPlayersInMatch = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.tabWatchServerMatches = new System.Windows.Forms.TabPage();
            this.btnSaveRoomDescription = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panelRoomSettings = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnCreateRoom = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.edtMatchAddLimitPerDay = new System.Windows.Forms.TextBox();
            this.checkAllowAddMatchesToAll = new System.Windows.Forms.CheckBox();
            this.checkAllowWatchOnlyMineMatches = new System.Windows.Forms.CheckBox();
            this.panelRoomUsers = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btnRemoveMatchFromServer = new System.Windows.Forms.Button();
            this.gvMatches = new System.Windows.Forms.DataGridView();
            this.clmDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPlayers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnWatchServerMatches = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabConnectServer = new System.Windows.Forms.TabPage();
            this.tabAddServerMatches = new System.Windows.Forms.TabPage();
            this.tabHelp = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.checkRoomVisibleOnlyForParticipants = new System.Windows.Forms.CheckBox();
            this.btnDeleteRoom = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabLocalGames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelMatchTimeOnServer.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabWatchServerMatches.SuspendLayout();
            this.panelRoomSettings.SuspendLayout();
            this.panelRoomUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMatches)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLocalGames);
            this.tabControl1.Controls.Add(this.tabWatchServerMatches);
            this.tabControl1.Controls.Add(this.tabConnectServer);
            this.tabControl1.Controls.Add(this.tabAddServerMatches);
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
            this.splitContainer1.Panel2.Controls.Add(this.panelMatchTimeOnServer);
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
            // panelMatchTimeOnServer
            // 
            this.panelMatchTimeOnServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMatchTimeOnServer.Controls.Add(this.label8);
            this.panelMatchTimeOnServer.Controls.Add(this.label7);
            this.panelMatchTimeOnServer.Controls.Add(this.edtMinTimePerMatch);
            this.panelMatchTimeOnServer.Controls.Add(this.label3);
            this.panelMatchTimeOnServer.Controls.Add(this.edtMatchDate);
            this.panelMatchTimeOnServer.Controls.Add(this.label2);
            this.panelMatchTimeOnServer.Location = new System.Drawing.Point(36, 385);
            this.panelMatchTimeOnServer.Name = "panelMatchTimeOnServer";
            this.panelMatchTimeOnServer.Size = new System.Drawing.Size(285, 118);
            this.panelMatchTimeOnServer.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(261, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 18);
            this.label8.TabIndex = 5;
            this.label8.Text = "2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(247, 18);
            this.label7.TabIndex = 4;
            this.label7.Text = "Сегодня можно добавить матчей:";
            // 
            // edtMinTimePerMatch
            // 
            this.edtMinTimePerMatch.Location = new System.Drawing.Point(189, 53);
            this.edtMinTimePerMatch.Name = "edtMinTimePerMatch";
            this.edtMinTimePerMatch.Size = new System.Drawing.Size(60, 24);
            this.edtMinTimePerMatch.TabIndex = 3;
            this.edtMinTimePerMatch.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Минимальное время, с";
            // 
            // edtMatchDate
            // 
            this.edtMatchDate.CustomFormat = "yyyy - dd MMM HH:mm";
            this.edtMatchDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.edtMatchDate.Location = new System.Drawing.Point(86, 20);
            this.edtMatchDate.Name = "edtMatchDate";
            this.edtMatchDate.ShowUpDown = true;
            this.edtMatchDate.Size = new System.Drawing.Size(163, 24);
            this.edtMatchDate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Начало";
            // 
            // btnChangeOrder
            // 
            this.btnChangeOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeOrder.Location = new System.Drawing.Point(182, 147);
            this.btnChangeOrder.Name = "btnChangeOrder";
            this.btnChangeOrder.Size = new System.Drawing.Size(197, 35);
            this.btnChangeOrder.TabIndex = 4;
            this.btnChangeOrder.Text = "Изменить порядок";
            this.btnChangeOrder.UseVisualStyleBackColor = true;
            this.btnChangeOrder.Click += new System.EventHandler(this.btnChangeOrder_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.btnRemoveFromGameList);
            this.panel2.Controls.Add(this.btnAddToGameList);
            this.panel2.Controls.Add(this.btnRun);
            this.panel2.Location = new System.Drawing.Point(327, 385);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(260, 118);
            this.panel2.TabIndex = 3;
            // 
            // btnRemoveFromGameList
            // 
            this.btnRemoveFromGameList.Location = new System.Drawing.Point(206, 12);
            this.btnRemoveFromGameList.Name = "btnRemoveFromGameList";
            this.btnRemoveFromGameList.Size = new System.Drawing.Size(49, 35);
            this.btnRemoveFromGameList.TabIndex = 3;
            this.btnRemoveFromGameList.Text = "-";
            this.btnRemoveFromGameList.UseVisualStyleBackColor = true;
            this.btnRemoveFromGameList.Click += new System.EventHandler(this.btnRemoveFromGameList_Click);
            // 
            // btnAddToGameList
            // 
            this.btnAddToGameList.Location = new System.Drawing.Point(3, 12);
            this.btnAddToGameList.Name = "btnAddToGameList";
            this.btnAddToGameList.Size = new System.Drawing.Size(197, 35);
            this.btnAddToGameList.TabIndex = 2;
            this.btnAddToGameList.Text = "В список игр (0)";
            this.btnAddToGameList.UseVisualStyleBackColor = true;
            this.btnAddToGameList.Click += new System.EventHandler(this.btnAddToGameList_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(3, 53);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(252, 62);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Запуск";
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
            this.panelPlayersInMatch.Size = new System.Drawing.Size(598, 141);
            this.panelPlayersInMatch.TabIndex = 0;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSelection.Location = new System.Drawing.Point(385, 147);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(197, 35);
            this.btnClearSelection.TabIndex = 5;
            this.btnClearSelection.Text = "Очистить выбор";
            this.btnClearSelection.UseVisualStyleBackColor = true;
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // tabWatchServerMatches
            // 
            this.tabWatchServerMatches.AutoScroll = true;
            this.tabWatchServerMatches.Controls.Add(this.btnDeleteRoom);
            this.tabWatchServerMatches.Controls.Add(this.btnSaveRoomDescription);
            this.tabWatchServerMatches.Controls.Add(this.textBox1);
            this.tabWatchServerMatches.Controls.Add(this.panelRoomSettings);
            this.tabWatchServerMatches.Controls.Add(this.panelRoomUsers);
            this.tabWatchServerMatches.Controls.Add(this.btnRemoveMatchFromServer);
            this.tabWatchServerMatches.Controls.Add(this.gvMatches);
            this.tabWatchServerMatches.Controls.Add(this.btnWatchServerMatches);
            this.tabWatchServerMatches.Controls.Add(this.button1);
            this.tabWatchServerMatches.Controls.Add(this.label4);
            this.tabWatchServerMatches.Controls.Add(this.comboBox1);
            this.tabWatchServerMatches.Location = new System.Drawing.Point(4, 27);
            this.tabWatchServerMatches.Margin = new System.Windows.Forms.Padding(4);
            this.tabWatchServerMatches.Name = "tabWatchServerMatches";
            this.tabWatchServerMatches.Padding = new System.Windows.Forms.Padding(4);
            this.tabWatchServerMatches.Size = new System.Drawing.Size(898, 518);
            this.tabWatchServerMatches.TabIndex = 1;
            this.tabWatchServerMatches.Text = "Матчи на сервере";
            this.tabWatchServerMatches.UseVisualStyleBackColor = true;
            // 
            // btnSaveRoomDescription
            // 
            this.btnSaveRoomDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnSaveRoomDescription.Location = new System.Drawing.Point(751, 38);
            this.btnSaveRoomDescription.Name = "btnSaveRoomDescription";
            this.btnSaveRoomDescription.Size = new System.Drawing.Size(91, 39);
            this.btnSaveRoomDescription.TabIndex = 12;
            this.btnSaveRoomDescription.Text = "Сохранить";
            this.btnSaveRoomDescription.UseVisualStyleBackColor = false;
            this.btnSaveRoomDescription.Click += new System.EventHandler(this.btnSaveRoomDescription_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 39);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(732, 38);
            this.textBox1.TabIndex = 11;
            // 
            // panelRoomSettings
            // 
            this.panelRoomSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRoomSettings.Controls.Add(this.checkRoomVisibleOnlyForParticipants);
            this.panelRoomSettings.Controls.Add(this.label9);
            this.panelRoomSettings.Controls.Add(this.textBox2);
            this.panelRoomSettings.Controls.Add(this.btnCreateRoom);
            this.panelRoomSettings.Controls.Add(this.label6);
            this.panelRoomSettings.Controls.Add(this.edtMatchAddLimitPerDay);
            this.panelRoomSettings.Controls.Add(this.checkAllowAddMatchesToAll);
            this.panelRoomSettings.Controls.Add(this.checkAllowWatchOnlyMineMatches);
            this.panelRoomSettings.Location = new System.Drawing.Point(16, 593);
            this.panelRoomSettings.Name = "panelRoomSettings";
            this.panelRoomSettings.Size = new System.Drawing.Size(837, 129);
            this.panelRoomSettings.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label9.Location = new System.Drawing.Point(14, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 18);
            this.label9.TabIndex = 6;
            this.label9.Text = "Новая комната:";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.textBox2.Location = new System.Drawing.Point(145, 96);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(420, 24);
            this.textBox2.TabIndex = 5;
            // 
            // btnCreateRoom
            // 
            this.btnCreateRoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnCreateRoom.Location = new System.Drawing.Point(582, 94);
            this.btnCreateRoom.Name = "btnCreateRoom";
            this.btnCreateRoom.Size = new System.Drawing.Size(147, 26);
            this.btnCreateRoom.TabIndex = 4;
            this.btnCreateRoom.Text = "Создать комнату";
            this.btnCreateRoom.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label6.Location = new System.Drawing.Point(14, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(366, 18);
            this.label6.TabIndex = 3;
            this.label6.Text = "Лимит добавления матчей пользователем за день";
            // 
            // edtMatchAddLimitPerDay
            // 
            this.edtMatchAddLimitPerDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.edtMatchAddLimitPerDay.Location = new System.Drawing.Point(393, 66);
            this.edtMatchAddLimitPerDay.Name = "edtMatchAddLimitPerDay";
            this.edtMatchAddLimitPerDay.Size = new System.Drawing.Size(121, 24);
            this.edtMatchAddLimitPerDay.TabIndex = 2;
            // 
            // checkAllowAddMatchesToAll
            // 
            this.checkAllowAddMatchesToAll.AutoSize = true;
            this.checkAllowAddMatchesToAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.checkAllowAddMatchesToAll.Location = new System.Drawing.Point(330, 38);
            this.checkAllowAddMatchesToAll.Name = "checkAllowAddMatchesToAll";
            this.checkAllowAddMatchesToAll.Size = new System.Drawing.Size(267, 22);
            this.checkAllowAddMatchesToAll.TabIndex = 1;
            this.checkAllowAddMatchesToAll.Text = "Разрешить добавлять матчи всем";
            this.checkAllowAddMatchesToAll.UseVisualStyleBackColor = false;
            // 
            // checkAllowWatchOnlyMineMatches
            // 
            this.checkAllowWatchOnlyMineMatches.AutoSize = true;
            this.checkAllowWatchOnlyMineMatches.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.checkAllowWatchOnlyMineMatches.Location = new System.Drawing.Point(14, 38);
            this.checkAllowWatchOnlyMineMatches.Name = "checkAllowWatchOnlyMineMatches";
            this.checkAllowWatchOnlyMineMatches.Size = new System.Drawing.Size(310, 22);
            this.checkAllowWatchOnlyMineMatches.TabIndex = 0;
            this.checkAllowWatchOnlyMineMatches.Text = "Разрешить смотреть только свои матчи";
            this.checkAllowWatchOnlyMineMatches.UseVisualStyleBackColor = false;
            // 
            // panelRoomUsers
            // 
            this.panelRoomUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRoomUsers.Controls.Add(this.button2);
            this.panelRoomUsers.Controls.Add(this.comboBox4);
            this.panelRoomUsers.Controls.Add(this.comboBox3);
            this.panelRoomUsers.Controls.Add(this.lblUserInfo);
            this.panelRoomUsers.Controls.Add(this.label5);
            this.panelRoomUsers.Controls.Add(this.comboBox2);
            this.panelRoomUsers.Location = new System.Drawing.Point(3, 444);
            this.panelRoomUsers.Name = "panelRoomUsers";
            this.panelRoomUsers.Size = new System.Drawing.Size(850, 143);
            this.panelRoomUsers.TabIndex = 9;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.button2.Location = new System.Drawing.Point(780, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 27);
            this.button2.TabIndex = 10;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // comboBox4
            // 
            this.comboBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(573, 7);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(201, 26);
            this.comboBox4.TabIndex = 11;
            // 
            // comboBox3
            // 
            this.comboBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Добавить в",
            "Переместить в",
            "Удалить из текущей"});
            this.comboBox3.Location = new System.Drawing.Point(313, 6);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(245, 26);
            this.comboBox3.TabIndex = 10;
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUserInfo.Location = new System.Drawing.Point(11, 48);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(836, 83);
            this.lblUserInfo.TabIndex = 9;
            this.lblUserInfo.Text = "Информация о команде";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "Команда:";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(92, 7);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(215, 26);
            this.comboBox2.TabIndex = 8;
            // 
            // btnRemoveMatchFromServer
            // 
            this.btnRemoveMatchFromServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnRemoveMatchFromServer.Location = new System.Drawing.Point(277, 410);
            this.btnRemoveMatchFromServer.Name = "btnRemoveMatchFromServer";
            this.btnRemoveMatchFromServer.Size = new System.Drawing.Size(176, 27);
            this.btnRemoveMatchFromServer.TabIndex = 5;
            this.btnRemoveMatchFromServer.Text = "Удалить";
            this.btnRemoveMatchFromServer.UseVisualStyleBackColor = false;
            // 
            // gvMatches
            // 
            this.gvMatches.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMatches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmDate,
            this.clmPlayers,
            this.clmStatus});
            this.gvMatches.Location = new System.Drawing.Point(13, 87);
            this.gvMatches.Name = "gvMatches";
            this.gvMatches.RowHeadersVisible = false;
            this.gvMatches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMatches.Size = new System.Drawing.Size(840, 305);
            this.gvMatches.TabIndex = 4;
            // 
            // clmDate
            // 
            this.clmDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmDate.FillWeight = 15F;
            this.clmDate.HeaderText = "Время";
            this.clmDate.Name = "clmDate";
            this.clmDate.ReadOnly = true;
            // 
            // clmPlayers
            // 
            this.clmPlayers.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmPlayers.FillWeight = 60F;
            this.clmPlayers.HeaderText = "Участники";
            this.clmPlayers.Name = "clmPlayers";
            this.clmPlayers.ReadOnly = true;
            // 
            // clmStatus
            // 
            this.clmStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmStatus.FillWeight = 30F;
            this.clmStatus.HeaderText = "Статус";
            this.clmStatus.Name = "clmStatus";
            this.clmStatus.ReadOnly = true;
            // 
            // btnWatchServerMatches
            // 
            this.btnWatchServerMatches.Location = new System.Drawing.Point(13, 410);
            this.btnWatchServerMatches.Name = "btnWatchServerMatches";
            this.btnWatchServerMatches.Size = new System.Drawing.Size(248, 27);
            this.btnWatchServerMatches.TabIndex = 3;
            this.btnWatchServerMatches.Text = "Смотреть выделенные матчи";
            this.btnWatchServerMatches.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(540, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "Обновить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 18);
            this.label4.TabIndex = 1;
            this.label4.Text = "Комната:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(107, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(427, 26);
            this.comboBox1.TabIndex = 0;
            // 
            // tabConnectServer
            // 
            this.tabConnectServer.Location = new System.Drawing.Point(4, 22);
            this.tabConnectServer.Name = "tabConnectServer";
            this.tabConnectServer.Size = new System.Drawing.Size(898, 523);
            this.tabConnectServer.TabIndex = 2;
            this.tabConnectServer.Text = "Подключение к серверу";
            this.tabConnectServer.UseVisualStyleBackColor = true;
            // 
            // tabAddServerMatches
            // 
            this.tabAddServerMatches.Location = new System.Drawing.Point(4, 22);
            this.tabAddServerMatches.Name = "tabAddServerMatches";
            this.tabAddServerMatches.Size = new System.Drawing.Size(898, 523);
            this.tabAddServerMatches.TabIndex = 3;
            this.tabAddServerMatches.Text = "Добавить матчи на сервер";
            this.tabAddServerMatches.UseVisualStyleBackColor = true;
            // 
            // tabHelp
            // 
            this.tabHelp.Location = new System.Drawing.Point(4, 22);
            this.tabHelp.Name = "tabHelp";
            this.tabHelp.Size = new System.Drawing.Size(898, 523);
            this.tabHelp.TabIndex = 4;
            this.tabHelp.Text = "Помощь";
            this.tabHelp.UseVisualStyleBackColor = true;
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
            // checkRoomVisibleOnlyForParticipants
            // 
            this.checkRoomVisibleOnlyForParticipants.AutoSize = true;
            this.checkRoomVisibleOnlyForParticipants.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.checkRoomVisibleOnlyForParticipants.Location = new System.Drawing.Point(14, 10);
            this.checkRoomVisibleOnlyForParticipants.Name = "checkRoomVisibleOnlyForParticipants";
            this.checkRoomVisibleOnlyForParticipants.Size = new System.Drawing.Size(522, 22);
            this.checkRoomVisibleOnlyForParticipants.TabIndex = 7;
            this.checkRoomVisibleOnlyForParticipants.Text = "Комната видна только админам и добавленным в нее пользователям";
            this.checkRoomVisibleOnlyForParticipants.UseVisualStyleBackColor = false;
            // 
            // btnDeleteRoom
            // 
            this.btnDeleteRoom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnDeleteRoom.Location = new System.Drawing.Point(665, 7);
            this.btnDeleteRoom.Name = "btnDeleteRoom";
            this.btnDeleteRoom.Size = new System.Drawing.Size(112, 25);
            this.btnDeleteRoom.TabIndex = 13;
            this.btnDeleteRoom.Text = "Удалить";
            this.btnDeleteRoom.UseVisualStyleBackColor = false;
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
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabLocalGames.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelMatchTimeOnServer.ResumeLayout(false);
            this.panelMatchTimeOnServer.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabWatchServerMatches.ResumeLayout(false);
            this.tabWatchServerMatches.PerformLayout();
            this.panelRoomSettings.ResumeLayout(false);
            this.panelRoomSettings.PerformLayout();
            this.panelRoomUsers.ResumeLayout(false);
            this.panelRoomUsers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMatches)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLocalGames;
        private System.Windows.Forms.TabPage tabWatchServerMatches;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnAddProgram;
        private System.Windows.Forms.Button btnAddHuman;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel panelPlayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel panelPlayersInMatch;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAddToGameList;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnRemoveFromGameList;
        private System.Windows.Forms.TabPage tabConnectServer;
        private System.Windows.Forms.TabPage tabAddServerMatches;
        private System.Windows.Forms.Button btnChangeOrder;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.TabPage tabHelp;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Panel panelMatchTimeOnServer;
        private System.Windows.Forms.DateTimePicker edtMatchDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edtMinTimePerMatch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView gvMatches;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPlayers;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStatus;
        private System.Windows.Forms.Button btnWatchServerMatches;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnRemoveMatchFromServer;
        private System.Windows.Forms.Panel panelRoomUsers;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelRoomSettings;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox edtMatchAddLimitPerDay;
        private System.Windows.Forms.CheckBox checkAllowAddMatchesToAll;
        private System.Windows.Forms.CheckBox checkAllowWatchOnlyMineMatches;
        private System.Windows.Forms.Button btnSaveRoomDescription;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnCreateRoom;
        private System.Windows.Forms.Button btnDeleteRoom;
        private System.Windows.Forms.CheckBox checkRoomVisibleOnlyForParticipants;
    }
}