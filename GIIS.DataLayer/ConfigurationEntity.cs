using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
  partial class Configuration
    {
      public static int UpdateValues(Configuration o)
      {
          try
          {
              string query = @"UPDATE ""CONFIGURATION"" SET ""VALUE"" = '" + o.Value + @"' WHERE ""NAME"" = '" + o.Name + "'";
              int rowAffected = DBManager.ExecuteNonQueryCommand(query, CommandType.Text, null);
              Log.InsertEntity(2, "Success", o.Id.ToString(), "Configuration", "Update");
              return rowAffected;
          }
          catch (Exception ex)
          {
              Log.InsertEntity("Configuration", "Update", 2, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
          }
          return -1;
      }
    }
}
