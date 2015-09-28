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

public partial class Pages_DoseList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewDoseList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Dose-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Dose");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Dose-dictionary" + language, wtList);
                }

                //controls
                this.lblScheduledVaccinationId.Text = wtList["DoseScheduledVaccination"];
                this.lblFullname.Text = wtList["DoseFullname"];

                //grid header text
                gvDose.Columns[1].HeaderText = wtList["DoseFullname"];
                gvDose.Columns[2].HeaderText = wtList["DoseAgeDefinition"];
                gvDose.Columns[3].HeaderText = wtList["DoseScheduledVaccination"];
                gvDose.Columns[4].HeaderText = wtList["DoseDoseNumber"];
                gvDose.Columns[5].HeaderText = wtList["DoseNotes"];
                gvDose.Columns[6].HeaderText = wtList["DoseIsActive"];

                gvExport.Columns[1].HeaderText = wtList["DoseFullname"];
                gvExport.Columns[2].HeaderText = wtList["DoseAgeDefinition"];
                gvExport.Columns[3].HeaderText = wtList["DoseScheduledVaccination"];
                gvExport.Columns[4].HeaderText = wtList["DoseDoseNumber"];
                gvExport.Columns[5].HeaderText = wtList["DoseNotes"];
                gvExport.Columns[6].HeaderText = wtList["DoseIsActive"];

                //actions
                this.btnAddNew.Visible = actionList.Contains("AddDose");
                //this.btnSearch.Visible = actionList.Contains("SearchDose");

                //buttons
                this.btnAddNew.Text = wtList["DoseAddNewButton"];
                this.btnSearch.Text = wtList["DoseSearchButton"];
                this.btnExcel.Text = wtList["DoseExcelButton"];

                //message
                this.lblWarning.Text = wtList["DoseSearchWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["DoseListPageTitle"];

                odsDose.SelectParameters.Clear();
                odsDose.SelectParameters.Add("fullname", "");
                odsDose.SelectParameters.Add("scheduledVaccinationId", "-1");
                gvDose.DataSourceID = "odsDose";
                gvDose.DataBind();

                Session["DoseList-Fullname"] = "";
                Session["DoseList-ScheduledVaccinationId"] = "-1";

                lblWarning.Visible = false;
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string Fsearch = txtFullname.Text.Replace("'", @"''");
        string Ssearch = ddlScheduledVaccination.SelectedValue;

        odsDose.SelectParameters.Clear();
        odsDose.SelectParameters.Add("fullname", Fsearch.ToUpper());
        odsDose.SelectParameters.Add("scheduledVaccinationId", Ssearch);
        gvDose.DataSourceID = "odsDose";
        gvDose.DataBind();

        Session["DoseList-Fullname"] = Fsearch.ToUpper();
        Session["DoseList-ScheduledVaccinationId"] = Ssearch;
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dose.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void gvDose_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDose.PageIndex = e.NewPageIndex;
    }
    protected void gvDose_DataBound(object sender, EventArgs e)
    {
        if (gvDose.Rows.Count <= 0)
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
        string fullname = "";
        int scheduledVaccinationId = -1;

        if (Session["DoseList-Fullname"] != null)
            fullname = Session["DoseList-Fullname"].ToString();
        if (Session["DoseList-ScheduledVaccinationId"] != null)
            scheduledVaccinationId = int.Parse(Session["DoseList-ScheduledVaccinationId"].ToString());

        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        List<Dose> list = Dose.GetPagedDoseList(fullname, scheduledVaccinationId, ref maximumRows, ref startRowIndex);
        gvExport.DataSource = list;
        gvExport.DataBind();
        gvExport.Visible = true;

        if (list.Count >= 1)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=DoseList.xls");
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