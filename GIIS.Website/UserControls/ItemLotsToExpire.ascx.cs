//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
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