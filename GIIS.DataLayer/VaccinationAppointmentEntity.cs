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
using System.Linq;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    partial class VaccinationAppointment
    {
        public static List<VaccinationAppointment> GetVaccinationAppointmentForList(string where)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_APPOINTMENT"" WHERE " + where + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationAppointment> GetVaccinationAppointmentsByChildNotModified(int childId)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_APPOINTMENT""  WHERE ""CHILD_ID"" = " + childId + @" AND ""ID"" not in (Select ""APPOINTMENT_ID"" from ""VACCINATION_EVENT"" where (""VACCINATION_STATUS"" = true or ""NONVACCINATION_REASON_ID"" <> 0)) ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<VaccinationAppointment> GetVaccinationAppointmentsByChild(int childId)
        {
            try
            {
                string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = " + childId + @";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationAppointment> GetVaccinationAppointmentsByChild(DateTime modifiedOn, int userId)
        {
            try
            {
                User user = User.GetUserById(userId);
                //string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = @ChildId AND VA.""MODIFIEDON"" >= @ModifiedOn AND VA.""MODIFIED_BY"" <> @UserId ;";
                string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" in (Select ""ID"" from ""CHILD"" where ""HEALTHCENTER_ID"" = @hfId ) AND VA.""MODIFIEDON"" >= @ModifiedOn  AND VA.""MODIFIED_BY"" <> @UserId;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@ChildId", DbType.Int32) { Value = childId },
                    new NpgsqlParameter("@ModifiedOn", DbType.DateTime) { Value = modifiedOn },
                     new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId.ToString()},
                       new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationAppointment> GetVaccinationAppointmentsByChild(string childIdList, DateTime modifiedOn, int userId)
        {
            try
            {
                //string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = @ChildId AND VA.""MODIFIEDON"" >= @ModifiedOn AND VA.""MODIFIED_BY"" <> @UserId ;";
                string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = ANY( CAST( string_to_array(@ChildList, ',' ) AS INTEGER[] )) AND VA.""MODIFIEDON"" >= @ModifiedOn  AND VA.""MODIFIED_BY"" <> @UserId;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                   new NpgsqlParameter("@ChildList", DbType.String) { Value = childIdList },
                    new NpgsqlParameter("@ModifiedOn", DbType.DateTime) { Value = modifiedOn },
                       new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationAppointment> GetVaccinationAppointmentsByChildBefore(DateTime modifiedOn, DateTime prevlogin, int userId)
        {
            try
            {
                User user = User.GetUserById(userId);
                //string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = @ChildId AND VA.""MODIFIEDON"" < @ModifiedOn  AND VA.""MODIFIED_BY"" <> @UserId ;";
                string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" in (Select ""ID"" from ""CHILD"" where ""HEALTHCENTER_ID"" = @hfId ) AND VA.""MODIFIEDON"" < @ModifiedOn AND ""MODIFIEDON"" > @PrevLogin AND VA.""MODIFIED_BY"" <> @UserId;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@ChildId", DbType.Int32) { Value = childId },
                    new NpgsqlParameter("@ModifiedOn", DbType.DateTime) { Value = modifiedOn },
                      new NpgsqlParameter("@PrevLogin", DbType.DateTime) { Value = prevlogin },
                     new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId.ToString()},
                     new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationAppointment> GetVaccinationAppointmentsByChildBefore(string childIdList, DateTime modifiedOn, DateTime prevlogin, int userId)
        {
            try
            {

                //string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = @ChildId AND VA.""MODIFIEDON"" < @ModifiedOn  AND VA.""MODIFIED_BY"" <> @UserId ;";
                string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = ANY( CAST( string_to_array(@ChildList, ',' ) AS INTEGER[] )) AND VA.""MODIFIEDON"" < @ModifiedOn AND ""MODIFIEDON"" > @PrevLogin AND VA.""MODIFIED_BY"" <> @UserId;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ChildList", DbType.String) { Value = childIdList },
                     new NpgsqlParameter("@PrevLogin", DbType.DateTime) { Value = prevlogin },
                    new NpgsqlParameter("@ModifiedOn", DbType.DateTime) { Value = modifiedOn },
                     new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<VaccinationAppointment> GetVaccinationAppointmentsByChildDayFirstLogin(DateTime modifiedOn, int userId)
        {
            try
            {
                User user = User.GetUserById(userId);
                //string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" = @ChildId AND VA.""MODIFIEDON""::date = @ModifiedOn::date  ;";
                string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" in (Select ""ID"" from ""CHILD"" where ""HEALTHCENTER_ID"" = @hfId ) AND VA.""MODIFIEDON""::date = @ModifiedOn::date ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@ChildId", DbType.Int32) { Value = childId },
                   new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId.ToString()},
                     new NpgsqlParameter("@ModifiedOn", DbType.DateTime) { Value = user.Lastlogin.AddDays(-1)}
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<VaccinationAppointment> GetVaccinationAppointmentsByHealthFacility(int hfId)
        {
            try
            {
                int year = DateTime.Today.Date.Year;
                string query = @"SELECT VA.* FROM ""VACCINATION_APPOINTMENT"" VA WHERE VA.""CHILD_ID"" in (Select ""ID"" from ""CHILD"" where ""HEALTHCENTER_ID"" = @hfId AND EXTRACT(YEAR from ""BIRTHDATE"") = @year );";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@hfId", DbType.Int32) { Value = hfId },
                    new NpgsqlParameter("@year", DbType.Int32) {Value = year}

                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationAppointmentAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "GetVaccinationAppointmentsByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int DeleteByChild(int id)
        {
            try
            {
                string query = @"DELETE FROM ""VACCINATION_APPOINTMENT"" WHERE ""CHILD_ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "DeleteByChild", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
      
        public static int InsertVaccinationsForChild(int childId, int userId)
        {
            try
            {
                Child child = Child.GetChildById(childId);

                List<Dose> vaccineDoseList = Dose.GetDosesByDates(child.Birthdate);

                foreach (Dose vaccineDose in vaccineDoseList)
                {
                    VaccinationAppointment o = new VaccinationAppointment();
                    o.ChildId = childId;
                    o.ScheduledFacilityId = child.HealthcenterId;
                    o.ScheduledDate = child.Birthdate.AddDays(vaccineDose.AgeDefinition.Days);
                    o.Notes = String.Empty;
                    o.IsActive = true;
                    o.ModifiedOn = DateTime.Now;
                    o.ModifiedBy = userId;

                    string where = string.Format(" \"CHILD_ID\" = {0} AND \"SCHEDULED_DATE\" = '{1}' ", child.Id, o.ScheduledDate.ToString("yyyy-MM-dd"));
                    List<VaccinationAppointment> list = GetVaccinationAppointmentForList(where);
                    int count = list.Count;

                    if (count == 0)
                    {
                        int lastApp = Insert(o);
                        if (lastApp > 0)
                        {
                            VaccinationEvent ve = new VaccinationEvent();

                            ve.AppointmentId = lastApp;
                            ve.ChildId = childId;
                            ve.DoseId = vaccineDose.Id;
                            ve.HealthFacilityId = child.HealthcenterId;
                            ve.ScheduledDate = o.ScheduledDate;
                            ve.VaccinationDate = o.ScheduledDate;
                            ve.Notes = String.Empty;
                            ve.VaccinationStatus = false;
                            int dosenum = vaccineDose.DoseNumber - 1;

                            if (VaccinationEvent.OtherDose(vaccineDose.ScheduledVaccinationId, childId, dosenum) && dosenum > 0)
                                ve.IsActive = false;
                            else
                                ve.IsActive = true;
                            ve.ModifiedOn = DateTime.Now;
                            ve.ModifiedBy = userId;
                            int i = VaccinationEvent.Insert(ve);
                            if (!(i > 0))
                            {
                                VaccinationAppointment.DeleteByChild(childId);
                                Child.Delete(childId);
                                return 0;
                            }
                        }
                        else
                        {
                            Child.Delete(childId);
                            return 0;
                        }
                    }
                    else
                    {
                        VaccinationAppointment appointment = list[0];
                        VaccinationEvent ve = new VaccinationEvent();

                        ve.AppointmentId = appointment.Id;
                        ve.ChildId = childId;
                        ve.DoseId = vaccineDose.Id;
                        ve.HealthFacilityId = child.HealthcenterId;
                        ve.ScheduledDate = o.ScheduledDate;
                        ve.VaccinationDate = o.ScheduledDate;
                        ve.Notes = String.Empty;
                        ve.VaccinationStatus = false;
                        int dosenum = vaccineDose.DoseNumber - 1;

                        if (VaccinationEvent.OtherDose(vaccineDose.ScheduledVaccinationId, childId, dosenum) && dosenum > 0)
                            ve.IsActive = false;
                        else
                            ve.IsActive = true;
                        ve.ModifiedOn = DateTime.Now;
                        ve.ModifiedBy = userId;
                        int i = VaccinationEvent.Insert(ve);
                        if (!(i > 0))
                        {
                            VaccinationAppointment.DeleteByChild(childId);
                            Child.Delete(childId);
                            return 0;
                        }
                    }
                }
                return 1;

            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "InsertVaccinationsForChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }

           // return 0;
        }

        public static int Update(int healthfacilityId, int id)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_APPOINTMENT"" SET ""SCHEDULED_FACILITY_ID"" = " + healthfacilityId + @" WHERE ""ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                Log.InsertEntity(2, "Success", id.ToString(), "VaccinationAppointment", "Update");
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }
        public static int Update(DateTime scheduledDate, int id)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_APPOINTMENT"" SET ""SCHEDULED_DATE"" = '" + scheduledDate.ToString("yyyy-MM-dd") + @"' WHERE ""ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                Log.InsertEntity(2, "Success", id.ToString(), "VaccinationAppointment", "Update");
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }
        public static int Update(bool status, int id)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_APPOINTMENT"" SET ""IS_ACTIVE"" = '" + status + @"' WHERE ""ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                Log.InsertEntity(2, "Success", id.ToString(), "VaccinationAppointment", "Update");
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationAppointment", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }
    }
}
