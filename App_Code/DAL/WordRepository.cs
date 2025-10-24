using LapTrinhWeb_TuDienWeb.App_Code.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DAL
{
    public class WordRepository
    {
        // Get All Words
        public static List<Word> GetAllWords()
        {
            List<Word> words = new List<Word>();
            string query = "SELECT * FROM Words ORDER BY WordText";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                words.Add(MapDataRowToWord(row));
            }

            return words;
        }
        // Get Word By ID
        public static Word GetWordById(int wordId)
        {
            string query = "SELECT * FROM Words WHERE WordID = @WordID";
            SqlParameter[] parameters = {
                new SqlParameter("@WordID", wordId)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToWord(dt.Rows[0]);
            }

            return null;
        }
        // Search Words By keyword
        public static List<Word> SearchWords(string keyword)
        {
            List<Word> words = new List<Word>();

            string query = @"SELECT * FROM Words 
                           WHERE WordText LIKE @Keyword 
                           OR Definition LIKE @Keyword
                           ORDER BY 
                               CASE WHEN WordText = @ExactKeyword THEN 0
                                    WHEN WordText LIKE @StartKeyword THEN 1
                                    ELSE 2 END,
                               ViewCount DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@Keyword", "%" + keyword + "%"),
                new SqlParameter("@ExactKeyword", keyword),
                new SqlParameter("@StartKeyword", keyword + "%")
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                words.Add(MapDataRowToWord(row));
            }

            return words;
        }
        // Get Word By Text
        public static Word GetWordByText(string wordText)
        {
            string query = "SELECT * FROM Words WHERE WordText = @WordText";
            SqlParameter[] parameters = {
                new SqlParameter("@WordText", wordText)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToWord(dt.Rows[0]);
            }

            return null;
        }
        // Add New Word
        public static int AddWord(Word word)
        {
            string query = @"INSERT INTO Words 
                             (WordText, Pronunciation, PartOfSpeech, Definition, Example, Synonyms, Antonyms, AudioURL, Category, Level, ViewCount, CreatedDate) 
                             VALUES 
                             (@WordText, @Pronunciation, @PartOfSpeech, @Definition, @Example, @Synonyms, @Antonyms, @AudioURL, @Category, @Level, @ViewCount, @CreatedDate)";
            SqlParameter[] parameters = {
                new SqlParameter("@WordText", word.WordText),
                new SqlParameter("@Pronunciation", word.Pronunciation ?? (object)DBNull.Value),
                new SqlParameter("@PartOfSpeech", word.PartOfSpeech ?? (object)DBNull.Value),
                new SqlParameter("@Definition", word.Definition ?? (object)DBNull.Value),
                new SqlParameter("@Example", word.Example ?? (object)DBNull.Value),
                new SqlParameter("@Synonyms", word.Synonyms ?? (object)DBNull.Value),
                new SqlParameter("@Antonyms", word.Antonyms ?? (object)DBNull.Value),
                new SqlParameter("@AudioURL", word.AudioURL ?? (object)DBNull.Value),
                new SqlParameter("@Category", word.Category ?? (object)DBNull.Value),
                new SqlParameter("@Level", word.Level ?? (object)DBNull.Value),
                new SqlParameter("@ViewCount", word.ViewCount),
                new SqlParameter("@CreatedDate", word.CreatedDate)
            };
            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }
        // Update Word
        public static bool UpdateWord(Word word)
        {
            string query = @"UPDATE Words SET 
                             WordText = @WordText,
                             Pronunciation = @Pronunciation,
                             PartOfSpeech = @PartOfSpeech,
                             Definition = @Definition,
                             Example = @Example,
                             Synonyms = @Synonyms,
                             Antonyms = @Antonyms,
                             AudioURL = @AudioURL,
                             Category = @Category,
                             Level = @Level,
                             ViewCount = @ViewCount
                             WHERE WordID = @WordID";
            SqlParameter[] parameters = {
                new SqlParameter("@WordText", word.WordText),
                new SqlParameter("@Pronunciation", word.Pronunciation ?? (object)DBNull.Value),
                new SqlParameter("@PartOfSpeech", word.PartOfSpeech ?? (object)DBNull.Value),
                new SqlParameter("@Definition", word.Definition ?? (object)DBNull.Value),
                new SqlParameter("@Example", word.Example ?? (object)DBNull.Value),
                new SqlParameter("@Synonyms", word.Synonyms ?? (object)DBNull.Value),
                new SqlParameter("@Antonyms", word.Antonyms ?? (object)DBNull.Value),
                new SqlParameter("@AudioURL", word.AudioURL ?? (object)DBNull.Value),
                new SqlParameter("@Category", word.Category ?? (object)DBNull.Value),
                new SqlParameter("@Level", word.Level ?? (object)DBNull.Value),
                new SqlParameter("@ViewCount", word.ViewCount),
                new SqlParameter("@WordID", word.WordID)
            };
            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
        // Delete Word
        public static bool DeleteWord(int wordId)
        {
            // Xóa các bản ghi liên quan trong các bảng khác
            string deleteHistory = "DELETE FROM SearchHistory WHERE WordID = @WordID";
            string deleteFavorites = "DELETE FROM Favorites WHERE WordID = @WordID";
            string deleteWord = "DELETE FROM Words WHERE WordID = @WordID";

            SqlParameter[] parameters = {
                new SqlParameter("@WordID", wordId)
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(deleteHistory, parameters);
                DatabaseHelper.ExecuteNonQuery(deleteFavorites, parameters);
                int rowsAffected = DatabaseHelper.ExecuteNonQuery(deleteWord, parameters);
                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }
        // Tăng ViewCount
        public static bool IncrementViewCount(int wordId)
        {
            string query = "UPDATE Words SET ViewCount = ViewCount + 1 WHERE WordID = @WordID";

            SqlParameter[] parameters = {
                new SqlParameter("@WordID", wordId)
            };

            int rowsAffected = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }
        // lấy từ vựng theo Cattorry
        public static List<Word> GetWordsByCategory(string category)
        {
            List<Word> words = new List<Word>();
            string query = "SELECT * FROM Words WHERE Category = @Category ORDER BY WordText";
            SqlParameter[] parameters = {
                new SqlParameter("@Category", category)
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            foreach (DataRow row in dt.Rows)
            {
                words.Add(MapDataRowToWord(row));
            }
            return words;
        }
        // Từ vựng  theo level
        public static List<Word> GetWordsByLevel(string level)
        {
            List<Word> words = new List<Word>();

            string query = "SELECT * FROM Words WHERE Level = @Level ORDER BY WordText";
            SqlParameter[] parameters = {
                new SqlParameter("@Level", level)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                words.Add(MapDataRowToWord(row));
            }

            return words;
        }
        // Lấy top từ được tìm kiếm 
        public static List<Word> GetTopViewedWords(int topN)
        {
            List<Word> words = new List<Word>();

            string query = @"SELECT TOP (@TopN) * FROM Words 
                           ORDER BY ViewCount DESC, WordText";
            SqlParameter[] parameters = {
                new SqlParameter("@TopN", topN)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                words.Add(MapDataRowToWord(row));
            }

            return words;
        }
        public static List<Word> GetLatestWords(int count)
        {
            List<Word> words = new List<Word>();

            string query = @"SELECT TOP (@Count) * FROM Words 
                           ORDER BY CreatedDate DESC";
            SqlParameter[] parameters = {
                new SqlParameter("@Count", count)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                words.Add(MapDataRowToWord(row));
            }

            return words;
        }
        public static int GetTotalWords()
        {
            string query = "SELECT COUNT(*) FROM Words";
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query));
        }

        // Lấy danh sách từ với phân trang
        public static List<Word> GetWordsPaged(int pageIndex, int pageSize)
        {
            List<Word> words = new List<Word>();

            string query = @"SELECT * FROM Words 
                           ORDER BY WordText
                           OFFSET @Offset ROWS 
                           FETCH NEXT @PageSize ROWS ONLY";

            SqlParameter[] parameters = {
                new SqlParameter("@Offset", pageIndex * pageSize),
                new SqlParameter("@PageSize", pageSize)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                words.Add(MapDataRowToWord(row));
            }

            return words;
        }
        // Kiểm tra từ xem tồn tại hay không
        public static bool IsWordExists(string wordText)
        {
            string query = "SELECT COUNT(*) FROM Words WHERE WordText = @WordText";

            SqlParameter[] parameters = {
                new SqlParameter("@WordText", wordText)
            };

            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }
        private static Word MapDataRowToWord(DataRow row)
        {
            return new Word
            {
                WordID = Convert.ToInt32(row["WordID"]),
                WordText = row["WordText"].ToString(),
                Pronunciation = row["Pronunciation"] != DBNull.Value ? row["Pronunciation"].ToString() : null,
                PartOfSpeech = row["PartOfSpeech"] != DBNull.Value ? row["PartOfSpeech"].ToString() : null,
                Definition = row["Definition"].ToString(),
                Example = row["Example"] != DBNull.Value ? row["Example"].ToString() : null,
                Synonyms = row["Synonyms"] != DBNull.Value ? row["Synonyms"].ToString() : null,
                Antonyms = row["Antonyms"] != DBNull.Value ? row["Antonyms"].ToString() : null,
                AudioURL = row["AudioURL"] != DBNull.Value ? row["AudioURL"].ToString() : null,
                Category = row["Category"] != DBNull.Value ? row["Category"].ToString() : null,
                Level = row["Level"] != DBNull.Value ? row["Level"].ToString() : null,
                ViewCount = Convert.ToInt32(row["ViewCount"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }
    }
}