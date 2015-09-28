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
public partial class UserRole
{

#region Properties
public Int32 Id { get; set; }
public Int32 UserId { get; set; }
public Int32 RoleId { get; set; }
public bool IsActive { get; set; }
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
public User User
{
get
{
if (this.UserId > 0)
return User.GetUserById(this.UserId);
else
return null;
}
}
#endregion

#region GetData
public static List<UserRole> GetUserRoleList()
{
try
{
string query = @"SELECT * FROM ""USER_ROLE"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetUserRoleAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "GetUserRoleList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetUserRoleForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""USER_ROLE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "GetUserRoleForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}


public static UserRole GetUserRoleById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""USER_ROLE"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("UserRole", i.ToString(), 4, DateTime.Now, 1);
return GetUserRoleAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "GetUserRoleById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(UserRole o)
{
try
{
string query = @"INSERT INTO ""USER_ROLE"" (""USER_ID"", ""ROLE_ID"", ""IS_ACTIVE"") VALUES (@UserId, @RoleId, @IsActive) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@UserId", DbType.Int32)  { Value = o.UserId },
new NpgsqlParameter("@RoleId", DbType.Int32)  { Value = o.RoleId },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("UserRole", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(UserRole o)
{
try
{
string query = @"UPDATE ""USER_ROLE"" SET ""ID"" = @Id, ""USER_ID"" = @UserId, ""ROLE_ID"" = @RoleId, ""IS_ACTIVE"" = @IsActive WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@UserId", DbType.Int32)  { Value = o.UserId },
new NpgsqlParameter("@RoleId", DbType.Int32)  { Value = o.RoleId },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("UserRole", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""USER_ROLE"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("UserRole", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""USER_ROLE"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("UserRole", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static UserRole GetUserRoleAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
UserRole o = new UserRole();
o.Id = Helper.ConvertToInt(row["ID"]);
o.UserId = Helper.ConvertToInt(row["USER_ID"]);
o.RoleId = Helper.ConvertToInt(row["ROLE_ID"]);
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "GetUserRoleAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<UserRole> GetUserRoleAsList(DataTable dt)
{
List<UserRole> oList = new List<UserRole>();
foreach (DataRow row in dt.Rows)
{
try
{
UserRole o = new UserRole();
o.Id = Helper.ConvertToInt(row["ID"]);
o.UserId = Helper.ConvertToInt(row["USER_ID"]);
o.RoleId = Helper.ConvertToInt(row["ROLE_ID"]);
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("UserRole", "GetUserRoleAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
