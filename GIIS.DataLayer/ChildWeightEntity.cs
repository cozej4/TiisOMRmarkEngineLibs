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
    public partial class ChildWeight
    {        
        public static string GetChildWeight(int  childId)
        {
            try
            {
                string query = string.Format(@"select get_message_by_weight({0})", childId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                if (dt.Rows.Count >= 1)
                    return dt.Rows[0][0].ToString();

                return "";
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetChildWeight", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetWeight(int days, string gender)
        {
            try
            {
                string query = @"SELECT * FROM ""WEIGHT"" WHERE ""DAY""= " + days + @" AND ""GENDER"" = '" + gender + "'";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ChildWeight> GetChildWeightByChildId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_WEIGHT"" WHERE ""CHILD_ID"" = " + i + @" ORDER BY ""DATE"" DESC ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildWeightAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetChildWeightByChildId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<ChildWeight> GetPagedChildWeightList(ref int maximumRows, ref int startRowIndex, int id)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_WEIGHT"" WHERE ""CHILD_ID"" = " + id + @" ORDER BY ""DATE"" DESC OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildWeightAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetPagedChildWeightList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountChildWeightList(int id)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""CHILD_WEIGHT"" WHERE ""CHILD_ID""= " + id + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetCountChildWeightList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static ChildWeight GetChildWeightByChildIdAndDate(Int32 id, DateTime date)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_WEIGHT"" WHERE ""CHILD_ID"" = " + id + @" AND ""DATE"" = '" + date.ToString("yyyy-MM-dd") + @"'";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildWeightAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetChildWeightByChildIdAndDate", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
