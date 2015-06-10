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
