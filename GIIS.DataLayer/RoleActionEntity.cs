using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    public partial class RoleAction
    {
        public static int DeleteByActionAndRole(int roleId, int actionId)
        {
            try
            {
                string query = @"DELETE FROM ""ROLE_ACTION"" WHERE ""ROLE_ID"" = " + roleId + @" AND ""ACTION_ID"" = " + actionId;
                int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
                return rowAffected;
            }
            catch (Exception ex)
            {
                Log.InsertEntity("RoleAction", "DeleteByActionAndRole", 3, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
        public static int Exists(int roleId, int actionId)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM ""ROLE_ACTION"" WHERE ""ROLE_ID"" = " + roleId + @" AND ""ACTION_ID"" = " + actionId;
                object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
                return int.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Log.InsertEntity("RoleAction", "GetCountRoleActionList", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
