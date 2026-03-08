using DoAnNhom_GAMERAN_;
using System;
using System.Windows.Forms;

public partial class FormRegister : Form
{
    private DatabaseHelper db;

    public FormRegister()
    {
        InitializeComponent();
        db = new DatabaseHelper();
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "Vui lòng nhập đầy đủ thông tin!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        int result = db.RegisterUser(username, password);
        if (result > 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Đăng ký thành công!";

            // Mở FormLogin và đóng FormRegister
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
            this.Close();
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Tên đăng nhập đã tồn tại!";
        }
    }
}