using GIIS.DataLayer;
using GIIS.DataLayer.Contract;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public class FormTZ02
    {


        /// <summary>
        /// Row data for TZ01 row
        /// </summary>
        /// <remarks>
        /// This could be done better, however GIIS' services are terrible
        /// </remarks>
        public struct Tz02RowData
        {
            /// <summary>
            /// Barcode data
            /// </summary>
            public String Barcode { get; set; }
            /// <summary>
            /// Date of birth
            /// </summary>
            public DateTime? DateOfBirth { get; set; }
            /// <summary>
            /// Gender
            /// </summary>
            public Boolean Gender { get; set; }
            /// <summary>
            /// Vaccination date
            /// </summary>
            public DateTime VaccineDate { get; set; }
            /// <summary>
            /// Vaccines given
            /// </summary>
            public List<ScheduledVaccination> VaccineGiven { get; set; }
            /// <summary>
            /// Get or sets the facility
            /// </summary>
            public Int32 FacilityId { get; set; }
            /// <summary>
            /// The row bounds
            /// </summary>
            public RectangleF RowBounds { get; set; }
            /// <summary>
            /// User info
            /// </summary>
            public User UserInfo { get; set; }
            /// <summary>
            /// Gets or sets the page
            /// </summary>
            public OmrPageOutput Page { get; set; }
            /// <summary>
            /// Historical doses
            /// </summary>
            public List<Dose> Doses { get; set; }
            /// <summary>
            /// Outreach indicator
            /// </summary>
            public bool Outreach { get; set; }
            public int ChildId { get; internal set; }
        }

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
                {
                    Err += "Must select month!; ";
                    return;
                }

                RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
                var userInfo = restUtil.Get<User>("UserManagement.svc/GetUserInfo", new KeyValuePair<string, object>("username", restUtil.GetCurrentUserName));
                var placeInfo = restUtil.Get<Place[]>("PlaceManagement.svc/GetPlaceByHealthFacilityId", new KeyValuePair<string, object>("hf_id", facilityId));
               
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
                        omrVaccDay = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "vaccDay"),
                        omrNew =  dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "newInfo"),
                        omrUpdate = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "correct"),
                        omrIgnore = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "moved");
                    OmrBubbleData[] omrBcg = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "BCG").ToArray(),
                        omrOpv = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "OPV").ToArray(),
                        omrPenta = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "PENTA").ToArray(),
                        omrPcv = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "PCV").ToArray(),
                        omrRota = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "ROTA").ToArray(),
                        omrMr = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "MR").ToArray(),
                        omrVaccine = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "vaccine").ToArray();

                    // From WCF Call
                    // Create row data for the verification form
                    OmrBarcodeField barcodeField = page.Template.FlatFields.Find(o => o.Id == String.Format("{0}Barcode", dtl.Id)) as OmrBarcodeField;
                    OmrBubbleField outreachField = page.Template.FlatFields.OfType<OmrBubbleField>().SingleOrDefault(o => o.AnswerRowGroup == dtl.Id && o.Question == "Outreach"),
                        monthField = page.Template.FlatFields.OfType<OmrBubbleField>().SingleOrDefault(o => o.AnswerRowGroup == dtl.Id && o.Question == "dobMonth" && o.Value == "12");

                    // Barcode bounds
                    Tz02RowData rowData = new Tz02RowData()
                    {
                        RowBounds = new RectangleF()
                        {
                            Location = new PointF(barcodeField.TopLeft.X, monthField.TopLeft.Y - 20),
                            Width = outreachField.TopLeft.X - barcodeField.TopLeft.X,
                            Height = barcodeField.BottomLeft.Y - monthField.TopLeft.Y - 20
                        },
                        UserInfo = userInfo,
                        Page = page,
                        FacilityId = facilityId
                    };


                    if (omrBarcode != null)
                        rowData.Barcode = omrBarcode.BarcodeData;
                    if (omrDobDay != null && omrDobDay10 != null &&
                        omrDobMonth != null && omrDobYear != null)
                        try
                        {
                            rowData.DateOfBirth = new DateTime((int)omrDobYear.ValueAsFloat, (int)omrDobMonth.ValueAsFloat, (int)omrDobDay10.ValueAsFloat + (int)omrDobDay.ValueAsFloat);
                        }
                        catch { }
                    if (omrGender != null)
                        rowData.Gender = omrGender.Value == "M" ? true : false;
                    if (omrOutreach != null)
                        rowData.Outreach = omrOutreach.Value == "T";

                    // Doses
                    rowData.Doses = new List<Dose>();
                    if (omrBcg != null && omrBcg.Length > 0)
                        rowData.Doses.Add(this.FindDoseOrThrow("BCG"));
                    if (omrOpv != null)
                        foreach (var bub in omrOpv)
                        {
                            VaccinationEvent opvEvent = null;
                            if (bub.Value == "0")
                                rowData.Doses.Add(this.FindDoseOrThrow("OPV0"));
                            else
                                rowData.Doses.Add(this.FindDoseOrThrow(String.Format("OPV {0}", bub.Value)));
                        }
                    if (omrPenta != null)
                        foreach (var bub in omrPenta)
                            rowData.Doses.Add(this.FindDoseOrThrow(String.Format("DTP-HepB-Hib {0}", bub.Value)));
                    if (omrPcv != null)
                        foreach (var bub in omrPcv)
                            rowData.Doses.Add(this.FindDoseOrThrow(String.Format("PCV-13 {0}", bub.Value)));
                    if (omrRota != null)
                        foreach (var bub in omrRota)
                            rowData.Doses.Add(this.FindDoseOrThrow(String.Format("Rota {0}", bub.Value)));
                    if (omrMr != null)
                        foreach (var bub in omrMr)
                            rowData.Doses.Add(this.FindDoseOrThrow(String.Format("Measles Rubella {0}", bub.Value), String.Format("Measles {0}", bub.Value)));

                    // Given vaccines
                    rowData.VaccineGiven = new List<ScheduledVaccination>();
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
                        var refData = ReferenceData.Current.Vaccines.FirstOrDefault(v => v.Name == antigenName);
                        if (refData != null)
                            rowData.VaccineGiven.Add(refData);
                        else
                            MessageBox.Show(String.Format("The form expected a vaccination named {0} but the server did not respond with such a vaccine. Server vaccinations: [{1}]", antigenName, String.Join(",", ReferenceData.Current.Vaccines.Select(o=>o.Name).ToArray())));
                    }

                    // Date of vaccination
                    rowData.VaccineDate = DateTime.Now;
                    if (omrVaccDay10 != null && omrVaccDay != null)
                        rowData.VaccineDate = new DateTime(DateTime.Now.Month < monthBubble.ValueAsFloat ? DateTime.Now.Year - 1 : DateTime.Now.Year, (int)monthBubble.ValueAsFloat, (int)omrVaccDay10.ValueAsFloat + (int)omrVaccDay.ValueAsFloat);

                    // Determine what to do 
                    if (omrUpdate?.Value == "T")
                    {
                        ChildSearch bc = new ChildSearch(rowData);
                        if (bc.ShowDialog() == DialogResult.OK)
                        {
                            rowData.Barcode = bc.Child.BarcodeId;
                            rowData.ChildId = bc.Child.Id;
                        }
                        else
                            continue;
                    }
                    else if (omrIgnore?.Value == "T")
                        continue;

                    if (BarcodeUtil.HasData(page, barcodeField) ||
                        dtl.Details.Count > 2)
                    {
                        Registration registration = new Registration(rowData);
                        registration.ShowDialog();
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
        /// Find dose or throw
        /// </summary>
        private Dose FindDoseOrThrow(params string[] doseName)
        {
            Dose retVal = null;

            foreach (var itm in doseName)
                retVal = retVal ?? ReferenceData.Current.Doses.Find(o => o.Fullname == itm);

            if (retVal == null)
                throw new KeyNotFoundException($"Dose {String.Join(",", doseName)} could not be found on server");

            return retVal;
        }
    }
}
