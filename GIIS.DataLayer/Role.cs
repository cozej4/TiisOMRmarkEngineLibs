using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class Role
{

#region Properties
public Int32 Id { get; set; }
public string Name { get; set; }
public bool IsActive { get; set; }
public DateTime ModifiedOn { get; set; }
public Int32 ModifiedBy { get; set; }
public string Notes { get; set; }
#endregion

#region GetData
public static List<Role> GetRoleList()
{
try
{
string query = @"SELECT * FROM ""ROLE"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetRoleAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Role", "GetRoleList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetRoleForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ROLE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("Role", "GetRoleForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static Role GetRoleById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""ROLE"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Role", i.ToString(), 4, DateTime.Now, 1);
return GetRoleAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Role", "GetRoleById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static Role GetRoleByName(string s)
{
try
{
string query = @"SELECT * FROM ""ROLE"" WHERE ""NAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Role", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetRoleAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Role", "GetRoleByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(Role o)
{
try
{
string query = @"INSERT INTO ""ROLE"" (""NAME"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""NOTES"") VALUES (@Name, @IsActive, @ModifiedOn, @ModifiedBy, @Notes) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Role", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("Role", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(Role o)
{
try
{
string query = @"UPDATE ""ROLE"" SET ""ID"" = @Id, ""NAME"" = @Name, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""NOTES"" = @Notes WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Role", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Role", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""ROLE"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Role", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Role", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""ROLE"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Role", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Role", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static Role GetRoleAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
Role o = new Role();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
o.Notes = row["NOTES"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("Role", "GetRoleAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<Role> GetRoleAsList(DataTable dt)
{
List<Role> oList = new List<Role>();
foreach (DataRow row in dt.Rows)
{
try
{
Role o = new Role();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
o.Notes = row["NOTES"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("Role", "GetRoleAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
