using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ItemLotsToExpire : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser != null)
        {
            string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
            int limit = int.Parse(Configuration.GetConfigurationByName("DefaultWarningDays").Value);

            int healthFacilityId = CurrentEnvironment.LoggedUser.HealthFacilityId;
            string dt = DateTime.Today.AddDays(limit).ToString("yyyy-MM-dd");

            //List<ItemLot> itemLotList = ItemLot.GetItemLotListBeforeExpire(healthFacilityId, dt);

            //Literal l = new Literal();
            //l.Text = "<ul>";

            //foreach (ItemLot o in itemLotList)
            //{
            //    l.Text += string.Format("<li> {1} - {2}</li>", o.Id, o.ItemObject.Name + "-" + o.LotNumber, o.ExpireDate.ToString(dateformat));
            //}
            
            //l.Text += "</ul>";

            //pnlItemLots.Controls.Add(l);
        }
        else
            pnlItemLots.Visible = false;
    }
}