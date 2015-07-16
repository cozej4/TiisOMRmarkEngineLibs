using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.DataLayer
{
    public partial class ItemManufacturer
    {
        public static DataTable GetItemManufacturerForList2()
        {
            try
            {
                string query = @"Select '-------' as ""GTIN"", '-1' as ""ID"", '-------' as ""CODE"" union SELECT ""GTIN"" || ' - ' || ""ITEM"".""CODE"" as ""GTIN"", ""GTIN"" AS ""ID"", ""ITEM"".""CODE"" as ""CODE""  FROM ""ITEM_MANUFACTURER"" join ""ITEM"" on ""ITEM_MANUFACTURER"".""ITEM_ID"" = ""ITEM"".""ID"" WHERE  ""ITEM_MANUFACTURER"".""IS_ACTIVE"" = true ORDER BY ""CODE"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetItemManufacturerForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetItemManufacturerByItemId(int itemId)
        {
            try
            {
                string query = @"Select  '-----' as ""GTIN"" UNION  SELECT ""GTIN""  FROM ""ITEM_MANUFACTURER"" WHERE ""IS_ACTIVE"" = true AND ""ITEM_ID"" = @ItemId ORDER BY ""GTIN"" ;";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ItemId", DbType.Int32) { Value = itemId }
                   
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetItemManufacturerForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static ItemManufacturer GetItemManufacturerByItem(int itemId)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_MANUFACTURER"" WHERE ""ITEM_ID"" = @ParamValue AND ""IS_ACTIVE"" = true ORDER BY ""GTIN"";";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = itemId }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("ItemManufacturer", s, 4, DateTime.Now, 1);
                return GetItemManufacturerAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetItemManufacturerByItem", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<ItemManufacturer> GetPagedItemManufacturerList(int itemId, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT ""ITEM_MANUFACTURER"".* FROM ""ITEM_MANUFACTURER"" join ""ITEM"" on ""ITEM_MANUFACTURER"".""ITEM_ID"" = ""ITEM"".""ID""  WHERE 1 = 1  "
                                + @" AND ""ITEM_ID"" = @ItemId OR @ItemId is null or @ItemId = '-1' "
                                + @" ORDER BY ""ITEM"".""CODE"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ItemId", DbType.Int32) { Value = itemId },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetItemManufacturerAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetPagedItemManufacturerList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int GetCountItemManufacturerList(int itemId)
        {
            try
            {
                string query = @"SELECT count(*) FROM ""ITEM_MANUFACTURER"" WHERE 1 = 1  "
                                + @" AND ""ITEM_ID"" = @ItemId OR @ItemId is null or @ItemId = '-1'  ;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                     new NpgsqlParameter("@ItemId", DbType.Int32) { Value = itemId }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetCountItemManufacturerList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetGtinByItemId(Int32 itemId)
        {
            try
            {
                string query = @"SELECT '-1' as ""ID"", '-----' as ""GTIN"" UNION SELECT ""GTIN"",""GTIN"" FROM ""ITEM_MANUFACTURER"" WHERE ""ITEM_ID"" = @ParamValue ORDER BY ""GTIN"" ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>() { 
                    new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = itemId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemManufacturer", string.Format("RecordId: {0}", itemId), 4, DateTime.Now, 1);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetGtinByItemId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetUomFromGtin(string gtin)
        {
            try
            {
                string query = @"Select ""NAME"" from ""UOM"" join ""ITEM_MANUFACTURER"" on ""ITEM_MANUFACTURER"".""BASE_UOM"" = ""UOM"".""NAME"" WHERE ""GTIN"" = @ParamValue " +
                    @" UNION Select ""NAME"" from ""UOM"" join ""ITEM_MANUFACTURER"" on ""ITEM_MANUFACTURER"".""ALT_1_UOM"" = ""UOM"".""NAME"" WHERE ""GTIN"" = @ParamValue " +
                    @" UNION Select ""NAME"" from ""UOM"" join ""ITEM_MANUFACTURER"" on ""ITEM_MANUFACTURER"".""ALT_2_UOM"" = ""UOM"".""NAME"" WHERE ""GTIN"" = @ParamValue ";
                
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>() { 
                    new NpgsqlParameter("@ParamValue", DbType.String) { Value = gtin }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetUomFromGtin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List< ItemManufacturer> GetItemManufacturerByParentGtin(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_KIT"" WHERE ""PARENT_GTIN"" = @ParamValue ORDER BY ""CHILD_GTIN"";";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
              
                return GetItemManufacturerAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetItemManufacturerByParentGtin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
