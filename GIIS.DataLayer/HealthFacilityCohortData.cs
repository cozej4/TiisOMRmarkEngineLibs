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
public partial class HealthFacilityCohortData
{

#region Properties
public Int32 Id { get; set; }
public Int32 HealthFacilityId { get; set; }
public Int32 Year { get; set; }
public Int32 Cohort { get; set; }
public string Notes { get; set; }
public DateTime ModifiedOn { get; set; }
public Int32 ModifiedBy { get; set; }
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
public static List<HealthFacilityCohortData> GetHealthFacilityCohortDataList()
{
try
{
string query = @"SELECT * FROM ""HEALTH_FACILITY_COHORT_DATA"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetHealthFacilityCohortDataAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetHealthFacilityCohortDataForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static HealthFacilityCohortData GetHealthFacilityCohortDataById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("HealthFacilityCohortData", i.ToString(), 4, DateTime.Now, 1);
return GetHealthFacilityCohortDataAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static HealthFacilityCohortData GetHealthFacilityCohortDataByHealthFacilityId(Int32 i)
{
try
{
string query = @"SELECT * FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE ""HEALTH_FACILITY_ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("HealthFacilityCohortData", i.ToString(), 4, DateTime.Now, 1);
return GetHealthFacilityCohortDataAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static HealthFacilityCohortData GetHealthFacilityCohortDataByYear(Int32 i)
{
try
{
string query = @"SELECT * FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE ""YEAR"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("HealthFacilityCohortData", i.ToString(), 4, DateTime.Now, 1);
return GetHealthFacilityCohortDataAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataByYear", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(HealthFacilityCohortData o)
{
try
{
string query = @"INSERT INTO ""HEALTH_FACILITY_COHORT_DATA"" (""HEALTH_FACILITY_ID"", ""YEAR"", ""COHORT"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@HealthFacilityId, @Year, @Cohort, @Notes, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId },
new NpgsqlParameter("@Year", DbType.Int32)  { Value = o.Year },
new NpgsqlParameter("@Cohort", DbType.Int32)  { Value = o.Cohort },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("HealthFacilityCohortData", id.ToString(), 1, DateTime.Now, 1);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(HealthFacilityCohortData o)
{
try
{
string query = @"UPDATE ""HEALTH_FACILITY_COHORT_DATA"" SET ""ID"" = @Id, ""HEALTH_FACILITY_ID"" = @HealthFacilityId, ""YEAR"" = @Year, ""COHORT"" = @Cohort, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId },
new NpgsqlParameter("@Year", DbType.Int32)  { Value = o.Year },
new NpgsqlParameter("@Cohort", DbType.Int32)  { Value = o.Cohort },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("HealthFacilityCohortData", o.Id.ToString(), 2, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("HealthFacilityCohortData", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region Helper Methods
public static HealthFacilityCohortData GetHealthFacilityCohortDataAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
HealthFacilityCohortData o = new HealthFacilityCohortData();
o.Id = Helper.ConvertToInt(row["ID"]);
o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
o.Year = Helper.ConvertToInt(row["YEAR"]);
o.Cohort = Helper.ConvertToInt(row["COHORT"]);
o.Notes = row["NOTES"].ToString();
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<HealthFacilityCohortData> GetHealthFacilityCohortDataAsList(DataTable dt)
{
List<HealthFacilityCohortData> oList = new List<HealthFacilityCohortData>();
foreach (DataRow row in dt.Rows)
{
try
{
HealthFacilityCohortData o = new HealthFacilityCohortData();
o.Id = Helper.ConvertToInt(row["ID"]);
o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
o.Year = Helper.ConvertToInt(row["YEAR"]);
o.Cohort = Helper.ConvertToInt(row["COHORT"]);
o.Notes = row["NOTES"].ToString();
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
