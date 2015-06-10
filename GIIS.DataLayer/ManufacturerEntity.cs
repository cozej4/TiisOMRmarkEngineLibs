using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Npgsql;

namespace GIIS.DataLayer
{
   partial class Manufacturer
    {
       public static DataTable GetManufacturerListForItemLot()
       {
           try
           {
               string query = @"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""MANUFACTURER"" ORDER BY ""NAME"";";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return dt;
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Manufacturer", "GetManufacturerListForItemLot", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

       public static List<Manufacturer> GetPagedManufacturerList(string name, string code, ref int maximumRows, ref int startRowIndex)
       {
           try
           {
               string query = @"SELECT * FROM ""MANUFACTURER"" WHERE 1 = 1 "
                                  + @" AND ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                  + @" AND ( UPPER(""CODE"") like @Code OR @Code is null or @Code = '')"
                                  + @" OFFSET @StartRowIndex LIMIT @MaximumRows;";

               List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@Code", DbType.String) { Value = ("%" + code + "%") },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
               return GetManufacturerAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Manufacturer", "GetPagedManufacturerList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

       public static int GetCountManufacturerList(string name, string code)
       {
           try
           {
               string query = @"SELECT COUNT(*) FROM ""MANUFACTURER"" WHERE 1 = 1 "
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
               Log.InsertEntity("Manufacturer", "GetCountManufacturerList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
    }
}
