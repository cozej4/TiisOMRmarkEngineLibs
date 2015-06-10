using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
    public partial class ChildSupplements
    {
        public static ChildSupplements GetChildSupplementsByChild(Int32 childId)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_SUPPLEMENTS"" WHERE ""CHILD_ID"" = " + childId + "";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildSupplementsAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildSupplements", "GetChildSupplementsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }

        public static ChildSupplements GetChildSupplementsByChild(Int32 childId, DateTime date)
        {
            try
            {
                string query = @"SELECT * FROM ""CHILD_SUPPLEMENTS"" WHERE ""CHILD_ID"" = " + childId + @" AND ""DATE"" = '" + date.ToString("yyyy-MM-dd") + "'";
                DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
                return GetChildSupplementsAsObject(dt);
            }
            catch (Exception ex)
            {
                Log.InsertEntity("ChildSupplements", "GetChildSupplementsByChild", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
                throw ex;
            }
        }
    }
}
