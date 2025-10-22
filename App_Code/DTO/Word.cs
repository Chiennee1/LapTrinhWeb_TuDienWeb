using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LapTrinhWeb_TuDienWeb.App_Code.DTO
{
    public class Word
    {
        public int WordID { get; set; }
        public string WordText { get; set; }
        public string Pronunciation { get; set; } // Phát âm
        public string PartOfSpeech { get; set; } // Meaning of word
        public string Definition { get; set; } 
        public string Example { get; set; } // Vidu

        public string Synonyms { get; set; } // Đồng nghĩa
        public string Antonyms { get; set; } // Từ Trái nghĩa
        public string AudioURL { get; set; }
        public string Category { get; set; } // loại
        public string Level { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }

        // Contructor
        public Word()
        {
            ViewCount = 0;
            CreatedDate = DateTime.Now;
        }
        public Word(string wordText, string definition, string partOfSpeech)
        {
            WordText = wordText;
            Definition = definition;
            PartOfSpeech = partOfSpeech;
            ViewCount = 0;
            CreatedDate = DateTime.Now;
        }

        // Lấy danh sách nghĩa
        public List<string> GetDefinitions()
        {
            if (string.IsNullOrEmpty(Definition))
                return new List<string>();

            return Definition.Split('|').Select(d => d.Trim()).ToList();
        }
        // Lấy ra list vídu
        public List<string> GetExamples()
        {
            if (string.IsNullOrEmpty(Example))
                return new List<string>();

            return Example.Split('|').Select(e => e.Trim()).ToList();
        }
        // Lấy ra list các từ đồng nghĩa
        public List<string> GetSynonyms()
        {
            if (string.IsNullOrEmpty(Synonyms))
                return new List<string>();

            return Synonyms.Split(',').Select(s => s.Trim()).ToList();
        }

        
        // Lấy danh sách từ trái nghĩa
        public List<string> GetAntonyms()
        {
            if (string.IsNullOrEmpty(Antonyms))
                return new List<string>();

            return Antonyms.Split(',').Select(a => a.Trim()).ToList();
        }

        //Lấy nghĩa đầu tiên (ngắn gọn để hiển thị)
        public string GetShortDefinition()
        {
            var definitions = GetDefinitions();
            if (definitions.Count > 0)
            {
                string firstDef = definitions[0];
                return firstDef.Length > 100 ? firstDef.Substring(0, 100) + "..." : firstDef;
            }
            return "";
        }
    }
}