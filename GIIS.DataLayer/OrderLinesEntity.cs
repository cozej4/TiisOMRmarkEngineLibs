using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public partial class OrderLines
    {
        public static int GetQuantityByOrderAndItem(int orderId, int itemId)
        {
            try
            {
                string query = string.Format(@"SELECT ""QUANTITY"" FROM ""ORDER_LINES"" WHERE ""ORDER_ID"" = {0} AND ""ITEM_ID"" = {1};", orderId, itemId);
                object quantity = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                if (quantity != null)
                    return int.Parse(quantity.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetQuantityByOrderAndItem", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static OrderLines GetOrderLineByOrderAndItem(int orderId, int itemId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""ORDER_LINES"" WHERE ""ORDER_ID"" = {0} AND ""ITEM_ID"" = {1};", orderId, itemId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetQuantityByOrderAndItem", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<OrderLines> GetOrderLineByOrder(int orderId)
        {
            try
            {
                string query = string.Format(@"SELECT ""ORDER_LINES"".* FROM ""ORDER_LINES""
                                                INNER JOIN ""ITEM"" ON ""ORDER_LINES"".""ITEM_ID"" = ""ITEM"".""ID""
                                                WHERE ""ORDER_ID"" = {0}
                                                ORDER BY ""ITEM_CATEGORY_ID"" ;", orderId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("OrderLines", "GetOrderLineByOrder", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
