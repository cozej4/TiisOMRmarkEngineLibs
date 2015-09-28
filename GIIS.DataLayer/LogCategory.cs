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
public partial class LogCategory
{

#region Properties
public Int32 Id { get; set; }
public string Name { get; set; }
public string Notes { get; set; }
#endregion

#region GetData
public static List<LogCategory> GetLogCategoryList()
{
try
{
string query = @"SELECT * FROM ""LOG_CATEGORY"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetLogCategoryAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "GetLogCategoryList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetLogCategoryForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""LOG_CATEGORY"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "GetLogCategoryForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static LogCategory GetLogCategoryById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""LOG_CATEGORY"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("LogCategory", i.ToString(), 4, DateTime.Now, 1);
return GetLogCategoryAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "GetLogCategoryById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static LogCategory GetLogCategoryByName(string s)
{
try
{
string query = @"SELECT * FROM ""LOG_CATEGORY"" WHERE ""NAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("LogCategory", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetLogCategoryAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "GetLogCategoryByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(LogCategory o)
{
try
{
string query = @"INSERT INTO ""LOG_CATEGORY"" (""NAME"", ""NOTES"") VALUES (@Name, @Notes) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("LogCategory", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(LogCategory o)
{
try
{
string query = @"UPDATE ""LOG_CATEGORY"" SET ""ID"" = @Id, ""NAME"" = @Name, ""NOTES"" = @Notes WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("LogCategory", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""LOG_CATEGORY"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("LogCategory", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static LogCategory GetLogCategoryAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
LogCategory o = new LogCategory();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.Notes = row["NOTES"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "GetLogCategoryAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<LogCategory> GetLogCategoryAsList(DataTable dt)
{
List<LogCategory> oList = new List<LogCategory>();
foreach (DataRow row in dt.Rows)
{
try
{
LogCategory o = new LogCategory();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.Notes = row["NOTES"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("LogCategory", "GetLogCategoryAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
