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
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class GtinHfStockPolicy
    {
        public static GtinHfStockPolicy GetGtinHfStockPolicyByHealthFacilityCodeAndGtin(string hfId, string gtin)
        {
            try
            {
                string query = @"SELECT * FROM ""GTIN_HF_STOCK_POLICY"" WHERE ""HEALTH_FACILITY_CODE"" = @ParamValue1 AND ""GTIN"" like @ParamValue2 ";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>() { 
                    new NpgsqlParameter("@ParamValue1", DbType.String) { Value = hfId } ,
                    new NpgsqlParameter("@ParamValue2", DbType.String) { Value = gtin + "%" }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("GtinHfStockPolicy", string.Format("RecordId: {0}", hfId+gtin), 4, DateTime.Now, 1);
                return GetGtinHfStockPolicyAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyByHealthFacilityCodeAndGtin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<GtinHfStockPolicy> GetGtinHfStockPolicyList(ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT ""GTIN_HF_STOCK_POLICY"".* FROM ""GTIN_HF_STOCK_POLICY"" join ""ITEM_MANUFACTURER"" using (""GTIN"") where ""IS_ACTIVE"" = true OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);

                return GetGtinHfStockPolicyAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "GetGtinHfStockPolicyList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountGtinHfStockPolicyList()
        {
            try
            {
                string query = @"SELECT count(*) FROM ""GTIN_HF_STOCK_POLICY"" join ""ITEM_MANUFACTURER"" using (""GTIN"") where ""IS_ACTIVE"" = true ;";

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("GtinHfStockPolicy", "GetCountGtinHfStockPolicyList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
