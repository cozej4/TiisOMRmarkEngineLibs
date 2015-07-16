using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    partial class VaccinationEvent
    {
        public static int NextDose(int vaccineId, int childId, int doseNo)
        {
            string query = string.Format("Select \"VACCINATION_EVENT\".\"ID\" from  \"VACCINATION_EVENT\" join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\" where \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = {0} AND \"DOSE_NUMBER\" ={2}  AND \"CHILD_ID\" = {1}", vaccineId, childId, doseNo);
            object i = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
            if (!(i == null))
            {
                int count = int.Parse(i.ToString());
                return count;
            }
            else
                return -1;
        }
        public static int UpdateIsActive(int veId, bool isactive)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_EVENT"" SET ""IS_ACTIVE"" = '" + isactive + @"' WHERE ""ID"" = " + veId;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }
        public static bool OtherDose(int vaccineId, int childId, int doseNo)
        {
            string query = string.Format("Select Count(*) from  \"VACCINATION_EVENT\" join \"DOSE\" on \"VACCINATION_EVENT\".\"DOSE_ID\" = \"DOSE\".\"ID\" where \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = {0} AND \"DOSE_NUMBER\" ={2}  AND \"CHILD_ID\" = {1}", vaccineId, childId, doseNo);
            object i = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
            if (!(i is DBNull))
            {
                int count = int.Parse(i.ToString());
                if (count > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
      
        public static List<VaccinationEvent> GetChildVaccinationEvent(int childId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""CHILD_ID""= " + childId + @" ORDER BY ""SCHEDULED_DATE""  ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationEventAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetChildVaccinationEvent", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationEvent> GetChildVaccinationEvent(int childId, DateTime modifiedOn, int userId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""CHILD_ID""= @ChildId AND ""MODIFIEDON"" > @ModifiedOn AND ""MODIFIED_BY"" <> @UserId ORDER BY ""SCHEDULED_DATE"";  ";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ChildId", DbType.Int32) { Value = childId },
                    new NpgsqlParameter("@ModifiedOn", DbType.DateTime) { Value = modifiedOn },
                       new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationEventAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetChildVaccinationEvent", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationEvent> GetChildVaccinationEventBefore(int childId, DateTime modifiedOn, DateTime prevlogin, int userId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""CHILD_ID""= @ChildId AND ""MODIFIEDON"" < @ModifiedOn AND ""MODIFIEDON"" > @PrevLogin AND ""MODIFIED_BY"" <> @UserId ORDER BY ""SCHEDULED_DATE"";  ";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ChildId", DbType.Int32) { Value = childId },
                    new NpgsqlParameter("@ModifiedOn", DbType.DateTime) { Value = modifiedOn },
                     new NpgsqlParameter("@PrevLogin", DbType.DateTime) { Value = prevlogin },
                       new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId }
                };
                
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationEventAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetChildVaccinationEventBefore", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationEvent> GetVaccinationEventByAppointmentId(int appId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""APPOINTMENT_ID""= " + appId + @" ORDER BY ""DOSE_ID""  ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationEventAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetVaccinationEventByAppointmentAndDose", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationEvent> GetVaccinationEventByAppointmentIdNew(int appId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""APPOINTMENT_ID""= " + appId + @" AND ""VACCINATION_STATUS"" = false and ""NONVACCINATION_REASON"" = 0 ORDER BY ""DOSE_ID""  ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationEventAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetVaccinationEventByAppointmentAndDose", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        // new method
        public static DataTable getVaccinationEventforAEFI(int childId) 
        {
            try 
            {
                string query = string.Format(@"Select v.""APPOINTMENT_ID"",  array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" FROM  ""VACCINATION_EVENT"" VE INNER JOIN ""DOSE"" ON VE.""DOSE_ID"" = ""DOSE"".""ID""  " +
                                            @"inner join ""VACCINATION_APPOINTMENT"" VA on VE.""APPOINTMENT_ID"" = VA.""ID"" WHERE  VE.""CHILD_ID"" = {0} AND  v.""APPOINTMENT_ID"" = VE.""APPOINTMENT_ID"" AND ""VACCINATION_STATUS"" = 'TRUE' " +
                                            @"AND (VA.""AEFI"" = 'FALSE' OR VA.""AEFI"" IS NULL) ), ', '::text) AS ""VACCINES"", " +
                                            @" ""VACCINATION_DATE"", ""HEALTH_FACILITY"".""NAME"" as ""HEALTH_FACILITY"", ""VACCINATION_STATUS"" As ""DONE"" " +
                                            @" from ""VACCINATION_EVENT"" v inner join ""HEALTH_FACILITY"" on v.""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITY"".""ID"" " +
                                            @"  inner join ""VACCINATION_APPOINTMENT"" vap on v.""APPOINTMENT_ID"" = vap.""ID"" " +
                                            @"  where  v.""CHILD_ID"" = {0} AND ""VACCINATION_STATUS"" = 'TRUE'  and (vap.""AEFI"" = 'FALSE' OR vap.""AEFI"" IS NULL) " +
                                            @"  GROUP BY v.""APPOINTMENT_ID"", ""VACCINATION_DATE"", ""HEALTH_FACILITY"".""NAME"", ""VACCINATION_STATUS"" ORDER BY ""VACCINATION_DATE"" DESC LIMIT 1 ", childId);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "getVaccinationEventforAEFI", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            } 
        }
        public static DataTable getVaccinationEventforAEFINew(int childId)
        {
            try
            {
                string query = string.Format(@"Select v.""APPOINTMENT_ID"",  array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" FROM  ""VACCINATION_EVENT"" VE INNER JOIN ""DOSE"" ON VE.""DOSE_ID"" = ""DOSE"".""ID""  " +
                                            @" WHERE VE.""CHILD_ID"" = {0} AND  v.""APPOINTMENT_ID"" = VE.""APPOINTMENT_ID"" ), ', '::text) AS ""VACCINES"", " +
                                            @" ""VACCINATION_DATE"", ""HEALTH_FACILITY"".""NAME"" as ""HEALTH_FACILITY"", ""VACCINATION_STATUS"" As ""DONE"", ""AEFI"", ""AEFI_DATE"", vap.""NOTES"" " +
                                            @" from ""VACCINATION_EVENT"" v inner join ""HEALTH_FACILITY"" on v.""HEALTH_FACILITY_ID"" = ""HEALTH_FACILITY"".""ID"" " +
                                            @"  inner join ""VACCINATION_APPOINTMENT"" vap on v.""APPOINTMENT_ID"" = vap.""ID"" " +
                                            @"  where  v.""CHILD_ID"" = {0} AND (""VACCINATION_STATUS"" = 'TRUE' OR ""NONVACCINATION_REASON_ID"" <> 0)   " +
                                            @"  GROUP BY v.""APPOINTMENT_ID"", ""VACCINATION_DATE"", ""HEALTH_FACILITY"".""NAME"", ""VACCINATION_STATUS"", ""AEFI"", ""AEFI_DATE"", vap.""NOTES"" ORDER BY ""VACCINATION_DATE"" ", childId);
                
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

               return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "getVaccinationEventforAFEI", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<VaccinationEvent> GetImmunizationCard(int i)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""CHILD_ID""= " + i + @" ORDER BY ""SCHEDULED_DATE""  ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationEventAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetChildVaccinationEvent", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int Update(int appId, int healthcenterId)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_EVENT"" SET ""HEALTH_FACILITY_ID"" = " + healthcenterId + @" WHERE ""APPOINTMENT_ID"" = " + appId;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }
        public static int Update(int appId, DateTime date)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_EVENT"" SET ""SCHEDULED_DATE"" = '" + date.ToString("yyyy-MM-dd") + @"', ""VACCINATION_DATE"" = '" + date.ToString("yyyy-MM-dd") + @"' WHERE ""APPOINTMENT_ID"" = " + appId;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }
        public static int UpdateEvent(int vId, DateTime date)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_EVENT"" SET ""SCHEDULED_DATE"" = '" + date.ToString("yyyy-MM-dd") + @"', ""VACCINATION_DATE"" = '" + date.ToString("yyyy-MM-dd") + @"' WHERE ""ID"" = " + vId;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "UpdateEvent", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }
              
        public static VaccinationEvent GetVaccinationEventByAppointmentIdAndDoseId(int appId, int doseId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""APPOINTMENT_ID"" = " + appId + @" AND ""DOSE_ID"" = " + doseId + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationEventAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetVaccinationEventByAppointmentIdAndDoseId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static VaccinationEvent GetVaccinationEventByChildIdAndDoseId(int childId, int doseId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_EVENT"" WHERE ""CHILD_ID"" = " + childId + @" AND ""DOSE_ID"" = " + doseId + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationEventAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetVaccinationEventByChildIdAndDoseId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetMonthlyPlan(ref int maximumRows, ref int startRowIndex, int hfId, DateTime scheduledDate)
        {

            try
            {
                if (hfId != 0)
                {
                    string query = string.Format(@"SELECT DISTINCT (""APPOINTMENT_ID""), monthly_plan.""ID"", ""NAME"", " +
                                                 @"array_to_string(ARRAY(SELECT ""DOSE_1"".""FULLNAME"" " +
                                                                @"FROM ""VACCINATION_EVENT"" " +
                                                                @"INNER JOIN ""DOSE"" ""DOSE_1"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE_1"".""ID"" left join ""AGE_DEFINITIONS"" on ""DOSE_1"".""TO_AGE_DEFINITION_ID"" = ""AGE_DEFINITIONS"".""ID"" " +
		                                                        @" WHERE  monthly_plan.""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" AND monthly_plan.""APPOINTMENT_ID"" = ""VACCINATION_EVENT"".""APPOINTMENT_ID"" " + 
		                                                        @" AND ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= '{1}' AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true " +
                                                                @" AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = false AND (""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 OR ""NONVACCINATION_REASON_ID"" in (Select ""ID"" from ""NONVACCINATION_REASON"" where ""KEEP_CHILD_DUE"" = true)) and (""DAYS"" is null  or  (""VACCINATION_EVENT"".""SCHEDULED_DATE""  + interval  '1' day * ""DAYS"" > 'now'::text::date)) ), ', '::text) AS ""VACCINES"", " +
                                                  @" ""SCHEDULE"", ""SCHEDULED_DATE"", ""DOMICILE"" " +
                                                  @" FROM monthly_plan join ""DOSE"" on ""DOSE_ID"" = ""DOSE"".""ID"" " +
                                                  @" WHERE  ""HEALTH_FACILITY_ID"" = {0} AND ""SCHEDULED_DATE"" <= '{1}' " +
                                                   @"   AND ( (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" ) IS NULL OR (""SCHEDULED_DATE""  + interval  '1' day * (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" )  > 'now'::text::date))  " +
                                                  @" GROUP BY ""APPOINTMENT_ID"", ""SCHEDULED_DATE"", ""DOMICILE"", ""NAME"",""SCHEDULE"", monthly_plan.""ID"" ORDER BY ""SCHEDULED_DATE"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + " ", hfId, scheduledDate.ToString("yyyy-MM-dd"));
                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetMonthlyPlan", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
            return null;
        }

        public static int GetCountMonthlyPlan(int hfId, DateTime scheduledDate)
        {

            try
            {
                if (hfId != 0)
                {
                    string query = string.Format(@"SELECT Count (DISTINCT(""APPOINTMENT_ID"")) " +
                                                  @" FROM monthly_plan join ""DOSE"" on ""DOSE_ID"" = ""DOSE"".""ID"" " +
                                                  @" WHERE ""HEALTH_FACILITY_ID"" = {0} AND ""SCHEDULED_DATE"" <= '{1}' AND ( (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" ) IS NULL OR (""SCHEDULED_DATE""  + interval  '1' day * (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" )  > 'now'::text::date))  " +
                                                  @" ", hfId, scheduledDate.ToString("yyyy-MM-dd"));
                    object i = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                    return int.Parse(i.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetCountMonthlyPlan", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
            return 0;
        }

        //public static List<VaccinationEvent> GetMonthlyPlan(int healthcenterId, int currentMonth, int currentYear)
        //{

        //    try
        //    {
        //        if (healthcenterId != 0)
        //        {

        //            DateTime monthDate = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));
        //            string query = string.Format("-- Get due data: vaccines of previous months that have status false: are not done " + Environment.NewLine + "-------------------------------------------------------------------------------------- " + Environment.NewLine + "SELECT \"VACCINATION_EVENT\".* FROM \"VACCINATION_APPOINTMENT\", \"VACCINATION_EVENT\" " + Environment.NewLine + "WHERE  \"VACCINATION_APPOINTMENT\".\"ID\" = \"VACCINATION_EVENT\".\"APPOINTMENT_ID\" " + Environment.NewLine + " AND  \"VACCINATION_EVENT\".\"VACCINATION_STATUS\" = False AND \"NONVACCINATION_REASON_ID\" = 0 AND \"VACCINATION_APPOINTMENT\".\"IS_ACTIVE\" = True " + Environment.NewLine + " AND  \"VACCINATION_EVENT\".\"SCHEDULED_DATE\" <= '{0}'  " + Environment.NewLine + " AND \"VACCINATION_EVENT\".\"IS_ACTIVE\" = true  AND \"SCHEDULED_FACILITY_ID\" = {1} " + Environment.NewLine + "-------------------------------------------------------------------------------------- " + Environment.NewLine + "ORDER BY \"VACCINATION_EVENT\".\"SCHEDULED_DATE\", \"VACCINATION_EVENT\".\"CHILD_ID\" ", monthDate.ToString("yyyy-MM-dd"), healthcenterId);

        //            DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                    
        //            List<VaccinationEvent> list = GetVaccinationEventAsList(dt);
        //            List<VaccinationEvent> newlist = new List<VaccinationEvent>();

        //            foreach (VaccinationEvent ve in list) {
        //                if (newlist.Count == 0)
        //                {
        //                    List<Dose> doseList = new List<Dose>();
        //                    ve.VaccineDose.VaccinationEventId = ve.Id;
        //                    doseList.Add(ve.VaccineDose);
        //                    ve.VaccineDoseList = doseList;
        //                    newlist.Add(ve);
        //                }
        //                else {
        //                    bool found = false;
        //                    foreach (VaccinationEvent ve1 in newlist)
        //                    {
        //                        if (ve.ChildId == ve1.ChildId)
        //                        {
        //                            ve.VaccineDose.VaccinationEventId = ve.Id;
        //                            ve1.VaccineDoseList.Add(ve.VaccineDose);
        //                            found = true;
        //                            break;
        //                        }
        //                    }

        //                    if (found == false)
        //                    {
        //                        List<Dose> doseList = new List<Dose>();
        //                        ve.VaccineDose.VaccinationEventId = ve.Id;
        //                        doseList.Add(ve.VaccineDose);
        //                        ve.VaccineDoseList = doseList;
        //                        newlist.Add(ve);
        //                    }
        //                }
        //            }

        //            return newlist;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertEntity("VaccinationEvent", "GetMonthlyPlan", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
        //        throw ex;
        //    }
        //    return null;
        //}

        public static DataTable GetNextDueVaccinesForChild(int childId)
        {

            try
            {
                string query = string.Format(@"SELECT v.""APPOINTMENT_ID"", " +
                                             @" array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" FROM " +
                                             @" ""VACCINATION_EVENT"" INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID"" left join ""AGE_DEFINITIONS"" on ""DOSE"".""TO_AGE_DEFINITION_ID"" = ""AGE_DEFINITIONS"".""ID"" " +
                                             @"  WHERE ""CHILD_ID"" = {0} AND  v.""APPOINTMENT_ID"" = ""VACCINATION_EVENT"".""APPOINTMENT_ID""  " +
                                             @" AND (""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= 'now' OR ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= ('now'::text::date + '2 mon'::interval)) AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true " +
                                             @" AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = false AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 and (""DAYS"" is null  or  (""VACCINATION_EVENT"".""SCHEDULED_DATE""  + interval  '1' day * ""DAYS"" > 'now'::text::date)) ), ', '::text) AS ""VACCINES"", " +
                                             @" v.""SCHEDULED_DATE"" " +
                                             @" FROM ""VACCINATION_EVENT"" v " +
                                             @" WHERE v.""CHILD_ID"" = {0} AND (v.""SCHEDULED_DATE"" >= 'now' or v.""SCHEDULED_DATE"" < 'now')  AND v.""IS_ACTIVE"" = true AND  v.""VACCINATION_STATUS"" = false AND v.""NONVACCINATION_REASON_ID"" = 0 " +
                                             @" GROUP BY v.""APPOINTMENT_ID"", v.""SCHEDULED_DATE"" ORDER BY v.""SCHEDULED_DATE"" LIMIT 1", childId);

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                    return dt;
                }
            
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetNextDueVaccinesForChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetDueVaccinesForChild(int childId)
        {

            try
            {
                string query = string.Format(@"SELECT v.""APPOINTMENT_ID"", " +
                                             @" array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" FROM " +
                                             @" ""VACCINATION_EVENT"" INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID"" left join ""AGE_DEFINITIONS"" on ""DOSE"".""TO_AGE_DEFINITION_ID"" = ""AGE_DEFINITIONS"".""ID"" " +
                                             @"  WHERE ""CHILD_ID"" = {0} AND  v.""APPOINTMENT_ID"" = ""VACCINATION_EVENT"".""APPOINTMENT_ID""  " +
                                             @" AND (""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= 'now' OR ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= ('now'::text::date + '2 mon'::interval)) AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true " +
                                             @" AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = false AND (""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0  OR ""NONVACCINATION_REASON_ID"" in (Select ""ID"" from ""NONVACCINATION_REASON"" where ""KEEP_CHILD_DUE"" = true)) and (""DAYS"" is null  or  (""VACCINATION_EVENT"".""SCHEDULED_DATE""  + interval  '1' day * ""DAYS"" > 'now'::text::date)) ), ', '::text) AS ""VACCINES"", " +
                                             @" a.""NAME"" as ""SCHEDULE"", v.""SCHEDULED_DATE"" " +
                                             @" FROM ""VACCINATION_EVENT"" v " +
                                             @" INNER JOIN ""DOSE"" on v.""DOSE_ID"" = ""DOSE"".""ID"" " +
                                             @" INNER JOIN ""AGE_DEFINITIONS"" a on ""DOSE"".""AGE_DEFINITION_ID"" = a.""ID"" " +
                                             @" WHERE v.""CHILD_ID"" = {0} AND (v.""SCHEDULED_DATE"" <= 'now' OR v.""SCHEDULED_DATE"" <= ('now'::text::date + '2 mon'::interval)) AND v.""IS_ACTIVE"" = true AND  v.""VACCINATION_STATUS"" = false AND (v.""NONVACCINATION_REASON_ID"" = 0 OR ""NONVACCINATION_REASON_ID"" in (Select ""ID"" from ""NONVACCINATION_REASON"" where ""KEEP_CHILD_DUE"" = true)) " +
                                             @"   AND ( (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" ) IS NULL OR (v.""SCHEDULED_DATE""  + interval  '1' day * (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" )  > 'now'::text::date))  " +
                                             @" GROUP BY v.""APPOINTMENT_ID"", v.""SCHEDULED_DATE"", a.""NAME"" ORDER BY v.""SCHEDULED_DATE""  ", childId);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                return dt;
            }

            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetDueVaccinesForChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetDueVaccinesForChild(int childId, DateTime scheduledDate)
        {
            try
            {
                string days = Configuration.GetConfigurationByName("EligibleForVaccination").Value;

                string query = string.Format(@"SELECT v.""APPOINTMENT_ID"", " +
                                             @" array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" FROM " +
                                             @" ""VACCINATION_EVENT"" INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID"" " +
                                             @"  WHERE ""CHILD_ID"" = {0} AND  v.""APPOINTMENT_ID"" = ""VACCINATION_EVENT"".""APPOINTMENT_ID""  " +
                                             @" AND (""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= @ScheduledDate OR ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= (@ScheduledDate::text::date + '{1} day'::interval)) AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true " +
                                             @" AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = false AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 ), ', '::text) AS ""VACCINES"", " +
                                             @" a.""NAME"" as ""SCHEDULE"", v.""SCHEDULED_DATE"" " +
                                             @" FROM ""VACCINATION_EVENT"" v " +
                                             @" INNER JOIN ""DOSE"" on v.""DOSE_ID"" = ""DOSE"".""ID"" " +
                                             @" INNER JOIN ""AGE_DEFINITIONS"" a on ""DOSE"".""AGE_DEFINITION_ID"" = a.""ID"" " +
                                             @" WHERE v.""CHILD_ID"" = {0} AND (v.""SCHEDULED_DATE"" <= @ScheduledDate OR v.""SCHEDULED_DATE"" <= (@ScheduledDate::text::date + '{1} day'::interval)) AND v.""IS_ACTIVE"" = true AND  v.""VACCINATION_STATUS"" = false AND v.""NONVACCINATION_REASON_ID"" = 0 " +
                                             @" GROUP BY v.""APPOINTMENT_ID"", v.""SCHEDULED_DATE"", a.""NAME"" ORDER BY v.""SCHEDULED_DATE""  ", childId, days);

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ScheduledDate", DbType.DateTime) { Value = scheduledDate }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

                return dt;
            }

            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetDueVaccinesForChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetDefaultersList()
        {
            try
            {
                string days = Configuration.GetConfigurationByName("Defaulters").Value;

                string query = string.Format(@"SELECT DISTINCT ON (""ID"") ""ID"" as ""Child Id"", ""NAME"" as ""Child Name"", " +
                                               @" array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" " +
                                               @" FROM ""VACCINATION_EVENT"" INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID"" " +
                                               @" WHERE  monthly_plan.""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" AND ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= ('now'::text::date - '{0} day'::interval)  AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true " +
                                               @" AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = false AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 ORDER BY ""SCHEDULED_DATE"" ), ', '::text) AS ""VACCINES"", " +
                                               @" ""SCHEDULED_DATE"" " +
                                               @" FROM monthly_plan " +
                                               @" WHERE ""SCHEDULED_DATE"" <= ('now'::text::date - '{0} day'::interval) " +
                                               @" GROUP BY ""ID"", ""NAME"", ""SCHEDULED_DATE"", ""DOMICILE"", ""DOMICILE_ID"" ", days);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetDefaultersList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
           // return null;
        }

        public static DataTable GetDefaulters()
        {
            try
            {
                string days = Configuration.GetConfigurationByName("Defaulters").Value;
                string query = string.Format(@"SELECT DISTINCT  ""CHILD_ID"" " +
                                               @" FROM ""VACCINATION_EVENT"" " +
                                               @" WHERE ""SCHEDULED_DATE"" <= ('now'::text::date - '{0} day'::interval) AND ""VACCINATION_STATUS"" = false and ""NONVACCINATION_REASON_ID"" = 0 AND ""IS_ACTIVE"" = true and ""CHILD_ID"" in (SELECT ""ID"" from ""CHILD"" where ""STATUS_ID"" = 1) " +
                                               @" GROUP BY ""CHILD_ID"" ", days);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetDefaulters", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
            // return null;
        }
        public static DataTable GetDefaultersList(string healthFacilityId, int domicileId)
        {
            try
            {
                if (!String.IsNullOrEmpty(healthFacilityId))
                {
                    string days = Configuration.GetConfigurationByName("Defaulters").Value;

                    string query = string.Format(@"SELECT DISTINCT ON (monthly_plan.""ID"") monthly_plan.""ID"", ""NAME"", " +
                                                @" array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" " +
                                                @" FROM ""VACCINATION_EVENT""  INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID"" left join ""AGE_DEFINITIONS"" on ""DOSE"".""TO_AGE_DEFINITION_ID"" = ""AGE_DEFINITIONS"".""ID"" " +
                                                @" WHERE monthly_plan.""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" AND ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= ('now'::text::date - '{1} day'::interval)  AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true " +
                                                @" AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = false AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 and (""DAYS"" is null  or  (""VACCINATION_EVENT"".""SCHEDULED_DATE""  + interval  '1' day * ""DAYS"" > 'now'::text::date)) ORDER BY ""SCHEDULED_DATE"" ), ', '::text) AS ""VACCINES"", " +
                                                @" ""SCHEDULED_DATE"",""DOMICILE"", ""DOMICILE_ID"" " +
                                                @" FROM monthly_plan join ""DOSE"" on ""DOSE_ID"" = ""DOSE"".""ID"" " +
                                                @" WHERE ""HEALTH_FACILITY_ID"" in ({0}) AND ""SCHEDULED_DATE"" <= ('now'::text::date - '{1} day'::interval) " +
                                                @"   AND ( (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" ) IS NULL OR (""SCHEDULED_DATE""  + interval  '1' day * (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" )  > 'now'::text::date))  " +
                                                @" GROUP BY monthly_plan.""ID"", ""NAME"", ""SCHEDULED_DATE"", ""DOMICILE"", ""DOMICILE_ID"" ", healthFacilityId, days);

                    if (domicileId > 0)
                        query = string.Format(@"SELECT DISTINCT ON (monthly_plan.""ID"") monthly_plan.""ID"", ""NAME"", " +
                                                @" array_to_string(ARRAY( SELECT ""DOSE"".""FULLNAME"" " +
                                                @" FROM ""VACCINATION_EVENT"" INNER JOIN ""DOSE"" ON ""VACCINATION_EVENT"".""DOSE_ID"" = ""DOSE"".""ID"" left join ""AGE_DEFINITIONS"" on ""DOSE"".""TO_AGE_DEFINITION_ID"" = ""AGE_DEFINITIONS"".""ID"" " +
                                                @" WHERE  monthly_plan.""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" AND ""VACCINATION_EVENT"".""SCHEDULED_DATE"" <= ('now'::text::date - '{2} day'::interval)  AND ""VACCINATION_EVENT"".""IS_ACTIVE"" = true " +
                                                @" AND ""VACCINATION_EVENT"".""VACCINATION_STATUS"" = false AND ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = 0 and (""DAYS"" is null  or  (""VACCINATION_EVENT"".""SCHEDULED_DATE""  + interval  '1' day * ""DAYS"" > 'now'::text::date)) ORDER BY ""SCHEDULED_DATE"" ), ', '::text) AS ""VACCINES"", " +
                                                @" ""SCHEDULED_DATE"",""DOMICILE"", ""DOMICILE_ID"" " +
                                                @" FROM monthly_plan join ""DOSE"" on ""DOSE_ID"" = ""DOSE"".""ID"" " +
                                                @" WHERE ""HEALTH_FACILITY_ID"" in ({0}) AND ""DOMICILE_ID"" = {1} AND ""SCHEDULED_DATE"" <= ('now'::text::date - '{2} day'::interval) " +
                                                @"   AND ( (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" ) IS NULL OR (""SCHEDULED_DATE""  + interval  '1' day * (Select ""DAYS"" from ""AGE_DEFINITIONS"" WHERE ""ID"" = ""DOSE"".""TO_AGE_DEFINITION_ID"" )  > 'now'::text::date))  " +
                                                @" GROUP BY monthly_plan.""ID"", ""NAME"", ""SCHEDULED_DATE"", ""DOMICILE"", ""DOMICILE_ID"" ", healthFacilityId, domicileId, days);
                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetDefaultersList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
            return null;
        }

        public static int DeleteByChild(int id)
        {
            try
            {
                string query = @"DELETE FROM ""VACCINATION_EVENT"" WHERE ""CHILD_ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "DeleteByChild", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }

    public struct VaccineQuantity
    {
        public int VaccineId { get; set; }
        public int Quantity { get; set; }
        public string VaccineCode { get; set; }

        #region Helper Methods
        public VaccineQuantity GetVaccineQuantityAsObject(DataTable dt)
        {
            VaccineQuantity o = new VaccineQuantity();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    o.VaccineId = Helper.ConvertToInt(row["VACCINE_ID"]);
                    o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
                    o.VaccineCode = ScheduledVaccination.GetScheduledVaccinationById(o.VaccineId).Code;

                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("VaccinationEvent", "GetVaccineQuantityAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return o;
        }

        public static List<VaccineQuantity> GetVaccineQuantityAsList(DataTable dt)
        {
            List<VaccineQuantity> oList = new List<VaccineQuantity>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    VaccineQuantity o = new VaccineQuantity();
                    o.VaccineId = Helper.ConvertToInt(row["VACCINE_ID"]);
                    o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
                    o.VaccineCode = ScheduledVaccination.GetScheduledVaccinationById(o.VaccineId).Code;


                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("VaccinationEvent", "GetVaccineQuantityAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

        public static List<VaccineQuantity> GetQuantityMonthlyPlan(string hfId, DateTime scheduledDate)
        {

            try
            {
                if (! String.IsNullOrEmpty(hfId))
                {
                    string query = string.Format("Select \"SCHEDULED_VACCINATION_ID\" as VACCINE_ID, COUNT(\"SCHEDULED_VACCINATION_ID\") as QUANTITY  FROM monthly_plan WHERE \"HEALTH_FACILITY_ID\" in ({0}) AND \"SCHEDULED_DATE\" <= '{1}'  " + Environment.NewLine + " GROUP BY \"SCHEDULED_VACCINATION_ID\"  ORDER BY \"SCHEDULED_VACCINATION_ID\" ", hfId, scheduledDate );

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                    return GetVaccineQuantityAsList(dt);
                }
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationEvent", "GetQuantityMonthlyPlan", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
            return null;
        }
    }
}