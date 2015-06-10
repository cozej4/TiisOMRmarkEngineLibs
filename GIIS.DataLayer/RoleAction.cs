using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class RoleAction
{

#region Properties
public Int32 Id { get; set; }
public Int32 RoleId { get; set; }
public Int32 ActionId { get; set; }
public bool IsActive { get; set; }
public Actions Action
{
get
{
if (this.ActionId > 0)
return Actions.GetActionsById(this.ActionId);
else
return null;
}
}
public Role Role
{
get
{
if (this.RoleId > 0)
return Role.GetRoleById(this.RoleId);
else
return null;
}
}
#endregion

#region GetData
public static List<RoleAction> GetRoleActionList()
{
try
{
string query = @"SELECT * FROM ""ROLE_ACTION"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetRoleActionAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "GetRoleActionList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetRoleActionForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ROLE_ACTION"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "GetRoleActionForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}


public static RoleAction GetRoleActionById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""ROLE_ACTION"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("RoleAction", i.ToString(), 4, DateTime.Now, 1);
return GetRoleActionAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "GetRoleActionById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(RoleAction o)
{
try
{
string query = @"INSERT INTO ""ROLE_ACTION"" (""ROLE_ID"", ""ACTION_ID"", ""IS_ACTIVE"") VALUES (@RoleId, @ActionId, @IsActive) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@RoleId", DbType.Int32)  { Value = o.RoleId },
new NpgsqlParameter("@ActionId", DbType.Int32)  { Value = o.ActionId },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("RoleAction", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(RoleAction o)
{
try
{
string query = @"UPDATE ""ROLE_ACTION"" SET ""ID"" = @Id, ""ROLE_ID"" = @RoleId, ""ACTION_ID"" = @ActionId, ""IS_ACTIVE"" = @IsActive WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@RoleId", DbType.Int32)  { Value = o.RoleId },
new NpgsqlParameter("@ActionId", DbType.Int32)  { Value = o.ActionId },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("RoleAction", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""ROLE_ACTION"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("RoleAction", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""ROLE_ACTION"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("RoleAction", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static RoleAction GetRoleActionAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
RoleAction o = new RoleAction();
o.Id = Helper.ConvertToInt(row["ID"]);
o.RoleId = Helper.ConvertToInt(row["ROLE_ID"]);
o.ActionId = Helper.ConvertToInt(row["ACTION_ID"]);
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "GetRoleActionAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<RoleAction> GetRoleActionAsList(DataTable dt)
{
List<RoleAction> oList = new List<RoleAction>();
foreach (DataRow row in dt.Rows)
{
try
{
RoleAction o = new RoleAction();
o.Id = Helper.ConvertToInt(row["ID"]);
o.RoleId = Helper.ConvertToInt(row["ROLE_ID"]);
o.ActionId = Helper.ConvertToInt(row["ACTION_ID"]);
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("RoleAction", "GetRoleActionAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
