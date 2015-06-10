using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class AuditTable
    {

        #region Properties
        public Int32 Id { get; set; }
        public string DbTable { get; set; }
        public string RecordIdOnTable { get; set; }
        public Int32 UserId { get; set; }
        public Int32 ActivityId { get; set; }
        public DateTime Date { get; set; }
        public Activities Activity
        {
            get
            {
                if (this.ActivityId > 0)
                    return Activities.GetActivitiesById(this.ActivityId);
                else
                    return null;
            }
        }
        public User User
        {
            get
            {
                if (this.UserId > 0)
                    return User.GetUserById(this.UserId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<AuditTable> GetAuditTableList()
        {
            try
            {
                string query = @"SELECT * FROM ""AUDIT_TABLE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetAuditTableAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AuditTable", "GetAuditTableList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetAuditTableForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""AUDIT_TABLE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AuditTable", "GetAuditTableForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static AuditTable GetAuditTableById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""AUDIT_TABLE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AuditTable", i.ToString(), 4, DateTime.Now, 1);
                return GetAuditTableAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AuditTable", "GetAuditTableById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(AuditTable o)
        {
            try
            {
                string query = @"INSERT INTO ""AUDIT_TABLE"" (""DB_TABLE"", ""RECORD_ID_ON_TABLE"", ""USER_ID"", ""ACTIVITY_ID"", ""DATE"") VALUES (@DbTable, @RecordIdOnTable, @UserId, @ActivityId, @Date) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@DbTable", DbType.String)  { Value = o.DbTable },
new NpgsqlParameter("@RecordIdOnTable", DbType.String)  { Value = o.RecordIdOnTable },
new NpgsqlParameter("@UserId", DbType.Int32)  { Value = o.UserId },
new NpgsqlParameter("@ActivityId", DbType.Int32)  { Value = o.ActivityId },
new NpgsqlParameter("@Date", DbType.DateTime)  { Value = o.Date }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("AuditTable", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                //Log.InsertEntity("AuditTable", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(AuditTable o)
        {
            try
            {
                string query = @"UPDATE ""AUDIT_TABLE"" SET ""ID"" = @Id, ""DB_TABLE"" = @DbTable, ""RECORD_ID_ON_TABLE"" = @RecordIdOnTable, ""USER_ID"" = @UserId, ""ACTIVITY_ID"" = @ActivityId, ""DATE"" = @Date WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@DbTable", DbType.String)  { Value = o.DbTable },
new NpgsqlParameter("@RecordIdOnTable", DbType.String)  { Value = o.RecordIdOnTable },
new NpgsqlParameter("@UserId", DbType.Int32)  { Value = o.UserId },
new NpgsqlParameter("@ActivityId", DbType.Int32)  { Value = o.ActivityId },
new NpgsqlParameter("@Date", DbType.DateTime)  { Value = o.Date },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("AuditTable", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AuditTable", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""AUDIT_TABLE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
               // AuditTable.InsertEntity("AuditTable", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AuditTable", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static AuditTable GetAuditTableAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    AuditTable o = new AuditTable();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.DbTable = row["DB_TABLE"].ToString();
                    o.RecordIdOnTable = row["RECORD_ID_ON_TABLE"].ToString();
                    o.UserId = Helper.ConvertToInt(row["USER_ID"]);
                    o.ActivityId = Helper.ConvertToInt(row["ACTIVITY_ID"]);
                    o.Date = Helper.ConvertToDate(row["DATE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("AuditTable", "GetAuditTableAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<AuditTable> GetAuditTableAsList(DataTable dt)
        {
            List<AuditTable> oList = new List<AuditTable>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    AuditTable o = new AuditTable();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.DbTable = row["DB_TABLE"].ToString();
                    o.RecordIdOnTable = row["RECORD_ID_ON_TABLE"].ToString();
                    o.UserId = Helper.ConvertToInt(row["USER_ID"]);
                    o.ActivityId = Helper.ConvertToInt(row["ACTIVITY_ID"]);
                    o.Date = Helper.ConvertToDate(row["DATE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("AuditTable", "GetAuditTableAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
