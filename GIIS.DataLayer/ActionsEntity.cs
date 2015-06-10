using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;

namespace GIIS.DataLayer
{
    public partial class Actions
    {
        public static ICollection<Actions> GetLeftActionsOfRole(int roleId)
        {
            try
            {
                string query = @"SELECT * FROM ""ACTIONS"" WHERE ""ID"" not in (Select ""ACTION_ID"" from ""ROLE_ACTION"" WHERE ""ROLE_ID"" = " + roleId + @") ORDER BY ""NAME"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetActionsAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Actions", "GetLeftActionsOfRole", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;

            }
        }
        public static ICollection<Actions> GetActionsOfRole(int roleId)
        {
            try
            {
                string query = @"SELECT ""ACTIONS"".* FROM ""ACTIONS"" inner join ""ROLE_ACTION"" on ""ROLE_ACTION"".""ACTION_ID"" = ""ACTIONS"".""ID"" WHERE ""ROLE_ACTION"".""ROLE_ID"" = " + roleId + @" ORDER BY ""NAME"" ";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetActionsAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Actions", "GetActionsOfRole", 1, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;

            }
        }
        public static List<Actions> GetActionsListByUserId(int UserId)
        {
            try
            {
                string query = @"SELECT A.* FROM ""ACTIONS"" A INNER JOIN ""ROLE_ACTION"" RA ON A.""ID"" = RA.""ACTION_ID"" INNER JOIN ""USER_ROLE"" UR ON UR.""ROLE_ID"" = RA.""ROLE_ID"" WHERE UR.""USER_ID"" = @UserId;";

                List<Npgsql.NpgsqlParameter> parameters = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@UserId", DbType.Int32) { Value = UserId }
                };

                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, parameters);
                return GetActionsAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Actions", "GetActionsListByUserId", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
