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

public partial class _ChildList : System.Web.UI.Page
{
    static String birthplaceId;
    static String healthFacilityId;
    static String communityId;
    static String domicileId;

    protected void Places_ValueSelected(object sender, System.EventArgs e)
    {
        birthplaceId = txtBirthplaceId.SelectedItemID.ToString();
    }
    protected void Domicile_ValueSelected(object sender, System.EventArgs e)
    {
        domicileId = txtDomicileId.SelectedItemID.ToString();
    }
    protected void Communities_ValueSelected(object sender, System.EventArgs e)
    {
        communityId = txtCommunityId.SelectedItemID.ToString();
    }
    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        healthFacilityId = txtHealthcenterId.SelectedItemID.ToString();
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
                            if (i == 26)
                                gvChild.Columns[i].Visible = false;
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
                this.lblFirstname1.Text = wtList["ChildFirstname1"];
                this.lblFirstname2.Text = wtList["ChildFirstname2"];
                this.lblLastname1.Text = wtList["ChildLastname1"];
                this.lblLastname2.Text = wtList["ChildLastname2"];
                this.lblBirthdateFrom.Text = wtList["ChildBirthdateFrom"];
                this.lblBirthdateTo.Text = wtList["ChildBirthdateTo"];
                this.lblHealthcenterId.Text = wtList["ChildHealthcenter"];
                this.lblBirthplaceId.Text = wtList["ChildBirthplace"];
                this.lblCommunityId.Text = wtList["ChildCommunity"];
                this.lblDomicileId.Text = wtList["ChildDomicile"];
                this.lblStatusId.Text = wtList["ChildStatus"];
                this.lblAddress.Text = wtList["ChildAddress"];
                this.lblPhone.Text = wtList["ChildPhone"];
                this.lblMobile.Text = wtList["ChildMobile"];
                this.lblEmail.Text = wtList["ChildEmail"];
                this.lblMotherId.Text = wtList["ChildMother"];
                this.lblMotherFirstname.Text = wtList["ChildMotherFirstname"];
                this.lblMotherLastname.Text = wtList["ChildMotherLastname"];
                this.lblFatherId.Text = wtList["ChildFather"];
                this.lblFatherFirstname.Text = wtList["ChildFatherFirstname"];
                this.lblFatherLastname.Text = wtList["ChildFatherLastname"];
                this.lblCaretakerId.Text = wtList["ChildCaretaker"];
                this.lblCaretakerFirstname.Text = wtList["ChildCaretakerFirstname"];
                this.lblCaretakerLastname.Text = wtList["ChildCaretakerLastname"];
                this.lblNotes.Text = wtList["ChildNotes"];
                this.lblIsActive.Text = wtList["ChildIsActive"];
                this.lblIdentificationNo1.Text = id1;
                this.lblIdentificationNo2.Text = id2;
                this.lblIdentificationNo3.Text = id3;

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

                gvChild.Columns[26].Visible = false;
                #endregion

                //buttons
                this.btnSearch.Text = wtList["ChildSearchButton"];

                //message
                //this.lblSuccess.Text = wtList["ChildSuccessText"];
                this.lblWarning.Text = wtList["ChildSearchWarningText"];
                //this.lblWarning.Text = wtList["ChildWarningText"];
                //this.lblError.Text = wtList["ChildErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["ChildListPageTitle"];

                //validators
                ConfigurationDate cd = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceBirthdateFrom.Format = cd.DateFormat;
                ceBirthdateTo.Format = cd.DateFormat;
                revBirthdateFrom.ErrorMessage = cd.DateFormat;
                revBirthdateTo.ErrorMessage = cd.DateFormat;
                revBirthdateFrom.ValidationExpression = cd.DateExpresion;
                revBirthdateTo.ValidationExpression = cd.DateExpresion;

