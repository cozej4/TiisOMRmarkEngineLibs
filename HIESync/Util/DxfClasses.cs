using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HIESync.Util
{
    /// <summary>
    /// Dxf message
    /// </summary>
    [XmlType("DxfMessage", Namespace = "http://dhis2.org/schema/dxf/2.0")]
    [XmlRoot("dxf", Namespace = "http://dhis2.org/schema/dxf/2.0")]
    public class DxfMessage
    {

        /// <summary>
        /// DXF message
        /// </summary>
        public DxfMessage()
        {
            this.ValueSets = new List<DxfValueSet>();
        }

        /// <summary>
        /// Value sets
        /// </summary>
        [XmlElement("dataValueSet", Namespace = "http://dhis2.org/schema/dxf/2.0")]
        public List<DxfValueSet> ValueSets { get; set; }
    }

    /// <summary>
    /// Value set
    /// </summary>
    [XmlType("DxfValueSet", Namespace = "http://dhis2.org/schema/dxf/2.0")]
    public class DxfValueSet
    {

        /// <summary>
        /// Value set ctor
        /// </summary>
        public DxfValueSet()
        {
            this.Value = new List<DxfValue>();
        }

        /// <summary>
        /// Values
        /// </summary>
        [XmlElement("dataValue", Namespace = "http://dhis2.org/schema/dxf/2.0")]
        public List<DxfValue> Value { get; set; }

    }

    /// <summary>
    /// Value
    /// </summary>
    [XmlType("DxfValue", Namespace = "http://dhis2.org/schema/dxf/2.0")]
    public class DxfValue
    {
        [XmlAttribute("period")]
        public String Period { get; set; }
        [XmlAttribute("orgUnit")]
        public String OrgUnit { get; set; }
        [XmlAttribute("dataElement")]        
        public String DataElement { get; set; }
        [XmlAttribute("value")]        
        public Decimal Value { get; set; }
        [XmlIgnore]
        public int Numerator { get; set; }
        [XmlIgnore]
        public int Denominator { get; set; }
    }
}
