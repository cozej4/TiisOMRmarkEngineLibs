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
    public partial class VaccinationEvent
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 AppointmentId { get; set; }
        public Int32 ChildId { get; set; }
        public Int32 DoseId { get; set; }
        public Int32 VaccineLotId { get; set; }
        public string VaccineLotText { get; set; }
        public Int32 HealthFacilityId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime VaccinationDate { get; set; }
        public string Notes { get; set; }
        public bool VaccinationStatus { get; set; }
        public Int32 NonvaccinationReasonId { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public VaccinationAppointment Appointment
        {
            get
            {
                if (this.AppointmentId > 0)
                    return VaccinationAppointment.GetVaccinationAppointmentById(this.AppointmentId);
                else
                    return null;
            }
        }
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
        public Dose Dose
        {
            get
            {
                if (this.DoseId > 0)
                    return Dose.GetDoseById(this.DoseId);
                else
                    return null;
            }
        }
        public NonvaccinationReason NonVaccinationReason
        {
            get
            {
                if (NonvaccinationReasonId != 0)
                    return NonvaccinationReason.GetNonvaccinationReasonById(NonvaccinationReasonId);
                else
                    return null;
            }
        }
        public string VaccineLot
        {
            get
            {
                if (VaccineLotId > 0)
                    return ItemLot.GetItemLotById(VaccineLotId).LotNumber;
                else if (VaccineLotId == -2)
                    return "------";
                else if (VaccineLotText != String.Empty)
                    return VaccineLotText;
                else
                    return "";
            }
        }
        #endregion

        #region GetData
        public static List<VaccinationEvent> GetVaccinationEventList()
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationEventAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetVaccinationEventList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetVaccinationEventForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""VACCINATION_EVENT"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetVaccinationEventForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }




        public static VaccinationEvent GetVaccinationEventById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("VaccinationEvent", i.ToString(), 4, DateTime.Now, 1);
                return GetVaccinationEventAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetVaccinationEventById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(VaccinationEvent o)
        {
            try
            {
                string query = @"INSERT INTO ""VACCINATION_EVENT"" (""APPOINTMENT_ID"", ""CHILD_ID"", ""DOSE_ID"", ""VACCINE_LOT_ID"", ""VACCINE_LOT_TEXT"", ""HEALTH_FACILITY_ID"", ""SCHEDULED_DATE"", ""VACCINATION_DATE"", ""NOTES"", ""VACCINATION_STATUS"", ""NONVACCINATION_REASON_ID"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""MODIFIEDON"") VALUES (@AppointmentId, @ChildId, @DoseId, @VaccineLotId, @VaccineLotText, @HealthFacilityId, @ScheduledDate, @VaccinationDate, @Notes, @VaccinationStatus, @NonvaccinationReasonId, @IsActive, @ModifiedOn, @ModifiedBy,@ModifiedOn) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@AppointmentId", DbType.Int32)  { Value = o.AppointmentId },
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@DoseId", DbType.Int32)  { Value = o.DoseId },
new NpgsqlParameter("@VaccineLotId", DbType.Int32)  { Value = (object)o.VaccineLotId ?? DBNull.Value },
new NpgsqlParameter("@VaccineLotText", DbType.String)  { Value = (object)o.VaccineLotText ?? DBNull.Value },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId },
new NpgsqlParameter("@ScheduledDate", DbType.Date)  { Value = o.ScheduledDate },
new NpgsqlParameter("@VaccinationDate", DbType.Date)  { Value = o.VaccinationDate },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@VaccinationStatus", DbType.Boolean)  { Value = o.VaccinationStatus },
new NpgsqlParameter("@NonvaccinationReasonId", DbType.Int32)  { Value = (object)o.NonvaccinationReasonId ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("VaccinationEvent", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(VaccinationEvent o)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_EVENT"" SET ""ID"" = @Id, ""APPOINTMENT_ID"" = @AppointmentId, ""CHILD_ID"" = @ChildId, ""DOSE_ID"" = @DoseId, ""VACCINE_LOT_ID"" = @VaccineLotId, ""VACCINE_LOT_TEXT"" = @VaccineLotText, ""HEALTH_FACILITY_ID"" = @HealthFacilityId, ""SCHEDULED_DATE"" = @ScheduledDate, ""VACCINATION_DATE"" = @VaccinationDate, ""NOTES"" = @Notes, ""VACCINATION_STATUS"" = @VaccinationStatus, ""NONVACCINATION_REASON_ID"" = @NonvaccinationReasonId, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""MODIFIEDON"" = @ModifiedOn WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@AppointmentId", DbType.Int32)  { Value = o.AppointmentId },
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@DoseId", DbType.Int32)  { Value = o.DoseId },
new NpgsqlParameter("@VaccineLotId", DbType.Int32)  { Value = (object)o.VaccineLotId ?? DBNull.Value },
new NpgsqlParameter("@VaccineLotText", DbType.String)  { Value = (object)o.VaccineLotText ?? DBNull.Value },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = o.HealthFacilityId },
new NpgsqlParameter("@ScheduledDate", DbType.Date)  { Value = o.ScheduledDate },
new NpgsqlParameter("@VaccinationDate", DbType.Date)  { Value = o.VaccinationDate },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@VaccinationStatus", DbType.Boolean)  { Value = o.VaccinationStatus },
new NpgsqlParameter("@NonvaccinationReasonId", DbType.Int32)  { Value = (object)o.NonvaccinationReasonId ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("VaccinationEvent", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""VACCINATION_EVENT"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("VaccinationEvent", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_EVENT"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("VaccinationEvent", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static VaccinationEvent GetVaccinationEventAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    VaccinationEvent o = new VaccinationEvent();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.AppointmentId = Helper.ConvertToInt(row["APPOINTMENT_ID"]);
                    o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
                    o.DoseId = Helper.ConvertToInt(row["DOSE_ID"]);
                    o.VaccineLotId = Helper.ConvertToInt(row["VACCINE_LOT_ID"]);
                    o.VaccineLotText = row["VACCINE_LOT_TEXT"].ToString();
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.ScheduledDate = Helper.ConvertToDate(row["SCHEDULED_DATE"]);
                    o.VaccinationDate = Helper.ConvertToDate(row["VACCINATION_DATE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.VaccinationStatus = Helper.ConvertToBoolean(row["VACCINATION_STATUS"]);
                    o.NonvaccinationReasonId = Helper.ConvertToInt(row["NONVACCINATION_REASON_ID"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Timestamp = Helper.ConvertToDate(row["MODIFIEDON"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("VaccinationEvent", "GetVaccinationEventAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<VaccinationEvent> GetVaccinationEventAsList(DataTable dt)
        {
            List<VaccinationEvent> oList = new List<VaccinationEvent>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    VaccinationEvent o = new VaccinationEvent();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.AppointmentId = Helper.ConvertToInt(row["APPOINTMENT_ID"]);
                    o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
                    o.DoseId = Helper.ConvertToInt(row["DOSE_ID"]);
                    o.VaccineLotId = Helper.ConvertToInt(row["VACCINE_LOT_ID"]);
                    o.VaccineLotText = row["VACCINE_LOT_TEXT"].ToString();
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.ScheduledDate = Helper.ConvertToDate(row["SCHEDULED_DATE"]);
                    o.VaccinationDate = Helper.ConvertToDate(row["VACCINATION_DATE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.VaccinationStatus = Helper.ConvertToBoolean(row["VACCINATION_STATUS"]);
                    o.NonvaccinationReasonId = Helper.ConvertToInt(row["NONVACCINATION_REASON_ID"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Timestamp = Helper.ConvertToDate(row["MODIFIEDON"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("VaccinationEvent", "GetVaccinationEventAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
