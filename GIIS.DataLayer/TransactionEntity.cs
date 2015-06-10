using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GIIS.DataLayer
{
    public partial class Transaction
    {
        public string SStatus
        {
            get
            {
                if (Status == 1)
                    return "Confirmed";
                else if (Status == 2)
                    return "Sent";
                else return "Draft";
            }
        }

        public static int GetMaxTransactionSerialNumber(int transactionTypeId, int healthFacilityId)
        {
            try
            {
                string query = string.Format(@"SELECT ""SERIAL_NO"" FROM ""TRANSACTION"" WHERE ""TRANSACTION_TYPE_ID"" = {0} AND ""SENDER_ID"" = {1} AND date_part('year', ""TRANSACTION_DATE"") = {2} ORDER BY ""SERIAL_NO"" DESC LIMIT 1;", transactionTypeId, healthFacilityId, DateTime.Today.Year);
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                if (count != null)
                    return int.Parse(count.ToString());
                else return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetMaxTransactionSerialNumber", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetIdTransaction(int transactionTypeId, int healthFacilityId, DateTime tdate)
        {
            try
            {
                string query = string.Format(@"SELECT ""ID"" FROM ""TRANSACTION"" WHERE ""TRANSACTION_TYPE_ID"" = {0} AND ""RECEIVER_ID"" = {1} AND ""SENDER_ID"" = {1} AND ""TRANSACTION_DATE"" = '{2}' LIMIT 1;", transactionTypeId, healthFacilityId, tdate.ToString("yyyy-MM-dd"));
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                if (id != null)
                    return int.Parse(id.ToString());
                else return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetIdTransaction", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetLastTransaction(int transactionTypeId, int healthFacilityId, DateTime tdate)
        {
            try
            {
                string query = string.Format(@"SELECT ""ID"" FROM ""TRANSACTION"" WHERE ""TRANSACTION_TYPE_ID"" = {0} AND ""HEALTH_FACILITY_ID"" = {1}  AND ""TRANSACTION_DATE"" = '{2}' LIMIT 1;", transactionTypeId, healthFacilityId, tdate.ToString("yyyy-MM-dd"));
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                if (id != null)
                    return int.Parse(id.ToString());
                else return 0;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetLastTransaction", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionList(string where)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSACTION"" WHERE " + where + @" ORDER BY ""TRANSACTION_DATE"" DESC ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetTransactionList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionListByOrderId(int orderId)
        {
            try
            {
                string query = @"SELECT * FROM ""TRANSACTION"" WHERE ""ORDER_ID"" = " + orderId + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Transaction", "GetTransactionListByOrderId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}