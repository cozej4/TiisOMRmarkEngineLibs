using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using System.Diagnostics;

namespace GIIS.DataLayer
{
    public partial class GtinHfStockPolicy
    {

        #region Properties
        public string HealthFacilityCode { get; set; }
        public string Gtin { get; set; }
        public double ReorderQty { get; set; }
        public double SafetyStock { get; set; }
        public double AvgDailyDemandRate { get; set; }
        public string ConsumptionLogic { get; set; }
        public Int32 LeadTime { get; set; }
        public double ForecastPeriodDemand { get; set; }
        public string Notes { get; set; }
        public ItemManufacturer GtinObject
        {
            get
            {
                if (this.Gtin == "")
                    return ItemManufacturer.GetItemManufacturerByGtin(this.Gtin);
                else
                    return null;
            }
        }
        public HealthFacility HealthFacilityCodeObject
        {
            get
            {
                if (this.HealthFacilityCode != "")
                    return HealthFacility.GetHealthFacilityByCode(this.HealthFacilityCode);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<GtinHfStockPolicy> GetGtinHfStockPolicyList()
        {
            try
            {
                string query = @"SELECT * FROM ""GTIN_HF_STOCK_POLICY"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetGtinHfStockPolicyAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetGtinHfStockPolicyForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""GTIN_HF_STOCK_POLICY"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static GtinHfStockPolicy GetGtinHfStockPolicyByHealthFacilityCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""GTIN_HF_STOCK_POLICY"" WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("GtinHfStockPolicy", s, 4, DateTime.Now, 1);
                return GetGtinHfStockPolicyAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyByHealthFacilityCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static GtinHfStockPolicy GetGtinHfStockPolicyByGtin(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""GTIN_HF_STOCK_POLICY"" WHERE ""GTIN"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("GtinHfStockPolicy", s, 4, DateTime.Now, 1);
                return GetGtinHfStockPolicyAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyByGtin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(GtinHfStockPolicy o)
        {
            try
            {
                string query = @"INSERT INTO ""GTIN_HF_STOCK_POLICY"" (""HEALTH_FACILITY_CODE"", ""GTIN"", ""REORDER_QTY"", ""SAFETY_STOCK"", ""AVG_DAILY_DEMAND_RATE"", ""CONSUMPTION_LOGIC"", ""LEAD_TIME"", ""FORECAST_PERIOD_DEMAND"", ""NOTES"") VALUES (@HealthFacilityCode, @Gtin, @ReorderQty, @SafetyStock, @AvgDailyDemandRate, @ConsumptionLogic, @LeadTime, @ForecastPeriodDemand, @Notes) returning ""HEALTH_FACILITY_CODE"" || ""GTIN"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@HealthFacilityCode", DbType.String)  { Value = o.HealthFacilityCode },
                new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
                new NpgsqlParameter("@ReorderQty", DbType.Double)  { Value = (object)o.ReorderQty ?? DBNull.Value },
                new NpgsqlParameter("@SafetyStock", DbType.Double)  { Value = (object)o.SafetyStock ?? DBNull.Value },
                new NpgsqlParameter("@AvgDailyDemandRate", DbType.Double)  { Value = (object)o.AvgDailyDemandRate ?? DBNull.Value },
                new NpgsqlParameter("@ConsumptionLogic", DbType.String)  { Value = (object)o.ConsumptionLogic ?? DBNull.Value },
                new NpgsqlParameter("@LeadTime", DbType.Int32)  { Value = (object)o.LeadTime ?? DBNull.Value },
                new NpgsqlParameter("@ForecastPeriodDemand", DbType.Double)  { Value = (object)o.ForecastPeriodDemand ?? DBNull.Value },
                new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }
                };

                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("GtinHfStockPolicy", id.ToString(), 1, DateTime.Now, 1);
                return 1;// int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(GtinHfStockPolicy o)
        {
            try
            {
                string query = @"UPDATE ""GTIN_HF_STOCK_POLICY"" SET ""REORDER_QTY"" = @ReorderQty, ""SAFETY_STOCK"" = @SafetyStock, ""AVG_DAILY_DEMAND_RATE"" = @AvgDailyDemandRate, ""CONSUMPTION_LOGIC"" = @ConsumptionLogic, ""LEAD_TIME"" = @LeadTime, ""FORECAST_PERIOD_DEMAND"" = @ForecastPeriodDemand, ""NOTES"" = @Notes WHERE ""HEALTH_FACILITY_CODE"" = @HealthFacilityCode and ""GTIN"" = @Gtin ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@HealthFacilityCode", DbType.String)  { Value = o.HealthFacilityCode },
                new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
                new NpgsqlParameter("@ReorderQty", DbType.Double)  { Value = (object)o.ReorderQty ?? DBNull.Value },
                new NpgsqlParameter("@SafetyStock", DbType.Double)  { Value = (object)o.SafetyStock ?? DBNull.Value },
                new NpgsqlParameter("@AvgDailyDemandRate", DbType.Double)  { Value = (object)o.AvgDailyDemandRate ?? DBNull.Value },
                new NpgsqlParameter("@ConsumptionLogic", DbType.String)  { Value = (object)o.ConsumptionLogic ?? DBNull.Value },
                new NpgsqlParameter("@LeadTime", DbType.Int32)  { Value = (object)o.LeadTime ?? DBNull.Value },
                new NpgsqlParameter("@ForecastPeriodDemand", DbType.Double)  { Value = (object)o.ForecastPeriodDemand ?? DBNull.Value },
                new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
                new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin }
                };

                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("GtinHfStockPolicy", string.Format("RecordId: {0}", o.Gtin), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(string healthFacilityCode, string gtin)
        {
            try
            {
                string query = @"DELETE FROM ""GTIN_HF_STOCK_POLICY"" WHERE ""HEALTH_FACILITY_CODE"" = @HealthFacilityCode AND  ""GTIN"" = @Gtin";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@HealthFacilityCode", DbType.String) { Value = healthFacilityCode },
                    new NpgsqlParameter("@Gtin", DbType.String) { Value = gtin }
                };

                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("GtinHfStockPolicy", string.Format("RecordId: {0}", healthFacilityCode + gtin), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static GtinHfStockPolicy GetGtinHfStockPolicyAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    GtinHfStockPolicy o = new GtinHfStockPolicy();
                    o.HealthFacilityCode = row["HEALTH_FACILITY_CODE"].ToString();
                    o.Gtin = row["GTIN"].ToString();
                    o.ReorderQty = Helper.ConvertToDecimal(row["REORDER_QTY"]);
                    o.SafetyStock = Helper.ConvertToDecimal(row["SAFETY_STOCK"]);
                    o.AvgDailyDemandRate = Helper.ConvertToDecimal(row["AVG_DAILY_DEMAND_RATE"]);
                    o.ConsumptionLogic = row["CONSUMPTION_LOGIC"].ToString();
                    o.LeadTime = Helper.ConvertToInt(row["LEAD_TIME"]);
                    o.ForecastPeriodDemand = Helper.ConvertToDecimal(row["FORECAST_PERIOD_DEMAND"]);
                    o.Notes = row["NOTES"].ToString();
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<GtinHfStockPolicy> GetGtinHfStockPolicyAsList(DataTable dt)
        {
            List<GtinHfStockPolicy> oList = new List<GtinHfStockPolicy>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    GtinHfStockPolicy o = new GtinHfStockPolicy();
                    o.HealthFacilityCode = row["HEALTH_FACILITY_CODE"].ToString();
                    o.Gtin = row["GTIN"].ToString();
                    o.ReorderQty = Helper.ConvertToDecimal(row["REORDER_QTY"]);
                    o.SafetyStock = Helper.ConvertToDecimal(row["SAFETY_STOCK"]);
                    o.AvgDailyDemandRate = Helper.ConvertToDecimal(row["AVG_DAILY_DEMAND_RATE"]);
                    o.ConsumptionLogic = row["CONSUMPTION_LOGIC"].ToString();
                    o.LeadTime = Helper.ConvertToInt(row["LEAD_TIME"]);
                    o.ForecastPeriodDemand = Helper.ConvertToDecimal(row["FORECAST_PERIOD_DEMAND"]);
                    o.Notes = row["NOTES"].ToString();
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
