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
