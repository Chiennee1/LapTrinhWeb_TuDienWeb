using LapTrinhWeb_TuDienWeb.App_Code.DAL;
using LapTrinhWeb_TuDienWeb.App_Code.DTO;
using LapTrinhWeb_TuDienWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.BLL
{
    public class UserService
    {
        // Register
        public static ServiceResult Register(string username, string email, string password, string confirmpass, string fullName)
        {
            if (string.IsNullOrEmpty(username))
                return new ServiceResult(false, "Username không được để trống");
            if (string.IsNullOrWhiteSpace(email))
                return new ServiceResult(false, "Email không được để trống");

            if (string.IsNullOrWhiteSpace(password))
                return new ServiceResult(false, "Password không được để trống");

            if (password != confirmpass)
                return new ServiceResult(false, "Mật khẩu xác nhận không khớp");
            
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]{3,50}$"))
                return new ServiceResult(false, "Username phải từ 3-50 ký tự, chỉ bao gồm chữ, số và dấu gạch dưới");
            if(password.Length < 6 || password.Length > 100)
                return new ServiceResult(false, "Mật khẩu phải từ 6-100 ký tự");
            if(UserRepository.IsUsernameExists(username))
                return new ServiceResult(false, "Username đã tồn tại");
            try
            {
                string hashedPassword = SecurityHelper.HashPassword(password);
                User newUser = new User
                {
                    UserName = username,
                    Email = email,
                    Password = hashedPassword,
                    FullName = fullName,
                    Role = "User",
                    IsActive = true,
                    CreateDate = DateTime.Now
                };
                int userID = UserRepository.AddUser(newUser);
                if (userID > 0)
                    return new ServiceResult(true, "Đăng ký tài khoản thành công", userID);
                else return new ServiceResult(false, "Đăng ký thất bại!");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }
       // Login
        public static ServiceResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new ServiceResult(false, "Username và Password không được để trống");
            try
            {
                string hashedPassword = SecurityHelper.HashPassword(password);
                User user = UserRepository.Login(username, hashedPassword);
                if (user != null)
                    return new ServiceResult(true, "Đăng nhập thành công", user);
                else
                    return new ServiceResult(false, "Username hoặc Password không đúng");

            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }
        // cập nhật thông tin 
        public static ServiceResult UpdateProfile(int userId, string email, string fullName)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new ServiceResult(false, "Email không được để trống");

            try
            {
                User user = UserRepository.GetUserById(userId);
                if (user == null)
                    return new ServiceResult(false, "User không tồn tại");

                User existingUser = UserRepository.GetUserByUsername(user.UserName);
                if (existingUser != null && existingUser.Email != email)
                {
                    if (UserRepository.IsEmailExists(email))
                        return new ServiceResult(false, "Email đã được sử dụng bởi tài khoản khác");
                }

                user.Email = email;
                user.FullName = fullName;

                bool success = UserRepository.UpdateUser(user);

                if (success)
                    return new ServiceResult(true, "Cập nhật thành công");
                else
                    return new ServiceResult(false, "Cập nhật thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }
        // đổi mật khẩu
        public static ServiceResult ChangePassword(int userId, string currentPassword, string newPassword, string confirmNewPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmNewPassword))
                return new ServiceResult(false, "Mật khẩu không được để trống");
            if (newPassword != confirmNewPassword)
                return new ServiceResult(false, "Mật khẩu xác nhận không khớp");
            try
            {
                User user = UserRepository.GetUserById(userId);
                if (user == null)
                    return new ServiceResult(false, "User không tồn tại");

                string hashedCurrentPassword = SecurityHelper.HashPassword(currentPassword);
                if (user.Password != hashedCurrentPassword)
                    return new ServiceResult(false, "Mật khẩu hiện tại không đúng");
                
                string hashedNewPassword = SecurityHelper.HashPassword(newPassword);
                bool success = UserRepository.UpdatePassword(userId, hashedNewPassword);
                if (success)
                    return new ServiceResult(true, "Đổi mật khẩu thành công");
                else
                    return new ServiceResult(false, "Đổi mật khẩu thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }
        // Lấy thông tin user theo ID
        public static User GetUserById(int userId)
        {
            return UserRepository.GetUserById(userId);
        }
        // lấy danh sách tất cả user
        public static List<User> GetAllUsers()
        {
            return UserRepository.GetAllUsers();
        }
        // Khóa/mở tài khoản Admin
        public static ServiceResult ToggleUserStatus(int userId)
        {
            try
            {
                User user = UserRepository.GetUserById(userId);
                if (user == null)
                    return new ServiceResult(false, "User không tồn tại");

                user.IsActive = !user.IsActive;
                bool success = UserRepository.UpdateUser(user);

                if (success)
                    return new ServiceResult(true, user.IsActive ? "Đã mở khóa tài khoản" : "Đã khóa tài khoản");
                else
                    return new ServiceResult(false, "Thao tác thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }
        // Xóa user
        public static ServiceResult DeleteUser(int userId)
        {
            try
            {
                bool success = UserRepository.PermanentDeleteUser(userId);

                if (success)
                    return new ServiceResult(true, "Xóa user thành công");
                else
                    return new ServiceResult(false, "Xóa user thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

    }
}