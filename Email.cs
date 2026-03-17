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
    public partial class Email : Form
    {
        public Email()
        {
            InitializeComponent();
        }

        //private void btnOTP_Click(object sender, EventArgs e)
        //{
        //    OTP otpForm = new OTP();
        //    otpForm.Show();
        //    this.Hide();
        //}
        int otpCode;

        
        private void btnSendOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            Database database = new Database();

            // kiểm tra email có trong database
            if (!database.CheckEmailExists(email))
            {
                MessageBox.Show("Email không tồn tại!");
                return;
            }

            // tạo OTP
            Random rnd = new Random();
            otpCode = rnd.Next(100000, 999999);

            // gửi OTP
            SendOTP otpSender = new SendOTP();
            otpSender.SendOTPEmail(email, otpCode);

            MessageBox.Show("OTP đã gửi đến email!");
            OTP otpForm = new OTP(email, otpCode);
            otpForm.Show();
            this.Hide();
        }
    }
}
