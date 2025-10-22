using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DTO
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }

        // Constructor
        public User()
        {
            Role = "User";
            IsActive = true;
            CreateDate = DateTime.Now;

        }
        public User(int userID, string userName, string password, string email, string fullName)
        {
            UserID = userID;
            UserName = userName;
            Password = password;
            Email = email;
            FullName = fullName;
            Role = "User";
            IsActive = true;
            CreateDate = DateTime.Now;
        }
        public bool IsAdmin()
        {
            return Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

    }
}