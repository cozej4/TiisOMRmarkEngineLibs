using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Transaction
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 TransactionTypeId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Int32 SenderId { get; set; }
        public Int32 ReceiverId { get; set; }
        public Int32 OrderId { get; set; }
        public Int32 HealthFacilityId { get; set; }
        public string SerialNo { get; set; }
        public Int32 Status { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Order Order
        {
            get
            {
                if (this.OrderId > 0)
                    return Order.GetOrderById(this.OrderId);
                else
                    return null;
            }
        }
        public TransactionType TransactionType
        {
            get
            {
                if (this.TransactionTypeId > 0)
                    return TransactionType.GetTransactionTypeById(this.TransactionTypeId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<Transaction> GetTransactionList()
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSACTION"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetTransactionList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetTransactionForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""TRANSACTION"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetTransactionForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static Transaction GetTransactionById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSACTION"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Transaction", i.ToString(), 4, DateTime.Now, 1);
                return GetTransactionAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetTransactionById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Transaction o)
        {
            try
            {
                string query = @"INSERT INTO ""TRANSACTION"" (""TRANSACTION_TYPE_ID"", ""TRANSACTION_DATE"", ""SENDER_ID"", ""RECEIVER_ID"", ""ORDER_ID"", ""HEALTH_FACILITY_ID"", ""SERIAL_NO"", ""STATUS"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@TransactionTypeId, @TransactionDate, @SenderId, @ReceiverId, @OrderId, @HealthFacilityId, @SerialNo, @Status, @Notes, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@TransactionTypeId", DbType.Int32)  { Value = o.TransactionTypeId },
new NpgsqlParameter("@TransactionDate", DbType.Date)  { Value = o.TransactionDate },
new NpgsqlParameter("@SenderId", DbType.Int32)  { Value = (object)o.SenderId ?? DBNull.Value },
new NpgsqlParameter("@ReceiverId", DbType.Int32)  { Value = (object)o.ReceiverId ?? DBNull.Value },
new NpgsqlParameter("@OrderId", DbType.Int32)  { Value = (object)o.OrderId ?? DBNull.Value },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = (object)o.HealthFacilityId ?? DBNull.Value },
new NpgsqlParameter("@SerialNo", DbType.String)  { Value = (object)o.SerialNo ?? DBNull.Value },
new NpgsqlParameter("@Status", DbType.Int32)  { Value = (object)o.Status ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Transaction", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Transaction o)
        {
            try
            {
                string query = @"UPDATE ""TRANSACTION"" SET ""ID"" = @Id, ""TRANSACTION_TYPE_ID"" = @TransactionTypeId, ""TRANSACTION_DATE"" = @TransactionDate, ""SENDER_ID"" = @SenderId, ""RECEIVER_ID"" = @ReceiverId, ""ORDER_ID"" = @OrderId, ""HEALTH_FACILITY_ID"" = @HealthFacilityId, ""SERIAL_NO"" = @SerialNo, ""STATUS"" = @Status, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@TransactionTypeId", DbType.Int32)  { Value = o.TransactionTypeId },
new NpgsqlParameter("@TransactionDate", DbType.Date)  { Value = o.TransactionDate },
new NpgsqlParameter("@SenderId", DbType.Int32)  { Value = (object)o.SenderId ?? DBNull.Value },
new NpgsqlParameter("@ReceiverId", DbType.Int32)  { Value = (object)o.ReceiverId ?? DBNull.Value },
new NpgsqlParameter("@OrderId", DbType.Int32)  { Value = (object)o.OrderId ?? DBNull.Value },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = (object)o.HealthFacilityId ?? DBNull.Value },
new NpgsqlParameter("@SerialNo", DbType.String)  { Value = (object)o.SerialNo ?? DBNull.Value },
new NpgsqlParameter("@Status", DbType.Int32)  { Value = (object)o.Status ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Transaction", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""TRANSACTION"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Transaction", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static Transaction GetTransactionAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Transaction o = new Transaction();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.TransactionTypeId = Helper.ConvertToInt(row["TRANSACTION_TYPE_ID"]);
                    o.TransactionDate = Helper.ConvertToDate(row["TRANSACTION_DATE"]);
                    o.SenderId = Helper.ConvertToInt(row["SENDER_ID"]);
                    o.ReceiverId = Helper.ConvertToInt(row["RECEIVER_ID"]);
                    o.OrderId = Helper.ConvertToInt(row["ORDER_ID"]);
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.SerialNo = row["SERIAL_NO"].ToString();
                    o.Status = Helper.ConvertToInt(row["STATUS"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Transaction", "GetTransactionAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Transaction> GetTransactionAsList(DataTable dt)
        {
            List<Transaction> oList = new List<Transaction>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Transaction o = new Transaction();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.TransactionTypeId = Helper.ConvertToInt(row["TRANSACTION_TYPE_ID"]);
                    o.TransactionDate = Helper.ConvertToDate(row["TRANSACTION_DATE"]);
                    o.SenderId = Helper.ConvertToInt(row["SENDER_ID"]);
                    o.ReceiverId = Helper.ConvertToInt(row["RECEIVER_ID"]);
                    o.OrderId = Helper.ConvertToInt(row["ORDER_ID"]);
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.SerialNo = row["SERIAL_NO"].ToString();
                    o.Status = Helper.ConvertToInt(row["STATUS"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Transaction", "GetTransactionAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}