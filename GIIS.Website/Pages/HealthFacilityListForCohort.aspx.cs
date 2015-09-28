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

public partial class Pages_HealthFacilityListForCohort : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHealthFacilityList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacility-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacility");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacility-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["HealthFacilityName"];
                this.lblCode.Text = wtList["HealthFacilityCode"];

                //grid header text
                gvHealthFacility.Columns[1].HeaderText = wtList["HealthFacilityName"];
                gvHealthFacility.Columns[2].HeaderText = wtList["HealthFacilityCode"];
                gvHealthFacility.Columns[3].HeaderText = wtList["HealthFacilityParent"];
                gvHealthFacility.Columns[4].HeaderText = wtList["HealthFacilityTopLevel"];
                gvHealthFacility.Columns[5].HeaderText = wtList["HealthFacilityLeaf"];
                gvHealthFacility.Columns[6].HeaderText = wtList["HealthFacilityVaccinationPoint"];
               // gvHealthFacility.Columns[7].HeaderText = wtList["HealthFacilityListViewRegister"];
              

                //actions
               // this.btnSearch.Visible = actionList.Contains("SearchHealthFacility");

                //buttons
                this.btnSearch.Text = wtList["HealthFacilitySearchButton"];

                //message
                this.lblWarning.Text = wtList["HealthFacilitySearchWarningText"];

                //Page Title
                //this.lblTitle.Text = wtList["HealthFacilityListPageTitle"];

                //gridview databind
                string sessionvar = "_healthfacility_" + CurrentEnvironment.LoggedUser.HealthFacilityId.ToString();
                string s;
                if (Session[sessionvar] != null)
                    s = Session[sessionvar].ToString();
                else
                {
                    s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId);
                    Session[sessionvar] = s;
                }
                odsHealthFacility.SelectParameters.Clear();
                odsHealthFacility.SelectParameters.Add("name", "");
                odsHealthFacility.SelectParameters.Add("code", "");
                odsHealthFacility.SelectParameters.Add("hfid", s);
                odsHealthFacility.DataBind();
                gvHealthFacility.DataSourceID = "odsHealthFacility";
                gvHealthFacility.DataBind();

                int count = gvHealthFacility.Rows.Count;
                if (count == 1)
                {
                    Response.Redirect("CohortCoverageReport.aspx?hfId=" + CurrentEnvironment.LoggedUser.HealthFacilityId);
                }
            }
            else
            {
                
                Response.Redirect("Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string wsearch = txtName.Text.Replace("'", @"''");
        string csearch = txtCode.Text.Replace("'", @"''");

        string s = string.Empty;

        string sessionvar = "_healthfacility_" + CurrentEnvironment.LoggedUser.HealthFacilityId.ToString();
        if (Session[sessionvar] != null)
            s = Session[sessionvar].ToString();
        else
            s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId);

        odsHealthFacility.SelectParameters.Clear();
        odsHealthFacility.SelectParameters.Add("name", wsearch.ToUpper());
        odsHealthFacility.SelectParameters.Add("code", csearch.ToUpper());
        odsHealthFacility.SelectParameters.Add("hfid", s);

        gvHealthFacility.DataSourceID = "odsHealthFacility";
        gvHealthFacility.DataBind();
    }
    protected void gvHealthFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacility.PageIndex = e.NewPageIndex;
    }

    protected void gvHealthFacility_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacility.Rows.Count == 0)
            lblWarning.Visible = true;
        else
            lblWarning.Visible = false;
    }
}