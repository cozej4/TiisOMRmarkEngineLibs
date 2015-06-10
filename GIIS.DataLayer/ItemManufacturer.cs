using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class ItemManufacturer
    {

        private List<ItemManufacturer> m_kitItems;

        /// <summary>
        /// Get a list of kit items
        /// </summary>
        /// HACK: This should be corrected into proper ItemKit entities
        public List<ItemManufacturer> KitItems
        {
            get
            {
                if(this.m_kitItems == null)
                {
                    this.m_kitItems = new List<ItemManufacturer>();
                    String sql = "SELECT * FROM \"ITEM_KIT\" WHERE \"PARENT_GTIN\" = @gtin;";
                    using(var dt = DBManager.ExecuteReaderCommand(sql, CommandType.Text, new List<NpgsqlParameter>() {
                        new NpgsqlParameter("@gtin", DbType.String) { Value = this.Gtin }
                    }))
                    {
                        using(var rdr = dt.CreateDataReader())
                            while(rdr.Read())
                                this.m_kitItems.Add(ItemManufacturer.GetItemManufacturerByGtin(Convert.ToString(rdr["CHILD_GTIN"])));
                    }
                }
                return this.m_kitItems;
            }
        }

        #region Properties
        public string Gtin { get; set; }
        public Int32 ItemId { get; set; }
        public Int32 ManufacturerId { get; set; }
        public string BaseUom { get; set; }
        public string Alt1Uom { get; set; }
        public Int32 Alt1QtyPer { get; set; }
        public string Alt2Uom { get; set; }
        public Int32 Alt2QtyPer { get; set; }
        public double Price { get; set; }
        public string GtinParent { get; set; }
        public double BaseUomChildPerBaseUomParent { get; set; }
        public double StorageSpace { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
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
        public Manufacturer ManufacturerObject
        {
            get
            {
                if (this.ManufacturerId > 0)
                    return Manufacturer.GetManufacturerById(this.ManufacturerId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<ItemManufacturer> GetItemManufacturerList()
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_MANUFACTURER"" ORDER BY ""GTIN"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetItemManufacturerAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetItemManufacturerList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemManufacturerForList()
        {
            try
            {
                string query = @"Select  '-----' as ""GTIN"" UNION  SELECT  ""GTIN"" FROM ""ITEM_MANUFACTURER"" WHERE ""IS_ACTIVE"" = true ORDER BY ""GTIN"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetItemManufacturerForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        
        public static ItemManufacturer GetItemManufacturerByGtin(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM_MANUFACTURER"" WHERE ""GTIN"" = @ParamValue ORDER BY ""GTIN"";";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemManufacturer",  s, 4, DateTime.Now, 1);
                return GetItemManufacturerAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "GetItemManufacturerByGtin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(ItemManufacturer o)
        {
            try
            {
                string query = @"INSERT INTO ""ITEM_MANUFACTURER"" (""GTIN"", ""ITEM_ID"", ""MANUFACTURER_ID"", ""BASE_UOM"", ""ALT_1_UOM"", ""ALT_1_QTY_PER"", ""ALT_2_UOM"", ""ALT_2_QTY_PER"", ""PRICE"", ""GTIN_PARENT"", ""BASE_UOM_CHILD_PER_BASE_UOM_PARENT"", ""STORAGE_SPACE"", ""IS_ACTIVE"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@Gtin, @ItemId, @ManufacturerId, @BaseUom, @Alt1Uom, @Alt1QtyPer, @Alt2Uom, @Alt2QtyPer, @Price, @GtinParent, @BaseUomChildPerBaseUomParent, @StorageSpace, @IsActive, @Notes, @ModifiedOn, @ModifiedBy) returning ""GTIN"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@ManufacturerId", DbType.Int32)  { Value = o.ManufacturerId },
new NpgsqlParameter("@BaseUom", DbType.String)  { Value = o.BaseUom },
new NpgsqlParameter("@Alt1Uom", DbType.String)  { Value = o.Alt1Uom },
new NpgsqlParameter("@Alt1QtyPer", DbType.Int32)  { Value = o.Alt1QtyPer },
new NpgsqlParameter("@Alt2Uom", DbType.String)  { Value = (object)o.Alt2Uom ?? DBNull.Value },
new NpgsqlParameter("@Alt2QtyPer", DbType.Int32)  { Value = (object)o.Alt2QtyPer ?? DBNull.Value },
new NpgsqlParameter("@Price", DbType.Double)  { Value = (object)o.Price ?? DBNull.Value },
new NpgsqlParameter("@GtinParent", DbType.String)  { Value = (object)o.GtinParent ?? DBNull.Value },
new NpgsqlParameter("@BaseUomChildPerBaseUomParent", DbType.Double)  { Value = (object)o.BaseUomChildPerBaseUomParent ?? DBNull.Value },
new NpgsqlParameter("@StorageSpace", DbType.Double)  { Value = (object)o.StorageSpace ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }
};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemManufacturer", o.Gtin, 1, DateTime.Now, o.ModifiedBy);
                return 1;// int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(ItemManufacturer o)
        {
            try
            {
                string query = @"UPDATE ""ITEM_MANUFACTURER"" SET ""ITEM_ID"" = @ItemId, ""MANUFACTURER_ID"" = @ManufacturerId, ""BASE_UOM"" = @BaseUom, ""ALT_1_UOM"" = @Alt1Uom, ""ALT_1_QTY_PER"" = @Alt1QtyPer, ""ALT_2_UOM"" = @Alt2Uom, ""ALT_2_QTY_PER"" = @Alt2QtyPer, ""PRICE"" = @Price, ""GTIN_PARENT"" = @GtinParent, ""BASE_UOM_CHILD_PER_BASE_UOM_PARENT"" = @BaseUomChildPerBaseUomParent, ""STORAGE_SPACE"" = @StorageSpace, ""IS_ACTIVE"" = @IsActive, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""GTIN"" = @Gtin ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Gtin", DbType.String)  { Value = o.Gtin },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@ManufacturerId", DbType.Int32)  { Value = o.ManufacturerId },
new NpgsqlParameter("@BaseUom", DbType.String)  { Value = o.BaseUom },
new NpgsqlParameter("@Alt1Uom", DbType.String)  { Value = o.Alt1Uom },
new NpgsqlParameter("@Alt1QtyPer", DbType.Int32)  { Value = o.Alt1QtyPer },
new NpgsqlParameter("@Alt2Uom", DbType.String)  { Value = (object)o.Alt2Uom ?? DBNull.Value },
new NpgsqlParameter("@Alt2QtyPer", DbType.Int32)  { Value = (object)o.Alt2QtyPer ?? DBNull.Value },
new NpgsqlParameter("@Price", DbType.Double)  { Value = (object)o.Price ?? DBNull.Value },
new NpgsqlParameter("@GtinParent", DbType.String)  { Value = (object)o.GtinParent ?? DBNull.Value },
new NpgsqlParameter("@BaseUomChildPerBaseUomParent", DbType.Double)  { Value = (object)o.BaseUomChildPerBaseUomParent ?? DBNull.Value },
new NpgsqlParameter("@StorageSpace", DbType.Double)  { Value = (object)o.StorageSpace ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }
};

                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemManufacturer", o.Gtin, 2, DateTime.Now, o.ModifiedBy);

                // Update the Kitted Items
                // HACK: This needs to be fixed
                parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@parentGtin", DbType.String)  { Value = o.Gtin }
                };
                query = "DELETE FROM \"ITEM_KIT\" WHERE \"PARENT_GTIN\" = @parentGtin";
                DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                
                foreach (var itm in o.KitItems)
                {
                    query = "INSERT INTO \"ITEM_KIT\" (\"PARENT_GTIN\",\"CHILD_GTIN\",\"QTY_CHILD_PARENT_UOM\",\"NOTES\",\"MODIFIED_BY\") VALUES (@parentGtin,@childGtin,1,@note,@userId); ";
                    parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@parentGtin", DbType.String)  { Value = o.Gtin },
                    new NpgsqlParameter("@childGtin", DbType.String)  { Value = itm.Gtin },
                    new NpgsqlParameter("@note", DbType.String)  { Value = itm.ItemObject.Name },
                    new NpgsqlParameter("@userId", DbType.Int32)  { Value = o.ModifiedBy }
                    };
                    DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);

                }

                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(string gtin)
        {
            try
            {
                string query = @"DELETE FROM ""ITEM_MANUFACTURER"" WHERE ""GTIN"" = @Gtin ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Gtin", DbType.String)  { Value = gtin }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemManufacturer", gtin, 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(string gtin)
        {
            try
            {
                string query = @"UPDATE ""ITEM_MANUFACTURER"" SET ""IS_ACTIVE"" = 'false' WHERE ""GTIN"" = @Gtin ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Gtin", DbType.String)  { Value = gtin }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ItemManufacturer",  gtin, 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ItemManufacturer", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static ItemManufacturer GetItemManufacturerAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ItemManufacturer o = new ItemManufacturer();
                    o.Gtin = row["GTIN"].ToString();
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.ManufacturerId = Helper.ConvertToInt(row["MANUFACTURER_ID"]);
                    o.BaseUom = row["BASE_UOM"].ToString();
                    o.Alt1Uom = row["ALT_1_UOM"].ToString();
                    o.Alt1QtyPer = Helper.ConvertToInt(row["ALT_1_QTY_PER"]);
                    o.Alt2Uom = row["ALT_2_UOM"].ToString();
                    o.Alt2QtyPer = Helper.ConvertToInt(row["ALT_2_QTY_PER"]);
                    o.Price = Helper.ConvertToDecimal(row["PRICE"]);
                    o.GtinParent = row["GTIN_PARENT"].ToString();
                    o.BaseUomChildPerBaseUomParent = Helper.ConvertToDecimal(row["BASE_UOM_CHILD_PER_BASE_UOM_PARENT"]);
                    o.StorageSpace = Helper.ConvertToDecimal(row["STORAGE_SPACE"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ItemManufacturer", "GetItemManufacturerAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<ItemManufacturer> GetItemManufacturerAsList(DataTable dt)
        {
            List<ItemManufacturer> oList = new List<ItemManufacturer>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ItemManufacturer o = new ItemManufacturer();
                    o.Gtin = row["GTIN"].ToString();
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.ManufacturerId = Helper.ConvertToInt(row["MANUFACTURER_ID"]);
                    o.BaseUom = row["BASE_UOM"].ToString();
                    o.Alt1Uom = row["ALT_1_UOM"].ToString();
                    o.Alt1QtyPer = Helper.ConvertToInt(row["ALT_1_QTY_PER"]);
                    o.Alt2Uom = row["ALT_2_UOM"].ToString();
                    o.Alt2QtyPer = Helper.ConvertToInt(row["ALT_2_QTY_PER"]);
                    o.Price = Helper.ConvertToDecimal(row["PRICE"]);
                    o.GtinParent = row["GTIN_PARENT"].ToString();
                    o.BaseUomChildPerBaseUomParent = Helper.ConvertToDecimal(row["BASE_UOM_CHILD_PER_BASE_UOM_PARENT"]);
                    o.StorageSpace = Helper.ConvertToDecimal(row["STORAGE_SPACE"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ItemManufacturer", "GetItemManufacturerAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}