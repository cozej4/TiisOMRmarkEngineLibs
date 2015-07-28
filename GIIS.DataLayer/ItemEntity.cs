using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    partial class Item
    {
        public static DataTable GetItemsList()
        {
            try
            {
                string query = @"SELECT -1 as ""ID"", '-----' as ""CODE"" UNION SELECT ""ID"", ""CODE"" FROM ""ITEM"" WHERE ""IS_ACTIVE"" = 'true' ORDER BY ""ID"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetVaccinesList()
        {
            try
            {
                string query = @"SELECT -1 as ""ID"", '-----' as ""CODE"" UNION SELECT ""ID"", ""CODE"" FROM ""ITEM"" WHERE ""ITEM_CATEGORY_ID"" = 1 AND ""IS_ACTIVE"" = 'true' ORDER BY ""ID"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetVaccinesList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static DataTable GetVaccines()
        {
            try
            {
                string query = @"SELECT ""ID"", ""CODE"" FROM ""ITEM"" WHERE ""ITEM_CATEGORY_ID"" = 1 AND ""IS_ACTIVE"" = 'true' ORDER BY ""ID"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetVaccines", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<Item> GetItemList(string where)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM ""ITEM"" WHERE {0} AND ""IS_ACTIVE"" = 'true';", where);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetItemAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemsExceptDiluentList()
        {
            try
            {
                ItemCategory itemCategory = ItemCategory.GetItemCategoryByCode("DIL");


                string query = string.Format(@"SELECT -1 as ""ID"", '-----' as ""CODE"" UNION SELECT ""ID"", ""CODE"" FROM ""ITEM"" WHERE ""ITEM_CATEGORY_ID"" <> {0} AND (""EXIT_DATE"" = '0001-01-01' OR ""EXIT_DATE"" is Null)  ORDER BY ""CODE"";", itemCategory.Id);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemsExceptDiluentList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetDiluentList()
        {
            try
            {
                int itemCategory = 2;


                string query = string.Format(@"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""ITEM"" WHERE ""ITEM_CATEGORY_ID"" = {0} ORDER BY ""ID"";", itemCategory);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetDiluentList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemByItemCategoryId(Int32 i)
        {
            try
            {
                string query = String.Format(@"SELECT -1 as ""ID"", '-----' as ""CODE"" UNION SELECT ""ID"",""CODE"" FROM ""ITEM"" WHERE ""ITEM_CATEGORY_ID"" = {0}  ORDER BY ""CODE"" ", i); //AND (""EXIT_DATE"" = '0001-01-01' OR ""EXIT_DATE"" is Null) AND ""IS_ACTIVE"" = true
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemByItemCategoryId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<Item> GetItemListNotDiluents()
        {
            try
            {
                int itemCategory = 2;


                string query = string.Format(@"SELECT * FROM ""ITEM"" WHERE ""ITEM_CATEGORY_ID"" <> {0}  ORDER BY ""ID"";", itemCategory);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetItemAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Item> GetPagedItemList(string name, int itemCategoryId, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM""  WHERE 1 = 1"
                                  + @" AND ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                  + @" AND ""ITEM_CATEGORY_ID"" = @ItemCategoryId OR @ItemCategoryId is null or @ItemCategoryId = -1"
                                  + @" ORDER BY ""ITEM_CATEGORY_ID"", ""NAME"" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@ItemCategoryId", DbType.Int32) { Value = itemCategoryId },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetItemAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetPagedItemList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountItemList(string name, int itemCategoryId)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""ITEM""  WHERE 1 = 1"
                                  + @" AND ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                  + @" AND ""ITEM_CATEGORY_ID"" = @ItemCategoryId OR @ItemCategoryId is null or @ItemCategoryId = -1;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@ItemCategoryId", DbType.Int32) { Value = itemCategoryId }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetCountItemList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
