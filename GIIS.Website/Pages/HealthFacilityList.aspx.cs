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

public partial class Pages_HealthFacilityList : System.Web.UI.Page
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
                gvHealthFacility.Columns[7].HeaderText = wtList["HealthFacilityNotes"];
                gvHealthFacility.Columns[8].HeaderText = wtList["HealthFacilityIsActive"];
                gvHealthFacility.Columns[11].HeaderText = wtList["HealthFacilityAddress"];

                gvExport.Columns[1].HeaderText = wtList["HealthFacilityName"];
                gvExport.Columns[2].HeaderText = wtList["HealthFacilityCode"];
                gvExport.Columns[3].HeaderText = wtList["HealthFacilityParent"];
                gvExport.Columns[4].HeaderText = wtList["HealthFacilityTopLevel"];
                gvExport.Columns[5].HeaderText = wtList["HealthFacilityLeaf"];
                gvExport.Columns[6].HeaderText = wtList["HealthFacilityVaccinationPoint"];
                gvExport.Columns[7].HeaderText = wtList["HealthFacilityNotes"];
                gvExport.Columns[8].HeaderText = wtList["HealthFacilityIsActive"];
                gvExport.Columns[11].HeaderText = wtList["HealthFacilityAddress"];

                //actions
                this.btnAddNew.Visible = actionList.Contains("AddHealthFacility");
               // this.btnSearch.Visible = actionList.Contains("SearchHealthFacility");

                //buttons
                this.btnAddNew.Text = wtList["HealthFacilityAddNewButton"];
                this.btnSearch.Text = wtList["HealthFacilitySearchButton"];
                this.btnExcel.Text = wtList["HealthFacilityExcelButton"];

                //message
                this.lblWarning.Text = wtList["HealthFacilitySearchWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["HealthFacilityListPageTitle"];

                //gridview databind
                odsHealthFacility.SelectParameters.Clear();


                if (HttpContext.Current.Session["HealthFacilityList-Name"] != null)
                    odsHealthFacility.SelectParameters.Add("name", HttpContext.Current.Session["HealthFacilityList-Name"].ToString().ToUpper());
                else odsHealthFacility.SelectParameters.Add("name", "");

                if (HttpContext.Current.Session["HealthFacilityList-Code"] != null)
                    odsHealthFacility.SelectParameters.Add("code", HttpContext.Current.Session["HealthFacilityList-Code"].ToString().ToUpper());
                else odsHealthFacility.SelectParameters.Add("code", "");

                Session["_healthfacility"] = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string s = HealthFacility.GetAllChildsForOneHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId);

                //if (HttpContext.Current.Session["HealthFacilityList-HFID"] != null)
                //    odsHealthFacility.SelectParameters.Add("hfid", HttpContext.Current.Session["HealthFacilityList-HFID"].ToString().ToUpper());
                //else odsHealthFacility.SelectParameters.Add("hfid", s);

                odsHealthFacility.SelectParameters.Add("hfid", s);

                gvHealthFacility.DataSourceID = "odsHealthFacility";
                gvHealthFacility.DataBind();
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
        string hf = string.Empty;
        if (Session["_healthfacility"] != null)
            hf = Session["_healthfacility"].ToString();

        if (!string.IsNullOrEmpty(hf))
        {
            s = HealthFacility.GetAllChildsForOneHealthFacility(int.Parse(hf));
        }

        odsHealthFacility.SelectParameters.Clear();
        odsHealthFacility.SelectParameters.Add("name", wsearch.ToUpper());
        odsHealthFacility.SelectParameters.Add("code", csearch.ToUpper());
        odsHealthFacility.SelectParameters.Add("hfid", s);

        Session["HealthFacilityList-Name"] = wsearch;
        Session["HealthFacilityList-Code"] = csearch;
        Session["HealthFacilityList-HFID"] = s;
        
    }
    protected void gvHealthFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacility.PageIndex = e.NewPageIndex;
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        //  HttpContext.Current.Session["_lastHealthfacility"] = null;
        Response.Clear();
        Response.Redirect("HealthFacility.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void gvHealthFacility_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacility.Rows.Count <= 0)
        {
            lblWarning.Visible = true;
            btnExcel.Visible = false;
        }
        else
        {
            btnExcel.Visible = true;
            lblWarning.Visible = false;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        string name = "";
        string code = "";
        string ids = "";

        if (HttpContext.Current.Session["HealthFacilityList-Name"] != null)
            name = HttpContext.Current.Session["HealthFacilityList-Name"].ToString();

        if (HttpContext.Current.Session["HealthFacilityList-Code"] != null)
            code = HttpContext.Current.Session["HealthFacilityList-Code"].ToString();

        if (HttpContext.Current.Session["HealthFacilityList-HFID"] != null)
            ids = HttpContext.Current.Session["HealthFacilityList-HFID"].ToString();

        List<HealthFacility> list = HealthFacility.GetPagedHealthFacilityList(name, code, ids, ref maximumRows, ref startRowIndex);
        gvExport.DataSource = list;
        gvExport.DataBind();
        gvExport.Visible = true;

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=HealthFacilityList.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvExport.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();

        gvExport.Visible = false;
    }

    protected override void Render(HtmlTextWriter writer)
    {
        // Ensure that the control is nested in a server form.
        if ((Page != null))
        {
            Page.VerifyRenderingInServerForm(this);
        }

        base.Render(writer);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //     server control at run time. 
    }
}