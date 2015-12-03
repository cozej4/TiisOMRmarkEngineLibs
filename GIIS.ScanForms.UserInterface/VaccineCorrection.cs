using GIIS.DataLayer;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public VaccineCorrection()
        {
            InitializeComponent();
        }

        public IEnumerable<VaccineCheckItem> Vaccines
        {
            get
            {
                return this.chkVaccines.CheckedItems.OfType<VaccineCheckItem>();
            }
        }

        public VaccineCorrection(OmrPageOutput page, OmrBarcodeField barcode, VaccinationAppointment appointment, VaccinationEvent[] vaccinationEvent, List<Dose> doseInfo)
        {
            InitializeComponent();

            // Crop the image
            String tPath = Path.GetTempFileName();
            using (Bitmap bmp = new Bitmap((int)barcode.TopRight.X - (int)barcode.TopLeft.X, (int)barcode.BottomLeft.Y - (int)barcode.TopLeft.Y))
            using (Graphics g = Graphics.FromImage(bmp))
            using (Image img = Image.FromFile(page.AnalyzedImage))
            {
                g.DrawImage(img, 0, 0, new Rectangle(new Point((int)barcode.TopLeft.X, (int)barcode.TopLeft.Y), bmp.Size), GraphicsUnit.Pixel);
                bmp.Save(tPath);
                pbBarcode.ImageLocation = tPath;
            }

            // Now to populate the listbox
            foreach(var vacc in vaccinationEvent.Where(o=>o.AppointmentId == appointment.Id).OrderBy(o=>doseInfo.Find(d=>d.Id == o.DoseId).Fullname))
            {
                chkVaccines.Items.Add(new VaccineCheckItem()
                {
                    DoseName = doseInfo.Find(o=>o.Id == vacc.DoseId).Fullname,
                    Event = vacc
                }, true);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
