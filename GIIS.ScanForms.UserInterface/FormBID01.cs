using OmrMarkEngine.Output;
using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public class FormBID01
    {
        public String Err { get; set; }

        public void UploadData(OmrPageOutput page)
        {
            if (!Connectivity.CheckForInternetConnection())
            {
                MessageBox.Show(
                    "Username is empty, This is usually caused by lack of internet connectivity. Please try again later");
                Exception e = new Exception("No internet connection");
                Trace.TraceError("Error:{0}", e);
                throw e;
            }
            try
            {
                this.Err = "";

                var restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
                Trace.TraceInformation("Need to sync {0} children with TIIS at {1}", page.Details.Count, ConfigurationManager.AppSettings["GIIS_URL"]);

                var refDoses = restUtil.Get<List<GIIS.DataLayer.Dose>>("DoseManagement.svc/GetDoseList");
                var refVaccines = restUtil.Get<List<GIIS.DataLayer.ScheduledVaccination>>("ScheduledVaccinationManagement.svc/GetScheduledVaccinationList");
                var refItems = restUtil.Get<List<GIIS.DataLayer.Item>>("ItemManagement.svc/GetItemList");
                var refItemLots = restUtil.Get<List<GIIS.DataLayer.ItemLot>>("StockManagement.svc/GetItemLots");

                foreach (var dtl in page.Details.OfType<OmrRowData>())
                {
                    // Get the barcode information for this child
                    var barcodes = dtl.Details.OfType<OmrBarcodeData>();
                    if (barcodes.Count() > 1)
                        throw new InvalidOperationException("Only one barcode per row is supported by this template");
                    else if (barcodes.Count() == 0)
                        throw new InvalidOperationException("No barcode found on row!");

                    // Child code
                    var childCode = barcodes.FirstOrDefault();

                    // Output sync
                    Trace.TraceInformation("Syncing child {0} to GIIS - Step 1 - Resolve Barcode ID", childCode.BarcodeData);

                    GIIS.DataLayer.Child childData = null;

                    // Get the child by barcode id
                    if (childCode.BarcodeData.StartsWith("T"))
                    {
                        var childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/GetChildById",
                            new KeyValuePair<string, object>("childId", childCode.BarcodeData.Replace("T", ""))
                        );
                        if (childDataList == null)
                            throw new InvalidOperationException("Could not deserialize response");
                        else if (childDataList.Count == 0)
                        {
                            throw new InvalidOperationException("Child with barcode " + childCode.BarcodeData + " not found!");
                        }
                        childData = childDataList[0].childEntity;
                    }
                    else
                    {
                        var childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/SearchByBarcode",
                                            new KeyValuePair<string, object>("barcodeId", childCode.BarcodeData));
                        if (childDataList == null)
                            throw new InvalidOperationException("Could not deserialize response");
                        else if (childDataList.Count == 0)
                        {
                            throw new InvalidOperationException("Child with barcode " + childCode.BarcodeData + " not found!");
                        }
                        childData = childDataList[0].childEntity;
                    }
                    int childId = childData.Id;

                    // Is this child a non-vaccination?
                    var bubbleData = dtl.Details.OfType<OmrBubbleData>();

                    // Load questions
                    OmrBubbleData weight10 = bubbleData.FirstOrDefault(o => o.Key == "weight10"),
                        weight1 = bubbleData.FirstOrDefault(o => o.Key == "weight"),
                        weight10ths = bubbleData.FirstOrDefault(o => o.Key == "weightD"),
                        date10 = bubbleData.FirstOrDefault(o => o.Key == "date10"),
                        date = bubbleData.FirstOrDefault(o => o.Key == "date"),
                        ebfr = bubbleData.FirstOrDefault(o => o.Key == "ebfr"),
                        vaccines = bubbleData.FirstOrDefault(o => o.Key == "vaccines"),
                        outreach = bubbleData.FirstOrDefault(o => o.Key == "outreach");

                    // No vaccinations?
                    if (vaccines == null)
                    {
                        Trace.TraceInformation("No vaccines detected!");
                        continue;
                    }

                    // Weight
                    float weight = 0.0f;
                    if (weight10 != null)
                        weight = weight + weight10.ValueAsFloat;
                    if (weight1 != null)
                        weight = weight + weight1.ValueAsFloat;
                    if (weight10ths != null)
                        weight = weight + (weight10ths.ValueAsFloat * 0.1f);

                    // Date
                    int day = 0;
                    if (date10 != null)
                        day += (int)date10.ValueAsFloat;
                    if (date != null)
                        day += (int)date.ValueAsFloat;
					
					int txYear = DateTime.Now.Year;
					int txMonth = Int32.Parse(page.Parameters[1]);
					
					if(txMonth > DateTime.Now.Month) // We got a form whose month (ex: 11) is greater than the current month (ex: 10), since we cannot scan in the future, it must be last year
						txYear--;
                    Trace.TraceInformation("Using Date {0}-{1}-{2}", day, txMonth, txYear);
                    DateTime dateOfTx = new DateTime(txYear, txMonth, day);

                    // Register child weight
                    restUtil.Get<object>("ChildManagement.svc/RegisterChildWeight",
                        new KeyValuePair<string, object>("childId", childId),
                        new KeyValuePair<string, object>("date", dateOfTx.ToString("yyyy-MM-dd HH:mm:ss")),
                        new KeyValuePair<string, object>("weight", weight),
                        new KeyValuePair<string, object>("modifiedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new KeyValuePair<string, object>("modifiedBy", 1)
                    );

                    // Register child vaccinations
                    if (vaccines == null || vaccines.Value == "none")
                    {

                        Trace.TraceInformation(string.Format("Child {0}, {1} was not vaccinated", childData.Lastname1, childData.Firstname1));
                        if (Err == null)
                            Err = String.Empty;
                        Err += String.Format("Child {0}, {1} requires manual reconciliation - Not all vaccinations given; ", childData.Lastname1, childData.Firstname1);
                        continue;
                    }
                    else
                    {
                        // Get child's vaccination card
                        var izCard = restUtil.Get<List<GIIS.DataLayer.VaccinationEvent>>("VaccinationEvent.svc/GetImmunizationCard",
                                              new KeyValuePair<string, object>("childId", childId),
                                              new KeyValuePair<string, object>("scheduledDate", dateOfTx.ToString("yyyy-MM-dd HH:MM:ss"))
                                          );
                        foreach (var vacc in izCard.Where(o => o.NonvaccinationReasonId == 0 && !o.VaccinationStatus && o.IsActive && (o.ScheduledDate.Month == txMonth || o.ScheduledDate.AddMonths(1).Month == txMonth) && o.ScheduledDate.Year == txYear))
                        {
                            var dose = refDoses.Find(o => o.Id == vacc.DoseId);
                            var vaccine = refVaccines.Find(o => o.Id == dose.ScheduledVaccinationId);
                            var item = refItems.Find(o => o.Id == vaccine.ItemId);
                            var itemLot = refItemLots.FindAll(o => o.ItemId == item.Id);
                            var facilityBalance = restUtil.Get<List<GIIS.DataLayer.HealthFacilityBalance>>("StockManagement.svc/GetCurrentStockByLot",
                                new KeyValuePair<String, Object>("hfId", page.Parameters[0]));
                            var balance = facilityBalance.FindAll(o => itemLot.Exists(il => il.Gtin == o.Gtin && il.LotNumber == o.LotNumber)).OrderByDescending(p => p.Balance).FirstOrDefault();
                            
                            if(balance == null)
                            {
                              Trace.TraceWarning("Facility has no balance for {0}! Can't determine vaccine lot / gtin", dose.Fullname);
                              Err += String.Format("Child {0} cannot receive immunization {1} because facility has no {1} in stock. Please update stock and retry", childId, dose.Fullname);
                            } 
                            
                            Trace.TraceInformation("Giving child {0} immunization {1} (stock GTIN: {2} LN {3})", childId, dose.Fullname, balance.Gtin, balance.LotNumber);
                            restUtil.Get<RestReturn>("VaccinationEvent.svc/UpdateVaccinationEvent",
                                new KeyValuePair<String, Object>("vaccineLotId", itemLot.Find(o => o.Gtin == balance.Gtin && o.LotNumber == balance.LotNumber).Id),
                                new KeyValuePair<String, Object>("healthFacilityId", page.Parameters[0]),
                                new KeyValuePair<String, Object>("vaccinationDate", dateOfTx.ToString("yyyy-MM-dd HH:mm:ss")),
                                new KeyValuePair<String, Object>("notes", "From Paper Register"),
                                new KeyValuePair<String, Object>("vaccinationStatus", true),
                                new KeyValuePair<String, Object>("userId", 1),
                                new KeyValuePair<String, Object>("vaccinationEventId", vacc.Id),
                                new KeyValuePair<String, Object>("outreach", outreach != null)
                            );

                            if (outreach != null)
                            {
                                Trace.TraceInformation("Updating appointment {0} to be outreach", vacc.AppointmentId);
                                restUtil.Get<RestReturn>("VaccinationAppointmentManagement.svc/UpdateVaccinationAppointment",
                                        new KeyValuePair<String, Object>("childId", childId),
                                        new KeyValuePair<String, Object>("vaccinationAppointmentId", vacc.AppointmentId),
                                        new KeyValuePair<String, Object>("outreach", true),
                                        new KeyValuePair<String, Object>("userId", 1),
                                        new KeyValuePair<String, Object>("scheduledFacilityId", page.Parameters[0]),
                                        new KeyValuePair<String, Object>("notes", "From Paper Form")
                                );
                            }

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Err = e.Message;
            }
        }

        public class ChildEntity
        {

            public GIIS.DataLayer.Child childEntity { get; set; }
        }
    }
}
