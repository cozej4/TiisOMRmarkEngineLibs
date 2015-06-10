using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class AdjustmentReason
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public bool Positive { get; set; }
        #endregion

        #region GetData
        public static List<AdjustmentReason> GetAdjustmentReasonList()
        {
            try
            {
                string query = @"SELECT * FROM ""ADJUSTMENT_REASON"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetAdjustmentReasonAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "GetAdjustmentReasonList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetAdjustmentReasonForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""ADJUSTMENT_REASON"" WHERE ""IS_ACTIVE"" = true AND ""ID"" <> 99 ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "GetAdjustmentReasonForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static AdjustmentReason GetAdjustmentReasonById(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""ADJUSTMENT_REASON"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AdjustmentReason", i.ToString(), 4, DateTime.Now, 1);
                return GetAdjustmentReasonAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "GetAdjustmentReasonById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static AdjustmentReason GetAdjustmentReasonByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""ADJUSTMENT_REASON"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AdjustmentReason", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetAdjustmentReasonAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "GetAdjustmentReasonByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(AdjustmentReason o)
        {
            try
            {
                string query = @"INSERT INTO ""ADJUSTMENT_REASON"" (""NAME"", ""IS_ACTIVE"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""POSITIVE"") VALUES (@Name, @IsActive, @Notes, @ModifiedOn, @ModifiedBy, @Positive) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Positive", DbType.Boolean)  { Value = o.Positive }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AdjustmentReason", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(AdjustmentReason o)
        {
            try
            {
                string query = @"UPDATE ""ADJUSTMENT_REASON"" SET ""ID"" = @Id, ""NAME"" = @Name, ""IS_ACTIVE"" = @IsActive, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""POSITIVE"" = @Positive WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Positive", DbType.Boolean)  { Value = o.Positive },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AdjustmentReason", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""ADJUSTMENT_REASON"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AdjustmentReason", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""ADJUSTMENT_REASON"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AdjustmentReason", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AdjustmentReason", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static AdjustmentReason GetAdjustmentReasonAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    AdjustmentReason o = new AdjustmentReason();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Positive = Helper.ConvertToBoolean(row["POSITIVE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("AdjustmentReason", "GetAdjustmentReasonAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<AdjustmentReason> GetAdjustmentReasonAsList(DataTable dt)
        {
            List<AdjustmentReason> oList = new List<AdjustmentReason>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    AdjustmentReason o = new AdjustmentReason();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.Positive = Helper.ConvertToBoolean(row["POSITIVE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("AdjustmentReason", "GetAdjustmentReasonAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
