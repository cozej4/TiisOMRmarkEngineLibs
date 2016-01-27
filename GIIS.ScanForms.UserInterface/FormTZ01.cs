using GIIS.DataLayer;
using GIIS.DataLayer.Contract;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public class FormTZ01
    {

        private static List<Dose> s_refDoses;
        private static List<ScheduledVaccination> s_refVaccines;
        private static List<Item> s_refItems;
        private static List<ItemLot> s_refItemLots;

        /// <summary>
        /// Error string
        /// </summary>
        public String Err { get; set; }

        public void UploadData(OmrPageOutput page)
        {

            StatusDialog dlg = new StatusDialog();
            dlg.Show();

            try
            {
                int facilityId = Int32.Parse(page.Parameters[0]);
                int month = Int32.Parse(page.Parameters[1]);
                int year = Int32.Parse(page.Parameters[2]);
                this.Err = "";

                Trace.TraceInformation("Reporting for facility {0} of {1}-{2}", facilityId, month, year);

                RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
                User userInfo = null;
                try
                {
                    userInfo = restUtil.Get<User>("UserManagement.svc/GetUserInfo", new KeyValuePair<string, object>("username", restUtil.GetCurrentUserName));
                }
                catch
                {
                    userInfo = restUtil.Get<User>("UserManagement.svc/GetUserInfo", new KeyValuePair<string, object>("username", restUtil.GetCurrentUserName));
                }
                var placeInfo = restUtil.Get<Place[]>("PlaceManagement.svc/GetPlaceByHealthFacilityId", new KeyValuePair<string, object>("hf_id", facilityId));
                
                if (s_refDoses == null)
                    s_refDoses = restUtil.Get<List<GIIS.DataLayer.Dose>>("DoseManagement.svc/GetDoseList");
                if(s_refVaccines == null)
                    s_refVaccines = restUtil.Get<List<GIIS.DataLayer.ScheduledVaccination>>("ScheduledVaccinationManagement.svc/GetScheduledVaccinationList");
                if(s_refItems == null)
                    s_refItems = restUtil.Get<List<GIIS.DataLayer.Item>>("ItemManagement.svc/GetItemList");
                if(s_refItemLots == null)
                    s_refItemLots = restUtil.Get<List<GIIS.DataLayer.ItemLot>>("StockManagement.svc/GetItemLots");

                foreach (var patientRow in page.Details.OfType<OmrRowData>().Where(o=>!o.Id.Contains("-")))
                {
                    // Master patient row processing
                    string rowNum = patientRow.Id.Substring(patientRow.Id.Length - 1, 1);

                    if (patientRow.Details.Count == 0)
                        continue;

                    // Barcodes only at this level
                    OmrBarcodeData omrBarcode = patientRow.Details.OfType<OmrBarcodeData>().FirstOrDefault(o => o.Id == String.Format("{0}Barcode", patientRow.Id)),
                        omrSticker = patientRow.Details.OfType<OmrBarcodeData>().FirstOrDefault(o => o.Id == String.Format("{0}Sticker", patientRow.Id));
                    String barcodeId = String.Empty;

                    if (omrBarcode == null)
                    {
                        // Show a correction form ...
                        var barcodeField = page.Template.Fields.FirstOrDefault(o => o.Id == String.Format("{0}Barcode", patientRow.Id)) as OmrBarcodeField;
                        BarcodeCorrection bc = new BarcodeCorrection(page, barcodeField);
                        if (BarcodeUtil.HasData(page, barcodeField) && bc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            barcodeId = bc.BarcodeId;
                        else
                            throw new InvalidOperationException(String.Format("Could not read barcode on row {0}", rowNum));
                    }
                    else
                        barcodeId = omrBarcode.BarcodeData;

                    // Try to lookup the poor chap
                    Child childData = null;
                    if (barcodeId.StartsWith("T")) // TEMP ID AUTH
                    {

                        // Validate ... do we want to associate maybe?
                        if(omrSticker == null && page.Details.OfType<OmrRowData>().First(o=>o.Id == String.Format("{0}-a", patientRow.Id)).Details.OfType<OmrBubbleData>().Count() != 0 ||
                            omrSticker != null && omrSticker.BarcodeData.Length != 10)
                        {
                            var barcodeField = page.Template.Fields.Find(o => o.Id == String.Format("{0}Sticker", patientRow.Id)) as OmrBarcodeField;
                            BarcodeCorrection bc = new BarcodeCorrection(page, barcodeField);
                            if(BarcodeUtil.HasData(page, barcodeField) && bc.ShowDialog() == DialogResult.OK)
                            {
                                omrSticker = new OmrBarcodeData()
                                {
                                    BarcodeData = bc.BarcodeId,
                                };
                            }

                        }
                        var childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/GetChildById",
                            new KeyValuePair<string, object>("childId", barcodeId.Replace("T", ""))
                        );
                        if (childDataList == null)
                            throw new InvalidOperationException("Could not deserialize response");
                        else if (childDataList.Count == 0)
                        {
                            throw new InvalidOperationException("Child with barcode " + barcodeId + " not found!");
                        }
                        childData = childDataList[0].childEntity;
                    }
                    else
                    {
                        var childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/SearchByBarcode",
                                            new KeyValuePair<string, object>("barcodeId", barcodeId));
                        if (childDataList == null)
                            throw new InvalidOperationException("Could not deserialize response");
                        else if (childDataList.Count == 0)
                        {
                            throw new InvalidOperationException("Child with barcode " + barcodeId + " not found!");
                        }
                        childData = childDataList[0].childEntity;
                    }

                    // Are we associating the poor chap?
                    if(omrSticker != null)
                    {
                        string permCode = omrSticker.BarcodeData;
                        // Now to update the child
                        Trace.TraceInformation("Will associated child id {0} with barcode {1}", childData.Id, permCode);
                        childData.BarcodeId = permCode;

                        // Send the update
                        restUtil.Get<RestReturn>("ChildManagement.svc/UpdateChild",
                            new KeyValuePair<string, object>("barcode", childData.BarcodeId),
                            new KeyValuePair<string, object>("firstname1", childData.Firstname1),
                            new KeyValuePair<string, object>("lastname1", childData.Lastname1),
                            new KeyValuePair<string, object>("birthdate", childData.Birthdate.ToString("yyyy-MM-dd")),
                            new KeyValuePair<string, object>("gender", childData.Gender),
                            new KeyValuePair<string, object>("healthFacilityId", childData.HealthcenterId),
                            new KeyValuePair<string, object>("birthplaceId", childData.BirthplaceId),
                            new KeyValuePair<string, object>("domicileId", childData.DomicileId),
                            new KeyValuePair<string, object>("statusId", childData.StatusId),
                            new KeyValuePair<string, object>("address", childData.Address),
                            new KeyValuePair<string, object>("phone", childData.Phone),
                            new KeyValuePair<string, object>("motherFirstname", childData.MotherFirstname),
                            new KeyValuePair<string, object>("motherLastname", childData.MotherLastname),
                            new KeyValuePair<string, object>("notes", "Updated by form scanning application"),
                            new KeyValuePair<string, object>("userId", 1),
                            new KeyValuePair<string, object>("childId", childData.Id)
                        );

                    }

                    // Get appointments and vaccinations for child
                    VaccinationAppointment[] appts = restUtil.Get<VaccinationAppointment[]>("VaccinationAppointmentManagement.svc/GetVaccinationAppointmentsByChildId", new KeyValuePair<string, object>("childId", childData.Id));
                    VaccinationEvent[] vaccinationEvent = restUtil.Get<VaccinationEvent[]>("VaccinationEvent.svc/GetVaccinationEventListByChildId", new KeyValuePair<string, object>("childId", childData.Id));

                    // iterate over the sub-rows
                    foreach (var aptRow in page.Details.OfType<OmrRowData>().Where(o=>o.Id.StartsWith(String.Format("{0}-", patientRow.Id))))
                    {
                        OmrBarcodeData omrAptId = aptRow.Details.OfType<OmrBarcodeData>().First();
                        OmrBubbleData omrDay10 = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "day10"),
                            omrDay = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "day"),
                            omrOutreach = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "outreach"),
                            omrVaccine = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "vaccine");

                        // Skip
                        if (omrVaccine == null)
                            continue;

                        if(omrDay10 == null || omrDay == null)
                        {
                            Err += String.Format("Row {0} has no date selected; ", aptRow);
                            continue;
                        }

                        // First, get the appointment data
                        VaccinationAppointment appointment = null;

                        if (omrAptId == null)
                        {
                            var barcodeField = page.Template.Fields.FirstOrDefault(o => o.AnswerRowGroup == aptRow.Id) as OmrBarcodeField;
                            // Show a correction form ...
                            BarcodeCorrection bc = new BarcodeCorrection(page, barcodeField);
                            if (BarcodeUtil.HasData(page, barcodeField) && bc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                appointment = appts.FirstOrDefault(o => o.ScheduledDate.Date.Equals(DateTime.ParseExact(bc.BarcodeId, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            appointment = appts.FirstOrDefault(o => o.Id.ToString() == omrAptId.BarcodeData);
                        }

                        // Validate we got an appointment
                        if (appointment == null)
                            throw new InvalidOperationException(String.Format("Could not find appointment data for update on row {0}", rowNum));

                        // All vaccines given
                        if (omrVaccine.Value == "all")
                        {
                            foreach (var vacc in vaccinationEvent.Where(o => o.AppointmentId == appointment.Id))
                            {
                                vacc.VaccinationStatus = true;
                                vacc.VaccinationDate = new DateTime(appointment.ScheduledDate.Year, appointment.ScheduledDate.Month, (int)(omrDay.ValueAsFloat + omrDay10.ValueAsFloat));
                                if(vacc.VaccinationDate > DateTime.Now)
                                    vacc.VaccinationDate = new DateTime(year, month, (int)(omrDay.ValueAsFloat + omrDay10.ValueAsFloat));
                                this.UpdateVaccination(vacc);

                                if (omrOutreach != null)
                                {
                                    restUtil.Get<RestReturn>("VaccinationAppointmentManagement.svc/UpdateVaccinationApp",
                                        new KeyValuePair<String, Object>("outreach", true),
                                        new KeyValuePair<String, Object>("userId", userInfo.Id),
                                        new KeyValuePair<String, Object>("barcode", barcodeId),
                                        new KeyValuePair<String, Object>("doseId", vacc.DoseId)
                                        );
                                }

                            }
                        }
                        else
                        {
                            VaccineCorrection vc = new VaccineCorrection(page , page.Template.Fields.FirstOrDefault(o => o.AnswerRowGroup == aptRow.Id) as OmrBarcodeField, appointment, vaccinationEvent, s_refDoses);
                            if (vc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                foreach(var vacc in vc.Vaccines)
                                {
                                    vacc.Event.VaccinationStatus = true;
                                    vacc.Event.VaccinationDate = new DateTime(appointment.ScheduledDate.Year, appointment.ScheduledDate.Month, (int)(omrDay.ValueAsFloat + omrDay10.ValueAsFloat));
                                    if (vacc.Event.VaccinationDate > DateTime.Now)
                                        vacc.Event.VaccinationDate = new DateTime(year, month, (int)(omrDay.ValueAsFloat + omrDay10.ValueAsFloat));
                                    this.UpdateVaccination(vacc.Event);

                                    if (omrOutreach != null)
                                    {
                                        restUtil.Get<RestReturn>("VaccinationAppointmentManagement.svc/UpdateVaccinationApp",
                                            new KeyValuePair<String, Object>("outreach", true),
                                            new KeyValuePair<String, Object>("userId", userInfo.Id),
                                            new KeyValuePair<String, Object>("barcode", barcodeId),
                                            new KeyValuePair<String, Object>("doseId", vacc.Event.DoseId)
                                            );
                                    }

                                }
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Trace.TraceError("Error:{0}",e);
                throw;
            }
            finally
            {
                dlg.Close();
            }

        }


        /// <summary>
        /// Update vaccination
        /// </summary>
        /// <param name="evt"></param>
        private void UpdateVaccination(VaccinationEvent evt)
        {
            RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));

            restUtil.Get<RestReturn>("VaccinationEvent.svc/UpdateVaccinationEventById",
                new KeyValuePair<string, object>("vaccinationEventId", evt.Id),
                new KeyValuePair<String, Object>("vaccinationDate", evt.ScheduledDate.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<String, Object>("notes", "From form scanner"),
                new KeyValuePair<String, Object>("vaccinationStatus", true));

        }

        public class ChildEntity
        {
            public GIIS.DataLayer.Child childEntity { get; set; }
        }

    }
}
