using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DoAnNhom_GAMERAN_
{
    public partial class OTP : Form
    {
        int otpCode;
        string userEmail;

        public OTP(string email, int otp)
        {
            InitializeComponent();
            userEmail = email;
            otpCode = otp;
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (txtOTP.Text == otpCode.ToString())
            {
                MessageBox.Show("OTP đúng!");
                NewPassword form = new NewPassword(userEmail);
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
