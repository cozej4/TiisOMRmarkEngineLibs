using GIIS.DataLayer;
using GIIS.DataLayer.Contract;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
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
    public class FormTZ02
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
                int facilityId = FacilitySelectionContext.FacilityId;

                // Non-remembered facility
                if (facilityId == 0)
                {
                    LocationSelectionBox location = new LocationSelectionBox();
                    if (location.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        throw new InvalidOperationException("Cannot upload data without selecting a facility");
                    if (location.Remember)
                        FacilitySelectionContext.FacilityId = location.FacilityId;
                    facilityId = location.FacilityId;
                }

                var monthBubble = page.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "month");
                if (monthBubble == null)
                    Err += "Must select month!; ";

                RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
                var userInfo = restUtil.Get<User>("UserManagement.svc/GetUserInfo", new KeyValuePair<string, object>("username", restUtil.GetCurrentUserName));
                var placeInfo = restUtil.Get<Place[]>("PlaceManagement.svc/GetPlaceByHealthFacilityId", new KeyValuePair<string, object>("hf_id", facilityId));
                if(s_refDoses == null)
                    s_refDoses = restUtil.Get<List<GIIS.DataLayer.Dose>>("DoseManagement.svc/GetDoseList");
                if(s_refVaccines == null)
                    s_refVaccines = restUtil.Get<List<GIIS.DataLayer.ScheduledVaccination>>("ScheduledVaccinationManagement.svc/GetScheduledVaccinationList");
                if(s_refItems == null)
                    s_refItems = restUtil.Get<List<GIIS.DataLayer.Item>>("ItemManagement.svc/GetItemList");
                if(s_refItemLots == null)
                    s_refItemLots = restUtil.Get<List<GIIS.DataLayer.ItemLot>>("StockManagement.svc/GetItemLots");

                foreach (var dtl in page.Details.OfType<OmrRowData>())
                {

                    string rowNum = dtl.Id.Substring(dtl.Id.Length - 1, 1);

                    if (dtl.Details.Count == 0)
                        continue;

                    // (string barcodeId, string firstname1, string firstname2, string lastname1, DateTime birthdate, bool gender,
                    // int healthFacilityId, int birthplaceId, int domicileId, string address, string phone, string motherFirstname,
                    // string motherLastname, string notes, int userId, DateTime modifiedOn)
                    OmrBarcodeData omrBarcode = dtl.Details.OfType<OmrBarcodeData>().FirstOrDefault();
                    OmrBubbleData omrDobDay = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "dobDay"),
                        omrDobDay10 = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "dobDay10"),
                        omrDobMonth = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "dobMonth"),
                        omrDobYear = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "dobYear"),
                        omrGender = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "gender"),
                        omrOutreach = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "Outreach"),
                        omrVaccDay10 = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "vaccDay10"),
                        omrVaccDay = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "vaccDay");
                    OmrBubbleData[] omrBcg = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "BCG").ToArray(),
                        omrOpv = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "OPV").ToArray(),
                        omrPenta = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "PENTA").ToArray(),
                        omrPcv = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "PCV").ToArray(),
                        omrRota = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "ROTA").ToArray(),
                        omrMr = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "MR").ToArray(),
                        omrVaccine = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "vaccine").ToArray();

                    // From WCF Call
                    string barcodeId = String.Empty;
                    string firstname1 = String.Empty;
                    string lastname1 = String.Empty;
                    DateTime birthdate = default(DateTime);
                    bool? gender = null;

                    if (omrBarcode != null)
                        barcodeId = omrBarcode.BarcodeData;
                    if(omrDobDay != null && omrDobDay10 != null &&
                        omrDobMonth != null && omrDobYear != null)
                        birthdate = new DateTime((int)omrDobYear.ValueAsFloat, (int)omrDobMonth.ValueAsFloat, (int)omrDobDay10.ValueAsFloat + (int)omrDobDay.ValueAsFloat);
                    if (omrGender != null)
                        gender = omrGender.Value == "M" ? true : false;

                    // Barcode read error
                    if (String.IsNullOrEmpty(barcodeId))
                    {
                        // Show a correction form ...
                        var barcodeField = page.Template.Fields.FirstOrDefault(o => o.Id == String.Format("{0}Barcode", dtl.Id)) as OmrBarcodeField;
                        BarcodeCorrection bc = new BarcodeCorrection(page, barcodeField);
                        if(BarcodeUtil.HasData(page, barcodeField) && bc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            barcodeId = bc.BarcodeId;
                        else
                            throw new InvalidOperationException(String.Format("Could not read barcode on row {0}", rowNum));

                    }
                    if (birthdate > DateTime.Now)
                        throw new InvalidOperationException(String.Format("Birthdate is in the future on row {0}", rowNum));
                    if (gender == null)
                        throw new InvalidOperationException(String.Format("Gender must be selected for row {0}", rowNum));
                    if (birthdate.Month > monthBubble.ValueAsFloat)
                        throw new InvalidOperationException(String.Format("Child with id {0} is born in the future (relative to the form date). Correct and re-scan sheet", barcodeId));

                    var childResult = restUtil.Get<RestReturn>("ChildManagement.svc/RegisterChildWithAppoitments", 
                        new KeyValuePair<string, object>("barcodeId", barcodeId),
                        new KeyValuePair<String, object>("gender", gender),
                        new KeyValuePair<String, Object>("birthdate", birthdate.ToString("yyyy-MM-dd HH:mm:ss")),
                        new KeyValuePair<String, Object>("healthFacilityId", facilityId),
                        new KeyValuePair<String, Object>("modifiedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new KeyValuePair<String, Object>("modifiedBy", userInfo.Id),
                        new KeyValuePair<String, Object>("domicileId", (placeInfo.FirstOrDefault(o=>o.Id > 0) ?? placeInfo.First()).Id)
                        );

                    if (childResult.Id < 0)
                        throw new InvalidOperationException("Server indicated error");

                    // Now we want to update the vaccinations given to be "true"
                    var vaccinationEvent = restUtil.Get<VaccinationEvent[]>("VaccinationEvent.svc/GetVaccinationEventListByChildId", new KeyValuePair<string, object>("childId", childResult.Id));
                    
                    if(omrBcg != null && omrBcg.Length > 0)
                    {
                        var bcgEvent = vaccinationEvent.FirstOrDefault(o => o.DoseId == (s_refDoses.Find(d => d.Fullname == "BCG").Id));
                        this.UpdateVaccination(bcgEvent);

                    }
                    if (omrOpv != null)
                        foreach(var bub in omrOpv)
                        {
                            VaccinationEvent opvEvent = null;
                            if (bub.Value == "0")
                                opvEvent = vaccinationEvent.FirstOrDefault(o => o.DoseId == (s_refDoses.Find(d => d.Fullname == "OPV0").Id));
                            else
                                opvEvent = vaccinationEvent.FirstOrDefault(o => o.DoseId == (s_refDoses.Find(d => d.Fullname == String.Format("OPV {0}", bub.Value)).Id));

                            this.UpdateVaccination(opvEvent);
                        }
                    if (omrPenta != null)
                        foreach (var bub in omrPenta)
                        {
                            VaccinationEvent pentaEvent = vaccinationEvent.FirstOrDefault(o => o.DoseId == (s_refDoses.Find(d => d.Fullname == String.Format("DTP-HepB-Hib {0}", bub.Value)).Id));
                            this.UpdateVaccination(pentaEvent);
                        }
                    if (omrPcv != null)
                        foreach (var bub in omrPcv)
                        {
                            VaccinationEvent pcvEvent = vaccinationEvent.FirstOrDefault(o => o.DoseId == (s_refDoses.Find(d => d.Fullname == String.Format("PCV {0}", bub.Value)).Id));
                            this.UpdateVaccination(pcvEvent);
                        }
                    if (omrRota != null)
                        foreach (var bub in omrRota)
                        {
                            VaccinationEvent rotaEvent = vaccinationEvent.FirstOrDefault(o => o.DoseId == (s_refDoses.Find(d => d.Fullname == String.Format("Rota {0}", bub.Value)).Id));
                            this.UpdateVaccination(rotaEvent);
                        }
                    if (omrMr != null)
                        foreach (var bub in omrMr)
                        {
                            VaccinationEvent mrEvent = vaccinationEvent.FirstOrDefault(o => o.DoseId == (s_refDoses.Find(d => d.Fullname == String.Format("Measles {0}", bub.Value)).Id));
                            this.UpdateVaccination(mrEvent);
                        }

                    // Now we need to update the new vaccines
                    DateTime vaccTime = DateTime.Now;
                    if (omrVaccDay10 != null && omrVaccDay != null)
                        vaccTime = new DateTime(DateTime.Now.Month < monthBubble.ValueAsFloat ? DateTime.Now.Year - 1 : DateTime.Now.Year, (int)monthBubble.ValueAsFloat, (int)omrVaccDay10.ValueAsFloat + (int)omrVaccDay.ValueAsFloat);
                    vaccinationEvent = restUtil.Get<VaccinationEvent[]>("VaccinationEvent.svc/GetVaccinationEventListByChildId", new KeyValuePair<string, object>("childId", childResult.Id));

                    foreach (var vacc in omrVaccine)
                    {
                        string antigenName = vacc.Value;
                        if (antigenName == "ROTA")
                            antigenName = "Rota";
                        else if (antigenName == "PENTA")
                            antigenName = "DTP-HepB-Hib";
                        else if (antigenName == "MR")
                            antigenName = "Measles Rubella";
                        else if (antigenName == "PCV")
                            antigenName = "PCV-13";

                        // Find the scheduled vaccine for this
                        List<Dose> sv = s_refDoses.FindAll(o => o.ScheduledVaccinationId == s_refVaccines.First(v=>v.Name == antigenName).Id);
                        // Find the current dose we're on
                        var lastVe = vaccinationEvent.Where(ve => sv.Exists(o => o.Id == ve.DoseId) && ve.VaccinationStatus == true).OrderByDescending(o=>sv.Find(d=>d.Id == o.DoseId).DoseNumber).FirstOrDefault();
                        int doseNumber = 0;
                        // hack: OPV is odd
                        if (antigenName == "OPV")
                            doseNumber--;
                        if(lastVe != null)
                            doseNumber = sv.Find(d => d.Id == lastVe.DoseId).DoseNumber;
                        
                        // Next we want to get the next dose
                        Dose myDose = sv.FirstOrDefault(o => o.DoseNumber == doseNumber + 1);
                        if (myDose == null)
                            MessageBox.Show(String.Format("Patient #{0} is marked to have antigen {1}. Have detected dose number {2} should be given however no dose of this exists", barcodeId, antigenName, doseNumber + 1));
                        else
                        {
                            // Find an event that suits us
                            var evt = vaccinationEvent.First(o => o.DoseId == myDose.Id);
                            evt.VaccinationDate = evt.ScheduledDate = vaccTime;
                            this.UpdateVaccination(evt);

                            if(omrOutreach != null)
                            {
                                restUtil.Get<RestReturn>("VaccinationAppointmentManagement.svc/UpdateVaccinationApp",
                                    new KeyValuePair<String,Object>("outreach", true),
                                    new KeyValuePair<String, Object>("userId", userInfo.Id),
                                    new KeyValuePair<String, Object>("barcode", barcodeId),
                                    new KeyValuePair<String, Object>("doseId", myDose.Id)
                                    );
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
                new KeyValuePair<String, Object>("vaccinationDate", evt.ScheduledDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss Z")),
                new KeyValuePair<String, Object>("notes", "From form scanner"),
                new KeyValuePair<String, Object>("vaccinationStatus", true));

        }
    }
}
