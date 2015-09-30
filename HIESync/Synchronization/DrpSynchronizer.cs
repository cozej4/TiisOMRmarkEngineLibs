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
