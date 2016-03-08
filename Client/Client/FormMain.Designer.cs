namespace Client
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabLocal = new System.Windows.Forms.TabPage();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.checkHumanTwo = new System.Windows.Forms.CheckBox();
            this.checkHumanOne = new System.Windows.Forms.CheckBox();
            this.btnAddressTwo = new System.Windows.Forms.Button();
            this.btnAddressOne = new System.Windows.Forms.Button();
            this.edtAddressTwo = new System.Windows.Forms.TextBox();
            this.edtAddressOne = new System.Windows.Forms.TextBox();
            this.tabServer = new System.Windows.Forms.TabPage();
            this.grd = new System.Windows.Forms.DataGridView();
            this.clmRoomName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPlayers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRefreshServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.edtServerAddress = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.edtServerMessages = new System.Windows.Forms.ListView();
            this.btnFileToServerDialog = new System.Windows.Forms.Button();
            this.edtFileToServer = new System.Windows.Forms.TextBox();
            this.btnUploadFileToServer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btmChangeLoginAndPassword = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabLocal.SuspendLayout();
            this.tabServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabLocal);
            this.tabControl.Controls.Add(this.tabServer);
            this.tabControl.Location = new System.Drawing.Point(12, 32);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(769, 452);
            this.tabControl.TabIndex = 0;
            // 
            // tabLocal
            // 
            this.tabLocal.Controls.Add(this.btnExchange);
            this.tabLocal.Controls.Add(this.btnStart);
            this.tabLocal.Controls.Add(this.checkHumanTwo);
            this.tabLocal.Controls.Add(this.checkHumanOne);
            this.tabLocal.Controls.Add(this.btnAddressTwo);
            this.tabLocal.Controls.Add(this.btnAddressOne);
            this.tabLocal.Controls.Add(this.edtAddressTwo);
            this.tabLocal.Controls.Add(this.edtAddressOne);
            this.tabLocal.Location = new System.Drawing.Point(4, 22);
            this.tabLocal.Name = "tabLocal";
            this.tabLocal.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabLocal.Size = new System.Drawing.Size(761, 426);
            this.tabLocal.TabIndex = 0;
            this.tabLocal.Text = "Локальный запуск";
            this.tabLocal.UseVisualStyleBackColor = true;
            // 
            // btnExchange
            // 
            this.btnExchange.Location = new System.Drawing.Point(233, 77);
            this.btnExchange.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(35, 34);
            this.btnExchange.TabIndex = 7;
            this.btnExchange.Text = "button4";
            this.btnExchange.UseVisualStyleBackColor = true;
            this.btnExchange.Click += new System.EventHandler(this.btnExchange_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(406, 183);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(152, 47);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "button3";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // checkHumanTwo
            // 
            this.checkHumanTwo.AutoSize = true;
            this.checkHumanTwo.Location = new System.Drawing.Point(17, 129);
            this.checkHumanTwo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkHumanTwo.Name = "checkHumanTwo";
            this.checkHumanTwo.Size = new System.Drawing.Size(80, 17);
            this.checkHumanTwo.TabIndex = 5;
            this.checkHumanTwo.Text = "checkBox2";
            this.checkHumanTwo.UseVisualStyleBackColor = true;
            // 
            // checkHumanOne
            // 
            this.checkHumanOne.AutoSize = true;
            this.checkHumanOne.Location = new System.Drawing.Point(17, 37);
            this.checkHumanOne.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkHumanOne.Name = "checkHumanOne";
            this.checkHumanOne.Size = new System.Drawing.Size(80, 17);
            this.checkHumanOne.TabIndex = 4;
            this.checkHumanOne.Text = "checkBox1";
            this.checkHumanOne.UseVisualStyleBackColor = true;
            // 
            // btnAddressTwo
            // 
            this.btnAddressTwo.Location = new System.Drawing.Point(436, 130);
            this.btnAddressTwo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddressTwo.Name = "btnAddressTwo";
            this.btnAddressTwo.Size = new System.Drawing.Size(50, 17);
            this.btnAddressTwo.TabIndex = 3;
            this.btnAddressTwo.Text = "button2";
            this.btnAddressTwo.UseVisualStyleBackColor = true;
            this.btnAddressTwo.Click += new System.EventHandler(this.btnAddressTwo_Click);
            // 
            // btnAddressOne
            // 
            this.btnAddressOne.Location = new System.Drawing.Point(436, 40);
            this.btnAddressOne.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddressOne.Name = "btnAddressOne";
            this.btnAddressOne.Size = new System.Drawing.Size(50, 16);
            this.btnAddressOne.TabIndex = 2;
            this.btnAddressOne.Text = "button1";
            this.btnAddressOne.UseVisualStyleBackColor = true;
            this.btnAddressOne.Click += new System.EventHandler(this.btnAddressOne_Click);
            // 
            // edtAddressTwo
            // 
            this.edtAddressTwo.Location = new System.Drawing.Point(93, 130);
            this.edtAddressTwo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edtAddressTwo.Name = "edtAddressTwo";
            this.edtAddressTwo.Size = new System.Drawing.Size(317, 20);
            this.edtAddressTwo.TabIndex = 1;
            // 
            // edtAddressOne
            // 
            this.edtAddressOne.Location = new System.Drawing.Point(93, 39);
            this.edtAddressOne.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.edtAddressOne.Name = "edtAddressOne";
            this.edtAddressOne.Size = new System.Drawing.Size(317, 20);
            this.edtAddressOne.TabIndex = 0;
            // 
            // tabServer
            // 
            this.tabServer.AutoScroll = true;
            this.tabServer.Controls.Add(this.btmChangeLoginAndPassword);
            this.tabServer.Controls.Add(this.label2);
            this.tabServer.Controls.Add(this.btnUploadFileToServer);
            this.tabServer.Controls.Add(this.edtFileToServer);
            this.tabServer.Controls.Add(this.btnFileToServerDialog);
            this.tabServer.Controls.Add(this.edtServerMessages);
            this.tabServer.Controls.Add(this.grd);
            this.tabServer.Controls.Add(this.btnRefreshServer);
            this.tabServer.Controls.Add(this.label1);
            this.tabServer.Controls.Add(this.edtServerAddress);
            this.tabServer.Location = new System.Drawing.Point(4, 22);
            this.tabServer.Name = "tabServer";
            this.tabServer.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabServer.Size = new System.Drawing.Size(761, 426);
            this.tabServer.TabIndex = 1;
            this.tabServer.Text = "Зайти на сервер";
            this.tabServer.UseVisualStyleBackColor = true;
            // 
            // grd
            // 
            this.grd.AllowUserToAddRows = false;
            this.grd.AllowUserToDeleteRows = false;
            this.grd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmRoomName,
            this.clmPlayers});
            this.grd.Location = new System.Drawing.Point(21, 128);
            this.grd.Name = "grd";
            this.grd.ReadOnly = true;
            this.grd.Size = new System.Drawing.Size(461, 150);
            this.grd.TabIndex = 4;
            // 
            // clmRoomName
            // 
            this.clmRoomName.HeaderText = "Комната";
            this.clmRoomName.Name = "clmRoomName";
            this.clmRoomName.ReadOnly = true;
            // 
            // clmPlayers
            // 
            this.clmPlayers.HeaderText = "Участники";
            this.clmPlayers.Name = "clmPlayers";
            this.clmPlayers.ReadOnly = true;
            // 
            // btnRefreshServer
            // 
            this.btnRefreshServer.Location = new System.Drawing.Point(349, 15);
            this.btnRefreshServer.Name = "btnRefreshServer";
            this.btnRefreshServer.Size = new System.Drawing.Size(96, 23);
            this.btnRefreshServer.TabIndex = 2;
            this.btnRefreshServer.Text = "Обновить";
            this.btnRefreshServer.UseVisualStyleBackColor = true;
            this.btnRefreshServer.Click += new System.EventHandler(this.btnRefreshServer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Адрес сервера:";
            // 
            // edtServerAddress
            // 
            this.edtServerAddress.Location = new System.Drawing.Point(108, 17);
            this.edtServerAddress.Name = "edtServerAddress";
            this.edtServerAddress.Size = new System.Drawing.Size(223, 20);
            this.edtServerAddress.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // edtServerMessages
            // 
            this.edtServerMessages.Location = new System.Drawing.Point(573, 6);
            this.edtServerMessages.Name = "edtServerMessages";
            this.edtServerMessages.Size = new System.Drawing.Size(182, 224);
            this.edtServerMessages.TabIndex = 5;
            this.edtServerMessages.UseCompatibleStateImageBehavior = false;
            // 
            // btnFileToServerDialog
            // 
            this.btnFileToServerDialog.Location = new System.Drawing.Point(349, 73);
            this.btnFileToServerDialog.Name = "btnFileToServerDialog";
            this.btnFileToServerDialog.Size = new System.Drawing.Size(46, 23);
            this.btnFileToServerDialog.TabIndex = 6;
            this.btnFileToServerDialog.Text = "...";
            this.btnFileToServerDialog.UseVisualStyleBackColor = true;
            this.btnFileToServerDialog.Click += new System.EventHandler(this.btnFileToServerDialog_Click);
            // 
            // edtFileToServer
            // 
            this.edtFileToServer.Location = new System.Drawing.Point(97, 73);
            this.edtFileToServer.Name = "edtFileToServer";
            this.edtFileToServer.Size = new System.Drawing.Size(223, 20);
            this.edtFileToServer.TabIndex = 7;
            // 
            // btnUploadFileToServer
            // 
            this.btnUploadFileToServer.Location = new System.Drawing.Point(420, 73);
            this.btnUploadFileToServer.Name = "btnUploadFileToServer";
            this.btnUploadFileToServer.Size = new System.Drawing.Size(104, 23);
            this.btnUploadFileToServer.TabIndex = 8;
            this.btnUploadFileToServer.Text = "Загрузить";
            this.btnUploadFileToServer.UseVisualStyleBackColor = true;
            this.btnUploadFileToServer.Click += new System.EventHandler(this.btnUploadFileToServer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Файл";
            // 
            // btmChangeLoginAndPassword
            // 
            this.btmChangeLoginAndPassword.Location = new System.Drawing.Point(471, 14);
            this.btmChangeLoginAndPassword.Name = "btmChangeLoginAndPassword";
            this.btmChangeLoginAndPassword.Size = new System.Drawing.Size(75, 53);
            this.btmChangeLoginAndPassword.TabIndex = 10;
            this.btmChangeLoginAndPassword.Text = "Изменить имя и пароль";
            this.btmChangeLoginAndPassword.UseVisualStyleBackColor = true;
            this.btmChangeLoginAndPassword.Click += new System.EventHandler(this.btmChangeLoginAndPassword_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 496);
            this.Controls.Add(this.tabControl);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tabControl.ResumeLayout(false);
            this.tabLocal.ResumeLayout(false);
            this.tabLocal.PerformLayout();
            this.tabServer.ResumeLayout(false);
            this.tabServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabLocal;
        private System.Windows.Forms.TabPage tabServer;
        private System.Windows.Forms.Button btnRefreshServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edtServerAddress;
        private System.Windows.Forms.DataGridView grd;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmRoomName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPlayers;
        private System.Windows.Forms.Button btnExchange;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox checkHumanTwo;
        private System.Windows.Forms.CheckBox checkHumanOne;
        private System.Windows.Forms.Button btnAddressTwo;
        private System.Windows.Forms.Button btnAddressOne;
        private System.Windows.Forms.TextBox edtAddressTwo;
        private System.Windows.Forms.TextBox edtAddressOne;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUploadFileToServer;
        private System.Windows.Forms.TextBox edtFileToServer;
        private System.Windows.Forms.Button btnFileToServerDialog;
        private System.Windows.Forms.ListView edtServerMessages;
        private System.Windows.Forms.Button btmChangeLoginAndPassword;
    }
}

