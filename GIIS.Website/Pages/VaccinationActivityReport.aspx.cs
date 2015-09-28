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
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_VaccinationActivityReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, System.EventArgs e)
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

            if ((actionList != null) && actionList.Contains("ViewVaccinationActivityReport") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);

                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["VaccinationActivityReport-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "VaccinationActivityReport");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("VaccinationActivityReport-dictionary" + language, wtList);
                }

                this.lblTitle.Text = wtList["VaccinationActivityReportPageTitle"];
                this.btnSearch.Text = wtList["VaccinationActivityReportSearchButton"];
                this.btnExcel.Text = wtList["VaccinationActivityReportExcelButton"];
                this.btnPrint.Text = wtList["VaccinationActivityReportPrintButton"];
                this.lblItem.Text = wtList["VaccinationActivityReportItem"];
                this.lblYear.Text = wtList["VaccinationActivityReportYear"];

                string jan = wtList["Jan"];
                string feb = wtList["Feb"];
                string mar = wtList["Mar"];
                string apr = wtList["Apr"];
                string may = wtList["May"];
                string jun = wtList["Jun"];
                string jul = wtList["Jul"];
                string aug = wtList["Aug"];
                string sep = wtList["Sep"];
                string oct = wtList["Oct"];
                string nov = wtList["Nov"];
                string dec = wtList["Dec"];

                string vaccine = wtList["Vaccine"];
                string total = wtList["Total"];
                string planned = wtList["Planned"];

                string months = string.Format(@" ""{12}"" text, ""{0}"" int, ""{1}"" int, ""{2}"" int, ""{3}"" int, ""{4}"" int, ""{5}"" int, ""{6}"" int, ""{7}"" int, ""{8}"" int, ""{9}"" int, ""{10}"" int, ""{11}"" int ", jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec, vaccine);
                
                Session["__Months"] = months;
                Session["__Total"] = total;
                Session["__Planned"] = planned;
                ddlYear.SelectedValue = DateTime.Today.Date.Year.ToString();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if ((CurrentEnvironment.LoggedUser != null))
        {
            int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
            string _hfId = (string)Request.QueryString["hfId"];
            if (!String.IsNullOrEmpty(_hfId))
                int.TryParse(_hfId, out hfId);

            string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);

            string where = string.Format(@" AND (""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" in ({0})) ", s);
            string tmp = where;

            HttpContext.Current.Session["VaccinationActivityReport-TempWhere"] = tmp;

            int year = int.Parse(ddlYear.SelectedValue);
            if (year != -1)
                where += string.Format(@" AND EXTRACT(YEAR FROM ""VACCINATION_EVENT"".""VACCINATION_DATE"")::INTEGER = {0} ", year);
            
            HttpContext.Current.Session["VaccinationActivityReport-Where"] = where;
            HttpContext.Current.Session["Year"] = year;

            string language = CurrentEnvironment.Language;
            int languageId = int.Parse(language);

            DataTable dt = VaccinationActivity.GetVaccinationActivity(languageId, where, Session["__Months"].ToString());
            DataTable source = GetDataTable(dt, tmp, languageId);

            gvReport.DataSource = source;
            gvReport.DataBind();

            string url = VaccinationActivityReportGraphHelper.GetUrl(source, this.lblTitle.Text);

            iActivityReport.Text = String.Format("<img src='{0}' alt='' />", url.Replace(" ", ""));
        }
    }

    public DataTable GetDataTable(DataTable dt, string tmp, int languageId)
    {
        string _total = Session["__Total"].ToString();
        string _planned = Session["__Planned"].ToString();

        DataColumn total = new DataColumn(_total, System.Type.GetType("System.Int32"));
        DataColumn planned = new DataColumn(_planned, System.Type.GetType("System.Int32"));

        dt.Columns.Add(total);
        dt.Columns.Add(planned);

        foreach (DataRow dr in dt.Rows)
        {
            string where = tmp;

            int s = 0;
            for (int index = 2; index <= 13; index++)
            {
                if (dr[index] != DBNull.Value)
                {
                    int v = int.Parse(dr[index].ToString());
                    s += v;
                }
            }
            dr[14] = s;

        
            where += string.Format(@" AND ""DOSE_ID"" = {0}", dr[0]);
            where += string.Format(@" AND ""VACCINATION_APPOINTMENT"".""SCHEDULED_DATE"" between '{0}-01-01' and now()", ddlYear.SelectedValue);

            dr[15] = VaccinationActivity.GetPlanedVaccinationActivity(languageId, where);
        }

        System.Data.DataRow drow = dt.Rows[dt.Rows.Count - 1];
        for (int index = 2; index <= 15; index++)
        {
            int sum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[index] != DBNull.Value)
                {
                    sum += int.Parse(dr[index].ToString());
                }
            }
            drow[index] = sum;
        }

        dt.AcceptChanges();

        return dt;
    }

    protected void gvReport_DataBound(object sender, EventArgs e)
    {
        lblItem.Visible = (gvReport.Rows.Count > 0);
        ddlItem.Visible = (gvReport.Rows.Count > 0);

        btnExcel.Visible = (gvReport.Rows.Count > 0);
        btnPrint.Visible = (gvReport.Rows.Count > 0);

        if (gvReport.Rows.Count > 0)
            foreach (GridViewRow gvr in gvReport.Rows)
                gvr.Cells[0].Visible = false;
        gvReport.HeaderRow.Cells[0].Visible = false;
       
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=VaccinationActivityReport.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvReport.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
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

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((CurrentEnvironment.LoggedUser != null))
        {
            int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
            string _hfId = (string)Request.QueryString["hfId"];
            if (!String.IsNullOrEmpty(_hfId))
                int.TryParse(_hfId, out hfId);

            string s = HealthFacility.GetAllChildsForOneHealthFacility(hfId);

            string where = string.Format(@" AND (""VACCINATION_EVENT"".""HEALTH_FACILITY_ID"" in ({0})) ", s);
            string tmp = where;

            HttpContext.Current.Session["VaccinationActivityReport-TempWhere"] = tmp;

            int year = int.Parse(ddlYear.SelectedValue);
            if (year != -1)
                where += string.Format(@" AND EXTRACT(YEAR FROM ""VACCINATION_EVENT"".""VACCINATION_DATE"")::INTEGER = {0} ", year);
            HttpContext.Current.Session["VaccinationActivityReport-Where"] = where;
            string language = CurrentEnvironment.Language;
            int languageId = int.Parse(language);

            DataTable dt = VaccinationActivity.GetVaccinationActivity(languageId, where, Session["__Months"].ToString(), int.Parse(ddlItem.SelectedValue));
            DataTable source = GetDataTable(dt, tmp, languageId);

            string url = VaccinationActivityReportGraphHelper.GetUrl(source, this.lblTitle.Text);

            iActivityReport.Text = String.Format("<img src='{0}' alt='' />", url.Replace(" ", ""));
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
        string _hfId = (string)Request.QueryString["hfId"];
        if (!String.IsNullOrEmpty(_hfId))
            int.TryParse(_hfId, out hfId);
        HttpContext.Current.Session["VaccinationActivityReport-Item"] = ddlItem.SelectedItem;
        string queryString = "PrintVaccinationActivityReport.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);

    }
  
}