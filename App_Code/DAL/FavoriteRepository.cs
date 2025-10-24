using LapTrinhWeb_TuDienWeb.App_Code.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DAL
{
    public class FavoriteRepository
    {
        /// Thêm từ vào danh sách yêu thích
        public static int AddFavorite(int userId, int wordId)
        {
            if (IsFavorite(userId, wordId))
            {
                return 0; 
            }

            string query = @"INSERT INTO Favorites (UserID, WordID, AddedDate)
                           VALUES (@UserID, @WordID, @AddedDate);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@WordID", wordId),
                new SqlParameter("@AddedDate", DateTime.Now)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        /// Xóa từ khỏi danh sách yêu thích
        public static bool RemoveFavorite(int userId, int wordId)
        {
            string query = "DELETE FROM Favorites WHERE UserID = @UserID AND WordID = @WordID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@WordID", wordId)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// Xóa một favorite theo ID
        public static bool DeleteFavorite(int favoriteId)
        {
            string query = "DELETE FROM Favorites WHERE FavoriteID = @FavoriteID";

            SqlParameter[] parameters = {
                new SqlParameter("@FavoriteID", favoriteId)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// Lấy danh sách từ yêu thích của user
        public static List<Favorite> GetFavoritesByUser(int userId)
        {
            List<Favorite> favorites = new List<Favorite>();

            string query = @"SELECT f.*, w.WordText, w.Pronunciation, w.Definition, w.PartOfSpeech
                           FROM Favorites f
                           INNER JOIN Words w ON f.WordID = w.WordID
                           WHERE f.UserID = @UserID
                           ORDER BY f.AddedDate DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                favorites.Add(MapDataRowToFavorite(row));
            }

            return favorites;
        }

        /// Kiểm tra từ đã có trong danh sách yêu thích chưa
        public static bool IsFavorite(int userId, int wordId)
        {
            string query = @"SELECT COUNT(*) FROM Favorites 
                           WHERE UserID = @UserID AND WordID = @WordID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@WordID", wordId)
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        /// Đếm số lượng từ yêu thích của user
        public static int GetFavoriteCount(int userId)
        {
            string query = "SELECT COUNT(*) FROM Favorites WHERE UserID = @UserID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId)
            };

            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
        }

        /// Xóa toàn bộ danh sách yêu thích của user
        public static bool ClearUserFavorites(int userId)
        {
            string query = "DELETE FROM Favorites WHERE UserID = @UserID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// Lấy danh sách từ yêu thích với phân trang
        public static List<Favorite> GetFavoritesPaged(int userId, int pageIndex, int pageSize)
        {
            List<Favorite> favorites = new List<Favorite>();

            string query = @"SELECT f.*, w.WordText, w.Pronunciation, w.Definition, w.PartOfSpeech
                           FROM Favorites f
                           INNER JOIN Words w ON f.WordID = w.WordID
                           WHERE f.UserID = @UserID
                           ORDER BY f.AddedDate DESC
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Offset", pageIndex * pageSize),
                new SqlParameter("@PageSize", pageSize)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                favorites.Add(MapDataRowToFavorite(row));
            }

            return favorites;
        }

        
        private static Favorite MapDataRowToFavorite(DataRow row)
        {
            return new Favorite
            {
                FavoriteID = Convert.ToInt32(row["FavoriteID"]),
                UserID = Convert.ToInt32(row["UserID"]),
                WordID = Convert.ToInt32(row["WordID"]),
                AddedDate = Convert.ToDateTime(row["AddedDate"]),
                WordText = row["WordText"] != DBNull.Value ? row["WordText"].ToString() : null,
                Pronunciation = row["Pronunciation"] != DBNull.Value ? row["Pronunciation"].ToString() : null,
                Definition = row["Definition"] != DBNull.Value ? row["Definition"].ToString() : null,
                PartOfSpeech = row["PartOfSpeech"] != DBNull.Value ? row["PartOfSpeech"].ToString() : null
            };
        }
    }
}