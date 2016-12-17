﻿/*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIESync.Control;
using HIESync.Synchronization;
using MohawkCollege.Util.Console.Parameters;
using System.IO;
using System.Reflection;

namespace HIESync
{
    class Program
    {
        /// <summary>
        /// Entry point into the 
        /// </summary>
        static void Main(string[] args)
        {


            // Add a logging capability
            String logDir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "logs");
            if(!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            if (File.Exists(Path.Combine(logDir, "lastrun.log")))
                File.Delete(Path.Combine(logDir, "lastrun.log"));

            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(logDir, "lastrun.log")) { TraceOutputOptions = TraceOptions.DateTime, Filter = new EventTypeFilter(SourceLevels.Error | SourceLevels.Warning | SourceLevels.Critical)});
            Trace.Listeners.Add(new TextWriterTraceListener(Path.ChangeExtension(Path.Combine(logDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")), "log")) { TraceOutputOptions = TraceOptions.DateTime, Filter = new EventTypeFilter(SourceLevels.Information | SourceLevels.Error | SourceLevels.Warning | SourceLevels.Verbose | SourceLevels.Critical ) });
            Trace.Listeners.Add(new ConsoleTraceListener(false));
            SynchronizationContext context = SynchronizationContext.CreateContext();

            try
            {
                Trace.TraceInformation("-- Synchronization started on {0} --", DateTime.Now);

                // Dump some info about the sync
                foreach (var arg in args)
                    Trace.TraceInformation("Control Parameter: {0}", arg);

                // Parse parameters
                var parameters = new ParameterParser<ConsoleParameters>().Parse(args);

                if (parameters.RunDrp)
                    new DrpSynchronizer().Run();

                using (PatientSynchronizer patientSync = new PatientSynchronizer(context))
                {
                    // Start the synchronization 
                    if (parameters.PullPatients)
                        patientSync.PullClients();
                    if (parameters.PushPatients)
                        patientSync.PushClients();
                }

                using (ClinicalDataSynchronizer clinicalSync = new ClinicalDataSynchronizer(context))
                {
                    if (parameters.PushClinical)
                        clinicalSync.PushClinical();
                }

                using (StockSynchronization stockSync = new StockSynchronization(context))
                    if (parameters.PushStock)
                        stockSync.SynchronizeStockCounts();

                if (parameters.PushMetrics)
                {
                    if (parameters.MetricMonth != null && parameters.MetricYear != null)
                        new SecondaryUseSynchronization(context).Synchronize(Int32.Parse(parameters.MetricYear), Int32.Parse(parameters.MetricMonth));
                    else if(parameters.MetricMonth != null)
                        new SecondaryUseSynchronization(context).Synchronize(DateTime.Now.Year, Int32.Parse(parameters.MetricMonth));
                    else
                        new SecondaryUseSynchronization(context).Synchronize(null, null);
                }


                context.Complete();

                Trace.TraceInformation("-- Synchronization ended on {0} --", DateTime.Now);
            }
            catch(Exception e)
            {
                Trace.TraceError(e.ToString());
                context.Error();
            }
        }
    }
}
