using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class OrderLines
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 OrderId { get; set; }
        public Int32 ItemId { get; set; }
        public Int32 Quantity { get; set; }
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
        public Item Item
        {
            get
            {
                if (this.ItemId > 0)
                    return Item.GetItemById(this.ItemId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<OrderLines> GetOrderLinesList()
        {
            try
            {
                string query = @"SELECT * FROM ""ORDER_LINES"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetOrderLinesList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetOrderLinesForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ORDER_LINES"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetOrderLinesForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static OrderLines GetOrderLinesById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""ORDER_LINES"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("OrderLines", i.ToString(), 4, DateTime.Now, 1);
                return GetOrderLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetOrderLinesById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static OrderLines GetOrderLinesByOrderId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""ORDER_LINES"" WHERE ""ORDER_ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("OrderLines", i.ToString(), 4, DateTime.Now, 1);
                return GetOrderLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetOrderLinesByOrderId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static OrderLines GetOrderLinesByItemId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""ORDER_LINES"" WHERE ""ITEM_ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("OrderLines", i.ToString(), 4, DateTime.Now, 1);
                return GetOrderLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetOrderLinesByItemId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(OrderLines o)
        {
            try
            {
                string query = @"INSERT INTO ""ORDER_LINES"" (""ORDER_ID"", ""ITEM_ID"", ""QUANTITY"") VALUES (@OrderId, @ItemId, @Quantity) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderId", DbType.Int32)  { Value = o.OrderId },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@Quantity", DbType.Int32)  { Value = o.Quantity }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("OrderLines", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(OrderLines o)
        {
            try
            {
                string query = @"UPDATE ""ORDER_LINES"" SET ""ID"" = @Id, ""ORDER_ID"" = @OrderId, ""ITEM_ID"" = @ItemId, ""QUANTITY"" = @Quantity WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@OrderId", DbType.Int32)  { Value = o.OrderId },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@Quantity", DbType.Int32)  { Value = o.Quantity },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("OrderLines", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""ORDER_LINES"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("OrderLines", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static OrderLines GetOrderLinesAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    OrderLines o = new OrderLines();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.OrderId = Helper.ConvertToInt(row["ORDER_ID"]);
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("OrderLines", "GetOrderLinesAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<OrderLines> GetOrderLinesAsList(DataTable dt)
        {
            List<OrderLines> oList = new List<OrderLines>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    OrderLines o = new OrderLines();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.OrderId = Helper.ConvertToInt(row["ORDER_ID"]);
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.Quantity = Helper.ConvertToInt(row["QUANTITY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("OrderLines", "GetOrderLinesAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
