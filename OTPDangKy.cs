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
    public partial class OTPDangKy : Form
    {
        int otpCode;
        string userEmail;

        public OTPDangKy(string email, int otp)
        {
            InitializeComponent();
            userEmail = email;
            otpCode = otp;
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (txtOTP.Text == otpCode.ToString())
            {
                MessageBox.Show("Đăng ký thành công!");
                FormLogin form = new FormLogin();
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("OTP sai!");
            }
        }


    }
}
