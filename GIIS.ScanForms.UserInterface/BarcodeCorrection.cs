using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIIS.ScanForms.UserInterface
{
    public partial class BarcodeCorrection : Form
    {

        private OmrPageOutput m_page = null;
        private OmrBarcodeField m_expectedArea = null;


        /// <summary>
        /// Gets the barcode identifier
        /// </summary>
        public string BarcodeId { get { return this.txtBarcode.Text; } }

        public BarcodeCorrection(OmrPageOutput page, OmrBarcodeField barcode)
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


        public BarcodeCorrection()
        {
            InitializeComponent();
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = !String.IsNullOrEmpty(txtBarcode.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
