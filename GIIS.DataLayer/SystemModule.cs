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
    public partial class SystemModule
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsUsed { get; set; }
        public string Notes { get; set; }
        #endregion

        #region GetData
        public static List<SystemModule> GetSystemModuleList()
        {
            try
            {
                string query = @"SELECT * FROM ""SYSTEM_MODULE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetSystemModuleAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "GetSystemModuleList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetSystemModuleForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""SYSTEM_MODULE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "GetSystemModuleForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static SystemModule GetSystemModuleById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""SYSTEM_MODULE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("SystemModule", i.ToString(), 4, DateTime.Now, 1);
                return GetSystemModuleAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "GetSystemModuleById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static SystemModule GetSystemModuleByCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""SYSTEM_MODULE"" WHERE ""CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("SystemModule", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetSystemModuleAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "GetSystemModuleByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static SystemModule GetSystemModuleByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""SYSTEM_MODULE"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("SystemModule", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetSystemModuleAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "GetSystemModuleByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static List<SystemModule> GetPagedSystemModuleList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""SYSTEM_MODULE"" WHERE " + where + " OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetSystemModuleAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "GetPagedSystemModuleList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountSystemModuleList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""SYSTEM_MODULE"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "GetCountSystemModuleList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region CRUD
        public static int Insert(SystemModule o)
        {
            try
            {
                string query = @"INSERT INTO ""SYSTEM_MODULE"" (""NAME"", ""CODE"", ""IS_USED"", ""NOTES"") VALUES (@Name, @Code, @IsUsed, @Notes) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@IsUsed", DbType.Boolean)  { Value = (object)o.IsUsed ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("SystemModule", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(SystemModule o)
        {
            try
            {
                string query = @"UPDATE ""SYSTEM_MODULE"" SET ""ID"" = @Id, ""NAME"" = @Name, ""CODE"" = @Code, ""IS_USED"" = @IsUsed, ""NOTES"" = @Notes WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@IsUsed", DbType.Boolean)  { Value = (object)o.IsUsed ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("SystemModule", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""SYSTEM_MODULE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("SystemModule", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("SystemModule", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static SystemModule GetSystemModuleAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    SystemModule o = new SystemModule();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.IsUsed = Helper.ConvertToBoolean(row["IS_USED"]);
                    o.Notes = row["NOTES"].ToString();
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("SystemModule", "GetSystemModuleAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<SystemModule> GetSystemModuleAsList(DataTable dt)
        {
            List<SystemModule> oList = new List<SystemModule>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    SystemModule o = new SystemModule();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.IsUsed = Helper.ConvertToBoolean(row["IS_USED"]);
                    o.Notes = row["NOTES"].ToString();
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("SystemModule", "GetSystemModuleAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
