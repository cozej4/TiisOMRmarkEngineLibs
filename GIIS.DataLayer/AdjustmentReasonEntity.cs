using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    public partial class AdjustmentReason
    {
        public static List<AdjustmentReason> GetPagedAdjustmentReasonList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""ADJUSTMENT_REASON"" WHERE " + where + @" ORDER BY ""NAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetAdjustmentReasonAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "GetPagedAdjustmentReasonList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountAdjustmentReasonList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""ADJUSTMENT_REASON"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "GetCountAdjustmentReasonList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<AdjustmentReason> GetRealAdjustmentReasonList()
        {
            try
            {
                string query = @"SELECT * FROM ""ADJUSTMENT_REASON"" where ""IS_ACTIVE"" = true AND ""ID"" <> 99 ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetAdjustmentReasonAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "GetRealAdjustmentReasonList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
