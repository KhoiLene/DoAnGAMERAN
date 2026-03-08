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
    public partial class FormGAME : Form
    {
        private int currentUserId;
        int currentPage = 0;
        int levelsPerPage = 8;
        int totalLevels = 20;
        int maxPage;

        public FormGAME()
        {
            InitializeComponent();
            InitLevels();
        }

        public FormGAME(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            InitLevels();
        }

        private void InitLevels()
        {
            maxPage = (int)Math.Ceiling((double)totalLevels / levelsPerPage) - 1;
            ShowLevels(currentPage);

            // Gán click event cho tất cả các nút level
            button1.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 1);
            button2.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 2);
            button3.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 3);
            button4.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 4);
            button5.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 5);
            button6.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 6);
            button7.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 7);
            button8.Click += (s, e) => OpenLevel(currentPage * levelsPerPage + 8);
        }

        private void OpenLevel(int level)
        {
            Form levelForm = null;

            switch (level)
            {
                case 1: levelForm = new FormLEVEL1(currentUserId); break;
                case 2: levelForm = new FormLEVEL2(currentUserId); break;
                case 3: levelForm = new FormLEVEL3(currentUserId); break;
                // Thêm các level khác ở đây khi bạn tạo form
                // case 4:  levelForm = new FormLEVEL4(currentUserId); break;
                // case 5:  levelForm = new FormLEVEL5(currentUserId); break;
                // ...
                default:
                    MessageBox.Show("Level " + level + " chưa được mở khóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
            }

            if (levelForm != null)
            {
                levelForm.Show();
                this.Close();
            }
        }

        private void ShowLevels(int page)
        {
            int startLevel = page * levelsPerPage + 1;

            Button[] buttons = { button1, button2, button3, button4,
                                  button5, button6, button7, button8 };

            for (int i = 0; i < buttons.Length; i++)
            {
                int lvl = startLevel + i;
                buttons[i].Text = "LEVEL " + lvl;
                buttons[i].Visible = lvl <= totalLevels;
            }

            // Cập nhật trạng thái nút Left/Right
            btnLeft.Enabled = currentPage > 0;
            btnRight.Enabled = currentPage < maxPage;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form1 menu = new Form1();
            menu.Show();
            this.Close();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                ShowLevels(currentPage);
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (currentPage < maxPage)
            {
                currentPage++;
                ShowLevels(currentPage);
            }
        }

        private void FormGAME_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}