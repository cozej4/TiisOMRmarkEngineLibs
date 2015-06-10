using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
public partial class ItemCategory
{

#region Properties
public Int32 Id { get; set; }
public string Name { get; set; }
public string Code { get; set; }
public bool IsActive { get; set; }
public string Notes { get; set; }
public DateTime ModifiedOn { get; set; }
public Int32 ModifiedBy { get; set; }
#endregion

#region GetData
public static List<ItemCategory> GetItemCategoryList()
{
try
{
string query = @"SELECT * FROM ""ITEM_CATEGORY"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetItemCategoryAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "GetItemCategoryList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetItemCategoryForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ITEM_CATEGORY"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "GetItemCategoryForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static ItemCategory GetItemCategoryById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""ITEM_CATEGORY"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ItemCategory", i.ToString(), 4, DateTime.Now, 1);
return GetItemCategoryAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "GetItemCategoryById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static ItemCategory GetItemCategoryByCode(string s)
{
try
{
string query = @"SELECT * FROM ""ITEM_CATEGORY"" WHERE ""CODE"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ItemCategory", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetItemCategoryAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "GetItemCategoryByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static ItemCategory GetItemCategoryByName(string s)
{
try
{
string query = @"SELECT * FROM ""ITEM_CATEGORY"" WHERE ""NAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ItemCategory", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetItemCategoryAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "GetItemCategoryByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(ItemCategory o)
{
try
{
string query = @"INSERT INTO ""ITEM_CATEGORY"" (""NAME"", ""CODE"", ""IS_ACTIVE"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@Name, @Code, @IsActive, @Notes, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ItemCategory", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(ItemCategory o)
{
try
{
string query = @"UPDATE ""ITEM_CATEGORY"" SET ""ID"" = @Id, ""NAME"" = @Name, ""CODE"" = @Code, ""IS_ACTIVE"" = @IsActive, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ItemCategory", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""ITEM_CATEGORY"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ItemCategory", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""ITEM_CATEGORY"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ItemCategory", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static ItemCategory GetItemCategoryAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
ItemCategory o = new ItemCategory();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.Code = row["CODE"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.Notes = row["NOTES"].ToString();
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "GetItemCategoryAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<ItemCategory> GetItemCategoryAsList(DataTable dt)
{
List<ItemCategory> oList = new List<ItemCategory>();
foreach (DataRow row in dt.Rows)
{
try
{
ItemCategory o = new ItemCategory();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
o.Code = row["CODE"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.Notes = row["NOTES"].ToString();
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("ItemCategory", "GetItemCategoryAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
