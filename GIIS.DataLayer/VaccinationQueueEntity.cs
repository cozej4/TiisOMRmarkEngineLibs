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
using System.Threading.Tasks;

namespace GIIS.DataLayer
{
    public partial class VaccinationQueue
    {
        //GetVaccinationQueue(_date, _userId) - merr te dhenat nga tabela "Vaccination Queue"
        //ku date = _date dhe  healthfacilityId = hf_id te userit dhe userId <> _userId

        public static List<VaccinationQueue> GetVaccinationQueueByDateAndUser(DateTime date, int userId)
        {
            try
            {
                User user = User.GetUserById(userId);
                string query = @"select V.* from ""VACCINATION_QUEUE"" V
                                WHERE V.""HEALTH_FACILITY_ID"" = @hfId
                                AND CAST(V.""DATE"" as date) = @Date
                                AND V.""USER_ID"" <> @UserId";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@UserId", DbType.Int32) { Value = userId },
                    new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId },
                    new NpgsqlParameter("@Date", DbType.Date) { Value = date }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetVaccinationQueueAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "GetVaccinationQueueByDateAndUser", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
