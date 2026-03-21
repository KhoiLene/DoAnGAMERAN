using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DoAnNhom_GAMERAN_
{
    public partial class Form1 : Form
    {
        private int currentUserId = -1;
        private DataClasses2DataContext db = new DataClasses2DataContext();

        public Form1(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            button1.Enabled = true; // đã login thì cho chơi luôn
        }

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false; // chưa chọn thì disable
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentUserId != -1)
            {
                FormGAME gameForm = new FormGAME(currentUserId);
                gameForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Bạn phải chọn Guest hoặc Login trước khi chơi!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn chọn chơi với tư cách Khách.\nNhấn OK để tiếp tục.",
                "Xác nhận Khách",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );

            if (result == DialogResult.OK)
            {
                currentUserId = CreateGuestUser();
                button1.Enabled = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
            this.Hide();
        }

        // ================= LINQ =================

        private int CreateGuestUser()
        {
            string guestName = "Guest" + Guid.NewGuid().ToString("N").Substring(0, 6);
            string email = guestName + "@gmail.com";

            User newUser = new User
            {
                Email = email,
                Username = guestName,
                Password = ""
            };

            db.Users.InsertOnSubmit(newUser);
            db.SubmitChanges();

            return newUser.Id;
        }
    }
}