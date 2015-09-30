using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIESync.Synchronization
{
    /// <summary>
    /// Equality comparotor
    /// </summary>
    public class VaccinationAppointmentComparator : IEqualityComparer<VaccinationAppointment>
    {
        public bool Equals(VaccinationAppointment x, VaccinationAppointment y)
        {
            return x == y || (x == null) ^ (y == null) || x.Id == y.Id; 
        }

        public int GetHashCode(VaccinationAppointment obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
