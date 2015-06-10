using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Birthplace
    {
        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        #endregion

        #region GetData
        public static List<Birthplace> GetBirthplaceList()
        {
            try
            {
                string query = @"SELECT * FROM ""BIRTHPLACE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetBirthplaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "GetBirthplaceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetBirthplaceForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""BIRTHPLACE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "GetBirthplaceForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Birthplace GetBirthplaceById(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""BIRTHPLACE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Birthplace", string.Format("RecordId: {0}", i), 4, DateTime.Now, 1);
                return GetBirthplaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "GetBirthplaceById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Birthplace o)
        {
            try
            {
                string query = @"INSERT INTO ""BIRTHPLACE"" (""NAME"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@Name, @Notes, @IsActive, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
                new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
                new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
                new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
                new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }
                };
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Birthplace", string.Format("RecordId: {0}", id), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Birthplace o)
        {
            try
            {
                string query = @"UPDATE ""BIRTHPLACE"" SET ""ID"" = @Id, ""NAME"" = @Name, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
                new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
                new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
                new NpgsqlParameter("@ModifiedOn", DbType.Date)  { Value = o.ModifiedOn },
                new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
                new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
                };
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Birthplace", string.Format("RecordId: {0}", o.Id), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""BIRTHPLACE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
                };
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Birthplace", string.Format("RecordId: {0}", id), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""BIRTHPLACE"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
                };
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Birthplace", string.Format("RecordId: {0}", id), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Birthplace GetBirthplaceAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Birthplace o = new Birthplace();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Birthplace", "GetBirthplaceAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Birthplace> GetBirthplaceAsList(DataTable dt)
        {
            List<Birthplace> oList = new List<Birthplace>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Birthplace o = new Birthplace();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Birthplace", "GetBirthplaceAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion
    }
}
