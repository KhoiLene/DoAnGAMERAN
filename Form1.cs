using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DoAnNhom_GAMERAN_
{
    public partial class Form1 : Form
    {
        private string connectionString =
            "Data Source=MSI\\SQLEXPRESS;Initial Catalog=gameRan;Integrated Security=True;";

        private int currentUserId = -1; // mặc định chưa chọn

        public Form1(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
        }

        public Form1()
        {
            InitializeComponent();

            // Lúc đầu, nút Start bị disable
            button1.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Chỉ cho chơi nếu đã chọn Guest hoặc Login
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
                button1.Enabled = true; // bật nút Start
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FormLogin loginForm = new FormLogin();
            loginForm.Show();   // mở form đăng nhập
            this.Hide();        // ẩn Form1
        }

        private int CreateGuestUser()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string guestName = "Guest" + Guid.NewGuid().ToString("N").Substring(0, 6);

                string insertUser = @"INSERT INTO Users (Email, Username, Password) 
                      OUTPUT INSERTED.Id 
                      VALUES (@Email, @Username, @Password)";

                using (SqlCommand cmd = new SqlCommand(insertUser, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", "guest@gmail.com"); // hoặc random
                    cmd.Parameters.AddWithValue("@Username", guestName);
                    cmd.Parameters.AddWithValue("@Password", "");

                    int newId = (int)cmd.ExecuteScalar();
                    return newId;
                }
            }
        }
    }
}