                if (HttpContext.Current.Session["_whereChildList"] != null)
                {
                    string where = HttpContext.Current.Session["_whereChildList"].ToString();
                    odsChild.SelectParameters.Clear();
                    odsChild.SelectParameters.Add("where", where);

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
        string where = string.Format(@" ""STATUS_ID"" = {0} ", ddlStatus.SelectedValue);
        DateTime dateto = new DateTime();
        DateTime datefrom = new DateTime();
        string bdcontrol;
        if ((txtBirthdateFrom.Text != String.Empty) && (txtBirthdateTo.Text != String.Empty))
        {
            datefrom = DateTime.ParseExact(txtBirthdateFrom.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            dateto = DateTime.ParseExact(txtBirthdateTo.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            bdcontrol = string.Format(@" AND ""BIRTHDATE"" between '{0}' and '{1}'", datefrom.ToString("yyyy-MM-dd"), dateto.ToString("yyyy-MM-dd"));
        }
        else if (txtBirthdateFrom.Text != String.Empty)
        {
            datefrom = DateTime.ParseExact(txtBirthdateFrom.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            bdcontrol = string.Format(@" AND ""BIRTHDATE"" >= '{0}' ", datefrom.ToString("yyyy-MM-dd"));
        }
        else if (txtBirthdateTo.Text != String.Empty)
        {
            dateto = DateTime.ParseExact(txtBirthdateTo.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            bdcontrol = string.Format(@" AND ""BIRTHDATE"" <= '{0}' ", dateto.ToString("yyyy-MM-dd"));
        }
        else
            bdcontrol = "";
        where += bdcontrol;
        List<PersonConfiguration> pcList = PersonConfiguration.GetPersonConfigurationList();

        foreach (PersonConfiguration pc in pcList)
        {
            if (pc.IsVisible == true)
            {
                Control c = FindMyControl(this, "txt" + pc.ColumnName);
                 string dbColumnName = string.Empty;
                if (pc.ColumnName.Contains("Identification") )
                   dbColumnName = pc.ColumnName.ToUpper().Replace("NO", "_NO");
                else
                dbColumnName  = pc.ColumnName.ToUpper().Replace("ID", "_ID").Replace("ERFIRSTNAME", "ER_FIRSTNAME").Replace("ERLASTNAME", "ER_LASTNAME");
               
                if (dbColumnName.Contains("HEALTHCENTER") && healthFacilityId != null)
                    where += string.Format(@" AND ""HEALTHCENTER_ID"" = {0} ", healthFacilityId);

                if (dbColumnName.Contains("BIRTHPLACE") && birthplaceId != null)
                    where += string.Format(@" AND ""BIRTHPLACE_ID"" = {0} ", birthplaceId);

                if (dbColumnName.Contains("COMMUNITY") && communityId != null)
                    where += string.Format(@" AND ""COMMUNITY_ID"" = {0} ", communityId);

                if (dbColumnName.Contains("DOMICILE") && domicileId != null)
                    where += string.Format(@" AND ""DOMICILE_ID"" = {0} ", domicileId);

                if (c is TextBox)
                    if (!string.IsNullOrEmpty((c as TextBox).Text))
                    {
                        if (dbColumnName.Contains("BIRTHDATE"))
                            where += "";

                        //else if (dbColumnName.Contains("FIRSTNAME") || dbColumnName.Contains("LASTNAME"))
                        //    where += string.Format(@" AND UPPER(""{1}"") LIKE '%{0}%' ", (c as TextBox).Text.ToUpper(), dbColumnName);
                        else
                            where += string.Format(@" AND UPPER(""{1}"") LIKE '%{0}%' ", (c as TextBox).Text.ToUpper(), dbColumnName);
                    }
            }
        }
        HttpContext.Current.Session["_whereChildList"] = where;

        healthFacilityId = null;
        domicileId = null;
        communityId = null;
        birthplaceId = null;

        odsChild.SelectParameters.Clear();
        odsChild.SelectParameters.Add("where", where);

        gvChild.DataSourceID = "odsChild";
        gvChild.DataBind();
        Response.Redirect(Request.RawUrl);
    }
    protected void gvChild_DataBound(object sender, System.EventArgs e)
    {
           string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
            ((BoundField)gvChild.Columns[6]).DataFormatString = "{0:" + dateformat + "}";
            if (gvChild.Rows.Count == 0)
                lblWarning.Visible = true;
            else
                lblWarning.Visible = false;
    }
}