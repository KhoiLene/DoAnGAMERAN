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
    public partial class FormLEVEL20 : Form
    {

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        List<Circle> enemy1 = new List<Circle>();
        List<Circle> enemy2 = new List<Circle>();

        string enemyDir1Str = "right";
        string enemyDir2Str = "down";




        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random rand = new Random();
        bool isPaused = false;
        bool goLeft, goRight, goDown, goUp;

        private Database db;

        private int currentUserId; // Add this field to the FormLEVEL1 class

        public FormLEVEL20(int userId)
        {
            InitializeComponent();

            new Settings();

            db = new Database(); // dùng class Database đã viết trước đó

            currentUserId = userId; // giả định userId = 1, có thể thay bằng logic đăng nhập

            // lấy điểm cao nhất của người chơi
            int userHS = db.GetUserHighScore(currentUserId, "Level20");
            txtHighScore.Text = "High Score:" + Environment.NewLine + userHS;
            txtHighScore.ForeColor = Color.Maroon;
            txtHighScore.TextAlign = ContentAlignment.MiddleCenter;

            // lấy điểm cao nhất server
            int serverHS = db.GetServerHighScore("Level20");
            txtServerHighScore.Text = "Server High Score:" + Environment.NewLine + serverHS;
            txtServerHighScore.ForeColor = Color.DarkBlue;
            txtServerHighScore.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void FormLEVEL2_Load(object sender, EventArgs e)
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
            // ===== Enemy 1 =====
            foreach (Circle c in enemy1)
            {
                canvas.FillEllipse(Brushes.Red,
                    c.X * Settings.Width,
                    c.Y * Settings.Height,
                    Settings.Width,
                    Settings.Height);
            }

            // ===== Enemy 2 =====
            foreach (Circle c in enemy2)
            {
                canvas.FillEllipse(Brushes.Orange,
                    c.X * Settings.Width,
                    c.Y * Settings.Height,
                    Settings.Width,
                    Settings.Height);
            }


            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
            (
            food.X * Settings.Width,
            food.Y * Settings.Height,
            Settings.Width, Settings.Height
            ));

            
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

        private string ChasePlayer(Circle enemyHead)
        {
            int dx = Snake[0].X - enemyHead.X;
            int dy = Snake[0].Y - enemyHead.Y;

            // ưu tiên hướng nào xa hơn
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                return dx > 0 ? "right" : "left";
            }
            else
            {
                return dy > 0 ? "down" : "up";
            }
        }

        


        private Circle GetNextPosition(Circle head, string dir)
        {
            Circle newHead = new Circle { X = head.X, Y = head.Y };

            switch (dir)
            {
                case "left": newHead.X--; break;
                case "right": newHead.X++; break;
                case "up": newHead.Y--; break;
                case "down": newHead.Y++; break;
            }

            return newHead;
        }

        private bool IsHitPlayerBody(Circle pos)
        {
            foreach (Circle s in Snake)
            {
                if (pos.X == s.X && pos.Y == s.Y)
                    return true;
            }
            return false;
        }

        private bool IsHitEnemyBody(Circle pos, List<Circle> enemy)
        {
            foreach (var c in enemy)
            {
                if (pos.X == c.X && pos.Y == c.Y)
                    return true;
            }
            return false;
        }

        private bool IsSafe(Circle pos, List<Circle> currentEnemy)
        {
            if (pos.X < 0 || pos.X > maxWidth ||
                pos.Y < 0 || pos.Y > maxHeight)
                return false;

            if (IsHitPlayerBody(pos))
                return false;
            // đâm chính nó
            if (IsHitEnemyBody(pos, currentEnemy))
                return false;

            // đâm enemy còn lại
            if (currentEnemy == enemy1 && IsHitEnemyBody(pos, enemy2))
                return false;

            if (currentEnemy == enemy2 && IsHitEnemyBody(pos, enemy1))
                return false;

            return true;
        }


        private string GetSafeDirection(string currentDir, Circle head, List<Circle> currentEnemy)
        {
            List<string> dirs = new List<string> { "left", "right", "up", "down" };

            foreach (string dir in dirs.OrderBy(x => rand.Next()))
            {
                Circle test = GetNextPosition(head, dir);

                if (IsSafe(test, currentEnemy))
                    return dir;
            }

            return currentDir;
        }


        private void MoveEnemies()
        {
            // ===== Enemy 1 =====
            Circle head1 = enemy1[0];

            // chọn hướng
            enemyDir1Str = GetSafeDirection(enemyDir1Str, head1, enemy1);

            Circle newHead1 = GetNextPosition(head1, enemyDir1Str);

            if (!IsSafe(newHead1, enemy1))
            {
                enemyDir1Str = GetSafeDirection(enemyDir1Str, head1, enemy1);
                newHead1 = GetNextPosition(head1, enemyDir1Str);
            }

            enemy1.Insert(0, newHead1);
            enemy1.RemoveAt(enemy1.Count - 1);


            // ===== Enemy 2 =====
            Circle head2 = enemy2[0];

            enemyDir2Str = GetSafeDirection(enemyDir2Str, head2, enemy2);

            Circle newHead2 = GetNextPosition(head2, enemyDir2Str);

            if (!IsSafe(newHead2, enemy2))
            {
                enemyDir2Str = GetSafeDirection(enemyDir2Str, head2, enemy2);
                newHead2 = GetNextPosition(head2, enemyDir2Str);
            }

            enemy2.Insert(0, newHead2);
            enemy2.RemoveAt(enemy2.Count - 1);
        }



        private bool IsHitEnemy()
        {
            foreach (Circle c in enemy1)
            {
                if (Snake[0].X == c.X && Snake[0].Y == c.Y)
                    return true;
            }

            foreach (Circle c in enemy2)
            {
                if (Snake[0].X == c.X && Snake[0].Y == c.Y)
                    return true;
            }

            return false;
        }

        private bool IsSameCell(Circle a, Circle b)
        {
            int tolerance = 0; // = 0 là phải trùng hẳn

            return Math.Abs(a.X - b.X) <= tolerance &&
                   Math.Abs(a.Y - b.Y) <= tolerance;
        }
        private void TakeDamage()
        {
            // luôn mất 1 đốt
            Snake.RemoveAt(Snake.Count - 1);

            // sau khi mất → nếu hết thì chết
            if (Snake.Count == 0)
            {
                GameOver();
            }
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
            // if (Snake[0].X < 0) Snake[0].X = maxWidth;
            // if (Snake[0].X > maxWidth) Snake[0].X = 0;
            // if (Snake[0].Y < 0) Snake[0].Y = maxHeight;
            // if (Snake[0].Y > maxHeight) Snake[0].Y = 0;

            //va cham

            if (Snake[0].X < 0 || Snake[0].X > maxWidth || Snake[0].Y < 0 || Snake[0].Y > maxHeight)
            {
                GameOver();
                return;
            }


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

            // ===== Va chạm enemy =====
            //foreach (Circle c in enemy1)
            //{
            //    if (IsSameCell(Snake[0], c))
            //    {
            //        GameOver();
            //        return;
            //    }
            //}

            //foreach (Circle c in enemy2)
            //{
            //    if (IsSameCell(Snake[0], c))
            //    {
            //        GameOver();
            //        return;
            //    }
            //}

            //int damageCooldown = 0;

            //if (damageCooldown > 0)
            //    damageCooldown--;

            //if (IsHitEnemy() && damageCooldown == 0)
            //{
            //    TakeDamage();
            //    damageCooldown = 10;
            //}
            if (IsHitEnemy())
            {
                GameOver();
            }    
            MoveEnemies();

            picCanvas.Invalidate();
        }

        private void RestartGame()
        {
            // ===== Enemy 1 =====
            enemy1.Clear();

            int startX = 10;
            int startY = 10;

            for (int i = 0; i < 10; i++)
            {
                enemy1.Add(new Circle
                {
                    X = startX - i,
                    Y = startY
                });
            }

            // ===== Enemy 2 =====
            enemy2.Clear();

            int x = 20;
            int y = 5;

            for (int i = 0; i < 10; i++)
            {
                enemy2.Add(new Circle
                {
                    X = x,
                    Y = y + i
                });
            }

            maxWidth = picCanvas.Width / Settings.Width;
            maxHeight = picCanvas.Height / Settings.Height;

            Snake.Clear();

            startButton.Enabled = false;
            snapButton.Enabled = false;
            Pause.Enabled = false;
            Back.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            int safeMargin = Snake.Count + 10;
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

                // If you have obstacles, check here if head is on an obstacle and set onObstacle = true if so

            } while (onObstacle);

            Snake.Add(head);

            for (int i = 0; i < 4; i++)
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



            db.SaveScore(currentUserId, "Level20", score);

            int userHS = db.GetUserHighScore(currentUserId, "Level20");

            txtHighScore.Text = "High Score:" + Environment.NewLine + userHS;

            int serverHS = db.GetServerHighScore("Level20");
            txtServerHighScore.Text = "Server High Score:" + Environment.NewLine + serverHS;

        }
    }
}