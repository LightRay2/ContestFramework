namespace Client
{
    partial class frmCreateGamesOnServer
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
            this.edtPlayers = new System.Windows.Forms.CheckedListBox();
            this.edtTime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // edtPlayers
            // 
            this.edtPlayers.CheckOnClick = true;
            this.edtPlayers.FormattingEnabled = true;
            this.edtPlayers.Location = new System.Drawing.Point(87, 52);
            this.edtPlayers.Name = "edtPlayers";
            this.edtPlayers.Size = new System.Drawing.Size(120, 124);
            this.edtPlayers.TabIndex = 0;
            // 
            // edtTime
            // 
            this.edtTime.Location = new System.Drawing.Point(441, 52);
            this.edtTime.Name = "edtTime";
            this.edtTime.Size = new System.Drawing.Size(100, 20);
            this.edtTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(337, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Время начала";
            // 
            // btnAddGame
            // 
            this.btnAddGame.Location = new System.Drawing.Point(380, 112);
            this.btnAddGame.Name = "btnAddGame";
            this.btnAddGame.Size = new System.Drawing.Size(75, 23);
            this.btnAddGame.TabIndex = 3;
            this.btnAddGame.Text = "Добавить";
            this.btnAddGame.UseVisualStyleBackColor = true;
            this.btnAddGame.Click += new System.EventHandler(this.btnAddGame_Click);
            // 
            // frmCreateGamesOnServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 282);
            this.Controls.Add(this.btnAddGame);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edtTime);
            this.Controls.Add(this.edtPlayers);
            this.Name = "frmCreateGamesOnServer";
            this.Text = "CreateGamesOnServer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox edtPlayers;
        private System.Windows.Forms.TextBox edtTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddGame;
    }
}