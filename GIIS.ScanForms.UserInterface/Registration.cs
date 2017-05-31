using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using GIIS.DataLayer;
using OmrMarkEngine.Template.Scripting.Util;

namespace GIIS.ScanForms.UserInterface
{
    public partial class Registration : Form
    {
        /// <summary>
        /// Place list item
        /// </summary>
        public class PlaceListItem
        {
            public Place Place { get; set; }
            public override string ToString()
            {
                return this.Place.Name;
            }
        }

        // Row data
        private FormTZ02.Tz02RowData m_rowData;

        // Places
        private RestUtil m_restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));

        public Registration(FormTZ02.Tz02RowData rowData)
        {
            InitializeComponent();

            // Crop the image
            String tPath = Path.GetTempFileName();
            using (Bitmap bmp = new Bitmap((int)rowData.Page.BottomRight.Y / 3, (int)rowData.RowBounds.Width / 3))
            using (Graphics g = Graphics.FromImage(bmp))
            using (Image img = Image.FromFile(rowData.Page.AnalyzedImage))
            using (Image scaled = (Image)new ResizeNearestNeighbor(img.Width / 3, img.Height / 3).Apply((Bitmap)img))
            using (Image rotated = (Image)new RotateBilinear(-90).Apply((Bitmap)scaled))
            {

                g.DrawImage(rotated, 0, 0, new Rectangle(0, (int)rowData.RowBounds.X / 3, (int)rowData.Page.BottomRight.Y / 3, (int)rowData.RowBounds.Width / 3), GraphicsUnit.Pixel);
                bmp.Save(tPath);
                pbScan.ImageLocation = tPath;
            }

            this.m_rowData = rowData;

            cbxVillage.Items.AddRange(ReferenceData.Current.Places.Select(o => new PlaceListItem() { Place = o as Place }).OfType<Object>().ToArray());



            dtpDob.MaxDate = dtpVaccDate.MaxDate = DateTime.Now;

            txtBarcode.Text = rowData.Barcode;
            try
            {
                if (rowData.DateOfBirth.HasValue)
                    dtpDob.Value = rowData.DateOfBirth.Value;
            }
            catch (Exception e){
                errBarcode.SetError(dtpDob, e.Message);

            }
            cbxGender.SelectedIndex = rowData.Gender ? 1 : 0;

            try
            {
                dtpVaccDate.Value = rowData.VaccineDate;
                dtpVaccDate.MinDate = dtpDob.Value;
            }
            catch (Exception e)
            {
                errBarcode.SetError(dtpVaccDate, e.Message);
            }

            chkOutreach.Checked = rowData.Outreach;

            // Selected antigens
            foreach(var itm in rowData.VaccineGiven)
            {
                String antigenName = itm.Name;
                if (antigenName == "ROTA")
                {
                    antigenName = ReferenceData.Current.Vaccines
                        .Single(d => d.Name.ToLower().Contains("rota")).Name;
                }
                else if (antigenName == "PENTA")
                {
                    antigenName = ReferenceData.Current.Vaccines
                        .Single(d => (d.Name.ToLower().Contains("dtp") || d.Name.ToLower().Contains("penta")))
                        .Name;
                }
                else if (antigenName == "MR")
                {
                    antigenName = ReferenceData.Current.Vaccines
                        .Single(d => (d.Name.ToLower().Contains("mr")))
                        .Name;
                }
                else if (antigenName == "PCV")
                {
                    antigenName = ReferenceData.Current.Vaccines
                        .Single(d => d.Name.ToLower().Contains("pcv")).Name;
                }
                else if (antigenName == "OPV")
                {
                    antigenName = ReferenceData.Current.Vaccines
                        .Single(d => d.Name.ToLower().Contains("opv")).Name;
                }
                
                var ctl = this.grpVaccine.Controls.Find(String.Format("chk{0}", antigenName.ToUpper()), false).FirstOrDefault() as CheckBox;
                if(ctl != null)
                    ctl.Checked = true;
            }

            // Selected vaccination doses
            foreach(var itm in rowData.Doses)
            {
                String doseName = itm.Fullname.Replace(" ", "").Replace("-", "");
                if (doseName == "BCG")
                    doseName = "BCG0";

                var ctl = this.grpHistoricalVacc.Controls.Find(String.Format("chk{0}", doseName), false).FirstOrDefault() as CheckBox;

                if (ctl != null)
                    ctl.Checked = true;
            }

            // Get existing data?
            if (this.m_rowData.Barcode != null || this.m_rowData.ChildId != 0)
                this.PopulatePatientDetails(this.m_rowData);

            this.ValidateForm();
        }

        /// <summary>
        /// Populate patient details
        /// </summary>
        private void PopulatePatientDetails(FormTZ02.Tz02RowData rowData)
        {
            Child child = null;

            if (!String.IsNullOrEmpty(rowData.Barcode))
            {
                var childData = this.m_restUtil.Get<List<FormTZ01.ChildEntity>>("ChildManagement.svc/SearchByBarcode", new KeyValuePair<string, object>("barcodeId", rowData.Barcode));
                if (childData.Count == 0)
                    return;
                child = childData[0].childEntity;
            }
            else
            {
                var childData = this.m_restUtil.Get<List<FormTZ01.ChildEntity>>("ChildManagement.svc/GetChildById", new KeyValuePair<string, object>("childid", rowData.ChildId));
                if (childData.Count == 0)
                    return;
                child = childData[0].childEntity;

            }

            this.txtBarcode.Text = child.BarcodeId;
            this.txtFamily.Text = child.Lastname1;
            this.txtGiven.Text = child.Firstname1;
            this.txtMotherFamily.Text = child.MotherLastname;
            this.txtMotherGiven.Text = child.MotherFirstname;
            this.txtTelephone.Text = child.Phone;

            if(!this.m_rowData.DateOfBirth.HasValue)
                this.dtpDob.Value = child.Birthdate;

            this.cbxGender.SelectedIndex = child.Gender ? 1 : 0;
            this.cbxVillage.SelectedItem = this.cbxVillage.Items.OfType<PlaceListItem>().SingleOrDefault(o => o.Place.Id == child.DomicileId);

        }

        /// <summary>
        /// Validate form
        /// </summary>
        private bool ValidateForm()
        {
            bool valid = true;
            if ((cbxVillage.SelectedItem as PlaceListItem)?.Place == null)
            {
                errBarcode.SetError(cbxVillage, "Village is mandatory");
                valid = false;
            }
            if (String.IsNullOrEmpty(txtBarcode.Text))
            {
                errBarcode.SetError(txtBarcode, "Barcode is mandatory");
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Upload the data to GIIS
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                btnSubmit.Enabled = false;
                errBarcode.Clear();
                // Validation stuff
                if (!this.ValidateForm()) return;
                Place selectedPlace = (cbxVillage.SelectedItem as PlaceListItem)?.Place;

                // Register the child
                var childData = this.m_restUtil.Get<List<FormTZ01.ChildEntity>>("ChildManagement.svc/SearchByBarcode", new KeyValuePair<string, object>("barcodeId", txtBarcode.Text));
                RestReturn childResult = null;
                if (childData.Count == 0) // Create
                    childResult = this.m_restUtil.Get<RestReturn>("ChildManagement.svc/RegisterChildWithAppoitments",
                        new KeyValuePair<string, object>("barcodeId", txtBarcode.Text),
                        new KeyValuePair<string, object>("firstname1", txtGiven.Text),
                        new KeyValuePair<string, object>("lastname1", txtFamily.Text),
                        new KeyValuePair<string, object>("birthdate", dtpDob.Value.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, object>("gender", cbxGender.SelectedIndex == 0 ? false : true),
                        new KeyValuePair<string, object>("healthFacilityId", this.m_rowData.FacilityId),
                        new KeyValuePair<string, object>("domicileId", selectedPlace.Id),
                        new KeyValuePair<string, object>("statusId", 1),
                        new KeyValuePair<string, object>("phone", txtTelephone.Text),
                        new KeyValuePair<string, object>("motherFirstname", txtMotherFamily.Text),
                        new KeyValuePair<string, object>("motherLastname", txtMotherGiven.Text),
                        new KeyValuePair<string, object>("notes", "Updated by form scanning application"),
                        new KeyValuePair<string, object>("userId", this.m_rowData.UserInfo.Id)
                    );
                else
                {
                    childResult = this.m_restUtil.Get<RestReturn>("ChildManagement.svc/UpdateChild",
                        new KeyValuePair<string, object>("childId", childData[0].childEntity.Id),
                        new KeyValuePair<string, object>("barcode", txtBarcode.Text),
                        new KeyValuePair<string, object>("firstname1", txtGiven.Text),
                        new KeyValuePair<string, object>("lastname1", txtFamily.Text),
                        new KeyValuePair<string, object>("birthdate", dtpDob.Value.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, object>("gender", cbxGender.SelectedIndex == 0 ? false : true),
                        new KeyValuePair<string, object>("healthFacilityId", this.m_rowData.FacilityId),
                        new KeyValuePair<string, object>("domicileId", selectedPlace.Id),
                        new KeyValuePair<string, object>("statusId", 1),
                        new KeyValuePair<string, object>("phone", txtTelephone.Text),
                        new KeyValuePair<string, object>("motherFirstname", txtMotherFamily.Text),
                        new KeyValuePair<string, object>("motherLastname", txtMotherGiven.Text),
                        new KeyValuePair<string, object>("notes", "Updated by form scanning application"),
                        new KeyValuePair<string, object>("userId", this.m_rowData.UserInfo.Id)
                    );
                    if (childResult.Id >= 0)
                        childResult = new RestReturn() { Id = childData[0].childEntity.Id };
                }
                if (childResult.Id < 0)
                {
                    MessageBox.Show("TIIS Service reported Error code. This is usually caused by a duplicate barcode sticker");
                    return;
                }


                // Update doses
                var vaccinationEvent = this.m_restUtil.Get<VaccinationEvent[]>("VaccinationEvent.svc/GetVaccinationEventListByChildId", new KeyValuePair<string, object>("childId", childResult.Id));
               
                foreach (var evt in vaccinationEvent)
                {
                    var dose = ReferenceData.Current.Doses.FirstOrDefault(o => o.Id == evt.DoseId);
                    String doseName = dose.Fullname.Replace(" ", "").Replace("-", "");
                    if (doseName == "BCG")
                        doseName = "BCG0";

                    var ctl = this.grpHistoricalVacc.Controls.Find(String.Format("chk{0}", doseName), false).FirstOrDefault() as CheckBox;
                    evt.VaccinationDate = evt.ScheduledDate;
                    if (ctl != null && ctl.Checked)
                    {
                        evt.VaccinationStatus = true;
                        this.UpdateVaccination(evt, dose);
                    }
                }

                
                // Re-fetch doses so we get new data from the system
                vaccinationEvent = this.m_restUtil.Get<VaccinationEvent[]>("VaccinationEvent.svc/GetVaccinationEventListByChildId", new KeyValuePair<string, object>("childId", childResult.Id));
                
               
                // Give doses
                foreach (var ctl in grpVaccine.Controls)
                {
                    if (!(ctl is CheckBox))
                        continue; // skip non check boxes

                    var checkbox = ctl as CheckBox;

                    if (!checkbox.Checked)
                        continue;

                    
                    string antigenName = checkbox.Name.Substring(3);
                    
                    if (antigenName == "ROTA")
                    {
                        antigenName = ReferenceData.Current.Vaccines
                            .Single(d => d.Name.ToLower().Contains("rota")).Name;
                    }
                    else if (antigenName == "PENTA")
                    {
                        antigenName = ReferenceData.Current.Vaccines
                            .Single(d => (d.Name.ToLower().Contains("dtp") || d.Name.ToLower().Contains("penta")))
                            .Name;
                    }
                    else if (antigenName == "MR")
                    {
                        antigenName = ReferenceData.Current.Vaccines
                            .Single(d => (d.Name.ToLower().Contains("mr")))
                            .Name;
                    }
                    else if (antigenName == "PCV")
                    {
                        antigenName = ReferenceData.Current.Vaccines
                            .Single(d => d.Name.ToLower().Contains("pcv")).Name;
                    }
                    else if (antigenName == "OPV")
                    {
                        antigenName = ReferenceData.Current.Vaccines
                            .Single(d => d.Name.ToLower().Contains("opv")).Name;
                    }
                    else if (antigenName == "Outreach")
                        continue;
                    
                    
                    
                    var vaccine = ReferenceData.Current.Vaccines.FirstOrDefault(o => o.Name.Equals(antigenName));
                    
                  

                    // Find the scheduled vaccine for this
                    List<Dose> sv = ReferenceData.Current.Doses.FindAll(o => o.ScheduledVaccinationId == vaccine.Id);
                    
                   
                    // Find the current dose we're on
                    var lastVe = vaccinationEvent
                        .Where(ve => sv.Exists(o => o.Id == ve.DoseId) && ve.VaccinationStatus == true)
                        .OrderByDescending(o => sv.Find(d => d.Id == o.DoseId).DoseNumber).FirstOrDefault();
                    
                    
                    int doseNumber = 0;
                    // hack: OPV is odd
                    if (antigenName == ReferenceData.Current.Vaccines
                            .Single(d => d.Name.ToLower().Contains("opv")).Name)
                    {
                        doseNumber--;
                    }
                    if (lastVe != null)
                    {
                        doseNumber = sv.Find(d => d.Id == lastVe.DoseId).DoseNumber;
                    }


                    
                    
                    // Next we want to get the next dose
                    Dose myDose = sv.FirstOrDefault(o => o.DoseNumber == doseNumber + 1);
                    if (myDose == null)
                        MessageBox.Show(String.Format("Patient #{0} is marked to have antigen {1}. Have detected dose number {2} should be given however no dose of this exists", txtBarcode.Text, antigenName, doseNumber + 1));
                    else
                    {
                        // Find an event that suits us
                        var evt = vaccinationEvent.FirstOrDefault(o => o.DoseId == myDose.Id);
                        if (evt == null)
                        {
                            // Is it because it is MR?
                            if (antigenName == "Measles Rubella") // Maybe "measles"
                            {
                                antigenName = "Measles";
                                vaccine = ReferenceData.Current.Vaccines.FirstOrDefault(o => o.Name == antigenName);
                                // Find the scheduled vaccine for this
                                sv = ReferenceData.Current.Doses.FindAll(o => o.ScheduledVaccinationId == vaccine.Id);
                                lastVe = vaccinationEvent.Where(ve => sv.Exists(o => o.Id == ve.DoseId) && ve.VaccinationStatus == true).OrderByDescending(o => sv.Find(d => d.Id == o.DoseId).DoseNumber).FirstOrDefault();
                                doseNumber = 0;
                                if (lastVe != null)
                                    doseNumber = sv.Find(d => d.Id == lastVe.DoseId).DoseNumber;
                                myDose = sv.FirstOrDefault(o => o.DoseNumber == doseNumber + 1);
                                evt = vaccinationEvent.FirstOrDefault(o => o.DoseId == myDose.Id);
                            }
                        }
                        if(evt == null)
                        { 
                            MessageBox.Show(String.Format("Patient was never scheduled to receive {0} so can't update that dose", myDose.Fullname));
                            return;
                        }
                        evt.VaccinationDate = evt.ScheduledDate = dtpVaccDate.Value;
                        evt.VaccinationStatus = true;
                        this.UpdateVaccination(evt, myDose);

                        if (chkOutreach.Checked)
                        {
                            this.m_restUtil.Get<RestReturn>("VaccinationAppointmentManagement.svc/UpdateVaccinationApp",
                                new KeyValuePair<String, Object>("outreach", true),
                                new KeyValuePair<String, Object>("userId", this.m_rowData.UserInfo.Id),
                                new KeyValuePair<String, Object>("barcode", txtBarcode.Text),
                                new KeyValuePair<String, Object>("doseId", myDose.Id)
                                );
                        }
                    }
                }


                this.Close();
            }
            finally
            {
                btnSubmit.Enabled = true;
            }

        }

        /// <summary>
        /// Update vaccination
        /// </summary>
        /// <param name="evt"></param>
        private void UpdateVaccination(VaccinationEvent evt, Dose dose)
        {
            RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));

            var retVal = this.m_restUtil.Get<RestReturn>("VaccinationEvent.svc/UpdateVaccinationEvent",
                new KeyValuePair<string, object>("vaccinationEventId", evt.Id),
                new KeyValuePair<String, Object>("vaccinationDate", evt.VaccinationDate.ToString("yyyy-MM-dd")),
                new KeyValuePair<String, Object>("notes", "From form scanner"),
                new KeyValuePair<String, Object>("userId", this.m_rowData.UserInfo.Id),
                new KeyValuePair<String, Object>("healthFacilityId", this.m_rowData.FacilityId),
                new KeyValuePair<String, Object>("vaccineLotId", -2),
                new KeyValuePair<String, Object>("vaccinationStatus", evt.VaccinationStatus));
            if (retVal.Id < 0)
            {
                MessageBox.Show("TIIS Service reported Error code, please attempt to submit again in a few moments");
                return;
            }


        }

        /// <summary>
        /// Update min vacc dates
        /// </summary>
        private void dtpDob_ValueChanged(object sender, EventArgs e)
        {
            dtpVaccDate.MinDate = dtpDob.Value;
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
