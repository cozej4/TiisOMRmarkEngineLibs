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

public partial class Pages_HealtFacilityListForAdjustments : System.Web.UI.Page
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
                gvHealthFacility.Columns[7].HeaderText = wtList["HealthFacilityListViewAdjustments"];


                //actions
                // this.btnSearch.Visible = actionList.Contains("SearchHealthFacility");

                //buttons
                this.btnSearch.Text = wtList["HealthFacilitySearchButton"];

                //message
                this.lblWarning.Text = wtList["HealthFacilitySearchWarningText"];

                //Page Title
                // this.lblTitle.Text = wtList["HealthFacilityListPageTitle"];

                //gridview databind
                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);

                HealthFacility hf = HealthFacility.GetHealthFacilityById(hfId);
                if (hf.VaccinationPoint && hf.Leaf)
                    Response.Redirect("ViewAdjustments.aspx?hfId=" + hfId, false);
                Context.ApplicationInstance.CompleteRequest();

                if (hf.TopLevel)
                {
                    odsHealthFacility.SelectParameters.Clear();
                    odsHealthFacility.SelectParameters.Add("where", "1 = 1");
                }
                else
                {
                    string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);
                    string where = string.Format(" \"ID\" in ({0}) ", s);
                    odsHealthFacility.SelectParameters.Clear();
                    odsHealthFacility.SelectParameters.Add("where", where);
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
        string csearch = txtCode.Text;
        string where;

        where = @" UPPER(""NAME"") like '%" + wsearch.ToUpper() + @"%' AND UPPER(""CODE"") like '%" + csearch.ToUpper() + "%'";
        int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
        string _hfId = (string)Request.QueryString["id"];
        if (!String.IsNullOrEmpty(_hfId))
            int.TryParse(_hfId, out hfId);

        HealthFacility hf = HealthFacility.GetHealthFacilityById(hfId);
        if (!hf.TopLevel)
        {
            string s = HealthFacility.GetAllChildsForOneHealthFacility(hf.Id);
            where += string.Format(" AND (\"ID\" in ({0})) ", s);
        }
        odsHealthFacility.SelectParameters.Clear();
        odsHealthFacility.SelectParameters.Add("where", where);
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