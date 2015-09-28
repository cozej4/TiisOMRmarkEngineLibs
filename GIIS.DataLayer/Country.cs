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
public partial class Country
{

#region Properties
public Int32 Id { get; set; }
public string Name { get; set; }
#endregion

#region GetData
public static List<Country> GetCountryList()
{
try
{
string query = @"SELECT * FROM ""COUNTRY"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetCountryAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Country", "GetCountryList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetCountryForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""COUNTRY"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("Country", "GetCountryForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static Country GetCountryById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""COUNTRY"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Country", i.ToString(), 4, DateTime.Now, 1);
return GetCountryAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Country", "GetCountryById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static Country GetCountryByName(string s)
{
try
{
string query = @"SELECT * FROM ""COUNTRY"" WHERE ""NAME"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Country", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetCountryAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("Country", "GetCountryByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(Country o)
{
try
{
string query = @"INSERT INTO ""COUNTRY"" (""NAME"") VALUES (@Name) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Country", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("Country", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(Country o)
{
try
{
string query = @"UPDATE ""COUNTRY"" SET ""ID"" = @Id, ""NAME"" = @Name WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Country", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Country", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""COUNTRY"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("Country", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("Country", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static Country GetCountryAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
Country o = new Country();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("Country", "GetCountryAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<Country> GetCountryAsList(DataTable dt)
{
List<Country> oList = new List<Country>();
foreach (DataRow row in dt.Rows)
{
try
{
Country o = new Country();
o.Id = Helper.ConvertToInt(row["ID"]);
o.Name = row["NAME"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("Country", "GetCountryAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
