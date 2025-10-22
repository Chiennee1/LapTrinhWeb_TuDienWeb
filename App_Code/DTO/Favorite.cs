using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DTO
{
    public class Favorite
    {
        public int FavoriteID { get; set; }
        public int UserID { get; set; }
        public int WordID { get; set; }
        public DateTime AddedDate { get; set; }

        // Thông tin bổ sung từ bảng Word 
        public string WordText { get; set; }
        public string Pronunciation { get; set; }
        public string Definition { get; set; }
        public string PartOfSpeech { get; set; }

        // Constructor 
        public Favorite()
        {
            AddedDate = DateTime.Now;
        }

        public Favorite(int userId, int wordId)
        {
            UserID = userId;
            WordID = wordId;
            AddedDate = DateTime.Now;
        }
    }
}