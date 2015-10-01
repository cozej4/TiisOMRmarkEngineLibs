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
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIESync.Synchronization;

namespace HIESync.Data
{
    /// <summary>
    /// Sync data access object
    /// </summary>
    public class SyncData : IDisposable
    {
        // The connection to the database
        private IDbConnection m_connection;
        // Current transaction
        private IDbTransaction m_transaction;
        // Lock object
        private Object m_lockObject = new object();

        /// <summary>
        /// Create a connection to the database
        /// </summary>
        private IDbConnection GetOrCreateConnection()
        {
            if(this.m_connection == null)
                lock(this.m_lockObject)
                    if (this.m_connection == null)
                    {
                        var connectionString = ConfigurationManager.ConnectionStrings["GiisConnectionString"];
                        var dbProvider = DbProviderFactories.GetFactory(connectionString.ProviderName);
                        if (dbProvider == null)
                            throw new ConfigurationErrorsException("Could not find DbProvider");

                        // Create the connection
                        this.m_connection = dbProvider.CreateConnection();
                        this.m_connection.ConnectionString = connectionString.ConnectionString;
                        this.m_connection.Open();
                    }
            return this.m_connection;
        }

        /// <summary>
        /// Get or create the transaction
        /// </summary>
        private IDbTransaction GetOrCreateTransaction()
        {
            if (this.m_transaction == null)
                lock (this.m_lockObject)
                    if (this.m_transaction == null)
                        this.m_transaction = this.GetOrCreateConnection().BeginTransaction();
            return this.m_transaction;
        }

        /// <summary>
        /// Create a command object
        /// </summary>
        /// <param name="conn">The connection</param>
        /// <param name="tx">The current transaction</param>
        /// <param name="spName">The stored procedure name</param>
        private IDbCommand CreateCommandStoredProc(String spName)
        {
            var retVal = this.GetOrCreateConnection().CreateCommand();
            retVal.CommandType = CommandType.StoredProcedure;
            retVal.CommandText = spName;
            retVal.Connection = this.GetOrCreateConnection();
            retVal.Transaction = this.m_transaction; // tx may not be needed
            return retVal;
        }

        /// <summary>
        /// Create an inbound parameter
        /// </summary>
        /// <param name="cmd">The command to which the parameter should be created </param>
        /// <param name="parmName">The name of the parameter</param>
        /// <param name="parmType">The type of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        private IDbDataParameter CreateParameterIn(IDbCommand cmd, String parmName, DbType parmType, Object value)
        {
            var retVal = cmd.CreateParameter();
            retVal.ParameterName = parmName;
            retVal.Direction = ParameterDirection.Input;
            retVal.DbType = parmType;
            retVal.Value = value;
            return retVal;
        }

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        public void Commit()
        {
            if (this.m_transaction != null)
            {
                this.m_transaction.Commit();
                this.m_transaction.Dispose();
                this.m_transaction = null;
            }
        }

        /// <summary>
        /// Rollback 
        /// </summary>
        public void Rollback()
        {
            if (this.m_transaction != null)
            {
                this.m_transaction.Rollback();
                this.m_transaction.Dispose();
                this.m_transaction = null;
            }
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            this.Rollback(); // Rollback any unsaved changes
            if (this.m_connection != null)
            {
                this.m_connection.Close();
                this.m_connection.Dispose();
            }
        }

