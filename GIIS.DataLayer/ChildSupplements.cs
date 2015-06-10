using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class ChildSupplements
{

#region Properties
public Int32 Id { get; set; }
public Int32 ChildId { get; set; }
public bool Vita { get; set; }
public bool Mebendezol { get; set; }
public DateTime Date { get; set; }
public DateTime ModifiedOn { get; set; }
public Int32 ModifiedBy { get; set; }
public Child Child
{
get
{
if (this.ChildId > 0)
return Child.GetChildById(this.ChildId);
else
return null;
}
}
#endregion

#region GetData
public static List<ChildSupplements> GetChildSupplementsList()
{
try
{
string query = @"SELECT * FROM ""CHILD_SUPPLEMENTS"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetChildSupplementsAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "GetChildSupplementsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetChildSupplementsForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""CHILD_SUPPLEMENTS"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "GetChildSupplementsForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static ChildSupplements GetChildSupplementsById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""CHILD_SUPPLEMENTS"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ChildSupplements", i.ToString(), 4, DateTime.Now, 1);
return GetChildSupplementsAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "GetChildSupplementsById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(ChildSupplements o)
{
try
{
string query = @"INSERT INTO ""CHILD_SUPPLEMENTS"" (""CHILD_ID"", ""VitA"", ""Mebendezol"", ""DATE"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@ChildId, @Vita, @Mebendezol, @Date, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@Vita", DbType.Boolean)  { Value = o.Vita },
new NpgsqlParameter("@Mebendezol", DbType.Boolean)  { Value = o.Mebendezol },
new NpgsqlParameter("@Date", DbType.Date)  { Value = o.Date },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ChildSupplements", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(ChildSupplements o)
{
try
{
string query = @"UPDATE ""CHILD_SUPPLEMENTS"" SET ""ID"" = @Id, ""CHILD_ID"" = @ChildId, ""VitA"" = @Vita, ""Mebendezol"" = @Mebendezol, ""DATE"" = @Date, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@Vita", DbType.Boolean)  { Value = o.Vita },
new NpgsqlParameter("@Mebendezol", DbType.Boolean)  { Value = o.Mebendezol },
new NpgsqlParameter("@Date", DbType.Date)  { Value = o.Date },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ChildSupplements", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""CHILD_SUPPLEMENTS"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ChildSupplements", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static ChildSupplements GetChildSupplementsAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
ChildSupplements o = new ChildSupplements();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
o.Vita = Helper.ConvertToBoolean(row["VitA"]);
o.Mebendezol = Helper.ConvertToBoolean(row["Mebendezol"]);
o.Date = Helper.ConvertToDate(row["DATE"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "GetChildSupplementsAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<ChildSupplements> GetChildSupplementsAsList(DataTable dt)
{
List<ChildSupplements> oList = new List<ChildSupplements>();
foreach (DataRow row in dt.Rows)
{
try
{
ChildSupplements o = new ChildSupplements();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
o.Vita = Helper.ConvertToBoolean(row["VitA"]);
o.Mebendezol = Helper.ConvertToBoolean(row["Mebendezol"]);
o.Date = Helper.ConvertToDate(row["DATE"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("ChildSupplements", "GetChildSupplementsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
