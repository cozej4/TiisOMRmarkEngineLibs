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
using AtnaApi.Model;
using AtnaApi.Transport;
using MARC.HI.EHRS.CR.Notification.PixPdq;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Base.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HIESync.Util
{
    /// <summary>
    /// Audit utilities
    /// </summary>
    public static class AuditUtil
    {
        /// <summary>
        /// Create the audit
        /// </summary>
        private static AuditMessage CreateAudit(ActionType action, OutcomeIndicator outcome, EventIdentifierType eventId, CodeValue<String> eventType, IMessage message, String remoteEndpoint)
        {
            var retVal = new AuditMessage(
                DateTime.Now, action, outcome, eventId, eventType);

            Terser mTerser = new Terser(message);
            retVal.Actors = new List<AuditActorData>()
            {
                new AuditActorData() { 
                    UserIdentifier = String.Format("{0}|{1}", mTerser.Get("/MSH-3"), mTerser.Get("/MSH-4")),
                    AlternativeUserId = Process.GetCurrentProcess().Id.ToString(),
                    ActorRoleCode = new List<CodeValue<string>>() { 
                        new CodeValue<String>("110153", "DCM") { DisplayName = "Source" }
                    },
                    NetworkAccessPointType = NetworkAccessPointType.MachineName,
                    NetworkAccessPointTypeSpecified = true,
                    NetworkAccessPointId = Dns.GetHostName()
                },
                new AuditActorData() {
                    UserIdentifier = String.Format("{0}|{1}", mTerser.Get("/MSH-5"), mTerser.Get("/MSH-6")),
                    ActorRoleCode = new List<CodeValue<string>>() { 
                        new CodeValue<String>("110152", "DCM") { DisplayName = "Desintation" }
                    },
                    NetworkAccessPointType = NetworkAccessPointType.MachineName,
                    NetworkAccessPointTypeSpecified = true,
                    NetworkAccessPointId = remoteEndpoint
                }
            };

            // Human?
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (Environment.UserInteractive)
                retVal.Actors.Add(new AuditActorData()
                {
                    UserIdentifier = identity.Name,
                    AlternativeUserId = String.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName),
                    ActorRoleCode = identity.Groups.Select(o => new CodeValue<String>(o.Translate(typeof(NTAccount)).Value, Environment.UserDomainName)).ToList()
                });

            // Audit source
            retVal.SourceIdentification = new List<AuditSourceIdentificationType>() {
                new AuditSourceIdentificationType() {
                    AuditEnterpriseSiteID = ConfigurationManager.AppSettings["giis_device_id_oid"],
                    AuditSourceID = String.Format("{0}\\{1}", Environment.UserDomainName, Environment.MachineName),
                    AuditSourceTypeCode = new List<CodeValue<AuditSourceType>>() {
                        new CodeValue<AuditSourceType>(AuditSourceType.EndUserInterface)
                    }
                }
            };
            
            return retVal;
        }

        /// <summary>
        /// Send the audit
        /// </summary>
        private static void SendAudit(AuditMessage message) {

            var endpoint = new Uri(ConfigurationManager.AppSettings["arrEndpoint"]);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(endpoint.Host), endpoint.Port);
            ITransporter transport = null;
            switch(endpoint.Scheme)
            {
                case "udp":
                    transport = new UdpSyslogTransport(ipep);
                    break;
                case "tcp":
                    transport = new TcpSyslogTransport(ipep);
                    break;
                case "stcp":
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

                    transport = new STcpSyslogTransport(ipep){
                        ClientCertificate = localCert,
                        ServerCertificate = remoteCert
                    };
                
                    break;
            }
            
            // Now now now... we send
            transport.SendMessage(message);
        }

        /// <summary>
        /// Send the PDQ audit
        /// </summary>
        public static void SendPDQAudit(NHapi.Model.V25.Message.QBP_Q21 request, NHapi.Model.V25.Message.RSP_K21 response)
        {
            OutcomeIndicator outcome = OutcomeIndicator.Success;
            if (!response.MSA.AcknowledgmentCode.Value.EndsWith("A"))
                outcome = OutcomeIndicator.EpicFail;
            var audit = CreateAudit(ActionType.Execute, outcome, EventIdentifierType.Query, new CodeValue<string>("ITI-21", "IHE Transactions") { DisplayName = "Patient Demographcis Query" }, request, ConfigurationManager.AppSettings["crEndpoint"]);

            audit.AuditableObjects = new List<AuditableObject>();

            // Results found
            if (response.QAK.QueryResponseStatus.Value == "OK")
                for (int i = 0; i < response.QUERY_RESPONSERepetitionsUsed;  i++)
                    audit.AuditableObjects.Add(
                        new AuditableObject()
                        {
                            Type = AuditableObjectType.Person,
                            TypeSpecified = true,
                            Role = AuditableObjectRole.Patient,
                            RoleSpecified = true,
                            LifecycleType = AuditableObjectLifecycle.Export,
                            IDTypeCode = new CodeValue<AuditableObjectIdType>(AuditableObjectIdType.PatientNumber),
                            ObjectId = string.Format("{0}^^^&{1}&ISO", response.GetQUERY_RESPONSE(i).PID.GetPatientIdentifierList(0).IDNumber.Value, response.GetQUERY_RESPONSE(i).PID.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value)
                        }
                    );

            // Query parameters
            audit.AuditableObjects.Add(new AuditableObject()
            {
                Type = AuditableObjectType.SystemObject,
                TypeSpecified = true,
                Role = AuditableObjectRole.Query,
                RoleSpecified = true,
                ObjectId = request.QPD.QueryTag.Value,
                IDTypeCode = new CodeValue<AuditableObjectIdType>(AuditableObjectIdType.ITI21),
                ObjectSpecChoice = ObjectDataChoiceType.ParticipantObjectQuery,
                ObjectSpec = Convert.ToBase64String(Encoding.UTF8.GetBytes(new PipeParser().Encode(request))),
                ObjectDetail = new List<ObjectDetailType>() {
                        new ObjectDetailType() {
                            Type = "MSH-10",
                            Value = System.Text.Encoding.UTF8.GetBytes(request.MSH.MessageControlID.Value)
                        }
                    }
            });

            SendAudit(audit);

        }

        /// <summary>
        /// Send a pix audit
        /// </summary>
        public static void SendPIXAudit(NHapi.Model.V25.Message.ADT_A01 request, NHapi.Model.V231.Message.ACK response)
        {
            ActionType act = ActionType.Create;
            OutcomeIndicator outcome = OutcomeIndicator.Success;
            if(request.MSH.MessageType.TriggerEvent.Value == "A08")
                act = ActionType.Update;
            if (!response.MSA.AcknowledgementCode.Value.EndsWith("A"))
                outcome = OutcomeIndicator.EpicFail;

            var audit = CreateAudit(act, outcome, EventIdentifierType.PatientRecord, new CodeValue<string>("ITI-8","IHE Transactions") { DisplayName = "Patient Identity Feed" }, request, ConfigurationManager.AppSettings["crEndpoint"]);
            audit.AuditableObjects = new List<AuditableObject>() {
                new AuditableObject() { 
                    Type = AuditableObjectType.Person,
                    TypeSpecified = true,
                    Role = AuditableObjectRole.Patient,
                    RoleSpecified = true,
                    LifecycleType = AuditableObjectLifecycle.Export,
                    IDTypeCode = new CodeValue<AuditableObjectIdType>(AuditableObjectIdType.PatientNumber),
                    ObjectId = string.Format("{0}^^^&{1}&ISO", request.PID.GetPatientIdentifierList(0).IDNumber.Value, request.PID.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value),
                    ObjectDetail = new List<ObjectDetailType>() {
                        new ObjectDetailType() {
                            Type = "MSH-10",
                            Value = System.Text.Encoding.UTF8.GetBytes(request.MSH.MessageControlID.Value)
                        }
                    }
                }
            };

            SendAudit(audit);
        }
        
        /// <summary>
        /// Send a pix query audit
        /// </summary>
        public static void SendPIXAudit(NHapi.Model.V25.Message.QBP_Q21 request, NHapi.Model.V25.Message.RSP_K23 response)
        {
            OutcomeIndicator outcome = OutcomeIndicator.Success;
            if (!response.MSA.AcknowledgmentCode.Value.EndsWith("A"))
                outcome = OutcomeIndicator.EpicFail;
            var audit = CreateAudit(ActionType.Execute, outcome, EventIdentifierType.Query, new CodeValue<string>("ITI-9", "IHE Transactions") { DisplayName = "PIX Query" }, request, ConfigurationManager.AppSettings["crEndpoint"]);

            audit.AuditableObjects = new List<AuditableObject>();

            // Results found
            if(response.QAK.QueryResponseStatus.Value == "OK")
                audit.AuditableObjects.Add(
                    new AuditableObject() { 
                        Type = AuditableObjectType.Person,
                        TypeSpecified = true,
                        Role = AuditableObjectRole.Patient,
                        RoleSpecified = true,
                        LifecycleType = AuditableObjectLifecycle.Export,
                        IDTypeCode = new CodeValue<AuditableObjectIdType>(AuditableObjectIdType.PatientNumber),
                        ObjectId = string.Format("{0}^^^&{1}&ISO", response.QUERY_RESPONSE.PID.GetPatientIdentifierList(0).IDNumber.Value, response.QUERY_RESPONSE.PID.GetPatientIdentifierList(0).AssigningAuthority.UniversalID.Value)
                    }
                );

            // Query parameters
            audit.AuditableObjects.Add(new AuditableObject()
            {
                Type = AuditableObjectType.SystemObject,
                TypeSpecified = true,
                Role = AuditableObjectRole.Query,
                RoleSpecified = true,
                IDTypeCode = new CodeValue<AuditableObjectIdType>(AuditableObjectIdType.ITI9),
                ObjectSpecChoice = ObjectDataChoiceType.ParticipantObjectQuery,
                ObjectId = request.QPD.QueryTag.Value,
                ObjectSpec = Convert.ToBase64String(Encoding.UTF8.GetBytes(new PipeParser().Encode(request))),
                ObjectDetail = new List<ObjectDetailType>() {
                        new ObjectDetailType() {
                            Type = "MSH-10",
                            Value = System.Text.Encoding.UTF8.GetBytes(request.MSH.MessageControlID.Value)
                        }
                    }
            });

            SendAudit(audit);

        }
    }
}
