using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class ChildWeight
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 ChildId { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Child Child
        {
            get
            {
                if (this.ChildId > 0)
                    return Child.GetChildById(this.ChildId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<ChildWeight> GetChildWeightList()
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_WEIGHT"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildWeightAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetChildWeightList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetChildWeightForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""CHILD_WEIGHT"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetChildWeightForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ChildWeight GetChildWeightById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_WEIGHT"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildWeight", i.ToString(), 4, DateTime.Now, 1);
                return GetChildWeightAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "GetChildWeightById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(ChildWeight o)
        {
            try
            {
                string query = @"INSERT INTO ""CHILD_WEIGHT"" (""CHILD_ID"", ""WEIGHT"", ""DATE"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@ChildId, @Weight, @Date, @Notes, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@Weight", DbType.Double)  { Value = o.Weight },
new NpgsqlParameter("@Date", DbType.Date)  { Value = o.Date },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildWeight", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(ChildWeight o)
        {
            try
            {
                string query = @"UPDATE ""CHILD_WEIGHT"" SET ""ID"" = @Id, ""CHILD_ID"" = @ChildId, ""WEIGHT"" = @Weight, ""DATE"" = @Date, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@Weight", DbType.Double)  { Value = o.Weight },
new NpgsqlParameter("@Date", DbType.Date)  { Value = o.Date },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildWeight", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""CHILD_WEIGHT"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildWeight", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildWeight", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static ChildWeight GetChildWeightAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ChildWeight o = new ChildWeight();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
                    o.Weight = Helper.ConvertToDecimal(row["WEIGHT"]);
                    o.Date = Helper.ConvertToDate(row["DATE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ChildWeight", "GetChildWeightAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<ChildWeight> GetChildWeightAsList(DataTable dt)
        {
            List<ChildWeight> oList = new List<ChildWeight>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ChildWeight o = new ChildWeight();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
                    o.Weight = Helper.ConvertToDecimal(row["WEIGHT"]);
                    o.Date = Helper.ConvertToDate(row["DATE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ChildWeight", "GetChildWeightAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
