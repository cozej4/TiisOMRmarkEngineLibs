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
    public partial class ItemLot
    {

        #region Properties
        public string Gtin { get; set; }
        public string LotNumber { get; set; }
        public Int32 ItemId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Notes { get; set; }
        public int Id { get; set; }
        public Boolean IsActive { get; set; }
        public ItemManufacturer GtinObject
        {
            get
            {
                if (this.Gtin == "")
                    return ItemManufacturer.GetItemManufacturerByGtin(this.Gtin);
                else
                    return null;
            }
        }
        public Item ItemObject
        {
            get
            {
                if (this.ItemId > 0)
                    return Item.GetItemById(this.ItemId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<ItemLot> GetItemLotList()
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_LOT"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetItemLotAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLotList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemLotForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ITEM_LOT"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLotForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static ItemLot GetItemLotByGtin(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_LOT"" WHERE ""GTIN"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetItemLotAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLotByGtin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ItemLot GetItemLotByLotNumber(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_LOT"" WHERE ""LOT_NUMBER"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetItemLotAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLotByLotNumber", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(ItemLot o)
        {
            try
            {
                string query = @"INSERT INTO ""ITEM_LOT"" (""GTIN"", ""LOT_NUMBER"", ""ITEM_ID"", ""EXPIRE_DATE"", ""NOTES"") VALUES (@Gtin, @LotNumber, @ItemId, @ExpireDate, @Notes) returning ""GTIN"" || ""LOT_NUMBER"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
new NpgsqlParameter("@LotNumber", DbType.String)  { Value = o.LotNumber },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@ExpireDate", DbType.Date)  { Value = o.ExpireDate},
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }
};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot",  o.Gtin + o.LotNumber, 1, DateTime.Now, 1);
                return 1;// int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(ItemLot o)
        {
            try
            {
                string query = @"UPDATE ""ITEM_LOT"" SET ""ITEM_ID"" = @ItemId, ""EXPIRE_DATE"" = @ExpireDate, ""NOTES"" = @Notes WHERE ""GTIN"" = @Gtin AND ""LOT_NUMBER"" = @LotNumber ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
new NpgsqlParameter("@LotNumber", DbType.String)  { Value = o.LotNumber },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@ExpireDate", DbType.Date)  { Value = o.ExpireDate },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot",  o.Gtin + o.LotNumber, 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(string gtin, string lotNumber)
        {
            try
            {
                string query = @"DELETE FROM ""ITEM_LOT"" WHERE ""GTIN"" = @Gtin AND ""LOT_NUMBER"" = @LotNumber ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Gtin", DbType.String)  { Value = gtin },
new NpgsqlParameter("@LotNumber", DbType.String)  { Value = lotNumber }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot", string.Format("RecordId: {0}", gtin + lotNumber), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""ITEM_LOT"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static ItemLot GetItemLotAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ItemLot o = new ItemLot();
                    o.Gtin = row["GTIN"].ToString();
                    o.LotNumber = row["LOT_NUMBER"].ToString();
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.ExpireDate = Helper.ConvertToDate(row["EXPIRE_DATE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ItemLot", "GetItemLotAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<ItemLot> GetItemLotAsList(DataTable dt)
        {
            List<ItemLot> oList = new List<ItemLot>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ItemLot o = new ItemLot();
                    o.Gtin = row["GTIN"].ToString();
                    o.LotNumber = row["LOT_NUMBER"].ToString();
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.ExpireDate = Helper.ConvertToDate(row["EXPIRE_DATE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ItemLot", "GetItemLotAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}