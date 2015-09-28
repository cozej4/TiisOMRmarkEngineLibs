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
public partial class VaccinationAppointment
{

#region Properties
public Int32 Id { get; set; }
public Int32 ChildId { get; set; }
public Int32 ScheduledFacilityId { get; set; }
public DateTime ScheduledDate { get; set; }
public string Notes { get; set; }
public bool IsActive { get; set; }
public DateTime ModifiedOn { get; set; }
public Int32 ModifiedBy { get; set; }
public bool Aefi { get; set; }
public DateTime AefiDate { get; set; }
public bool Outreach { get; set; }
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
public HealthFacility ScheduledFacility
{
get
{
if (this.ScheduledFacilityId > 0)
return HealthFacility.GetHealthFacilityById(this.ScheduledFacilityId);
else
return null;
}
}
#endregion

#region GetData
public static List<VaccinationAppointment> GetVaccinationAppointmentList()
{
try
{
string query = @"SELECT * FROM ""VACCINATION_APPOINTMENT"";";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return GetVaccinationAppointmentAsList(dt);
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static DataTable GetVaccinationAppointmentForList()
{
try
{
string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""VACCINATION_APPOINTMENT"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
return dt;
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}


public static VaccinationAppointment GetVaccinationAppointmentById(Int32 i)
{
try
{
string query = @"SELECT * FROM ""VACCINATION_APPOINTMENT"" WHERE ""ID"" = @ParamValue ";
List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("VaccinationAppointment", i.ToString(), 4, DateTime.Now, 1);
return GetVaccinationAppointmentAsObject(dt);
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

#endregion

#region CRUD
public static int Insert(VaccinationAppointment o)
{
try
{
string query = @"INSERT INTO ""VACCINATION_APPOINTMENT"" (""CHILD_ID"", ""SCHEDULED_FACILITY_ID"", ""SCHEDULED_DATE"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""AEFI"", ""AEFI_DATE"", ""OUTREACH"") VALUES (@ChildId, @ScheduledFacilityId, @ScheduledDate, @Notes, @IsActive, @ModifiedOn, @ModifiedBy, @Aefi, @AefiDate, @Outreach) returning ""ID"" ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@ScheduledFacilityId", DbType.Int32)  { Value = o.ScheduledFacilityId },
new NpgsqlParameter("@ScheduledDate", DbType.Date)  { Value = o.ScheduledDate },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Aefi", DbType.Boolean)  { Value = (object)o.Aefi ?? DBNull.Value },
new NpgsqlParameter("@AefiDate", DbType.Date)  { Value = (object)o.AefiDate ?? DBNull.Value },
new NpgsqlParameter("@Outreach", DbType.Boolean)  { Value = o.Outreach }};
object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("VaccinationAppointment", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
return int.Parse(id.ToString());
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "Insert", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Update(VaccinationAppointment o)
{
try
{
string query = @"UPDATE ""VACCINATION_APPOINTMENT"" SET ""ID"" = @Id, ""CHILD_ID"" = @ChildId, ""SCHEDULED_FACILITY_ID"" = @ScheduledFacilityId, ""SCHEDULED_DATE"" = @ScheduledDate, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""AEFI"" = @Aefi, ""AEFI_DATE"" = @AefiDate, ""OUTREACH"" = @Outreach WHERE ""ID"" = @Id ";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@ScheduledFacilityId", DbType.Int32)  { Value = o.ScheduledFacilityId },
new NpgsqlParameter("@ScheduledDate", DbType.Date)  { Value = o.ScheduledDate },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Aefi", DbType.Boolean)  { Value = (object)o.Aefi ?? DBNull.Value },
new NpgsqlParameter("@AefiDate", DbType.Date)  { Value = (object)o.AefiDate ?? DBNull.Value },
new NpgsqlParameter("@Outreach", DbType.Boolean)  { Value = o.Outreach },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("VaccinationAppointment", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
}
return -1;
}

public static int Delete(int id)
{
try
{
string query = @"DELETE FROM ""VACCINATION_APPOINTMENT"" WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("VaccinationAppointment", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "Delete", 1,  ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}

public static int Remove(int id)
{
try
{
string query = @"UPDATE ""VACCINATION_APPOINTMENT"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
AuditTable.InsertEntity("VaccinationAppointment", id.ToString(), 3, DateTime.Now, 1);
return rowAffected;
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
#endregion

#region Helper Methods
public static VaccinationAppointment GetVaccinationAppointmentAsObject(DataTable dt)
{
foreach (DataRow row in dt.Rows)
{
try
{
VaccinationAppointment o = new VaccinationAppointment();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
o.ScheduledFacilityId = Helper.ConvertToInt(row["SCHEDULED_FACILITY_ID"]);
o.ScheduledDate = Helper.ConvertToDate(row["SCHEDULED_DATE"]);
o.Notes = row["NOTES"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
o.Aefi = Helper.ConvertToBoolean(row["AEFI"]);
o.AefiDate = Helper.ConvertToDate(row["AEFI_DATE"]);
o.Outreach = Helper.ConvertToBoolean(row["OUTREACH"]);
return o;
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return null;
}

public static List<VaccinationAppointment> GetVaccinationAppointmentAsList(DataTable dt)
{
List<VaccinationAppointment> oList = new List<VaccinationAppointment>();
foreach (DataRow row in dt.Rows)
{
try
{
VaccinationAppointment o = new VaccinationAppointment();
o.Id = Helper.ConvertToInt(row["ID"]);
o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
o.ScheduledFacilityId = Helper.ConvertToInt(row["SCHEDULED_FACILITY_ID"]);
o.ScheduledDate = Helper.ConvertToDate(row["SCHEDULED_DATE"]);
o.Notes = row["NOTES"].ToString();
o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
o.Aefi = Helper.ConvertToBoolean(row["AEFI"]);
o.AefiDate = Helper.ConvertToDate(row["AEFI_DATE"]);
o.Outreach = Helper.ConvertToBoolean(row["OUTREACH"]);
oList.Add(o);
}
catch (Exception ex)
{
Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
throw ex;
}
}
return oList;
}
#endregion

}
}
