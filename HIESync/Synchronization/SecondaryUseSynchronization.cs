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
using HIESeeding.Util;
using HIESync.Data;
using HIESync.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HIESync.Synchronization
{

    /// <summary>
    /// Secondary use synchro
    /// </summary>
    public class SecondaryUseSynchronization
    {
        /// <summary>
        /// Indicators
        /// </summary>
        private XmlDocument m_indicators;

        // Context
        private SynchronizationContext m_context;

        /// <summary>
        /// Secondary use synchrnonization
        /// </summary>
        public SecondaryUseSynchronization(SynchronizationContext context)
        {
            this.m_context = context;
            this.m_indicators = new XmlDocument();
            this.m_indicators.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "indicators.xml"));
        }

        /// <summary>
        /// Synchronize the indicators
        /// </summary>
        public void Synchronize(int? year, int? month)
        {
            this.SynchronizeImmunizations(year, month);
            this.SynchronizeWeights(year, month);
        }

        /// <summary>
        /// Synchronize weights
        /// </summary>
        private void SynchronizeWeights(int? year, int? month)
        {
            String query = "SELECT * FROM CHILD_WEIGHT_SUMMARY";
            if (year.HasValue && month.HasValue)
                query += " WHERE \"YEAR\" = @year AND \"MONTH\" = @month";
            query += " ORDER BY \"YEAR\", \"MONTH\"";

            DxfMessage message = new DxfMessage();
            DxfValueSet valueSet = new DxfValueSet();
            message.ValueSets.Add(valueSet);

            // Load indicator map
            List<KeyValuePair<String, KeyValuePair<Int32, String>>> indicatorMap = new List<KeyValuePair<string, KeyValuePair<int, string>>>();

            XmlDocument map = new XmlDocument();
            map.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "WeightIndicatorMap.xml"));

            foreach (XmlNode nd in map.SelectNodes("//*[local-name() = 'indicator']"))
                indicatorMap.Add(new KeyValuePair<string, KeyValuePair<int, string>>(nd.Attributes["id"].Value, new KeyValuePair<int, string>(Int32.Parse(nd.Attributes["ageGroup"].Value), nd.Attributes["column"].Value)));
            
            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["GiisConnectionString"].ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@year", year.HasValue ? year.Value : 0);
                    cmd.Parameters.Add("@month", month.HasValue ? month.Value : 0);

                    cmd.CommandType = System.Data.CommandType.Text;
                    using (var dataReader = cmd.ExecuteReader())
                        while (dataReader.Read())
                        {

                            // < 60%
                            var imap = indicatorMap.FindAll(o=>o.Value.Key == Convert.ToInt32(dataReader["age_id"]));

                            foreach(var itm in imap)
                            {
                                valueSet.Value.Add(new DxfValue() {
                                    DataElement = itm.Key,
                                    Numerator = Convert.ToInt32(dataReader[itm.Value.Value]),
                                    Denominator = 1,
                                    OrgUnit = Convert.ToString(dataReader["HEALTH_FACILITY_CODE"]).Replace("urn:uuid:", ""),
                                    Period = string.Format("{0}M{1}", dataReader["YEAR"], dataReader["MONTH"]),
                                    Value= Convert.ToInt32(dataReader[itm.Value.Value])
                                });

                            }

                        }
                }
            }
            this.SendDxfMessage(message);

        }

        public void SynchronizeImmunizations(int? year, int? month)
        {
            String query = "SELECT * FROM COVERAGE_REPORT_INDICATOR WHERE ADMIN_COVERAGE IS NOT NULL ";
            if (year.HasValue && month.HasValue)
                query += " AND \"VACC_YEAR\" = @year AND \"VACC_MONTH\" = @month OR \"CVX_CODE\" = '102' AND (\"VACC_YEAR\" * 12 + \"VACC_MONTH\") >= (@year * 12 + @month) - 3";
            query += " ORDER BY \"VACC_YEAR\", \"VACC_MONTH\"";

            DxfMessage message = new DxfMessage();
            DxfValueSet valueSet = new DxfValueSet();
            message.ValueSets.Add(valueSet);
            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["GiisConnectionString"].ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@year", year.HasValue ? year.Value : 0);
                    cmd.Parameters.Add("@month", month.HasValue ? month.Value : 0);

                    cmd.CommandType = System.Data.CommandType.Text;
                    using (var dataReader = cmd.ExecuteReader())
                        while (dataReader.Read())
                        {


                            // For each of the items that we match in our document
                            foreach (XmlElement nd in this.m_indicators.SelectNodes(String.Format("//*[local-name() = 'indicator' and ./fullname/text() = '{0}']", dataReader["FULLNAME"])))
                            {
                                var thisPeriod = String.Format("{0}M{1}", dataReader["VACC_YEAR"], dataReader["VACC_MONTH"]);
                                DxfValue value = valueSet.Value.Find(o => o.OrgUnit == dataReader["HF_CODE"].ToString().Replace("urn:uuid:", "") && o.DataElement == nd.Attributes["id"].Value && o.Period == thisPeriod);

                                // Was an existing value found?
                                if (value == null)
                                {
                                    value = new DxfValue()
                                    {
                                        Period = String.Format("{0}M{1}", dataReader["VACC_YEAR"], dataReader["VACC_MONTH"]),
                                        OrgUnit = dataReader["HF_CODE"].ToString().Replace("urn:uuid:", ""),
                                        Value = Convert.ToDecimal(dataReader["ADMIN_COVERAGE"]),
                                        DataElement = nd.Attributes["id"].Value
                                    };
                                }

                                // Age and droupout specs?
                                var age = nd.SelectSingleNode("./*[local-name() = 'ageGroup']");
                                var dropout = nd.SelectSingleNode("./*[local-name() = 'dropout']");
                                if (age != null && age.InnerText == dataReader["AGE_GROUP"].ToString() ||
                                    age == null)
                                {
                                    value.Numerator += Convert.ToInt32(dataReader["given"]);
                                    value.Denominator += Convert.ToInt32(dataReader["target"]);

                                    // Is this a dropout?
                                    if (dropout != null)
                                    {
                                        int prevMonth = Convert.ToInt32(dataReader["VACC_MONTH"]) + Int32.Parse(dropout.SelectSingleNode("./*[local-name() = 'month']").InnerText);
                                        int prevYear = Convert.ToInt32(dataReader["VACC_YEAR"]) - (prevMonth <= 0 ? 1 : 0);
                                        if (prevMonth <= 0)
                                            prevMonth = 12 - prevMonth;
                                        var dropoutPrevObs = valueSet.Value.Find(o => o.Period == String.Format("{0}M{1}", prevYear, prevMonth) && o.DataElement == dropout.SelectSingleNode("./*[local-name() = 'indicator']/@id").Value && o.OrgUnit == value.OrgUnit);
                                        if (dropoutPrevObs != null)
                                        {
                                            value.Numerator = dropoutPrevObs.Numerator - value.Numerator;
                                            value.Denominator = dropoutPrevObs.Numerator;
                                        }
                                        else
                                            continue;
                                    }

                                    value.Value = (decimal)(((float)value.Numerator / value.Denominator) * 100);

                                }
                                else
                                    continue;

                                valueSet.Value.Add(value);

                            }

                        }
                }
            }

            // Remove anything not in our reporting year
            if (month.HasValue && year.HasValue)
                valueSet.Value.RemoveAll(o => o.Period != String.Format("{0}M{1}", year, month));

            this.SendDxfMessage(message);
        }

        private void SendDxfMessage(DxfMessage message)
        {
            try
            {
                XmlSerializer xsz = new XmlSerializer(typeof(DxfMessage));
                using (XmlWriter xw = XmlWriter.Create(String.Format("datavalues-{0}.dxf", DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss")), new XmlWriterSettings() { Indent = true }))
                    xsz.Serialize(xw, message);

                RestUtil util = new RestUtil(new Uri(ConfigurationManager.AppSettings["DHIS_URL"]));
                util.PostXml("dataValueSets", message, ConfigurationManager.AppSettings["DHIS_UN"], ConfigurationManager.AppSettings["DHIS_PWD"]);
            }
            catch(Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }


    }
}
