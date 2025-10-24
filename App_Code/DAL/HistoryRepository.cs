using LapTrinhWeb_TuDienWeb.App_Code.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DAL
{
    public class HistoryRepository
    {
        // Thêm lịch sử tra cứu
        public static int AddHistory(int userId, int wordId)
        {
            string query = @"INSERT INTO SearchHistory (UserID, WordID, SearchDate)
                           VALUES (@UserID, @WordID, @SearchDate);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@WordID", wordId),
                new SqlParameter("@SearchDate", DateTime.Now)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }
        // Lịch sử tra cứu của user
        public static List<SearchHistory> GetHistoryByUser(int userId)
        {
            List<SearchHistory> histories = new List<SearchHistory>();

            string query = @"SELECT h.*, w.WordText, w.Definition 
                           FROM SearchHistory h
                           INNER JOIN Words w ON h.WordID = w.WordID
                           WHERE h.UserID = @UserID
                           ORDER BY h.SearchDate DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                histories.Add(MapDataRowToHistory(row));
            }

            return histories;
        }
        // Lịch sử tra cứu của user với bao nhiêu 
        public static List<SearchHistory> GetRecentHistory(int userId, int count)
        {
            List<SearchHistory> histories = new List<SearchHistory>();

            string query = @"SELECT TOP (@Count) h.*, w.WordText, w.Definition 
                           FROM SearchHistory h
                           INNER JOIN Words w ON h.WordID = w.WordID
                           WHERE h.UserID = @UserID
                           ORDER BY h.SearchDate DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Count", count)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                histories.Add(MapDataRowToHistory(row));
            }

            return histories;
        }
        // Xóa một bản ghi lịch sử
        public static bool DeleteHistory(int historyId)
        {
            string query = "Delete from SearchHistory where HistoryID = @HistoryID";
            SqlParameter[] parameters = {
                new SqlParameter("@HistoryID", historyId) };
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
        // Xóa toàn bộ lịch sử của user 
        public static bool ClearUserHistory(int userId)
        {
            string query = "Delete from SearchHistory where UserID = @UserID";
            SqlParameter[] sqlParameter =
            {
                new SqlParameter("@UserID",userId) 
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, sqlParameter);
            return rowsAffected > 0;
        }
        // Đếm số lượng lịch sử của user
        public static int GetHistoryCount(int userId)
        {
            string query = "select count(*) from SearchHistory where UserID = @UserID";
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", userId)
            };
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        // Tổng số lượt tra cứu
        public static int GetTotalSearches()
        {
            string query = "SELECT COUNT(*) FROM SearchHistory";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        
        /// Kiểm tra user đã tra từ này chưa
        public static bool HasSearched(int userId, int wordId)
        {
            string query = @"SELECT COUNT(*) FROM SearchHistory 
                           WHERE UserID = @UserID AND WordID = @WordID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@WordID", wordId)
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

      
        /// Lấy danh sách từ được tra nhiều nhất (thống kê)
        public static DataTable GetMostSearchedWords(int topN)
        {
            string query = @"SELECT TOP (@TopN) 
                           w.WordText, 
                           w.Definition,
                           COUNT(h.HistoryID) as SearchCount
                           FROM SearchHistory h
                           INNER JOIN Words w ON h.WordID = w.WordID
                           GROUP BY w.WordText, w.Definition
                           ORDER BY SearchCount DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@TopN", topN)
            };

            return DatabaseHelper.ExecuteQuery(query, parameters);
        }

        // Thống kê lượt tra cứu theo ngày
        public static DataTable GetSearchStatsByDate(int days)
        {
            string query = @"SELECT 
                           CAST(SearchDate AS DATE) as SearchDay,
                           COUNT(*) as SearchCount
                           FROM SearchHistory
                           WHERE SearchDate >= DATEADD(day, -@Days, GETDATE())
                           GROUP BY CAST(SearchDate AS DATE)
                           ORDER BY SearchDay DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@Days", days)
            };

            return DatabaseHelper.ExecuteQuery(query, parameters);
        }
        private static SearchHistory MapDataRowToHistory(DataRow row)
        {
            return new SearchHistory
            {
                HistoryID = Convert.ToInt32(row["HistoryID"]),
                UserID = Convert.ToInt32(row["UserID"]),
                WordID = Convert.ToInt32(row["WordID"]),
                SearchDate = Convert.ToDateTime(row["SearchDate"]),
                WordText = row["WordText"] != DBNull.Value ? row["WordText"].ToString() : null,
                Definition = row["Definition"] != DBNull.Value ? row["Definition"].ToString() : null
            };
        }
    }
}