using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.Utils
{
    public class SecurityHelper
    {
        // Băm mật khẩu sử dụng SHA256
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        // Kiểm tra chuỗi có chứa dấu hiệu SQL Injection không
        public static bool ContainsSQLInjection(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string[] sqlKeywords = {
                "--", ";--", "';", "/*", "*/", "@@", "@",
                "char", "nchar", "varchar", "nvarchar",
                "alter", "begin", "cast", "create", "cursor",
                "declare", "delete", "drop", "end", "exec",
                "execute", "fetch", "insert", "kill", "select",
                "sys", "sysobjects", "syscolumns", "table", "update",
                "union", "xp_", "sp_"
            };

            string lowerInput = input.ToLower();

            foreach (string keyword in sqlKeywords)
            {
                if (lowerInput.Contains(keyword.ToLower()))
                    return true;
            }

            return false;
        }
    }
}