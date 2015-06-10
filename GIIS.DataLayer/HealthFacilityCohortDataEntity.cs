using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GIIS.DataLayer
{
    public partial class HealthFacilityCohortData
    {
        
        public static HealthFacilityCohortData GetHealthFacilityCohortDataByYearAndHealthFacility(int hfId, int year)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE ""YEAR"" = " + year + @" AND ""HEALTH_FACILITY_ID"" = " + hfId + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityCohortDataAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityCohortData", "GetHealthFacilityCohortDataByYear", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacilityCohortData> GetPagedHealthFacilityCohortDataList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE " + where + @" ORDER BY ""YEAR"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityCohortDataAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityCohortData", "GetPagedHealthFacilityCohortDataList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountHealthFacilityCohortDataList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""HEALTH_FACILITY_COHORT_DATA"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityCohortData", "GetCountHealthFacilityCohortDataList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
