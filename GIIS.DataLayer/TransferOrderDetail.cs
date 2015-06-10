using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class TransferOrderDetail
    {

        #region Properties
        public Int32 OrderNum { get; set; }
        public Int32 OrderDetailNum { get; set; }
        public string OrderGtin { get; set; }
        public string OrderGtinLotnum { get; set; }
        public bool OrderCustomItem { get; set; }
        public string OrderDetailDescription { get; set; }
        public double OrderQty { get; set; }
        public string OrderUom { get; set; }
        public double OrderQtyInBaseUom { get; set; }
        public Int32 OrderDetailStatus { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Int32 RevNum { get; set; }
        public ItemManufacturer OrderGtinObject
        {
            get
            {
                if (this.OrderGtin == "")
                      return null;
                else
                    return ItemManufacturer.GetItemManufacturerByGtin(this.OrderGtin);
            }
        }
        public TransferOrderHeader OrderNumObject
        {
            get
            {
                if (this.OrderNum > 0)
                    return TransferOrderHeader.GetTransferOrderHeaderByOrderNum(this.OrderNum);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<TransferOrderDetail> GetTransferOrderDetailList()
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSFER_ORDER_DETAIL"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransferOrderDetailAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetTransferOrderDetailForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""TRANSFER_ORDER_DETAIL"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static List<TransferOrderDetail> GetTransferOrderDetailByOrderNum(int orderNumber)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSFER_ORDER_DETAIL"" WHERE ""ORDER_NUM"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = orderNumber }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("TransferOrderDetail", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetTransferOrderDetailAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailByOrderNum", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static TransferOrderDetail GetTransferOrderDetailByOrderDetailNum(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSFER_ORDER_DETAIL"" WHERE ""ORDER_DETAIL_NUM"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("TransferOrderDetail", i.ToString(), 4, DateTime.Now, 1);
                return GetTransferOrderDetailAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailByOrderDetailNum", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(TransferOrderDetail o)
        {
            try
            {
                string query = @"INSERT INTO ""TRANSFER_ORDER_DETAIL"" (""ORDER_NUM"", ""ORDER_GTIN"", ""ORDER_GTIN_LOTNUM"", ""ORDER_CUSTOM_ITEM"", ""ORDER_DETAIL_DESCRIPTION"", ""ORDER_QTY"", ""ORDER_UOM"", ""ORDER_QTY_IN_BASE_UOM"", ""ORDER_DETAIL_STATUS"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""REV_NUM"")
                                                                VALUES (@OrderNum, @OrderGtin, @OrderGtinLotnum, @OrderCustomItem, @OrderDetailDescription, @OrderQty, @OrderUom, @OrderQtyInBaseUom, @OrderDetailStatus, @ModifiedOn, @ModifiedBy, @RevNum) returning ""ORDER_DETAIL_NUM"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderNum", DbType.Int32)  { Value = o.OrderNum },
//new NpgsqlParameter("@OrderDetailNum", DbType.Int32)  { Value = o.OrderDetailNum },
new NpgsqlParameter("@OrderGtin", DbType.String)  { Value = o.OrderGtin },
new NpgsqlParameter("@OrderGtinLotnum", DbType.String)  { Value = (object)o.OrderGtinLotnum ?? DBNull.Value },
new NpgsqlParameter("@OrderCustomItem", DbType.Boolean)  { Value = (object)o.OrderCustomItem ?? DBNull.Value },
new NpgsqlParameter("@OrderDetailDescription", DbType.String)  { Value = (object)o.OrderDetailDescription ?? DBNull.Value },
new NpgsqlParameter("@OrderQty", DbType.Double)  { Value = o.OrderQty },
new NpgsqlParameter("@OrderUom", DbType.String)  { Value = (object)o.OrderUom ?? DBNull.Value },
new NpgsqlParameter("@OrderQtyInBaseUom", DbType.Double)  { Value = (object)o.OrderQtyInBaseUom ?? DBNull.Value },
new NpgsqlParameter("@OrderDetailStatus", DbType.Int32)  { Value = (object)o.OrderDetailStatus ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@RevNum", DbType.Int32)  { Value = o.RevNum }
};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransferOrderDetail", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(TransferOrderDetail o)
        {
            try
            {
                string query = @"UPDATE ""TRANSFER_ORDER_DETAIL"" SET ""ORDER_GTIN"" = @OrderGtin, ""ORDER_GTIN_LOTNUM"" = @OrderGtinLotnum, ""ORDER_CUSTOM_ITEM"" = @OrderCustomItem, ""ORDER_DETAIL_DESCRIPTION"" = @OrderDetailDescription, ""ORDER_QTY"" = @OrderQty, ""ORDER_UOM"" = @OrderUom, ""ORDER_QTY_IN_BASE_UOM"" = @OrderQtyInBaseUom, ""ORDER_DETAIL_STATUS"" = @OrderDetailStatus, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""REV_NUM"" = @RevNum WHERE ""ORDER_NUM"" = @OrderNum AND ""ORDER_DETAIL_NUM"" = @OrderDetailNum ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderNum", DbType.Int32)  { Value = o.OrderNum },
new NpgsqlParameter("@OrderDetailNum", DbType.Int32)  { Value = o.OrderDetailNum },
new NpgsqlParameter("@OrderGtin", DbType.String)  { Value = o.OrderGtin },
new NpgsqlParameter("@OrderGtinLotnum", DbType.String)  { Value = (object)o.OrderGtinLotnum ?? DBNull.Value },
new NpgsqlParameter("@OrderCustomItem", DbType.Boolean)  { Value = (object)o.OrderCustomItem ?? DBNull.Value },
new NpgsqlParameter("@OrderDetailDescription", DbType.String)  { Value = (object)o.OrderDetailDescription ?? DBNull.Value },
new NpgsqlParameter("@OrderQty", DbType.Double)  { Value = o.OrderQty },
new NpgsqlParameter("@OrderUom", DbType.String)  { Value = (object)o.OrderUom ?? DBNull.Value },
new NpgsqlParameter("@OrderQtyInBaseUom", DbType.Double)  { Value = (object)o.OrderQtyInBaseUom ?? DBNull.Value },
new NpgsqlParameter("@OrderDetailStatus", DbType.Int32)  { Value = (object)o.OrderDetailStatus ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@RevNum", DbType.Int32)  { Value = o.RevNum }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransferOrderDetail", o.OrderNum.ToString() + " " + o.OrderDetailNum.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(string orderNum, int orderDetailNum)
        {
            try
            {
                string query = @"DELETE FROM ""TRANSFER_ORDER_DETAIL"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderNum", DbType.Int32)  { Value = orderNum },
new NpgsqlParameter("@OrderDetailNum", DbType.Int32)  { Value = orderDetailNum }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("TransferOrderDetail", string.Format("RecordId: {0}", orderNum + orderDetailNum), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderDetail", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static TransferOrderDetail GetTransferOrderDetailAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    TransferOrderDetail o = new TransferOrderDetail();
                    o.OrderNum = Helper.ConvertToInt(row["ORDER_NUM"].ToString());
                    o.OrderDetailNum = Helper.ConvertToInt(row["ORDER_DETAIL_NUM"]);
                    o.OrderGtin = row["ORDER_GTIN"].ToString();
                    o.OrderGtinLotnum = row["ORDER_GTIN_LOTNUM"].ToString();
                    o.OrderCustomItem = Helper.ConvertToBoolean(row["ORDER_CUSTOM_ITEM"]);
                    o.OrderDetailDescription = row["ORDER_DETAIL_DESCRIPTION"].ToString();
                    o.OrderQty = Helper.ConvertToDecimal(row["ORDER_QTY"]);
                    o.OrderUom = row["ORDER_UOM"].ToString();
                    o.OrderQtyInBaseUom = Helper.ConvertToDecimal(row["ORDER_QTY_IN_BASE_UOM"]);
                    o.OrderDetailStatus = Helper.ConvertToInt(row["ORDER_DETAIL_STATUS"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.RevNum = Helper.ConvertToInt(row["REV_NUM"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<TransferOrderDetail> GetTransferOrderDetailAsList(DataTable dt)
        {
            List<TransferOrderDetail> oList = new List<TransferOrderDetail>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    TransferOrderDetail o = new TransferOrderDetail();
                    o.OrderNum = Helper.ConvertToInt(row["ORDER_NUM"].ToString());
                    o.OrderDetailNum = Helper.ConvertToInt(row["ORDER_DETAIL_NUM"]);
                    o.OrderGtin = row["ORDER_GTIN"].ToString();
                    o.OrderGtinLotnum = row["ORDER_GTIN_LOTNUM"].ToString();
                    o.OrderCustomItem = Helper.ConvertToBoolean(row["ORDER_CUSTOM_ITEM"]);
                    o.OrderDetailDescription = row["ORDER_DETAIL_DESCRIPTION"].ToString();
                    o.OrderQty = Helper.ConvertToDecimal(row["ORDER_QTY"]);
                    o.OrderUom = row["ORDER_UOM"].ToString();
                    o.OrderQtyInBaseUom = Helper.ConvertToDecimal(row["ORDER_QTY_IN_BASE_UOM"]);
                    o.OrderDetailStatus = Helper.ConvertToInt(row["ORDER_DETAIL_STATUS"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.RevNum = Helper.ConvertToInt(row["REV_NUM"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("TransferOrderDetail", "GetTransferOrderDetailAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}