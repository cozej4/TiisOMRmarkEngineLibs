using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class NonvaccinationReason
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public string Notes { get; set; }
        public bool KeepChildDue { get; set; }
        #endregion

        #region GetData
        public static List<NonvaccinationReason> GetNonvaccinationReasonList()
        {
            try
            {
                string query = @"SELECT * FROM ""NONVACCINATION_REASON"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetNonvaccinationReasonAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "GetNonvaccinationReasonList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetNonvaccinationReasonForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""NONVACCINATION_REASON"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "GetNonvaccinationReasonForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static NonvaccinationReason GetNonvaccinationReasonById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""NONVACCINATION_REASON"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("NonvaccinationReason", i.ToString(), 4, DateTime.Now, 1);
                return GetNonvaccinationReasonAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "GetNonvaccinationReasonById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static NonvaccinationReason GetNonvaccinationReasonByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""NONVACCINATION_REASON"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("NonvaccinationReason", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetNonvaccinationReasonAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "GetNonvaccinationReasonByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(NonvaccinationReason o)
        {
            try
            {
                string query = @"INSERT INTO ""NONVACCINATION_REASON"" (""NAME"", ""CODE"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""NOTES"", ""KEEP_CHILD_DUE"") VALUES (@Name, @Code, @IsActive, @ModifiedOn, @ModifiedBy, @Notes, @KeepChildDue) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@KeepChildDue", DbType.Boolean)  { Value = o.KeepChildDue }
};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("NonvaccinationReason", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(NonvaccinationReason o)
        {
            try
            {
                string query = @"UPDATE ""NONVACCINATION_REASON"" SET ""ID"" = @Id, ""NAME"" = @Name, ""CODE"" = @Code, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""NOTES"" = @Notes, ""KEEP_CHILD_DUE"" = @KeepChildDue WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@KeepChildDue", DbType.Boolean)  { Value = o.KeepChildDue },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("NonvaccinationReason", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""NONVACCINATION_REASON"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("NonvaccinationReason", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""NONVACCINATION_REASON"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("NonvaccinationReason", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("NonvaccinationReason", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static NonvaccinationReason GetNonvaccinationReasonAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    NonvaccinationReason o = new NonvaccinationReason();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Notes = row["NOTES"].ToString();
                    o.KeepChildDue = Helper.ConvertToBoolean(row["KEEP_CHILD_DUE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("NonvaccinationReason", "GetNonvaccinationReasonAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<NonvaccinationReason> GetNonvaccinationReasonAsList(DataTable dt)
        {
            List<NonvaccinationReason> oList = new List<NonvaccinationReason>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    NonvaccinationReason o = new NonvaccinationReason();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Notes = row["NOTES"].ToString();
                    o.KeepChildDue = Helper.ConvertToBoolean(row["KEEP_CHILD_DUE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("NonvaccinationReason", "GetNonvaccinationReasonAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
