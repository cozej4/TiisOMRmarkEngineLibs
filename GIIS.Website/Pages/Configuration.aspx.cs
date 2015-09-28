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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Configuration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.Page.IsPostBack)
        {
            List<string> actionList = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
            }

            if ((actionList != null) && actionList.Contains("ViewConfiguration") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Configuration-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Configuration");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Configuration-dictionary" + language, wtList);
                }

                //controls
                this.lblCountry.Text = wtList["ConfigurationCountry"];
                this.lblLanguage.Text = wtList["ConfigurationLanguage"];
                this.lblDateFormat.Text = wtList["ConfigurationDateFormat"];
                this.lblCurrency.Text = wtList["ConfigurationCurrency"];
                this.lblHomePageText.Text = wtList["ConfigurationHomePageText"];
                this.lblSupplyChain.Text = wtList["ConfigurationSupplyChainLevels"];
                this.lblWarningDays.Text = wtList["ConfigurationWarningDays"];
                //this.lblEligible.Text = wtList["ConfigurationEligible"];
                //this.lblDefaulters.Text = wtList["ConfigurationDefaulters"];

                //actions
                this.btnSave.Visible = actionList.Contains("SaveConfiguration");

                //buttons
                this.btnSave.Text = wtList["ConfigurationSaveButton"];

                //message
                this.lblSuccess.Text = wtList["ConfigurationSuccessText"];
                this.lblWarning.Text = wtList["ConfigurationWarningText"];
                this.lblError.Text = wtList["ConfigurationErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["ConfigurationPageTitle"];

                //validators
                revCurrency.ErrorMessage = wtList["ConfigurationCurrencyValidator"];
                revSupplyChainLevels.ErrorMessage = wtList["ConfigurationSupplyChainLevelsValidator"];
                revWarningDays.ErrorMessage = wtList["ConfigurationWarningDaysValidator"];

                ddlCountry.SelectedValue = Configuration.GetConfigurationByName("Country").Value;
                ddlLanguage.SelectedValue = Configuration.GetConfigurationByName("Language").Value;
                ddlDateFormat.SelectedValue = Configuration.GetConfigurationByName("DateFormat").Value;
                txtCurrency.Text = Configuration.GetConfigurationByName("Currency").Value;
                txtHomePageText.Text = Configuration.GetConfigurationByName("HomePageText").Value;
                txtSupplyChain.Text = Configuration.GetConfigurationByName("SupplyChainLevels").Value;
                txtWarningDays.Text = Configuration.GetConfigurationByName("DefaultWarningDays").Value;
                txtEligible.Text = Configuration.GetConfigurationByName("EligibleForVaccination").Value;
                txtDefaulters.Text = Configuration.GetConfigurationByName("Defaulters").Value;

                string link = Configuration.GetConfigurationByName("LinkImmunizationwithStockManagement").Value;
                if (link == "1")
                    rblLink.SelectedIndex = 0;
                else
                    rblLink.SelectedIndex = 1;
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                string country = ddlCountry.SelectedValue;
                string language = ddlLanguage.SelectedValue;
                string dateformat = ddlDateFormat.SelectedValue;
                string currecy = txtCurrency.Text.Replace("'", @"''");
                string homepage = txtHomePageText.Text.Replace("'", @"''");
                string supplyChainLevels = txtSupplyChain.Text;
                string warningDays = txtWarningDays.Text;
                string eligible = txtEligible.Text;
                string defaulters = txtDefaulters.Text;

                string prevcountry = Configuration.GetConfigurationByName("Country").Value;

                int i = 0;
                Configuration o = new Configuration();
                o.Name = "Country";
                o.Value = country;
                i = Configuration.UpdateValues(o);
                if (country != "0" && prevcountry == "0")
                {
                    HealthFacility hf = HealthFacility.GetHealthFacilityById(1);
                    if (hf != null)
                    {
                        hf.Name = Country.GetCountryById(int.Parse(country)).Name;
                        HealthFacility.Update(hf);
                    }
                    if (Place.GetPlaceByParentId(0) == null)
                    {
                        Place p = new Place();
                        p.Name = Country.GetCountryById(int.Parse(country)).Name;
                        p.ParentId = 0;
                        p.Leaf = false;
                        p.IsActive = true;
                        p.Notes = "";
                        p.ModifiedBy = 1;
                        p.ModifiedOn = DateTime.Today.Date;
                        Place.Insert(p);
                    }
                }

                o.Name = "Language";
                o.Value = language;
                i = Configuration.UpdateValues(o);

                o.Name = "DateFormat";
                o.Value = dateformat;
                i = Configuration.UpdateValues(o);

                o.Name = "Currency";
                o.Value = currecy;
                i = Configuration.UpdateValues(o);

                o.Name = "HomePageText";
                o.Value = homepage;
                i = Configuration.UpdateValues(o);

                o.Name = "SupplyChainLevels";
                o.Value = supplyChainLevels;
                i = Configuration.UpdateValues(o);

                o.Name = "DefaultWarningDays";
                o.Value = warningDays;
                i = Configuration.UpdateValues(o);

                o.Name = "LinkImmunizationwithStockManagement";
                if (rblLink.SelectedIndex == 0)
                    o.Value = "1";
                else
                    o.Value = "0";
                i = Configuration.UpdateValues(o);

                o.Name = "EligibleForVaccination";
                o.Value = eligible;
                i = Configuration.UpdateValues(o);

                o.Name = "Defaulters";
                o.Value = defaulters;
                i = Configuration.UpdateValues(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }


  
}
