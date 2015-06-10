using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GIIS.DataLayer
{
    partial class Menu
    {
        public static ICollection<Menu> GetTrueChildren(int parentId)
        {
            try
            {
                string query = string.Format("SELECT distinct \"MENU\".\"ID\", \"PARENT_ID\", \"TITLE\", \"NAVIGATE_URL\", \"IS_ACTIVE\", \"DISPLAY_ORDER\" FROM \"MENU\" WHERE \"PARENT_ID\" = {0} AND \"IS_ACTIVE\" = 'True' ORDER BY \"DISPLAY_ORDER\"; ", parentId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetMenuAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Menu", "GetChildren", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ICollection<Menu> GetTrueParents()
        {
            try
            {
                string query = string.Format("SELECT DISTINCT \"MENU\".\"ID\", \"PARENT_ID\", \"TITLE\", \"NAVIGATE_URL\", \"IS_ACTIVE\", \"DISPLAY_ORDER\" FROM \"MENU\" WHERE \"PARENT_ID\" = 0 AND \"IS_ACTIVE\" = 'True' ORDER BY \"DISPLAY_ORDER\"");
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetMenuAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Menu", "GetParents", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ICollection<Menu> GetParents()
        {
            try
            {
                string query = string.Format("SELECT DISTINCT \"MENU\".\"ID\", \"PARENT_ID\", \"TITLE\", \"NAVIGATE_URL\", \"IS_ACTIVE\", \"DISPLAY_ORDER\" FROM \"MENU\" WHERE \"PARENT_ID\" = 0 AND \"IS_ACTIVE\" = 'True' ORDER BY \"DISPLAY_ORDER\"");
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetMenuAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Menu", "GetParents", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ICollection<Menu> GetChildren(int parentId)
        {
            try
            {
                string query = string.Format("SELECT distinct \"MENU\".\"ID\", \"PARENT_ID\", \"TITLE\", \"NAVIGATE_URL\", \"IS_ACTIVE\", \"DISPLAY_ORDER\" FROM \"MENU\" WHERE \"PARENT_ID\" = {0} AND \"IS_ACTIVE\" = 'True' ORDER BY \"DISPLAY_ORDER\"; ", parentId);
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetMenuAsList(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("Menu", "GetChildren", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
