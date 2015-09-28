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
using System.Text;
using System.Data;
using Npgsql;
using System.Diagnostics;

namespace GIIS.DataLayer
{
    public partial class HealthFacilityBalance
    {

        // Policy of the health facility attached to the balance
        private GtinHfStockPolicy m_stockPolicy;

        // Get Average monthly consumption
        public Double AvgMonthlyConsumption
        {
            get
            {
                try
                {

                    string sql = "SELECT SUM(CONSUMPTION)/COUNT(*) AS AMC FROM FACILITY_MONTHLY_CONSUMPTION WHERE \"HEALTH_FACILITY_CODE\" = @hfCode GROUP BY \"GTIN\" HAVING \"GTIN\" = @gtin ";
                    return Convert.ToDouble(DBManager.ExecuteScalarCommand(sql, CommandType.Text, new List<NpgsqlParameter>() { 
                        new NpgsqlParameter("@hfCode", DbType.String) { Value = this.HealthFacilityCode },
                        new NpgsqlParameter("@gtin", DbType.String) { Value = this.Gtin }
                    }));
                }
                catch
                {
                    Trace.TraceError("Error getting facility AMC for {0}", this.HealthFacilityCode);
                    return 0.0f;
                }
            }
        }



        #region Properties
        public string HealthFacilityCode { get; set; }
        public string Gtin { get; set; }
        public string LotNumber { get; set; }
        public double Received { get; set; }
        public double Distributed { get; set; }
        public double Used { get; set; }
        public double Wasted { get; set; }
        public double StockCount { get; set; }
        public double Balance { get; set; }
        public double Allocated { get; set; }
        public GtinHfStockPolicy GtinStockPolicy
        {
            get
            {
                if (this.m_stockPolicy == null)
                    this.m_stockPolicy = GtinHfStockPolicy.GetGtinHfStockPolicyByHealthFacilityCode(this.HealthFacilityCode);
                return this.m_stockPolicy;
            }
        }
        public ItemManufacturer GtinObject
        {
            get
            {
                if (this.Gtin == "")
                    return null;
                else
                 return ItemManufacturer.GetItemManufacturerByGtin(this.Gtin);
            }
        }
        public HealthFacility HealthFacilityObject
        {
            get
            {
                if (this.HealthFacilityCode == "")
                       return null;
                else
                    return HealthFacility.GetHealthFacilityByCode(this.HealthFacilityCode);
            }
        }
        #endregion

