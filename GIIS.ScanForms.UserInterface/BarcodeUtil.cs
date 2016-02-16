using AForge.Imaging;
using AForge.Imaging.Filters;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.ScanForms.UserInterface
{
    public static class BarcodeUtil
    {

        public static bool HasData(OmrPageOutput page, OmrBarcodeField barcode)
        {
            String tPath = Path.GetTempFileName();
            using (Bitmap bmp = new Bitmap((int)barcode.TopRight.X - (int)barcode.TopLeft.X, (int)barcode.BottomLeft.Y - (int)barcode.TopLeft.Y, PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                using (System.Drawing.Image img = System.Drawing.Image.FromFile(page.AnalyzedImage))
                {
                    g.DrawImage(img, 0, 0, new Rectangle(new Point((int)barcode.TopLeft.X, (int)barcode.TopLeft.Y), bmp.Size), GraphicsUnit.Pixel);
                    // is g empty?

                }

                // Blobs
                BlobCounter blobCounter = new BlobCounter();
                blobCounter.MinHeight = 30;
                blobCounter.MinWidth = 30;
                blobCounter.FilterBlobs = true;

                //blobCounter.BackgroundThreshold = Color.Gray;// new Color(255, 255, 255);

                using (var grayscale = new GrayscaleY().Apply(bmp))
                {
                    Threshold binaryThreshold = new Threshold(page.Template.ScanThreshold);
                    binaryThreshold.ApplyInPlace(grayscale);
                    new Invert().ApplyInPlace(grayscale);

                    // Check for circles
                    blobCounter.ProcessImage(grayscale);
                    Blob[] blobs = blobCounter.GetObjectsInformation();
                    return blobs.Where(o => o.ColorMean == Color.FromArgb(255, 255, 255, 255)).Count() > 0;
                }
            }
        }
    }
}
