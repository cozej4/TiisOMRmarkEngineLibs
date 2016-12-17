//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace GIIS.DataLayer
{
    public partial class Log
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logCategoryId"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="className"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static int InsertEntity(int logCategoryId, string title, string message, string className, string method)
        {
            //-1 = instanceId
            //return InsertEntity("", -1, logCategoryId, "", message, method, className, title, 1);
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logCategoryId"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="className"></param>
        /// <param name="method"></param>
        /// <param name="userId"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public static int InsertEntity(int logCategoryId, string title, string message, string className, string method, int userId, object instanceId)
        {
            int iId = int.Parse(instanceId.ToString());

            //return InsertEntity("", iId, logCategoryId, "", message, method, className, title, userId);
            return -1;
        }

        /// <summary>
        /// Insert Log into the database, after parsing the parameters on an Log object
        /// </summary>
        /// <param name="className">Class Name</param>
        /// <param name="instanceId">Instance Id</param>
        /// <param name="logCategoryId">Category Id</param>
        /// <param name="machine">Machine Name</param>
        /// <param name="message">Message</param>
        /// <param name="method">Method</param>
        /// <param name="title">Title</param>
        /// <param name="userId">User Id</param>
        /// <returns>The Id of the inserted record</returns>
        public static int InsertEntity(string className, string method, int logCategoryId, string message, string title)
        {
            try
            {

                Log o = new Log();
                o.Class = className;
                o.Created = DateTime.Now;
                o.LogCategoryId = logCategoryId;
                o.Message = message;
                o.Method = method;
                o.Title = title;

                Trace.TraceInformation("LOG: {0} : {1}", title, message);

                int inserted = Insert(o);
                return inserted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}