using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LapTrinhWeb_TuDienWeb.App_Code.DTO;
namespace LapTrinhWeb_TuDienWeb.Utils
{
    // Tầng để quản lý các session cho User khi sử dụng
    public class SessionHelper
    {
        private const string SESSION_USER_ID = "UserID";
        private const string SESSION_USERNAME = "Username";
        private const string SESSION_FULLNAME = "FullName";
        private const string SESSION_ROLE = "Role";
        private const string SESSION_EMAIL = "Email";
        //Lưu thông tin của user khi đăng nhập
        public static void SetUserSession(User user)
        {
            if(user != null)
            {
                HttpContext.Current.Session[SESSION_USER_ID] = user.UserID;
                HttpContext.Current.Session[SESSION_USERNAME] = user.UserName;
                HttpContext.Current.Session[SESSION_FULLNAME] = user.FullName;
                HttpContext.Current.Session[SESSION_ROLE] = user.Role;
                HttpContext.Current.Session[SESSION_EMAIL] = user.Email;
            }
        }
        // Xóa session khi đăng xuất
        public static void ClearUserSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
        // Lấy Role từ session
        public static string GetRole()
        {
            return HttpContext.Current.Session[SESSION_ROLE]?.ToString();
        }
        public static string GetEmail()
        {
            return HttpContext.Current.Session[SESSION_EMAIL]?.ToString();
        }
        // Kiểm tra user dăng nhập 
        public static bool IsLoggedIn()
        {
            return HttpContext.Current.Session[SESSION_USER_ID] != null;
        }
        // Kiểm tra user có phải admin không
        public static bool IsAdmin()
        {
            string role = GetRole();
            return role != null && role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }
        // Lấy thông tin user từ session
        public static string GetUsername()
        {
            return HttpContext.Current.Session[SESSION_USERNAME]?.ToString();
        }
        public static string GetFullName()
        {
            return HttpContext.Current.Session[SESSION_FULLNAME]?.ToString();
        }
        // Kiểm tra quyền Admin
        public static void CheckAdminAccess()
        {
            if (!IsLoggedIn())
            {
                HttpContext.Current.Response.Redirect("~/Client/Login.aspx");
            }
            else if (!IsAdmin())
            {
                HttpContext.Current.Response.Redirect("~/Client/Default.aspx");
            }
        }
        public static void RequireLogin()
        {
            if (!IsLoggedIn())
            {
                HttpContext.Current.Response.Redirect("~/Client/Login.aspx");
            }
        }
        // Cập nhấth thông tin User trong session
        public static void UpdateUserInfo(string fullName, string email)
        {
            if (IsLoggedIn())
            {
                HttpContext.Current.Session[SESSION_FULLNAME] = fullName;
                HttpContext.Current.Session[SESSION_EMAIL] = email;
            }
        }
    }
}