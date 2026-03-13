using System;
using System.Data.SqlClient;

namespace DoAnNhom_GAMERAN_
{
public class DatabaseHelper
{
    private string connectionString =
        "Data Source=MSI\\SQLEXPRESS;Initial Catalog=gameRan;Integrated Security=True;";


    // Lưu điểm của người chơi
    public void SaveScore(int userId, string levelName, int score)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // Kiểm tra userId có tồn tại trong bảng Users không
            string checkUser = "SELECT COUNT(*) FROM Users WHERE Id = @UserId";
            using (SqlCommand cmdCheck = new SqlCommand(checkUser, conn))
            {
                cmdCheck.Parameters.AddWithValue("@UserId", userId);
                int exists = (int)cmdCheck.ExecuteScalar();
                if (exists == 0)
                {
                    throw new Exception($"UserId {userId} không tồn tại trong bảng Users!");
                }
            }

            // 1. Lưu điểm vào HighScores2 (lịch sử điểm người chơi)
            string insertScore = "INSERT INTO HighScores2 (UserId, LevelName, Score) VALUES (@UserId, @LevelName, @Score)";
            using (SqlCommand cmd = new SqlCommand(insertScore, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@LevelName", levelName);
                cmd.Parameters.AddWithValue("@Score", score);
                cmd.ExecuteNonQuery();
            }

            // 2. Kiểm tra điểm cao nhất hiện tại trong HighScores (server)
            string checkHighScore = "SELECT Score FROM HighScores WHERE LevelName = @LevelName";
            int currentHigh = -1;
            using (SqlCommand cmd = new SqlCommand(checkHighScore, conn))
            {
                cmd.Parameters.AddWithValue("@LevelName", levelName);
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                    currentHigh = Convert.ToInt32(result);
            }

            // 3. Nếu chưa có hoặc điểm mới cao hơn → cập nhật HighScores
            if (currentHigh < score)
            {
                string upsertHighScore = @"
                    IF EXISTS (SELECT 1 FROM HighScores WHERE LevelName = @LevelName)
                        UPDATE HighScores SET Score = @Score WHERE LevelName = @LevelName
                    ELSE
                        INSERT INTO HighScores (LevelName, Score) VALUES (@LevelName, @Score)";
                using (SqlCommand cmd = new SqlCommand(upsertHighScore, conn))
                {
                    cmd.Parameters.AddWithValue("@LevelName", levelName);
                    cmd.Parameters.AddWithValue("@Score", score);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    // Lấy điểm cao nhất của người chơi cho một level
    public int GetUserHighScore(int userId, string levelName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT MAX(Score) FROM HighScores2 WHERE UserId = @UserId AND LevelName = @LevelName";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@LevelName", levelName);
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }

    // Lấy điểm cao nhất server cho một level
    public int GetServerHighScore(string levelName)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT Score FROM HighScores WHERE LevelName = @LevelName";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@LevelName", levelName);
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
    }
    public int LoginUser(string username, string password)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT Id FROM Users WHERE Username=@user AND Password=@pass";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@user", username);
            cmd.Parameters.AddWithValue("@pass", password);

            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }
    }

    // Đăng ký: trả về UserId mới, -1 nếu username đã tồn tại
    public int RegisterUser(string username, string password)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string checkUser = "SELECT COUNT(*) FROM Users WHERE Username=@Username";
            SqlCommand cmdCheck = new SqlCommand(checkUser, conn);
            cmdCheck.Parameters.AddWithValue("@Username", username);
            int exists = (int)cmdCheck.ExecuteScalar();
            if (exists > 0) return -1;

            string insertUser = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password); SELECT SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(insertUser, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

}
}