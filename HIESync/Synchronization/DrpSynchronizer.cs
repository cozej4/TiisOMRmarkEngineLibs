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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HIESync.Synchronization
{
    /// <summary>
    /// DRP Engine sycnrhonizer
    /// </summary>
    public class DrpSynchronizer
    {

        public void Run()
        {
            String sqlScript = new StreamReader(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data", "SQL", "DRP.SQL")).ReadToEnd();
            while(sqlScript.Contains(";"))
            {
                try
                {
                    string cmdlet = sqlScript.Substring(0, sqlScript.IndexOf(";") + 1);
                    sqlScript = sqlScript.Substring(sqlScript.IndexOf(";") + 1);
                    Trace.TraceInformation("EXEC {0}", cmdlet);
                    GIIS.DataLayer.DBManager.ExecuteNonQueryCommand(cmdlet, System.Data.CommandType.Text, null);
                    Trace.TraceInformation("DONE");
                }
                catch(Exception e)
                {
                    Trace.TraceError(e.Message);
                }

            }
        }

    }
}
