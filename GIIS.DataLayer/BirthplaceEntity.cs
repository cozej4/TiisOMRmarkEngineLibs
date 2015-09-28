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
    public partial class Birthplace
    {
        public static Birthplace GetBirthplaceByName(string s)
        {
            try
            {
                string query = @"SELECT * FROM ""BIRTHPLACE"" WHERE ""NAME"" = @ParamValue ";
                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@ParamValue", DbType.String) { Value = s }
                };
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetBirthplaceAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "GetBirthplaceByName", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static DataTable GetBirthplaceListNew()
        {
            try
            {
                string query = string.Format(@"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""BIRTHPLACE"" WHERE ""IS_ACTIVE"" = 'true' ORDER BY ""NAME"";");
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return dt; 
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Birthplace", "GetBirthplaceList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
