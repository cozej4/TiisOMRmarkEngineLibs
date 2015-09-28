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
    public partial class Dose
    {

        #region Properties
        public Int32 Id { get; set; }
        public Int32 ScheduledVaccinationId { get; set; }
        public Int32 DoseNumber { get; set; }
        public Int32 AgeDefinitionId { get; set; }
        public string Fullname { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public Int32? FromAgeDefinitionId { get; set; }
        public Int32? ToAgeDefinitionId { get; set; }
        public AgeDefinitions AgeDefinition
        {
            get
            {
                if (this.AgeDefinitionId > 0)
                    return AgeDefinitions.GetAgeDefinitionsById(this.AgeDefinitionId);
                else
                    return null;
            }
        }
        public ScheduledVaccination ScheduledVaccination
        {
            get
            {
                if (this.ScheduledVaccinationId > 0)
                    return ScheduledVaccination.GetScheduledVaccinationById(this.ScheduledVaccinationId);
                else
                    return null;
            }
        }
        public AgeDefinitions FromAgeDefinition
        {
            get
            {
                if (this.FromAgeDefinitionId > 0)
                    return AgeDefinitions.GetAgeDefinitionsById(this.FromAgeDefinitionId);
                else
                    return null;
            }
        }
        public AgeDefinitions ToAgeDefinition
        {
            get
            {
                if (this.ToAgeDefinitionId > 0)
                    return AgeDefinitions.GetAgeDefinitionsById(this.ToAgeDefinitionId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<Dose> GetDoseList()
        {
            try
            {
                string query = @"SELECT * FROM ""DOSE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetDoseAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDoseList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetDoseForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""DOSE"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDoseForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }




        public static Dose GetDoseById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""DOSE"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Dose", i.ToString(), 4, DateTime.Now, 1);
                return GetDoseAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDoseById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Dose GetDoseByFullname(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""DOSE"" WHERE ""FULLNAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Dose", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetDoseAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "GetDoseByFullname", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Dose o)
        {
            try
            {
                string query = @"INSERT INTO ""DOSE"" (""SCHEDULED_VACCINATION_ID"", ""DOSE_NUMBER"", ""AGE_DEFINITION_ID"", ""FULLNAME"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""FROM_AGE_DEFINITION_ID"", ""TO_AGE_DEFINITION_ID"") VALUES (@ScheduledVaccinationId, @DoseNumber, @AgeDefinitionId, @Fullname, @Notes, @IsActive, @ModifiedOn, @ModifiedBy, @FromAgeDefinitionId, @ToAgeDefinitionId) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ScheduledVaccinationId", DbType.Int32)  { Value = o.ScheduledVaccinationId },
new NpgsqlParameter("@DoseNumber", DbType.Int32)  { Value = o.DoseNumber },
new NpgsqlParameter("@AgeDefinitionId", DbType.Int32)  { Value = o.AgeDefinitionId },
new NpgsqlParameter("@Fullname", DbType.String)  { Value = o.Fullname },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = (object)o.ModifiedOn ?? DBNull.Value },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = (object)o.ModifiedBy ?? DBNull.Value },
new NpgsqlParameter("@FromAgeDefinitionId", DbType.Int32)  { Value = (object)o.FromAgeDefinitionId ?? DBNull.Value },
new NpgsqlParameter("@ToAgeDefinitionId", DbType.Int32)  { Value = (object)o.ToAgeDefinitionId ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Dose", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Dose o)
        {
            try
            {
                string query = @"UPDATE ""DOSE"" SET ""ID"" = @Id, ""SCHEDULED_VACCINATION_ID"" = @ScheduledVaccinationId, ""DOSE_NUMBER"" = @DoseNumber, ""AGE_DEFINITION_ID"" = @AgeDefinitionId, ""FULLNAME"" = @Fullname, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""FROM_AGE_DEFINITION_ID"" = @FromAgeDefinitionId, ""TO_AGE_DEFINITION_ID"" = @ToAgeDefinitionId WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ScheduledVaccinationId", DbType.Int32)  { Value = o.ScheduledVaccinationId },
new NpgsqlParameter("@DoseNumber", DbType.Int32)  { Value = o.DoseNumber },
new NpgsqlParameter("@AgeDefinitionId", DbType.Int32)  { Value = o.AgeDefinitionId },
new NpgsqlParameter("@Fullname", DbType.String)  { Value = o.Fullname },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = (object)o.ModifiedOn ?? DBNull.Value },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = (object)o.ModifiedBy ?? DBNull.Value },
new NpgsqlParameter("@FromAgeDefinitionId", DbType.Int32)  { Value = (object)o.FromAgeDefinitionId ?? DBNull.Value },
new NpgsqlParameter("@ToAgeDefinitionId", DbType.Int32)  { Value = (object)o.ToAgeDefinitionId ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Dose", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""DOSE"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Dose", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""DOSE"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Dose", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Dose", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Dose GetDoseAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Dose o = new Dose();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.ScheduledVaccinationId = Helper.ConvertToInt(row["SCHEDULED_VACCINATION_ID"]);
                    o.DoseNumber = Helper.ConvertToInt(row["DOSE_NUMBER"]);
                    o.AgeDefinitionId = Helper.ConvertToInt(row["AGE_DEFINITION_ID"]);
                    o.Fullname = row["FULLNAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.FromAgeDefinitionId = Helper.ConvertToInt(row["FROM_AGE_DEFINITION_ID"]);
                    o.ToAgeDefinitionId = Helper.ConvertToInt(row["TO_AGE_DEFINITION_ID"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Dose", "GetDoseAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Dose> GetDoseAsList(DataTable dt)
        {
            List<Dose> oList = new List<Dose>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Dose o = new Dose();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.ScheduledVaccinationId = Helper.ConvertToInt(row["SCHEDULED_VACCINATION_ID"]);
                    o.DoseNumber = Helper.ConvertToInt(row["DOSE_NUMBER"]);
                    o.AgeDefinitionId = Helper.ConvertToInt(row["AGE_DEFINITION_ID"]);
                    o.Fullname = row["FULLNAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.FromAgeDefinitionId = Helper.ConvertToInt(row["FROM_AGE_DEFINITION_ID"]);
                    o.ToAgeDefinitionId = Helper.ConvertToInt(row["TO_AGE_DEFINITION_ID"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Dose", "GetDoseAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
