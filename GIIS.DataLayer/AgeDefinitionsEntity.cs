using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    partial class AgeDefinitions
    {
        public static List<AgeDefinitions> GetPagedAgeDefinitionsList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""AGE_DEFINITIONS"" WHERE " + where + @" ORDER BY ""DAYS"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetAgeDefinitionsAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "GetPagedAgeDefinitionsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountAgeDefinitionsList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""AGE_DEFINITIONS"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "GetCountAgeDefinitionsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
