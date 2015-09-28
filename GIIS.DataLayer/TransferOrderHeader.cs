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

namespace GIIS.DataLayer
{
    public partial class TransferOrderHeader
    {

        #region Properties
        public Int32 OrderNum { get; set; }
        public DateTime OrderSchedReplenishDate { get; set; }
        public string OrderFacilityFrom { get; set; }
        public string OrderFacilityTo { get; set; }
        public string OrderCarrier { get; set; }
        public Int32 OrderStatus { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Int32 RevNum { get; set; }
        
        public HealthFacility OrderFacilityFromObject
        {
            get
            {
                if (this.OrderFacilityFrom != "")
                    return HealthFacility.GetHealthFacilityByCode(this.OrderFacilityFrom);
                else
                    return null;
            }
        }

        public HealthFacility OrderFacilityToObject
        {
            get
            {
                if (this.OrderFacilityTo != "")
                    return HealthFacility.GetHealthFacilityByCode(this.OrderFacilityTo);
                else
                    return null;
            }
        }
        public string OrderStatusName { get; set; }
        #endregion

        #region GetData
        public static List<TransferOrderHeader> GetTransferOrderHeaderList()
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSFER_ORDER_HEADER"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransferOrderHeaderAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetTransferOrderHeaderList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetTransferOrderHeaderForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""TRANSFER_ORDER_HEADER"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetTransferOrderHeaderForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static TransferOrderHeader GetTransferOrderHeaderByOrderNum(int s)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSFER_ORDER_HEADER"" WHERE ""ORDER_NUM"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("TransferOrderHeader", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetTransferOrderHeaderAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetTransferOrderHeaderByOrderNum", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(TransferOrderHeader o)
        {
            try
            {
                string query = @"INSERT INTO ""TRANSFER_ORDER_HEADER"" (""ORDER_SCHED_REPLENISH_DATE"", ""ORDER_FACILITY_FROM"", ""ORDER_FACILITY_TO"", ""ORDER_CARRIER"", ""ORDER_STATUS"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""REV_NUM"") VALUES (@OrderSchedReplenishDate, @OrderFacilityFrom, @OrderFacilityTo, @OrderCarrier, @OrderStatus, @ModifiedOn, @ModifiedBy, @RevNum) returning ""ORDER_NUM"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    //new NpgsqlParameter("@OrderNum", DbType.Int32)  { Value = o.OrderNum },
                    new NpgsqlParameter("@OrderSchedReplenishDate", DbType.Date)  { Value = o.OrderSchedReplenishDate },
                    new NpgsqlParameter("@OrderFacilityFrom", DbType.String)  { Value = o.OrderFacilityFrom },
                    new NpgsqlParameter("@OrderFacilityTo", DbType.String)  { Value = o.OrderFacilityTo },
                    new NpgsqlParameter("@OrderCarrier", DbType.String)  { Value = (object)o.OrderCarrier ?? DBNull.Value },
                    new NpgsqlParameter("@OrderStatus", DbType.Int32)  { Value = o.OrderStatus },
                    new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
                    new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
                    new NpgsqlParameter("@RevNum", DbType.Int32)  { Value = o.RevNum }
                };

                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransferOrderHeader", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(TransferOrderHeader o)
        {
            try
            {
                string query = @"UPDATE ""TRANSFER_ORDER_HEADER"" SET ""ORDER_SCHED_REPLENISH_DATE"" = @OrderSchedReplenishDate, ""ORDER_FACILITY_FROM"" = @OrderFacilityFrom, ""ORDER_FACILITY_TO"" = @OrderFacilityTo, ""ORDER_CARRIER"" = @OrderCarrier, ""ORDER_STATUS"" = @OrderStatus, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""REV_NUM"" = @RevNum WHERE ""ORDER_NUM"" = @OrderNum ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderNum", DbType.Int32)  { Value = o.OrderNum },
new NpgsqlParameter("@OrderSchedReplenishDate", DbType.Date)  { Value = o.OrderSchedReplenishDate },
new NpgsqlParameter("@OrderFacilityFrom", DbType.String)  { Value = o.OrderFacilityFrom },
new NpgsqlParameter("@OrderFacilityTo", DbType.String)  { Value = o.OrderFacilityTo },
new NpgsqlParameter("@OrderCarrier", DbType.String)  { Value = (object)o.OrderCarrier ?? DBNull.Value },
new NpgsqlParameter("@OrderStatus", DbType.Int32)  { Value = o.OrderStatus },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@RevNum", DbType.Int32)  { Value = o.RevNum }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransferOrderHeader", o.OrderNum.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(string orderNum)
        {
            try
            {
                string query = @"DELETE FROM ""TRANSFER_ORDER_HEADER"" WHERE ""ORDER_NUM"" = @OrderNum";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderNum", DbType.Int32)  { Value = orderNum }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransferOrderHeader", string.Format("RecordId: {0}", orderNum), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static TransferOrderHeader GetTransferOrderHeaderAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    TransferOrderHeader o = new TransferOrderHeader();
                    o.OrderNum = Helper.ConvertToInt(row["ORDER_NUM"].ToString());
                    o.OrderSchedReplenishDate = Helper.ConvertToDate(row["ORDER_SCHED_REPLENISH_DATE"]);
                    o.OrderFacilityFrom = row["ORDER_FACILITY_FROM"].ToString();
                    o.OrderFacilityTo = row["ORDER_FACILITY_TO"].ToString();
                    o.OrderCarrier = row["ORDER_CARRIER"].ToString();
                    o.OrderStatus = Helper.ConvertToInt(row["ORDER_STATUS"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.RevNum = Helper.ConvertToInt(row["REV_NUM"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("TransferOrderHeader", "GetTransferOrderHeaderAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<TransferOrderHeader> GetTransferOrderHeaderAsList(DataTable dt)
        {
            List<TransferOrderHeader> oList = new List<TransferOrderHeader>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    TransferOrderHeader o = new TransferOrderHeader();
                    o.OrderNum = Helper.ConvertToInt(row["ORDER_NUM"].ToString());
                    o.OrderSchedReplenishDate = Helper.ConvertToDate(row["ORDER_SCHED_REPLENISH_DATE"]);
                    o.OrderFacilityFrom = row["ORDER_FACILITY_FROM"].ToString();
                    o.OrderFacilityTo = row["ORDER_FACILITY_TO"].ToString();
                    o.OrderCarrier = row["ORDER_CARRIER"].ToString();
                    o.OrderStatus = Helper.ConvertToInt(row["ORDER_STATUS"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.RevNum = Helper.ConvertToInt(row["REV_NUM"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("TransferOrderHeader", "GetTransferOrderHeaderAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        public static List<TransferOrderHeader> GetTransferOrderHeaderAsListForStatus(DataTable dt)
        {
            List<TransferOrderHeader> oList = new List<TransferOrderHeader>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    TransferOrderHeader o = new TransferOrderHeader();
                    o.OrderNum = Helper.ConvertToInt(row["ORDER_NUM"].ToString());
                    o.OrderSchedReplenishDate = Helper.ConvertToDate(row["ORDER_SCHED_REPLENISH_DATE"]);
                    o.OrderFacilityFrom = row["ORDER_FACILITY_FROM"].ToString();
                    o.OrderFacilityTo = row["ORDER_FACILITY_TO"].ToString();
                    o.OrderCarrier = row["ORDER_CARRIER"].ToString();
                    o.OrderStatusName = row["ORDER_STATUS"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.RevNum = Helper.ConvertToInt(row["REV_NUM"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("TransferOrderHeader", "GetTransferOrderHeaderAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}