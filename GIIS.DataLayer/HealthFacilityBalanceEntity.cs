using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class HealthFacilityBalance
    {
        public static List<HealthFacilityBalance> GetHealthFacilityBalanceByHealthFacility(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"SELECT * FROM ""HEALTH_FACILITY_BALANCE"" join ""ITEM_MANUFACTURER"" using (""GTIN"") join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID"" join ""ITEM_LOT"" using (""GTIN"", ""LOT_NUMBER"") WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue and ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true and ""ITEM_LOT"".""IS_ACTIVE"" = true  ORDER BY ""ITEM"".""CODE"" ";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                            };
                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

                    return GetHealthFacilityBalanceAsList(dt);
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountHealthFacilityBalanceByHealthFacility(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"SELECT count(*) FROM ""HEALTH_FACILITY_BALANCE"" WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue ";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                            {
                            new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                            };

                    object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                    return int.Parse(count.ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemManufacturerBalanceForDropDown(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"Select  '-----' as ""GTIN"", '------' as ""VALUE"", '------' as ""CODE""  UNION  SELECT DISTINCT (""HEALTH_FACILITY_BALANCE"".""GTIN"" || ' - ' || ""ITEM"".""CODE"") as ""GTIN"", ""HEALTH_FACILITY_BALANCE"".""GTIN"" AS ""VALUE"", ""ITEM"".""CODE"" as ""CODE""  FROM ""ITEM_MANUFACTURER"" join ""HEALTH_FACILITY_BALANCE"" using (""GTIN"") join ""ITEM"" on ""ITEM_MANUFACTURER"".""ITEM_ID"" = ""ITEM"".""ID"" WHERE  ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true AND  ""HEALTH_FACILITY_CODE"" = @ParamValue and (""GTIN_PARENT"" = '' or ""GTIN_PARENT"" is null) ORDER BY ""CODE"" ;";
                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                    };

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityEntity", "GetItemManufacturerBalanceForDropDown", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetHealthFacilityBalanceForList(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"Select '-----' as ""GTIN"" UNION  SELECT DISTINCT ""GTIN"" FROM ""HEALTH_FACILITY_BALANCE"" join ""ITEM_MANUFACTURER"" using (""GTIN"") 
                                    WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue and ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true ";

                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@ParamValue", DbType.String) { Value = hf.Code }
                    };

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public DataTable GetChartData(int id)
        {
            try
            {
                HealthFacility hf = HealthFacility.GetHealthFacilityById(id);
                if (hf != null)
                {
                    string query = @"SELECT VHFB.""NAME"", sum ""BALANCE"", ""SAFETY_STOCK"" FROM ""V_HEALTH_FACILITY_BALANCE_HELPER"" VHFB
                                        LEFT JOIN ""V_GTIN_HF_STOCK_POLICY_HELPER"" VSP ON VHFB.""NAME"" = VSP.""NAME""
                                        WHERE VHFB.""HEALTH_FACILITY_CODE"" = @hfCode AND 
                                        VSP.""HEALTH_FACILITY_CODE"" = @hfCode
                                        order by 1 asc";


                    List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@hfCode", DbType.String) { Value = hf.Code }
                    };

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public DataTable GetCoverageChart(int hfId)
        {
            try
            {
                string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);
                if (!string.IsNullOrEmpty(s))
                {
                    string query = @"with tmp as
(
SELECT SV.""NAME"" as ""Name"", date_part('month', ""SCHEDULED_DATE"") as ""Month"", date_part('year', ""SCHEDULED_DATE"")  ""Year"", COUNT(*) as ""Count"", 'Scheduled' as ""Type""

FROM ""VACCINATION_EVENT"" VE

INNER JOIN ""DOSE"" D ON VE.""DOSE_ID"" = D.""ID"" 
INNER JOIN ""SCHEDULED_VACCINATION"" SV ON SV.""ID"" = D.""SCHEDULED_VACCINATION_ID""

WHERE ""SCHEDULED_DATE"" BETWEEN (date_trunc('month', now()) - interval '3 month') AND (date_trunc('month', now())::date - 1)
AND ""HEALTH_FACILITY_ID"" IN (" + s + @")

GROUP BY SV.""NAME"", date_part('month', ""SCHEDULED_DATE""), date_part('year', ""SCHEDULED_DATE"")

union all

SELECT SV.""NAME"", date_part('month', ""VACCINATION_DATE""), date_part('year', ""VACCINATION_DATE""), COUNT(*), 'Done'

FROM ""VACCINATION_EVENT"" VE

INNER JOIN ""DOSE"" D ON VE.""DOSE_ID"" = D.""ID"" 
INNER JOIN ""SCHEDULED_VACCINATION"" SV ON SV.""ID"" = D.""SCHEDULED_VACCINATION_ID""

WHERE ""VACCINATION_DATE"" BETWEEN (date_trunc('month', now()) - interval '3 month') AND (date_trunc('month', now())::date - 1)
AND ""VACCINATION_STATUS"" = true
AND ""HEALTH_FACILITY_ID"" IN (" + s + @")

GROUP BY SV.""NAME"", date_part('month', ""VACCINATION_DATE""), date_part('year', ""VACCINATION_DATE"")
)

select t1.""Name"", t1.""Month"", t1.""Year"", trunc((t2.""Count"" / t1.""Count""::float * 100)::numeric, 2) as ""Percentage""
from tmp t1 left outer join tmp t2
on t1.""Name"" = t2.""Name"" and t1.""Month"" = t2.""Month"" and t1.""Year"" = t2.""Year"" and t1.""Type"" <> t2.""Type""
where t1.""Type"" = 'Scheduled'

order by t1.""Name"", t1.""Year"", t1.""Month""; ";


                    //List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    //{
                    //    new NpgsqlParameter("@hfid_1", DbType.String) { Value = s },
                    //     new NpgsqlParameter("@hfid_2", DbType.String) { Value = s }
                    //};

                    DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                    return dt;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}