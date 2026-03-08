using System;
using System.Data;
using System.Data.SqlClient;

public class HighScoreDB
{
    private readonly string connStr = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=gameRan;Integrated Security=True;";

    // Lấy điểm cao nhất server theo level
    public int GetHighScore(string levelName)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT ISNULL(MAX(Score), 0) FROM HighScores WHERE LevelName = @level";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@level", levelName);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetHighScore lỗi: " + ex.Message);
            return 0;
        }
    }

    // Cập nhật điểm cao nhất server (chỉ lưu nếu score mới cao hơn)
    public void UpdateServerHighScore(string levelName, int score)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Kiểm tra đã có record chưa
                string checkSql = "SELECT COUNT(*) FROM HighScores WHERE LevelName = @level";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@level", levelName);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        // Chưa có → INSERT
                        string insertSql = "INSERT INTO HighScores (LevelName, Score) VALUES (@level, @score)";
                        using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@level", levelName);
                            cmd.Parameters.AddWithValue("@score", score);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Đã có → UPDATE nếu score mới cao hơn
                        string updateSql = @"UPDATE HighScores 
                                             SET Score = @score 
                                             WHERE LevelName = @level AND Score < @score";
                        using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@level", levelName);
                            cmd.Parameters.AddWithValue("@score", score);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("UpdateServerHighScore lỗi: " + ex.Message);
        }
    }



    // Lưu điểm user vào HighScores2
    public void SaveUserScore(int userId, string levelName, int score)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"INSERT INTO HighScores2 (UserId, LevelName, Score)
                               VALUES (@userId, @level, @score)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@level", levelName);
                    cmd.Parameters.AddWithValue("@score", score);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("SaveUserScore lỗi: " + ex.Message);
        }
    }

    // Lấy điểm cao nhất của 1 user theo level
    public int GetUserHighScore(int userId, string levelName)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT ISNULL(MAX(Score), 0) FROM HighScores2 
                               WHERE UserId = @userId AND LevelName = @level";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@level", levelName);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetUserHighScore lỗi: " + ex.Message);
            return 0;
        }
    }

    // Lấy top 5 user điểm cao nhất theo level (join với bảng Users)
    public DataTable GetLeaderboard(string levelName)
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT TOP 5 
                                   u.Username, 
                                   MAX(h.Score) AS BestScore
                               FROM HighScores2 h
                               JOIN dbo.Users u ON u.Id = h.UserId
                               WHERE h.LevelName = @level
                               GROUP BY u.Username
                               ORDER BY BestScore DESC";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@level", levelName);
                    new SqlDataAdapter(cmd).Fill(dt);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("GetLeaderboard lỗi: " + ex.Message);
        }
        return dt;
    }
}