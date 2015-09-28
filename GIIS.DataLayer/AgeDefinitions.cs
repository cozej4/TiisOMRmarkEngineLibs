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
    public partial class AgeDefinitions
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public Int32 Days { get; set; }
        #endregion

        #region GetData
        public static List<AgeDefinitions> GetAgeDefinitionsList()
        {
            try
            {
                string query = @"SELECT * FROM ""AGE_DEFINITIONS"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetAgeDefinitionsAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "GetAgeDefinitionsList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetAgeDefinitionsForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""AGE_DEFINITIONS"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "GetAgeDefinitionsForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static AgeDefinitions GetAgeDefinitionsById(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""AGE_DEFINITIONS"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AgeDefinitions", i.ToString(), 4, DateTime.Now, 1);
                return GetAgeDefinitionsAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "GetAgeDefinitionsById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static AgeDefinitions GetAgeDefinitionsByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""AGE_DEFINITIONS"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AgeDefinitions", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetAgeDefinitionsAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "GetAgeDefinitionsByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(AgeDefinitions o)
        {
            try
            {
                string query = @"INSERT INTO ""AGE_DEFINITIONS"" (""NAME"", ""IS_ACTIVE"", ""NOTES"", ""DAYS"") VALUES (@Name, @IsActive, @Notes, @Days) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Days", DbType.Int32)  { Value = o.Days }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AgeDefinitions", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(AgeDefinitions o)
        {
            try
            {
                string query = @"UPDATE ""AGE_DEFINITIONS"" SET ""ID"" = @Id, ""NAME"" = @Name, ""IS_ACTIVE"" = @IsActive, ""NOTES"" = @Notes, ""DAYS"" = @Days WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Days", DbType.Int32)  { Value = o.Days },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AgeDefinitions", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""AGE_DEFINITIONS"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AgeDefinitions", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""AGE_DEFINITIONS"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("AgeDefinitions", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("AgeDefinitions", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static AgeDefinitions GetAgeDefinitionsAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    AgeDefinitions o = new AgeDefinitions();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.Days = Helper.ConvertToInt(row["DAYS"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("AgeDefinitions", "GetAgeDefinitionsAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<AgeDefinitions> GetAgeDefinitionsAsList(DataTable dt)
        {
            List<AgeDefinitions> oList = new List<AgeDefinitions>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    AgeDefinitions o = new AgeDefinitions();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.Days = Helper.ConvertToInt(row["DAYS"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("AgeDefinitions", "GetAgeDefinitionsAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
