using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class Hl7Manufacturers
{

#region Properties
public Int32 Id { get; set; }
public string MvxCode { get; set; }
public string Name { get; set; }
public string Notes { get; set; }
public bool IsActive { get; set; }
public DateTime LastUpdated { get; set; }
public Int32 InternalId { get; set; }
#endregion

#region GetData
public static List<Hl7Manufacturers> GetHl7ManufacturersList()
{
try
{
string query = @"SELECT * FROM ""HL7_MANUFACTURERS"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetHl7ManufacturersAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "GetHl7ManufacturersList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetHl7ManufacturersForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""HL7_MANUFACTURERS"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "GetHl7ManufacturersForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static Hl7Manufacturers GetHl7ManufacturersById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""HL7_MANUFACTURERS"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Hl7Manufacturers", i.ToString(), 4, DateTime.Now, 1);
return GetHl7ManufacturersAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "GetHl7ManufacturersById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static Hl7Manufacturers GetHl7ManufacturersByMvxCode(string s)
{
try
{
string query = @"SELECT * FROM ""HL7_MANUFACTURERS"" WHERE ""MVX_CODE"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Hl7Manufacturers", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetHl7ManufacturersAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "GetHl7ManufacturersByMvxCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static Hl7Manufacturers GetHl7ManufacturersByName(string s)
{
try
{
string query = @"SELECT * FROM ""HL7_MANUFACTURERS"" WHERE ""NAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Hl7Manufacturers", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetHl7ManufacturersAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "GetHl7ManufacturersByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(Hl7Manufacturers o)
{
try
{
string query = @"INSERT INTO ""HL7_MANUFACTURERS"" (""MVX_CODE"", ""NAME"", ""NOTES"", ""IS_ACTIVE"", ""LAST_UPDATED"", ""INTERNAL_ID"") VALUES (@MvxCode, @Name, @Notes, @IsActive, @LastUpdated, @InternalId) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@MvxCode", DbType.String)  { Value = o.MvxCode },
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@LastUpdated", DbType.Date)  { Value = o.LastUpdated },
new NpgsqlParameter("@InternalId", DbType.Int32)  { Value = o.InternalId }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Hl7Manufacturers", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(Hl7Manufacturers o)
{
try
{
string query = @"UPDATE ""HL7_MANUFACTURERS"" SET ""ID"" = @Id, ""MVX_CODE"" = @MvxCode, ""NAME"" = @Name, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""LAST_UPDATED"" = @LastUpdated, ""INTERNAL_ID"" = @InternalId WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@MvxCode", DbType.String)  { Value = o.MvxCode },
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@LastUpdated", DbType.Date)  { Value = o.LastUpdated },
new NpgsqlParameter("@InternalId", DbType.Int32)  { Value = o.InternalId },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Hl7Manufacturers", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""HL7_MANUFACTURERS"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Hl7Manufacturers", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""HL7_MANUFACTURERS"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Hl7Manufacturers", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static Hl7Manufacturers GetHl7ManufacturersAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
Hl7Manufacturers o = new Hl7Manufacturers();
o.Id = Helper.ConvertToInt(row["ID"]);
o.MvxCode = row["MVX_CODE"].ToString();
o.Name = row["NAME"].ToString();
o.Notes = row["NOTES"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.LastUpdated = Helper.ConvertToDate(row["LAST_UPDATED"]);
o.InternalId = Helper.ConvertToInt(row["INTERNAL_ID"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "GetHl7ManufacturersAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<Hl7Manufacturers> GetHl7ManufacturersAsList(DataTable dt)
{
List<Hl7Manufacturers> oList = new List<Hl7Manufacturers>();
foreach (DataRow row in dt.Rows)
{
try
{
Hl7Manufacturers o = new Hl7Manufacturers();
o.Id = Helper.ConvertToInt(row["ID"]);
o.MvxCode = row["MVX_CODE"].ToString();
o.Name = row["NAME"].ToString();
o.Notes = row["NOTES"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.LastUpdated = Helper.ConvertToDate(row["LAST_UPDATED"]);
o.InternalId = Helper.ConvertToInt(row["INTERNAL_ID"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("Hl7Manufacturers", "GetHl7ManufacturersAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
