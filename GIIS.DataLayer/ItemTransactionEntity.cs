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
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class ItemTransaction
    {
        public static List<ItemTransaction> GetItemTransactionListByHealthFacilityCode(string hfcode)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_TRANSACTION"" where ""HEALTH_FACILITY_CODE"" = @hfcode ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@hfcode", DbType.String) { Value = hfcode }
                    };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetItemTransactionAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "GetItemTransactionList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
