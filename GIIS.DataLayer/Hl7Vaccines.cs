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
    public partial class Hl7Vaccines
    {

        #region Properties
        public Int32 Id { get; set; }
        public String CvxCode { get; set; }
        public string Code { get; set; }
        public string Fullname { get; set; }
        public string Notes { get; set; }
        public bool VaccineStatus { get; set; }
        public Int32 InternalId { get; set; }
        public bool NonVaccine { get; set; }
        public DateTime UpdateDate { get; set; }
        #endregion

        #region GetData
        public static List<Hl7Vaccines> GetHl7VaccinesList()
        {
            try
            {
                string query = @"SELECT * FROM ""HL7_VACCINES"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetHl7VaccinesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        //public static DataTable GetHl7VaccinesForList()
        //{
        //    try
        //    {
        //        string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""HL7_VACCINES"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
        //        DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
        //        throw ex;
        //    }
        //}
        public static Hl7Vaccines GetHl7VaccinesById(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""HL7_VACCINES"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Hl7Vaccines", i.ToString(), 4, DateTime.Now, 1);
                return GetHl7VaccinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Hl7Vaccines GetHl7VaccinesByCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""HL7_VACCINES"" WHERE ""CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Hl7Vaccines", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetHl7VaccinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Hl7Vaccines GetHl7VaccinesByCvxCode(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""HL7_VACCINES"" WHERE ""CVX_CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Hl7Vaccines", i.ToString(), 4, DateTime.Now, 1);
                return GetHl7VaccinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesByCvxCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Hl7Vaccines GetHl7VaccinesByInternalId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""HL7_VACCINES"" WHERE ""INTERNAL_ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Hl7Vaccines", i.ToString(), 4, DateTime.Now, 1);
                return GetHl7VaccinesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesByInternalId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Hl7Vaccines o)
        {
            try
            {
                string query = @"INSERT INTO ""HL7_VACCINES"" (""CVX_CODE"", ""CODE"", ""FULLNAME"", ""NOTES"", ""VACCINE_STATUS"", ""INTERNAL_ID"", ""NON_VACCINE"", ""UPDATE_DATE"") VALUES (@CvxCode, @Code, @Fullname, @Notes, @VaccineStatus, @InternalId, @NonVaccine, @UpdateDate) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@CvxCode", DbType.Int32)  { Value = o.CvxCode },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@Fullname", DbType.String)  { Value = o.Fullname },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@VaccineStatus", DbType.Boolean)  { Value = o.VaccineStatus },
new NpgsqlParameter("@InternalId", DbType.Int32)  { Value = o.InternalId },
new NpgsqlParameter("@NonVaccine", DbType.Boolean)  { Value = o.NonVaccine },
new NpgsqlParameter("@UpdateDate", DbType.Date)  { Value = o.UpdateDate }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Hl7Vaccines", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Hl7Vaccines o)
        {
            try
            {
                string query = @"UPDATE ""HL7_VACCINES"" SET ""ID"" = @Id, ""CVX_CODE"" = @CvxCode, ""CODE"" = @Code, ""FULLNAME"" = @Fullname, ""NOTES"" = @Notes, ""VACCINE_STATUS"" = @VaccineStatus, ""INTERNAL_ID"" = @InternalId, ""NON_VACCINE"" = @NonVaccine, ""UPDATE_DATE"" = @UpdateDate WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@CvxCode", DbType.Int32)  { Value = o.CvxCode },
new NpgsqlParameter("@Code", DbType.String)  { Value = o.Code },
new NpgsqlParameter("@Fullname", DbType.String)  { Value = o.Fullname },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@VaccineStatus", DbType.Boolean)  { Value = o.VaccineStatus },
new NpgsqlParameter("@InternalId", DbType.Int32)  { Value = o.InternalId },
new NpgsqlParameter("@NonVaccine", DbType.Boolean)  { Value = o.NonVaccine },
new NpgsqlParameter("@UpdateDate", DbType.Date)  { Value = o.UpdateDate },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Hl7Vaccines", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""HL7_VACCINES"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Hl7Vaccines", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Hl7Vaccines", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static Hl7Vaccines GetHl7VaccinesAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Hl7Vaccines o = new Hl7Vaccines();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.CvxCode = row["CVX_CODE"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.Fullname = row["FULLNAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.VaccineStatus = Helper.ConvertToBoolean(row["VACCINE_STATUS"]);
                    o.InternalId = Helper.ConvertToInt(row["INTERNAL_ID"]);
                    o.NonVaccine = Helper.ConvertToBoolean(row["NON_VACCINE"]);
                    o.UpdateDate = Helper.ConvertToDate(row["UPDATE_DATE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Hl7Vaccines> GetHl7VaccinesAsList(DataTable dt)
        {
            List<Hl7Vaccines> oList = new List<Hl7Vaccines>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Hl7Vaccines o = new Hl7Vaccines();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.CvxCode = row["CVX_CODE"].ToString();
                    o.Code = row["CODE"].ToString();
                    o.Fullname = row["FULLNAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.VaccineStatus = Helper.ConvertToBoolean(row["VACCINE_STATUS"]);
                    o.InternalId = Helper.ConvertToInt(row["INTERNAL_ID"]);
                    o.NonVaccine = Helper.ConvertToBoolean(row["NON_VACCINE"]);
                    o.UpdateDate = Helper.ConvertToDate(row["UPDATE_DATE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
