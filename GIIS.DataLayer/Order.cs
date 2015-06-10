using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Order
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public Int32 SenderId { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public Int32 Status { get; set; }
        public bool Processed { get; set; }
        public string Notes { get; set; }
        public Int32 ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region GetData
        public static List<Order> GetOrderList()
        {
            try
            {
                string query = @"SELECT * FROM ""ORDER"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetOrderList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetOrderForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ORDER"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetOrderForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Order GetOrderById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""ORDER"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Order", i.ToString(), 4, DateTime.Now, 1);
                return GetOrderAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetOrderById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Order o)
        {
            try
            {
                string query = @"INSERT INTO ""ORDER"" (""ORDER_NUMBER"", ""ORDER_DATE"", ""SENDER_ID"", ""CONFIRMATION_DATE"", ""STATUS"", ""PROCESSED"", ""NOTES"", ""MODIFIED_BY"", ""MODIFIED_ON"", ""IS_ACTIVE"") VALUES (@OrderNumber, @OrderDate, @SenderId, @ConfirmationDate, @Status, @Processed, @Notes, @ModifiedBy, @ModifiedOn, @IsActive) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderNumber", DbType.Int32)  { Value = o.OrderNumber },
new NpgsqlParameter("@OrderDate", DbType.Date)  { Value = o.OrderDate },
new NpgsqlParameter("@SenderId", DbType.Int32)  { Value = o.SenderId },
new NpgsqlParameter("@ConfirmationDate", DbType.Date)  { Value = (object)o.ConfirmationDate ?? DBNull.Value },
new NpgsqlParameter("@Status", DbType.Int32)  { Value = o.Status },
new NpgsqlParameter("@Processed", DbType.Boolean)  { Value = o.Processed },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Order", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Order o)
        {
            try
            {
                string query = @"UPDATE ""ORDER"" SET ""ID"" = @Id, ""ORDER_NUMBER"" = @OrderNumber, ""ORDER_DATE"" = @OrderDate, ""SENDER_ID"" = @SenderId, ""CONFIRMATION_DATE"" = @ConfirmationDate, ""STATUS"" = @Status, ""PROCESSED"" = @Processed, ""NOTES"" = @Notes, ""MODIFIED_BY"" = @ModifiedBy, ""MODIFIED_ON"" = @ModifiedOn, ""IS_ACTIVE"" = @IsActive WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderNumber", DbType.Int32)  { Value = o.OrderNumber },
new NpgsqlParameter("@OrderDate", DbType.Date)  { Value = o.OrderDate },
new NpgsqlParameter("@SenderId", DbType.Int32)  { Value = o.SenderId },
new NpgsqlParameter("@ConfirmationDate", DbType.Date)  { Value = (object)o.ConfirmationDate ?? DBNull.Value },
new NpgsqlParameter("@Status", DbType.Int32)  { Value = o.Status },
new NpgsqlParameter("@Processed", DbType.Boolean)  { Value = o.Processed },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Order", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""ORDER"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Order", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""ORDER"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Order", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Order GetOrderAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Order o = new Order();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.OrderNumber = Helper.ConvertToInt(row["ORDER_NUMBER"]);
                    o.OrderDate = Helper.ConvertToDate(row["ORDER_DATE"]);
                    o.SenderId = Helper.ConvertToInt(row["SENDER_ID"]);
                    o.ConfirmationDate = Helper.ConvertToDate(row["CONFIRMATION_DATE"]);
                    o.Status = Helper.ConvertToInt(row["STATUS"]);
                    o.Processed = Helper.ConvertToBoolean(row["PROCESSED"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Order", "GetOrderAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Order> GetOrderAsList(DataTable dt)
        {
            List<Order> oList = new List<Order>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Order o = new Order();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.OrderNumber = Helper.ConvertToInt(row["ORDER_NUMBER"]);
                    o.OrderDate = Helper.ConvertToDate(row["ORDER_DATE"]);
                    o.SenderId = Helper.ConvertToInt(row["SENDER_ID"]);
                    o.ConfirmationDate = Helper.ConvertToDate(row["CONFIRMATION_DATE"]);
                    o.Status = Helper.ConvertToInt(row["STATUS"]);
                    o.Processed = Helper.ConvertToBoolean(row["PROCESSED"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Order", "GetOrderAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
