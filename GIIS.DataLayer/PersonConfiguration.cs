using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class PersonConfiguration
{

#region Properties
public Int32 Id { get; set; }
public string ColumnName { get; set; }
public bool IsVisible { get; set; }
public bool IsMandatory { get; set; }
#endregion

#region GetData
public static List<PersonConfiguration> GetPersonConfigurationList()
{
try
{
string query = @"SELECT * FROM ""PERSON_CONFIGURATION"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetPersonConfigurationAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "GetPersonConfigurationList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetPersonConfigurationForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""PERSON_CONFIGURATION"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "GetPersonConfigurationForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static PersonConfiguration GetPersonConfigurationById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""PERSON_CONFIGURATION"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("PersonConfiguration", i.ToString(), 4, DateTime.Now, 1);
return GetPersonConfigurationAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "GetPersonConfigurationById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static PersonConfiguration GetPersonConfigurationByColumnName(string s)
{
try
{
string query = @"SELECT * FROM ""PERSON_CONFIGURATION"" WHERE ""COLUMN_NAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("PersonConfiguration", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetPersonConfigurationAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "GetPersonConfigurationByColumnName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(PersonConfiguration o)
{
try
{
string query = @"INSERT INTO ""PERSON_CONFIGURATION"" (""COLUMN_NAME"", ""IS_VISIBLE"", ""IS_MANDATORY"") VALUES (@ColumnName, @IsVisible, @IsMandatory) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ColumnName", DbType.String)  { Value = o.ColumnName },
new NpgsqlParameter("@IsVisible", DbType.Boolean)  { Value = o.IsVisible },
new NpgsqlParameter("@IsMandatory", DbType.Boolean)  { Value = o.IsMandatory }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("PersonConfiguration", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(PersonConfiguration o)
{
try
{
string query = @"UPDATE ""PERSON_CONFIGURATION"" SET ""ID"" = @Id, ""COLUMN_NAME"" = @ColumnName, ""IS_VISIBLE"" = @IsVisible, ""IS_MANDATORY"" = @IsMandatory WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ColumnName", DbType.String)  { Value = o.ColumnName },
new NpgsqlParameter("@IsVisible", DbType.Boolean)  { Value = o.IsVisible },
new NpgsqlParameter("@IsMandatory", DbType.Boolean)  { Value = o.IsMandatory },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("PersonConfiguration", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""PERSON_CONFIGURATION"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("PersonConfiguration", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static PersonConfiguration GetPersonConfigurationAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
PersonConfiguration o = new PersonConfiguration();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ColumnName = row["COLUMN_NAME"].ToString();
o.IsVisible = Helper.ConvertToBoolean(row["IS_VISIBLE"]);
o.IsMandatory = Helper.ConvertToBoolean(row["IS_MANDATORY"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "GetPersonConfigurationAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<PersonConfiguration> GetPersonConfigurationAsList(DataTable dt)
{
List<PersonConfiguration> oList = new List<PersonConfiguration>();
foreach (DataRow row in dt.Rows)
{
try
{
PersonConfiguration o = new PersonConfiguration();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ColumnName = row["COLUMN_NAME"].ToString();
o.IsVisible = Helper.ConvertToBoolean(row["IS_VISIBLE"]);
o.IsMandatory = Helper.ConvertToBoolean(row["IS_MANDATORY"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("PersonConfiguration", "GetPersonConfigurationAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
