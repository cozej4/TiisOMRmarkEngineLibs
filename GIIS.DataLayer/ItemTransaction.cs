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
    public partial class ItemTransaction
    {

        #region Properties
        public Int32 Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Gtin { get; set; }
        public string HealthFacilityCode { get; set; }
        public string GtinLot { get; set; }
        public string RefId { get; set; }
        public Int32 RefIdNum { get; set; }
        public Int32 TransactionTypeId { get; set; }
        public double TransactionQtyInBaseUom { get; set; }
        public string Notes { get; set; }
        public Int32 ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32? AdjustmentId { get; set; }
        public AdjustmentReason AdjustmentObject
        {
            get
            {
                if (this.AdjustmentId > 0)
                    return AdjustmentReason.GetAdjustmentReasonById(this.AdjustmentId);
                else
                    return null;
            }
        }
        public ItemManufacturer GtinObject
        {
            get
            {
                if (this.Gtin == "")
                    return null;
                else
                    return ItemManufacturer.GetItemManufacturerByGtin(this.Gtin);

            }
        }
        public HealthFacility HealthFacilityCodeObject
        {
            get
            {
                if (this.HealthFacilityCode == "")
                    return null;
                else
                    return HealthFacility.GetHealthFacilityByCode(this.HealthFacilityCode);
            }
        }
        public TransactionType TransactionTypeObject
        {
            get
            {
                if (this.TransactionTypeId > 0)
                    return TransactionType.GetTransactionTypeById(this.TransactionTypeId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<ItemTransaction> GetItemTransactionList()
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_TRANSACTION"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetItemTransactionAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "GetItemTransactionList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemTransactionForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ITEM_TRANSACTION"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "GetItemTransactionForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }




        public static ItemTransaction GetItemTransactionById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_TRANSACTION"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemTransaction", i.ToString(), 4, DateTime.Now, 1);
                return GetItemTransactionAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "GetItemTransactionById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(ItemTransaction o)
        {
            try
            {
                string query = @"INSERT INTO ""ITEM_TRANSACTION"" (""TRANSACTION_DATE"", ""GTIN"", ""HEALTH_FACILITY_CODE"", ""GTIN_LOT"", ""REF_ID"", ""REF_ID_NUM"", ""TRANSACTION_TYPE_ID"", ""TRANSACTION_QTY_IN_BASE_UOM"", ""NOTES"", ""MODIFIED_BY"", ""MODIFIED_ON"", ""ADJUSTMENT_ID"") VALUES (@TransactionDate, @Gtin, @HealthFacilityCode, @GtinLot, @RefId, @RefIdNum, @TransactionTypeId, @TransactionQtyInBaseUom, @Notes, @ModifiedBy, @ModifiedOn, @AdjustmentId) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@TransactionDate", DbType.Date)  { Value = (object)o.TransactionDate ?? DBNull.Value },
                    new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
                    new NpgsqlParameter("@HealthFacilityCode", DbType.String)  { Value = o.HealthFacilityCode },
                    new NpgsqlParameter("@GtinLot", DbType.String)  { Value = (object)o.GtinLot ?? DBNull.Value },
                    new NpgsqlParameter("@RefId", DbType.String)  { Value = (object)o.RefId ?? DBNull.Value },
                    new NpgsqlParameter("@RefIdNum", DbType.Int32)  { Value = (object)o.RefIdNum ?? DBNull.Value },
                    new NpgsqlParameter("@TransactionTypeId", DbType.Int32)  { Value = (object)o.TransactionTypeId ?? DBNull.Value },
                    new NpgsqlParameter("@TransactionQtyInBaseUom", DbType.Double)  { Value = (object)o.TransactionQtyInBaseUom ?? DBNull.Value },
                    new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
                    new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = (object)o.ModifiedBy ?? DBNull.Value },
                    new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = (object)o.ModifiedOn ?? DBNull.Value },
                    new NpgsqlParameter("@AdjustmentId", DbType.Int32)  { Value = (object)o.AdjustmentId ?? DBNull.Value }
                };

                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemTransaction", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(ItemTransaction o)
        {
            try
            {
                string query = @"UPDATE ""ITEM_TRANSACTION"" SET ""ID"" = @Id, ""TRANSACTION_DATE"" = @TransactionDate, ""GTIN"" = @Gtin, ""HEALTH_FACILITY_CODE"" = @HealthFacilityCode, ""GTIN_LOT"" = @GtinLot, ""REF_ID"" = @RefId, ""REF_ID_NUM"" = @RefIdNum, ""TRANSACTION_TYPE_ID"" = @TransactionTypeId, ""TRANSACTION_QTY_IN_BASE_UOM"" = @TransactionQtyInBaseUom, ""NOTES"" = @Notes, ""MODIFIED_BY"" = @ModifiedBy, ""MODIFIED_ON"" = @ModifiedOn, ""ADJUSTMENT_ID"" = @AdjustmentId WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@TransactionDate", DbType.Date)  { Value = (object)o.TransactionDate ?? DBNull.Value },
new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
new NpgsqlParameter("@HealthFacilityCode", DbType.String)  { Value = o.HealthFacilityCode },
new NpgsqlParameter("@GtinLot", DbType.String)  { Value = (object)o.GtinLot ?? DBNull.Value },
new NpgsqlParameter("@RefId", DbType.String)  { Value = (object)o.RefId ?? DBNull.Value },
new NpgsqlParameter("@RefIdNum", DbType.Int32)  { Value = (object)o.RefIdNum ?? DBNull.Value },
new NpgsqlParameter("@TransactionTypeId", DbType.Int32)  { Value = (object)o.TransactionTypeId ?? DBNull.Value },
new NpgsqlParameter("@TransactionQtyInBaseUom", DbType.Double)  { Value = (object)o.TransactionQtyInBaseUom ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = (object)o.ModifiedBy ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = (object)o.ModifiedOn ?? DBNull.Value },
new NpgsqlParameter("@AdjustmentId", DbType.Int32)  { Value = (object)o.AdjustmentId ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemTransaction", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""ITEM_TRANSACTION"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemTransaction", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemTransaction", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static ItemTransaction GetItemTransactionAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ItemTransaction o = new ItemTransaction();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.TransactionDate = Helper.ConvertToDate(row["TRANSACTION_DATE"]);
                    o.Gtin = row["GTIN"].ToString();
                    o.HealthFacilityCode = row["HEALTH_FACILITY_CODE"].ToString();
                    o.GtinLot = row["GTIN_LOT"].ToString();
                    o.RefId = row["REF_ID"].ToString();
                    o.RefIdNum = Helper.ConvertToInt(row["REF_ID_NUM"]);
                    o.TransactionTypeId = Helper.ConvertToInt(row["TRANSACTION_TYPE_ID"]);
                    o.TransactionQtyInBaseUom = Helper.ConvertToDecimal(row["TRANSACTION_QTY_IN_BASE_UOM"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.AdjustmentId = Helper.ConvertToInt(row["ADJUSTMENT_ID"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ItemTransaction", "GetItemTransactionAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<ItemTransaction> GetItemTransactionAsList(DataTable dt)
        {
            List<ItemTransaction> oList = new List<ItemTransaction>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ItemTransaction o = new ItemTransaction();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.TransactionDate = Helper.ConvertToDate(row["TRANSACTION_DATE"]);
                    o.Gtin = row["GTIN"].ToString();
                    o.HealthFacilityCode = row["HEALTH_FACILITY_CODE"].ToString();
                    o.GtinLot = row["GTIN_LOT"].ToString();
                    o.RefId = row["REF_ID"].ToString();
                    o.RefIdNum = Helper.ConvertToInt(row["REF_ID_NUM"]);
                    o.TransactionTypeId = Helper.ConvertToInt(row["TRANSACTION_TYPE_ID"]);
                    o.TransactionQtyInBaseUom = Helper.ConvertToDecimal(row["TRANSACTION_QTY_IN_BASE_UOM"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.AdjustmentId = Helper.ConvertToInt(row["ADJUSTMENT_ID"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ItemTransaction", "GetItemTransactionAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}