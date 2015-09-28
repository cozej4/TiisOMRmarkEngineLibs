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
   partial class Role
    {
       public static ICollection<Role> GetRoleByStatus(bool s)
       {
           try
           {
               string query = @"SELECT * FROM ""ROLE"" WHERE ""IS_ACTIVE"" = '" + s + @"'  ORDER BY ""NAME"" ";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return GetRoleAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Role", "GetRoleByStatus", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

      public static ICollection<Role> GetLeftRolesOfUser(int userId)
       {
           try
           {
               string query = @"SELECT * FROM ""ROLE"" WHERE ""ID"" not in (Select ""ROLE_ID"" from ""USER_ROLE"" WHERE ""USER_ID"" = " + userId + @") ORDER BY ""NAME"" ";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return GetRoleAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Role", "GetLeftRolesOfUser", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;

           }
       }
       public static ICollection<Role> GetRolesOfUser(int userId)
       {
           try
           {
               string query = @"SELECT ""ROLE"".* FROM ""ROLE"" inner join ""USER_ROLE"" on ""USER_ROLE"".""ROLE_ID"" = ""ROLE"".""ID"" WHERE ""USER_ROLE"".""USER_ID"" = " + userId + @" ORDER BY ""NAME"" ";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return GetRoleAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Role", "GetRolesOfUser", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;

           }
       }
       public static List<Role> GetPagedRoleList(ref int maximumRows, ref int startRowIndex, string where)
       {
           try
           {
               string query = @"SELECT * FROM ""ROLE"" WHERE " + where + @" ORDER BY ""ID"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return GetRoleAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Role", "GetPagedRoleList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

       public static int GetCountRoleList(string where)
       {
           try
           {
               string query = @"SELECT COUNT(*) FROM ""ROLE"" WHERE " + where + ";";
               object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
               return int.Parse(count.ToString());
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Role", "GetCountRoleList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

   
   }
}
