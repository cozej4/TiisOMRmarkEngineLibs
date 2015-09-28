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
    public partial class VaccinationQueue
    {

        #region Properties
        public Int32 Id { get; set; }
        public string BarcodeId { get; set; }
        public Int32 HealthFacilityId { get; set; }
        public Int32 UserId { get; set; }
        public DateTime Date { get; set; }
        #endregion

        #region GetData
        public static List<VaccinationQueue> GetVaccinationQueueList()
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_QUEUE"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationQueueAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "GetVaccinationQueueList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<VaccinationQueue> GetPagedVaccinationQueueList(ref int maximumRows, ref int startRowIndex, string where)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_QUEUE"" WHERE " + where + " OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationQueueAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "GetPagedVaccinationQueueList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountVaccinationQueueList(string where)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""VACCINATION_QUEUE"" WHERE " + where + ";";
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "GetCountVaccinationQueueList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static VaccinationQueue GetVaccinationQueueById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""VACCINATION_QUEUE"" WHERE ""ID"" = " + i + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetVaccinationQueueAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "GetVaccinationQueueById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        

        #endregion

        #region CRUD
        public static int Insert(VaccinationQueue o)
        {
            try
            {
                string query = @"INSERT INTO ""VACCINATION_QUEUE"" (""BARCODE_ID"", ""HEALTH_FACILITY_ID"", ""DATE"", ""USER_ID"") VALUES ('" + o.BarcodeId + @"', " + o.HealthFacilityId + @", '" + o.Date + @"', " + o.UserId + @") returning ""ID"" ";
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                Log.InsertEntity(1, "Success", id.ToString(), "VaccinationQueue", "Insert");
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(VaccinationQueue o, int id)
        {
            try
            {
                string query = @"UPDATE ""VACCINATION_QUEUE"" SET ""ID"" = " + o.Id + @", ""BARCODE_ID"" = '" + o.BarcodeId + @"', ""HEALTH_FACILITY_ID"" = " + o.HealthFacilityId + @", ""DATE"" = " + o.Date + @", ""USER_ID"" = " + o.UserId + @"WHERE ""ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                Log.InsertEntity(2, "Success", id.ToString(), "VaccinationQueue", "Update");
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""VACCINATION_QUEUE"" WHERE ""ID"" = " + id;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("VaccinationQueue", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static VaccinationQueue GetVaccinationQueueAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    VaccinationQueue o = new VaccinationQueue();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.BarcodeId = row["BARCODE_ID"].ToString();
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.UserId = Helper.ConvertToInt(row["USER_ID"]);
                    o.Date = Helper.ConvertToDate(row["DATE"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("VaccinationQueue", "GetVaccinationQueueAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<VaccinationQueue> GetVaccinationQueueAsList(DataTable dt)
        {
            List<VaccinationQueue> oList = new List<VaccinationQueue>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    VaccinationQueue o = new VaccinationQueue();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.BarcodeId = row["BARCODE_ID"].ToString();
                    o.HealthFacilityId = Helper.ConvertToInt(row["HEALTH_FACILITY_ID"]);
                    o.UserId = Helper.ConvertToInt(row["USER_ID"]);
                    o.Date = Helper.ConvertToDate(row["DATE"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("VaccinationQueue", "GetVaccinationQueueAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;

        }
        #endregion

    }
}
