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
    public partial class Uom
    {

        #region Properties
        public string Name { get; set; }
        public string Notes { get; set; }
        public Int32 Id { get; set; }
        #endregion

        #region GetData
        public static List<Uom> GetUomList()
        {
            try
            {
                string query = @"SELECT * FROM ""UOM"" ORDER BY ""ID"";";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetUomAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Uom", "GetUomList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetUomForList()
        {
            try
            {
                string query = @"Select -1 as ""ID"", '-----' as ""NAME"" UNION  SELECT ""ID"", ""NAME"" FROM ""UOM"" ORDER BY ""ID"" ;";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Uom", "GetUomForList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Uom GetUomById(Int32 i)
        {
            try
            {
                string query = @"SELECT * FROM ""UOM"" WHERE ""ID"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.Int32) { Value = i }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Uom", i.ToString(), 4, DateTime.Now, 1);
                return GetUomAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Uom", "GetUomById", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static Uom GetUomByName(string name)
        {
            try
            {
                string query = @"SELECT * FROM ""UOM"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = name }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                //AuditTable.InsertEntity("Uom", i.ToString(), 4, DateTime.Now, 1);
                return GetUomAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Uom", "GetUomByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        #endregion

        #region CRUD
        public static int Insert(Uom o)
        {
            try
            {
                string query = @"INSERT INTO ""UOM"" (""NAME"", ""NOTES"") VALUES (@Name, @Notes) returning ""ID"" ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value }};
                object id = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Uom", id.ToString(), 1, DateTime.Now, 1);
                return int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Uom", "Insert", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Update(Uom o)
        {
            try
            {
                string query = @"UPDATE ""UOM"" SET ""NAME"" = @Name, ""NOTES"" = @Notes, ""ID"" = @Id WHERE ""ID"" = @Id ";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Name", DbType.String)  { Value = o.Name },
new NpgsqlParameter("@Notes", DbType.String)  { Value = (object)o.Notes ?? DBNull.Value },
new NpgsqlParameter("@Id", DbType.Int32) { Value = o.Id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Uom", o.Id.ToString(), 2, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Uom", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
            }
            return -1;
        }

        public static int Delete(int id)
        {
            try
            {
                string query = @"DELETE FROM ""UOM"" WHERE ""ID"" = @Id";
                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@Id", DbType.Int32) { Value = id }
};
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Uom", id.ToString(), 3, DateTime.Now, 1);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Uom", "Delete", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        #endregion

        #region Helper Methods
        public static Uom GetUomAsObject(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Uom o = new Uom();
                    o.Name = row["NAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    return o;
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Uom", "GetUomAsObject", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return null;
        }

        public static List<Uom> GetUomAsList(DataTable dt)
        {
            List<Uom> oList = new List<Uom>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Uom o = new Uom();
                    o.Name = row["NAME"].ToString();
                    o.Notes = row["NOTES"].ToString();
                    o.Id = Helper.ConvertToInt(row["ID"]);
                    oList.Add(o);
                }
                catch (Exception ex)
                {
                    Log.InsertEntity("Uom", "GetUomAsList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                    throw ex;
                }
            }
            return oList;
        }
        #endregion

    }
}