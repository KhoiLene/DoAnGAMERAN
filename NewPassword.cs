using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAnNhom_GAMERAN_
{
    public partial class NewPassword : Form
    {
        private Database db;
        string email;
        public NewPassword(string userEmail)
        {
            InitializeComponent();
            db = new Database();
            email = userEmail;

            LoadUsername();


            txtUsername.TabIndex = 0;
            txtPassword.TabIndex = 1;
            txtPassword2.TabIndex = 2;
        }
        private void LoadUsername()
        {
            var user = db.GetUserByEmail(email);

            if (user != null)
            {
                txtUsername.Text = user.Username;
                
            }
        }
        private void Agree_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string pass1 = txtPassword.Text.Trim();
            string pass2 = txtPassword2.Text.Trim();

            if (string.IsNullOrEmpty(pass1) || string.IsNullOrEmpty(pass2))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                return;
            }

            if (pass1 != pass2)
            {
                MessageBox.Show("Mật khẩu không khớp!");
                return;
            }

            var user = db.GetUserByEmail(email);

            if (user == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản!");
                return;
            }

            bool changedUsername = user.Username != username;


            db.UpdatePassword(email, username, pass1);

            if (changedUsername)
                MessageBox.Show("Cập nhật thành công!");
            else
                MessageBox.Show("Đổi mật khẩu thành công!");

            FormLogin loginForm = new FormLogin();
            loginForm.Show();
            this.Close();
        }
        private void ShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !ShowPassword.Checked;
            txtPassword2.UseSystemPasswordChar = !ShowPassword.Checked;
        }

    }
}
