using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public partial class VWordTranslate
    {
        public static List<VWordTranslate> GetVWordTranslateList(int languageId, string page, Int32 userId, string machineName)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""V_WORD_TRANSLATE"" where ""Page_Name"" = '{0}' AND ""Language_Id"" = {1};", page, languageId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVWordTranslateAsList(dt, userId, machineName);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VWordTranslate", "GetVWordTranslateList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
