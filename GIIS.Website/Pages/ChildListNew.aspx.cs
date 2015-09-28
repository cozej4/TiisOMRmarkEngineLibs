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
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Pages_ChildListNew : System.Web.UI.Page
{
    //static String birthplaceId;
    //static String healthFacilityId;
    //static String communityId;
    //static String domicileId;

    //protected void Places_ValueSelected(object sender, System.EventArgs e)
    //{
    //    //birthplaceId = txtBirthplaceId.SelectedItemID.ToString();
    //    Session["_birthplace"] = ddlBirthplace.SelectedItem.ToString();
    //}
    protected void ddlBirthplace_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["_birthplace"] = ddlBirthplace.SelectedValue.ToString();
    }
    protected void Domicile_ValueSelected(object sender, System.EventArgs e)
    {
        // domicileId = txtDomicileId.SelectedItemID.ToString();
        Session["_domicile"] = txtDomicileId.SelectedItemID.ToString();
    }
    protected void Communities_ValueSelected(object sender, System.EventArgs e)
    {
        Session["_community"] = txtCommunityId.SelectedItemID.ToString();

    }
    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        Session["_hfChildSearch"] = txtHealthcenterId.SelectedItemID.ToString();
    }

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

            if ((actionList != null) && actionList.Contains("ViewChildList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Child-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Child");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Child-dictionary" + language, wtList);
                }

                #region Person Configuration

                List<PersonConfiguration> pcList = PersonConfiguration.GetPersonConfigurationList();

                foreach (PersonConfiguration pc in pcList)
                {
                    if (pc.IsVisible == false)
                    {
                        Control lbl = FindMyControl(this, "lbl" + pc.ColumnName);
                        Control txt = FindMyControl(this, "txt" + pc.ColumnName);
                        //Control rbl = FindMyControl(this, "rbl" + pc.ColumnName);
                        Control tr = FindMyControl(this, "tr" + pc.ColumnName);

                        if (lbl != null)
                            lbl.Visible = false;

                        if (txt != null)
                            txt.Visible = false;

                        //if (rbl != null)
                        //    rbl.Visible = false;

                        if (tr != null)
                            tr.Visible = false;

                    }
                }
                string id1 = Configuration.GetConfigurationByName("IdentificationNo1").Value;
                string id2 = Configuration.GetConfigurationByName("IdentificationNo2").Value;
                string id3 = Configuration.GetConfigurationByName("IdentificationNo3").Value;
                #endregion

                //controls
                #region Controls

                this.lblSystemId.Text = wtList["ChildSystem"];
                this.lblBarcode.Text = wtList["BarcodeId"];
                this.lblTempId.Text = wtList["TempId"];
                this.lblFirstname1.Text = wtList["ChildFirstname1"];
                this.lblLastname1.Text = wtList["ChildLastname1"];
                this.lblMotherFirstname.Text = wtList["ChildMotherFirstname"];
                this.lblMotherLastname.Text = wtList["ChildMotherLastname"];
                this.lblBirthdateFrom.Text = wtList["ChildBirthdateFrom"];
                this.lblBirthdateTo.Text = wtList["ChildBirthdateTo"];
                this.lblHealthcenterId.Text = wtList["ChildHealthcenter"];
                this.lblBirthplaceId.Text = wtList["ChildBirthplace"];
                this.lblCommunityId.Text = wtList["ChildCommunity"];
                this.lblDomicileId.Text = wtList["ChildDomicile"];
                this.lblStatusId.Text = wtList["ChildStatus"];


                #endregion

               
                //buttons
                this.btnSearch.Text = wtList["ChildSearchButton"];
                //this.btnExcel.Text = wtList["ChildExcelButton"];

                //Page Title
                this.lblTitle.Text = wtList["ChildListNewPageTitle"];

                //message
                this.lblWarning.Text = wtList["ChildSearchWarningText"];

                //validators
                ConfigurationDate cd = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceBirthdateFrom.Format = cd.DateFormat;
                ceBirthdateTo.Format = cd.DateFormat;
                revBirthdateFrom.ErrorMessage = cd.DateFormat;
                revBirthdateTo.ErrorMessage = cd.DateFormat;
                revBirthdateFrom.ValidationExpression = cd.DateExpresion;
                revBirthdateTo.ValidationExpression = cd.DateExpresion;

                ceBirthdateFrom.EndDate = DateTime.Today.Date;
                ceBirthdateTo.EndDate = DateTime.Today.Date;
               
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        
    }
    private static Control FindMyControl(Control Root, string Id)
    {
        if (Id == "txtIsActive")
            return null;

        if (Root.ID == Id)
            return Root;

        foreach (Control Ctl in Root.Controls)
        {
            Control FoundCtl = FindMyControl(Ctl, Id);
            if (FoundCtl != null)
                return FoundCtl;
        }

        return null;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DateTime dateto = new DateTime();
        DateTime datefrom = new DateTime();
        string format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();
        if (!string.IsNullOrEmpty(txtBirthdateFrom.Text))
            datefrom = DateTime.ParseExact(txtBirthdateFrom.Text, format, CultureInfo.CurrentCulture);
        if (!string.IsNullOrEmpty(txtBirthdateTo.Text))
            dateto = DateTime.ParseExact(txtBirthdateTo.Text, format, CultureInfo.CurrentCulture);
        string s = string.Empty, hf = string.Empty;
        if (Session["_hfChildSearch"] != null)
            hf = Session["_hfChildSearch"].ToString();
        if (!string.IsNullOrEmpty(hf) && hf != "1")        
            s = HealthFacility.GetAllChildsForOneHealthFacility(int.Parse(hf));

        Session["_systemIdChildListNew"] = txtSystemId.Text.Replace("'", @"''").ToUpper();
        Session["_idFieldsChildListNew"] = txtIdFields.Text.Replace("'", @"''").ToUpper();
        Session["_barcodeIdChildListNew"] = txtBarcode.Text.Replace("'", @"''").ToUpper();
        Session["_tempIdChildListNew"] = txtTempId.Text.Replace("'", @"''").ToUpper();
        Session["_firstname1ChildListNew"] = txtFirstname1.Text.Replace("'", @"''").ToUpper();
        Session["_lastname1ChildListNew"] = txtLastname1.Text.Replace("'", @"''").ToUpper();
        Session["_motherfirstname1ChildListNew"] = txtMotherFirstname.Text.Replace("'", @"''").ToUpper();
        Session["_motherlastname1ChildListNew"] = txtMotherLastname.Text.Replace("'", @"''").ToUpper();
        Session["_birthdateFromChildListNew"] = datefrom.ToString("yyyy-MM-dd");
        Session["_birthdateToChildListNew"] = dateto.ToString("yyyy-MM-dd");
        Session["_healthCenterIdChildListNew"] = s.ToString();
        if (ddlBirthplace.SelectedIndex > 0)
            Session["_birthplace"] = ddlBirthplace.SelectedValue.ToString();
        Session["_statusChildListNew"] = ddlStatus.SelectedValue;

        if (HttpContext.Current.Session["_barcodeIdChildListNew"] != null )
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Session["_barcodeIdChildListNew"].ToString()))
            {
                //redirect to child page
                Child child = Child.GetChildByBarcode(HttpContext.Current.Session["_barcodeIdChildListNew"].ToString());
                if (child != null)
                {
                    ClearSession();
                    Response.Redirect(string.Format("Child.aspx?id={0}", child.Id));                    
                }

            }
        }

        Response.Redirect("ChildListNewSearch.aspx");
    }

    protected void ClearSession() 
    {
        Session["_systemIdChildListNew"] = null;
        Session["_idFieldsChildListNew"] = null;
        Session["_barcodeIdChildListNew"] = null;
        Session["_tempIdChildListNew"] = null;
        Session["_firstname1ChildListNew"] = null;
        Session["_lastname1ChildListNew"] = null;
        Session["_motherfirstname1ChildListNew"] = null;
        Session["_motherlastname1ChildListNew"] = null;
        Session["_birthdateFromChildListNew"] = null;
        Session["_birthdateToChildListNew"] = null;
        Session["_healthCenterIdChildListNew"] = null;
        Session["_birthplace"] = null;
        Session["_domicile"] = null;
        Session["_community"] = null;
        Session["_statusChildListNew"] = null;
    }
    
    protected void ValidateBirthdate(object sender, ServerValidateEventArgs e)
    {
        ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));

        if (!string.IsNullOrEmpty(txtBirthdateFrom.Text) && !string.IsNullOrEmpty(txtBirthdateTo.Text))
        {
            DateTime datefrom = DateTime.ParseExact(txtBirthdateFrom.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);
            DateTime dateto = DateTime.ParseExact(txtBirthdateTo.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);

            e.IsValid = datefrom < dateto;
        }
    }

    
}