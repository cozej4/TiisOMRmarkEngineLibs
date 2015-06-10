using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class TransactionLines
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 TransactionId { get; set; }
        public Int32 ItemLotId { get; set; }
        public Int32 Quantity { get; set; }
        public bool IsActive { get; set; }
        public Int32 AdjustmentId { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public ItemLot ItemLot
        {
            get
            {
                if (this.ItemLotId > 0)
                    return ItemLot.GetItemLotById(this.ItemLotId);
                else
                    return null;
            }
        }
        public Transaction Transaction
        {
            get
            {
                if (this.TransactionId > 0)
                    return Transaction.GetTransactionById(this.TransactionId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<TransactionLines> GetTransactionLinesList()
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSACTION_LINES"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetTransactionLinesForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""TRANSACTION_LINES"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static TransactionLines GetTransactionLinesById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSACTION_LINES"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransactionLines", i.ToString(), 4, DateTime.Now, 1);
                return GetTransactionLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(TransactionLines o)
        {
            try
            {
                string query = @"INSERT INTO ""TRANSACTION_LINES"" (""TRANSACTION_ID"", ""ITEM_LOT_ID"", ""QUANTITY"", ""IS_ACTIVE"", ""ADJUSTMENT_ID"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@TransactionId, @ItemLotId, @Quantity, @IsActive, @AdjustmentId, @Notes, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@TransactionId", DbType.Int32)  { Value = o.TransactionId },
new NpgsqlParameter("@ItemLotId", DbType.Int32)  { Value = o.ItemLotId },
new NpgsqlParameter("@Quantity", DbType.Int32)  { Value = o.Quantity },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@AdjustmentId", DbType.Int32)  { Value = (object)o.AdjustmentId ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransactionLines", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(TransactionLines o)
        {
            try
            {
                string query = @"UPDATE ""TRANSACTION_LINES"" SET ""ID"" = @Id, ""TRANSACTION_ID"" = @TransactionId, ""ITEM_LOT_ID"" = @ItemLotId, ""QUANTITY"" = @Quantity, ""IS_ACTIVE"" = @IsActive, ""ADJUSTMENT_ID"" = @AdjustmentId, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@TransactionId", DbType.Int32)  { Value = o.TransactionId },
new NpgsqlParameter("@ItemLotId", DbType.Int32)  { Value = o.ItemLotId },
new NpgsqlParameter("@Quantity", DbType.Int32)  { Value = o.Quantity },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@AdjustmentId", DbType.Int32)  { Value = (object)o.AdjustmentId ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransactionLines", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""TRANSACTION_LINES"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransactionLines", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""TRANSACTION_LINES"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("TransactionLines", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static TransactionLines GetTransactionLinesAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    TransactionLines o = new TransactionLines();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.TransactionId = Helper.ConvertToInt(row["TRANSACTION_ID"]);
                    o.ItemLotId = Helper.ConvertToInt(row["ITEM_LOT_ID"]);
                    o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.AdjustmentId = Helper.ConvertToInt(row["ADJUSTMENT_ID"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("TransactionLines", "GetTransactionLinesAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<TransactionLines> GetTransactionLinesAsList(DataTable dt)
        {
            List<TransactionLines> oList = new List<TransactionLines>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    TransactionLines o = new TransactionLines();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.TransactionId = Helper.ConvertToInt(row["TRANSACTION_ID"]);
                    o.ItemLotId = Helper.ConvertToInt(row["ITEM_LOT_ID"]);
                    o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.AdjustmentId = Helper.ConvertToInt(row["ADJUSTMENT_ID"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("TransactionLines", "GetTransactionLinesAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
