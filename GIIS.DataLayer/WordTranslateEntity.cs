using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    public partial class WordTranslate
    {
        public static List<WordTranslate> GetWordTranslateList(string where)
        {
            try
            {
                string query = @"SELECT * FROM ""WORD_TRANSLATE"" WHERE " + where;
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWordTranslateAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetWordTranslateList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static string GetWordByLanguageAndName(string language, string name)
        {
            try
            {
                int languageId = Language.GetLanguageByName(language).Id;

                string query = string.Format(@"SELECT * FROM ""WORD_TRANSLATE"" WHERE ""LANGUAGE_ID"" = {0} AND ""NAME"" = '{1}';", languageId, name);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWordTranslateAsObject(dt).Code;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetWordByLanguageAndName", 1, ex.StackTrace, ex.Message);
                return "";
                //throw ex;
            }
        }
        public static string GetWordByLanguageAndName(int languageId, string name)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""WORD_TRANSLATE"" WHERE ""LANGUAGE_ID"" = {0} AND ""NAME"" = '{1}';", languageId, name);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWordTranslateAsObject(dt).Code;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetWordByLanguageAndName", 1, ex.StackTrace, ex.Message);
                return "";
                //throw ex;
            }
        }

        public static string GetWordByLanguageAndCode(int languageId, string code)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""WORD_TRANSLATE"" WHERE ""LANGUAGE_ID"" = {0} AND ""CODE"" = '{1}';", languageId, code);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetWordTranslateAsObject(dt).Name;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetWordByLanguageAndName", 1, ex.StackTrace, ex.Message);
                return "";
                //throw ex;
            }
        }

        public static List<WordTranslate> GetWordByLanguage(int languageId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""WORD_TRANSLATE"" WHERE ""LANGUAGE_ID"" = {0};", languageId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                List<WordTranslate> wordTranslateList = GetWordTranslateAsList(dt);
                return wordTranslateList;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetWordByLanguage", 1, ex.StackTrace, ex.Message);
                return null;
                //throw ex;
            }
        }
        public static List<WordTranslate> GetWordByLanguage(int languageId, int pageId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""WORD_TRANSLATE"" WHERE ""LANGUAGE_ID"" = {0} AND ""PAGE_ID"" = {1};", languageId, pageId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                List<WordTranslate> wordTranslateList = GetWordTranslateAsList(dt);
                return wordTranslateList;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetWordByLanguage", 1, ex.StackTrace, ex.Message);
                return null;
                //throw ex;
            }
        }
        public static List<WordTranslate> GetWordByLanguage(int languageId, string page)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""WORD_TRANSLATE"" WHERE ""LANGUAGE_ID"" = {0} AND ""PAGE_NAME"" = '{1}' ORDER BY ""CODE"";", languageId, page);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                List<WordTranslate> wordTranslateList = GetWordTranslateAsList(dt);
                return wordTranslateList;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetWordByLanguage", 1, ex.StackTrace, ex.Message);
                return null;
                //throw ex;
            }
        }

        public static List<Page> GetAllPages()
        {
            try
            {
                string query = string.Format(@"SELECT DISTINCT ""PAGE_NAME"" FROM ""WORD_TRANSLATE"" ORDER BY ""PAGE_NAME"";");
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                List<Page> pageList = new List<Page>();
                foreach (DataRow row in dt.Rows)
                {
                    Page p = new Page();
                    p.PageName = row["PAGE_NAME"].ToString();
                    pageList.Add(p);
                }

                return pageList;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("WordTranslate", "GetAllPages", 1, ex.StackTrace, ex.Message);
                return null;
                //throw ex;
            }
        }
    }

    public class Page
    {
        public string PageName {get; set;}
    }
}