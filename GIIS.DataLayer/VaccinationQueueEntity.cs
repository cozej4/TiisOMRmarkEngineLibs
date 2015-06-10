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
