using LapTrinhWeb_TuDienWeb.App_Code.DAL;
using LapTrinhWeb_TuDienWeb.App_Code.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.BLL
{
    public class SearchService
    {
        /// Tìm kiếm từ và lưu lịch sử (nếu user đã đăng nhập)
        public static List<Word> SearchAndSaveHistory(string keyword, int? userId)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Word>();

            // Tìm kiếm từ
            List<Word> results = WordRepository.SearchWords(keyword.Trim());

            // Nếu user đã đăng nhập và có kết quả, lưu lịch sử cho từ đầu tiên
            if (userId.HasValue && results.Count > 0)
            {
                // Lưu lịch sử cho từ đầu tiên (khớp nhất)
                Word firstWord = results[0];
                SaveSearchHistory(userId.Value, firstWord.WordID);

                // Tăng view count
                WordRepository.IncrementViewCount(firstWord.WordID);
            }

            return results;
        }

        /// Lưu lịch sử tra cứu
        public static bool SaveSearchHistory(int userId, int wordId)
        {
            try
            {
                int historyId = HistoryRepository.AddHistory(userId, wordId);
                return historyId > 0;
            }
            catch
            {
                return false;
            }
        }

        /// Lấy lịch sử tra cứu của user
        public static List<SearchHistory> GetUserHistory(int userId)
        {
            return HistoryRepository.GetHistoryByUser(userId);
        }

        /// Lấy lịch sử gần đây
        public static List<SearchHistory> GetRecentHistory(int userId, int count)
        {
            return HistoryRepository.GetRecentHistory(userId, count);
        }

        /// Xóa một bản ghi lịch sử
        public static ServiceResult DeleteHistory(int historyId)
        {
            try
            {
                bool success = HistoryRepository.DeleteHistory(historyId);

                if (success)
                    return new ServiceResult(true, "Đã xóa lịch sử");
                else
                    return new ServiceResult(false, "Xóa lịch sử thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        /// Xóa toàn bộ lịch sử của user
        public static ServiceResult ClearHistory(int userId)
        {
            try
            {
                bool success = HistoryRepository.ClearUserHistory(userId);

                if (success)
                    return new ServiceResult(true, "Đã xóa toàn bộ lịch sử");
                else
                    return new ServiceResult(false, "Xóa lịch sử thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        /// Lấy số lượng lịch sử của user
        public static int GetHistoryCount(int userId)
        {
            return HistoryRepository.GetHistoryCount(userId);
        }

        /// Thêm từ vào danh sách yêu thích
        public static ServiceResult AddToFavorites(int userId, int wordId)
        {
            try
            {
                // Kiểm tra đã có trong favorites chưa
                if (FavoriteRepository.IsFavorite(userId, wordId))
                    return new ServiceResult(false, "Từ này đã có trong danh sách yêu thích");

                int favoriteId = FavoriteRepository.AddFavorite(userId, wordId);

                if (favoriteId > 0)
                    return new ServiceResult(true, "Đã thêm vào danh sách yêu thích");
                else
                    return new ServiceResult(false, "Thêm vào yêu thích thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        /// Xóa từ khỏi danh sách yêu thích
        public static ServiceResult RemoveFromFavorites(int userId, int wordId)
        {
            try
            {
                bool success = FavoriteRepository.RemoveFavorite(userId, wordId);

                if (success)
                    return new ServiceResult(true, "Đã xóa khỏi danh sách yêu thích");
                else
                    return new ServiceResult(false, "Xóa khỏi yêu thích thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        /// Lấy danh sách từ yêu thích
        public static List<Favorite> GetUserFavorites(int userId)
        {
            return FavoriteRepository.GetFavoritesByUser(userId);
        }

        /// Kiểm tra từ có trong danh sách yêu thích không
        public static bool IsFavorite(int userId, int wordId)
        {
            return FavoriteRepository.IsFavorite(userId, wordId);
        }

        /// Xóa toàn bộ danh sách yêu thích
        public static ServiceResult ClearFavorites(int userId)
        {
            try
            {
                bool success = FavoriteRepository.ClearUserFavorites(userId);

                if (success)
                    return new ServiceResult(true, "Đã xóa toàn bộ danh sách yêu thích");
                else
                    return new ServiceResult(false, "Xóa danh sách yêu thích thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        /// Lấy thống kê từ được tra nhiều nhất
        public static DataTable GetMostSearchedWords(int topN)
        {
            return HistoryRepository.GetMostSearchedWords(topN);
        }

        /// Lấy thống kê tra cứu theo ngày
        public static DataTable GetSearchStatsByDate(int days)
        {
            return HistoryRepository.GetSearchStatsByDate(days);
        }
    }
}