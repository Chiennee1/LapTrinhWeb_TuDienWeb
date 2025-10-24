using LapTrinhWeb_TuDienWeb.App_Code.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DAL
{
    public class UserRepository
    {
        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string query = "SELECT * FROM Users ORDER BY CreatedDate DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                users.Add(MapDataRowToUser(row));
            }

            return users;
        }
        // lấy user theo ID
        public static User GetUserById(int userId)
        {
            string query = "SELECT * FROM Users WHERE UserID = @UserID";
            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToUser(dt.Rows[0]);
            }

            return null;
        }
        // Lấy user theo username
        public static User GetUserByUsername(string username)
        {
            string query = "SELECT * FROM Users WHERE Username = @Username";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToUser(dt.Rows[0]);
            }

            return null;
        }
        
        // Chuyên đổi DataRow thành User
        private static User MapDataRowToUser(DataRow row)
        {
            return new User
            {
                UserID = Convert.ToInt32(row["UserID"]),
                UserName = row["Username"].ToString(),
                Password = row["Password"].ToString(),
                Email = row["Email"].ToString(),
                FullName = row["FullName"] != DBNull.Value ? row["FullName"].ToString() : null,
                Role = row["Role"].ToString(),
                IsActive = Convert.ToBoolean(row["IsActive"]),
                CreateDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }
        // Đăng nhập người dùng
        public static User Login(string username, string password)
        {
            string query = @"select * form Users 
                           where Username = @Username 
                           and Password = @Password 
                           and IsActive = 1";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToUser(dt.Rows[0]);
            }

            return null;
        }
        // Thêm người dùng mới
        public static int AddUser(User user)
        {
            string query = @"INSERT INTO Users (Username, Password, Email, FullName, Role, IsActive, CreatedDate)
                           VALUES (@Username, @Password, @Email, @FullName, @Role, @IsActive, @CreatedDate);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@FullName", user.FullName ?? (object)DBNull.Value),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@CreatedDate", user.CreateDate)
            };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // Cập nhật thông tin người dùng
        public static bool UpdateUser(User user)
        {
            string query = @"UPDATE Users SET 
                           Username = @Username,
                           Email = @Email,
                           FullName = @FullName,
                           Role = @Role,
                           IsActive = @IsActive
                           WHERE UserID = @UserID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", user.UserID),
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@FullName", user.FullName ?? (object)DBNull.Value),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@IsActive", user.IsActive)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
        // Cập nhật mật khẩu người dùng
        public static bool UpdatePassword(int userId, string newPassword)
        {
            string query = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Password", newPassword)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
        // Xoá người dùng vĩnh viễn
        public static bool PermanentDeleteUser(int userId)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";

            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userId)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
        // Kiểm tra username đã tồn tại chưa
        public static bool IsUsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", username)
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }
        //List người dùng theo trang
        public static List<User> GetUsersPaged(int pageIndex, int pageSize)
        {
            List<User> users = new List<User>();

            string query = @"SELECT * FROM Users 
                           WHERE IsActive = 1
                           ORDER BY CreatedDate DESC
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";

            SqlParameter[] parameters = {
                new SqlParameter("@Offset", pageIndex * pageSize),
                new SqlParameter("@PageSize", pageSize)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                users.Add(MapDataRowToUser(row));
            }

            return users;
        }
        // Kiểm tra email đã tồn tại chưa
        public static bool IsEmailExists(string email)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

            SqlParameter[] parameters = {
                new SqlParameter("@Email", email)
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }
    }
}