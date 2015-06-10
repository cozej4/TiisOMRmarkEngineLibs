using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Place
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public Int32 ParentId { get; set; }
        public bool Leaf { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Int32? HealthFacilityId { get; set; }
        public string Code { get; set; }
        public HealthFacility HealthFacility
        {
            get
            {
                if (this.HealthFacilityId > 0)
                    return HealthFacility.GetHealthFacilityById(this.HealthFacilityId);
                else
                    return null;
            }
        }
        public Place Parent
        {
            get
            {
                return Place.GetPlaceById(ParentId);
            }
        }
        #endregion

        #region GetData
        public static List<Place> GetPlaceList()
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetPlaceForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""PLACE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static Place GetPlaceById(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", i.ToString(), 4, DateTime.Now, 1);
                return GetPlaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Place GetPlaceByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetPlaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Place GetPlaceByParentId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" WHERE ""PARENT_ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", i.ToString(), 4, DateTime.Now, 1);
                return GetPlaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceByParentId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Place o)
        {
            try
            {
                string query = @"INSERT INTO ""PLACE"" (""NAME"", ""PARENT_ID"", ""LEAF"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""HEALTH_FACILITY_ID"", ""CODE"") VALUES (@Name, @ParentId, @Leaf, @Notes, @IsActive, @ModifiedOn, @ModifiedBy, @HealthFacilityId, @Code) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@ParentId", DbType.Int32)  { Value = o.ParentId },
new NpgsqlParameter("@Leaf", DbType.Boolean)  { Value = o.Leaf },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = (object)o.HealthFacilityId ?? DBNull.Value },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Place o)
        {
            try
            {
                string query = @"UPDATE ""PLACE"" SET ""ID"" = @Id, ""NAME"" = @Name, ""PARENT_ID"" = @ParentId, ""LEAF"" = @Leaf, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""HEALTH_FACILITY_ID"" = @HealthFacilityId, ""CODE"" = @Code WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@ParentId", DbType.Int32)  { Value = o.ParentId },
new NpgsqlParameter("@Leaf", DbType.Boolean)  { Value = o.Leaf },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@HealthFacilityId", DbType.Int32)  { Value = (object)o.HealthFacilityId ?? DBNull.Value },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""PLACE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""PLACE"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Place GetPlaceAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Place o = new Place();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.ParentId = Helper.ConvertToInt(row["PARENT_ID"]);
                    o.Leaf = Helper.ConvertToBoolean(row["LEAF"]);
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.Code = row["CODE"].ToString();
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Place", "GetPlaceAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Place> GetPlaceAsList(DataTable dt)
        {
            List<Place> oList = new List<Place>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Place o = new Place();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.ParentId = Helper.ConvertToInt(row["PARENT_ID"]);
                    o.Leaf = Helper.ConvertToBoolean(row["LEAF"]);
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.Code = row["CODE"].ToString();
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Place", "GetPlaceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
