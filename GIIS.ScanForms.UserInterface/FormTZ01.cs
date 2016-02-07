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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public class FormTZ01
    {


        /// <summary>
        /// Tz01 vaccination
        /// </summary>
        public struct Tz01Vaccination
        {
            /// <summary>
            /// The date of vaccination
            /// </summary>
            public DateTime? Date { get; set; }
            /// <summary>
            /// True if all vaccines were given
            /// </summary>
            public bool All { get; set; }
            /// <summary>
            /// True if the vaccine is outreach
            /// </summary>
            public bool Outreach { get; set; }
            /// <summary>
            /// Gets or sets the appointment
            /// </summary>
            public VaccinationAppointment Appointment { get; set; }
        }

        /// <summary>
        /// Row data for TZ01 row
        /// </summary>
        /// <remarks>
        /// This could be done better, however GIIS' services are terrible
        /// </remarks>
        public struct Tz01RowData
        {
            /// <summary>
            /// Barcode data
            /// </summary>
            public String Barcode { get; set; }
            /// <summary>
            /// Barcode data
            /// </summary>
            public String StickerValue { get; set; }
            /// <summary>
            /// Early appointment
            /// </summary>
            public List<Tz01Vaccination> Appointment { get; set; }
            /// <summary>
            /// The child to which the row applies
            /// </summary>
            public Child Child { get; set; }
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
                
                foreach (var patientRow in page.Details.OfType<OmrRowData>().Where(o=>!o.Id.Contains("-")))
                {
                    // Master patient row processing
                    string rowNum = patientRow.Id.Substring(patientRow.Id.Length - 1, 1);

                    if (patientRow.Details.Count == 0)
                        continue;


                    // Create row data for the verification form
                    OmrBarcodeField barcodeField = page.Template.FlatFields.Find(o => o.Id == String.Format("{0}Barcode", patientRow.Id)) as OmrBarcodeField,
                        omrStickerField = page.Template.FlatFields.Find(o => o.Id == String.Format("{0}Sticker", patientRow.Id)) as OmrBarcodeField;

                    Tz01RowData rowData = new Tz01RowData()
                    {
                        RowBounds = new RectangleF()
                        {
                            Location = barcodeField.TopLeft,
                            Width = page.BottomRight.X - barcodeField.TopLeft.X,
                            Height = omrStickerField.BottomRight.Y - barcodeField.TopLeft.Y
                        },
                        UserInfo = userInfo,
                        Page = page
                    };

                    // Barcodes only at this level
                    OmrBarcodeData omrBarcode = patientRow.Details.OfType<OmrBarcodeData>().FirstOrDefault(o => o.Id == String.Format("{0}Barcode", patientRow.Id)),
                        omrSticker = patientRow.Details.OfType<OmrBarcodeData>().FirstOrDefault(o => o.Id == String.Format("{0}Sticker", patientRow.Id));

                    
                    if (omrBarcode == null)
                        rowData.Barcode = String.Empty;
                    else
                        rowData.Barcode = omrBarcode.BarcodeData;
                    rowData.StickerValue = omrSticker?.BarcodeData;

                    // Try to lookup the poor chap
                    if (rowData.Barcode.StartsWith("T")) // TEMP ID AUTH
                    {
                        rowData.StickerValue = omrSticker?.BarcodeData;
                        var childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/GetChildById",
                            new KeyValuePair<string, object>("childId", rowData.Barcode.Replace("T", ""))
                        );
                        if (childDataList == null)
                            throw new InvalidOperationException("Could not deserialize response");
                        else if (childDataList.Count == 0)
                        {
                            throw new InvalidOperationException("Child with barcode " + rowData.Barcode + " not found!");
                        }
                        rowData.Child = childDataList[0].childEntity;
                    }
                    else
                    {
                        var childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/SearchByBarcode",
                                            new KeyValuePair<string, object>("barcodeId", rowData.Barcode));
                        if (childDataList == null)
                            throw new InvalidOperationException("Could not deserialize response");
                        else if (childDataList.Count == 0)
                        {
                            if (!String.IsNullOrEmpty(rowData.StickerValue))
                                childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/SearchByBarcode",
                                                new KeyValuePair<string, object>("barcodeId", rowData.StickerValue));

                            if (childDataList == null)
                                throw new InvalidOperationException("Could not deserialize response");
                            else if (childDataList.Count == 0)
                                throw new InvalidOperationException("Child with barcode " + rowData.Barcode + " not found!");
                        }                        
                        rowData.Child = childDataList[0].childEntity;
                    }

                    // Get appointments and vaccinations for child
                    VaccinationAppointment[] appts = restUtil.Get<VaccinationAppointment[]>("VaccinationAppointmentManagement.svc/GetVaccinationAppointmentsByChildId", new KeyValuePair<string, object>("childId", rowData.Child.Id));
                    VaccinationEvent[] vaccinationEvent = restUtil.Get<VaccinationEvent[]>("VaccinationEvent.svc/GetVaccinationEventListByChildId", new KeyValuePair<string, object>("childId", rowData.Child.Id));
                    rowData.Appointment = new List<Tz01Vaccination>();

                    // iterate over the sub-rows
                    foreach (var aptRow in page.Details.OfType<OmrRowData>().Where(o=>o.Id.StartsWith(String.Format("{0}-", patientRow.Id))).OrderBy(r=>r.Id))
                    {
                        OmrBarcodeData omrAptId = aptRow.Details.OfType<OmrBarcodeData>().FirstOrDefault();
                        
                        OmrBubbleData omrDay10 = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "day10"),
                            omrDay = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "day"),
                            omrOutreach = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "outreach"),
                            omrVaccine = aptRow.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "vaccine");

                        // First, get the appointment data
                        Tz01Vaccination vaccineData = new Tz01Vaccination();

                        if (omrAptId == null)
                        {
                            barcodeField = page.Template.Fields.FirstOrDefault(o => o.AnswerRowGroup == aptRow.Id) as OmrBarcodeField;
                            // Show a correction form ...
                            BarcodeCorrection bc = new BarcodeCorrection(page, barcodeField);
                            if (BarcodeUtil.HasData(page, barcodeField) && bc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                vaccineData.Appointment = appts.FirstOrDefault(o => o.ScheduledDate.Date.Equals(DateTime.ParseExact(bc.BarcodeId, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            vaccineData.Appointment = appts.FirstOrDefault(o => o.Id.ToString() == omrAptId.BarcodeData);
                        }

                        // Validate we got an appointment
                        if (vaccineData.Appointment == null)
                            throw new InvalidOperationException(String.Format("Could not find appointment data for update on row {0}", rowNum));

                        vaccineData.All = omrVaccine?.Value == "all";
                        vaccineData.Outreach = Boolean.Parse(omrOutreach?.Value ?? "false");
                        if(omrDay != null || omrDay10 != null)
                            try
                            {
                                vaccineData.Date = new DateTime(vaccineData.Appointment.ScheduledDate.Year, vaccineData.Appointment.ScheduledDate.Month, (int)(omrDay?.ValueAsFloat + omrDay10?.ValueAsFloat));
                                if (vaccineData.Date > DateTime.Now)
                                    vaccineData.Date = new DateTime(year, month, (int)(omrDay.ValueAsFloat + omrDay10.ValueAsFloat));
                            }
                            catch(Exception e) {
                                Trace.TraceError(e.ToString());
                            }
                        rowData.Appointment.Add(vaccineData);
                    }

                    if (rowData.Appointment[0].Date.HasValue ||
                        rowData.Appointment[1].Date.HasValue ||
                        !String.IsNullOrEmpty(rowData.StickerValue))
                    {
                        VaccineCorrection vc = new VaccineCorrection(rowData, vaccinationEvent, ReferenceData.Current.Doses);
                        vc.ShowDialog();
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
