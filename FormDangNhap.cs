using System;
using System.Windows.Forms;

namespace DoAnNhom_GAMERAN_
{
    public partial class FormLogin : Form
    {
        private Database db;

        public FormLogin()
        {
            InitializeComponent();
            db = new Database();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            int userId = db.LoginUser(username, password);
            if (userId > 0)
            {
                MessageBox.Show("Đăng nhập thành công!");
                Form1 mainForm = new Form1(userId); // truyền userId vào Form1
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            FormRegister registerForm = new FormRegister();
            registerForm.Show();
            this.Hide();
        }
    }
}