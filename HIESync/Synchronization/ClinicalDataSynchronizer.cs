using HIESync.Data;
using MARC.Everest.DataTypes;
using MARC.Everest.Formatters.XML.Datatypes.R1;
using MARC.Everest.Formatters.XML.ITS1;
using MARC.Everest.RMIM.UV.CDAr2.POCD_MT000040UV;
using MARC.Everest.RMIM.UV.CDAr2.Vocabulary;
using MARC.Everest.Threading;
using MARC.IHE.Xds;
using MARC.IHE.Xds.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HIESync.Synchronization
{
    /// <summary>
    /// Clinical data sync
    /// </summary>
    public class ClinicalDataSynchronizer : IDisposable
    {

        // Thread pool.
        private WaitThreadPool m_threadPool = new WaitThreadPool(1);

        // Context of the sync
        private SynchronizationContext m_context;

        // Error state
        private bool m_errorState;

          /// <summary>
        /// Creates a new instance of the sync context
        /// </summary>
        public ClinicalDataSynchronizer(SynchronizationContext context)
        {
            this.m_context = context;
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            this.m_threadPool.Dispose();
        }

        /// <summary>
        /// Push clinical records onto the stack and upload them to the HIE
        /// </summary>
        public void PushClinical()
        {
            Trace.TraceInformation("{0}: -- Starting PUSH of clinical data to SHR --", this.m_context.JobId);

            using(var dao = new SyncData())
            {
                // Last modified filter
                var lastSync = dao.GetLastSync();
                DateTime? lastModifiedFilter = lastSync == null ? null : (DateTime?)lastSync.StartTime;

                foreach (var child in dao.GetUnsyncedChildrenRecords())
                    this.m_threadPool.QueueUserWorkItem(this.PushClinicalAsync, child);
            }

            Trace.TraceInformation("{0}: -- Finished PUSH of clinical data to SHR --", this.m_context.JobId);

        }

        /// <summary>
        /// Push clinical data to the repository in an async way
        /// </summary>
        private void PushClinicalAsync(object state)
        {
            var child = state as GIIS.DataLayer.Child;

            Trace.TraceInformation("{0}: -- Starting PUSH of GIIS Child {1} --", this.m_context.JobId, child.Id);


            // Child identifier
            string ecid = null;
            using (var dao = new SyncData())
            {
                try
                {
                    ecid = dao.GetPatientEcid(child.Id);
                    if (ecid == null)
                    {
                        Trace.TraceWarning("{0}: GIIS Child {1} has no ECID, has it been Synced?", this.m_context.JobId, state);
                        return;
                    }


                    // Get the existing document id
                    var existingSyncId = dao.GetExistingDocumentId(child.Id);
                    // First we try to register the document
                    var docId = dao.RegisterDocumentSync(child.Id, this.m_context.JobId, existingSyncId);

                    // Get new data to be synced
                    var vaccinations = dao.GetNewVaccinationEvents(child.Id).ToList();
                    var weights = dao.GetNewWeights(child.Id);
                    var adverseEvents = dao.GetNewAppointments(child.Id).Distinct(new VaccinationAppointmentComparator()).ToList();

                    var dates = vaccinations.Select(o => o.VaccinationDate).Union(weights.Select(o => o.Date)).Union(adverseEvents.Select(o => o.ModifiedOn)).ToArray();
                    Trace.TraceInformation("{0}: Child {4} has {1} Vaccinations, {2} Appointments w/AEFI, and {3} weights that are active", this.m_context.JobId, state, vaccinations.Count, adverseEvents.Count, weights.Count, child.Id);

                    if (dates.Length == 0)
                    {
                        Trace.TraceWarning("{0} : GIIS Child {1} has no data to report, not sending", this.m_context.JobId, child.Id);
                        dao.Rollback();
                        return;
                    }

                    var authors = vaccinations.Where(o => o.IsActive).Select(o => o.ModifiedBy).Union(weights.Select(o=>o.ModifiedBy)).Union(adverseEvents.Select(o=>o.ModifiedBy)).Distinct().Select(o => GIIS.DataLayer.User.GetUserById(o)).ToList();
                    authors.AddRange(vaccinations.Where(o => o.IsActive && !authors.Exists(p => p.Id == o.ModifiedBy)).Select(o => o.ModifiedBy).Distinct().Select(o => GIIS.DataLayer.User.GetUserById(o)));

                    DateTime startTime = dates.Min(),
                        stopTime = dates.Max();

                    // Populate the cda
                    // Immunizations
                    var izSection = this.CreateIZSection(vaccinations.Where(o => o.IsActive).Select(o =>
                    {
                        var existingId = dao.GetVaccinationEventDocumentId(o.Id);
                        dao.RegisterDocumentVaccinationEvent(o.Id, docId, existingId.HasValue ? "U" : "C");
                        return this.CreateIZEntry(o, docId, existingId);
                    }).ToArray());
                    // Weights
                    var vsSection = this.CreateVitalSignsSection(weights.Select(o => {
                        var existingId = dao.GetChildWeightDocumentId(o.Id);
                        dao.RegisterDocumentChildWeight(o.Id, docId, existingId.HasValue ? "U" : "C");
                        return this.CreateVitalSignsOrganizer(o, docId, existingId);
                    }).ToArray());
                    // Appointments
                    var aefiSection = this.CreateAllergyIntolerancesSection(adverseEvents.Select(o =>
                    {
                        var existingId = dao.GetVaccinationAppointmentDocumentId(o.Id);
                        dao.RegisterDocumentVaccinationAppointment(o.Id, docId, existingId.HasValue ? "U" : "C");
                        return this.CreateProblemStatement(o, docId, existingId);
                    }).ToArray());
                    var cda = this.CreateICDocument(child, ecid, authors, izSection, vsSection, aefiSection);

                    // Register the document stuff
                    cda.Id = new II(ConfigurationManager.AppSettings["giis_document_id_oid"], docId.ToString());
                    if (existingSyncId.HasValue)
                        cda.RelatedDocument.Add(new RelatedDocument(

                            x_ActRelationshipDocument.APND,
                            new ParentDocument(
                                SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_document_id_oid"], existingSyncId.Value.ToString()))
                            )
                        ));

                    this.ProvideAndRegister(child, ecid, authors, cda.Code, startTime, stopTime, "urn:ihe:pcc:ic:2009", cda);
                    dao.Commit();

                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                    dao.Rollback();
                }
            
            }

            Trace.TraceInformation("{0}: -- Finished PUSH of GIIS Child {1} --", this.m_context.JobId, child.Id);

        }

        /// <summary>
        /// Create an IC document
        /// </summary>
        public ClinicalDocument CreateICDocument(GIIS.DataLayer.Child recordTarget, string ecid, List<GIIS.DataLayer.User> authors, params Section[] sections)
        {
            SET<II> patientIds = new SET<II>()
            {
                new II(ConfigurationManager.AppSettings["ecid"], ecid),
                new II(ConfigurationManager.AppSettings["giis_child_id_oid"], recordTarget.Id.ToString())
            };

            if (!String.IsNullOrEmpty(recordTarget.IdentificationNo1))
                patientIds.Add(new II(ConfigurationManager.AppSettings["giis_child_alt_id_1_oid"], recordTarget.IdentificationNo1));
            if (!String.IsNullOrEmpty(recordTarget.IdentificationNo2))
                patientIds.Add(new II(ConfigurationManager.AppSettings["giis_child_alt_id_2_oid"], recordTarget.IdentificationNo2));
            if (!String.IsNullOrEmpty(recordTarget.IdentificationNo3))
                patientIds.Add(new II(ConfigurationManager.AppSettings["giis_child_alt_id_3_oid"], recordTarget.IdentificationNo3));

            // Map patient address
            var patientAddress = new AD();
            if (recordTarget.Domicile != null)
            {
                patientAddress.Part.Add(new ADXP(recordTarget.Domicile.Code, AddressPartType.CensusTract));
                Queue<AddressPartType> addressParts = new Queue<AddressPartType>(new AddressPartType[] {
                    AddressPartType.AdditionalLocator, // Kitongoji
                    AddressPartType.StreetAddressLine, // Street or Village
                    AddressPartType.City, // Ward
                    AddressPartType.County, // district
                    AddressPartType.State, // Region
                    AddressPartType.Country // National
                });

                // Queue places 
                Queue<GIIS.DataLayer.Place> domicileParts = new Queue<GIIS.DataLayer.Place>();
                GIIS.DataLayer.Place current = recordTarget.Domicile;
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
                    patientAddress.Part.Add(new ADXP(domicileParts.Dequeue().Name, addressParts.Dequeue()));
            }
            else
                patientAddress.NullFlavor = NullFlavor.NoInformation;

            ClinicalDocument doc = new ClinicalDocument()
            {
                TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.1.18.1.2")),
                Id = Guid.NewGuid(),
                Code = new CE<String>("11369-6", "2.16.840.1.113883.6.1", "LOINC", null, "HISTORY OF IMMUNIZATIONS", null),
                Title = "Immunization History",
                TypeId = new II("2.16.840.1.113883.1.3", "POCD_HD000040"),
                RealmCode = SET<CS<BindingRealm>>.CreateSET(BindingRealm.UniversalRealmOrContextUsedInEveryInstance),
                EffectiveTime = DateTime.Now.Subtract(new TimeSpan(1, 0, 0)),
                ConfidentialityCode = x_BasicConfidentialityKind.Normal,
                LanguageCode = "en-US",
                RecordTarget = new List<RecordTarget>()
                {
                    new RecordTarget() { 
                        ContextControlCode = ContextControl.OverridingPropagating,
                        PatientRole = new PatientRole() {
                            Id = patientIds,
                            Addr = SET<AD>.CreateSET(patientAddress),
                            Telecom = SET<TEL>.CreateSET(String.IsNullOrEmpty(recordTarget.Phone) ? new TEL() { NullFlavor = NullFlavor.NoInformation } : new TEL(recordTarget.Phone)),
                            Patient = new Patient() {
                                Name = SET<PN>.CreateSET(PN.FromFamilyGiven(EntityNameUse.Legal, recordTarget.Lastname1, recordTarget.Firstname1)),
                                AdministrativeGenderCode = recordTarget.Gender ? "M" : "F",
                                BirthTime = new TS(recordTarget.Birthdate, DatePrecision.Day)
                            }
                        }
                    }
                },
                Custodian = new Custodian(
                    new AssignedCustodian(
                        new CustodianOrganization(
                            SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_device_id_oid"], "GIIS")),
                            new ON(null, new ENXP[] { new ENXP("GIIS") }),
                            null,
                            null
                        )
                    )
                ),
                Component = new Component2(
                    ActRelationshipHasComponent.HasComponent,
                    true,
                    new StructuredBody()
                )
            };

            // authors
            foreach (var author in authors)
            {
                var v3Author = new Author()
                {
                    ContextControlCode = ContextControl.AdditivePropagating,
                    Time = DateTime.Now.Subtract(new TimeSpan(1, 0, 0)),
                    AssignedAuthor = new AssignedAuthor()
                    {
                        Id = SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_user_id_oid"], author.Id.ToString())),
                        Addr = new SET<AD>() { new AD() { NullFlavor = NullFlavor.NoInformation } },
                        Telecom = new SET<TEL>() { new TEL(String.Format("mailto:{0}", author.Email)) },
                        AssignedAuthorChoice = AuthorChoice.CreatePerson(
                            SET<PN>.CreateSET(PN.FromFamilyGiven(EntityNameUse.Legal, 
                                String.IsNullOrEmpty(author.Lastname) ? null : author.Lastname, 
                                String.IsNullOrEmpty(author.Firstname) ? null : author.Firstname))
                        )
                    }
                };
                // Author rep org
                if (author.HealthFacility != null)
                    v3Author.AssignedAuthor.RepresentedOrganization = new Organization()
                    {
                        Id = new SET<II>(new II(ConfigurationManager.AppSettings["giis_facility_id_oid"], author.HealthFacility.Id.ToString())),
                        Name = SET<ON>.CreateSET(new ON(null, new ENXP[] { new ENXP(author.HealthFacility.Name) }))
                    };

                doc.Author.Add(v3Author);

            }

            // mother?
            if(!String.IsNullOrEmpty(recordTarget.MotherLastname) || !String.IsNullOrEmpty(recordTarget.MotherFirstname))
            {
                doc.Participant.Add(new Participant1(ParticipationType.IND, ContextControl.OverridingNonpropagating, null, new IVL<TS>(new TS(recordTarget.Birthdate, DatePrecision.Year)), null)
                    {
                        AssociatedEntity = new AssociatedEntity(RoleClassAssociative.NextOfKin,
                        new SET<II>( new II() { NullFlavor = NullFlavor.NoInformation }),
                        new CE<string>("MTH", "2.16.840.1.113883.5.111", null, null, "Mother", null),
                        SET<AD>.CreateSET(new AD() { NullFlavor = NullFlavor.NoInformation }),
                        SET<TEL>.CreateSET(new TEL() { NullFlavor = NullFlavor.NoInformation }),
                        new Person(SET<PN>.CreateSET(PN.FromFamilyGiven(EntityNameUse.Legal, recordTarget.MotherLastname, recordTarget.MotherFirstname))),
                        null)
                    });
            }

            foreach (var sct in sections)
                doc.Component.GetBodyChoiceIfStructuredBody().Component.Add(new Component3(
                    ActRelationshipHasComponent.HasComponent,
                    true,
                    sct));

            return doc;
        }

        #region Sections

        /// <summary>
        /// Adverse events section
        /// </summary>
        private Section CreateAllergyIntolerancesSection(params Entry[] entries)
        {
            var retVal = new Section()
            {
                Id = Guid.NewGuid(),
                TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.3.13")),
                Title = "Allergies, Adverse Reactions, Alerts",
                Code = new CE<String>("48765-2", "2.16.840.1.113883.6.1", "LOINC", null, "Allergies, adverse reactions, alerts", null),
                Entry = new List<Entry>(entries)
            };

            string refText = "<table><caption>Adverse Events</caption><thead><tr><th>Date</th><th>Agent</th><th>Health Centre</th><th>Comments</th></tr></thead><tbody>";
            foreach (var ent in entries)
            {
                String refId = string.Format("id{0}", Guid.NewGuid().ToString().Substring(0, 5));
                refText += String.Format("<tr ID=\"{0}\">{1}</tr>", refId, System.Text.Encoding.ASCII.GetString(ent.GetClinicalStatementIfAct().EntryRelationship[0].GetClinicalStatementIfObservation().Text.Data));
                ent.GetClinicalStatementIfAct().EntryRelationship[0].GetClinicalStatementIfObservation().Text.Data = null;
                ent.GetClinicalStatementIfAct().EntryRelationship[0].GetClinicalStatementIfObservation().Text.Reference = new TEL(String.Format("#{0}", refId));

            }
            retVal.Text = refText + "</tbody></table>";
            retVal.Text.Representation = MARC.Everest.DataTypes.Interfaces.EncapsulatedDataRepresentation.XML;
            retVal.Text.MediaType = null;
            return retVal;
        }


        /// <summary>
        /// Create vital signs sections
        /// </summary>
        public Section CreateVitalSignsSection(params Entry[] entries)
        {
            var retVal = new Section()
            {
                Id = Guid.NewGuid(),
                TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.1.5.3.2", "1.3.6.1.4.1.19376.1.5.3.1.3.25")),
                Title = "Vital Signs",
                Code = new CE<String>("8716-3", "2.16.840.1.113883.6.1", "LOINC", null, "Vital Signs", null),
                Entry = new List<Entry>(entries)
            };

            string refText = "<list>";
            foreach (var ent in entries)
            {
                String refId = string.Format("id{0}", Guid.NewGuid().ToString().Substring(0, 5));
                if (ent.GetClinicalStatementIfOrganizer() != null)
                {
                    refText += String.Format("<item ID=\"{0}\"><table><caption>Visit {1}</caption><thead><tr><th>Observation</th><th>Value</th></tr></thead><tbody>", refId, ent.GetClinicalStatementIfOrganizer().EffectiveTime.Value.DateValue.ToString("yyyy-MMM-dd"));
                    foreach (var ente in ent.GetClinicalStatementIfOrganizer().Component)
                    {
                        refId = string.Format("id{0}", Guid.NewGuid().ToString().Substring(0, 5));
                        refText += String.Format("<tr ID=\"{0}\">{1}</tr>", refId, System.Text.Encoding.UTF8.GetString(ente.GetClinicalStatementIfObservation().Text.Data));
                        ente.GetClinicalStatementIfObservation().Text.Data = null;
                        ente.GetClinicalStatementIfObservation().Text.Reference = new TEL(String.Format("#{0}", refId));
                    }
                    refText += "</tbody></table></item>";
                }

            }
            retVal.Text = refText + "</list>";
            retVal.Text.Representation = MARC.Everest.DataTypes.Interfaces.EncapsulatedDataRepresentation.XML;
            retVal.Text.MediaType = null;
            return retVal;
        }

        /// <summary>
        /// Create the IZ section
        /// </summary>
        public Section CreateIZSection(params Entry[] entries)
        {
            var retVal = new Section()
            {
                Id = Guid.NewGuid(),
                TemplateId = LIST<II>.CreateList(new II("2.16.840.1.113883.10.20.1.6"), new II("1.3.6.1.4.1.19376.1.5.3.1.3.23")),
                Title = "Immunizations",
                Code = new CE<String>("11369-6", "2.16.840.1.113883.6.1", "LOINC", null, "HISTORY OF IMMUNIZATIONS", null),
                Entry = new List<Entry>(entries)
            };

            string refText = "<table><caption>Schedule of Vaccinations Given</caption><thead><tr><th>Date</th><th>Status</th><th>Vaccine</th><th>Dose #</th><th>Lot #</th><th>Age at exposure</th><th>Health Facility</th><th>Comments</th></tr></thead><tbody>";
            foreach (var ent in entries)
            {
                String refId = string.Format("id{0}", Guid.NewGuid().ToString().Substring(0, 5));
                refText += String.Format("<tr ID=\"{0}\">{1}</tr>", refId, System.Text.Encoding.ASCII.GetString(ent.GetClinicalStatementIfSubstanceAdministration().Text.Data));
                ent.GetClinicalStatementIfSubstanceAdministration().Text.Data = null;
                ent.GetClinicalStatementIfSubstanceAdministration().Text.Reference = new TEL(String.Format("#{0}", refId));

            }
            retVal.Text = refText + "</tbody></table>";
            retVal.Text.Representation = MARC.Everest.DataTypes.Interfaces.EncapsulatedDataRepresentation.XML;
            retVal.Text.MediaType = null;
            return retVal;
        }
        #endregion 

        #region Entries
        /// <summary>
        /// Create a simple observation
        /// </summary>
        public Observation CreateSimpleObservation(II id, DateTime creationTime, CD<String> code, ANY value, II replacesId)
        {
            var statement = new Observation(x_ActMoodDocumentObservation.Eventoccurrence)
            {
                TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.4.13", "2.16.840.1.113883.10.20.1.31")),
                Id = SET<II>.CreateSET(id),
                Code = code,
                EffectiveTime = new IVL<TS>(creationTime),
                StatusCode = ActStatus.Completed,
                Value = value,
                Text = new ED(String.Format("<tr><td>{0}</td><td>{1}</td></tr>", code.DisplayName, value.ToString()))
            };

            // Replacement?
            if (replacesId != null)
            {
                statement.Reference.Add(new Reference(
                    x_ActRelationshipExternalReference.RPLC,
                    new ExternalAct(
                        "SBADM", SET<II>.CreateSET(replacesId), null, null
                    )
                ));
            }

            return statement;
        }

        /// <summary>
        /// Create an immunization content (IC) document with minimal data
        /// </summary>
        public Entry CreateIZEntry(GIIS.DataLayer.VaccinationEvent vaccEvent, decimal documentId, decimal? previousDocId)
        {

            Trace.TraceInformation("{0} : Creating entry for vaccination event {1}", this.m_context.JobId, vaccEvent.Id);

            Consumable consumable = null;
            if(vaccEvent.Dose != null && vaccEvent.Dose.ScheduledVaccination != null 
                && vaccEvent.Dose.ScheduledVaccination.Item != null 
                && vaccEvent.Dose.ScheduledVaccination.Item.Hl7Vaccine != null)
            {
                var hl7Vaccine = vaccEvent.Dose.ScheduledVaccination.Item.Hl7Vaccine;
                consumable = new Consumable(
                    new ManufacturedProduct(RoleClassManufacturedProduct.ManufacturedProduct)
                    {
                        ManufacturedDrugOrOtherMaterial = new Material(EntityDeterminerDetermined.Described,
                            new CE<String>(hl7Vaccine.CvxCode, "2.16.840.1.113883.6.59", "HL7 CVX", null, hl7Vaccine.Code, (ED)vaccEvent.Dose.ScheduledVaccination.Name),
                            new EN(null, new ENXP[] { new ENXP(vaccEvent.Dose.ScheduledVaccination.Item.Name) }),
                            vaccEvent.VaccineLot
                        )
                    }
                );
            }
            else
            {
                Trace.TraceWarning("{0} : Vaccination event {1} has no HL7 code for the vaccine given", this.m_context.JobId, vaccEvent.Id);
                consumable = new Consumable(
                    new ManufacturedProduct(RoleClassManufacturedProduct.ManufacturedProduct)
                    {
                        ManufacturedDrugOrOtherMaterial = new Material(EntityDeterminerDetermined.Described,
                            new CE<String>(null, "2.16.840.1.113883.6.59", "HL7 CVX", null, null, null)
                            {
                                NullFlavor = NullFlavor.Other,
                                Translation = SET<CD<string>>.CreateSET(
                                    new CD<String>(vaccEvent.Dose.ScheduledVaccination.Item.Code, ConfigurationManager.AppSettings["giis_item_id_oid"], "GIIS Item Identifiers", null, vaccEvent.Dose.ScheduledVaccination.Name, null)
                                )
                            },
                            new EN(null, new ENXP[] { new ENXP(vaccEvent.Dose.ScheduledVaccination.Item.Name) }),
                            vaccEvent.VaccineLot
                        )
                    }
                );
            }

            // Statment
            SubstanceAdministration statement = new SubstanceAdministration()
            {
                TemplateId = new LIST<II>() { new II("1.3.6.1.4.1.19376.1.5.3.1.4.12"), new II("2.16.840.1.113883.10.20.1.24") },
                Code = new CD<string>("IMMUNIZ", "2.16.840.1.113883.5.4", "ActCode", null, "Immunization", null),
                MoodCode = x_DocumentSubstanceMood.Eventoccurrence,
                Id = SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_vaccination_id_oid"], this.FormatIdentifier(documentId, vaccEvent.Id))),
                StatusCode = ActStatus.Completed,
                EffectiveTime = new List<GTS>() { 
                    new GTS(new IVL<TS>(vaccEvent.VaccinationDate))
                },
                RouteCode = new CE<string>() { NullFlavor = NullFlavor.Unknown },
                DoseQuantity = new IVL<PQ>(new PQ(1, null)),
                Consumable = consumable
            };


            // Replacement?
            if(previousDocId.HasValue)
            {
                statement.Reference.Add(new Reference(
                    x_ActRelationshipExternalReference.RPLC,
                    new ExternalAct(
                        "SBADM", SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_vaccination_id_oid"], this.FormatIdentifier(previousDocId.Value, vaccEvent.Id))), null, null
                    )
                ));
            }

            var age = (((TS)vaccEvent.VaccinationDate) - ((TS)vaccEvent.Child.Birthdate)).Convert("mo");
            age.Precision = 1;

            // Age at dose
            statement.EntryRelationship.Add(new EntryRelationship(x_ActRelationshipEntryRelationship.SUBJ, true, null, null, null, null,
                new Observation()
                {
                    TemplateId = LIST<II>.CreateList(new II("2.16.840.1.113883.10.20.1.38")),
                    Code = new CD<string>("397659008", "2.16.840.1.113883.6.96", "SNOMED CT", null, "Age", null),
                    Value = age
                }));


            // Author of the observation
            statement.Author.Add(
                new Author(
                    ContextControl.AdditiveNonpropagating,
                    vaccEvent.ModifiedOn,
                    new AssignedAuthor(
                        SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_user_id_oid"], vaccEvent.ModifiedBy.ToString()))
                    )
                )
            );

            // Set the dose number if exists
            if(vaccEvent.Dose != null)
                statement.EntryRelationship.Add(new EntryRelationship(x_ActRelationshipEntryRelationship.SUBJ, true, new Observation(x_ActMoodDocumentObservation.Eventoccurrence)
                {
                    TemplateId = LIST<II>.CreateList(new II("2.16.840.1.113883.10.20.1.46")),
                    Code = new CD<string>("30973-2", "2.16.840.1.113883.6.1", "LOINC", null, "Dose number", null),
                    StatusCode = ActStatus.Completed,
                    Value = new INT(vaccEvent.Dose.DoseNumber)
                }));


            // Negation indicator with the non-vaccination reason as a reason code
            String nvacReasonText = "Unknown";
            if (vaccEvent.NonvaccinationReasonId > 0)
            {
                var nvacReason = GIIS.DataLayer.NonvaccinationReason.GetNonvaccinationReasonById(vaccEvent.NonvaccinationReasonId);
                nvacReasonText = nvacReason.Name;
            }

            statement.NegationInd = !vaccEvent.VaccinationStatus;
            
            // intent?????? This is so odd
            if (vaccEvent.VaccinationDate > DateTime.Now)
                statement.MoodCode = new CS<x_DocumentSubstanceMood>(x_DocumentSubstanceMood.Intent);

            var noteId = String.Format("txt{0}", Guid.NewGuid().ToString().Substring(0, 6));
            if(!String.IsNullOrEmpty(vaccEvent.Notes))
            {
                statement.EntryRelationship.Add(
                    new EntryRelationship(x_ActRelationshipEntryRelationship.SUBJ, true, null, null, null, null, 
                        this.CreateCommentEntry(statement.Author, null, noteId))
                    );
            }
            
            statement.Text = new ED(String.Format("<tr><td>{0}</td><td>{9}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{8}</td><td ID=\"{5}\">{6}{7}</td></tr>", 
                vaccEvent.VaccinationDate.ToString("yyyy-MMM-dd"), 
                vaccEvent.Dose.ScheduledVaccination.Name, 
                vaccEvent.Dose.DoseNumber,
                vaccEvent.VaccineLot,
                age,
                noteId,
                vaccEvent.NonvaccinationReasonId > 0 ? String.Format("NOT VACCINATED - {0}" , nvacReasonText) : null,
                vaccEvent.Notes,
                vaccEvent.HealthFacility.Name, 
                vaccEvent.VaccinationStatus ? "Given" : "Not Given"));
            statement.Text.Representation = MARC.Everest.DataTypes.Interfaces.EncapsulatedDataRepresentation.XML;

            return new Entry(x_ActRelationshipEntry.HasComponent, true, statement);
            //return null;
        }

        /// <summary>
        /// Format an identifier
        /// </summary>
        private string FormatIdentifier(decimal docId, int recordId)
        {
            return String.Format("{0}-{1}", recordId, docId);
        }

        /// <summary>
        /// Create comment entry
        /// </summary>
        private Observation CreateCommentEntry(List<Author> authors, String commentText, String commentRef)
        {
            return new Observation()
                    {
                        TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.4.2")),
                        Code = new CD<string>("48767-8", "2.16.840.1.113883.6.1", "LOINC", null, "Annotation Comment", null),
                        Text = commentText ?? "#" + commentRef,
                        Author = authors
                    };
        }

        /// <summary>
        /// Create an allergy/intolerance AEFI
        /// </summary>
        public Entry CreateProblemStatement(GIIS.DataLayer.VaccinationAppointment appointment, decimal documentId, decimal? previousDocId)
        {
            Trace.TraceInformation("{0} : Create Problem Statement for AEFI {1}", this.m_context, appointment.Id);
            String noteId = String.Format("txt{0}", Guid.NewGuid().ToString().Substring(0, 5));
            
            CE<String> agent = new CE<string>();
            var agents = GIIS.DataLayer.VaccinationEvent.GetVaccinationEventByAppointmentId(appointment.Id).Where(o=>o.IsActive && o.VaccinationStatus).ToList();
            if (agents.Count == 1)
            {
                var sect = this.CreateIZEntry(agents[0], documentId, previousDocId);
                agent = sect.GetClinicalStatementIfSubstanceAdministration().Consumable.ManufacturedProduct.GetManufacturedDrugOrOtherMaterialIfManufacturedMaterial().Code;
            }
            else
                agent.NullFlavor = NullFlavor.Unknown;
            if (agents.Count == 0)
                agents.Add(new GIIS.DataLayer.VaccinationEvent() { VaccinationDate = appointment.ModifiedOn });

            var retVal = new Act(x_ActClassDocumentEntryAct.Act, x_DocumentActMood.Eventoccurrence)
            {
                TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.4.5.1", "1.3.6.1.4.1.19376.1.5.3.1.4.5.3")),
                Id = SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_appointment_id_oid"], this.FormatIdentifier(documentId, appointment.Id))),
                Code = new CD<string>() { NullFlavor = NullFlavor.NotApplicable },
                Author = new List<Author>() {
                    new Author(ContextControl.OverridingNonpropagating, appointment.ModifiedOn, new AssignedAuthor(SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_user_id_oid"], appointment.ModifiedBy.ToString()))))
                },
                StatusCode = appointment.IsActive ? ActStatus.Active : ActStatus.Completed,
                EffectiveTime = new IVL<TS>(agents[0].VaccinationDate, null),
                EntryRelationship = new List<EntryRelationship>() {
                    new EntryRelationship(x_ActRelationshipEntryRelationship.SUBJ, null, 
                        new Observation() {
                            TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.4.6"), new II("1.3.6.1.4.1.19376.1.5.3.1.4.5")),
                            Id = SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_appointment_id_oid"] + ".1", this.FormatIdentifier(documentId, appointment.Id))),
                            Code = new CD<string>("DNAINT", "2.16.840.1.113883.5.4", "ObservationIntoleranceType", null, "Drug Non-Allergy Intolerance", "AEFI"),
                            Value = MARC.Everest.Connectors.Util.Convert<CD<String>>(agent),
                            Participant = new List<Participant2>() {
                                new Participant2(ParticipationType.Consumable, ContextControl.OverridingNonpropagating) {
                                    ParticipantRole = new ParticipantRole("MANU") {
                                        PlayingEntityChoice = new PlayingEntity(EntityClassRoot.ManufacturedMaterial) {
                                            Code = agent
                                        }
                                    }
                                }
                            },
                            Text = String.Format("<tr><td>{0}</td><td>{4}</td><td>{1}</td><td ID=\"{2}\">{3}</td></tr>",
                                agents[0].VaccinationDate.ToString("yyyy-MMM-dd"), 
                                appointment.ScheduledFacility.Name,
                                noteId,
                                appointment.Notes,
                                agent.IsNull ? "Unknown" : (String)agent.DisplayName
                            )
                        }
                    )
                }
            };

            // Replacement?
            if (previousDocId.HasValue)
            {
                retVal.Reference.Add(new Reference(
                    x_ActRelationshipExternalReference.RPLC,
                    new ExternalAct(
                        "SBADM", SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_vaccination_id_oid"], this.FormatIdentifier(previousDocId.Value, appointment.Id))), null, null
                    )
                ));
            }

            // Comment entry
            retVal.EntryRelationship.Add(new EntryRelationship(x_ActRelationshipEntryRelationship.SUBJ, true, null, null, null, null, this.CreateCommentEntry(retVal.Author, null, noteId)));
            return new Entry(x_ActRelationshipEntry.HasComponent, true, retVal);
        }


        /// <summary>
        /// Create Vital signs organizer
        /// </summary>
        public Entry CreateVitalSignsOrganizer(GIIS.DataLayer.ChildWeight weight, decimal documentId, decimal? previousDocId)
        {
            Trace.TraceInformation("{0} : Creating Entry for Weight {1}", this.m_context.JobId, weight.Id);
            var retVal = new Organizer(x_ActClassDocumentEntryOrganizer.BATTERY)
            {
                TemplateId = LIST<II>.CreateList(new II("1.3.6.1.4.1.19376.1.5.3.1.4.13.1")),
                Code = new CD<string>("46680005", "2.16.840.1.113883.6.96", "SNOMED CT", null, "Vital Signs", null),
                StatusCode = ActStatus.Completed,
                Author = new List<Author>()
                {
                    new Author(
                        ContextControl.AdditiveNonpropagating, 
                        weight.Date, 
                        new AssignedAuthor(SET<II>.CreateSET(new II(ConfigurationManager.AppSettings["giis_user_id_oid"], weight.ModifiedBy.ToString())))
                    ),
                },
                EffectiveTime = new IVL<TS>(weight.Date),
                Component = new List<Component4>()
                {
                    new Component4(ActRelationshipHasComponent.HasComponent, true, CreateSimpleObservation(
                        new II(ConfigurationManager.AppSettings["giis_weight_id_oid"], this.FormatIdentifier(documentId, weight.Id)), 
                            weight.Date, 
                            new CD<string>("3141-9", "2.16.840.1.113883.6.1", "LOINC", null, "Body weight measured", null), 
                            new PQ((decimal)weight.Weight, "kg"),
                            previousDocId.HasValue ? new II(ConfigurationManager.AppSettings["giis_weight_id_oid"], this.FormatIdentifier(previousDocId.Value, weight.Id)) : null
                        ))
                }
            };
            foreach (var comp in retVal.Component)
                comp.GetClinicalStatementIfObservation().TemplateId.Add(new II("1.3.6.1.4.1.19376.1.5.3.1.4.13.2"));

            return new Entry(x_ActRelationshipEntry.DRIV, true, retVal);
        }
        #endregion 

        /// <summary>
        /// Perform a PnR for an IC
        /// </summary>
        public void ProvideAndRegister(GIIS.DataLayer.Child patient, String ecid, List<GIIS.DataLayer.User> authors, CV<String> classCode, DateTime startTime, DateTime stopTime, String formatCode, ClinicalDocument document)
        {

            String sourcePatientId = string.Format("{0}^^^&{1}&ISO", patient.Id, ConfigurationManager.AppSettings["giis_child_id_oid"]),
                docUniqueId = String.Format("{0}.{1}", document.Id.Root, document.Id.Extension, 0);
            //var ecid = new SyncData().GetPatientEcid(patient.Id);

            //ProvideAndRegisterDocumentSetRequest
            var extrinsicObject = XdsUtil.CreateExtrinsicObject("text/xml",
                "Immunization History",
                XdsGuidType.XDSDocumentEntry,
                XdsUtil.CreateSlot("creationTime", DateTime.Now.ToString("yyyyMMdd")),
                XdsUtil.CreateSlot("languageCode", "en-us"),
                XdsUtil.CreateSlot("serviceStartTime", startTime.ToString("yyyyMMddHHmm")),
                XdsUtil.CreateSlot("serviceStopTime", stopTime.ToString("yyyyMMddHHmm")),
                XdsUtil.CreateSlot("sourcePatientId", sourcePatientId),
                XdsUtil.CreateSlot("sourcePatientInfo",
                    String.Format("PID-3|{0}", sourcePatientId),
                    String.Format("PID-5|{0}^{1}", patient.Lastname1, patient.Firstname1),
                    String.Format("PID-7|{0}", patient.Birthdate.ToString("yyyyMMdd")),
                    String.Format("PID-8|{0}", patient.Gender ? "M" : "F")
                )
            );


            // Create classifications
            List<ClassificationType> classification = new List<ClassificationType>() {
                XdsUtil.CreateClassification(extrinsicObject, XdsGuidType.XDSDocumentEntry_PracticeSettingCode, "Outpatient Care", "Outpatient Care", 
                    XdsUtil.CreateSlot("codingScheme", "Connect-a-thon practiceSettingCodes")
                ),
                XdsUtil.CreateClassification(extrinsicObject, XdsGuidType.XDSDocumentEntry_ClassCode, classCode.Code, classCode.DisplayName,
                    XdsUtil.CreateSlot("codingScheme", classCode.CodeSystem)
                ),
                XdsUtil.CreateClassification(extrinsicObject, XdsGuidType.XDSDocumentEntry_ConfidentialityCode, "1.3.6.1.4.1.21367.2006.7.101", "Normal",
                    XdsUtil.CreateSlot("codingScheme", "Connect-a-thon confidentialityCodes")
                ),
                // HACK: should be the IC format code
                XdsUtil.CreateClassification(extrinsicObject, XdsGuidType.XDSDocumentEntry_FormatCode, formatCode, formatCode, 
                    XdsUtil.CreateSlot("codingScheme", "1.3.6.1.4.1.19376.1.2.3")
                ),
                XdsUtil.CreateClassification(extrinsicObject, XdsGuidType.XDSDocumentEntry_TypeCode, classCode.Code, classCode.DisplayName,
                    XdsUtil.CreateSlot("codingScheme", classCode.CodeSystem)
                ),
                XdsUtil.CreateClassification(extrinsicObject, new XdsGuidType("f33fb8ac-18af-42cc-ae0e-ed0b0bdb91e1"), "PC", "Primary Care Clinic",
                    XdsUtil.CreateSlot("codingScheme", "2.16.840.1.113883.5.11")
                )
            },
            subClassification = new List<ClassificationType>() ;

            var package = XdsUtil.CreateRegistryPackage();

            // Add authors
            foreach (var author in authors)
            {
                string authorId = String.Format("{0}^{1}^{2}^^^^^^&{3}&ISO", author.Id, author.Lastname, author.Firstname, ConfigurationManager.AppSettings["giis_user_id_oid"]);
                classification.Add(XdsUtil.CreateClassification(extrinsicObject, XdsGuidType.XDSDocumentEntry_Author, String.Empty, null,
                    XdsUtil.CreateSlot("authorPerson", authorId),
                    XdsUtil.CreateSlot("authorInstitution", author.HealthFacility.Name),
                    XdsUtil.CreateSlot("authorRole", "Attending")
                ));
                subClassification.Add(XdsUtil.CreateClassification(package, XdsGuidType.XDSSubmissionSet_Author, String.Empty, null,
                    XdsUtil.CreateSlot("authorPerson", authorId),
                    XdsUtil.CreateSlot("authorInstitution", author.HealthFacility.Name),
                    XdsUtil.CreateSlot("authorRole", "Attending")
                ));
            }
            extrinsicObject.Classification = classification.ToArray();
            
            // External identifiers
            extrinsicObject.ExternalIdentifier = new ExternalIdentifierType[]
            {
                XdsUtil.CreateExternalIdentifier(extrinsicObject, XdsGuidType.XDSDocumentEntry_PatientId, String.Format("{0}^^^&{1}&ISO", ecid, ConfigurationManager.AppSettings["ecid"])),
                XdsUtil.CreateExternalIdentifier(extrinsicObject, XdsGuidType.XDSDocumentEntry_UniqueId, docUniqueId)
            };

            // Package
            package.Slot = new SlotType1[]
            {
                XdsUtil.CreateSlot("submissionTime", DateTime.Now.ToString("yyyyMMddHHmmss"))
            };
            package.Classification = new List<ClassificationType>(subClassification) {
                XdsUtil.CreateClassification(package, XdsGuidType.XDSSubmissionSet_ContentType, classCode.Code, classCode.DisplayName, XdsUtil.CreateSlot("codingScheme", classCode.CodeSystem))
            }.ToArray();
            package.ExternalIdentifier = new ExternalIdentifierType[] {
                XdsUtil.CreateExternalIdentifier(extrinsicObject, XdsGuidType.XDSSubmissionSet_PatientId, String.Format("{0}^^^&{1}&ISO", ecid, ConfigurationManager.AppSettings["ecid"])),
                XdsUtil.CreateExternalIdentifier(extrinsicObject, XdsGuidType.XDSSubmissionSet_UniqueId, String.Format("{0}.1", docUniqueId)),
                XdsUtil.CreateExternalIdentifier(extrinsicObject, XdsGuidType.XDSSubmissionSet_SourceId, String.Format("{0}.2", docUniqueId))
                
            };
            package.Name = new InternationalStringType() { LocalizedString = new LocalizedStringType[] { new LocalizedStringType() { value = document.Title } } };

            // Submit objects request
            var submitObjectRequest = XdsUtil.CreateSubmitObjectsRequest(extrinsicObject, package, XdsUtil.CreateNodeClassification(package, XdsGuidType.XDSSubmissionSet), XdsUtil.CreateAssociation(package, extrinsicObject, "Original", "urn:oasis:names:tc:ebxml-regrep:AssociationType:HasMember"));

            // The document appending
            //foreach(var doc in document.RelatedDocument)
            //{
            //    var association = XdsUtil.CreateAssociation(extrinsicObject, null, ", "urn:ihe:iti:2007:AssociationType:" + doc.TypeCode.Code.ToString());
            //    association.targetObject = String.Format("{0}.{1}", doc.ParentDocument.Id[0].Root, doc.ParentDocument.Id[0].Extension);
            //    association.objectType = "urn:oasis:names:tc:ebxml-regrep:ObjectType:RegistryObject:Association";
            //}

            // TODO: Fill out the document with content
            var documentData = new ProvideAndRegisterDocumentSetRequestTypeDocument();
            documentData.id = extrinsicObject.id;

            // Format
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlIts1Formatter fmtr = new XmlIts1Formatter() { ValidateConformance = false, Settings = SettingsType.DefaultUniprocessor })
                {
                    fmtr.GraphAides.Add(new ClinicalDocumentDatatypeFormatter());
                    using (XmlWriter xw = XmlWriter.Create(ms, new XmlWriterSettings() {  OmitXmlDeclaration = true, Indent = true }))
                        fmtr.Graph(xw, document);

                }
                ms.Flush();
                documentData.Value = ms.ToArray();
                Trace.TraceInformation("{0} : Source CDA: {1}", this.m_context.JobId, System.Text.Encoding.UTF8.GetString(documentData.Value));
            }

            // Create the request
            var xdsRequest = XdsUtil.CreateProvideAndRegisterRequest(submitObjectRequest, documentData);

            // Save 
            var id = Guid.NewGuid().ToString();
            using (var fs = new StringWriter())
            { 
                new XmlSerializer(typeof(ProvideAndRegisterDocumentSetRequestType)).Serialize(fs, xdsRequest);
                fs.Flush();
                Trace.TraceInformation("{0} : PnR Request: {1} ", this.m_context.JobId, fs.ToString());
            }

            var client = new MARC.IHE.Xds.Repository.DocumentRepository_PortTypeClient("shrRepository");
            var provideResponse = client.DocumentRepository_ProvideAndRegisterDocumentSetb(xdsRequest);

            using (var fs = new StringWriter())
            {
                new XmlSerializer(typeof(RegistryResponseType)).Serialize(fs, provideResponse);
                fs.Flush();
                Trace.TraceInformation("PnR Response: {0}", fs.ToString());
            }

            if (!provideResponse.status.Contains("Success"))
                throw new InvalidOperationException("Could not PnR");

        }

    }
}
