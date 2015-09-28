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
    partial class Dose
    {
        public static List<Dose> GetDosesByDates(DateTime fromDate)
        {

            try
            {
                string query = string.Format("Select \"DOSE\".* " + " from \"DOSE\",\"SCHEDULED_VACCINATION\" where " + " \"DOSE\".\"SCHEDULED_VACCINATION_ID\" = \"SCHEDULED_VACCINATION\".\"ID\" AND \"ENTRY_DATE\" <= '{0}' AND ((\"EXIT_DATE\" >= '{0}') OR (\"EXIT_DATE\" is NULL) OR (\"EXIT_DATE\" = '0001-01-01')) AND \"DOSE\".\"IS_ACTIVE\" = true  ORDER BY \"DOSE\".\"ID\" ", fromDate.ToString("yyyy-MM-dd"));
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetDoseAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDosesByDates", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Dose GetDoseBySchVaccinationAndDoseNumber(int sch, int dosenr)
        {
            try
            {
                string query = @"SELECT * FROM ""DOSE"" WHERE ""SCHEDULED_VACCINATION_ID"" = " + sch + @" AND ""DOSE_NUMBER"" = " + dosenr + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetDoseAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDoseBySchVaccinationAndDoseNumber", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<Dose> GetDoseByVaccine(int sch)
        {
            try
            {
                string query = @"SELECT * FROM ""DOSE"" WHERE ""SCHEDULED_VACCINATION_ID"" = @vaccineId AND ""IS_ACTIVE"" = true ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@vaccineId", DbType.Int32) { Value = sch }
                   
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetDoseAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDoseByVaccine", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetDoseListForReport()
        {
            try
            {
                string query = String.Format(@"Select -1 as ""ID"", '------' as ""FULLNAME"" UNION SELECT ""ID"", ""FULLNAME"" FROM ""DOSE"" ORDER BY ""ID"" ;");
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDoseListForReport", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Dose> GetPagedDoseList(string fullname, int scheduledVaccinationId, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT * FROM ""DOSE"" WHERE 1 = 1  "
                                  + @" AND ( UPPER(""FULLNAME"") like @Fullname OR @Fullname is null or @Fullname = '')"
                                  + @" AND ""SCHEDULED_VACCINATION_ID"" = @ScheduledVaccinationId OR @ScheduledVaccinationId is null or @ScheduledVaccinationId = -1"
                                  + @" ORDER BY ""SCHEDULED_VACCINATION_ID"", ""FULLNAME"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Fullname", DbType.String) { Value = ("%" + fullname + "%") },
                    new NpgsqlParameter("@ScheduledVaccinationId", DbType.Int32) { Value = scheduledVaccinationId },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetDoseAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetPagedDoseList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountDoseList(string fullname, string scheduledVaccinationId)
        {
            try
            {
                string query = @"SELECT count(*) FROM ""DOSE"" WHERE 1 = 1  "
                                  + @" AND ( UPPER(""FULLNAME"") like @Fullname OR @Fullname is null or @Fullname = '')"
                                  + @" AND ""SCHEDULED_VACCINATION_ID"" = @ScheduledVaccinationId OR @ScheduledVaccinationId is null or @ScheduledVaccinationId = -1;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Fullname", DbType.String) { Value = ("%" + fullname + "%") },
                    new NpgsqlParameter("@ScheduledVaccinationId", DbType.Int32) { Value = scheduledVaccinationId }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetCountDoseList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
