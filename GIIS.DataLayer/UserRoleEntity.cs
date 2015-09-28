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


namespace GIIS.DataLayer
{
   partial class UserRole
    {
       public static UserRole GetUserRoleByUserId(Int32 i)
       {
           try
           {
               string query = @"SELECT * FROM ""USER_ROLE"" WHERE ""USER_ID"" = " + i + "";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return GetUserRoleAsObject(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("UserRole", "GetUserRoleByUserId", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
       public static List<UserRole> GetUserRoleListByUserId(Int32 i)
       {
           try
           {
               string query = @"SELECT * FROM ""USER_ROLE"" WHERE ""USER_ID"" = " + i + "";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return GetUserRoleAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("UserRole", "GetUserRoleListByUserId", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
       public static int DeleteByUserAndRole(int userId, int roleId)
       {
           try
           {
               string query = @"DELETE FROM ""USER_ROLE"" WHERE ""ROLE_ID"" = " + roleId + @" AND ""USER_ID"" = " + userId;
               int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
               return rowAffected;
           }
           catch (Exception ex)
           {
               Log.InsertEntity("UserRole", "DeleteByUserAndRole", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
       public static int Exists(int userId, int roleId)
       {
           try
           {
               string query = @"SELECT COUNT(*) FROM ""USER_ROLE"" WHERE ""USER_ID"" = " + userId + @" AND ""ROLE_ID"" = " + roleId;
               object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
               return int.Parse(count.ToString());
           }
           catch (Exception ex)
           {
               Log.InsertEntity("UserRole", "Exists", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
    }

}
