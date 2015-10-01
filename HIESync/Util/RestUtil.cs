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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HIESeeding.Util
{
    /// <summary>
    /// Represents a utility for calling rest functions and interpreting the results
    /// </summary>
    public class RestUtil
    {

        /// <summary>
        /// The base uri
        /// </summary>
        private Uri m_baseUri;


        /// <summary>
        /// Creates a new instance of the rest utility
        /// </summary>
        public RestUtil(Uri baseUri)
        {
            this.m_baseUri = baseUri;
        }

        /// <summary>
        /// Get the specified resource contents
        /// </summary>
        public T Post<T>(String resourcePath, params KeyValuePair<String, Object>[] parms)
        {

            try
            {
                Uri requestUri = new Uri(String.Format("{0}/{1}", this.m_baseUri, resourcePath));
                WebRequest request = WebRequest.Create(requestUri);
                request.Method = "POST";
                request.ContentType = "application/json";
                using(StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.WriteLine("{");
                    foreach (var itm in parms)
                        sw.WriteLine("\t{0} : '{1}', ", itm.Key, itm.Value);
                    sw.WriteLine("\t\"isActive\" : true");
                    sw.WriteLine("}");
                }
                //serializer.WriteObject(request.GetRequestStream(), data);
                var response = request.GetResponse();

                var serializer = new DataContractJsonSerializer(typeof(T));
                T retVal = (T)serializer.ReadObject(response.GetResponseStream());
                return retVal;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        /// <summary>
        /// Get the specified resource contents
        /// </summary>
        public void PostXml(String resourcePath, Object data, String username = null, String password = null)
        {

            try
            {
                Uri requestUri = new Uri(String.Format("{0}/{1}", this.m_baseUri, resourcePath));
                WebRequest request = WebRequest.Create(requestUri);
                request.Method = "POST";
                request.ContentType = "application/xml";
                if (username != null && password != null)
                    request.Credentials = new NetworkCredential(username, password);
                
                XmlSerializer xsz = new XmlSerializer(data.GetType());
                xsz.Serialize(request.GetRequestStream(), data);

                //serializer.WriteObject(request.GetRequestStream(), data);
                var response = request.GetResponse();

            }
            catch (Exception e)
            {
                throw;
            }

        }
        /// <summary>
        /// Get the specified resource contents
        /// </summary>
        public T Get<T>(String resourcePath, params KeyValuePair<String,Object>[] queryParms)
        {

            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                if(queryParms != null)
                    foreach(var qp in queryParms)
                        queryBuilder.AppendFormat("{0}={1}&", qp.Key, qp.Value);
                if(queryBuilder.Length > 0)
                    queryBuilder.Remove(queryBuilder.Length - 1, 1);
                Uri requestUri = new Uri(String.Format("{0}/{1}?{2}", this.m_baseUri, resourcePath, queryBuilder));

                WebRequest request = WebRequest.Create(requestUri);
                request.Timeout = 600000;
                request.Method = "GET";
                var response = request.GetResponse();
                
                var serializer = new DataContractJsonSerializer(typeof(T));
                T retVal = (T)serializer.ReadObject(response.GetResponseStream());
                return retVal;
            }
            catch(Exception e)
            {
                throw;
            }

        }
    }
}
