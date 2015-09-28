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
using System.Linq;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    partial class TransactionLines
    {
        public static int GetTransactionIDByItemLot(int lotid, DateTime tdate)
        {
            try
            {
                string query = string.Format(@"SELECT ""TRANSACTION_ID"" FROM ""TRANSACTION_LINES"" WHERE ""ITEM_LOT_ID"" = {0} AND ""TRANSACTION_ID"" IN (SELECT ""ID"" FROM ""TRANSACTION"" WHERE ""TRANSACTION_TYPE_ID"" = 1 AND ""TRANSACTION_DATE"" = '{1}'); ", lotid, tdate.ToString("yyyy-MM-dd"));
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                Log.InsertEntity(1, "Success", id.ToString(), "ItemLot", "Insert");
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionIDByItemLot", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int DeleteByTransactionId(int id, int lotid)
        {
            try
            {
                string query = @"DELETE FROM ""TRANSACTION_LINES"" WHERE ""TRANSACTION_ID"" = " + id + @" AND ""ITEM_LOT_ID"" = " + lotid;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<TransactionLines> GetPagedStockCountsByDateInterval(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = string.Format(@"SELECT ""TRANSACTION_LINES"".""ID"", ""TRANSACTION_LINES"".""TRANSACTION_ID"", ""TRANSACTION_LINES"".""ITEM_LOT_ID"", ""TRANSACTION_LINES"".""QUANTITY"", ""TRANSACTION_LINES"".""IS_ACTIVE"", ""TRANSACTION_LINES"".""ADJUSTMENT_ID"", ""TRANSACTION_LINES"".""NOTES"", ""TRANSACTION_DATE"" AS ""MODIFIED_ON"", ""TRANSACTION_LINES"".""MODIFIED_BY""
                                            FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID""
                                            WHERE ""TRANSACTION"".""TRANSACTION_TYPE_ID"" = 3 AND ""ADJUSTMENT_ID"" = 0 {0}  
                                            ORDER BY ""TRANSACTION"".""TRANSACTION_DATE"", ""TRANSACTION_LINES"".""ID"" DESC
                                            OFFSET {1} LIMIT {2};", where, startRowIndex, maximumRows);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetPagedStockCountsByDateInterval", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<TransactionLines> GetStockCountsByDateInterval(string where)
        {
            try
            {
                string query = string.Format(@"SELECT ""TRANSACTION_LINES"".""ID"", ""TRANSACTION_LINES"".""TRANSACTION_ID"", ""TRANSACTION_LINES"".""ITEM_LOT_ID"", ""TRANSACTION_LINES"".""QUANTITY"", ""TRANSACTION_LINES"".""IS_ACTIVE"", ""TRANSACTION_LINES"".""ADJUSTMENT_ID"", ""TRANSACTION_LINES"".""NOTES"", ""TRANSACTION_DATE"" AS ""MODIFIED_ON"", ""TRANSACTION_LINES"".""MODIFIED_BY""
                                            FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID""
                                            WHERE ""TRANSACTION"".""TRANSACTION_TYPE_ID"" = 3 AND ""ADJUSTMENT_ID"" = 0  {0}
                                            ORDER BY ""TRANSACTION"".""TRANSACTION_DATE"", ""TRANSACTION_LINES"".""ID"" DESC;", where);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetStockCountsByDateInterval", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetStockCountsByDateIntervalForGraph(string where)
        {
            try
            {
                string query = string.Format(@"SELECT ""TRANSACTION_LINES"".""ID"", ""TRANSACTION_LINES"".""TRANSACTION_ID"", ""TRANSACTION_LINES"".""ITEM_LOT_ID"", ""TRANSACTION_LINES"".""QUANTITY"", ""TRANSACTION_LINES"".""IS_ACTIVE"", ""TRANSACTION_LINES"".""ADJUSTMENT_ID"", ""TRANSACTION_LINES"".""NOTES"", ""TRANSACTION_DATE"" AS ""MODIFIED_ON"", ""TRANSACTION_LINES"".""MODIFIED_BY""
                                            FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID""
                                            WHERE ""TRANSACTION"".""TRANSACTION_TYPE_ID"" = 3 AND ""ADJUSTMENT_ID"" = 0  {0}
                                            ORDER BY ""TRANSACTION"".""TRANSACTION_DATE"", ""TRANSACTION_LINES"".""ID"" DESC;", where);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetStockCountsByDateInterval", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<TransactionLines> GetPagedAdjustmentsByDateInterval(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = string.Format(@"SELECT ""TRANSACTION_LINES"".""ID"", ""TRANSACTION_LINES"".""TRANSACTION_ID"", ""TRANSACTION_LINES"".""ITEM_LOT_ID"", ""TRANSACTION_LINES"".""QUANTITY"", ""TRANSACTION_LINES"".""IS_ACTIVE"", ""TRANSACTION_LINES"".""ADJUSTMENT_ID"", ""TRANSACTION_LINES"".""NOTES"", ""TRANSACTION_DATE"" AS ""MODIFIED_ON"", ""TRANSACTION_LINES"".""MODIFIED_BY""
                                            FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID""
                                            WHERE ""TRANSACTION"".""TRANSACTION_TYPE_ID"" = 4 {0} 
                                            ORDER BY ""TRANSACTION"".""TRANSACTION_DATE"", ""TRANSACTION_LINES"".""ID"" DESC
                                            OFFSET {1} LIMIT {2};", where, startRowIndex, maximumRows);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetPagedAdjustmentsByDateInterval", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int GetCountStockCountsByDateInterval(string where)
        {
            try
            {
                string query = string.Format(@"SELECT count(*)
                                            FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID""
                                            WHERE ""TRANSACTION"".""TRANSACTION_TYPE_ID"" = 3 AND ""ADJUSTMENT_ID"" = 0 {0} ;", where);
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetCountStockCountsByDateInterval", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int GetCountAdjustmentsByDateInterval(string where)
        {
            try
            {
                string query = string.Format(@"SELECT count(*)
                                            FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID""
                                            WHERE ""TRANSACTION"".""TRANSACTION_TYPE_ID"" = 4 {0} ;", where);
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetCountAdjustmentsByDateInterval", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<TransactionLines> GetTransactionLinesByTransactionId(int transactionId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""TRANSACTION_LINES"" WHERE ""TRANSACTION_ID"" = {0};", transactionId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesByTransactionId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<TransactionLines> GetTransactionLinesByTransactionIdNotWastage(int transactionId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""TRANSACTION_LINES"" WHERE ""TRANSACTION_ID"" = {0} AND ""ADJUSTMENT_ID"" = 0 ;", transactionId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesByTransactionIdNotWastage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static TransactionLines GetTransactionLinesByTransactionIdLotId(int transactionId, int lotId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""TRANSACTION_LINES"" WHERE ""TRANSACTION_ID"" = {0} AND ""ITEM_LOT_ID"" = {1} ;", transactionId, lotId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesByTransactionId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static TransactionLines GetTransactionLinesByTransactionIdLotIdNotWastage(int transactionId, int lotId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""TRANSACTION_LINES"" WHERE ""TRANSACTION_ID"" = {0} AND ""ITEM_LOT_ID"" = {1} AND ""ADJUSTMENT_ID"" = 0  ;", transactionId, lotId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesByTransactionIdLotIdNotWastage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static TransactionLines GetTransactionLinesByTransactionIdLotIdWastage(int transactionId, int lotId)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""TRANSACTION_LINES"" WHERE ""TRANSACTION_ID"" = {0} AND ""ITEM_LOT_ID"" = {1} AND ""ADJUSTMENT_ID"" = 99  ;", transactionId, lotId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetTransactionLinesByTransactionIdLotIdNotWastage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<TransactionLines> GetLotTracking(ref int maximumRows, ref int startRowIndex, int hfId, int itemlotid)
        {
            string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);
            try
            {
                string query = String.Format(@"SELECT DISTINCT ""TRANSACTION_LINES"".""ID"", ""TRANSACTION_LINES"".""TRANSACTION_ID"", ""TRANSACTION_LINES"".""ITEM_LOT_ID"", ""TRANSACTION_LINES"".""QUANTITY"", ""TRANSACTION_LINES"".""IS_ACTIVE"", ""TRANSACTION_LINES"".""ADJUSTMENT_ID"", ""TRANSACTION_LINES"".""NOTES"", ""TRANSACTION_DATE"" AS ""MODIFIED_ON"", ""TRANSACTION_LINES"".""MODIFIED_BY"", ""SENDER_ID""
                                           FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID"" inner join ""HEALTH_FACILITY"" ON  (""TRANSACTION"".""SENDER_ID"" = ""HEALTH_FACILITY"".""ID"" OR ""TRANSACTION"".""RECEIVER_ID"" = ""HEALTH_FACILITY"".""ID"")
                                           WHERE ""TRANSACTION_LINES"".""IS_ACTIVE"" = true AND ""TRANSACTION_TYPE_ID"" in (1,2) AND ""ITEM_LOT_ID"" = {1} AND (""SENDER_ID"" in ({0}) OR ""RECEIVER_ID"" in ({0})) ORDER BY ""TRANSACTION_DATE"", ""SENDER_ID""  OFFSET {2} LIMIT {3} ;", s, itemlotid, startRowIndex, maximumRows);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetTransactionLinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetPagedTransactionLinesList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountLotTracking(int hfId, int itemlotid)
        {
            try
            {
                string query = String.Format(@"SELECT COUNT(*)
                                           FROM ""TRANSACTION"" inner join ""TRANSACTION_LINES"" on ""TRANSACTION"".""ID"" = ""TRANSACTION_LINES"".""TRANSACTION_ID"" inner join ""HEALTH_FACILITY"" ON  (""TRANSACTION"".""SENDER_ID"" = ""HEALTH_FACILITY"".""ID"" OR ""TRANSACTION"".""RECEIVER_ID"" = ""HEALTH_FACILITY"".""ID"")
                                           WHERE ""TRANSACTION_LINES"".""IS_ACTIVE"" = true AND ""TRANSACTION_TYPE_ID"" in (1,2) AND ""ITEM_LOT_ID"" = {1} AND (""SENDER_ID"" = {0} OR ""RECEIVER_ID"" = {0} or ""PARENT_ID"" = {0})  ;", hfId, itemlotid);

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetCountTransactionLinesList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetConsumption(int year, int hfId)
        {
            try
            {
                string query = String.Format(@"Select * from crosstab 
                        ('SELECT ""ITEM"".""NAME"",   
	                    EXTRACT(MONTH FROM ""TRANSACTION"".""TRANSACTION_DATE"")::INTEGER,  
	                    CAST(Sum(""TRANSACTION_LINES"".""QUANTITY"") AS TEXT)
                            FROM ""ITEM"" 
                            LEFT OUTER JOIN ""ITEM_LOT"" ON ""ITEM_LOT"".""ITEM_ID"" = ""ITEM"".""ID"" 
	                    LEFT OUTER JOIN ""TRANSACTION_LINES"" ON ""TRANSACTION_LINES"".""ITEM_LOT_ID"" = ""ITEM_LOT"".""ID"" 
	                    LEFT OUTER JOIN ""TRANSACTION"" ON ""TRANSACTION_LINES"".""TRANSACTION_ID"" = ""TRANSACTION"".""ID"" 
                                WHERE 
                        ""TRANSACTION"".""STATUS"" = 1 AND ""TRANSACTION"".""TRANSACTION_TYPE_ID"" in (2,4,5)  AND 
                        ""ITEM_LOT"".""IS_DILUENT"" = false AND 
                        ""TRANSACTION"".""SENDER_ID"" = {1} AND EXTRACT(YEAR FROM ""TRANSACTION"".""TRANSACTION_DATE"")::INTEGER= {0} 
	                    GROUP BY ""ITEM"".""NAME"", EXTRACT(MONTH FROM ""TRANSACTION"".""TRANSACTION_DATE"")::INTEGER ORDER BY 1,2', 
	                        'select m from generate_series(1,12) m') 
	                    AS kolonat(""Item"" text, 
	                    ""JAN"" int, ""FEB"" int, ""MAR"" int, ""APR"" int, 
	                    ""MAY"" int, ""JUN"" int, ""JUL"" int, ""AUG"" int, 
	                    ""SEP"" int, ""OCT"" int, ""NOV"" int, ""DEC"" int); ", year, hfId);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetConsumption", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetConsumption(int year, int hfId, int itemId, string months)
        {
            try
            {
                string query = String.Format(@"Select * from crosstab 
                        ('SELECT ""ITEM"".""NAME"",   
	                    EXTRACT(MONTH FROM ""TRANSACTION"".""TRANSACTION_DATE"")::INTEGER,  
	                    CAST(Sum(""TRANSACTION_LINES"".""QUANTITY"") AS TEXT)
                            FROM ""ITEM"" 
                            LEFT OUTER JOIN ""ITEM_LOT"" ON ""ITEM_LOT"".""ITEM_ID"" = ""ITEM"".""ID"" 
	                    LEFT OUTER JOIN ""TRANSACTION_LINES"" ON ""TRANSACTION_LINES"".""ITEM_LOT_ID"" = ""ITEM_LOT"".""ID"" 
	                    LEFT OUTER JOIN ""TRANSACTION"" ON ""TRANSACTION_LINES"".""TRANSACTION_ID"" = ""TRANSACTION"".""ID"" 
                                WHERE 
                        ""TRANSACTION"".""STATUS"" = 1 AND ""TRANSACTION"".""TRANSACTION_TYPE_ID"" in (2,4,5)  AND 
                        ""ITEM_LOT"".""IS_DILUENT"" = false AND ""ITEM"".""ID"" = {2} and
                        ""TRANSACTION"".""SENDER_ID"" = {1} AND EXTRACT(YEAR FROM ""TRANSACTION"".""TRANSACTION_DATE"")::INTEGER= {0} 
	                    GROUP BY ""ITEM"".""NAME"", EXTRACT(MONTH FROM ""TRANSACTION"".""TRANSACTION_DATE"")::INTEGER ORDER BY 1,2', 
	                        'select m from generate_series(1,12) m') 
	                    AS kolonat(""Item"" text, {3}); ", year, hfId, itemId, months);

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetConsumption", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetClosedVialWastage(int languageId, string where)
        {
            try
            {
                string funcQueryHelper = string.Format("SELECT create_v_adjustment();");
                DataTable dtfHelper = DBManager.ExecuteReaderCommand(funcQueryHelper, CommandType.Text, null);

                string query = String.Format("Select * from v_Adjustments Where 1=1 {0} ", where);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransactionLines", "GetClosedVialWastage", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                return null;
            }
        }

    }
}