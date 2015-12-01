using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.DataLayer.Contract
{
    public class WeighTallySubmission
    {

        /// <summary>
        /// The month for the weigh tally submission
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// Facility identifier
        /// </summary>
        public int FacilityId { get; set; }
        /// <summary>
        /// Data elements
        /// </summary>
        public List<WeighTallyData> Data { get; set; }
    }

    /// <summary>
    /// Weigh tally data
    /// </summary>
    public class WeighTallyData
    {
        /// <summary>
        /// Age group as a string 
        /// </summary>
        public String AgeGroup { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        public String Gender { get; set; }
        /// <summary>
        /// Weight group
        /// </summary>
        public String WeightGroup { get; set; }
        /// <summary>
        /// Exclusive breastfeeding
        /// </summary>
        public bool Ebf { get; set; }
        public bool Rf { get; set; }
    }
}
