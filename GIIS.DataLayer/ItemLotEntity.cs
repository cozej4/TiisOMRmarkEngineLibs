using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GIIS.DataLayer
{
    partial class ItemLot
    {
        public static ItemLot GetItemLotById(int i)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_LOT"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot", i.ToString(), 4, DateTime.Now, 1);
                return GetItemLotAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLotById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int GetItemLotByDoseId(int i)
        {
            try
            {
                string query = string.Format(@"Select ""ITEM_LOT"".""ID"" from ""ITEM_LOT"" join ""SCHEDULED_VACCINATION"" using (""ITEM_ID"") join ""DOSE"" on  ""SCHEDULED_VACCINATION_ID"" = ""SCHEDULED_VACCINATION"".""ID"" where ""DOSE"".""ID"" =  @ParamValue ");
                 List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
                };
                 object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                 return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLotByDoseId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    
        //stock count
        public static DataTable GetItemLotsForStockCount(string gtin, string hfId)
        {
            try
            {
                string query = string.Format(@"SELECT  '-----' as ""LOT_NUMBER"", now() as ""EXPIRE_DATE"" " +
                                            "UNION" +
                                            @" SELECT  ""LOT_NUMBER"", ""EXPIRE_DATE"" FROM ""ITEM_LOT"" join ""HEALTH_FACILITY_BALANCE"" using (""LOT_NUMBER"") WHERE ""ITEM_LOT"".""GTIN"" = @gtin  AND ""HEALTH_FACILITY_CODE"" = @hfId AND ""EXPIRE_DATE"" >= @date AND ""ITEM_LOT"".""IS_ACTIVE"" = true ORDER BY ""EXPIRE_DATE"" ");
               List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@gtin", DbType.String) { Value = gtin },
                    new NpgsqlParameter("@hfId", DbType.String) { Value = hfId },
                    new NpgsqlParameter("@date", DbType.Date) { Value = DateTime.Today.Date }
                   
                };
               
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemLotsForStockCount", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetItemLots(string gtin) //transfer orders
        {
            try
            {
                string query = string.Format(@"SELECT '-----' as ""LOT_NUMBER"", now() as ""EXPIRE_DATE""  UNION SELECT ""LOT_NUMBER"", ""EXPIRE_DATE"" FROM ""ITEM_LOT"" WHERE ""GTIN"" = @gtin AND ""EXPIRE_DATE"" > @date AND ""ITEM_LOT"".""IS_ACTIVE"" = true  ORDER BY ""EXPIRE_DATE"";");
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@gtin", DbType.String) { Value = gtin },
                    new NpgsqlParameter("@date", DbType.Date) { Value = DateTime.Today.Date }
                   
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLots", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
              
        //Item Lot New
        public static ItemLot GetItemLotByGtinAndLotNo(string gtin, string lotNumber)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_LOT"" WHERE ""GTIN"" = @ParamValue1 AND ""LOT_NUMBER"" = @ParamValue2 ";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>() { 
                    new NpgsqlParameter("@ParamValue1", DbType.String) { Value = gtin } ,
                    new NpgsqlParameter("@ParamValue2", DbType.String) { Value = lotNumber }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemLot", string.Format("RecordId: {0}", gtin + lotNumber), 4, DateTime.Now, 1);
                return GetItemLotAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetItemLotByGtinAndLotNo", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<ItemLot> GetPagedItemLotList(int itemId, string gtin, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_LOT"" join ""ITEM"" on ""ITEM_LOT"".""ITEM_ID"" = ""ITEM"".""ID""  WHERE 1 = 1  "
                                + @" AND ( ""ITEM_ID"" = @ItemId OR @ItemId is null or @ItemId = '-1' )"
                                + @" AND ( ""GTIN"" = @Gtin OR @Gtin is null or @Gtin = '' )"
                                + @" ORDER BY ""ITEM"".""CODE"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ItemId", DbType.Int32) { Value = itemId },
                    new NpgsqlParameter("@Gtin", DbType.String) { Value = gtin },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetItemLotAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetPagedItemLotList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int GetCountItemLotList(int itemId, string gtin)
        {
            try
            {
                string query = @"SELECT count(*) FROM ""ITEM_LOT"" WHERE 1 = 1  "
                                + @" AND ""ITEM_ID"" = @ItemId OR @ItemId is null or @ItemId = '-1'  "
                                + @" AND ""GTIN"" = @Gtin OR @Gtin is null or @Gtin = '' ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                     new NpgsqlParameter("@ItemId", DbType.Int32) { Value = itemId },
                     new NpgsqlParameter("@Gtin", DbType.String) { Value = gtin }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemLot", "GetCountItemLotList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        } 
        public static DataTable GetItemLotForList(int itemId, string hfId) // Vaccination
        {
            try
            {

                string query = string.Format(@"SELECT -1 as  ""ID"", '-----' as ""LOT_NUMBER"", now() as ""EXPIRE_DATE"" UNION Select -2 as ""ID"", 'I don''t know the lot' AS ""LOT_NUMBER"", now() as ""EXPIRE_DATE"" " +
                                            "UNION" +
                                            @" SELECT ""ITEM_LOT"".""ID"", ""LOT_NUMBER"", ""EXPIRE_DATE"" FROM ""ITEM_LOT"" join ""HEALTH_FACILITY_BALANCE"" using (""LOT_NUMBER"") join ""ITEM_MANUFACTURER"" on ""HEALTH_FACILITY_BALANCE"".""GTIN"" = ""ITEM_MANUFACTURER"".""GTIN"" WHERE ""ITEM_LOT"".""ITEM_ID"" = {0}  AND ""HEALTH_FACILITY_CODE"" = '{1}' AND  ""EXPIRE_DATE"" > '{2}' and ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true AND ""ITEM_LOT"".""IS_ACTIVE"" = true ORDER BY ""EXPIRE_DATE"" " +
                                            "", itemId, hfId, DateTime.Today.Date.ToString("yyyy-MM-dd"));
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemLotForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
