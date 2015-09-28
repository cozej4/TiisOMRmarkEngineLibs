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
using System.Data;
using System.Linq;
using System.Text;
using Npgsql;
//using System.Threading.Tasks;

namespace GIIS.DataLayer
{
    public partial class Place
    {
        public static List<Place> GetPlaceByParentId(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" ";
                if (i.HasValue)
                    query += @" WHERE ""PARENT_ID"" = " + i.Value;
                query += @" ORDER BY ""NAME"" ";

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceByParentId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Place> GetPlaceByHealthFacilityId(Int32? i)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" ";
                if (i.HasValue)
                    query += @" WHERE ""HEALTH_FACILITY_ID"" = " + i.Value;

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceByHealthFacilityId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetPlaceListbyHealthFacility(Int32 hfId)
        {
            try
            {
                string str = HealthFacility.GetAllChildsForOneHealthFacility(hfId);
                string query = @"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""PLACE"" WHERE ""HEALTH_FACILITY_ID"" in ( " + str + @")  ORDER BY ""NAME"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt; // GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceListbyHealthFacility", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Place> GetPagedPlaceList(string name, ref int maximumRows, ref int startRowIndex)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE""  "
                                + @" WHERE ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '')"
                                + @" OFFSET @StartRowIndex LIMIT @MaximumRows;";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") },
                    new NpgsqlParameter("@MaximumRows", DbType.Int32) { Value = maximumRows },
                    new NpgsqlParameter("@StartRowIndex", DbType.Int32) { Value = startRowIndex }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPagedPlaceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static int GetCountPlaceList(string name)
        {
            try
            {
                string query = @"SELECT count(*) FROM ""PLACE""  "
                                + @" WHERE ( UPPER(""NAME"") like @Name OR @Name is null or @Name = '');";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@Name", DbType.String) { Value = ("%" + name + "%") }
                };

                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, parameters);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetCountPlaceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Place> GetLeafPlaces()
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" WHERE ""LEAF"" = 'true';";

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetLeafPlaces", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Place GetPlaceByName(string s, int parent_id)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" WHERE ""NAME"" = '" + s + @"' and ""PARENT_ID"" =" + parent_id + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetPlaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static Place GetPlaceByCode(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""PLACE"" WHERE ""CODE"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
{
new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
};
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                AuditTable.InsertEntity("Place", string.Format("RecordId: {0}", s), 4, DateTime.Now, 1);
                return GetPlaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlaceByCode", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Place> GetPlacesByHealthFacilityIdSinceLastLogin(int userId)
        {
            try
            {
                User user = User.GetUserById(userId);
                string query = string.Format(@"select P.* from ""PLACE"" P
                                                WHERE P.""HEALTH_FACILITY_ID"" = @hfId 
                                                AND P.""MODIFIED_ON"" <= @lastlogin ");
                                               
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@hfId", DbType.Int32) { Value = user.HealthFacilityId.ToString()},
                     new NpgsqlParameter("@lastlogin", DbType.DateTime) { Value = user.Lastlogin}
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Child", "GetPlacesByHealthFacilityIdSinceLastLogin", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static List<Place> GetPlacesByList(string placeList)
        {
           
            try
            {
                string query = @"SELECT * FROM ""PLACE"" WHERE ""ID"" = ANY( CAST( string_to_array(@pList, ',' ) AS INTEGER[] ));";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@pList", DbType.String) { Value = placeList }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetPlaceAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Place", "GetPlacesByList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
