using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
   partial class Community
    {

        public static List<Community> GetPagedCommunityList(string name, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT * FROM ""COMMUNITY""  "
                                + @" WHERE ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                + @" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

                return GetCommunityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Community", "GetPagedCommunityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountCommunityList(string name)
        {
            try
            {
                string query = @"SELECT count(*) FROM ""COMMUNITY""  "
                                + @" WHERE ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '');";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);

                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Community", "GetCountCommunityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
