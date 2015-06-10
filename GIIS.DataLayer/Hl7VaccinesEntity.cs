using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GIIS.DataLayer
{
   partial  class Hl7Vaccines
    {
       public static DataTable GetHl7VaccinesForList()
       {
           try
           {
               string query = @"SELECT -1 as ""ID"", '-----' as ""CODE"" UNION SELECT ""ID"", ""CODE"" FROM ""HL7_VACCINES"" WHERE ""VACCINE_STATUS"" = 'true' ORDER BY ""CODE"" ";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return dt;
           }
           catch (Exception ex)
           {
               Log.InsertEntity("Hl7Vaccines", "GetHl7VaccinesforList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
    }
}
