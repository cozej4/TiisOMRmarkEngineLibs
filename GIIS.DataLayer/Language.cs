//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Language
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Abbrevation { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public string WritingDirection { get; set; }
        public string NameEnglish { get; set; }
        #endregion

        #region GetData
        public static List<Language> GetLanguageList()
        {
            try
            {
                string query = @"SELECT * FROM ""LANGUAGE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetLanguageAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "GetLanguageList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetLanguageForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""LANGUAGE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "GetLanguageForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Language GetLanguageById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""LANGUAGE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Language", i.ToString(), 4, DateTime.Now, 1);
                return GetLanguageAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "GetLanguageById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Language GetLanguageByAbbrevation(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""LANGUAGE"" WHERE ""ABBREVATION"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Language", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetLanguageAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "GetLanguageByAbbrevation", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Language GetLanguageByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""LANGUAGE"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Language", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetLanguageAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "GetLanguageByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Language> GetPagedLanguageList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""LANGUAGE"" WHERE ""IS_ACTIVE"" = 'True' AND " + where + @" ORDER BY ""NAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetLanguageAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "GetPagedLanguageList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountLanguageList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""LANGUAGE"" WHERE ""IS_ACTIVE"" = 'True' AND " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "GetCountLanguageList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Language o)
        {
            try
            {
                string query = @"INSERT INTO ""LANGUAGE"" (""NAME"", ""ABBREVATION"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""WRITING_DIRECTION"", ""NAME_ENGLISH"") VALUES (@Name, @Abbrevation, @Notes, @IsActive, @ModifiedOn, @ModifiedBy, @WritingDirection, @NameEnglish) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Abbrevation", DbType.String)  { Value = o.Abbrevation },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = (object)o.IsActive ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@WritingDirection", DbType.String)  { Value = o.WritingDirection },
new NpgsqlParameter("@NameEnglish", DbType.String)  { Value = (object)o.NameEnglish ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Language", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Language o)
        {
            try
            {
                string query = @"UPDATE ""LANGUAGE"" SET ""ID"" = @Id, ""NAME"" = @Name, ""ABBREVATION"" = @Abbrevation, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""WRITING_DIRECTION"" = @WritingDirection, ""NAME_ENGLISH"" = @NameEnglish WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Abbrevation", DbType.String)  { Value = o.Abbrevation },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = (object)o.IsActive ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@WritingDirection", DbType.String)  { Value = o.WritingDirection },
new NpgsqlParameter("@NameEnglish", DbType.String)  { Value = (object)o.NameEnglish ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Language", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""LANGUAGE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Language", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""LANGUAGE"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Language", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Language", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Language GetLanguageAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Language o = new Language();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Abbrevation = row["ABBREVATION"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.WritingDirection = row["WRITING_DIRECTION"].ToString();
                    o.NameEnglish = row["NAME_ENGLISH"].ToString();
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Language", "GetLanguageAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Language> GetLanguageAsList(DataTable dt)
        {
            List<Language> oList = new List<Language>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Language o = new Language();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Abbrevation = row["ABBREVATION"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.WritingDirection = row["WRITING_DIRECTION"].ToString();
                    o.NameEnglish = row["NAME_ENGLISH"].ToString();
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Language", "GetLanguageAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