        /// <summary>
        /// Create synchronization data
        /// </summary>
        public decimal CreateSynchronizationJob(int pid)
        {

            try
            {
                // We need a transaction, create one if not already created
                this.GetOrCreateTransaction();
                using (var cmd = this.CreateCommandStoredProc("hie_create_sync"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "pid_in", DbType.Int32, pid));
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
            catch(Exception e)
            {
                Trace.TraceError("Error registering job in database: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Get the last sync job completed
        /// </summary>
        public SynchronizationContext GetLastSync()
        {
            try
            {
                // Get the last sync
                using(var cmd = this.CreateCommandStoredProc("hie_get_last_sync"))
                    using(var rdr = cmd.ExecuteReader())
                        if(rdr.Read())
                            return SynchronizationContext.CreateContext(
                                Convert.ToDecimal(rdr["job_id"]), Convert.ToDateTime(rdr["start_timestamp"]), Convert.ToDateTime(rdr["stop_timestamp"]), Convert.ToChar(rdr["outcome"]) == 'S'
                                );
                        else
                            return null;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting last sync job from database: {0}", e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Register that we've updated a patient record 
        /// </summary>
        public decimal RegisterPatientSync(int patientId, decimal jobId, AtnaApi.Model.ActionType action, String ecid)
        {
            try
            {
                // Convert action
                String actionString = String.Empty;
                switch(action)
                {
                    case AtnaApi.Model.ActionType.Create:
                        actionString = "C";
                        break;
                    case AtnaApi.Model.ActionType.Delete:
                        actionString = "D";
                        break;
                    case AtnaApi.Model.ActionType.Read:
                        actionString = "R";
                        break;
                    case AtnaApi.Model.ActionType.Update:
                        actionString = "U";
                        break;
                }
                // Create tx
                this.GetOrCreateTransaction();

                // Run command
                using(var cmd = this.CreateCommandStoredProc("hie_create_patient_sync"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "giis_patient_id_in", DbType.Int32, patientId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "job_id_in", DbType.Decimal, jobId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "ecid_in", DbType.String, ecid));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "action_in", DbType.StringFixedLength, actionString));

                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
            catch(Exception e)
            {
                Trace.TraceError("Error registering sync of patient data: {0}", e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Complete a sync job
        /// </summary>
        public void CompleteSynchronizationJob(decimal jobId, bool success)
        {
            try
            {
                // We need a transaction, create one if not already created
                this.GetOrCreateTransaction();
                using (var cmd = this.CreateCommandStoredProc("hie_complete_sync"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "job_id_in", DbType.Decimal, jobId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "outcome_in", DbType.StringFixedLength, success ? 'S' : 'E'));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error completing job in database: {0}", e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get the patient ECID
        /// </summary>
        public string GetPatientEcid(int giisPatientId)
        {
            try
            {
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_get_patient_ecid"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "giis_patient_id_in", DbType.Int32, giisPatientId));
                    object retVal = cmd.ExecuteScalar();
                    if (retVal == DBNull.Value)
                        return null;
                    else
                        return Convert.ToString(retVal);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting ECID from database: {0}", e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get the list of unsynced child id
        /// </summary>
        public List<int> GetUnsyncedChildrenId()
        {
            try
            {
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_get_unsynced_child"))
                    using(var rdr = cmd.ExecuteReader())
                        while(rdr.Read())
                            retVal.Add(Convert.ToInt32(rdr["ID"]));
                return retVal;
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting unsynced children from database: {0}", e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get a list of children identifiers who have data which is not synchronized
        /// </summary>
        public List<GIIS.DataLayer.Child> GetUnsyncedChildrenRecords()
        {
            try
            {
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("HIE_GET_CHILDREN_WITH_EXPIRED_DATA"))
                using(var dt = new DataTable())
                {
                    dt.Load(cmd.ExecuteReader());
                    return GIIS.DataLayer.Child.GetChildAsList(dt);
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting unsynced children records from database: {0}", e.ToString());
                throw;
            }
            
        }

        /// <summary>
        /// Get the existing document from the last sync
        /// </summary>
        public Decimal? GetExistingDocumentId(int childId)
        {
            try
            {
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_get_last_document_id"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@childId", DbType.Int32, childId));
                    var retVal = cmd.ExecuteScalar();
                    return retVal == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(retVal);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting last synced document from database: {0}", e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get new vaccination records
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<GIIS.DataLayer.VaccinationEvent> GetNewVaccinationEvents(int childId)
        {
            try
            {
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_get_unsynced_vaccinations"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@childId", DbType.Int32, childId));
                    using (var dt = new DataTable())
                    {
                        dt.Load(cmd.ExecuteReader());
                        return GIIS.DataLayer.VaccinationEvent.GetVaccinationEventAsList(dt);
                    }
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting unsynced vaccination records from database: {0}", e.ToString());
                throw;
            }
            
            
        }

        /// <summary>
        /// Get new weight events
        /// </summary>
        public List<GIIS.DataLayer.ChildWeight> GetNewWeights(int childId)
        {
            try
            {
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_get_unsynced_weights"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@childId", DbType.Int32, childId));
                    using (var dt = new DataTable())
                    {
                        dt.Load(cmd.ExecuteReader());
                        return GIIS.DataLayer.ChildWeight.GetChildWeightAsList(dt);
                    }
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting unsynced weight records from database: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Get new appointments
        /// </summary>
        public List<GIIS.DataLayer.VaccinationAppointment> GetNewAppointments(int childId)
        {
            try
            {
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_get_unsynced_appointments"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@childId", DbType.Int32, childId));
                    using (var dt = new DataTable())
                    {
                        dt.Load(cmd.ExecuteReader());
                        return GIIS.DataLayer.VaccinationAppointment.GetVaccinationAppointmentAsList(dt);
                    }
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting unsynced appointment records from database: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Register a document synchronization
        /// </summary>
        public Decimal RegisterDocumentSync(int childId, decimal jobId, decimal? replacesDocumentId)
        {
            try
            {
                this.GetOrCreateTransaction();
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_create_document_sync"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@childId", DbType.Int32, childId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@jobId", DbType.Decimal, jobId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@replacesDocumentId", DbType.Decimal, replacesDocumentId.HasValue ? replacesDocumentId.Value : (object)DBNull.Value));
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error registering document: {0}", e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get the document id which contains the specified vaccination
        /// </summary>
        public Decimal? GetVaccinationEventDocumentId(int vaccinationId)
        {

            try
            {
                using(var cmd = this.CreateCommandStoredProc("hie_get_document_id_for_vaccination_event"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@vaccinationEventId", DbType.Int32, vaccinationId));
                    var retVal = cmd.ExecuteScalar();
                    return retVal == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(retVal);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting vaccination document id: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Get the child ecid
        /// </summary>
        public String GetChildEcid(int childId)
        {
            try
            {
                using (var cmd = this.CreateCommandStoredProc("hie_get_child_ecid"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@childId", DbType.Int32, childId));
                    var retVal = cmd.ExecuteScalar();
                    return retVal == DBNull.Value ? null : Convert.ToString(retVal);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting child ecid document id: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Get the document id which contains the specified vaccination
        /// </summary>
        public Decimal? GetChildWeightDocumentId(int weightId)
        {

            try
            {
                using (var cmd = this.CreateCommandStoredProc("hie_get_document_id_for_child_weight"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@childWeightId", DbType.Int32, weightId));
                    var retVal = cmd.ExecuteScalar();
                    return retVal == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(retVal);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting child weught document id: {0}", e.ToString());
                throw;
            }

        }


        /// <summary>
        /// Get the document id which contains the specified vaccination
        /// </summary>
        public Decimal? GetVaccinationAppointmentDocumentId(int appointmentId)
        {

            try
            {
                using (var cmd = this.CreateCommandStoredProc("hie_get_document_id_for_vaccination_appointment"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@vaccinationAppointmentId", DbType.Int32, appointmentId));
                    var retVal = cmd.ExecuteScalar();
                    return retVal == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(retVal);
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Error getting appointment document id: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Register document value
        /// </summary>
        public void RegisterDocumentVaccinationEvent(int vaccinationId, decimal documentId, string action)
        {
            try
            {
                this.GetOrCreateTransaction();
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_create_document_vaccination_event"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@documentId", DbType.Decimal, documentId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@vaccinationEvent", DbType.Int32, vaccinationId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@action", DbType.StringFixedLength, action));
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error registering document: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Register document value
        /// </summary>
        public void RegisterDocumentVaccinationAppointment(int appointmentId, decimal documentId, string action)
        {
            try
            {
                this.GetOrCreateTransaction();
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_create_document_appointment"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@documentId", DbType.Decimal, documentId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@appointmentId", DbType.Int32, appointmentId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@action", DbType.StringFixedLength, action));
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error registering document: {0}", e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Register document value
        /// </summary>
        public void RegisterDocumentChildWeight(int weight, decimal documentId, string action)
        {
            try
            {
                this.GetOrCreateTransaction();
                List<Int32> retVal = new List<int>();
                // We need a transaction, create one if not already created
                using (var cmd = this.CreateCommandStoredProc("hie_create_document_child_weight"))
                {
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@documentId", DbType.Decimal, documentId));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@weightId", DbType.Int32, weight));
                    cmd.Parameters.Add(this.CreateParameterIn(cmd, "@action", DbType.StringFixedLength, action));
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error registering document: {0}", e.ToString());
                throw;
            }

        }

    }
}
