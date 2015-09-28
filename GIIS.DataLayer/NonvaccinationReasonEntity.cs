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
using System.Data;
using System.Text;

namespace GIIS.DataLayer
{
   public partial class NonvaccinationReason
    {
        public static List<NonvaccinationReason> GetPagedNonvaccinationReasonList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""NONVACCINATION_REASON"" WHERE " + where + @" ORDER BY ""NAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetNonvaccinationReasonAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "GetPagedNonvaccinationReasonList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountNonvaccinationReasonList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""NONVACCINATION_REASON"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "GetCountNonvaccinationReasonList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
