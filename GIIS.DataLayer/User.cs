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
public partial class User
{

#region Properties
public Int32 Id { get; set; }
public string Username { get; set; }
public string Password { get; set; }
public string Firstname { get; set; }
public string Lastname { get; set; }
public bool IsActive { get; set; }
public bool Deleted { get; set; }
public string Notes { get; set; }
public string Email { get; set; }
public Int32 Failedlogins { get; set; }
public bool Isloggedin { get; set; }
public DateTime Lastlogin { get; set; }
public DateTime PrevLogin { get; set; }
public string Lastip { get; set; }
public Int32 HealthFacilityId { get; set; }
public HealthFacility HealthFacility
{
get
{
if (this.HealthFacilityId > 0)
return HealthFacility.GetHealthFacilityById(this.HealthFacilityId);
else
return null;
}
}
#endregion

#region GetData
public static List<User> GetUserList()
{
try
{
string query = @"SELECT * FROM ""USER"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetUserAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("User", "GetUserList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetUserForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""USER"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("User", "GetUserForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static User GetUserById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""USER"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("User", i.ToString(), 4, DateTime.Now, i);
return GetUserAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("User", "GetUserById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static User GetUserByUsername(string s)
{
try
{
string query = @"SELECT * FROM ""USER"" WHERE ""USERNAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("User", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetUserAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("User", "GetUserByUsername", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(User o)
{
try
{
string query = @"INSERT INTO ""USER"" (""USERNAME"", ""PASSWORD"", ""FIRSTNAME"", ""LASTNAME"", ""IS_ACTIVE"", ""DELETED"", ""NOTES"", ""EMAIL"", ""FAILEDLOGINS"", ""ISLOGGEDIN"", ""LASTLOGIN"", ""LASTIP"", ""HEALTH_FACILITY_ID"") VALUES (@Username, @Password, @Firstname, @Lastname, @IsActive, @Deleted, @Notes, @Email, @Failedlogins, @Isloggedin, @Lastlogin, @Lastip, @HealthFacilityId) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Username", DbType.String)  { Value = o.Username },
new NpgsqlParameter("@Password", DbType.String)  { Value = o.Password },
new NpgsqlParameter("@Firstname", DbType.String)  { Value = (object)o.Firstname ?? DBNull.Value },
new NpgsqlParameter("@Lastname", DbType.String)  { Value = (object)o.Lastname ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = (object)o.IsActive ?? DBNull.Value },
new NpgsqlParameter("@Deleted", DbType.Boolean)  { Value = (object)o.Deleted ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Email", DbType.String)  { Value = (object)o.Email ?? DBNull.Value },
new NpgsqlParameter("@Failedlogins", DbType.Int32)  { Value = (object)o.Failedlogins ?? DBNull.Value },
new NpgsqlParameter("@Isloggedin", DbType.Boolean)  { Value = (object)o.Isloggedin ?? DBNull.Value },
new NpgsqlParameter("@Lastlogin", DbType.DateTime)  { Value = (object)o.Lastlogin ?? DBNull.Value },
new NpgsqlParameter("@Lastip", DbType.String)  { Value = (object)o.Lastip ?? DBNull.Value },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("User", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("User", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(User o)
{
try
{
string query = @"UPDATE ""USER"" SET ""USERNAME"" = @Username, ""PASSWORD"" = @Password, ""FIRSTNAME"" = @Firstname, ""LASTNAME"" = @Lastname, ""IS_ACTIVE"" = @IsActive, ""DELETED"" = @Deleted, ""NOTES"" = @Notes, ""EMAIL"" = @Email, ""FAILEDLOGINS"" = @Failedlogins, ""ISLOGGEDIN"" = @Isloggedin, ""LASTLOGIN"" = @Lastlogin, ""LASTIP"" = @Lastip, ""HEALTH_FACILITY_ID"" = @HealthFacilityId, ""PREVLOGIN"" = @PrevLogin WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Username", DbType.String)  { Value = o.Username },
new NpgsqlParameter("@Password", DbType.String)  { Value = o.Password },
new NpgsqlParameter("@Firstname", DbType.String)  { Value = (object)o.Firstname ?? DBNull.Value },
new NpgsqlParameter("@Lastname", DbType.String)  { Value = (object)o.Lastname ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = (object)o.IsActive ?? DBNull.Value },
new NpgsqlParameter("@Deleted", DbType.Boolean)  { Value = (object)o.Deleted ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Email", DbType.String)  { Value = (object)o.Email ?? DBNull.Value },
new NpgsqlParameter("@Failedlogins", DbType.Int32)  { Value = (object)o.Failedlogins ?? DBNull.Value },
new NpgsqlParameter("@Isloggedin", DbType.Boolean)  { Value = (object)o.Isloggedin ?? DBNull.Value },
new NpgsqlParameter("@Lastlogin", DbType.DateTime)  { Value = (object)o.Lastlogin ?? DBNull.Value },
new NpgsqlParameter("@Lastip", DbType.String)  { Value = (object)o.Lastip ?? DBNull.Value },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id },
new NpgsqlParameter("@PrevLogin", DbType.DateTime)  { Value = (object)o.PrevLogin ?? DBNull.Value }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("User", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("User", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""USER"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("User", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("User", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""USER"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("User", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("User", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static User GetUserAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
User o = new User();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Username = row["USERNAME"].ToString();
o.Password = row["PASSWORD"].ToString();
o.Firstname = row["FIRSTNAME"].ToString();
o.Lastname = row["LASTNAME"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.Deleted = Helper.ConvertToBoolean(row["DELETED"]);
o.Notes = row["NOTES"].ToString();
o.Email = row["EMAIL"].ToString();
o.Failedlogins = Helper.ConvertToInt(row["FAILEDLOGINS"]);
o.Isloggedin = Helper.ConvertToBoolean(row["ISLOGGEDIN"]);
o.Lastlogin = Helper.ConvertToDate(row["LASTLOGIN"]);
o.PrevLogin = Helper.ConvertToDate(row["PREVLOGIN"]);
o.Lastip = row["LASTIP"].ToString();
o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("User", "GetUserAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<User> GetUserAsList(DataTable dt)
{
List<User> oList = new List<User>();
foreach (DataRow row in dt.Rows)
{
try
{
User o = new User();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Username = row["USERNAME"].ToString();
o.Password = row["PASSWORD"].ToString();
o.Firstname = row["FIRSTNAME"].ToString();
o.Lastname = row["LASTNAME"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.Deleted = Helper.ConvertToBoolean(row["DELETED"]);
o.Notes = row["NOTES"].ToString();
o.Email = row["EMAIL"].ToString();
o.Failedlogins = Helper.ConvertToInt(row["FAILEDLOGINS"]);
o.Isloggedin = Helper.ConvertToBoolean(row["ISLOGGEDIN"]);
o.Lastlogin = Helper.ConvertToDate(row["LASTLOGIN"]);
o.PrevLogin = Helper.ConvertToDate(row["PREVLOGIN"]);
o.Lastip = row["LASTIP"].ToString();
o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("User", "GetUserAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
