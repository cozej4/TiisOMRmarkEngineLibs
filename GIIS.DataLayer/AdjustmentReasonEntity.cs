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
