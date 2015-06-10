using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class HealthFacility
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Int32 ParentId { get; set; }
        public bool TopLevel { get; set; }
        public bool Leaf { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public bool VaccinationPoint { get; set; }
        public string Address { get; set; }
        public bool VaccineStore { get; set; }
        public string Contact { get; set; }
        public double ColdStorageCapacity { get; set; }
        public Int32 TypeId { get; set; }
        public Int32 Ownership { get; set; }
        public Int32 Lowcode { get; set; }
        public HealthFacilityType Type
        {
            get
            {
                if (this.TypeId > 0)
                    return null;//return HealthFacilityType.GetHealthFacilityTypeById(this.TypeId);
                else
                    return null;
            }
        }
        public HealthFacility Parent
        {
            get
            {
                return HealthFacility.GetHealthFacilityById(ParentId);
            }
        }
        #endregion

        #region GetData
        public static List<HealthFacility> GetHealthFacilityList()
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetHealthFacilityForList()
        {
            try
            {
                string query = @"Select '-----' as ""CODE"", '-----' as ""NAME"" UNION  SELECT ""CODE"", ""NAME"" FROM ""HEALTH_FACILITY"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static HealthFacility GetHealthFacilityById(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", i.ToString(), 4, DateTime.Now, 1);
                return GetHealthFacilityAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static HealthFacility GetHealthFacilityByCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetHealthFacilityAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static HealthFacility GetHealthFacilityByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetHealthFacilityAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "GetHealthFacilityByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(HealthFacility o)
        {
            try
            {
                string query = @"INSERT INTO ""HEALTH_FACILITY"" (""NAME"", ""CODE"", ""PARENT_ID"", ""TOP_LEVEL"", ""LEAF"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""VACCINATION_POINT"", ""ADDRESS"", ""VACCINE_STORE"", ""CONTACT"", ""COLD_STORAGE_CAPACITY"", ""TYPE_ID"", ""OWNERSHIP"", ""LOWCODE"") VALUES (@Name, @Code, @ParentId, @TopLevel, @Leaf, @Notes, @IsActive, @ModifiedOn, @ModifiedBy, @VaccinationPoint, @Address, @VaccineStore, @Contact, @ColdStorageCapacity, @TypeId, @Ownership, @Lowcode) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@ParentId", DbType.Int32)  { Value = o.ParentId },
new NpgsqlParameter("@TopLevel", DbType.Boolean)  { Value = o.TopLevel },
new NpgsqlParameter("@Leaf", DbType.Boolean)  { Value = o.Leaf },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@VaccinationPoint", DbType.Boolean)  { Value = o.VaccinationPoint },
new NpgsqlParameter("@Address", DbType.String)  { Value = (object)o.Address ?? DBNull.Value },
new NpgsqlParameter("@VaccineStore", DbType.Boolean)  { Value = o.VaccineStore },
new NpgsqlParameter("@Contact", DbType.String)  { Value = (object)o.Contact ?? DBNull.Value },
new NpgsqlParameter("@ColdStorageCapacity", DbType.Double)  { Value = (object)o.ColdStorageCapacity ?? DBNull.Value },
new NpgsqlParameter("@TypeId", DbType.Int32)  { Value = (object)o.TypeId ?? DBNull.Value },
new NpgsqlParameter("@Ownership", DbType.Int32)  { Value = (object)o.Ownership ?? DBNull.Value },
new NpgsqlParameter("@Lowcode", DbType.Int32)  { Value = (object)o.Lowcode ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(HealthFacility o)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITY"" SET ""ID"" = @Id, ""NAME"" = @Name, ""CODE"" = @Code, ""PARENT_ID"" = @ParentId, ""TOP_LEVEL"" = @TopLevel, ""LEAF"" = @Leaf, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""VACCINATION_POINT"" = @VaccinationPoint, ""ADDRESS"" = @Address, ""VACCINE_STORE"" = @VaccineStore, ""CONTACT"" = @Contact, ""COLD_STORAGE_CAPACITY"" = @ColdStorageCapacity, ""TYPE_ID"" = @TypeId, ""OWNERSHIP"" = @Ownership, ""LOWCODE"" = @Lowcode WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@ParentId", DbType.Int32)  { Value = o.ParentId },
new NpgsqlParameter("@TopLevel", DbType.Boolean)  { Value = o.TopLevel },
new NpgsqlParameter("@Leaf", DbType.Boolean)  { Value = o.Leaf },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@VaccinationPoint", DbType.Boolean)  { Value = o.VaccinationPoint },
new NpgsqlParameter("@Address", DbType.String)  { Value = (object)o.Address ?? DBNull.Value },
new NpgsqlParameter("@VaccineStore", DbType.Boolean)  { Value = o.VaccineStore },
new NpgsqlParameter("@Contact", DbType.String)  { Value = (object)o.Contact ?? DBNull.Value },
new NpgsqlParameter("@ColdStorageCapacity", DbType.Double)  { Value = (object)o.ColdStorageCapacity ?? DBNull.Value },
new NpgsqlParameter("@TypeId", DbType.Int32)  { Value = (object)o.TypeId ?? DBNull.Value },
new NpgsqlParameter("@Ownership", DbType.Int32)  { Value = (object)o.Ownership ?? DBNull.Value },
new NpgsqlParameter("@Lowcode", DbType.Int32)  { Value = (object)o.Lowcode ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""HEALTH_FACILITY"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITY"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacility", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacility", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static HealthFacility GetHealthFacilityAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacility o = new HealthFacility();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.ParentId = Helper.ConvertToInt(row["PARENT_ID"]);
                    o.TopLevel = Helper.ConvertToBoolean(row["TOP_LEVEL"]);
                    o.Leaf = Helper.ConvertToBoolean(row["LEAF"]);
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.VaccinationPoint = Helper.ConvertToBoolean(row["VACCINATION_POINT"]);
                    o.Address = row["ADDRESS"].ToString();
                    o.VaccineStore = Helper.ConvertToBoolean(row["VACCINE_STORE"]);
                    o.Contact = row["CONTACT"].ToString();
                    o.ColdStorageCapacity = Helper.ConvertToDecimal(row["COLD_STORAGE_CAPACITY"]);
                    o.TypeId = Helper.ConvertToInt(row["TYPE_ID"]);
                    o.Ownership = Helper.ConvertToInt(row["OWNERSHIP"]);
                    o.Lowcode = Helper.ConvertToInt(row["LOWCODE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacility", "GetHealthFacilityAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<HealthFacility> GetHealthFacilityAsList(DataTable dt)
        {
            List<HealthFacility> oList = new List<HealthFacility>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacility o = new HealthFacility();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.ParentId = Helper.ConvertToInt(row["PARENT_ID"]);
                    o.TopLevel = Helper.ConvertToBoolean(row["TOP_LEVEL"]);
                    o.Leaf = Helper.ConvertToBoolean(row["LEAF"]);
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.VaccinationPoint = Helper.ConvertToBoolean(row["VACCINATION_POINT"]);
                    o.Address = row["ADDRESS"].ToString();
                    o.VaccineStore = Helper.ConvertToBoolean(row["VACCINE_STORE"]);
                    o.Contact = row["CONTACT"].ToString();
                    o.ColdStorageCapacity = Helper.ConvertToDecimal(row["COLD_STORAGE_CAPACITY"]);
                    o.TypeId = Helper.ConvertToInt(row["TYPE_ID"]);
                    o.Ownership = Helper.ConvertToInt(row["OWNERSHIP"]);
                    o.Lowcode = Helper.ConvertToInt(row["LOWCODE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacility", "GetHealthFacilityAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
