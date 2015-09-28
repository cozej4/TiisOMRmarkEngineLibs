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
   partial class ItemCategory
    {
      
       public static DataTable GetItemCategoryForListAll()
       {
           try
           {
               string query = @"SELECT -1 as ""ID"", '-----' as ""NAME"" UNION SELECT ""ID"", ""NAME"" FROM ""ITEM_CATEGORY"" WHERE ""IS_ACTIVE"" = 'true' ORDER BY ""ID"" ";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return dt;
           }
           catch (Exception ex)
           {
               Log.InsertEntity("ItemCategory", "GetItemCategoryForListAll", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
       public static List<ItemCategory> GetPagedItemCategoryList(ref int maximumRows, ref int startRowIndex, string where)
       {
           try
           {
               string query = @"SELECT * FROM ""ITEM_CATEGORY"" WHERE " + where + @" ORDER BY ""NAME"" OFFSET " + startRowIndex + " LIMIT " + maximumRows + ";";
               DataTable dt = DBManager.ExecuteReaderCommand(query, CommandType.Text, null);
               return GetItemCategoryAsList(dt);
           }
           catch (Exception ex)
           {
               Log.InsertEntity("ItemCategory", "GetPagedItemCategoryList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }

       public static int GetCountItemCategoryList(string where)
       {
           try
           {
               string query = @"SELECT COUNT(*) FROM ""ITEM_CATEGORY"" WHERE " + where + ";";
               object count = DBManager.ExecuteScalarCommand(query, CommandType.Text, null);
               return int.Parse(count.ToString());
           }
           catch (Exception ex)
           {
               Log.InsertEntity("ItemCategory", "GetCountItemCategoryList", 4, ex.StackTrace.Replace("'", ""), ex.Message.Replace("'", ""));
               throw ex;
           }
       }
    }
}
