/*
 *	TIIS HIE Synchronization Program, Copyright (C) 2015 ecGroup
 *  Development services by Fyfe Software Inc.
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
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
