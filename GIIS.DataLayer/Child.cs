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
    public partial class Child
    {

        #region Properties
        public Int32 Id { get; set; }
        public string SystemId { get; set; }
        public string Firstname1 { get; set; }
        public string Firstname2 { get; set; }
        public string Lastname1 { get; set; }
        public string Lastname2 { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Gender { get; set; }
        public Int32 HealthcenterId { get; set; }
        public Int32? BirthplaceId { get; set; }
        public Int32? CommunityId { get; set; }
        public Int32? DomicileId { get; set; }
        public Int32 StatusId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string MotherId { get; set; }
        public string MotherFirstname { get; set; }
        public string MotherLastname { get; set; }
        public string FatherId { get; set; }
        public string FatherFirstname { get; set; }
        public string FatherLastname { get; set; }
        public string CaretakerId { get; set; }
        public string CaretakerFirstname { get; set; }
        public string CaretakerLastname { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int32 ModifiedBy { get; set; }
        public string IdentificationNo1 { get; set; }
        public string IdentificationNo2 { get; set; }
        public string IdentificationNo3 { get; set; }
        public string BarcodeId { get; set; }
        public string TempId { get; set; }
        public string Name
        {
            get
            {
                string fname = Firstname1 + " " + Firstname2 + " " + Lastname1;
                return fname;
            }
        }
        public HealthFacility Healthcenter
        {
            get
            {
                if (this.HealthcenterId > 0)
                    return HealthFacility.GetHealthFacilityById(this.HealthcenterId);
                else
                    return null;
            }
        }
        public Status Status
        {
            get
            {
                if (this.StatusId > 0)
                    return Status.GetStatusById(this.StatusId);
                else
                    return null;
            }
        }
        public Birthplace Birthplace
        {
            get
            {
                if (this.BirthplaceId > 0)
                    return Birthplace.GetBirthplaceById(this.BirthplaceId);
                else
                    return null;
            }
        }
        public Place Domicile
        {
            get
            {
                if (this.DomicileId != 0)
                    return Place.GetPlaceById(this.DomicileId);
                else
                    return null;
            }
        }
        public Community Community
        {
            get
            {
                if (this.CommunityId > 0)
                    return Community.GetCommunityById(this.CommunityId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<Child> GetChildList()
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetChildForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""CHILD"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Child GetChildById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                    {
                    new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
                    };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Child", i.ToString(), 4, DateTime.Now, 1);
                return GetChildAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetChildById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region CRUD
        public static int Insert(Child o)
        {
            try
            {
                string query = @"INSERT INTO ""CHILD"" (""SYSTEM_ID"", ""FIRSTNAME1"", ""FIRSTNAME2"", ""LASTNAME1"", ""LASTNAME2"", ""BIRTHDATE"", ""GENDER"", ""HEALTHCENTER_ID"", ""BIRTHPLACE_ID"", ""COMMUNITY_ID"", ""DOMICILE_ID"", ""STATUS_ID"", ""ADDRESS"", ""PHONE"", ""MOBILE"", ""EMAIL"", ""MOTHER_ID"", ""MOTHER_FIRSTNAME"", ""MOTHER_LASTNAME"", ""FATHER_ID"", ""FATHER_FIRSTNAME"", ""FATHER_LASTNAME"", ""CARETAKER_ID"", ""CARETAKER_FIRSTNAME"", ""CARETAKER_LASTNAME"", ""NOTES"", ""IS_ACTIVE"", ""MODIFIED_ON"", ""MODIFIED_BY"", ""IDENTIFICATION_NO1"", ""IDENTIFICATION_NO2"", ""IDENTIFICATION_NO3"", ""BARCODE_ID"", ""TEMP_ID"") VALUES (@SystemId, @Firstname1, @Firstname2, @Lastname1, @Lastname2, @Birthdate, @Gender, @HealthcenterId, @BirthplaceId, @CommunityId, @DomicileId, @StatusId, @Address, @Phone, @Mobile, @Email, @MotherId, @MotherFirstname, @MotherLastname, @FatherId, @FatherFirstname, @FatherLastname, @CaretakerId, @CaretakerFirstname, @CaretakerLastname, @Notes, @IsActive, @ModifiedOn, @ModifiedBy, @IdentificationNo1, @IdentificationNo2, @IdentificationNo3, @BarcodeId, @TempId) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@SystemId", DbType.String)  { Value = o.SystemId },
new NpgsqlParameter("@Firstname1", DbType.String)  { Value = (object)o.Firstname1 ?? DBNull.Value },
new NpgsqlParameter("@Firstname2", DbType.String)  { Value = (object)o.Firstname2 ?? DBNull.Value },
new NpgsqlParameter("@Lastname1", DbType.String)  { Value = (object)o.Lastname1 ?? DBNull.Value },
new NpgsqlParameter("@Lastname2", DbType.String)  { Value = (object)o.Lastname2 ?? DBNull.Value },
new NpgsqlParameter("@Birthdate", DbType.Date)  { Value = o.Birthdate },
new NpgsqlParameter("@Gender", DbType.Boolean)  { Value = o.Gender },
new NpgsqlParameter("@HealthcenterId", DbType.Int32)  { Value = o.HealthcenterId },
new NpgsqlParameter("@BirthplaceId", DbType.Int32)  { Value = (object)o.BirthplaceId ?? DBNull.Value },
new NpgsqlParameter("@CommunityId", DbType.Int32)  { Value = (object)o.CommunityId ?? DBNull.Value },
new NpgsqlParameter("@DomicileId", DbType.Int32)  { Value = (object)o.DomicileId ?? DBNull.Value },
new NpgsqlParameter("@StatusId", DbType.Int32)  { Value = o.StatusId },
new NpgsqlParameter("@Address", DbType.String)  { Value = (object)o.Address ?? DBNull.Value },
new NpgsqlParameter("@Phone", DbType.String)  { Value = (object)o.Phone ?? DBNull.Value },
new NpgsqlParameter("@Mobile", DbType.String)  { Value = (object)o.Mobile ?? DBNull.Value },
new NpgsqlParameter("@Email", DbType.String)  { Value = (object)o.Email ?? DBNull.Value },
new NpgsqlParameter("@MotherId", DbType.String)  { Value = (object)o.MotherId ?? DBNull.Value },
new NpgsqlParameter("@MotherFirstname", DbType.String)  { Value = (object)o.MotherFirstname ?? DBNull.Value },
new NpgsqlParameter("@MotherLastname", DbType.String)  { Value = (object)o.MotherLastname ?? DBNull.Value },
new NpgsqlParameter("@FatherId", DbType.String)  { Value = (object)o.FatherId ?? DBNull.Value },
new NpgsqlParameter("@FatherFirstname", DbType.String)  { Value = (object)o.FatherFirstname ?? DBNull.Value },
new NpgsqlParameter("@FatherLastname", DbType.String)  { Value = (object)o.FatherLastname ?? DBNull.Value },
new NpgsqlParameter("@CaretakerId", DbType.String)  { Value = (object)o.CaretakerId ?? DBNull.Value },
new NpgsqlParameter("@CaretakerFirstname", DbType.String)  { Value = (object)o.CaretakerFirstname ?? DBNull.Value },
new NpgsqlParameter("@CaretakerLastname", DbType.String)  { Value = (object)o.CaretakerLastname ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive }
,
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@IdentificationNo1", DbType.String)  { Value = (object)o.IdentificationNo1 ?? DBNull.Value },
new NpgsqlParameter("@IdentificationNo2", DbType.String)  { Value = (object)o.IdentificationNo2 ?? DBNull.Value },
new NpgsqlParameter("@IdentificationNo3", DbType.String)  { Value = (object)o.IdentificationNo3 ?? DBNull.Value },
new NpgsqlParameter("@BarcodeId", DbType.String)  { Value = (object)o.BarcodeId ?? DBNull.Value },
new NpgsqlParameter("@TempId", DbType.String)  { Value = (object)o.TempId ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Child", id.ToString(), 1, DateTime.Now, o.ModifiedBy);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Child o)
        {
            try
            {
                string query = @"UPDATE ""CHILD"" SET ""ID"" = @Id, ""SYSTEM_ID"" = @SystemId, ""FIRSTNAME1"" = @Firstname1, ""FIRSTNAME2"" = @Firstname2, ""LASTNAME1"" = @Lastname1, ""LASTNAME2"" = @Lastname2, ""BIRTHDATE"" = @Birthdate, ""GENDER"" = @Gender, ""HEALTHCENTER_ID"" = @HealthcenterId, ""BIRTHPLACE_ID"" = @BirthplaceId, ""COMMUNITY_ID"" = @CommunityId, ""DOMICILE_ID"" = @DomicileId, ""STATUS_ID"" = @StatusId, ""ADDRESS"" = @Address, ""PHONE"" = @Phone, ""MOBILE"" = @Mobile, ""EMAIL"" = @Email, ""MOTHER_ID"" = @MotherId, ""MOTHER_FIRSTNAME"" = @MotherFirstname, ""MOTHER_LASTNAME"" = @MotherLastname, ""FATHER_ID"" = @FatherId, ""FATHER_FIRSTNAME"" = @FatherFirstname, ""FATHER_LASTNAME"" = @FatherLastname, ""CARETAKER_ID"" = @CaretakerId, ""CARETAKER_FIRSTNAME"" = @CaretakerFirstname, ""CARETAKER_LASTNAME"" = @CaretakerLastname, ""NOTES"" = @Notes, ""IS_ACTIVE"" = @IsActive, ""MODIFIED_ON"" = @ModifiedOn, ""MODIFIED_BY"" = @ModifiedBy, ""IDENTIFICATION_NO1"" = @IdentificationNo1, ""IDENTIFICATION_NO2"" = @IdentificationNo2, ""IDENTIFICATION_NO3"" = @IdentificationNo3, ""BARCODE_ID"" = @BarcodeId, ""TEMP_ID"" = @TempId WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@SystemId", DbType.String)  { Value = o.SystemId },
new NpgsqlParameter("@Firstname1", DbType.String)  { Value = (object)o.Firstname1 ?? DBNull.Value },
new NpgsqlParameter("@Firstname2", DbType.String)  { Value = (object)o.Firstname2 ?? DBNull.Value },
new NpgsqlParameter("@Lastname1", DbType.String)  { Value = (object)o.Lastname1 ?? DBNull.Value },
new NpgsqlParameter("@Lastname2", DbType.String)  { Value = (object)o.Lastname2 ?? DBNull.Value },
new NpgsqlParameter("@Birthdate", DbType.Date)  { Value = o.Birthdate },
new NpgsqlParameter("@Gender", DbType.Boolean)  { Value = o.Gender },
new NpgsqlParameter("@HealthcenterId", DbType.Int32)  { Value = o.HealthcenterId },
new NpgsqlParameter("@BirthplaceId", DbType.Int32)  { Value = (object)o.BirthplaceId ?? DBNull.Value },
new NpgsqlParameter("@CommunityId", DbType.Int32)  { Value = (object)o.CommunityId ?? DBNull.Value },
new NpgsqlParameter("@DomicileId", DbType.Int32)  { Value = (object)o.DomicileId ?? DBNull.Value },
new NpgsqlParameter("@StatusId", DbType.Int32)  { Value = o.StatusId },
new NpgsqlParameter("@Address", DbType.String)  { Value = (object)o.Address ?? DBNull.Value },
new NpgsqlParameter("@Phone", DbType.String)  { Value = (object)o.Phone ?? DBNull.Value },
new NpgsqlParameter("@Mobile", DbType.String)  { Value = (object)o.Mobile ?? DBNull.Value },
new NpgsqlParameter("@Email", DbType.String)  { Value = (object)o.Email ?? DBNull.Value },
new NpgsqlParameter("@MotherId", DbType.String)  { Value = (object)o.MotherId ?? DBNull.Value },
new NpgsqlParameter("@MotherFirstname", DbType.String)  { Value = (object)o.MotherFirstname ?? DBNull.Value },
new NpgsqlParameter("@MotherLastname", DbType.String)  { Value = (object)o.MotherLastname ?? DBNull.Value },
new NpgsqlParameter("@FatherId", DbType.String)  { Value = (object)o.FatherId ?? DBNull.Value },
new NpgsqlParameter("@FatherFirstname", DbType.String)  { Value = (object)o.FatherFirstname ?? DBNull.Value },
new NpgsqlParameter("@FatherLastname", DbType.String)  { Value = (object)o.FatherLastname ?? DBNull.Value },
new NpgsqlParameter("@CaretakerId", DbType.String)  { Value = (object)o.CaretakerId ?? DBNull.Value },
new NpgsqlParameter("@CaretakerFirstname", DbType.String)  { Value = (object)o.CaretakerFirstname ?? DBNull.Value },
new NpgsqlParameter("@CaretakerLastname", DbType.String)  { Value = (object)o.CaretakerLastname ?? DBNull.Value },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@IsActive", DbType.Boolean)  { Value = o.IsActive },
new NpgsqlParameter("@ModifiedOn", DbType.DateTime)  { Value = o.ModifiedOn },
new NpgsqlParameter("@ModifiedBy", DbType.Int32)  { Value = o.ModifiedBy },
new NpgsqlParameter("@IdentificationNo1", DbType.String)  { Value = (object)o.IdentificationNo1 ?? DBNull.Value },
new NpgsqlParameter("@IdentificationNo2", DbType.String)  { Value = (object)o.IdentificationNo2 ?? DBNull.Value },
new NpgsqlParameter("@IdentificationNo3", DbType.String)  { Value = (object)o.IdentificationNo3 ?? DBNull.Value },
new NpgsqlParameter("@BarcodeId", DbType.String)  { Value = (object)o.BarcodeId ?? DBNull.Value },
new NpgsqlParameter("@TempId", DbType.String)  { Value = (object)o.TempId ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Child", o.Id.ToString(), 2, DateTime.Now, o.ModifiedBy);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""CHILD"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Child", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int Remove(int id)
        {
            try
            {
                string query = @"UPDATE ""CHILD"" SET ""IS_ACTIVE"" = 'false' WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Child", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "Remove", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        public static Child GetChildAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Child o = new Child();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.SystemId = row["SYSTEM_ID"].ToString();
                    o.Firstname1 = row["FIRSTNAME1"].ToString();
                    o.Firstname2 = row["FIRSTNAME2"].ToString();
                    o.Lastname1 = row["LASTNAME1"].ToString();
                    o.Lastname2 = row["LASTNAME2"].ToString();
                    o.Birthdate = Helper.ConvertToDate(row["BIRTHDATE"]);
                    o.Gender = Helper.ConvertToBoolean(row["GENDER"]);
                    o.HealthcenterId = Helper.ConvertToInt(row["HEALTHCENTER_ID"]);
                    o.BirthplaceId = Helper.ConvertToInt(row["BIRTHPLACE_ID"]);
                    o.CommunityId = Helper.ConvertToInt(row["COMMUNITY_ID"]);
                    o.DomicileId = Helper.ConvertToInt(row["DOMICILE_ID"]);
                    o.StatusId = Helper.ConvertToInt(row["STATUS_ID"]);
                    o.Address = row["ADDRESS"].ToString();
                    o.Phone = row["PHONE"].ToString();
                    o.Mobile = row["MOBILE"].ToString();
                    o.Email = row["EMAIL"].ToString();
                    o.MotherId = row["MOTHER_ID"].ToString();
                    o.MotherFirstname = row["MOTHER_FIRSTNAME"].ToString();
                    o.MotherLastname = row["MOTHER_LASTNAME"].ToString();
                    o.FatherId = row["FATHER_ID"].ToString();
                    o.FatherFirstname = row["FATHER_FIRSTNAME"].ToString();
                    o.FatherLastname = row["FATHER_LASTNAME"].ToString();
                    o.CaretakerId = row["CARETAKER_ID"].ToString();
                    o.CaretakerFirstname = row["CARETAKER_FIRSTNAME"].ToString();
                    o.CaretakerLastname = row["CARETAKER_LASTNAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.IdentificationNo1 = row["IDENTIFICATION_NO1"].ToString();
                    o.IdentificationNo2 = row["IDENTIFICATION_NO2"].ToString();
                    o.IdentificationNo3 = row["IDENTIFICATION_NO3"].ToString();
                    o.BarcodeId = row["BARCODE_ID"].ToString();
                    o.TempId = row["TEMP_ID"].ToString();
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Child", "GetChildAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Child> GetChildAsList(DataTable dt)
        {
            List<Child> oList = new List<Child>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Child o = new Child();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.SystemId = row["SYSTEM_ID"].ToString();
                    o.Firstname1 = row["FIRSTNAME1"].ToString();
                    o.Firstname2 = row["FIRSTNAME2"].ToString();
                    o.Lastname1 = row["LASTNAME1"].ToString();
                    o.Lastname2 = row["LASTNAME2"].ToString();
                    o.Birthdate = Helper.ConvertToDate(row["BIRTHDATE"]);
                    o.Gender = Helper.ConvertToBoolean(row["GENDER"]);
                    o.HealthcenterId = Helper.ConvertToInt(row["HEALTHCENTER_ID"]);
                    o.BirthplaceId = Helper.ConvertToInt(row["BIRTHPLACE_ID"]);
                    o.CommunityId = Helper.ConvertToInt(row["COMMUNITY_ID"]);
                    o.DomicileId = Helper.ConvertToInt(row["DOMICILE_ID"]);
                    o.StatusId = Helper.ConvertToInt(row["STATUS_ID"]);
                    o.Address = row["ADDRESS"].ToString();
                    o.Phone = row["PHONE"].ToString();
                    o.Mobile = row["MOBILE"].ToString();
                    o.Email = row["EMAIL"].ToString();
                    o.MotherId = row["MOTHER_ID"].ToString();
                    o.MotherFirstname = row["MOTHER_FIRSTNAME"].ToString();
                    o.MotherLastname = row["MOTHER_LASTNAME"].ToString();
                    o.FatherId = row["FATHER_ID"].ToString();
                    o.FatherFirstname = row["FATHER_FIRSTNAME"].ToString();
                    o.FatherLastname = row["FATHER_LASTNAME"].ToString();
                    o.CaretakerId = row["CARETAKER_ID"].ToString();
                    o.CaretakerFirstname = row["CARETAKER_FIRSTNAME"].ToString();
                    o.CaretakerLastname = row["CARETAKER_LASTNAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.IsActive = Helper.ConvertToBoolean(row["IS_ACTIVE"]);
                    o.ModifiedOn = Helper.ConvertToDate(row["MODIFIED_ON"]);
                    o.ModifiedBy = Helper.ConvertToInt(row["MODIFIED_BY"]);
                    o.IdentificationNo1 = row["IDENTIFICATION_NO1"].ToString();
                    o.IdentificationNo2 = row["IDENTIFICATION_NO2"].ToString();
                    o.IdentificationNo3 = row["IDENTIFICATION_NO3"].ToString();
                    o.BarcodeId = row["BARCODE_ID"].ToString();
                    o.TempId = row["TEMP_ID"].ToString();
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Child", "GetChildAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }

    
        #endregion

    }
}
