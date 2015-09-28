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
    public partial class ScheduledVaccination
    {

        #region Properties
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Int32 ItemId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public bool Status { get; set; }
        public string Deseases { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Item Item
        {
            get
            {
                if (this.ItemId > 0)
                    return Item.GetItemById(this.ItemId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<ScheduledVaccination> GetScheduledVaccinationList()
        {
            try
            {
                string query = @"SELECT * FROM ""SCHEDULED_VACCINATION"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetScheduledVaccinationAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        //public static DataTable GetScheduledVaccinationForList()
        //{
        //    try
        //    {
        //        string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""SCHEDULED_VACCINATION"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
        //        DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
        //        throw ex;
        //    }
        //}

        public static ScheduledVaccination GetScheduledVaccinationById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""SCHEDULED_VACCINATION"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ScheduledVaccination", i.ToString(), 4, DateTime.Now, 1);
                return GetScheduledVaccinationAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static ScheduledVaccination GetScheduledVaccinationByItemId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""SCHEDULED_VACCINATION"" WHERE ""ITEM_ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("ScheduledVaccination", i.ToString(), 4, DateTime.Now, 1);
                return GetScheduledVaccinationAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationByItemId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static ScheduledVaccination GetScheduledVaccinationByCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""SCHEDULED_VACCINATION"" WHERE ""CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ScheduledVaccination", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetScheduledVaccinationAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ScheduledVaccination GetScheduledVaccinationByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""SCHEDULED_VACCINATION"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ScheduledVaccination", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetScheduledVaccinationAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(ScheduledVaccination o)
        {
            try
            {
                string query = @"INSERT INTO ""SCHEDULED_VACCINATION"" (""NAME"", ""CODE"", ""ITEM_ID"", ""ENTRY_DATE"", ""EXIT_DATE"", ""STATUS"", ""DESEASES"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"") VALUES (@Name, @Code, @ItemId, @EntryDate, @ExitDate, @Status, @Deseases, @Notes, @IsActive, @ModifiedOn, @ModifiedBy) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@EntryDate", DbType.Date)  { Value = o.EntryDate },
new NpgsqlParameter("@ExitDate", DbType.Date)  { Value = (object)o.ExitDate ?? DBNull.Value },
new NpgsqlParameter("@Status", DbType.Boolean)  { Value = o.Status },
new NpgsqlParameter("@Deseases", DbType.String)  { Value = (object)o.Deseases ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ScheduledVaccination", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(ScheduledVaccination o)
        {
            try
            {
                string query = @"UPDATE ""SCHEDULED_VACCINATION"" SET ""ID"" = @Id, ""NAME"" = @Name, ""CODE"" = @Code, ""ITEM_ID"" = @ItemId, ""ENTRY_DATE"" = @EntryDate, ""EXIT_DATE"" = @ExitDate, ""STATUS"" = @Status, ""DESEASES"" = @Deseases, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@ItemId", DbType.Int32)  { Value = o.ItemId },
new NpgsqlParameter("@EntryDate", DbType.Date)  { Value = o.EntryDate },
new NpgsqlParameter("@ExitDate", DbType.Date)  { Value = (object)o.ExitDate ?? DBNull.Value },
new NpgsqlParameter("@Status", DbType.Boolean)  { Value = o.Status },
new NpgsqlParameter("@Deseases", DbType.String)  { Value = (object)o.Deseases ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ScheduledVaccination", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""SCHEDULED_VACCINATION"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ScheduledVaccination", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""SCHEDULED_VACCINATION"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ScheduledVaccination", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ScheduledVaccination", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static ScheduledVaccination GetScheduledVaccinationAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ScheduledVaccination o = new ScheduledVaccination();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.EntryDate = Helper.ConvertToDate(row["ENTRY_DATE"]);
                    o.ExitDate = Helper.ConvertToDate(row["EXIT_DATE"]);
                    o.Status = Helper.ConvertToBoolean(row["STATUS"]);
                    o.Deseases = row["DESEASES"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<ScheduledVaccination> GetScheduledVaccinationAsList(DataTable dt)
        {
            List<ScheduledVaccination> oList = new List<ScheduledVaccination>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ScheduledVaccination o = new ScheduledVaccination();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.Name = row["NAME"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.ItemId = Helper.ConvertToInt(row["ITEM_ID"]);
                    o.EntryDate = Helper.ConvertToDate(row["ENTRY_DATE"]);
                    o.ExitDate = Helper.ConvertToDate(row["EXIT_DATE"]);
                    o.Status = Helper.ConvertToBoolean(row["STATUS"]);
                    o.Deseases = row["DESEASES"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ScheduledVaccination", "GetScheduledVaccinationAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
