using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Manufacturer
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Int32 Hl7ManufacturerId { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        #endregion

        #region GetData
        public static List<Manufacturer> GetManufacturerList()
        {
            try
            {
                string query = @"SELECT * FROM ""MANUFACTURER"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetManufacturerAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "GetManufacturerList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetManufacturerForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""MANUFACTURER"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "GetManufacturerForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Manufacturer GetManufacturerById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""MANUFACTURER"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Manufacturer", i.ToString(), 4, DateTime.Now, 1);
                return GetManufacturerAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "GetManufacturerById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Manufacturer GetManufacturerByCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""MANUFACTURER"" WHERE ""CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Manufacturer", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetManufacturerAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "GetManufacturerByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Manufacturer GetManufacturerByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""MANUFACTURER"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Manufacturer", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetManufacturerAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "GetManufacturerByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Manufacturer o)
        {
            try
            {
                string query = @"INSERT INTO ""MANUFACTURER"" (""NAME"", ""CODE"", ""HL7_MANUFACTURER_ID"", ""IS_ACTIVE"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@Name, @Code, @Hl7ManufacturerId, @IsActive, @Notes, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@Hl7ManufacturerId", DbType.Int32)  { Value = (object)o.Hl7ManufacturerId ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Manufacturer", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Manufacturer o)
        {
            try
            {
                string query = @"UPDATE ""MANUFACTURER"" SET ""ID"" = @Id, ""NAME"" = @Name, ""CODE"" = @Code, ""HL7_MANUFACTURER_ID"" = @Hl7ManufacturerId, ""IS_ACTIVE"" = @IsActive, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@Hl7ManufacturerId", DbType.Int32)  { Value = (object)o.Hl7ManufacturerId ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Manufacturer", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""MANUFACTURER"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Manufacturer", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""MANUFACTURER"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Manufacturer", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Manufacturer", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Manufacturer GetManufacturerAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Manufacturer o = new Manufacturer();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.Hl7ManufacturerId = Helper.ConvertToInt(row["HL7_MANUFACTURER_ID"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Manufacturer", "GetManufacturerAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Manufacturer> GetManufacturerAsList(DataTable dt)
        {
            List<Manufacturer> oList = new List<Manufacturer>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Manufacturer o = new Manufacturer();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.Hl7ManufacturerId = Helper.ConvertToInt(row["HL7_MANUFACTURER_ID"]);
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Manufacturer", "GetManufacturerAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
