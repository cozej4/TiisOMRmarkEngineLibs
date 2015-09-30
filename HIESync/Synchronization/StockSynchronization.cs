using MARC.Everest.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HIESync.Synchronization
{
    /// <summary>
    /// Stock synchronization class
    /// </summary>
    public class StockSynchronization : IDisposable
    {

        // Context of synchrnonization
        private SynchronizationContext m_context;

        /// <summary>
        /// Stock synchronization
        /// </summary>
        public StockSynchronization(SynchronizationContext context)
        {
            this.m_context = context;
        }

        /// <summary>
        /// Synchronize the stock counts
        /// </summary>
        public void SynchronizeStockCounts()
        {

            List<Partner> partners = new List<Partner>();
            List<String> partIds = new List<String>();
            // HF facilities
            foreach (var hf in GIIS.DataLayer.HealthFacility.GetHealthFacilityList().Where(o => o.VaccineStore).Take(200))
            {
                partners.Add(new Partner()
                {
                    Identifier = new PartnerIdentification() { Authority = "GS1", Value = hf.Code },
                    
                    ContactInformation = new ContactInformation[] { 
                            new ContactInformation()
                            {
                                Contact = hf.Contact,
                                EmailAddress = hf.Address,
                                ContactTypeIdentifier = hf.Type != null ? hf.Type.Code : null,
                            }
                        }
                });
                partIds.Add(hf.Code);
            }
            // Create the BMD stock transaction
            InventoryReportMessageType report = new InventoryReportMessageType();
            report.StandardBusinessDocumentHeader = new StandardBusinessDocumentHeader()
            {
                HeaderVersion = "1.0",
                DocumentIdentification = new DocumentIdentification()
                {
                    Standard = "GS1",
                    TypeVersion = "3.2",
                    InstanceIdentifier = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0).ToString("X"),
                    Type = "Inventory Type",
                    MultipleType = false,
                    MultipleTypeSpecified = true,
                    CreationDateAndTime = DateTime.Now
                },
                Sender = partners.ToArray(),
                Receiver = new Partner[] { 
                    new Partner() {
                        Identifier = new PartnerIdentification() {  Authority = "GS1", Value = ConfigurationManager.AppSettings["GS1_RECEIVER"] },
                        ContactInformation = new ContactInformation[] { 
                            new ContactInformation()
                            {
                                Contact = ConfigurationManager.AppSettings["GS1_RECEIVER_CONTACT"],
                                EmailAddress = "TODO: GET THIS",
                                ContactTypeIdentifier = "REGION"
                            }
                        }
                    }
                }
            };

            List<InventoryReportType> inventoryReports = new List<InventoryReportType>();

            // Inventory report
            foreach (var hf in GIIS.DataLayer.HealthFacility.GetHealthFacilityList().Where(o => o.VaccineStore))
            {

                // Inventory report
                var inventoryReport = new InventoryReportType();
                inventoryReport.creationDateTime = DateTime.Now;
                inventoryReport.inventoryReportTypeCode = InventoryReportTypeEnumerationType.INVENTORY_STATUS;
                inventoryReport.structureTypeCode = new StructureTypeCodeType() { Value = "LOCATION_BY_ITEM" };
                inventoryReport.documentActionCodeSpecified = true;
                inventoryReport.documentEffectiveDate = new DateOptionalTimeType()
                {
                    date = DateTime.Now.Date
                };
                inventoryReport.documentStatusCode = DocumentStatusEnumerationType.ORIGINAL;
                inventoryReport.inventoryReportIdentification = new EntityIdentificationType()
                {
                    entityIdentification = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0).ToString("X"),
                    contentOwner = new PartyIdentificationType()
                    {
                        gln = hf.Code
                    }
                };
                inventoryReport.inventoryReportToParty = new TransactionalPartyType()
                {
                    gln = ConfigurationManager.AppSettings["GS1_RECEIVER"]
                };
                inventoryReport.inventoryReportingParty = new TransactionalPartyType() { gln = hf.Code };
                inventoryReport.reportingPeriod = new DateTimeRangeType()
                {
                    beginDate = DateTime.Now.Date
                };

                // Item location report
                List<InventoryItemLocationInformationType> itemBalances = new List<InventoryItemLocationInformationType>();
                int lineItem = 0;
                foreach (var ilm in GIIS.DataLayer.HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode(hf.Code))
                    itemBalances.Add(new InventoryItemLocationInformationType()
                    {
                        inventoryLocation = new TransactionalPartyType()
                        {
                            gln = hf.Code,
                            organisationDetails = new OrganisationType()
                            {
                                organisationName = hf.Name
                            }
                        },
                        transactionalTradeItem = new TransactionalTradeItemType()
                        {
                            gtin = ilm.Gtin
                        },
                        inventoryStatusLineItem = new InventoryStatusLineItemType[] { 
                            new InventoryStatusLineItemType()
                            {
                                lineItemNumber = (lineItem++).ToString(),
                                inventoryStatusQuantitySpecification = new InventoryStatusQuantitySpecificationType[] {
                                
                                    new InventoryStatusQuantitySpecificationType() 
                                    {
                                        inventoryStatusType = new InventoryStatusCodeType() { Value = "DAMAGED" },
                                        quantityOfUnits = new QuantityType() { measurementUnitCode = "DOSES", Value = (decimal)ilm.Wasted }
                                    },
                                    new InventoryStatusQuantitySpecificationType() 
                                    {
                                        inventoryStatusType = new InventoryStatusCodeType() { Value = "ALLOCATED_FOR_ORDER" },
                                        quantityOfUnits = new QuantityType() { measurementUnitCode = "DOSES", Value = (decimal)ilm.Allocated }
                                    },
                                    new InventoryStatusQuantitySpecificationType() 
                                    {
                                        inventoryStatusType = new InventoryStatusCodeType() { Value = "ON_HAND" },
                                        quantityOfUnits = new QuantityType() { measurementUnitCode = "DOSES", Value = (decimal)ilm.StockCount }
                                    }

                                }
                            }
                        }
                    });

                inventoryReport.inventoryItemLocationInformation = itemBalances.ToArray();
                inventoryReports.Add(inventoryReport);

            }


            report.inventoryReport = inventoryReports.ToArray();

            // Serializer
            this.UploadReport(report);
        }
    
        /// <summary>
        /// Upload the report
        /// </summary>
        private void UploadReport(InventoryReportMessageType report)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                {
                    if (ConfigurationManager.AppSettings["remoteCertificateHash"] == null) return true;
                    var cert = ConfigurationManager.AppSettings["remoteCertificateHash"].Split(',');
                    bool isValid = false;
                    foreach(var crt in chain.ChainElements)
                        isValid |= crt.Certificate.Thumbprint.ToLower().Equals(cert[2].ToLower());
                    return isValid;
                };
                using (MemoryStream ms = new MemoryStream())
                {
                    XmlSerializer xsz = new XmlSerializer(typeof(InventoryReportMessageType));
                    xsz.Serialize(ms, report);
                    ms.Seek(0, SeekOrigin.Begin);

                    Uri reportUri = new Uri(String.Format("{0}/BMD1-{1}.xml", ConfigurationManager.AppSettings["GS1_FTP"], DateTime.Now.ToString("yyyy-MM-dd")));
                    Trace.TraceInformation("Uploading report to {0} ({1} bytes)", reportUri, ms.Length);
                    FtpWebRequest ftpRequest = WebRequest.Create(reportUri) as FtpWebRequest;
                    ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                    ftpRequest.UsePassive = false;
                    ftpRequest.UseBinary = false;
                    ftpRequest.KeepAlive = true;
                    ftpRequest.EnableSsl = false;
                    ftpRequest.ConnectionGroupName = "group";
                    ftpRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["GS1_FTP_UN"], ConfigurationManager.AppSettings["GS1_FTP_PWD"]);

                    byte[] buffer = new byte[10240];
                    int br = 1;
                    var requestStream = ftpRequest.GetRequestStream();

                    while (br > 0)
                    {
                        br = ms.Read(buffer, 0, 10240);
                        requestStream.Write(buffer, 0, br);
                        Trace.TraceInformation("Written {0} of {1} bytes", ms.Position, ms.Length);
                    }

                    requestStream.Flush();
                    requestStream.Close();
                }

                Trace.TraceInformation("Upload successful");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
        }

 
        public void Dispose()
        {
        }
    }
}
