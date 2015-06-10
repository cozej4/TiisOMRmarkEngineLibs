using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Birthplace
    {
        public static Birthplace GetBirthplaceByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""BIRTHPLACE"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetBirthplaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "GetBirthplaceByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetBirthplaceListNew()
        {
            try
            {
                string query = string.Format(@"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""BIRTHPLACE"" WHERE ""IS_ACTIVE"" = 'true' ORDER BY ""NAME"";");
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt; 
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "GetBirthplaceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
