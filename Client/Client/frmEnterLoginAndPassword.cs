using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class frmEnterLoginAndPassword : Form
    {
        static frmEnterLoginAndPassword _instance;
        public static frmEnterLoginAndPassword e
        {
            get { 
                if (_instance == null) 
                    _instance = new frmEnterLoginAndPassword();
                return _instance;
            }
            
        }
        frmEnterLoginAndPassword()
        {
            InitializeComponent();
        }

        public string login, password;

        public DialogResult ShowDialog(string login, string password){

            this.edtServerLogin.Text = login;
            this.edtServerPassword.Text = password;
            var result = this.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                this.login = this.edtServerLogin.Text;
                this.password = this.edtServerPassword.Text;
            }
            return result;
        }

        //private void btnOk_Click(object sender, EventArgs e)
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
        //    this.Hide();
        //}

        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    this.Hide();
        //}

    }
}
