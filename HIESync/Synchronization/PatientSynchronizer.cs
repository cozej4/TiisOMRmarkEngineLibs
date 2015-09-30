using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AtnaApi.Model;
using HIESync.Data;
using MARC.Everest.DataTypes;
using MARC.Everest.Threading;
using MARC.HI.EHRS.CR.Notification.PixPdq;
using NHapi.Base.Model;
using NHapi.Base.Util;
using NHapi.Model.V25.Group;
using NHapi.Model.V25.Message;
using NHapi.Model.V25.Segment;
using HIESync.Util;
using System.Collections;

namespace HIESync.Synchronization
{
    /// <summary>
    /// Patient sync functionality
    /// </summary>
    public class PatientSynchronizer : IDisposable
    {
        /// <summary>
        /// The context of synchronization
        /// </summary>
        private SynchronizationContext m_context;
        // The sender
        private MllpMessageSender m_sender;
        // Threads for processing 
        private WaitThreadPool m_waitThread = new WaitThreadPool();
        // Worker threads
        private Stack<RSP_K21_QUERY_RESPONSE> m_workerItems = new Stack<RSP_K21_QUERY_RESPONSE>();
        // True if the sync is in error state
        private bool m_errorState = false;
        // Sync state
        private Object m_syncState = new object();

        /// <summary>
        /// Creates a new instance of the sync context
        /// </summary>
        public PatientSynchronizer(SynchronizationContext context)
        {
            this.m_context = context;

            Uri crEndpoint = new Uri(ConfigurationManager.AppSettings["crEndpoint"]);
            String localCertHash = ConfigurationManager.AppSettings["myCertificateHash"],
                remoteCertHash = ConfigurationManager.AppSettings["remoteCertificateHash"];


            // Certs
            X509Certificate2 localCert = null, remoteCert = null;
            if (localCertHash != null)
            {
                string[] parts = localCertHash.Split(',');
                StoreLocation loc = (StoreLocation)Enum.Parse(typeof(StoreLocation), parts[0]);
                StoreName name = (StoreName)Enum.Parse(typeof(StoreName), parts[1]);

                localCert = MllpMessageSender.FindCertificate(name, loc, X509FindType.FindByThumbprint, parts[2]);
            }
            if (remoteCertHash != null)
            {
                string[] parts = remoteCertHash.Split(',');
                StoreLocation loc = (StoreLocation)Enum.Parse(typeof(StoreLocation), parts[0]);
                StoreName name = (StoreName)Enum.Parse(typeof(StoreName), parts[1]);


                remoteCert = MllpMessageSender.FindCertificate(name, loc, X509FindType.FindByThumbprint, parts[2]);

            }
            this.m_sender = new MllpMessageSender(crEndpoint, localCert, remoteCert);
        }

        /// <summary>
        /// Pull clients
        /// </summary>
        public void PullClients()
        {
            Trace.TraceInformation("{0}: -- Starting PULL of patients from CR --", this.m_context.JobId);

            QBP_Q21 request = null;

            // Get the last sync to be completed
            using (SyncData dao = new SyncData())
            {
                // Last modified filter
                var lastSync = dao.GetLastSync();
                DateTime? lastModifiedFilter = lastSync == null ? null : (DateTime?)lastSync.StartTime;

                // Create a PDQ message
                if (lastModifiedFilter.HasValue)
                {
                    Trace.TraceInformation("{0}: Last sync was on {1}", this.m_context.JobId, lastModifiedFilter.Value
                        );
                    // Create a series of OR parameters representing days we're out of sync
                    for (int i = 0; i <= this.m_context.StartTime.Subtract(lastModifiedFilter.Value).TotalDays; i++)
                    {
                        request = this.CreatePDQSearch(new KeyValuePair<string, string>("@PID.33", new TS(this.m_context.StartTime.AddDays(-i), DatePrecision.Day))) as QBP_Q21;
                        Trace.TraceInformation("{0}: Only PULL patients modified on {1:yyyy-MMM-dd}", this.m_context.JobId, new TS(this.m_context.StartTime.AddDays(-i), DatePrecision.Day).DateValue);
                        this.m_waitThread.QueueUserWorkItem(this.PullPatientsAsync, request);
                    }
                }
                else // No last modification date, we have to trick the CR into giving us a complete list
                {
                    for (int i = DateTime.Now.Year - 3; i <= DateTime.Now.Year; i++)
                    {

                        request = this.CreatePDQSearch(new KeyValuePair<String, String>("@PID.8", "F"), new KeyValuePair<String, String>("@PID.7", string.Format("{0:0000}", i))) as QBP_Q21;
                        this.m_waitThread.QueueUserWorkItem(this.PullPatientsAsync, request);
                        request = this.CreatePDQSearch(new KeyValuePair<String, String>("@PID.8", "M"), new KeyValuePair<String, String>("@PID.7", string.Format("{0:0000}", i))) as QBP_Q21;
                        this.m_waitThread.QueueUserWorkItem(this.PullPatientsAsync, request);
                    }
                }
            }

            // The wtp worker for query contiuation
            // This is when the response is not complete but there are more results waiting
            this.m_waitThread.WaitOne();
            if (this.m_errorState)
                throw new InvalidOperationException("Sync resulted in error state");

            // Work items
            while (this.m_workerItems.Count > 0)
                this.m_waitThread.QueueUserWorkItem(this.ProcessPIDAsync, this.m_workerItems.Pop());

            this.m_waitThread.WaitOne();

            if (this.m_errorState)
                throw new InvalidOperationException("Sync resulted in error state");

        }

