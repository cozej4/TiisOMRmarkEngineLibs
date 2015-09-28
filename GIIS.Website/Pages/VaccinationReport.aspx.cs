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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;

public partial class Pages_VaccinationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            List<int> months = new List<int>();
            for (int i = 1; i <= 12; i++) months.Add(i);

            MonthDropdown.DataSource = months;
            MonthDropdown.DataBind();
            MonthDropdown.SelectedIndex = (DateTime.Now.Month - 1);

            Dictionary<string, int> facilities = new Dictionary<string, int>();
            foreach (HealthFacility h in HealthFacility.GetHealthFacilityList().OrderBy(x => x.Name))
            {
                facilities.Add(h.Name, h.Id);
            }

            HealthFacilityDropdown.DataSource = facilities;
            HealthFacilityDropdown.DataTextField = "key";
            HealthFacilityDropdown.DataValueField = "value";
            HealthFacilityDropdown.DataBind();
        }

        UpdateHyperLink(null, null);
    }

    protected void UpdateHyperLink(object sender,EventArgs e)
    {
        string jasperLocation = System.Configuration.ConfigurationManager.AppSettings["JasperServer"];
        string jasperUsername = System.Configuration.ConfigurationManager.AppSettings["JasperUser"];
        string jasperPassword = System.Configuration.ConfigurationManager.AppSettings["JasperPassword"];

        DownloadLink.NavigateUrl = String.Format("{0}Vaccine_Report_Barcode.pdf?j_username={3}&j_password={4}&Facility={1}&Month={2}", 
            jasperLocation, 
            int.Parse(HealthFacilityDropdown.SelectedValue), 
            int.Parse(MonthDropdown.SelectedValue),
            jasperUsername,
            jasperPassword);

    }

}