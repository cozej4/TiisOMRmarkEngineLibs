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
