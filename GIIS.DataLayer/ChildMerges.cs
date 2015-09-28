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
    public partial class ChildMerges
    {

        #region Properties
        public Int32 Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Int32 ChildId { get; set; }
        public Int32 SubsumedId { get; set; }
        public Child Child
        {
            get
            {
                if (this.ChildId > 0)
                    return Child.GetChildById(this.ChildId);
                else
                    return null;
            }
        }
        public Child Subsumed
        {
            get
            {
                if (this.SubsumedId > 0)
                    return Child.GetChildById(this.SubsumedId);
                else
                    return null;
            }
        }
        #endregion

        #region GetData
        public static List<ChildMerges> GetChildMergesList()
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_MERGES"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildMergesAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildMerges", "GetChildMergesList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetChildMergesForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""CHILD_MERGES"" WHERE ""IS_ACTIVE"" = true ORDER BY ""NAME"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildMerges", "GetChildMergesForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }


        public static ChildMerges GetChildMergesById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_MERGES"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildMerges", i.ToString(), 4, DateTime.Now, 1);
                return GetChildMergesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildMerges", "GetChildMergesById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ChildMerges GetChildMergesBySubsumedId(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_MERGES"" WHERE ""SUBSUMED_ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildMerges", i.ToString(), 4, DateTime.Now, 1);
                return GetChildMergesAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildMerges", "GetChildMergesBySubsumedId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region CRUD
        public static int Insert(ChildMerges o)
        {
            try
            {
                string query = @"INSERT INTO ""CHILD_MERGES"" (""EFFECTIVE_DATE"", ""CHILD_ID"", ""SUBSUMED_ID"") VALUES (@EffectiveDate, @ChildId, @SubsumedId) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@EffectiveDate", DbType.Date)  { Value = o.EffectiveDate },
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@SubsumedId", DbType.Int32)  { Value = o.SubsumedId }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildMerges", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildMerges", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(ChildMerges o)
        {
            try
            {
                string query = @"UPDATE ""CHILD_MERGES"" SET ""ID"" = @Id, ""EFFECTIVE_DATE"" = @EffectiveDate, ""CHILD_ID"" = @ChildId, ""SUBSUMED_ID"" = @SubsumedId WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@EffectiveDate", DbType.Date)  { Value = o.EffectiveDate },
new NpgsqlParameter("@ChildId", DbType.Int32)  { Value = o.ChildId },
new NpgsqlParameter("@SubsumedId", DbType.Int32)  { Value = o.SubsumedId },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildMerges", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildMerges", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""CHILD_MERGES"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("ChildMerges", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildMerges", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static ChildMerges GetChildMergesAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ChildMerges o = new ChildMerges();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.EffectiveDate = Helper.ConvertToDate(row["EFFECTIVE_DATE"]);
                    o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
                    o.SubsumedId = Helper.ConvertToInt(row["SUBSUMED_ID"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ChildMerges", "GetChildMergesAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<ChildMerges> GetChildMergesAsList(DataTable dt)
        {
            List<ChildMerges> oList = new List<ChildMerges>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    ChildMerges o = new ChildMerges();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    o.EffectiveDate = Helper.ConvertToDate(row["EFFECTIVE_DATE"]);
                    o.ChildId = Helper.ConvertToInt(row["CHILD_ID"]);
                    o.SubsumedId = Helper.ConvertToInt(row["SUBSUMED_ID"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("ChildMerges", "GetChildMergesAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}
