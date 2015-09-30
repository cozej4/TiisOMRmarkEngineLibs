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
