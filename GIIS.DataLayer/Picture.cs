using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Picture
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public byte[] Storage { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public Int32 DisplayOrder { get; set; }
        public string Notes { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        #endregion

        #region GetData
        public static List<Picture> GetPictureList()
        {
            try
            {
                string query = @"SELECT * FROM ""PICTURE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetPictureAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Picture", "GetPictureList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetPictureForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""PICTURE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Picture", "GetPictureForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Picture GetPictureById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""PICTURE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Picture", i.ToString(), 4, DateTime.Now, 1);
                return GetPictureAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Picture", "GetPictureById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Picture GetPictureByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""PICTURE"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Picture", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetPictureAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Picture", "GetPictureByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Picture o)
        {
            try
            {
                string query = @"INSERT INTO ""PICTURE"" (""NAME"", ""URL"", ""STORAGE"", ""WIDTH"", ""HEIGHT"", ""DISPLAY_ORDER"", ""NOTES"", ""CREATED_BY"", ""CREATED_ON"") VALUES (@Name, @Url, @Storage, @Width, @Height, @DisplayOrder, @Notes, @CreatedBy, @CreatedOn) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = (object)o.Name ?? DBNull.Value },
new NpgsqlParameter("@Url", DbType.String)  { Value = (object)o.Url ?? DBNull.Value },
new NpgsqlParameter("@Width", DbType.Int32)  { Value = (object)o.Width ?? DBNull.Value },
new NpgsqlParameter("@Height", DbType.Int32)  { Value = (object)o.Height ?? DBNull.Value },
new NpgsqlParameter("@DisplayOrder", DbType.Int32)  { Value = (object)o.DisplayOrder ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@CreatedBy", DbType.Int32)  { Value = (object)o.CreatedBy ?? DBNull.Value },
new NpgsqlParameter("@CreatedOn", DbType.Date)  { Value = (object)o.CreatedOn ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Picture", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Picture", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Picture o)
        {
            try
            {
                string query = @"UPDATE ""PICTURE"" SET ""ID"" = @Id, ""NAME"" = @Name, ""URL"" = @Url, ""STORAGE"" = @Storage, ""WIDTH"" = @Width, ""HEIGHT"" = @Height, ""DISPLAY_ORDER"" = @DisplayOrder, ""NOTES"" = @Notes, ""CREATED_BY"" = @CreatedBy, ""CREATED_ON"" = @CreatedOn WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = (object)o.Name ?? DBNull.Value },
new NpgsqlParameter("@Url", DbType.String)  { Value = (object)o.Url ?? DBNull.Value },
new NpgsqlParameter("@Width", DbType.Int32)  { Value = (object)o.Width ?? DBNull.Value },
new NpgsqlParameter("@Height", DbType.Int32)  { Value = (object)o.Height ?? DBNull.Value },
new NpgsqlParameter("@DisplayOrder", DbType.Int32)  { Value = (object)o.DisplayOrder ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@CreatedBy", DbType.Int32)  { Value = (object)o.CreatedBy ?? DBNull.Value },
new NpgsqlParameter("@CreatedOn", DbType.Date)  { Value = (object)o.CreatedOn ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Picture", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Picture", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""PICTURE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Picture", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Picture", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static Picture GetPictureAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Picture o = new Picture();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Url = row["URL"].ToString();
                    o.Width = Helper.ConvertToInt(row["WIDTH"]);
                    o.Height = Helper.ConvertToInt(row["HEIGHT"]);
                    o.DisplayOrder = Helper.ConvertToInt(row["DISPLAY_ORDER"]);
                    o.Notes = row["NOTES"].ToString();
                    o.CreatedBy = Helper.ConvertToInt(row["CREATED_BY"]);
                    o.CreatedOn = Helper.ConvertToDate(row["CREATED_ON"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Picture", "GetPictureAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Picture> GetPictureAsList(DataTable dt)
        {
            List<Picture> oList = new List<Picture>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Picture o = new Picture();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Url = row["URL"].ToString();
                    o.Width = Helper.ConvertToInt(row["WIDTH"]);
                    o.Height = Helper.ConvertToInt(row["HEIGHT"]);
                    o.DisplayOrder = Helper.ConvertToInt(row["DISPLAY_ORDER"]);
                    o.Notes = row["NOTES"].ToString();
                    o.CreatedBy = Helper.ConvertToInt(row["CREATED_BY"]);
                    o.CreatedOn = Helper.ConvertToDate(row["CREATED_ON"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Picture", "GetPictureAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
