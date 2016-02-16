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

        private OmrPageOutput m_page = null;
        private OmrBarcodeField m_expectedArea = null;

        private RestUtil m_restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));

        /// <summary>
        /// Gets the barcode identifier
        /// </summary>
        public string BarcodeId { get { return this.txtBarcode.Text; } }

        public ChildSearch(OmrPageOutput page, OmrBarcodeField barcode)
        {
            this.m_page = page;
            this.m_expectedArea = barcode;
            InitializeComponent();

            // Crop the image
            String tPath = Path.GetTempFileName();
            using (Bitmap bmp = new Bitmap((int)barcode.TopRight.X - (int)barcode.TopLeft.X, (int)barcode.BottomLeft.Y - (int)barcode.TopLeft.Y, PixelFormat.Format24bppRgb))
            using (Graphics g = Graphics.FromImage(bmp))
            using (Image img = Image.FromFile(this.m_page.AnalyzedImage))
            {
                g.DrawImage(img, 0, 0, new Rectangle(new Point((int)barcode.TopLeft.X, (int)barcode.TopLeft.Y), bmp.Size), GraphicsUnit.Pixel);
                bmp.Save(tPath);
            }

            // Rotate
            using (Image img = Image.FromFile(tPath))
            using (var rotated = new AForge.Imaging.Filters.RotateBilinear(-90).Apply((Bitmap)img))
            using(var scaled = new AForge.Imaging.Filters.ResizeBilinear((int)(rotated.Width * 0.75), (int)(rotated.Height * .75)).Apply(rotated))
            {
                tPath = Path.GetTempFileName();
                scaled.Save(tPath);
                pbBarcode.ImageLocation = tPath;
            }
        }


        public ChildSearch()
        {
            InitializeComponent();
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = !String.IsNullOrEmpty(txtBarcode.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Perform search
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // Show results
            var childData = this.m_restUtil.Get<List<FormTZ01.ChildEntity>>("ChildManagement.svc/SearchByBarcode", new KeyValuePair<string, object>("barcodeId", txtBarcode.Text));

            if(childData.Count == 0)
            {
                btnSearch.Visible = false;
                btnOk.Location = btnSearch.Location;
                btnOk.Enabled = true;
            }
            else
            {
                this.Height = this.btnOk.Top + (this.btnOk.Height * 3);
                lblDob.Text = childData[0].childEntity.Birthdate.ToString("yyyy-MMM-dd");
                lnkName.Text = String.Format("{0} {1}", childData[0].childEntity.Firstname1, childData[0].childEntity.Lastname1);
                lnkName.Tag = childData[0].childEntity.Id;
                lnkName.Links.Clear();
                lnkName.Links.Add(0, lnkName.Text.Length, ConfigurationManager.AppSettings["GIIS_URL"].Replace("/SVC", "/Pages/Child.aspx?id=" + childData[0].childEntity.Id.ToString()));
                btnOk.Enabled = true;
                btnSearch.Enabled = false;
                txtBarcode.ReadOnly = true;
            }

        }

        private void lnkName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
        }
    }
}
