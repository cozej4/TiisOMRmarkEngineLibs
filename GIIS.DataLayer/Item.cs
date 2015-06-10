using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Item
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 ItemCategoryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Int32? Hl7VaccineId { get; set; }
        public DateTime ExitDate { get; set; }
        public ItemCategory ItemCategory
        {
            get
            {
                if (this.ItemCategoryId > 0)
                    return ItemCategory.GetItemCategoryById(this.ItemCategoryId);
                else
                    return null;
            }
        }
        public Hl7Vaccines Hl7Vaccine
        {
            get
            {
                if (this.Hl7VaccineId > 0)
                    return Hl7Vaccines.GetHl7VaccinesById(Hl7VaccineId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<Item> GetItemList()
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetItemAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetItemForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ITEM"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Item GetItemById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Item", i.ToString(), 4, DateTime.Now, 1);
                return GetItemAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Item GetItemByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""ITEM"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Item", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetItemAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "GetItemByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Item o)
        {
            try
            {
                string query = @"INSERT INTO ""ITEM"" (""ITEM_CATEGORY_ID"", ""NAME"", ""CODE"", ""ENTRY_DATE"", ""IS_ACTIVE"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""HL7_VACCINE_ID"", ""EXIT_DATE"") VALUES (@ItemCategoryId, @Name, @Code, @EntryDate, @IsActive, @Notes, @ModifiedOn, @ModifiedBy, @Hl7VaccineId, @ExitDate) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ItemCategoryId", DbType.Int32)  { Value = o.ItemCategoryId },
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@EntryDate", DbType.Date)  { Value = o.EntryDate },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Hl7VaccineId", DbType.Int32)  { Value = (object)o.Hl7VaccineId ?? DBNull.Value },
new NpgsqlParameter("@ExitDate", DbType.Date)  { Value = (object)o.ExitDate ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Item", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Item o)
        {
            try
            {
                string query = @"UPDATE ""ITEM"" SET ""ID"" = @Id, ""ITEM_CATEGORY_ID"" = @ItemCategoryId, ""NAME"" = @Name, ""CODE"" = @Code, ""ENTRY_DATE"" = @EntryDate, ""IS_ACTIVE"" = @IsActive, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""HL7_VACCINE_ID"" = @Hl7VaccineId, ""EXIT_DATE"" = @ExitDate WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ItemCategoryId", DbType.Int32)  { Value = o.ItemCategoryId },
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@EntryDate", DbType.Date)  { Value = o.EntryDate },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Hl7VaccineId", DbType.Int32)  { Value = (object)o.Hl7VaccineId ?? DBNull.Value },
new NpgsqlParameter("@ExitDate", DbType.Date)  { Value = (object)o.ExitDate ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Item", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""ITEM"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Item", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""ITEM"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Item", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Item", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Item GetItemAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Item o = new Item();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.ItemCategoryId = Helper.ConvertToInt(row["ITEM_CATEGORY_ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.EntryDate = Helper.ConvertToDate(row["ENTRY_DATE"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Hl7VaccineId = Helper.ConvertToInt(row["HL7_VACCINE_ID"]);
                    o.ExitDate = Helper.ConvertToDate(row["EXIT_DATE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Item", "GetItemAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Item> GetItemAsList(DataTable dt)
        {
            List<Item> oList = new List<Item>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Item o = new Item();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.ItemCategoryId = Helper.ConvertToInt(row["ITEM_CATEGORY_ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.EntryDate = Helper.ConvertToDate(row["ENTRY_DATE"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Hl7VaccineId = Helper.ConvertToInt(row["HL7_VACCINE_ID"]);
                    o.ExitDate = Helper.ConvertToDate(row["EXIT_DATE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Item", "GetItemAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
