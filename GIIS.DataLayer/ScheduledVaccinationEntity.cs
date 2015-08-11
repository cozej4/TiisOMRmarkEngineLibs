using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
   partial  class ScheduledVaccination
   {
       public static DataTable GetScheduledVaccinationForList()
       {
           try
           {
               string query = @"SELECT -1 as ""ID"", '-----' as ""CODE"" UNION SELECT ""ID"", ""CODE"" FROM ""SCHEDULED_VACCINATION"" ORDER BY ""CODE"" ";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return dt;
           }
           catch (Exception ex)
           {
               Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

       public static List<ScheduledVaccination> GetPagedScheduledVaccinationList(string name, string code, ref int maximumRows, ref int startRowIndex)
       {
           try
           {
               string query = @"SELECT * FROM ""SCHEDULED_VACCINATION"" WHERE 1 = 1 "
                                  + @" AND ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                  + @" AND ( UPPER(""CODE"") like @Code OR @Code is null or @Code = '')"
                                  + @" ORDER BY ""CODE"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

               List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@Code", DbType.String) { Value = ("%" + code + "%") },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
               return GetScheduledVaccinationAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("ScheduledVaccination", "GetPagedScheduledVaccinationList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

       public static int GetCountScheduledVaccinationList(string name, string code)
       {
           try
           {
               string query = @"SELECT count(*) FROM ""SCHEDULED_VACCINATION"" WHERE 1 = 1 "
                                  + @" AND ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                  + @" AND ( UPPER(""CODE"") like @Code OR @Code is null or @Code = '');";

               List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@Code", DbType.String) { Value = ("%" + code + "%") }
                };

               object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
               return int.Parse(count.ToString());
           }
           catch (Exception ex)
           {
               Log.InsertEntity("ScheduledVaccination", "GetCountScheduledVaccinationList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
   }
}
