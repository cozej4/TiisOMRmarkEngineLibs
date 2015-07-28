using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GIIS.DataLayer
{
    public partial class Child
    {
        public static Child GetChildByBarcode(string barcodeId)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD"" WHERE UPPER(""BARCODE_ID"") = @BarcodeId;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@BarcodeId", DbType.String) { Value = barcodeId.ToUpper() }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByBarcode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Child GetChildByTempId(string tempId)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD"" WHERE UPPER(""TEMP_ID"") = @TempId;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@TempId", DbType.String) { Value = tempId.ToUpper() }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByTempId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Child GetPersonByLastnameBirthdate(string lastName, DateTime dtime)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""CHILD"" WHERE ""LASTNAME1"" = '{0}' AND ""BIRTHDATE"" = '{1}' ", lastName, dtime.ToString("yyyy-MM-dd"));
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetPersonByLasnameBirthdateGender", 1, ex.StackTrace, ex.Message);
                throw ex;
            }
        }
        public static Child GetPersonByLastnameBirthdateGender(string lastName, DateTime dtime, bool gender)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""CHILD"" WHERE ""LASTNAME1"" = '{0}' AND ""BIRTHDATE"" = '{1}' AND ""GENDER"" = '{2}' ", lastName, dtime.ToString("yyyy-MM-dd"), gender);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetPersonByLasnameBirthdateGender", 1, ex.StackTrace, ex.Message);
                throw ex;
            }
        }
        public static Child GetPersonIdentification1(string id)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""CHILD"" WHERE ""IDENTIFICATION_NO1"" = '{0}' ", id);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetPersonIdentification1", 1, ex.StackTrace, ex.Message);
                throw ex;
            }
        }
        public static Child GetPersonIdentification2(string id)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""CHILD"" WHERE ""IDENTIFICATION_NO2"" = '{0}' ", id);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetPersonIdentification2", 1, ex.StackTrace, ex.Message);
                throw ex;
            }
        }
        public static Child GetPersonIdentification3(string id)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""CHILD"" WHERE ""IDENTIFICATION_NO3"" = '{0}' ", id);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetPersonIdentification3", 1, ex.StackTrace, ex.Message);
                throw ex;
            }
        }
        public static int GetLastChildHealthFacilityByYear(int healthfacility, int year)
        {
            try
            {
                string query = string.Format(@"SELECT MAX(""ID"") FROM ""CHILD"" WHERE ""HEALTHCENTER_ID"" = '{0}' AND EXTRACT(YEAR FROM ""BIRTHDATE"") = {1} ", healthfacility, year);
                object i = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                if (!(i is DBNull))
                    return int.Parse(i.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetLastChildHealthFacilityByYear", 1, ex.StackTrace, ex.Message);
                throw ex;
            }
        }

        public static List<Child> GetImmunizedChildrenByLot(ref int maximumRows, ref int startRowIndex, int hfId, int itemlotid)
        {
            try
            {
                string query = String.Format(@"SELECT ""CHILD"".* FROM ""CHILD"" inner join ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" 
                                            WHERE ""VACCINE_LOT_ID"" = {1} AND (""HEALTH_FACILITY_ID"" = {0} OR ""HEALTH_FACILITY_ID"" in (SELECT ""ID"" from ""HEALTH_FACILITY"" where ""PARENT_ID"" = {0})) ORDER BY ""BIRTHDATE"",""LASTNAME1"" OFFSET {2} LIMIT {3}", hfId, itemlotid, startRowIndex, maximumRows);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetImmunizedChildrenByLot", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountImmunizedChildrenByLot(int hfId, int itemlotid)
        {
            try
            {
                string query = String.Format(@"SELECT COUNT(*) FROM ""CHILD"" inner join ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" 
                                            WHERE ""VACCINE_LOT_ID"" = {1} AND (""HEALTH_FACILITY_ID"" = {0} OR ""HEALTH_FACILITY_ID"" in (SELECT ""ID"" from ""HEALTH_FACILITY"" where ""PARENT_ID"" = {0})) ", hfId, itemlotid);

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetCountImmunizedChildrenByLot", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Child> GetDuplications(bool birthdateFlag, bool firstnameFlag, bool genderFlag, int healthFacilityId)
        {
            try
            {
                if (birthdateFlag || firstnameFlag || genderFlag)
                {
                    string query = String.Format(@"SELECT DISTINCT C1.* from ""CHILD"" C1, ""CHILD"" C2 where C1.""ID"" <> C2.""ID"" AND 
                                            C1.""STATUS_ID"" = 1 AND C2.""STATUS_ID"" = 1 ");

                    query += String.Format(@" AND C1.""LASTNAME1"" = C2.""LASTNAME1"" ");
                    if (firstnameFlag)
                        query += String.Format(@" AND C1.""FIRSTNAME1"" = C2.""FIRSTNAME1"" ");
                    if (birthdateFlag)
                        query += String.Format(@" AND C1.""BIRTHDATE"" = C2.""BIRTHDATE"" ");
                    if (genderFlag)
                        query += String.Format(@" AND C1.""GENDER"" = C2.""GENDER"" ");

                    HealthFacility hf = HealthFacility.GetHealthFacilityById(healthFacilityId);
                    string s = "";
                    if (!hf.TopLevel)
                    {
                        s = HealthFacility.GetAllChildsForOneHealthFacility(hf.Id);
                        query += string.Format(@" AND (C1.""HEALTHCENTER_ID"" in (" + s + @") OR C2.""HEALTHCENTER_ID"" in (" + s + ")) ");
                    }

                    query += String.Format(@" ORDER BY C1.""LASTNAME1"" ");


                //    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                //{
                //    new NpgsqlParameter("@HealthCenterId", DbType.String) { Value = s }
                //};

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                    return GetChildAsList(dt);
                }
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetDuplications", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }

            return null;
        }

        public static List<Child> GetNotImmunizedChildren(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = String.Format(@"SELECT ""CHILD"".""ID"", ""SYSTEM_ID"", ""FIRSTNAME1"", ""FIRSTNAME2"", ""LASTNAME1"", ""LASTNAME2"", 
                               ""BIRTHDATE"", ""GENDER"", ""HEALTHCENTER_ID"", ""BIRTHPLACE_ID"", ""COMMUNITY_ID"", 
                               ""DOMICILE_ID"", ""STATUS_ID"", ""ADDRESS"", ""PHONE"", ""MOBILE"", ""EMAIL"", 
                               ""MOTHER_ID"", ""MOTHER_FIRSTNAME"", ""MOTHER_LASTNAME"", ""FATHER_ID"", 
                               ""FATHER_FIRSTNAME"", ""FATHER_LASTNAME"", ""CARETAKER_ID"", ""CARETAKER_FIRSTNAME"", 
                               ""CARETAKER_LASTNAME"", ""NONVACCINATION_REASON"".""NAME"" as ""NOTES"", ""CHILD"".""IS_ACTIVE"", ""CHILD"".""MODIFIED_ON"", ""CHILD"".""MODIFIED_BY"", 
                               ""IDENTIFICATION_NO1"", ""IDENTIFICATION_NO2"", ""IDENTIFICATION_NO3"", ""BARCODE_ID"", ""TEMP_ID""
                                FROM ""CHILD"" inner join ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" LEFT JOIN ""NONVACCINATION_REASON"" ON ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = ""NONVACCINATION_REASON"".""ID""
                                WHERE ""CHILD"".""STATUS_ID"" = 1 and ""VACCINATION_STATUS"" = false {0}  ORDER BY ""BIRTHDATE"",""LASTNAME1"" OFFSET {1} LIMIT {2}", where, startRowIndex, maximumRows);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetNotImmunizedChildren", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountNotImmunizedChildren(string where)
        {
            try
            {
                string query = String.Format(@"SELECT COUNT(*) FROM ""CHILD"" inner join ""VACCINATION_EVENT"" on ""CHILD"".""ID"" = ""VACCINATION_EVENT"".""CHILD_ID"" LEFT JOIN ""NONVACCINATION_REASON"" ON ""VACCINATION_EVENT"".""NONVACCINATION_REASON_ID"" = ""NONVACCINATION_REASON"".""ID""
                                            WHERE ""CHILD"".""STATUS_ID"" = 1 and ""VACCINATION_STATUS"" = false {0} ", where);

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetCountNotImmunizedChildren", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static List<Child> GetChildListforSMSNotification(int healthcenterId, int currentMonth, int currentYear)
        {
            try
            {
                if (healthcenterId != 0)
                {
                    List<Child> child = new List<Child>();
                    string query = string.Format("-- Get due data: vaccines of previous months that have status false: are not done " + Environment.NewLine + "-------------------------------------------------------------------------------------- " + Environment.NewLine + "SELECT DISTINCT \"VACCINATION_EVENT\".\"CHILD_ID\" FROM \"VACCINATION_APPOINTMENT\", \"VACCINATION_EVENT\" " + Environment.NewLine + "WHERE  \"VACCINATION_APPOINTMENT\".\"ID\" = \"VACCINATION_EVENT\".\"APPOINTMENT_ID\" " + Environment.NewLine + " AND  (\"VACCINATION_EVENT\".\"VACCINATION_STATUS\" = False) AND \"VACCINATION_APPOINTMENT\".\"IS_ACTIVE\" = True " + Environment.NewLine + " AND  ((EXTRACT(MONTH FROM \"VACCINATION_EVENT\".\"SCHEDULED_DATE\") < {0}   -- current month " + Environment.NewLine + " AND  EXTRACT(YEAR FROM \"VACCINATION_EVENT\".\"SCHEDULED_DATE\") = {1}) OR  EXTRACT(YEAR FROM \"VACCINATION_EVENT\".\"SCHEDULED_DATE\") < {1}) -- current year  " + Environment.NewLine + " AND \"VACCINATION_EVENT\".\"IS_ACTIVE\" = true AND \"VACCINATION_EVENT\".\"CHILD_ID\" in (SELECT \"ID\" from \"CHILD\" where \"STATUS_ID\" in (1,6) AND \"IS_ACTIVE\" = true) AND \"SCHEDULED_FACILITY_ID\" = {2} " + Environment.NewLine + "-------------------------------------------------------------------------------------- " + Environment.NewLine + "ORDER BY \"VACCINATION_EVENT\".\"CHILD_ID\" ", currentMonth, currentYear, healthcenterId);

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);

                    foreach (DataRow dr in dt.Rows)
                    {
                        Child objChild = new Child();
                        objChild = GetChildById(Convert.ToInt16(dr["CHILD_ID"]));
                        child.Add(objChild);
                    }
                    return child;
                }
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetMonthlyPlanChildListforNotification", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
            return null;
        }

        public static List<Child> GetChildByHealthFacilityId(int healthFacilityId)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD"" WHERE ""HEALTHCENTER_ID"" = @HealthFacilityId";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@HealthFacilityId", DbType.Int32) { Value = healthFacilityId.ToString() }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Child> GetChildByHealthFacilityIdSinceLastLogin(int userId)
        {
            try
            {
                User user = User.GetUserById(userId);
                string query = string.Format(@"select distinct C.* from ""CHILD"" C join ""VACCINATION_EVENT"" V on V.""CHILD_ID"" = C.""ID"" 
                                                WHERE C.""HEALTHCENTER_ID"" = @hfId 
                                                AND ((C.""MODIFIED_ON"" >= @lastlogin
                                                AND C.""MODIFIED_BY"" <> @UserId) OR  (""MODIFIEDON"" >= @lastlogin  AND V.""MODIFIED_BY"" <> @UserId)) ");

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    
                     new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId.ToString()},
                     new NpgsqlParameter("@lastlogin", DbType.DateTime) { Value = user.Lastlogin},
                    new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId.ToString() }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByHealthFacilityIdSinceLastLogin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Child> GetChildByHealthFacilityIdBeforeLastLogin(int userId)
        {

            try
            {
                User user = User.GetUserById(userId);
                string query = string.Format(@"select distinct C.* from ""CHILD"" C join ""VACCINATION_EVENT"" V on V.""CHILD_ID"" = C.""ID""
                                                WHERE C.""HEALTHCENTER_ID"" = @hfId 
                                                AND ((C.""MODIFIED_ON"" < @lastlogin AND C.""MODIFIED_ON"" > @PrevLogin
                                                AND C.""MODIFIED_BY"" <> @UserId) OR (""MODIFIEDON"" < @lastlogin AND ""MODIFIEDON"" > @PrevLogin  AND V.""MODIFIED_BY"" <> @UserId)) ");

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    
                     new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId.ToString()},
                     new NpgsqlParameter("@lastlogin", DbType.DateTime) { Value = user.Lastlogin},
                     new NpgsqlParameter("@PrevLogin", DbType.DateTime) { Value = user.PrevLogin },
                    new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId.ToString() }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByHealthFacilityIdBeforeLastLogin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Child> GetChildByHealthFacilityIdDayFirstLogin(int userId)
        {

            try
            {
                User user = User.GetUserById(userId);
                string query = string.Format(@"select distinct C.* from ""CHILD"" C join ""VACCINATION_EVENT"" V on V.""CHILD_ID"" = C.""ID""
                                                WHERE C.""HEALTHCENTER_ID"" = @hfId 
                                                AND (C.""MODIFIED_ON""::date = @lastlogin::date OR ""MODIFIEDON""::date = @lastlogin::date) ");

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    
                     new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId.ToString()},
                     new NpgsqlParameter("@lastlogin", DbType.DateTime) { Value = user.Lastlogin.AddDays(-1)}
                    
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByHealthFacilityIdBeforeLastLogin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<Child> GetPagedChildList(int statusId, DateTime birthdateFrom, DateTime birthdateTo, string firstname1, string lastname1,
            string idFields, string healthFacilityId, int birthplaceId, int communityId, int domicileId,
            string motherFirstname, string motherLastname, string systemId, string barcodeId, string tempId,
            ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD"" WHERE 1 = 1 "
                                + @" AND ( ""BIRTHDATE"" >= @BirthdateFrom OR @BirthdateFrom is null or @BirthdateFrom = '0001-01-01')"
                                + @" AND ( ""BIRTHDATE"" <= @BirthdateTo OR @BirthdateTo is null or @BirthdateTo = '0001-01-01')"
                                + @" AND ( ""STATUS_ID"" = @StatusId OR @StatusId is null or @StatusId = -1)"
                                + @" AND ( ""BIRTHPLACE_ID"" = @BirthplaceId OR @BirthplaceId is null or @BirthplaceId = 0)"
                                + @" AND ( ""COMMUNITY_ID"" = @CommunityId OR @CommunityId is null or @CommunityId = 0)"
                                + @" AND ( ""DOMICILE_ID"" = @DomicileId OR @DomicileId is null or @DomicileId = 0)"
                                + @" AND (( ""HEALTHCENTER_ID"" = ANY( CAST( string_to_array(@HealthFacilityId, ',' ) AS INTEGER[] ))) or @HealthFacilityId = '')"

                                + @" AND ( UPPER(""FIRSTNAME1"") LIKE @Firstname1 or @Firstname1 is null or @Firstname1 = '%%' )"
                                + @" AND ( UPPER(""LASTNAME1"") LIKE @Lastname1 or @Lastname1 is null or @Lastname1 = '%%' )"

                                + @" AND ( UPPER(""SYSTEM_ID"") LIKE @SystemId or @SystemId is null or @SystemId = '%%' )"
                                + @" AND ( UPPER(""BARCODE_ID"") LIKE @BarcodeId or @BarcodeId is null or @BarcodeId = '%%') "
                                + @" AND ( UPPER(""TEMP_ID"") LIKE @TempId or @TempId is null or @TempId = '%%' )"

                                + @" AND ( UPPER(""MOTHER_FIRSTNAME"") LIKE @MotherFirstname or @MotherFirstname is null or @MotherFirstname = '%%' )"
                                + @" AND ( UPPER(""MOTHER_LASTNAME"") LIKE @MotherLastname or @MotherLastname is null or @MotherLastname = '%%' )"
                                + @" AND ( UPPER(""IDENTIFICATION_NO1"") LIKE @IdFields OR UPPER(""IDENTIFICATION_NO2"") LIKE @IdFields OR UPPER(""IDENTIFICATION_NO3"") LIKE @IdFields or (@IdFields is null ) or @IdFields = '%%' )"

                                + @" ORDER BY ""BIRTHDATE"" DESC,""LASTNAME1"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                if (healthFacilityId == null) { healthFacilityId = ""; };

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@StatusId", DbType.Int32) { Value = statusId },
                    new NpgsqlParameter("@BirthdateFrom", DbType.Date) { Value = birthdateFrom },
                    new NpgsqlParameter("@BirthdateTo", DbType.Date) { Value = birthdateTo },
                    new NpgsqlParameter("@Firstname1", DbType.String) { Value = "%" + firstname1 + "%" },
                    new NpgsqlParameter("@Lastname1", DbType.String) { Value = "%" + lastname1 + "%" },
                    new NpgsqlParameter("@MotherFirstname", DbType.String) { Value = "%" + motherFirstname + "%" },
                    new NpgsqlParameter("@MotherLastname", DbType.String) { Value = "%" + motherLastname + "%" },
                    new NpgsqlParameter("@IdFields", DbType.String) { Value = "%" + idFields + "%" },
                    new NpgsqlParameter("@SystemId", DbType.String) { Value = "%" + systemId + "%" },
                    new NpgsqlParameter("@BarcodeId", DbType.String) { Value = "%" + barcodeId + "%" },
                    new NpgsqlParameter("@TempId", DbType.String) { Value = "%" + tempId + "%" },
                    new NpgsqlParameter("@HealthFacilityId", DbType.String) { Value = healthFacilityId },
                    new NpgsqlParameter("@BirthplaceId", DbType.Int32) { Value = birthplaceId },
                    new NpgsqlParameter("@CommunityId", DbType.Int32) { Value = communityId },
                    new NpgsqlParameter("@DomicileId", DbType.Int32) { Value = domicileId },

                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetPagedChildList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountChildList(int statusId, DateTime birthdateFrom, DateTime birthdateTo, string firstname1, string lastname1,
            string idFields, string healthFacilityId, int birthplaceId, int communityId, int domicileId,
            string motherFirstname, string motherLastname, string systemId, string barcodeId, string tempId)
        {
            try
            {
                string query = @"SELECT count(*) FROM ""CHILD"" WHERE 1 = 1 "
                              + @" AND ( ""BIRTHDATE"" >= @BirthdateFrom OR @BirthdateFrom is null or @BirthdateFrom = '0001-01-01')"
                                + @" AND ( ""BIRTHDATE"" <= @BirthdateTo OR @BirthdateTo is null or @BirthdateTo = '0001-01-01')"
                                + @" AND ( ""STATUS_ID"" = @StatusId OR @StatusId is null or @StatusId = -1)"
                                + @" AND ( ""BIRTHPLACE_ID"" = @BirthplaceId OR @BirthplaceId is null or @BirthplaceId = 0)"
                                + @" AND ( ""COMMUNITY_ID"" = @CommunityId OR @CommunityId is null or @CommunityId = 0)"
                                + @" AND ( ""DOMICILE_ID"" = @DomicileId OR @DomicileId is null or @DomicileId = 0)"
                                + @" AND (( ""HEALTHCENTER_ID"" = ANY( CAST( string_to_array(@HealthFacilityId, ',' ) AS INTEGER[] ))) or @HealthFacilityId = '')"

                                + @" AND ( UPPER(""FIRSTNAME1"") LIKE @Firstname1 or @Firstname1 is null or @Firstname1 = '%%' )"
                                + @" AND ( UPPER(""LASTNAME1"") LIKE @Lastname1 or @Lastname1 is null or @Lastname1 = '%%' )"

                                + @" AND ( UPPER(""SYSTEM_ID"") LIKE @SystemId or @SystemId is null or @SystemId = '%%' )"
                                + @" AND ( UPPER(""BARCODE_ID"") LIKE @BarcodeId or @BarcodeId is null or @BarcodeId = '%%') "
                                + @" AND ( UPPER(""TEMP_ID"") LIKE @TempId or @TempId is null or @TempId = '%%' )"

                                + @" AND ( UPPER(""MOTHER_FIRSTNAME"") LIKE @MotherFirstname or @MotherFirstname is null or @MotherFirstname = '%%' )"
                                + @" AND ( UPPER(""MOTHER_LASTNAME"") LIKE @MotherLastname or @MotherLastname is null or @MotherLastname = '%%' )"
                                + @" AND ( UPPER(""IDENTIFICATION_NO1"") LIKE @IdFields OR UPPER(""IDENTIFICATION_NO2"") LIKE @IdFields OR UPPER(""IDENTIFICATION_NO3"") LIKE @IdFields or (@IdFields is null ) or @IdFields = '%%' )"
                                + @" ;";

                if (healthFacilityId == null) { healthFacilityId = ""; };

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@StatusId", DbType.Int32) { Value = statusId },
                    new NpgsqlParameter("@BirthdateFrom", DbType.Date) { Value = birthdateFrom },
                    new NpgsqlParameter("@BirthdateTo", DbType.Date) { Value = birthdateTo },
                    new NpgsqlParameter("@Firstname1", DbType.String) { Value = "%" + firstname1 + "%" },
                    new NpgsqlParameter("@Lastname1", DbType.String) { Value = "%" + lastname1 + "%" },
                    new NpgsqlParameter("@MotherFirstname", DbType.String) { Value = "%" + motherFirstname + "%" },
                    new NpgsqlParameter("@MotherLastname", DbType.String) { Value = "%" + motherLastname + "%" },
                    new NpgsqlParameter("@IdFields", DbType.String) { Value = "%" + idFields + "%" },
                    new NpgsqlParameter("@SystemId", DbType.String) { Value = "%" + systemId + "%" },
                    new NpgsqlParameter("@BarcodeId", DbType.String) { Value = "%" + barcodeId + "%" },
                    new NpgsqlParameter("@TempId", DbType.String) { Value = "%" + tempId + "%" },
                    new NpgsqlParameter("@HealthFacilityId", DbType.String) { Value = healthFacilityId },
                    new NpgsqlParameter("@BirthplaceId", DbType.Int32) { Value = birthplaceId },
                    new NpgsqlParameter("@CommunityId", DbType.Int32) { Value = communityId },
                    new NpgsqlParameter("@DomicileId", DbType.Int32) { Value = domicileId }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetCountChildList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int ChildExists(string firstname1, string lastname1, string motherFirstname, string motherLastname, DateTime birthdate, bool gender)
        {
            try
            {
                string query = @"SELECT count(*) FROM ""CHILD"" WHERE 1 = 1 "

                                + @" AND ( ""BIRTHDATE"" = @Birthdate OR @Birthdate is null or @Birthdate = '0001-01-01')"
                                + @" AND ( ""GENDER"" = @Gender or @Gender is null )"
                                + @" AND ( UPPER(""FIRSTNAME1"") ILIKE @Firstname1 or @Firstname1 is null or @Firstname1 = '%%' )"
                                + @" AND ( UPPER(""LASTNAME1"") ILIKE @Lastname1 or @Lastname1 is null or @Lastname1 = '%%' )"
                                + @" AND ( UPPER(""MOTHER_FIRSTNAME"") ILIKE @MotherFirstname or @MotherFirstname is null or @MotherFirstname = '%%' )"
                                + @" AND ( UPPER(""MOTHER_LASTNAME"") ILIKE @MotherLastname or @MotherLastname is null or @MotherLastname = '%%' )"
                                + @" ;";


                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Gender", DbType.Boolean) { Value = gender },
                    new NpgsqlParameter("@Birthdate", DbType.Date) { Value = birthdate },
                    new NpgsqlParameter("@Firstname1", DbType.String) { Value = "%" + firstname1 + "%" },
                    new NpgsqlParameter("@Lastname1", DbType.String) { Value = "%" + lastname1 + "%" },
                    new NpgsqlParameter("@MotherFirstname", DbType.String) { Value = "%" + motherFirstname + "%" },
                    new NpgsqlParameter("@MotherLastname", DbType.String) { Value = "%" + motherLastname + "%" }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "ChildExists", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Child> GetChildByIdList(string childIdList, int userId)
        {
           //tring[] childList = childIdList.Split(',');

            try
            {
                User user = User.GetUserById(userId);
                string query = string.Format(@"select distinct C.* from ""CHILD"" C join ""VACCINATION_EVENT"" V on V.""CHILD_ID"" = C.""ID""
                                                WHERE C.""ID"" = ANY( CAST( string_to_array(@ChildList, ',' ) AS INTEGER[] ))
                                                AND ((C.""MODIFIED_ON"" <= @lastlogin AND C.""MODIFIED_ON"" > @PrevLogin
                                                AND C.""MODIFIED_BY"" <> @UserId) 
                                                OR (""MODIFIEDON"" < @lastlogin AND ""MODIFIEDON"" > @PrevLogin  AND V.""MODIFIED_BY"" <> @UserId)) ");

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId.ToString() },
                     new NpgsqlParameter("@lastlogin", DbType.DateTime) { Value = user.Lastlogin},
                     new NpgsqlParameter("@PrevLogin", DbType.DateTime) { Value = user.PrevLogin },
                    new NpgsqlParameter("@ChildList", DbType.String) { Value = childIdList }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByIdList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Child> GetChildByIdListSince(string childIdList, int userId)
        {
            //tring[] childList = childIdList.Split(',');

            try
            {
                User user = User.GetUserById(userId);
                string query = string.Format(@"select distinct C.* from ""CHILD"" C join ""VACCINATION_EVENT"" V on V.""CHILD_ID"" = C.""ID"" 
                                                WHERE C.""ID"" = ANY( CAST( string_to_array(@ChildList, ',' ) AS INTEGER[] ))
                                                AND ((C.""MODIFIED_ON"" >= @lastlogin
                                                AND C.""MODIFIED_BY"" <> @UserId)
                                                OR  (""MODIFIEDON"" >= @lastlogin  AND V.""MODIFIED_BY"" <> @UserId))  ");

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId.ToString() },
                     new NpgsqlParameter("@lastlogin", DbType.DateTime) { Value = user.Lastlogin},
                    new NpgsqlParameter("@ChildList", DbType.String) { Value = childIdList }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildByIdListSince", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
