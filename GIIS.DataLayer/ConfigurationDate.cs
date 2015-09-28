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
public partial class ConfigurationDate
{

#region Properties
public Int32 Id { get; set; }
public string DateFormat { get; set; }
public string DateExpresion { get; set; }
#endregion

#region GetData
public static List<ConfigurationDate> GetConfigurationDateList()
{
try
{
string query = @"SELECT * FROM ""CONFIGURATION_DATE"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetConfigurationDateAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "GetConfigurationDateList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetConfigurationDateForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""CONFIGURATION_DATE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "GetConfigurationDateForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
public static ConfigurationDate GetConfigurationDateById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""CONFIGURATION_DATE"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ConfigurationDate", i.ToString(), 4, DateTime.Now, 1);
return GetConfigurationDateAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "GetConfigurationDateById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static ConfigurationDate GetConfigurationDateByDateFormat(string s)
{
try
{
string query = @"SELECT * FROM ""CONFIGURATION_DATE"" WHERE ""DATE_FORMAT"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ConfigurationDate", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
return GetConfigurationDateAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "GetConfigurationDateByDateFormat", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(ConfigurationDate o)
{
try
{
string query = @"INSERT INTO ""CONFIGURATION_DATE"" (""DATE_FORMAT"", ""DATE_EXPRESION"") VALUES (@DateFormat, @DateExpresion) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@DateFormat", DbType.String)  { Value = o.DateFormat },
new NpgsqlParameter("@DateExpresion", DbType.String)  { Value = o.DateExpresion }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ConfigurationDate", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(ConfigurationDate o)
{
try
{
string query = @"UPDATE ""CONFIGURATION_DATE"" SET ""ID"" = @Id, ""DATE_FORMAT"" = @DateFormat, ""DATE_EXPRESION"" = @DateExpresion WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@DateFormat", DbType.String)  { Value = o.DateFormat },
new NpgsqlParameter("@DateExpresion", DbType.String)  { Value = o.DateExpresion },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ConfigurationDate", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""CONFIGURATION_DATE"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("ConfigurationDate", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static ConfigurationDate GetConfigurationDateAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
ConfigurationDate o = new ConfigurationDate();
o.Id = Helper.ConvertToInt(row["ID"]);
o.DateFormat = row["DATE_FORMAT"].ToString();
o.DateExpresion = row["DATE_EXPRESION"].ToString();
return o;
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "GetConfigurationDateAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<ConfigurationDate> GetConfigurationDateAsList(DataTable dt)
{
List<ConfigurationDate> oList = new List<ConfigurationDate>();
foreach (DataRow row in dt.Rows)
{
try
{
ConfigurationDate o = new ConfigurationDate();
o.Id = Helper.ConvertToInt(row["ID"]);
o.DateFormat = row["DATE_FORMAT"].ToString();
o.DateExpresion = row["DATE_EXPRESION"].ToString();
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("ConfigurationDate", "GetConfigurationDateAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
