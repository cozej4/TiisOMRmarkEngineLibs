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
    public partial class HealthFacilityType
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        #endregion

        #region GetData
        public static List<HealthFacilityType> GetHealthFacilityTypeList()
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_TYPE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHealthFacilityTypeAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "GetHealthFacilityTypeList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetHealthFacilityTypeForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""HEALTH_FACILITY_TYPE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "GetHealthFacilityTypeForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static HealthFacilityType GetHealthFacilityTypeById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""HEALTH_FACILITY_TYPE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityType", i.ToString(), 4, DateTime.Now, 1);
                return GetHealthFacilityTypeAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "GetHealthFacilityTypeById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(HealthFacilityType o)
        {
            try
            {
                string query = @"INSERT INTO ""HEALTH_FACILITY_TYPE"" (""NAME"", ""CODE"", ""IS_ACTIVE"", ""NOTES"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@Name, @Code, @IsActive, @Notes, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityType", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(HealthFacilityType o)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITY_TYPE"" SET ""ID"" = @Id, ""NAME"" = @Name, ""CODE"" = @Code, ""IS_ACTIVE"" = @IsActive, ""NOTES"" = @Notes, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = (object)o.Code ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityType", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""HEALTH_FACILITY_TYPE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityType", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""HEALTH_FACILITY_TYPE"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("HealthFacilityType", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("HealthFacilityType", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static HealthFacilityType GetHealthFacilityTypeAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityType o = new HealthFacilityType();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityType", "GetHealthFacilityTypeAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<HealthFacilityType> GetHealthFacilityTypeAsList(DataTable dt)
        {
            List<HealthFacilityType> oList = new List<HealthFacilityType>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    HealthFacilityType o = new HealthFacilityType();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.Notes = row["NOTES"].ToString();
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("HealthFacilityType", "GetHealthFacilityTypeAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
