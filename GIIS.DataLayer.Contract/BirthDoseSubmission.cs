using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.DataLayer.Contract
{
    public class BirthDoseSubmission
    {
        public int FacilityId { get; set; }
        public int Month { get; set; }
        public List<BirthDoseData> Data { get; set; }
    }

    public class BirthDoseData
    {
        public String Gender { get; set; }
        public List<String> Doses { get; set; }
    }
}