        #region GetData
        public static List<HealthFacilityBalance> GetHealthFacilityBalanceList()
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_BALANCE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityBalanceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static HealthFacilityBalance GetHealthFacilityBalance(string hf, string gtin, string lot_num)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_BALANCE"" WHERE ""HEALTH_FACILITY_CODE"" = @hf " +
                               @"AND ""GTIN"" = @gtin AND ""LOT_NUMBER"" = @lotnum ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@hf", DbType.String) { Value = hf },
                     new NpgsqlParameter("@gtin", DbType.String) { Value = gtin },
                      new NpgsqlParameter("@lotnum", DbType.String) { Value = lot_num }
                    };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("HealthFacilityBalance", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetHealthFacilityBalanceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalance", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<HealthFacilityBalance> GetHealthFacilityBalanceByHealthFacilityCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_BALANCE"" WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBalance", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetHealthFacilityBalanceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByHealthFacilityCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static HealthFacilityBalance GetHealthFacilityBalanceByGtin(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_BALANCE"" WHERE ""GTIN"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBalance", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetHealthFacilityBalanceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByGtin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static HealthFacilityBalance GetHealthFacilityBalanceByLotNumber(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_BALANCE"" WHERE ""LOT_NUMBER"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBalance", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetHealthFacilityBalanceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceByLotNumber", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(HealthFacilityBalance o)
        {
            try
            {
                string query = @"INSERT INTO ""HEALTH_FACILITY_BALANCE"" (""HEALTH_FACILITY_CODE"", ""GTIN"", ""LOT_NUMBER"", ""RECEIVED"", ""DISTRIBUTED"", ""USED"", ""WASTED"", ""STOCK_COUNT"", ""BALANCE"", ""ALLOCATED"") VALUES (@HealthFacilityCode, @Gtin, @LotNumber, @Received, @Distributed, @Used, @Wasted, @StockCount, @Balance, @Allocated) returning ""HEALTH_FACILITY_CODE"" || ""GTIN"" || ""LOT_NUMBER"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@HealthFacilityCode", DbType.String)  { Value = o.HealthFacilityCode },
new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
new NpgsqlParameter("@LotNumber", DbType.String)  { Value = o.LotNumber },
new NpgsqlParameter("@Received", DbType.Double)  { Value = (object)o.Received ?? DBNull.Value },
new NpgsqlParameter("@Distributed", DbType.Double)  { Value = (object)o.Distributed ?? DBNull.Value },
new NpgsqlParameter("@Used", DbType.Double)  { Value = (object)o.Used ?? DBNull.Value },
new NpgsqlParameter("@Wasted", DbType.Double)  { Value = (object)o.Wasted ?? DBNull.Value },
new NpgsqlParameter("@StockCount", DbType.Double)  { Value = (object)o.StockCount ?? DBNull.Value },
new NpgsqlParameter("@Balance", DbType.Double)  { Value = (object)o.Balance ?? DBNull.Value },
new NpgsqlParameter("@Allocated", DbType.Double)  { Value = (object)o.Allocated ?? DBNull.Value }
};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBalance", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(HealthFacilityBalance o)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITY_BALANCE"" SET ""RECEIVED"" = @Received, ""DISTRIBUTED"" = @Distributed, ""USED"" = @Used, ""WASTED"" = @Wasted, ""STOCK_COUNT"" = @StockCount, ""BALANCE"" = @Balance, ""ALLOCATED"" = @Allocated WHERE ""HEALTH_FACILITY_CODE"" = @HealthFacilityCode AND ""GTIN"" = @Gtin AND ""LOT_NUMBER"" = @LotNumber  ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@HealthFacilityCode", DbType.String)  { Value = o.HealthFacilityCode },
new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
new NpgsqlParameter("@LotNumber", DbType.String)  { Value = o.LotNumber },
new NpgsqlParameter("@Received", DbType.Double)  { Value = (object)o.Received ?? DBNull.Value },
new NpgsqlParameter("@Distributed", DbType.Double)  { Value = (object)o.Distributed ?? DBNull.Value },
new NpgsqlParameter("@Used", DbType.Double)  { Value = (object)o.Used ?? DBNull.Value },
new NpgsqlParameter("@Wasted", DbType.Double)  { Value = (object)o.Wasted ?? DBNull.Value },
new NpgsqlParameter("@StockCount", DbType.Double)  { Value = (object)o.StockCount ?? DBNull.Value },
new NpgsqlParameter("@Balance", DbType.Double)  { Value = (object)o.Balance ?? DBNull.Value },
new NpgsqlParameter("@Allocated", DbType.Double)  { Value = (object)o.Allocated ?? DBNull.Value }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBalance", string.Format("RecordId: {0}", o.HealthFacilityCode + o.Gtin + o.LotNumber), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(string healthFacilityCode, string gtin, string lotNumber)
        {
            try
            {
                string query = @"DELETE FROM ""HEALTH_FACILITY_BALANCE"" WHERE ""HEALTH_FACILITY_CODE"" = @HealthFacilityCode AND ""GTIN"" = @Gtin AND ""LOT_NUMBER"" = @LotNumber";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@HealthFacilityCode", DbType.String)  { Value = healthFacilityCode },
new NpgsqlParameter("@Gtin", DbType.String)  { Value = gtin },
new NpgsqlParameter("@LotNumber", DbType.String)  { Value = lotNumber }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityBalance", string.Format("RecordId: {0}", healthFacilityCode + gtin + lotNumber), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityBalance", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static HealthFacilityBalance GetHealthFacilityBalanceAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityBalance o = new HealthFacilityBalance();
                    o.HealthFacilityCode = row["HEALTH_FACILITY_CODE"].ToString();
                    o.Gtin = row["GTIN"].ToString();
                    o.LotNumber = row["LOT_NUMBER"].ToString();
                    o.Received = Helper.ConvertToDecimal(row["RECEIVED"]);
                    o.Distributed = Helper.ConvertToDecimal(row["DISTRIBUTED"]);
                    o.Used = Helper.ConvertToDecimal(row["USED"]);
                    o.Wasted = Helper.ConvertToDecimal(row["WASTED"]);
                    o.StockCount = Helper.ConvertToDecimal(row["STOCK_COUNT"]);
                    o.Balance = Helper.ConvertToDecimal(row["BALANCE"]);
                    o.Allocated = Helper.ConvertToDecimal(row["ALLOCATED"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<HealthFacilityBalance> GetHealthFacilityBalanceAsList(DataTable dt)
        {
            List<HealthFacilityBalance> oList = new List<HealthFacilityBalance>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityBalance o = new HealthFacilityBalance();
                    o.HealthFacilityCode = row["HEALTH_FACILITY_CODE"].ToString();
                    o.Gtin = row["GTIN"].ToString();
                    o.LotNumber = row["LOT_NUMBER"].ToString();
                    o.Received = Helper.ConvertToDecimal(row["RECEIVED"]);
                    o.Distributed = Helper.ConvertToDecimal(row["DISTRIBUTED"]);
                    o.Used = Helper.ConvertToDecimal(row["USED"]);
                    o.Wasted = Helper.ConvertToDecimal(row["WASTED"]);
                    o.StockCount = Helper.ConvertToDecimal(row["STOCK_COUNT"]);
                    o.Balance = Helper.ConvertToDecimal(row["BALANCE"]);
                    o.Allocated = Helper.ConvertToDecimal(row["ALLOCATED"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityBalance", "GetHealthFacilityBalanceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}