        /// <summary>
        /// Push clients
        /// </summary>
        public void PushClients()
        {
            Trace.TraceInformation("{0}: -- Starting PUSH of patients to CR --", this.m_context.JobId);

            List<Int32> unsyncedChildren = new List<int>();

            // Get the last sync to be completed
            using (SyncData dao = new SyncData())
            {
                // Last modified filter
                var lastSync = dao.GetLastSync();
                DateTime? lastModifiedFilter = lastSync == null ? null : (DateTime?)lastSync.StartTime;

                foreach (var id in dao.GetUnsyncedChildrenId())
                    this.m_waitThread.QueueUserWorkItem(this.PushPatientsAsync, id);

            }

            // Wait for the worker threads to finish
            this.m_waitThread.WaitOne();
            if (this.m_errorState)
                throw new InvalidOperationException("Sync resulted in error state");

        }

        /// <summary>
        /// Process a response message doing query continuation if needed
        /// </summary>
        private void PushPatientsAsync(Object state)
        {
            Int32 childId = (Int32)state;

            // Get the child record
            try
            {
                GIIS.DataLayer.Child child = GIIS.DataLayer.Child.GetChildById(childId);


                // Determine if this is an update to information (A08) or registration (A04)
                using (var dao = new SyncData())
                {
                    ADT_A01 request = this.CreateADT(child, dao);

                    ActionType action = ActionType.Create;
                    String ecid = dao.GetPatientEcid(childId);
                    if (ecid == null) // no ECID = Register
                        this.UpdateMSH(request.MSH, "ADT_A01", "ADT", "A04");
                    else
                    {
                        this.UpdateMSH(request.MSH, "ADT_A01", "ADT", "A08");
                        action = ActionType.Update;
                    }

                    // Send the message
                    var response = this.m_sender.SendAndReceive(request) as NHapi.Model.V231.Message.ACK;
                    AuditUtil.SendPIXAudit(request, response);
                    if (response == null || !response.MSA.AcknowledgementCode.Value.EndsWith("A"))
                    {
                        Trace.TraceError("{0}: Error registering the child in HIE (child id#{1})", this.m_context.JobId, childId);
                        foreach (var err in response.ERR.GetErrorCodeAndLocation())
                            Trace.TraceError("{0}: CR ERR: {1} ({2})", this.m_context.JobId, err.CodeIdentifyingError.Text, err.CodeIdentifyingError.AlternateText);
                        // Kill!
                        Trace.TraceError("Stopping sync");
                        this.m_errorState = true;
                        return;
                    }

                    // Get the ECID if not already got
                    if (action == ActionType.Create)
                    {
                        var pixSearch = this.CreatePIXSearch(child.Id, ConfigurationManager.AppSettings["giis_child_id_oid"], ConfigurationManager.AppSettings["ecid"]);
                        var pixResponse = this.m_sender.SendAndReceive(pixSearch) as RSP_K23;
                        AuditUtil.SendPIXAudit(pixSearch, pixResponse);
                        // Is the response success?
                        if (pixResponse == null || pixResponse.QAK.QueryResponseStatus.Value != "OK")
                        {
                            Trace.TraceError("{0}: Error retrieving the ECID for created patient", this.m_context.JobId);
                            foreach (var err in pixResponse.ERR.GetErrorCodeAndLocation())
                                Trace.TraceError("{0}: CR ERR: {1} ({2})", this.m_context.JobId, err.CodeIdentifyingError.Text, err.CodeIdentifyingError.AlternateText);
                            // Kill!
                            Trace.TraceError("Stopping sync");
                            this.m_errorState = true;
                            return;
                        }
                        else
                        {
                            var cx = pixResponse.QUERY_RESPONSE.PID.GetPatientIdentifierList(0);
                            // Sanity check
                            if (cx.AssigningAuthority.UniversalID.Value.Equals(ConfigurationManager.AppSettings["ecid"]))
                                ecid = cx.IDNumber.Value;
                            else
                            {
                                Trace.TraceError("{0}: Should not be here! CX.4 indicates ECID OID is {1} but configuration is {2}?", this.m_context.JobId, cx.AssigningAuthority.UniversalID.Value, ConfigurationManager.AppSettings["ecid"]);
                                this.m_errorState = true;
                                return;
                            }
                        }
                    }

                    // Update
                    if (String.IsNullOrEmpty(child.TempId))
                        child.TempId = ecid;

                    child.ModifiedOn = DateTime.Now;
                    child.ModifiedBy = Int32.Parse(ConfigurationManager.AppSettings["giis_authority_user_id"]);
                    GIIS.DataLayer.Child.Update(child);

                    dao.RegisterPatientSync(childId, this.m_context.JobId, action, ecid);
                    dao.Commit();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                this.m_errorState = true;
            }
        }

        /// <summary>
        /// Create a PIX search message
        /// </summary>
        private QBP_Q21 CreatePIXSearch(int localId, string localDomain, string targetDomain)
        {
            QBP_Q21 retVal = new QBP_Q21();
            this.UpdateMSH(retVal.MSH, "QBP_Q21", "QBP", "Q23");
            Terser terser = new Terser(retVal);
            terser.Set("/QPD-1", "IHE PIX Query");
            terser.Set("/QPD-2", Guid.NewGuid().ToString().Substring(0, 8));
            terser.Set("/QPD-3-1", localId.ToString());
            terser.Set("/QPD-3-4-2", localDomain);
            terser.Set("/QPD-3-4-3", "ISO");
            terser.Set("/QPD-4-4-2", targetDomain);
            terser.Set("/QPD-4-4-3", "ISO");
            return retVal;
        }

        /// <summary>
        /// Process a response message doing query continuation if needed
        /// </summary>
        private void PullPatientsAsync(Object state)
        {
            // Cast request
            QBP_Q21 request = state as QBP_Q21;

            // Send the PDQ message
            try
            {
                var response = this.m_sender.SendAndReceive(request) as RSP_K21;
                AuditUtil.SendPDQAudit(request, response);
                if (response == null || response.MSA.AcknowledgmentCode.Value != "AA")
                {
                    foreach (var err in response.ERR.GetErrorCodeAndLocation())
                        Trace.TraceError("{0}: CR ERR: {1} ({2})", this.m_context.JobId, err.CodeIdentifyingError.Text, err.CodeIdentifyingError.AlternateText);
                    // Kill!
                    Trace.TraceError("Stopping sync");
                    this.m_errorState = true;

                }

                // Is there a continuation pointer?
                if (!String.IsNullOrEmpty(response.DSC.ContinuationPointer.Value))
                {
                    Trace.TraceInformation("{0}: Need to continue query", this.m_context.JobId);
                    request.DSC.ContinuationPointer.Value = response.DSC.ContinuationPointer.Value;
                    this.UpdateMSH(request.MSH, "QBP_Q21", "QBP", "Q22");
                    this.m_waitThread.QueueUserWorkItem(this.PullPatientsAsync, request);
                }

                // Process the patients in this response
                lock (this.m_syncState)
                    for (int i = 0; i < response.QUERY_RESPONSERepetitionsUsed; i++)
                    {
                        var responseData = response.GetQUERY_RESPONSE(i);
                        this.m_workerItems.Push(responseData);
                    }

                // Relieve memorypressure 
                lock (this.m_syncState)
                    while (this.m_workerItems.Count > 2048)
                    {
                        this.m_waitThread.QueueUserWorkItem(this.ProcessPIDAsync, this.m_workerItems.Pop());
                    }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                this.m_errorState = true;
            }
        }

        /// <summary>
        /// Process PID data async manner
        /// </summary>
        private void ProcessPIDAsync(object state)
        {
            try
            {
                var responseData = state as RSP_K21_QUERY_RESPONSE;
                GIIS.DataLayer.Child child = null;

                // Now sync patient data if they don't exist

                // Is there a GIIS identifier in the list of identifiers for this patient?
                String ecidIdentifier = responseData.PID.GetPatientIdentifierList().Where(id => id.AssigningAuthority.UniversalID.Value == ConfigurationManager.AppSettings["ecid"]).Select(o => o.IDNumber.Value).First();

                List<String> localGiisIdentifier = responseData.PID.GetPatientIdentifierList().Where(id => id.AssigningAuthority.UniversalID.Value == ConfigurationManager.AppSettings["giis_child_id_oid"]).Select(o => o.IDNumber.Value).ToList();

                // If there is a local GIIS identifier! The patient exists in GIIS 
                if (localGiisIdentifier.Count > 0)
                {
                    var children = localGiisIdentifier.Select(o => GIIS.DataLayer.Child.GetChildById(Int32.Parse(o))).ToList();
                    children.RemoveAll(o => o == null);
                    if(children.Count == 0)
                    {
                        Trace.TraceInformation("{0} : No children records found with local ID, maybe they're gone?", this.m_context.JobId);
                        return;
                    }

                    // Find the child that has the ECID
                    using (var dao = new SyncData())
                    {
                        // Get the survivor or the only
                        child = children.FirstOrDefault(o => o != null && o.IsActive && dao.GetChildEcid(o.Id) == ecidIdentifier);
                        if (child == null && children.Count == 1)
                        {
                            child = children.First();
                            children.Clear();
                        }
                        else
                            children.RemoveAll(o => o.Id == child.Id && child.IsActive);

                        // Is the record in the CR newer than the record in GIIS?
                        if (((TS)responseData.PID.LastUpdateDateTime.Time.Value).DateValue.Date > child.ModifiedOn.Date.AddDays(1))
                        {
                            // Register that we've updated this patient in the sync log
                            Trace.TraceInformation("{0} : Updating child record {1}", this.m_context.JobId, child.Id);

                            // Copy fields
                            this.CopyChildRecordFields(child, responseData.PID);
                            // Now update
                            GIIS.DataLayer.Child.Update(child);
                            dao.RegisterPatientSync(child.Id, this.m_context.JobId, ActionType.Update, ecidIdentifier);
                        }
                        else
                        {
                            Trace.TraceWarning("{0} : Child record {1} in GIIS appears to be up to date", this.m_context.JobId, child.Id);
                            //return;
                        }

                        // Merge patient data
                        if (children.Count > 0)
                        {

                            // Load all completed vaccinations/appointments/weights for the survivor
                            var survivorsVaccinationEvents = GIIS.DataLayer.VaccinationEvent.GetImmunizationCard(child.Id).Where(o => o.VaccinationStatus || o.NonvaccinationReasonId != 0).ToList();
                            var survivorsNonVaccinationAppointments = GIIS.DataLayer.VaccinationEvent.GetImmunizationCard(child.Id).Where(o => !o.VaccinationStatus && o.NonvaccinationReasonId == 0).Select(o => o.Appointment).Distinct(new VaccinationAppointmentComparator());
                            var survivorsWeightEvents = GIIS.DataLayer.ChildWeight.GetChildWeightByChildId(child.Id);
                            var survivorsSupplements = GIIS.DataLayer.ChildSupplements.GetChildSupplementsByChild(child.Id);

                            // Now we want to add vaccination events
                            foreach (var victim in children)
                            {
                                Trace.TraceInformation("Merging CHILD {0} INTO {1}", victim.Id, child.Id);
                                // GEt all victim's vaccination events that were not given to the survivor
                                IEnumerable victimsVaccinationEvents = GIIS.DataLayer.VaccinationEvent.GetImmunizationCard(victim.Id)
                                    .Where(o => (o.VaccinationStatus || o.NonvaccinationReasonId != 0) &&
                                        !survivorsVaccinationEvents.Exists(s => s.DoseId == o.DoseId)),
                                    victimsWeightEvents = GIIS.DataLayer.ChildWeight.GetChildWeightByChildId(victim.Id);
                                var victimSupplements = GIIS.DataLayer.ChildSupplements.GetChildSupplementsByChild(victim.Id);

                                // Now we want to copy from victim to survivor
                                foreach (GIIS.DataLayer.VaccinationEvent victimEvent in victimsVaccinationEvents)
                                {
                                    victimEvent.ChildId = child.Id;
                                    victimEvent.ModifiedBy = Int32.Parse(ConfigurationManager.AppSettings["giis_authority_user_id"]);
                                    victimEvent.ModifiedOn = DateTime.Now;
                                    GIIS.DataLayer.VaccinationEvent.Insert(victimEvent);
                                }

                                // Now copy weights over
                                foreach (GIIS.DataLayer.ChildWeight weight in victimsWeightEvents)
                                {
                                    weight.ChildId = child.Id;
                                    weight.ModifiedBy = Int32.Parse(ConfigurationManager.AppSettings["giis_authority_user_id"]);
                                    weight.ModifiedOn = DateTime.Now;
                                    GIIS.DataLayer.ChildWeight.Insert(weight);
                                }

                                // Supplements
                                if (survivorsSupplements != null)
                                {
                                    survivorsSupplements.Mebendezol = victimSupplements.Mebendezol || survivorsSupplements.Mebendezol;
                                    survivorsSupplements.Vita = victimSupplements.Vita || survivorsSupplements.Vita;
                                    GIIS.DataLayer.ChildSupplements.Update(survivorsSupplements);
                                }

                                // Obsolete the old version
                                victim.StatusId = GIIS.DataLayer.Status.GetStatusByName("Duplicate").Id;
                                GIIS.DataLayer.Child.Update(victim);

                                GIIS.DataLayer.ChildMerges mergeData = new GIIS.DataLayer.ChildMerges();
                                mergeData.ChildId = child.Id;
                                mergeData.SubsumedId = victim.Id;
                                mergeData.EffectiveDate = DateTime.Now;
                                GIIS.DataLayer.ChildMerges.Insert(mergeData);
                            }
                        }

                        dao.Commit();
                    }
                }
                else // We need to create the child record in GIIS
                {

                    Trace.TraceInformation("{0} : Creating child record from ECID {1}", this.m_context.JobId, ecidIdentifier);

                    // Child
                    child = new GIIS.DataLayer.Child();
                    this.CopyChildRecordFields(child, responseData.PID);

                    try
                    {
                        // Is the child older than the last vaccination? If so we don't really care
                        if (GIIS.DataLayer.Dose.GetDosesByDates(child.Birthdate).Count == 0)
                        {
                            Trace.TraceInformation("Don't care about this child as they don't have any vaccinations required");
                            return;
                        }

                        child.Id = GIIS.DataLayer.Child.Insert(child);
                        GIIS.DataLayer.VaccinationAppointment.InsertVaccinationsForChild(child.Id, 1);

                        // Register the patient in the PIX manager
                        // Register the fact we create the child in the sync log
                        using (var dao = new SyncData())
                        {
                            dao.RegisterPatientSync(child.Id, this.m_context.JobId, ActionType.Create, ecidIdentifier);
                            ADT_A01 message = this.CreateADT(child, dao);
                            var response = this.m_sender.SendAndReceive(message) as NHapi.Model.V231.Message.ACK;
                            AuditUtil.SendPIXAudit(message, response);
                            if (!response.MSA.AcknowledgementCode.Value.EndsWith("A"))
                            {
                                Trace.TraceError("{0} : Registration of the patient failed in CR");
                                foreach (var er in response.ERR.GetErrorCodeAndLocation())
                                    Trace.TraceError("{0} : ERR {1}", this.m_context.JobId, er.CodeIdentifyingError.Text.Value);
                            }
                            dao.Commit();
                        }



                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("{0} : Error registering child record: {1}", this.m_context.JobId, e);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceInformation(e.ToString());
                this.m_errorState = true;
            }
        }

        /// <summary>
        /// Update a child record to match the PID segment
        /// </summary>
        private void CopyChildRecordFields(GIIS.DataLayer.Child child, PID pid)
        {

            // Gender
            if (!String.IsNullOrEmpty(pid.AdministrativeSex.Value))
                child.Gender = pid.AdministrativeSex.Value == "M";

            // Date of birth
            if (!String.IsNullOrEmpty(pid.DateTimeOfBirth.Time.Value))
                child.Birthdate = (DateTime)(TS)pid.DateTimeOfBirth.Time.Value;

            // Is part of a multiple birth?
            if (!String.IsNullOrEmpty(pid.MultipleBirthIndicator.Value))
                child.Notes = "twin";

            // Phone
            if (!String.IsNullOrEmpty(pid.GetPhoneNumberHome(0).TelephoneNumber.Value))
                child.Phone = pid.GetPhoneNumberHome(0).TelephoneNumber.Value;
            else if (!String.IsNullOrEmpty(pid.GetPhoneNumberHome(0).AnyText.Value))
                child.Phone = pid.GetPhoneNumberHome(0).AnyText.Value;

            // Mother's name?
            if (pid.MotherSMaidenNameRepetitionsUsed > 0)
            {
                child.MotherLastname = pid.GetMotherSMaidenName()[0].FamilyName.Surname.Value;
                child.MotherFirstname = pid.GetMotherSMaidenName()[0].GivenName.Value;
            }

            // Name
            if (pid.PatientNameRepetitionsUsed > 0)
            {
                child.Lastname1 = pid.GetPatientName()[0].FamilyName.Surname.Value;
                child.Lastname2 = pid.GetPatientName()[0].FamilyName.OwnSurname.Value;
                child.Firstname1 = pid.GetPatientName()[0].GivenName.Value;
                child.Firstname2 = pid.GetPatientName()[0].SecondAndFurtherGivenNamesOrInitialsThereof.Value;
            }

            // Identification numbers
            foreach (var id in pid.GetPatientIdentifierList())
                if (id.AssigningAuthority.UniversalID.Value == ConfigurationManager.AppSettings["giis_child_alt_id_1_oid"])
                    child.IdentificationNo1 = id.IDNumber.Value;
                else if (id.AssigningAuthority.UniversalID.Value == ConfigurationManager.AppSettings["giis_child_alt_id_2_oid"])
                    child.IdentificationNo2 = id.IDNumber.Value;
                else if (id.AssigningAuthority.UniversalID.Value == ConfigurationManager.AppSettings["giis_child_alt_id_3_oid"])
                    child.IdentificationNo3 = id.IDNumber.Value;
                else if (id.AssigningAuthority.UniversalID.Value == ConfigurationManager.AppSettings["giis_child_barcode_oid"])
                    child.BarcodeId = id.IDNumber.Value;
                else if (id.AssigningAuthority.UniversalID.Value == ConfigurationManager.AppSettings["ecid"])
                    child.TempId = id.IDNumber.Value;

            // Birthplace - 
            // TODO: How does this map to GIIS?
            if (!String.IsNullOrEmpty(pid.BirthPlace.Value))
            {
                var place = GIIS.DataLayer.Birthplace.GetBirthplaceByName(pid.BirthPlace.Value);
                if (place == null)
                    Trace.TraceWarning("{0} : Could not map birthplace {1} for child {2}", this.m_context.JobId, pid.BirthPlace.Value, child.Id);
                else
                    child.BirthplaceId = place.Id;
            }

            // Address information
            // TODO: How does this map to GIIS?
            if (pid.PatientAddressRepetitionsUsed > 0)
            {

                // Domicile = Cascade down the heirarchy
                var xad = pid.GetPatientAddress()[0];
                GIIS.DataLayer.Place current = null;

                if (!String.IsNullOrEmpty(xad.CensusTract.Value)) // prefer census tract
                    current = GIIS.DataLayer.Place.GetPlaceByCode(xad.CensusTract.Value);
                if (current == null)
                {
                    Queue<String> placeSearch = new Queue<string>(new String[] {
                        xad.Country.Value,
                        xad.StateOrProvince.Value, // Region
                        xad.CountyParishCode.Value, // District
                        xad.City.Value, // Ward
                        xad.StreetAddress.StreetOrMailingAddress.Value, // Village
                        xad.OtherDesignation.Value // Kitongoji
                    });
                    while (placeSearch.Count > 0)
                    {
                        var xadPart = placeSearch.Dequeue();
                        if (xadPart == null) continue;
                        
                        var candidate = GIIS.DataLayer.Place.GetPlaceByName(xadPart, current == null ? 0 : current.Id);
                        if (candidate == null) break;
                        current = candidate;
                    }
                }
                if (current != null)
                {
                    if (child.DomicileId != current.Id)
                    {
                        child.DomicileId = current.Id;
                        if (child.Healthcenter == null)
                        {
                            if (current.HealthFacility != null)
                                child.HealthcenterId = current.HealthFacility.Id;
                            else
                                child.HealthcenterId = Int32.Parse(ConfigurationManager.AppSettings["giis_unknown_facility"]);
                        }
                        else
                            Trace.TraceInformation("Will not update child {0} health facility", child.Id);
                    }
                }


            }

            if (child.Healthcenter == null)
                child.HealthcenterId = Int32.Parse(ConfigurationManager.AppSettings["giis_unknown_facility"]);


            // Death?
            if (pid.PatientDeathIndicator.Value == "Y")
            {
                child.StatusId = GIIS.DataLayer.Status.GetStatusByName("Dead").Id;
                child.IsActive = false;
            }
            else
            {
                child.StatusId = GIIS.DataLayer.Status.GetStatusByName("Active").Id;
                child.IsActive = true;
            }
            child.StatusId = child.Status.Id;

            if (pid.LastUpdateDateTime.Time.Value != null)
                child.ModifiedOn = ((TS)pid.LastUpdateDateTime.Time.Value).DateValue;
            else
                child.ModifiedOn = DateTime.Now;
            child.ModifiedBy = Int32.Parse(ConfigurationManager.AppSettings["giis_authority_user_id"]);

            if (child.SystemId == null)
            {
                lock(this.m_syncState)
                    child.SystemId = String.Format("{0:X16}", BitConverter.ToInt64(Guid.NewGuid().ToByteArray().Take(16).ToArray(), 0), BitConverter.ToInt64(Guid.NewGuid().ToByteArray().Skip(8).Take(8).ToArray(), 0));
            }

        }

        /// <summary>
        /// Create an ADT message for the child
        /// </summary>
        private ADT_A01 CreateADT(GIIS.DataLayer.Child child, SyncData dao = null)
        {
            ADT_A01 retVal = new ADT_A01();
            retVal.MSH.VersionID.VersionID.Value = "2.3.1";

            this.UpdateMSH(retVal.MSH, "ADT_A01", "ADT", "A01");
            this.UpdatePID(retVal.PID, child, dao);

            return retVal;
        }

        /// <summary>
        /// Update PID segment from a child
        /// </summary>
        private void UpdatePID(PID pid, GIIS.DataLayer.Child child, SyncData dao = null)
        {
            pid.AdministrativeSex.Value = child.Gender ? "M" : "F";
            if (child.Birthplace != null)
                pid.BirthPlace.Value = child.Birthplace.Name;
            pid.DateTimeOfBirth.Time.Value = new TS(child.Birthdate, DatePrecision.Day).Value;

            // Ethnic group
            if (child.Community != null)
                pid.GetEthnicGroup(0).Identifier.Value = child.Community.Name;

            if (!String.IsNullOrEmpty(child.Phone))
                pid.GetPhoneNumberHome(0).AnyText.Value = child.Phone;

            if (!String.IsNullOrEmpty(child.MotherFirstname))
                pid.GetMotherSMaidenName(0).GivenName.Value = child.MotherFirstname;
            if (!String.IsNullOrEmpty(child.MotherLastname))
                pid.GetMotherSMaidenName(0).FamilyName.Surname.Value = child.MotherLastname;

            if (!String.IsNullOrEmpty(child.Firstname1))
                pid.GetPatientName(0).GivenName.Value = child.Firstname1;
            if (!String.IsNullOrEmpty(child.Firstname2))
                pid.GetPatientName(0).SecondAndFurtherGivenNamesOrInitialsThereof.Value = child.Firstname2;
            if (!String.IsNullOrEmpty(child.Lastname1))
                pid.GetPatientName(0).FamilyName.Surname.Value = child.Lastname1;
            if (!String.IsNullOrEmpty(child.Lastname2))
                pid.GetPatientName(0).FamilyName.OwnSurname.Value = child.Lastname2;

            // Domicile
            if (child.Domicile != null)
            {

                if (!String.IsNullOrEmpty(child.Domicile.Code))
                    pid.GetPatientAddress(0).CensusTract.Value = child.Domicile.Code;

                Queue<AbstractPrimitive> addressParts = new Queue<AbstractPrimitive>(new AbstractPrimitive[] {
                    pid.GetPatientAddress(0).OtherDesignation, // Kitongoji
                    pid.GetPatientAddress(0).StreetAddress.StreetOrMailingAddress, // Street or Village
                    pid.GetPatientAddress(0).City, // Ward
                    pid.GetPatientAddress(0).CountyParishCode, // district
                    pid.GetPatientAddress(0).StateOrProvince, // Region
                    pid.GetPatientAddress(0).Country // National
                });

                // Queue places 
                Queue<GIIS.DataLayer.Place> domicileParts = new Queue<GIIS.DataLayer.Place>();
                GIIS.DataLayer.Place current = child.Domicile;
                while (current != null)
                {
                    domicileParts.Enqueue(current);
                    current = current.Parent;
                }

                // Now trim
                while (addressParts.Count > domicileParts.Count)
                    addressParts.Dequeue();

                // Now map
                while (domicileParts.Count > 0)
                    addressParts.Dequeue().Value = domicileParts.Dequeue().Name;

            }

            if (child.Status.Name == "Dead")
                pid.PatientDeathIndicator.Value = "Y";

            // Identifiers
            int i = 0;
            pid.GetPatientIdentifierList(i).IDNumber.Value = child.Id.ToString();
            pid.GetPatientIdentifierList(i).AssigningAuthority.UniversalID.Value = ConfigurationManager.AppSettings["giis_child_id_oid"];
            pid.GetPatientIdentifierList(i++).AssigningAuthority.UniversalIDType.Value = "ISO";

            // 1st alt
            if (!String.IsNullOrEmpty(child.IdentificationNo1) && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["giis_child_alt_id_1_oid"]))
            {
                pid.GetPatientIdentifierList(i).IDNumber.Value = child.IdentificationNo1.ToString();
                pid.GetPatientIdentifierList(i).AssigningAuthority.UniversalID.Value = ConfigurationManager.AppSettings["giis_child_alt_id_1_oid"];
                pid.GetPatientIdentifierList(i++).AssigningAuthority.UniversalIDType.Value = "ISO";
            }
            // 2nd alt
            if (!String.IsNullOrEmpty(child.IdentificationNo2) && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["giis_child_alt_id_2_oid"]))
            {
                pid.GetPatientIdentifierList(i).IDNumber.Value = child.IdentificationNo2.ToString();
                pid.GetPatientIdentifierList(i).AssigningAuthority.UniversalID.Value = ConfigurationManager.AppSettings["giis_child_alt_id_2_oid"];
                pid.GetPatientIdentifierList(i++).AssigningAuthority.UniversalIDType.Value = "ISO";
            }
            // 3rd alt
            if (!String.IsNullOrEmpty(child.IdentificationNo3) && !String.IsNullOrEmpty(ConfigurationManager.AppSettings["giis_child_alt_id_3_oid"]))
            {
                pid.GetPatientIdentifierList(i).IDNumber.Value = child.IdentificationNo3.ToString();
                pid.GetPatientIdentifierList(i).AssigningAuthority.UniversalID.Value = ConfigurationManager.AppSettings["giis_child_alt_id_3_oid"];
                pid.GetPatientIdentifierList(i++).AssigningAuthority.UniversalIDType.Value = "ISO";
            }
            // Barcode
            if (!String.IsNullOrEmpty(child.BarcodeId))
            {
                pid.GetPatientIdentifierList(i).IDNumber.Value = child.BarcodeId.ToString();
                pid.GetPatientIdentifierList(i).AssigningAuthority.UniversalID.Value = ConfigurationManager.AppSettings["giis_child_barcode_oid"];
                pid.GetPatientIdentifierList(i++).AssigningAuthority.UniversalIDType.Value = "ISO";
            }

            // ECID?
            if (dao == null)
                dao = new SyncData();
            string ecid = dao.GetPatientEcid(child.Id);
            if (ecid != null)
            {
                pid.GetPatientIdentifierList(i).IDNumber.Value = ecid;
                pid.GetPatientIdentifierList(i).AssigningAuthority.UniversalID.Value = ConfigurationManager.AppSettings["ecid"];
                pid.GetPatientIdentifierList(i++).AssigningAuthority.UniversalIDType.Value = "ISO";
            }

        }

        /// <summary>
        /// Update the MSH header
        /// </summary>
        private void UpdateMSH(MSH header, String message, String structure, String trigger)
        {
            // Message header
            header.AcceptAcknowledgmentType.Value = "AL"; // Always send response
            header.DateTimeOfMessage.Time.Value = DateTime.Now.ToString("yyyyMMddHHmmss"); // Date/time of creation of message
            header.MessageControlID.Value = Guid.NewGuid().ToString(); // Unique id for message
            header.MessageType.MessageStructure.Value = message; // Message structure type (Query By Parameter Type 21)
            header.MessageType.MessageCode.Value = structure; // Message Structure Code (Query By Parameter)
            header.MessageType.TriggerEvent.Value = trigger; // Trigger event (Event Query 22)
            header.ProcessingID.ProcessingID.Value = "P"; // Production
            header.ReceivingApplication.NamespaceID.Value = "CR"; // Client Registry
            header.ReceivingFacility.NamespaceID.Value = "BID"; // SAMPLE
            header.SendingApplication.NamespaceID.Value = "GIIS"; // What goes here?
            header.SendingFacility.NamespaceID.Value = "BID"; // You're at the college ... right?
        }

        /// <summary>
        /// Create a PDQ search message
        /// </summary>
        /// <param name="filters">The parameters for query</param>
        private QBP_Q21 CreatePDQSearch(params KeyValuePair<String, String>[] filters)
        {
            // Search - Construct a v2 message this is found in IHE ITI TF-2:3.21
            QBP_Q21 message = new QBP_Q21();

            this.UpdateMSH(message.MSH, "QBP_Q21", "QBP", "Q22");
            //message.MSH.VersionID.VersionID.Value = "2.3.1";

            // Message query
            message.QPD.MessageQueryName.Identifier.Value = "Patient Demographics Query";

            // Sometimes it is easier to use a terser
            Terser terser = new Terser(message);
            terser.Set("/QPD-2", Guid.NewGuid().ToString()); // Tag of the query
            terser.Set("/QPD-1", "Patient Demographics Query"); // Name of the query
            for (int i = 0; i < filters.Length; i++)
            {
                terser.Set(String.Format("/QPD-3({0})-1", i), filters[i].Key);
                terser.Set(String.Format("/QPD-3({0})-2", i), filters[i].Value);
            }

            return message;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.m_waitThread.Dispose();
        }
    }
}
