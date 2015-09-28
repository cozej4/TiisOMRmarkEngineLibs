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
public partial class Actions
{

#region Properties
public Int32 Id { get; set; }
public string Name { get; set; }
public string Notes { get; set; }
#endregion

#region GetData
public static List<Actions> GetActionsList()
{
try
{
string query = @"SELECT * FROM ""ACTIONS"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetActionsAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "GetActionsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetActionsForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ACTIONS"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "GetActionsForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static Actions GetActionsById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""ACTIONS"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Actions", i.ToString(), 4, DateTime.Now, 1);
return GetActionsAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "GetActionsById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static Actions GetActionsByName(string s)
{
try
{
string query = @"SELECT * FROM ""ACTIONS"" WHERE ""NAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Actions", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetActionsAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "GetActionsByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(Actions o)
{
try
{
string query = @"INSERT INTO ""ACTIONS"" (""NAME"", ""NOTES"") VALUES (@Name, @Notes) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Actions", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(Actions o)
{
try
{
string query = @"UPDATE ""ACTIONS"" SET ""ID"" = @Id, ""NAME"" = @Name, ""NOTES"" = @Notes WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Actions", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""ACTIONS"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Actions", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static Actions GetActionsAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
Actions o = new Actions();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.Notes = row["NOTES"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "GetActionsAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<Actions> GetActionsAsList(DataTable dt)
{
List<Actions> oList = new List<Actions>();
foreach (DataRow row in dt.Rows)
{
try
{
Actions o = new Actions();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.Notes = row["NOTES"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("Actions", "GetActionsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
