using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public partial class Order
    {
        public static int GetMaxOrderSerialNumber(int healthFacilityId)
        {
            try
            {
                string query = string.Format(@"select ""ORDER_NUMBER"" FROM ""ORDER""  where ""IS_ACTIVE"" = 'True' and ""SENDER_ID"" = {0} and date_part('year', ""ORDER_DATE"") = {1} order by ""ORDER_NUMBER"" desc limit 1;", healthFacilityId, DateTime.Today.Year);
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                if (count != null)
                    return int.Parse(count.ToString());
                else return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetMaxOrderSerialNumber", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Order> GetOrderListByHealthFacilityId(int healthFacilityId)
        {
            try
            {
                string query = @"select * from ""ORDER""  where ""IS_ACTIVE"" = 'True' and ""SENDER_ID"" in (select ""ID"" FROM ""HEALTH_FACILITY"" WHERE ""PARENT_ID"" = " + healthFacilityId + @") and ""STATUS"" = 2 order by ""ORDER_DATE"";";//STATUS = 2(SENT)
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetOrderListByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountOrderListByHealthFacilityId(int healthFacilityId, string where)
        {
            try
            {
                string query = @"select count(*) from ""ORDER""  where " + where + @" ""IS_ACTIVE"" = 'True' and ""SENDER_ID"" in (select ""ID"" FROM ""HEALTH_FACILITY"" WHERE ""PARENT_ID"" = " + healthFacilityId + @") ;";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetCountOrderListByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Order> GetOrderListByHealthFacility(int healthFacilityId, string where)
        {
            try
            {
                string query = @"select * from ""ORDER""  where " + where + @" AND ""IS_ACTIVE"" = 'True' and ""SENDER_ID""  = " + healthFacilityId + @" order by ""ORDER_DATE"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetOrderListByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<Order> GetIncomingOrdersByHealthFacility(int healthFacilityId)
        {
            try
            {
                string query = @"select * from ""ORDER""  where ""IS_ACTIVE"" = 'True' and ""SENDER_ID""  in  (select ""ID"" from ""HEALTH_FACILITY"" where ""PARENT_ID"" = " + healthFacilityId + @" ) and ""STATUS""=1 and ""ID"" not in (Select ""ORDER_ID"" from ""TRANSACTION"") order by ""ORDER_DATE"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetIncomingOrdersByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        //public static List<Order> GetOrderListByHealthFacility(ref int maximumRows, ref int startRowIndex, int healthFacilityId, string where)
        //{
        //    try
        //    {
        //        string query = @"select * from ""ORDER""  where " + where + @" AND ""IS_ACTIVE"" = 'True' and ""SENDER_ID""  = " + healthFacilityId + @" order by ""ORDER_DATE"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
        //        DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
        //        return GetOrderAsList(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertEntity("Order", "GetOrderListByHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
        //        throw ex;
        //    }
        //}
        public static Order GetOrderByDateAndHealthFacility(DateTime orderdate, Int32 healthFacilityId)
        {
            try
            {
                string query = @"SELECT * FROM ""ORDER""  where ""IS_ACTIVE"" = 'True' and ""ORDER_DATE"" = '" + orderdate.ToString("yyyy-MM-dd") + @"' AND ""SENDER_ID"" = " + healthFacilityId + " ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetOrderAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Order", "GetOrderByDateAndHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

    }
}
