using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_CohortCoverageReport : System.Web.UI.Page
{
    static String communityId;
    static String domicileId;

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

            if ((actionList != null) && actionList.Contains("CohortCoverageReport") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["CohortCoverageReport-dictionary" + language + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "CohortCoverageReport");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("CohortCoverageReport-dictionary" + language + language, wtList);
                }

                //controls
                this.lblFromDate.Text = wtList["CohortCoverageReportFromDate"];
                this.lblToDate.Text = wtList["CohortCoverageReportToDate"];
                this.lblEndDate.Text = wtList["CohortCoverageReportEndDate"];
                this.lblVaccine.Text = wtList["CohortCoverageReportVaccine"];
                this.lblCommunityId.Text = wtList["CohortCoverageReportCommunity"];
                this.lblDomicileId.Text = wtList["CohortCoverageReportDomicile"];

                //buttons
                this.btnSearch.Text = wtList["CohortCoverageReportSearchButton"];
                this.btnExcel.Text = wtList["CohortCoverageReportExelButton"];
                this.btnPrint.Text = wtList["CohortCoverageReportPrintButton"];

                //Page Title
                this.lblTitle.Text = wtList["CohortCoverageReportPageTitle"];

                //validators
                ConfigurationDate cd = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceFromDate.Format = cd.DateFormat;
                revFromDate.ErrorMessage = cd.DateFormat;
                revFromDate.ValidationExpression = cd.DateExpresion;
                ceToDate.Format = cd.DateFormat;
                revToDate.ErrorMessage = cd.DateFormat;
                revToDate.ValidationExpression = cd.DateExpresion;
                ceEndDate.Format = cd.DateFormat;
                revEndDate.ErrorMessage = cd.DateFormat;
                revEndDate.ValidationExpression = cd.DateExpresion;

                txtEndDate.Text = DateTime.Today.ToString(cd.DateFormat);

                rfvFromDate.ErrorMessage = wtList["CohortCoverageReportDateErrorMessage"];
                rfvToDate.ErrorMessage = wtList["CohortCoverageReportDateErrorMessage"];
                rfvEndDate.ErrorMessage = wtList["CohortCoverageReportDateErrorMessage"];
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    private DataTable CreateDataTable(DataTable dt)
    {
        DataTable table = new DataTable();


        DataColumn col0 = new DataColumn("View Not Vaccinated Children");
        DataColumn vId = new DataColumn("VaccineId");
        DataColumn col1 = new DataColumn("Vaccine");
        DataColumn col2 = new DataColumn("Cohort");
        DataColumn col3 = new DataColumn("Immunized");
        DataColumn col4 = new DataColumn("Vaccination Coverage");

        col0.DataType = System.Type.GetType("System.String");
        vId.DataType = System.Type.GetType("System.String");
        col1.DataType = System.Type.GetType("System.String");
        col2.DataType = System.Type.GetType("System.String");
        col3.DataType = System.Type.GetType("System.String");
        col4.DataType = System.Type.GetType("System.String");

        table.Columns.Add(col0);
        table.Columns.Add(vId);
        table.Columns.Add(col1);
        table.Columns.Add(col2);
        table.Columns.Add(col3);
        table.Columns.Add(col4);


        List<NonvaccinationReason> nvrList = NonvaccinationReason.GetNonvaccinationReasonList();
        foreach (NonvaccinationReason nvr in nvrList)
        {
            DataColumn col = new DataColumn(nvr.Name);
            col.DataType = System.Type.GetType("System.String");
            table.Columns.Add(col);
        }


        return table;
    }

    private DataRow CreateDataRow(string[] result, DataTable table)
    {
        DataRow row = table.NewRow();

        string cohort = "", immunized = "";
        int index = 1;
        string dateFormat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();
        DateTime dateto = DateTime.ParseExact(txtToDate.Text, dateFormat, CultureInfo.CurrentCulture);

        foreach (string word in result)
        {
            if (index == 5)
            {
                index++;
                row[index] = word;
            }

            row[index] = word;

            if (index == 3)
            {
                cohort = word;
                if (cohort == "0")
                {
                    HealthFacilityCohortData hfcohort = HealthFacilityCohortData.GetHealthFacilityCohortDataByYearAndHealthFacility(CurrentEnvironment.LoggedUser.HealthFacilityId, dateto.Year);
                    if (hfcohort != null)
                    {
                        cohort = hfcohort.Cohort.ToString();
                        row[3] = cohort;
                    }
                }
            }
            if (index == 4) immunized = word;

           
            index++;
        }

        row[0] = (int.Parse(cohort) - int.Parse(immunized));//View Not Vaccinated Children
        if (cohort != "0")
        {
            int i = int.Parse(immunized);
            int c = int.Parse(cohort);
            double r = (double)i / c;
            row[5] = (r * 100).ToString("0.00") + "  %";//Vaccination Coverage
        }
        else row[5] = "0 %";

        table.Rows.Add(row);

        return row;
    }
    protected void Domicile_ValueSelected(object sender, System.EventArgs e)
    {
        domicileId = txtDomicileId.SelectedItemID.ToString();
    }
    protected void Communities_ValueSelected(object sender, System.EventArgs e)
    {
        communityId = txtCommunityId.SelectedItemID.ToString();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (CurrentEnvironment.LoggedUser != null)
        {
            string dateFormat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();

            DateTime datefrom = DateTime.ParseExact(txtFromDate.Text, dateFormat, CultureInfo.CurrentCulture);
            DateTime dateto = DateTime.ParseExact(txtToDate.Text, dateFormat, CultureInfo.CurrentCulture);
            DateTime dateend = DateTime.ParseExact(txtEndDate.Text, dateFormat, CultureInfo.CurrentCulture);

            string language = CurrentEnvironment.Language;
            int languageId = int.Parse(language);

            int vaccineId = int.Parse(ddlVaccine.SelectedValue);
            int community = 0;
            int domicile = 0;
            if (!String.IsNullOrEmpty(communityId))
                community = int.Parse(communityId);
            if (!String.IsNullOrEmpty(domicileId))
                domicile = int.Parse(domicileId);

            int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
            string _hfId = (string)Request.QueryString["hfId"];
            if (!String.IsNullOrEmpty(_hfId))
                int.TryParse(_hfId, out hfId);

            string healthFacilityId = "1";
            if (hfId != 1)
                healthFacilityId = HealthFacility.GetAllChildsForOneHealthFacility(hfId, true);
            
            HttpContext.Current.Session["fromdate"] = txtFromDate.Text;
            HttpContext.Current.Session["todate"] = txtToDate.Text;
            HttpContext.Current.Session["enddate"] = txtEndDate.Text;
            HttpContext.Current.Session["vaccineId"] = vaccineId;
            HttpContext.Current.Session["community"] = community;
            HttpContext.Current.Session["domicile"] = domicile;

            DataTable dt = CohortCoverage.GetCohortCoverage(languageId, healthFacilityId, datefrom, dateto, dateend, vaccineId, community, domicile);
            DataTable table = CreateDataTable(dt);

            if (dt != null)
            {
                char[] stringSeparators = new char[] { ',' };
                string[] result;
                foreach (DataRow dr in dt.Rows)
                {
                    result = dr[0].ToString().Replace(" ", "").Replace("(", "").Replace(")", "").Replace("\"", "").Split(stringSeparators);
                    CreateDataRow(result, table);
                }

                gvCohortCoverage.DataSource = table;
                gvCohortCoverage.DataBind();

                string url = VaccineCoverageReportGrahpHelper.GetUrl(table, this.lblTitle.Text);

                iCoverageReport.Text = String.Format("<img src='{0}' alt='' />", url.Replace(" ", ""));
            }
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=CohortCoverageReport.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvCohortCoverage.RenderControl(htmlWrite);
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int hfId;
        if (Request.QueryString["hfId"] != null)
            hfId = int.Parse(Request.QueryString["hfId"].ToString());
        else
            hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

        string queryString = "PrintCohortCoverageReport.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        HttpContext.Current.Session["hfId"] = null;
    }
    protected void gvCohortCoverage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int hfId;
            if (Request.QueryString["hfId"] != null)
                hfId = int.Parse(Request.QueryString["hfId"].ToString());
            else
                hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

            if (e.Row.Cells[0].Text != "0")
            {
                e.Row.Cells[0].Font.Bold = true;
                HyperLink hlNotVaccinated = new HyperLink();
                hlNotVaccinated.Text = e.Row.Cells[0].Text;
                hlNotVaccinated.NavigateUrl = "ListNotImmunizedChildren.aspx?hfId=" + hfId.ToString() + "&vaccineId=" + e.Row.Cells[1].Text;
                hlNotVaccinated.Target = "_blank";
                e.Row.Cells[0].Controls.Add(hlNotVaccinated);
            }
        }
    }
    protected void gvCohortCoverage_DataBound(object sender, EventArgs e)
    {
        if (gvCohortCoverage.Rows.Count > 0)
        {
            btnExcel.Visible = true;
            btnPrint.Visible = true;
            foreach (GridViewRow gvr in gvCohortCoverage.Rows)
            {
                gvr.Cells[1].Visible = false;
            }
            gvCohortCoverage.HeaderRow.Cells[1].Visible = false;
        }
        else
        {
            btnExcel.Visible = false;
            btnPrint.Visible = false;
        }
       
    }
    protected void btnNewSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.RawUrl, false);
    }
}