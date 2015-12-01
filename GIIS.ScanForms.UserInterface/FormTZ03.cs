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
    public class FormTZ03
    {

        /// <summary>
        /// Error string
        /// </summary>
        public String Err { get; set; }

        public void UploadData(OmrPageOutput page)
        {

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
                WeighTallySubmission submission = new WeighTallySubmission()
                {
                    Month = (int)monthBubble.ValueAsFloat,
                    FacilityId = facilityId,
                    Data = new List<WeighTallyData>()
                };

                // Iterate through the rows
                foreach(var dtl in page.Details.OfType<OmrRowData>())
                {

                    OmrBubbleData ageData = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "Age"),
                        genderData = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "Gender"),
                        weightData = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "Weight"),
                        ebfData = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "EBF"),
                        rfData = dtl.Details.OfType<OmrBubbleData>().FirstOrDefault(o => o.Key == "RF");

                    if (ageData == null)
                    {
                        Err += String.Format("Row {0} is missing age group, skipping; ", dtl.Id);
                        continue;
                    }
                    else if (weightData == null)
                    {
                        Err += String.Format("Row {0} is missing weight group, skipping; ", dtl.Id);
                        continue;
                    }
                    else if (genderData == null)
                    {
                        Err += String.Format("Row {0} is missing gender group, skipping; ", dtl.Id);
                        continue;
                    }

                    bool ebf = ebfData != null;
                    bool rf = rfData != null;

                    submission.Data.Add(new WeighTallyData()
                    {
                        AgeGroup = ageData.Value,
                        Gender = genderData.Value,
                        WeightGroup = weightData.Value,
                        Ebf = ebf,
                        Rf = rf
                    });
                }

                RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
                restUtil.Post("AnonymousDataManagement.svc/PostWeighTally", submission);
            }
            catch(Exception e)
            {
                Trace.TraceError("Error:{0}",e);
                throw;
            }

        }

    }
}
