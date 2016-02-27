using GIIS.DataLayer;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public struct VaccineCheckItem
    {
        public VaccinationEvent Event { get; set; }
        public String DoseName { get; set; }
        public override string ToString()
        {
            return this.DoseName;
        }
    }

    public partial class VaccineCorrection : Form
    {

        // Row data
        private FormTZ01.Tz01RowData m_rowData;
        private RestUtil m_restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
        private VaccinationEvent[] m_vaccineEvents;

        public VaccineCorrection()
        {
            InitializeComponent();
        }


        public VaccineCorrection(FormTZ01.Tz01RowData rowData, VaccinationEvent[] vaccinationEvents, List<Dose> refDoses)
        {
            InitializeComponent();

            this.m_rowData = rowData;
            this.m_vaccineEvents = vaccinationEvents;
            
            // Crop the image
            String tPath = Path.GetTempFileName();
            using (Bitmap bmp = new Bitmap((int)rowData.RowBounds.Width / 2, (int)rowData.RowBounds.Height / 2))
            using (Graphics g = Graphics.FromImage(bmp))
            using (Image img = Image.FromFile(rowData.Page.AnalyzedImage))
            using (Image scaled = (Image)new AForge.Imaging.Filters.ResizeNearestNeighbor(img.Width / 2, img.Height / 2).Apply((Bitmap)img))
            {

                g.DrawImage(scaled, 0, 0, new Rectangle((int)rowData.RowBounds.X / 2, (int)rowData.RowBounds.Y / 2, (int)rowData.RowBounds.Width / 2, (int)rowData.RowBounds.Height / 2), GraphicsUnit.Pixel);
                bmp.Save(tPath);
                pbBarcode.ImageLocation = tPath;
            }

            lblChildName.Text = String.Format("{1} {0}", rowData.Child?.Lastname1, rowData.Child?.Firstname1);
            txtBarcode.Text = rowData.StickerValue;

            // First appointment
            foreach (var apt in rowData.Appointment)
            {
                int idx = rowData.Appointment.IndexOf(apt);

                foreach (var vacc in vaccinationEvents.Where(o => o.AppointmentId == apt.Appointment.Id).OrderBy(o => refDoses.Find(d => d.Id == o.DoseId).Fullname))
                {

                    var vacItm = new VaccineCheckItem()
                    {
                        DoseName = refDoses.Find(o => o.Id == vacc.DoseId).Fullname,
                        Event = vacc
                    };
                    if (idx == 0)
                        chkVaccA.Items.Add(vacItm, apt.All);
                    else
                        chkVaccB.Items.Add(vacItm, apt.All);

                }

                // Other data
                if (idx == 0 && apt.Date.HasValue)
                {
                    dtpVaccA.Value = apt.Date.Value;
                    chkOutreachA.Checked = apt.Outreach;
                }
                else if (idx == 1 && apt.Date.HasValue)
                {
                    dtpVaccB.Value = apt.Date.Value;
                    chkSendB.Checked = true;
                    chkOutreachB.Checked = apt.Outreach;
                }

            }


            this.dtpVaccA.MaxDate = this.dtpVaccB.MaxDate = DateTime.Now;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Update vaccination
        /// </summary>
        /// <param name="evt"></param>
        private void UpdateVaccination(VaccinationEvent evt)
        {

            var retVal = this.m_restUtil.Get<RestReturn>("VaccinationEvent.svc/UpdateVaccinationEvent",
                new KeyValuePair<string, object>("vaccinationEventId", evt.Id),
                new KeyValuePair<String, Object>("vaccinationDate", evt.VaccinationDate.ToString("yyyy-MM-dd")),
                new KeyValuePair<String, Object>("notes", "From form scanner"),
                new KeyValuePair<String, Object>("userId", this.m_rowData.UserInfo.Id),
                new KeyValuePair<String, Object>("healthFacilityId", evt.HealthFacilityId),
                new KeyValuePair<String, Object>("vaccineLotId", evt.VaccineLotId),
                new KeyValuePair<String, Object>("vaccinationStatus", evt.VaccinationStatus));
            if (retVal.Id < 0)
            {
                MessageBox.Show("TIIS Service reported Error code, please attempt to submit again in a few moments");
                return;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                btnSubmit.Enabled = false;
                // Associate the child
                if (!String.IsNullOrEmpty(txtBarcode.Text))
                {
                    var retVal = this.m_restUtil.Get<RestReturn>("ChildManagement.svc/UpdateChild",
                        new KeyValuePair<string, object>("barcode", txtBarcode.Text),
                        new KeyValuePair<string, object>("firstname1", this.m_rowData.Child.Firstname1),
                        new KeyValuePair<string, object>("lastname1", this.m_rowData.Child.Lastname1),
                        new KeyValuePair<string, object>("birthdate", this.m_rowData.Child.Birthdate.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, object>("gender", this.m_rowData.Child.Gender),
                        new KeyValuePair<string, object>("healthFacilityId", this.m_rowData.Child.HealthcenterId),
                        new KeyValuePair<string, object>("birthplaceId", this.m_rowData.Child.BirthplaceId),
                        new KeyValuePair<string, object>("domicileId", this.m_rowData.Child.DomicileId),
                        new KeyValuePair<string, object>("statusId", this.m_rowData.Child.StatusId),
                        new KeyValuePair<string, object>("address", this.m_rowData.Child.Address),
                        new KeyValuePair<string, object>("phone", this.m_rowData.Child.Phone),
                        new KeyValuePair<string, object>("motherFirstname", this.m_rowData.Child.MotherFirstname),
                        new KeyValuePair<string, object>("motherLastname", this.m_rowData.Child.MotherLastname),
                        new KeyValuePair<string, object>("notes", "Updated by form scanning application"),
                        new KeyValuePair<string, object>("userId", this.m_rowData.UserInfo.Id),
                        new KeyValuePair<string, object>("childId", this.m_rowData.Child.Id)
                    );
                    if (retVal.Id < 0)
                    {
                        MessageBox.Show("TIIS Service reported Error code. This is usually caused by a duplicate barcode sticker");
                        return;
                    }
                }


                foreach (var apt in this.m_rowData.Appointment)
                {

                    int idx = this.m_rowData.Appointment.IndexOf(apt);
                    CheckedListBox vaccineBox = idx == 0 ? chkVaccA : chkVaccB;
                    if (vaccineBox.Enabled)
                        foreach (var vacc in vaccineBox.Items.OfType<VaccineCheckItem>())
                        {
                            vacc.Event.VaccinationStatus = true;
                            vacc.Event.VaccinationDate = idx == 0 ? dtpVaccA.Value : dtpVaccB.Value;
                            vacc.Event.VaccinationStatus = vaccineBox.CheckedItems.Contains(vacc);
                            this.UpdateVaccination(vacc.Event);

                            if (idx == 0 ? chkOutreachA.Checked : chkOutreachB.Checked)
                            {
                                this.m_restUtil.Get<RestReturn>("VaccinationAppointmentManagement.svc/UpdateVaccinationApp",
                                    new KeyValuePair<String, Object>("outreach", true),
                                    new KeyValuePair<String, Object>("userId", this.m_rowData.UserInfo.Id),
                                    new KeyValuePair<String, Object>("barcode", this.m_rowData.Barcode.StartsWith("T") ? txtBarcode.Text : this.m_rowData.Barcode),
                                    new KeyValuePair<String, Object>("doseId", vacc.Event.DoseId)
                                    );
                            }

                        }
                }
                this.Close();
            }
            finally
            {
                btnSubmit.Enabled = false;
            }
        }

        private void chkSendB_CheckedChanged(object sender, EventArgs e)
        {
            chkOutreachB.Enabled = chkVaccB.Enabled = dtpVaccB.Enabled = chkSendB.Checked;
        }

        private void chkSendA_CheckedChanged(object sender, EventArgs e)
        {
            chkOutreachA.Enabled = chkVaccA.Enabled = dtpVaccA.Enabled = chkSendA.Checked;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
