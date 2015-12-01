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
    public class FormBID03
    {

        public String Err { get; set; }

        public void UploadData(OmrPageOutput page)
        {
            try
            {
                this.Err = "";
                var restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
                Trace.TraceInformation("Need to sync {0} children with TIIS at {1}", page.Details.Count, ConfigurationManager.AppSettings["GIIS_URL"]);

                foreach (var dtl in page.Details.OfType<OmrRowData>())
                {
                    // Get the barcode information for this child
                    var barcodes = dtl.Details.OfType<OmrBarcodeData>();
                    if (barcodes.Count() == 0)
                        throw new InvalidOperationException("No barcode found on row!");

                    // Child code
                    var childCode = barcodes.FirstOrDefault(o => o.Id == dtl.Id + "_TempId");

                    if (childCode == null)
                        throw new InvalidOperationException("No GIIS TempId Found on Row!");

                    GIIS.DataLayer.Child childData = null;

                    // Get the child by barcode id
                    var childDataList = restUtil.Get<List<ChildEntity>>("ChildManagement.svc/GetChildById",
                        new KeyValuePair<string, object>("childId", childCode.BarcodeData.Replace("T", ""))
                    );
                    if (childDataList == null)
                        throw new InvalidOperationException("Could not deserialize response");
                    else if (childDataList.Count == 0)
                    {
                        throw new InvalidOperationException("Child with id " + childCode.BarcodeData + " not found!");
                    }
                    childData = childDataList[0].childEntity;

                    var permCode = barcodes.FirstOrDefault(o => o.Id == dtl.Id + "_Barcode");

                    // Permcode == null = no association
                    if (permCode == null)
                    {
                        Err += String.Format("{0}, {1} will not be associated with a barcode sticker; ", childData.Lastname1, childData.Firstname1);
                        continue;
                    }

                    // Now to update the child
                    Trace.TraceInformation("Will associated child id {0} with barcode {1}", childData.Id, permCode.BarcodeData);

                    childData.BarcodeId = permCode.BarcodeData;

                    // Send the update
                    restUtil.Get<RestReturn>("ChildManagement.svc/UpdateChild",
                        new KeyValuePair<string, object>("barcode", childData.BarcodeId),
                        new KeyValuePair<string, object>("firstname1", childData.Firstname1),
                        new KeyValuePair<string, object>("lastname1", childData.Lastname1),
                        new KeyValuePair<string, object>("birthdate", childData.Birthdate.ToString("yyyy-MM-dd")),
                        new KeyValuePair<string, object>("gender", childData.Gender),
                        new KeyValuePair<string, object>("healthFacilityId", childData.HealthcenterId),
                        new KeyValuePair<string, object>("birthplaceId", childData.BirthplaceId),
                        new KeyValuePair<string, object>("domicileId", childData.DomicileId),
                        new KeyValuePair<string, object>("statusId", childData.StatusId),
                        new KeyValuePair<string, object>("address", childData.Address),
                        new KeyValuePair<string, object>("phone", childData.Phone),
                        new KeyValuePair<string, object>("motherFirstname", childData.MotherFirstname),
                        new KeyValuePair<string, object>("motherLastname", childData.MotherLastname),
                        new KeyValuePair<string, object>("notes", "Updated by form scanning application"),
                        new KeyValuePair<string, object>("userId", 1),
                        new KeyValuePair<string, object>("childId", childData.Id)
                    );
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }

        }

        public class ChildEntity
        {

            public GIIS.DataLayer.Child childEntity { get; set; }
        }
    }
}
