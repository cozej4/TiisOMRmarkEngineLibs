using OmrMarkEngine.Template.Scripting.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIIS.DataLayer;

namespace GIIS.ScanForms.UserInterface
{
    public class ReferenceData
    {

        private static object s_lockObject = new object();
        private static ReferenceData s_instance;
        
        private ReferenceData()
        {
            RestUtil restUtil = new RestUtil(new Uri(ConfigurationManager.AppSettings["GIIS_URL"]));
            if (this.Doses == null)
                this.Doses = restUtil.Get<List<GIIS.DataLayer.Dose>>("DoseManagement.svc/GetDoseList");
            if (this.Vaccines == null)
                this.Vaccines = restUtil.Get<List<GIIS.DataLayer.ScheduledVaccination>>("ScheduledVaccinationManagement.svc/GetScheduledVaccinationList");
            if (this.Items == null)
                this.Items = restUtil.Get<List<GIIS.DataLayer.Item>>("ItemManagement.svc/GetItemList");
            if (this.ItemLots == null)
                this.ItemLots = restUtil.Get<List<GIIS.DataLayer.ItemLot>>("StockManagement.svc/GetItemLots");
            if(this.Places == null)
                this.Places = restUtil.Get<List<GIIS.DataLayer.Place>>("PlaceManagement.svc/GetLeafPlaces");

        }

        public static ReferenceData Current
        {
            get
            {
                if(s_instance == null)
                {
                    lock(s_lockObject)
                        s_instance = new ReferenceData();
                }
                return s_instance;
            }
        }

        public List<Dose> Doses { get; private set; }
        public List<ScheduledVaccination> Vaccines { get; private set; }
        public List<Item> Items { get; private set; }
        public List<ItemLot> ItemLots { get; private set; }
        public List<Place> Places { get; private set; }
    }
}
