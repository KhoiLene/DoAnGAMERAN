using DoAnNhom_GAMERAN_;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

public partial class FormRegister : Form
{
    private Database db;
    int otpCode;

    public FormRegister()
    {
        InitializeComponent();
        db = new Database();
        txtEmail.TabIndex = 0;
        txtUsername.TabIndex = 1;
        txtPassword.TabIndex = 2;
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        string Email = txtEmail.Text.Trim();
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        

        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            lblMessage.Text = "Vui lòng nhập đầy đủ thông tin!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        // kiểm tra email đã tồn tại
        if (db.CheckEmailExists(Email))
        {
            lblMessage.Text = "Email đã tồn tại!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        int result = db.RegisterUser(Email, username, password);
        if (result > 0)
        {
            Random rnd = new Random();
            otpCode = rnd.Next(100000, 999999);

            // gửi OTP
            SendOTP otpSender = new SendOTP();
            otpSender.SendOTPEmail(Email, otpCode);

            MessageBox.Show("OTP đã gửi đến email!");
            OTPDangKy otpForm = new OTPDangKy(Email, otpCode);
            otpForm.Show();
            this.Hide();

        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Tên đăng nhập đã tồn tại!";
        }
    }

    private void ShowPass_CheckedChanged(object sender, EventArgs e)
    {
        txtPassword.UseSystemPasswordChar = !ShowPass.Checked;
    }

    private void Back_Click(object sender, EventArgs e)
    {
        FormLogin loginForm = new FormLogin();
        loginForm.Show();
        this.Hide();

    }
}
