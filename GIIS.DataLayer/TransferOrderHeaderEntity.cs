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
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.DataLayer
{
    public partial class TransferOrderHeader
    {
        public static DataTable GetTransferStatus(string OrderFacilityFrom)
        {
            try
            {
                string query = @"SELECT case when ""ORDER_STATUS"" = 0 then 'Requested'
                                            when ""ORDER_STATUS"" = 1 then 'Released'
                                            when ""ORDER_STATUS"" = 2 then 'Packed'
                                            when ""ORDER_STATUS"" = 3 then 'Shipped'
                                            when ""ORDER_STATUS"" = -1 then 'Canceled'
                                            end ""Order Status"",
                                            COUNT(*) as ""Total"" FROM ""TRANSFER_ORDER_HEADER"" WHERE "
                + @" ( ""ORDER_FACILITY_FROM"" = @OrderFacilityFrom ) GROUP BY ""ORDER_STATUS"" ";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@OrderFacilityFrom", DbType.String) { Value = OrderFacilityFrom }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetTransferStatus", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<TransferOrderHeader> GetPagedTransferOrderHeaderList(string OrderFacilityFrom, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT ""ORDER_NUM"", ""ORDER_SCHED_REPLENISH_DATE"", ""ORDER_FACILITY_FROM"", 
       ""ORDER_FACILITY_TO"", ""ORDER_CARRIER"", case when ""ORDER_STATUS"" = 0 then 'Requested'
                                            when ""ORDER_STATUS"" = 1 then 'Released'
                                            when ""ORDER_STATUS"" = 2 then 'Packed'
                                            when ""ORDER_STATUS"" = 3 then 'Shipped'
                                            when ""ORDER_STATUS"" = -1 then 'Canceled'
                                            end ""ORDER_STATUS"", ""MODIFIED_ON"", 
       ""MODIFIED_BY"", ""REV_NUM"" FROM ""TRANSFER_ORDER_HEADER"" WHERE "
                + @" ( ""ORDER_FACILITY_FROM"" = @OrderFacilityFrom ) ORDER BY ""ORDER_NUM"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@OrderFacilityFrom", DbType.String) { Value = OrderFacilityFrom },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetTransferOrderHeaderAsListForStatus(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetPagedTransferOrderHeaderList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<TransferOrderHeader> GetPagedTransferOrderHeaderList(string OrderFacilityFrom, string orderStatus, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                int status = 999;
                if (!String.IsNullOrEmpty(orderStatus))
                {
                    switch (orderStatus)
                    {
                        case "Requested":
                            status = 0;
                            break;
                        case "Released":
                            status = 1;
                            break;
                        case "Packed":
                            status = 2;
                            break;
                        case "Shipped":
                            status = 3;
                            break;
                        case "Canceled":
                            status = -1;
                            break;
                    }
                }

                string query = @"SELECT ""ORDER_NUM"", ""ORDER_SCHED_REPLENISH_DATE"", ""ORDER_FACILITY_FROM"", 
       ""ORDER_FACILITY_TO"", ""ORDER_CARRIER"", case when ""ORDER_STATUS"" = 0 then 'Requested'
                                            when ""ORDER_STATUS"" = 1 then 'Released'
                                            when ""ORDER_STATUS"" = 2 then 'Packed'
                                            when ""ORDER_STATUS"" = 3 then 'Shipped'
                                            when ""ORDER_STATUS"" = -1 then 'Canceled'
                                            end ""ORDER_STATUS"", ""MODIFIED_ON"", 
       ""MODIFIED_BY"", ""REV_NUM"" FROM ""TRANSFER_ORDER_HEADER"" WHERE "
                + @" ( ""ORDER_FACILITY_FROM"" = @OrderFacilityFrom ) "
                + @" AND ( ""ORDER_STATUS"" = @OrderStatus OR @OrderStatus IS NULL OR @OrderStatus = 999)  ORDER BY ""ORDER_NUM"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@OrderFacilityFrom", DbType.String) { Value = OrderFacilityFrom },
                    new NpgsqlParameter("@OrderStatus", DbType.Int32) { Value = status },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetTransferOrderHeaderAsListForStatus(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetPagedTransferOrderHeaderList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int GetCountTransferOrderHeaderList(string OrderFacilityFrom)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""TRANSFER_ORDER_HEADER"" WHERE ""ORDER_FACILITY_FROM"" = @OrderFacilityFrom ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@OrderFacilityFrom", DbType.String) { Value = OrderFacilityFrom }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetCountTransferOrderHeaderList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int GetCountTransferOrderHeaderList(string OrderFacilityFrom, string orderStatus)
        {
            try
            {
                int status = 999;
                if (!String.IsNullOrEmpty(orderStatus))
                {
                    switch (orderStatus)
                    {
                        case "Released":
                            status = 1;
                            break;
                        case "Packed":
                            status = 2;
                            break;
                        case "Shipped":
                            status = 3;
                            break;
                        case "Canceled":
                            status = -1;
                            break;
                    }
                }
                string query = @"SELECT COUNT(*) FROM ""TRANSFER_ORDER_HEADER"" WHERE ""ORDER_FACILITY_FROM"" = @OrderFacilityFrom AND ( ""ORDER_STATUS"" = @OrderStatus OR @OrderStatus IS NULL OR @OrderStatus = 999) ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@OrderFacilityFrom", DbType.String) { Value = OrderFacilityFrom },
                    new NpgsqlParameter("@OrderStatus", DbType.Int32) { Value = status }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("TransferOrderHeader", "GetCountTransferOrderHeaderList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
