using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MohawkCollege.Util.Console.Parameters;

namespace HIESync.Control
{
    /// <summary>
    /// Parameter class used by the console parameter parser. 
    /// </summary>
    /// <remarks>
    /// Controls the behavior of the application
    /// </remarks>
    public class ConsoleParameters
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public ConsoleParameters()
        {
            
        }

        /// <summary>
        /// Pull patients only
        /// </summary>
        [Description("Indicates the sync tool should only pull patients from the HIE")]
        [Parameter("pullClients")]
        public bool PullPatients { get; set; }

        /// <summary>
        /// Push patients only
        /// </summary>
        [Description("Indicates the sync tool should only push patients to the HIE")]
        [Parameter("pushClients")]
        public bool PushPatients { get; set; }

        /// <summary>
        /// Push clinical data only
        /// </summary>
        [Description("Indicates the sync tool should only push clinical data to the HIE")]
        [Parameter("pushClinical")]
        public bool PushClinical { get; set; }

        [Description("Push stock balances")]
        [Parameter("pushStock")]
        public bool PushStock { get; set; }

        [Description("Push metrics to IL")]
        [Parameter("pushMetrics")]
        public bool PushMetrics { get; set; }

        [Description("Only push current month metrics")]
        [Parameter("month")]
        public string MetricMonth { get; set; }

        [Description("Only push current year metrics")]
        [Parameter("year")]
        public string MetricYear { get; set; }

        [Description("Run the DRP")]
        [Parameter("runDrp")]
        public bool RunDrp { get; set; }
    }
}
