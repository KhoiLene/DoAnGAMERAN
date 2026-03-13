using System;
using System.Data.SqlClient;
using System.Linq;

namespace DoAnNhom_GAMERAN_
{
    public class Database
    {
        DataClasses2DataContext db = new DataClasses2DataContext();


        // Lưu điểm của người chơi
        public void SaveScore(int userId, string levelName, int score)
        {
            // 1. Kiểm tra user có tồn tại
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new Exception("User không tồn tại");

            // 2. Lưu lịch sử điểm vào HighScores2
            HighScores2 newScore = new HighScores2
            {
                UserId = userId,
                LevelName = levelName,
                Score = score
            };

            db.HighScores2s.InsertOnSubmit(newScore);
            db.SubmitChanges();

            // 3. Lấy điểm cao nhất server
            var serverHigh = db.HighScores
                               .FirstOrDefault(h => h.LevelName == levelName);

            // 4. Nếu chưa có record
            if (serverHigh == null)
            {
                HighScore hs = new HighScore
                {
                    LevelName = levelName,
                    Score = score
                };

                db.HighScores.InsertOnSubmit(hs);
            }
            else
            {
                // nếu điểm mới cao hơn thì update
                if (score > serverHigh.Score)
                {
                    serverHigh.Score = score;
                }
            }

            db.SubmitChanges();
        }

        // Lấy điểm cao nhất của người chơi cho một level
        public int GetUserHighScore(int userId, string levelName)
        {
            var score = db.HighScores2s
                          .Where(s => s.UserId == userId && s.LevelName == levelName)
                          .Max(s => (int?)s.Score);

            return score ?? 0;
        }

        // Lấy điểm cao nhất server cho một level
        public int GetServerHighScore(string levelName)
        {
            var score = db.HighScores
                          .Where(s => s.LevelName == levelName)
                          .Select(s => (int?)s.Score)
                          .FirstOrDefault();

            return score ?? 0;
        }
        public int LoginUser(string username, string password)
        {
            var user = db.Users
                         .FirstOrDefault(u => u.Username == username && u.Password == password);

            return user != null ? user.Id : -1;
        }

        // Đăng ký: trả về UserId mới, -1 nếu username đã tồn tại
        public int RegisterUser(string username, string password)
        {
            var exists = db.Users.Any(u => u.Username == username);

            if (exists) return -1;

            User newUser = new User
            {
                Username = username,
                Password = password
            };

            db.Users.InsertOnSubmit(newUser);
            db.SubmitChanges();

            return newUser.Id;
        }

    }
}