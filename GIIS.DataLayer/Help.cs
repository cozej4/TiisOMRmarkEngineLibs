using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class Help
{

#region Properties
public Int32 Id { get; set; }
public string Page { get; set; }
public string HelpText { get; set; }
public Int32 LanguageId { get; set; }
public DateTime ModifiedOn { get; set; }
public Int32 ModifiedBy { get; set; }
public Language Language
{
get
{
if (this.LanguageId > 0)
return Language.GetLanguageById(this.LanguageId);
else
return null;
}
}
#endregion

#region GetData
public static List<Help> GetHelpList()
{
try
{
string query = @"SELECT * FROM ""HELP"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetHelpAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Help", "GetHelpList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetHelpForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""HELP"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("Help", "GetHelpForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static Help GetHelpById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""HELP"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Help", i.ToString(), 4, DateTime.Now, 1);
return GetHelpAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Help", "GetHelpById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(Help o)
{
try
{
string query = @"INSERT INTO ""HELP"" (""PAGE"", ""HELP_TEXT"", ""LANGUAGE_ID"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@Page, @HelpText, @LanguageId, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Page", DbType.String)  { Value = (object)o.Page ?? DBNull.Value },
new NpgsqlParameter("@HelpText", DbType.String)  { Value = (object)o.HelpText ?? DBNull.Value },
new NpgsqlParameter("@LanguageId", DbType.Int32)  { Value = o.LanguageId },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Help", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("Help", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(Help o)
{
try
{
string query = @"UPDATE ""HELP"" SET ""ID"" = @Id, ""PAGE"" = @Page, ""HELP_TEXT"" = @HelpText, ""LANGUAGE_ID"" = @LanguageId, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Page", DbType.String)  { Value = (object)o.Page ?? DBNull.Value },
new NpgsqlParameter("@HelpText", DbType.String)  { Value = (object)o.HelpText ?? DBNull.Value },
new NpgsqlParameter("@LanguageId", DbType.Int32)  { Value = o.LanguageId },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Help", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Help", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""HELP"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Help", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Help", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static Help GetHelpAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
Help o = new Help();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Page = row["PAGE"].ToString();
o.HelpText = row["HELP_TEXT"].ToString();
o.LanguageId = Helper.ConvertToInt(row["LANGUAGE_ID"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("Help", "GetHelpAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<Help> GetHelpAsList(DataTable dt)
{
List<Help> oList = new List<Help>();
foreach (DataRow row in dt.Rows)
{
try
{
Help o = new Help();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Page = row["PAGE"].ToString();
o.HelpText = row["HELP_TEXT"].ToString();
o.LanguageId = Helper.ConvertToInt(row["LANGUAGE_ID"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("Help", "GetHelpAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
