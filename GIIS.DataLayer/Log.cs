//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class Log
{

#region Properties
public Int32 Id { get; set; }
public Int32 LogCategoryId { get; set; }
public DateTime Created { get; set; }
public string Title { get; set; }
public string Message { get; set; }
public string Class { get; set; }
public string Method { get; set; }
public LogCategory LogCategory
{
get
{
if (this.LogCategoryId > 0)
return LogCategory.GetLogCategoryById(this.LogCategoryId);
else
return null;
}
}
#endregion

#region GetData
public static List<Log> GetLogList()
{
try
{
string query = @"SELECT * FROM ""LOG"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetLogAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Log", "GetLogList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetLogForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""LOG"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("Log", "GetLogForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static Log GetLogById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""LOG"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Log", i.ToString(), 4, DateTime.Now, 1);
return GetLogAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Log", "GetLogById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(Log o)
{
try
{
string query = @"INSERT INTO ""LOG"" (""LOG_CATEGORY_ID"", ""CREATED"", ""TITLE"", ""MESSAGE"", ""CLASS"", ""METHOD"") VALUES (@LogCategoryId, @Created, @Title, @Message, @Class, @Method) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@LogCategoryId", DbType.Int32)  { Value = o.LogCategoryId },
new NpgsqlParameter("@Created", DbType.Date)  { Value = (object)o.Created ?? DBNull.Value },
new NpgsqlParameter("@Title", DbType.String)  { Value = (object)o.Title ?? DBNull.Value },
new NpgsqlParameter("@Message", DbType.String)  { Value = (object)o.Message ?? DBNull.Value },
new NpgsqlParameter("@Class", DbType.String)  { Value = (object)o.Class ?? DBNull.Value },
new NpgsqlParameter("@Method", DbType.String)  { Value = (object)o.Method ?? DBNull.Value }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
//AuditTable.InsertEntity("Log", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
    throw ex;
//Log.InsertEntity("Log", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
//return -1;
}

public static int Update(Log o)
{
try
{
string query = @"UPDATE ""LOG"" SET ""ID"" = @Id, ""LOG_CATEGORY_ID"" = @LogCategoryId, ""CREATED"" = @Created, ""TITLE"" = @Title, ""MESSAGE"" = @Message, ""CLASS"" = @Class, ""METHOD"" = @Method WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@LogCategoryId", DbType.Int32)  { Value = o.LogCategoryId },
new NpgsqlParameter("@Created", DbType.Date)  { Value = (object)o.Created ?? DBNull.Value },
new NpgsqlParameter("@Title", DbType.String)  { Value = (object)o.Title ?? DBNull.Value },
new NpgsqlParameter("@Message", DbType.String)  { Value = (object)o.Message ?? DBNull.Value },
new NpgsqlParameter("@Class", DbType.String)  { Value = (object)o.Class ?? DBNull.Value },
new NpgsqlParameter("@Method", DbType.String)  { Value = (object)o.Method ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Log", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Log", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""LOG"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Log", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Log", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static Log GetLogAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
Log o = new Log();
o.Id = Helper.ConvertToInt(row["ID"]);
o.LogCategoryId = Helper.ConvertToInt(row["LOG_CATEGORY_ID"]);
o.Created = Helper.ConvertToDate(row["CREATED"]);
o.Title = row["TITLE"].ToString();
o.Message = row["MESSAGE"].ToString();
o.Class = row["CLASS"].ToString();
o.Method = row["METHOD"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("Log", "GetLogAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<Log> GetLogAsList(DataTable dt)
{
List<Log> oList = new List<Log>();
foreach (DataRow row in dt.Rows)
{
try
{
Log o = new Log();
o.Id = Helper.ConvertToInt(row["ID"]);
o.LogCategoryId = Helper.ConvertToInt(row["LOG_CATEGORY_ID"]);
o.Created = Helper.ConvertToDate(row["CREATED"]);
o.Title = row["TITLE"].ToString();
o.Message = row["MESSAGE"].ToString();
o.Class = row["CLASS"].ToString();
o.Method = row["METHOD"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("Log", "GetLogAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
