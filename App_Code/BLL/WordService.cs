using LapTrinhWeb_TuDienWeb.App_Code.DAL;
using LapTrinhWeb_TuDienWeb.App_Code.DTO;
using LapTrinhWeb_TuDienWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.BLL
{
    public class WordService
    {
        // Tìm kiếm từ vựng
        public static List<Word> SearchWords(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Word>();

            keyword = keyword.Trim();
            return WordRepository.SearchWords(keyword);
        }
        // Lấy từ vựng theo ID
        public static Word GetWordById(int wordId)
        {
            return WordRepository.GetWordById(wordId);
        }
        // Lấy từ vựng theo text
        public static Word GetWordByText(string wordText)
        {
            if (string.IsNullOrWhiteSpace(wordText))
                return null;

            return WordRepository.GetWordByText(wordText.Trim());
        }

        // Thêm từ vựng mới (Admin)
        public static ServiceResult AddWord(Word word)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(word.WordText))
                return new ServiceResult(false, "Từ vựng không được để trống");

            if (string.IsNullOrWhiteSpace(word.Definition))
                return new ServiceResult(false, "Định nghĩa không được để trống");

            // Kiểm tra từ đã tồn tại
            if (WordRepository.IsWordExists(word.WordText.Trim()))
                return new ServiceResult(false, "Từ vựng đã tồn tại trong từ điển");

            try
            {
                word.WordText = word.WordText.Trim();
                word.CreatedDate = DateTime.Now;
                word.ViewCount = 0;

                int wordId = WordRepository.AddWord(word);

                if (wordId > 0)
                    return new ServiceResult(true, "Thêm từ vựng thành công", wordId);
                else
                    return new ServiceResult(false, "Thêm từ vựng thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        // Cập nhật từ vựng (Admin)
        public static ServiceResult UpdateWord(Word word)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(word.WordText))
                return new ServiceResult(false, "Từ vựng không được để trống");

            if (string.IsNullOrWhiteSpace(word.Definition))
                return new ServiceResult(false, "Định nghĩa không được để trống");

            try
            {
                word.WordText = word.WordText.Trim();
                bool success = WordRepository.UpdateWord(word);

                if (success)
                    return new ServiceResult(true, "Cập nhật từ vựng thành công");
                else
                    return new ServiceResult(false, "Cập nhật từ vựng thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        // Xóa từ vựng (Admin)
        public static ServiceResult DeleteWord(int wordId)
        {
            try
            {
                bool success = WordRepository.DeleteWord(wordId);

                if (success)
                    return new ServiceResult(true, "Xóa từ vựng thành công");
                else
                    return new ServiceResult(false, "Xóa từ vựng thất bại");
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, "Lỗi: " + ex.Message);
            }
        }

        // Lấy tất cả từ vựng
        public static List<Word> GetAllWords()
        {
            return WordRepository.GetAllWords();
        }

        // Lấy từ vựng với phân trang
        public static List<Word> GetWordsPaged(int pageIndex, int pageSize)
        {
            return WordRepository.GetWordsPaged(pageIndex, pageSize);
        }

        // Lấy tổng số từ
        public static int GetTotalWords()
        {
            return WordRepository.GetTotalWords();
        }

        // Lấy từ theo category
        public static List<Word> GetWordsByCategory(string category)
        {
            return WordRepository.GetWordsByCategory(category);
        }

        // Lấy từ theo level
        public static List<Word> GetWordsByLevel(string level)
        {
            return WordRepository.GetWordsByLevel(level);
        }

        // Lấy top từ được xem nhiều nhất
        public static List<Word> GetTopViewedWords(int topN)
        {
            return WordRepository.GetTopViewedWords(topN);
        }

        // Lấy từ mới nhất
        public static List<Word> GetLatestWords(int count)
        {
            return WordRepository.GetLatestWords(count);
        }

        // Tăng view count khi user xem từ
        public static void IncrementViewCount(int wordId)
        {
            WordRepository.IncrementViewCount(wordId);
        }

        // Import từ vựng từ danh sách (dùng khi import Excel)
        public static ServiceResult ImportWords(List<Word> words)
        {
            if (words == null || words.Count == 0)
                return new ServiceResult(false, "Danh sách từ vựng trống");

            int successCount = 0;
            int failCount = 0;
            List<string> errors = new List<string>();

            foreach (var word in words)
            {
                try
                {
                    if (WordRepository.IsWordExists(word.WordText))
                    {
                        failCount++;
                        errors.Add($"Từ '{word.WordText}' đã tồn tại");
                        continue;
                    }

                    word.CreatedDate = DateTime.Now;
                    word.ViewCount = 0;

                    int wordId = WordRepository.AddWord(word);
                    if (wordId > 0)
                        successCount++;
                    else
                        failCount++;
                }
                catch (Exception ex)
                {
                    failCount++;
                    errors.Add($"Lỗi khi thêm từ '{word.WordText}': {ex.Message}");
                }
            }

            string message = $"Import hoàn tất: {successCount} thành công, {failCount} thất bại";
            if (errors.Count > 0 && errors.Count <= 10)
            {
                message += "\n" + string.Join("\n", errors);
            }

            return new ServiceResult(successCount > 0, message, new { successCount, failCount });
        }
    }
}