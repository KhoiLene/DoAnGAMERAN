using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging; // add this for the JPG compressor
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DoAnNhom_GAMERAN_
{
    public partial class FormLEVEL1 : Form
    {

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random rand = new Random();
        bool isPaused = false;
        bool goLeft, goRight, goDown, goUp;

        private DatabaseHelper db;

        private int currentUserId; // Add this field to the FormLEVEL1 class


        

        public FormLEVEL1(int userId)
        {
            InitializeComponent();

            new Settings();

            db = new DatabaseHelper(); // dùng class DatabaseHelper đã viết trước đó

            currentUserId = userId; // giả định userId = 1, có thể thay bằng logic đăng nhập

            // lấy điểm cao nhất của người chơi
            int userHS = db.GetUserHighScore(currentUserId, "Level1");
            highScore = userHS;
            txtHighScore.Text = "High Score:" + Environment.NewLine + userHS;
            txtHighScore.ForeColor = Color.Maroon;
            txtHighScore.TextAlign = ContentAlignment.MiddleCenter;

            // lấy điểm cao nhất server
            int serverHS = db.GetServerHighScore("Level1");
            txtServerHighScore.Text = "Server High Score:" + Environment.NewLine + serverHS;
            txtServerHighScore.ForeColor = Color.DarkBlue;
            txtServerHighScore.TextAlign = ContentAlignment.MiddleCenter;
        }


        private void FormLEVEL1_Load(object sender, EventArgs e)
        {
            // tính số ô của map dựa trên kích thước picCanvas và Settings
            maxWidth = picCanvas.Width / Settings.Width;
            maxHeight = picCanvas.Height / Settings.Height;

            // hiện kích thước map
            Label mapSizeLabel = new Label();
            mapSizeLabel.Text = $"Map size: {maxWidth + 1} x {maxHeight + 1} ô";
            mapSizeLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            mapSizeLabel.ForeColor = Color.Blue;
            mapSizeLabel.AutoSize = true;
            mapSizeLabel.Location = new Point(10, 10); // góc trên trái
            this.Controls.Add(mapSizeLabel);
        }
        private void FormLEVEL1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        private void TogglePause()
        {
            if (isPaused)
            {
                gameTimer.Interval = 100;
                gameTimer.Start();
                isPaused = false;

                startButton.Enabled = false;
                snapButton.Enabled = false;
                Pause.Enabled = false;
                Back.Enabled = false;

            }
            else
            {
                gameTimer.Interval = 100;
                gameTimer.Stop();
                isPaused = true;
                startButton.Enabled = true;
                snapButton.Enabled = true;
                Pause.Enabled = true;
                Back.Enabled = true;

            }

        }

        private void StartGame(object sender, EventArgs e)
        {
            gameTimer.Interval = 100;
            RestartGame();
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            TogglePause();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            FormGAME menu = new FormGAME(currentUserId);
            menu.Show();
            this.Close();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored: " + score + " and my Highscore is " + highScore + " on the Snake Game from MOO ICT";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.Purple;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game SnapShot MOO ICT";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }


        string nextDirection = "right"; // hướng mặc định

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right")
                nextDirection = "left";
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
                nextDirection = "right";
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
                nextDirection = "up";
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
                nextDirection = "down";

            if (e.KeyCode == Keys.P)
                TogglePause();
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            Settings.directions = nextDirection;

            // di chuyen minh
            for (int i = Snake.Count - 1; i >= 1; i--)
            {
                Snake[i].X = Snake[i - 1].X;
                Snake[i].Y = Snake[i - 1].Y;
            }

            // di chuyen dau
            switch (Settings.directions)
            {
                case "left": Snake[0].X--; break;
                case "right": Snake[0].X++; break;
                case "up": Snake[0].Y--; break;
                case "down": Snake[0].Y++; break;
            }

            // goc
            if (Snake[0].X < 0) Snake[0].X = maxWidth;
            if (Snake[0].X > maxWidth) Snake[0].X = 0;
            if (Snake[0].Y < 0) Snake[0].Y = maxHeight;
            if (Snake[0].Y > maxHeight) Snake[0].Y = 0;

            // thuc an
            if (Snake[0].X == food.X && Snake[0].Y == food.Y)
            {
                EatFood();
            }

            // cham than
            for (int j = 1; j < Snake.Count; j++)
            {
                if (Snake[0].X == Snake[j].X && Snake[0].Y == Snake[j].Y)
                {
                    GameOver();
                }
            }

            picCanvas.Invalidate();
        }


        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            Brush snakeColour;

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.DarkGreen;
                }

                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
            }


            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
            (
            food.X * Settings.Width,
            food.Y * Settings.Height,
            Settings.Width, Settings.Height
            ));

            //kich thuoc map
            picCanvas.Width = 560;
            picCanvas.Height = 600;
        }



        private void GenerateFood()
        {
            Circle newFood;
            bool onSnake;

            do
            {
                onSnake = false;
                newFood = new Circle
                {
                    X = rand.Next(2, maxWidth),
                    Y = rand.Next(2, maxHeight)
                };

                // kiểm tra xem thức ăn có trùng với rắn không
                foreach (Circle part in Snake)
                {
                    if (part.X == newFood.X && part.Y == newFood.Y)
                    {
                        onSnake = true;
                        break;
                    }
                }
            } while (onSnake);

            food = newFood;
        }
        private void EatFood()
        {
            score += 1;
            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);

            GenerateFood(); // gọi hàm mới
        }


        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width;
            maxHeight = picCanvas.Height / Settings.Height;

            Snake.Clear();

            startButton.Enabled = false;
            snapButton.Enabled = false;
            Pause.Enabled = false;
            Back.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            int safeMargin = Snake.Count + 3;

            // chọn vị trí đầu rắn không trùng với vật cản
            Circle head;
            bool onObstacle;
            do
            {
                onObstacle = false;
                head = new Circle
                {
                    X = rand.Next(safeMargin, maxWidth - safeMargin),
                    Y = rand.Next(safeMargin, maxHeight - safeMargin)
                };

                // You may want to check for obstacles here, but since the original code is incomplete,
                // we'll just proceed to add the head and body as in your code.
            } while (onObstacle);

            Snake.Add(head);

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

            gameTimer.Start();
        }
        private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;
            Back.Enabled = true;

            startButton.Text = "Restart";


            db.SaveScore(currentUserId, "Level1", score);

            int userHS = db.GetUserHighScore(currentUserId, "Level1");
            txtHighScore.Text = "High Score:" + Environment.NewLine + userHS;

            int serverHS = db.GetServerHighScore("Level1");
            txtServerHighScore.Text = "Server High Score:" + Environment.NewLine + serverHS;

        }


    }
}