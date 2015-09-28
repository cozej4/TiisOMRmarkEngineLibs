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
using System.Globalization;

public partial class Pages_ScheduledVaccinationList : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewScheduledVaccinationList") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ScheduledVaccination-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ScheduledVaccination");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ScheduledVaccination-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["ScheduledVaccinationName"];
                this.lblCode.Text = wtList["ScheduledVaccinationCode"];
                
                //grid header text
                gvScheduledVaccination.Columns[1].HeaderText = wtList["ScheduledVaccinationName"];
                gvScheduledVaccination.Columns[2].HeaderText = wtList["ScheduledVaccinationCode"];
                //  gvScheduledVaccination.Columns[3].HeaderText = wtList["ScheduledVaccinationHl7Vaccine"];
                gvScheduledVaccination.Columns[3].HeaderText = wtList["ScheduledVaccinationItem"];
                gvScheduledVaccination.Columns[4].HeaderText = wtList["ScheduledVaccinationEntryDate"];
                gvScheduledVaccination.Columns[5].HeaderText = wtList["ScheduledVaccinationExitDate"];
                gvScheduledVaccination.Columns[6].HeaderText = wtList["ScheduledVaccinationStatus"];
                gvScheduledVaccination.Columns[7].HeaderText = wtList["ScheduledVaccinationDeseases"];
                gvScheduledVaccination.Columns[8].HeaderText = wtList["ScheduledVaccinationNotes"];
                gvScheduledVaccination.Columns[9].HeaderText = wtList["ScheduledVaccinationIsActive"];

                //actions
                //this.btnAddNew.Visible = actionList.Contains("AddScheduledVaccination");
                //this.btnSearch.Visible = actionList.Contains("SearchScheduledVaccination");

                //buttons
                this.btnAddNew.Text = wtList["ScheduledVaccinationAddNewButton"];
                this.btnSearch.Text = wtList["ScheduledVaccinationSearchButton"];
                this.btnExcel.Text=wtList["ScheduleVaccinationExcelButton"];

                //message
                this.lblWarning.Text = wtList["ScheduledVaccinationSearchWarningText"];

                //Page Title
                this.lblTitle.Text = wtList["ScheduledVaccinationListPageTitle"];

                odsScheduledVaccination.SelectParameters.Clear();
                odsScheduledVaccination.SelectParameters.Add("name", "");
                odsScheduledVaccination.SelectParameters.Add("code", "");
                gvScheduledVaccination.DataSourceID = "odsScheduledVaccination";
                gvScheduledVaccination.DataBind();
                lblWarning.Visible = false;

                Session["ScheduledVaccinationList-Name"] = "";
                Session["ScheduledVaccinationList-Code"] = "";
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string wsearch = txtName.Text.Replace("'", @"''");
        string csearch = txtCode.Text.Replace("'", @"''");
        
        odsScheduledVaccination.SelectParameters.Clear();
        odsScheduledVaccination.SelectParameters.Add("name", wsearch.ToUpper());
        odsScheduledVaccination.SelectParameters.Add("code", csearch.ToUpper());
        gvScheduledVaccination.DataSourceID = "odsScheduledVaccination";
        gvScheduledVaccination.DataBind();

        Session["ScheduledVaccinationList-Name"] = wsearch.ToUpper();
        Session["ScheduledVaccinationList-Code"] = csearch.ToUpper();
    }
    protected void gvScheduledVaccination_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvScheduledVaccination.PageIndex = e.NewPageIndex;
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ScheduledVaccination.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
    protected void gvScheduledVaccination_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        //((BoundField)gvScheduledVaccination.Columns[4]).DataFormatString = "{0:" + dateformat + "}";
        //((BoundField)gvScheduledVaccination.Columns[5]).DataFormatString = "{0:" + dateformat + "}";

        //((BoundField)gvExport.Columns[4]).DataFormatString = "{0:" + dateformat + "}";
        //((BoundField)gvExport.Columns[5]).DataFormatString = "{0:" + dateformat + "}";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string exitDate = e.Row.Cells[5].Text;
            //DateTime date = DateTime.ParseExact(exitDate, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            DateTime date = DateTime.ParseExact(exitDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture);
            if (date.ToString("yyyy-MM-dd").Equals("0001-01-01"))
                e.Row.Cells[5].Text = String.Empty;
        }
    }
    protected void gvScheduledVaccination_DataBound(object sender, EventArgs e)
    {
        if (gvScheduledVaccination.Rows.Count <= 0)
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
        string name = "";
        string code = "";

        if (Session["ScheduledVaccinationList-Name"] != null)
            name = Session["ScheduledVaccinationList-Name"].ToString();
        if (Session["ScheduledVaccinationList-Code"] != null)
            code = Session["ScheduledVaccinationList-Code"].ToString();

        int maximumRows = int.MaxValue;
        int startRowIndex = 0;

        List<ScheduledVaccination> list = ScheduledVaccination.GetPagedScheduledVaccinationList(name, code, ref maximumRows, ref startRowIndex);
        gvExport.DataSource = list;
        gvExport.DataBind();
        gvExport.Visible = true;

        if (list.Count >= 1)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ScheduledVaccinationList.xls");
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