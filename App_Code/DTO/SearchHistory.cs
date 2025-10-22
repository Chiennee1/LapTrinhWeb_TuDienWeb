using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DTO
{
    public class SearchHistory
    {
        public int HistoryID { get; set; }
        public int UserID { get; set; }
        public int WordID { get; set; }
        public DateTime SearchDate { get; set; }
        // Thông tin bổ sung 
        public string WordText { get; set; }
        public string Definition { get; set; }
        public string Username { get; set; }

        // Constructor
        public SearchHistory()
        {
            SearchDate = DateTime.Now;
        }

        public SearchHistory(int userId, int wordId)
        {
            UserID = userId;
            WordID = wordId;
            SearchDate = DateTime.Now;
        }

       
        //Lấy thời gian tra cứu dưới dạng friendly format
        public string GetTimeAgo()
        {
            TimeSpan timeSpan = DateTime.Now - SearchDate;

            if (timeSpan.TotalMinutes < 1)
                return "Vừa xong";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} phút trước";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} giờ trước";
            if (timeSpan.TotalDays < 30)
                return $"{(int)timeSpan.TotalDays} ngày trước";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} tháng trước";

            return $"{(int)(timeSpan.TotalDays / 365)} năm trước";
        }
    }
}