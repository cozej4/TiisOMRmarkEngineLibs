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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIESync.Data;

namespace HIESync.Synchronization
{
    /// <summary>
    /// Synchronization context
    /// </summary>
    public class SynchronizationContext
    {

        /// <summary>
        /// Private ctor
        /// </summary>
        private SynchronizationContext()
        {
        }


        /// <summary>
        /// Create a context object from the specified data reader
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static SynchronizationContext CreateContext(decimal jobId, DateTime startDate, DateTime endDate, bool success)
        {
            return new SynchronizationContext()
            {
                JobId = jobId,
                StartTime = startDate,
                StopTime = endDate,
                Success = success
            };
        }

        /// <summary>
        /// Get the last sync context
        /// </summary>
        public static SynchronizationContext GetLastSync()
        {
            try
            {
                Trace.TraceInformation("Get last Synchrnoization Job Data");
                // Register
                using (var dao = new SyncData())
                {
                    return dao.GetLastSync();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                return null;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Create a synchronization context
        /// </summary>
        public static SynchronizationContext CreateContext()
        {

            try
            {
                Trace.TraceInformation("Create Synchrnoization Job Data");
                // Register
                using(var dao = new SyncData())
                {
                    // Sync context
                    var retVal = new SynchronizationContext()
                    {
                        JobId = dao.CreateSynchronizationJob(Process.GetCurrentProcess().Id),
                        StartTime = DateTime.Now
                    };

                    Trace.TraceInformation("DB: Registered sync job id #{0}", retVal.JobId);
                    dao.Commit();
                    return retVal;
                }
            }
            catch(Exception e)
            {
                Trace.TraceError(e.ToString());
                return null;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Gets the job identifier
        /// </summary>
        public decimal JobId { get; private set; }

        /// <summary>
        /// Gets the start time
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Gets the stop time
        /// </summary>
        public DateTime StopTime { get; private set; }

        /// <summary>
        /// Gets a success indicator
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Flag the sync as complete
        /// </summary>
        public void Complete()
        {
            try
            {
                Trace.TraceInformation("Marking Synchrnoization Job Complete");
                
                // Register
                using (var dao = new SyncData())
                {
                    this.Success = true;
                    this.StopTime = DateTime.Now;
                    dao.CompleteSynchronizationJob(this.JobId, true);
                    dao.Commit();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// Mark sync as error
        /// </summary>
        public void Error()
        {
            try
            {
                Trace.TraceInformation("Marking Synchrnoization Job Error");

                // Register
                using (var dao = new SyncData())
                {
                    this.Success = false;
                    this.StopTime = DateTime.Now;
                    dao.CompleteSynchronizationJob(this.JobId, false);
                    dao.Commit();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
            finally
            {
            }
        }
    }
}
