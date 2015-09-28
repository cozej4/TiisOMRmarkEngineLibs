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
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public class Register
    {
        public static DataTable GetRegister(int languageId, string where)
        {
            try
            {
                string funcQueryHelper = string.Format("SELECT create_v_register();");
                DataTable dtfHelper = DBManager.ExecuteReaderCommand(funcQueryHelper, CommandType.Text, null);

                string funcQuery = string.Format("select get_register_query({0});", languageId);
                DataTable dtf = DBManager.ExecuteReaderCommand(funcQuery, CommandType.Text, null);

                string query = "";
                if (dtf.Rows.Count != 0)
                {
                    query = string.Format("{1} Where 1=1 {0};", where, dtf.Rows[0][0].ToString());
                }

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Register", "GetRegister", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }

        public static DataTable GetRegister(int healthFacilityId, string firstname, string lastname, int year, int languageId)
        {
            try
            {
                string funcQueryHelper = string.Format("SELECT create_v_register();");
                DataTable dtfHelper = DBManager.ExecuteReaderCommand(funcQueryHelper, CommandType.Text, null);

                string funcQuery = string.Format("select get_register_query({0});", languageId);
                DataTable dtf = DBManager.ExecuteReaderCommand(funcQuery, CommandType.Text, null);

                string s = HealthFacility.GetAllChildsForOneHealthFacility(healthFacilityId);

                string query = "";
                if (dtf.Rows.Count != 0)
                {
                    query = dtf.Rows[0][0].ToString();
                    query = @"{0} Where 1=1 "
                                + @" AND (( ""ADMINISTRATION_ID"" = ANY( CAST( string_to_array(@AdministrationId, ',' ) AS INTEGER[] ))) or @AdministrationId = '')"
                                + @" AND ( UPPER(""FIRSTNAME"") LIKE @Firstname or @Firstname is null or @Firstname = '%%' )"
                                + @" AND ( UPPER(""LASTNAME"") LIKE @Lastname or @Lastname is null or @Lastname = '%%' )"
                                + @" AND EXTRACT(YEAR FROM to_date(""BIRTHDATE"", get_date_format())) = @Year OR @Year is null or @Year = 0 "
                                + ";";
                }

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@AdministrationId", DbType.String) { Value = s },
                    new NpgsqlParameter("@Firstname", DbType.String) { Value = firstname.Replace("'", @"''").ToUpper() },
                    new NpgsqlParameter("@Lastname", DbType.String) { Value = lastname.Replace("'", @"''").ToUpper() },
                    new NpgsqlParameter("@Year", DbType.Int32) { Value = year }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Register", "GetRegister", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }
    }
}
