using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    partial class Help
    {
        public static int CheckExists(string pageName, int languageId)
        {
            try
            {
                string query = string.Format("Select COUNT(*) FROM \"HELP\" WHERE UPPER(\"PAGE\") like '{0}%' AND \"LANGUAGE_ID\"={1} ", pageName.ToUpper(), languageId);
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Help", "CheckExists", 7, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
           
        }

        public static Help GetHelpByPageAndLanguage(string pageName, int languageId)
        {
            try
            {
                string query = string.Format("SELECT * " + "FROM \"HELP\" " + "WHERE UPPER(\"PAGE\") like '%{0}%' AND \"LANGUAGE_ID\" = {1} " + "ORDER BY \"PAGE\"; ", pageName.ToUpper(), languageId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHelpAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Help", "GetHelpByPageAndLanguage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Help> GetHelpByLanguage(int languageId)
        {
            try
            {
                string query = string.Format("SELECT * " + "FROM \"HELP\" " + "WHERE \"LANGUAGE_ID\" = {0} " + "ORDER BY \"PAGE\"; ", languageId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHelpAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Help", "GetHelpByLanguage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
