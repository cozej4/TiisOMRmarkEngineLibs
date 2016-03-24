using GIIS.DataLayer;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public partial class ChildSearch : Form
    {

        public class ChildResults
        {
            public Int32 Id { get; set; }
            public string Firstname1 { get; set; }
            public string Lastname1 { get; set; }
            public DateTime Birthdate { get; set; }
            public bool Gender { get; set; }
            public string HealthcenterId { get; set; }
            public string DomicileId { get; set; }
            public string MotherFirstname { get; set; }
            public string MotherLastname { get; set; }
            public string BarcodeId { get; set; }
            public string Firstname2 { get; set; }
        }

        private OmrPageOutput m_page = null;

        private RestUtil m_restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));

        /// <summary>
        /// Gets the barcode identifier
        /// </summary>
        public ChildResults Child
        {
            get; set;
        }

        public ChildSearch(FormTZ02.Tz02RowData rowData)
        {
            this.m_page = rowData.Page;
            InitializeComponent();

            // Crop the image
            String tPath = Path.GetTempFileName();
            using (Bitmap bmp = new Bitmap((int)rowData.Page.BottomRight.Y / 3, (int)rowData.RowBounds.Width / 3))
            using (Graphics g = Graphics.FromImage(bmp))
            using (Image img = Image.FromFile(rowData.Page.AnalyzedImage))
            using (Image scaled = (Image)new AForge.Imaging.Filters.ResizeNearestNeighbor(img.Width / 3, img.Height / 3).Apply((Bitmap)img))
            using (Image rotated = (Image)new AForge.Imaging.Filters.RotateBilinear(-90).Apply((Bitmap)scaled))
            {

                g.DrawImage(rotated, 0, 0, new Rectangle(0, (int)rowData.RowBounds.X / 3, (int)rowData.Page.BottomRight.Y / 3, (int)rowData.RowBounds.Width / 3), GraphicsUnit.Pixel);
                bmp.Save(tPath);
                this.pbBarcode.ImageLocation = tPath;
            }

            cbxVillage.Items.AddRange(ReferenceData.Current.Places.Select(o => new Registration.PlaceListItem() { Place = o as Place }).OfType<Object>().ToArray());

        }


        public ChildSearch()
        {
            InitializeComponent();
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = btnSearch.Enabled = !String.IsNullOrEmpty(txtBarcode.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Child = lsvResults.SelectedItems[0].Tag as ChildResults;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Perform search
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder searchBuilder = new StringBuilder();
                if (!String.IsNullOrEmpty(txtBarcode.Text))
                    searchBuilder.AppendFormat("barcode={0}!", txtBarcode.Text);
                if (!String.IsNullOrEmpty(txtFamily.Text))
                    searchBuilder.AppendFormat("lastname1={0}!", txtFamily.Text);
                if (!String.IsNullOrEmpty(txtGiven.Text))
                    searchBuilder.AppendFormat("firstname1={0}!", txtGiven.Text);
                if (chkDob.Checked)
                    searchBuilder.AppendFormat("birthdatefrom={0}!", dtpDob.Value.ToString("yyyy-MM-dd"));
                if (chkDobTo.Checked)
                    searchBuilder.AppendFormat("birthdateto={0}!", dtpDobTo.Value.ToString("yyyy-MM-dd"));
                if (cbxGender.SelectedIndex >= 0)
                    searchBuilder.AppendFormat("gender={0}!", cbxGender.SelectedIndex == 0 ? false : true);
                if (cbxVillage.SelectedItem != null)
                    searchBuilder.AppendFormat("domicileid={0}!", (cbxVillage.SelectedItem as Registration.PlaceListItem).Place.Id);
                searchBuilder.Remove(searchBuilder.Length - 1, 1);

                btnSearch.Enabled = false;
                lsvResults.Items.Clear();

                var retVal = this.m_restUtil.Get<List<ChildResults>>("ChildManagement.svc/Search",
                    new KeyValuePair<string, object>("where", searchBuilder.ToString()));


                // Iterate items
                foreach (var itm in retVal)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = itm.Id.ToString();
                    lvi.Tag = itm;
                    lvi.SubItems.Add(itm.Lastname1);
                    lvi.SubItems.Add(itm.Firstname1);
                    lvi.SubItems.Add(itm.Gender ? "Male" : "Female");
                    lvi.SubItems.Add(itm.DomicileId);
                    lvi.SubItems.Add(itm.Birthdate.ToString("yyyy-MMM-dd"));
                    lsvResults.Items.Add(lvi);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Trace.TraceError(ex.ToString());
            }
            finally
            {
                btnSearch.Enabled = true;
            }

        }

        private void lnkName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }

        private void chkDob_CheckedChanged(object sender, EventArgs e)
        {
            dtpDob.Enabled = chkDob.Checked;
        }

        private void chkDobTo_CheckedChanged(object sender, EventArgs e)
        {
            dtpDobTo.Enabled = chkDobTo.Checked;
        }

        private void lsvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = lsvResults.SelectedItems.Count == 1;
        }
    }
}
