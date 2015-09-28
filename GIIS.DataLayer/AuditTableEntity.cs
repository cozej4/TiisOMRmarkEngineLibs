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
using System.Threading.Tasks;

namespace GIIS.DataLayer
{
    public partial class AuditTable
    {
        public static int InsertEntity(string dbTable, string recordId, int activityId, DateTime date, int userId)
        {
            try
            {
                AuditTable o = new AuditTable();
                o.DbTable = dbTable;
                o.RecordIdOnTable = recordId;
                o.UserId = userId;
                o.Date = date;
                o.ActivityId = activityId;
                    
                int inserted = Insert(o);
                return inserted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
