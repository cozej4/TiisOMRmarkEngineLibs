using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.BusinessLogic
{
    /// <summary>
    /// Status of orders and lines
    /// </summary>
    public enum OrderStatusType : short
    {
        Requested = 0,
        Released = 1,
        Packed = 2,
        Shipped = 3,
        Cancelled = -1
    }
}
