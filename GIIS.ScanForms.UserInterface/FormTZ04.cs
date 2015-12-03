using GIIS.DataLayer.Contract;
using OmrMarkEngine.Output;
using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.ScanForms.UserInterface
{
    public class FormTZ04
    {


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

                var monthBubble = page.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "Month");
                if (monthBubble == null)
                    Err += "Must select month!; ";

                // Now we want to upload the data 
                BirthDoseSubmission submission = new BirthDoseSubmission()
                {
                    Month = (int)monthBubble.ValueAsFloat,
                    FacilityId = facilityId,
                    Data = new List<BirthDoseData>()
                };

                // Iterate through the rows
                foreach(var dtl in page.Details.OfType<OmrRowData>())
                {

                    OmrBubbleData genderData = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "Gender");
                    OmrBubbleData[] birthDose = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "BirthDose").ToArray(),
                        ttDose = dtl.Details.OfType<OmrBubbleData>().Where(o => o.Key == "TTDose").ToArray();

                    if (genderData == null)
                    {
                        Err += String.Format("Row {0} is missing gender group, skipping; ", dtl.Id);
                        continue;
                    }

                    var data = new BirthDoseData()
                    {
                        Gender = genderData.Value,
                        Doses = new List<string>()
                    };
                    // Doses
                    data.Doses.AddRange(birthDose.Select(o => o.Value));
                    data.Doses.AddRange(ttDose.Select(o => o.Value));
                    submission.Data.Add(data);
                }

                RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
                restUtil.Post("AnonymousDataManagement.svc/PostBirthDose", submission);
            }
            catch(Exception e)
            {
                Trace.TraceError("Error:{0}",e);
                throw;
            }
            finally { dlg.Close(); }
        }

    }
}
