using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class TransferOrderDetail
    {
        public static TransferOrderDetail GetTransferOrderDetailByOrderNumAndOrderDetail(int orderNumber, int orderDetailNum)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSFER_ORDER_DETAIL"" WHERE ""ORDER_NUM"" = @OrderNum AND  ""ORDER_DETAIL_NUM"" = @OrderDetailNum";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@OrderNum", DbType.Int32) { Value = orderNumber },
                new NpgsqlParameter("@OrderDetailNum", DbType.Int32) { Value = orderDetailNum }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("TransferOrderDetail", string.Format("RecordId: {0}", s + i), 4, DateTime.Now, 1);
                return GetTransferOrderDetailAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailByOrderNumAndOrderDetail", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static TransferOrderDetail GetTransferOrderDetailByOrderNumGtinLotNumber(int orderNumber, string gtin, string lot)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSFER_ORDER_DETAIL"" WHERE ""ORDER_NUM"" = @OrderNum AND  ""ORDER_GTIN"" = @gtin AND ""ORDER_GTIN_LOTNUM"" = @gtinlot";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@OrderNum", DbType.Int32) { Value = orderNumber },
                new NpgsqlParameter("@gtin", DbType.String) { Value = gtin },
                new NpgsqlParameter("@gtinlot", DbType.String) { Value = lot }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("TransferOrderDetail", string.Format("RecordId: {0}", s + i), 4, DateTime.Now, 1);
                return GetTransferOrderDetailAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailByOrderNumGtinLotNumber", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetGtinForTransferOrderDetail(string healthFacilityCode)
        {
            try
            {
                string query = @"select DISTINCT ""GTIN"", i.""CODE"" as ""ITEM"" FROM ""HEALTH_FACILITY_BALANCE"" join ""ITEM_MANUFACTURER"" using (""GTIN"") join ""ITEM"" i on i.""ID"" = ""ITEM_MANUFACTURER"".""ITEM_ID"" where ""HEALTH_FACILITY_CODE"" = @HealthFacilityCode and (""GTIN_PARENT"" = '' OR ""GTIN_PARENT"" is null ) AND ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true ORDER BY i.""CODE"" ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@HealthFacilityCode", DbType.String) { Value = healthFacilityCode}
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetGtinForTransferOrderDetail", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<TransferOrderDetail> GetTransferOrderDetailByOrderNumAsList(Int32 orderNumber, string healthFacilityCode)
        {
            try
            {
                string query = @"select distinct ""ORDER_NUM"", ""ORDER_DETAIL_NUM"", ""GTIN"" AS ""ORDER_GTIN"", ""ORDER_GTIN_LOTNUM"", 
                               ""ORDER_CUSTOM_ITEM"", ""ORDER_DETAIL_DESCRIPTION"", ""ORDER_QTY"", 
                               ""ORDER_UOM"", ""ORDER_QTY_IN_BASE_UOM"", ""ORDER_DETAIL_STATUS"", 
                               TOD.""MODIFIED_ON"", TOD.""MODIFIED_BY"", ""REV_NUM"", ""ITEM"".""CODE""
                               from ""TRANSFER_ORDER_DETAIL"" TOD
                                JOIN ""HEALTH_FACILITY_BALANCE"" HFB ON HFB.""GTIN"" = TOD.""ORDER_GTIN"" join ""ITEM_MANUFACTURER"" using (""GTIN"") join ""ITEM"" on ""ITEM_ID"" = ""ITEM"".""ID""
                               where ""ORDER_NUM"" = @OrderNumber and HFB.""HEALTH_FACILITY_CODE"" = @HealthFacilityCode AND ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true ORDER BY ""ITEM"".""CODE"" ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@OrderNumber", DbType.Int32) { Value = orderNumber },
                    new NpgsqlParameter("@HealthFacilityCode", DbType.String) { Value = healthFacilityCode}
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("TransferOrderDetail",  orderNumber, 4, DateTime.Now, 1);
                return GetTransferOrderDetailAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailByOrderNumAsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetTransferOrderDetailsByHealthFacilityId(string OrderFacilityFrom)
        {
            try
            {
                string query = @"SELECT ""ORDER_GTIN"" as ""Order Gtin"", ""ORDER_GTIN_LOTNUM"" as ""Order Gtin Lot Number"", SUM(""ORDER_QTY"") as ""Quantity""
                                FROM ""TRANSFER_ORDER_DETAIL"" TOD INNER JOIN ""TRANSFER_ORDER_HEADER"" TOH ON TOD.""ORDER_NUM"" = TOH.""ORDER_NUM""
                                WHERE ""ORDER_FACILITY_FROM"" = @OrderFacilityFrom
                                GROUP BY ""ORDER_GTIN"", ""ORDER_GTIN_LOTNUM"" ";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@OrderFacilityFrom", DbType.String) { Value = OrderFacilityFrom }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailsByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}