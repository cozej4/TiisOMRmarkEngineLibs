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

public partial class _SearchImmunizationCard : System.Web.UI.Page
{
    //static String birthplaceId;
    //static String healthFacilityId;
    //static String communityId;
    //static String domicileId;

    //protected void Places_ValueSelected(object sender, System.EventArgs e)
    //{
    //    //birthplaceId = txtBirthplaceId.SelectedItemID.ToString();
    //    Session["birthplaceImm"] = txtBirthplaceId.SelectedItemID.ToString();
    //}
    protected void ddlBirthplace_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["birthplaceImm"] = ddlBirthplace.SelectedValue.ToString();
    }
    protected void Domicile_ValueSelected(object sender, System.EventArgs e)
    {
        // domicileId = txtDomicileId.SelectedItemID.ToString();
        Session["domicileImm"] = txtDomicileId.SelectedItemID.ToString();
    }
    protected void Communities_ValueSelected(object sender, System.EventArgs e)
    {
        Session["communityImm"] = txtCommunityId.SelectedItemID.ToString();

    }
    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        Session["healthfacilityImm"] = txtHealthcenterId.SelectedItemID.ToString();
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
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Child-dictionary1"];
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

                        for (int i = 1; i < gvChild.Columns.Count; i++)
                        {
                            if (gvChild.Columns[i].HeaderText == pc.ColumnName)
                            {
                                gvChild.Columns[i].Visible = false;
                                break;
                            }
                        }
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

                //grid header text
                #region Grid Columns
                gvChild.Columns[1].HeaderText = wtList["ChildSystem"];
                gvChild.Columns[2].HeaderText = wtList["ChildFirstname1"];
                gvChild.Columns[3].HeaderText = wtList["ChildFirstname2"];
                gvChild.Columns[4].HeaderText = wtList["ChildLastname1"];
                gvChild.Columns[5].HeaderText = wtList["ChildLastname2"];
                gvChild.Columns[6].HeaderText = wtList["ChildBirthdate"];
                gvChild.Columns[7].HeaderText = wtList["ChildGender"];
                gvChild.Columns[8].HeaderText = wtList["ChildHealthcenter"];
                gvChild.Columns[9].HeaderText = wtList["ChildBirthplace"];
                gvChild.Columns[10].HeaderText = wtList["ChildCommunity"];
                gvChild.Columns[11].HeaderText = wtList["ChildDomicile"];
                gvChild.Columns[12].HeaderText = wtList["ChildStatus"];
                gvChild.Columns[13].HeaderText = wtList["ChildAddress"];
                gvChild.Columns[14].HeaderText = wtList["ChildPhone"];
                gvChild.Columns[15].HeaderText = wtList["ChildMobile"];
                gvChild.Columns[16].HeaderText = wtList["ChildEmail"];
                gvChild.Columns[17].HeaderText = wtList["ChildMother"];
                gvChild.Columns[18].HeaderText = wtList["ChildMotherFirstname"];
                gvChild.Columns[19].HeaderText = wtList["ChildMotherLastname"];
                gvChild.Columns[20].HeaderText = wtList["ChildFather"];
                gvChild.Columns[21].HeaderText = wtList["ChildFatherFirstname"];
                gvChild.Columns[22].HeaderText = wtList["ChildFatherLastname"];
                gvChild.Columns[23].HeaderText = wtList["ChildCaretaker"];
                gvChild.Columns[24].HeaderText = wtList["ChildCaretakerFirstname"];
                gvChild.Columns[25].HeaderText = wtList["ChildCaretakerLastname"];
                gvChild.Columns[26].HeaderText = wtList["ChildNotes"];
                gvChild.Columns[27].HeaderText = wtList["ChildIsActive"];
                gvChild.Columns[30].HeaderText = id1;
                gvChild.Columns[31].HeaderText = id2;
                gvChild.Columns[32].HeaderText = id3;

              
                #endregion

                //buttons
                this.btnSearch.Text = wtList["ChildSearchButton"];

                //message
                //this.lblSuccess.Text = wtList["ChildSuccessText"];
                this.lblWarning.Text = wtList["ImmunizationCardWarningSearchText"];
                //this.lblError.Text = wtList["ChildErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["SearchImmunizationCardPageTitle"];

                //validators
                ConfigurationDate cd = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceBirthdateFrom.Format = cd.DateFormat;
                ceBirthdateTo.Format = cd.DateFormat;
                revBirthdateFrom.ErrorMessage = cd.DateFormat;
                revBirthdateTo.ErrorMessage = cd.DateFormat;
                revBirthdateFrom.ValidationExpression = cd.DateExpresion;
                revBirthdateTo.ValidationExpression = cd.DateExpresion;

                odsChild.SelectParameters.Clear();

                if (HttpContext.Current.Session["statusChildListNew"] != null)
                    odsChild.SelectParameters.Add("statusId", HttpContext.Current.Session["statusChildListNew"].ToString());
                if (HttpContext.Current.Session["birthdateFromChildListNew"] != null)
                    odsChild.SelectParameters.Add("birthdateFrom", HttpContext.Current.Session["birthdateFromChildListNew"].ToString());
                if (HttpContext.Current.Session["birthdateToChildListNew"] != null)
                    odsChild.SelectParameters.Add("birthdateTo", HttpContext.Current.Session["birthdateToChildListNew"].ToString());
                if (HttpContext.Current.Session["firstname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("firstname1", HttpContext.Current.Session["firstname1ChildListNew"].ToString());
                if (HttpContext.Current.Session["lastname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("lastname1", HttpContext.Current.Session["lastname1ChildListNew"].ToString());

                if (HttpContext.Current.Session["motherfirstname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("motherFirstname", HttpContext.Current.Session["motherfirstname1ChildListNew"].ToString());
                if (HttpContext.Current.Session["motherlastname1ChildListNew"] != null)
                    odsChild.SelectParameters.Add("motherLastname", HttpContext.Current.Session["motherlastname1ChildListNew"].ToString());

                if (HttpContext.Current.Session["idFieldsChildListNew"] != null)
                    odsChild.SelectParameters.Add("idFields", HttpContext.Current.Session["idFieldsChildListNew"].ToString());

                if (HttpContext.Current.Session["systemIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("systemId", HttpContext.Current.Session["systemIdChildListNew"].ToString());
                if (HttpContext.Current.Session["barcodeIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("barcodeId", HttpContext.Current.Session["barcodeIdChildListNew"].ToString());
                if (HttpContext.Current.Session["tempIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("tempId", HttpContext.Current.Session["tempIdChildListNew"].ToString());

                if (HttpContext.Current.Session["healthCenterIdChildListNew"] != null)
                    odsChild.SelectParameters.Add("healthFacilityId", HttpContext.Current.Session["healthCenterIdChildListNew"].ToString());

                if (HttpContext.Current.Session["birthplaceImm"] != null)
                    odsChild.SelectParameters.Add("birthplaceId", HttpContext.Current.Session["birthplaceImm"].ToString());
                else odsChild.SelectParameters.Add("birthplaceId", "");
                if (HttpContext.Current.Session["communityImm"] != null)
                    odsChild.SelectParameters.Add("communityId", HttpContext.Current.Session["communityImm"].ToString());
                else odsChild.SelectParameters.Add("communityId", "");
                if (HttpContext.Current.Session["domicileImm"] != null)
                    odsChild.SelectParameters.Add("domicileId", HttpContext.Current.Session["domicileImm"].ToString());
                else odsChild.SelectParameters.Add("domicileId", "");

                if (odsChild.SelectParameters.Count > 3)
                {
                    gvChild.DataSourceID = "odsChild";
                    gvChild.DataBind();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void gvChild_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvChild.PageIndex = e.NewPageIndex;
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
        string IdFsearch = txtIdFields.Text.Replace("'", @"''").ToUpper();
        string systemId = txtSystemId.Text.Replace("'", @"''").ToUpper();
        string barcodeId = txtBarcode.Text.Replace("'", @"''").ToUpper();
        string tempId = txtTempId.Text.Replace("'", @"''").ToUpper();
        string FtNsearch = txtFirstname1.Text.Replace("'", @"''").ToUpper();
        string LtNsearch = txtLastname1.Text.Replace("'", @"''").ToUpper();
        string MFtNsearch = txtMotherFirstname.Text.Replace("'", @"''").ToUpper();
        string MLtNsearch = txtMotherLastname.Text.Replace("'", @"''").ToUpper();
        string Ssearch = ddlStatus.SelectedValue;

        DateTime dateto = new DateTime();
        DateTime datefrom = new DateTime();

        string format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();

        if (!string.IsNullOrEmpty(txtBirthdateFrom.Text))
            datefrom = DateTime.ParseExact(txtBirthdateFrom.Text, format, CultureInfo.CurrentCulture);
        if (!string.IsNullOrEmpty(txtBirthdateTo.Text))
            dateto = DateTime.ParseExact(txtBirthdateTo.Text, format, CultureInfo.CurrentCulture);

        string s = string.Empty;
        string hf = string.Empty;
        if (Session["healthfacilityImm"] != null)
            hf = Session["healthfacilityImm"].ToString();

        if (!string.IsNullOrEmpty(hf) && hf != "1")
        {
            s = HealthFacility.GetAllChildsForOneHealthFacility(int.Parse(hf));
        }

        Session["systemIdChildListNew"] = systemId;
        Session["barcodeIdChildListNew"] = barcodeId;
        Session["tempIdChildListNew"] = tempId;

        Session["idFieldsChildListNew"] = IdFsearch;
        Session["firstname1ChildListNew"] = FtNsearch;
        Session["lastname1ChildListNew"] = LtNsearch;
        Session["motherfirstname1ChildListNew"] = MFtNsearch;
        Session["motherlastname1ChildListNew"] = MLtNsearch;
        Session["birthdateFromChildListNew"] = datefrom.ToString("yyyy-MM-dd");
        Session["birthdateToChildListNew"] = dateto.ToString("yyyy-MM-dd");
        Session["statusChildListNew"] = Ssearch;
        Session["healthCenterIdChildListNew"] = s.ToString();

        odsChild.SelectParameters.Clear();
        odsChild.SelectParameters.Add("statusId", Ssearch);
        odsChild.SelectParameters.Add("birthdateFrom", datefrom.ToString("yyyy-MM-dd"));
        odsChild.SelectParameters.Add("birthdateTo", dateto.ToString("yyyy-MM-dd"));
        odsChild.SelectParameters.Add("firstname1", FtNsearch);
        odsChild.SelectParameters.Add("lastname1", LtNsearch);
        odsChild.SelectParameters.Add("motherFirstname", MFtNsearch);
        odsChild.SelectParameters.Add("motherLastname", MLtNsearch);
        odsChild.SelectParameters.Add("idFields", IdFsearch);
        odsChild.SelectParameters.Add("systemId", systemId);
        odsChild.SelectParameters.Add("barcodeId", barcodeId);
        odsChild.SelectParameters.Add("tempId", tempId);
        odsChild.SelectParameters.Add("healthFacilityId", s);
        if (Session["birthplaceImm"] != null)
            odsChild.SelectParameters.Add("birthplaceId", Session["birthplaceImm"].ToString());
        else
            odsChild.SelectParameters.Add("birthplaceId", string.Empty);
        if (Session["communityImm"] != null)
            odsChild.SelectParameters.Add("communityId", Session["communityImm"].ToString());
        else
            odsChild.SelectParameters.Add("communityId", string.Empty);
        if (Session["domicileImm"] != null)
            odsChild.SelectParameters.Add("domicileId", Session["domicileImm"].ToString());
        else
            odsChild.SelectParameters.Add("domicileId", string.Empty);

        //gvChild.DataSourceID = "odsChild";
        //gvChild.DataBind();
        Response.Redirect(Request.RawUrl);
    }

    protected void gvChild_DataBound(object sender, System.EventArgs e)
    {
        //string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        //((BoundField)gvChild.Columns[6]).DataFormatString = "{0:" + dateformat + "}";
        //((BoundField)gvExport.Columns[6]).DataFormatString = "{0:" + dateformat + "}";

        if (gvChild.Rows.Count <= 0)
        {
            lblWarning.Visible = true;
        }
        else
        {
            lblWarning.Visible = false;
        }
    }

}