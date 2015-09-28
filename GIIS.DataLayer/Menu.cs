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
public partial class Menu
{

#region Properties
public Int32 Id { get; set; }
public Int32 ParentId { get; set; }
public string Title { get; set; }
public string NavigateUrl { get; set; }
public bool IsActive { get; set; }
public Int32 DisplayOrder { get; set; }
#endregion

#region GetData
public static List<Menu> GetMenuList()
{
try
{
string query = @"SELECT * FROM ""MENU"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetMenuAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "GetMenuList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetMenuForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""MENU"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "GetMenuForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static Menu GetMenuById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""MENU"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Menu", i.ToString(), 4, DateTime.Now, 1);
return GetMenuAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "GetMenuById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(Menu o)
{
try
{
string query = @"INSERT INTO ""MENU"" (""PARENT_ID"", ""TITLE"", ""NAVIGATE_URL"", ""IS_ACTIVE"", ""DISPLAY_ORDER"") VALUES (@ParentId, @Title, @NavigateUrl, @IsActive, @DisplayOrder) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParentId", DbType.Int32)  { Value = (object)o.ParentId ?? DBNull.Value },
new NpgsqlParameter("@Title", DbType.String)  { Value = o.Title },
new NpgsqlParameter("@NavigateUrl", DbType.String)  { Value = (object)o.NavigateUrl ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = (object)o.IsActive ?? DBNull.Value },
new NpgsqlParameter("@DisplayOrder", DbType.Int32)  { Value = (object)o.DisplayOrder ?? DBNull.Value }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Menu", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(Menu o)
{
try
{
string query = @"UPDATE ""MENU"" SET ""ID"" = @Id, ""PARENT_ID"" = @ParentId, ""TITLE"" = @Title, ""NAVIGATE_URL"" = @NavigateUrl, ""IS_ACTIVE"" = @IsActive, ""DISPLAY_ORDER"" = @DisplayOrder WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParentId", DbType.Int32)  { Value = (object)o.ParentId ?? DBNull.Value },
new NpgsqlParameter("@Title", DbType.String)  { Value = o.Title },
new NpgsqlParameter("@NavigateUrl", DbType.String)  { Value = (object)o.NavigateUrl ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = (object)o.IsActive ?? DBNull.Value },
new NpgsqlParameter("@DisplayOrder", DbType.Int32)  { Value = (object)o.DisplayOrder ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Menu", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""MENU"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Menu", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""MENU"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Menu", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static Menu GetMenuAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
Menu o = new Menu();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ParentId = Helper.ConvertToInt(row["PARENT_ID"]);
o.Title = row["TITLE"].ToString();
o.NavigateUrl = row["NAVIGATE_URL"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.DisplayOrder = Helper.ConvertToInt(row["DISPLAY_ORDER"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "GetMenuAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<Menu> GetMenuAsList(DataTable dt)
{
List<Menu> oList = new List<Menu>();
foreach (DataRow row in dt.Rows)
{
try
{
Menu o = new Menu();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ParentId = Helper.ConvertToInt(row["PARENT_ID"]);
o.Title = row["TITLE"].ToString();
o.NavigateUrl = row["NAVIGATE_URL"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.DisplayOrder = Helper.ConvertToInt(row["DISPLAY_ORDER"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("Menu", "GetMenuAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
