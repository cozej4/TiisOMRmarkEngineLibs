using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;
using System.Diagnostics;
using System.IO;

public partial class Pages_MonthlyPlan : System.Web.UI.Page
{
    static String healthCenterId;
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

            if ((actionList != null) && actionList.Contains("ViewMonthlyPlan") && (CurrentEnvironment.LoggedUser != null))
            {

                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["MonthlyPlan-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "MonthlyPlan");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("MonthlyPlan-dictionary" + language, wtList);
                }
                Dictionary<string, string> wtListHF = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacility-dictionary" + language];
                if (wtListHF == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacility");
                    wtListHF = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtListHF.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacility-dictionary" + language, wtListHF);
                }

                //Page Title
                this.lblTitle.Text = wtList["MonthlyPlanPageTitle"];
                this.lblHealthCenter.Text = wtList["MonthlyPlanHealthCenter"];

                //buttons
                this.btnFind.Text = wtList["MonthlyPlanFindButton"];
                this.btnPrint.Text = wtList["MonthlyPlanPrintButton"];
               // this.hlSMSPage.Text = wtList["MonthlyPlanDefaultersList"];

                //grid header text
                gvVaccinationEvent.Columns[2].HeaderText = wtList["MonthlyPlanChild"];
                gvVaccinationEvent.Columns[3].HeaderText = wtList["MonthlyPlanVaccineDose"];
                gvVaccinationEvent.Columns[4].HeaderText = wtList["MonthlyPlanAge"];
                gvVaccinationEvent.Columns[5].HeaderText = wtList["MonthlyPlanScheduleDate"];
                gvVaccinationEvent.Columns[6].HeaderText = wtList["MonthlyPlanAddress"];
                gvTotalVaccinesRequired.Columns[0].HeaderText = wtList["MonthlyPlanItem"];
                gvTotalVaccinesRequired.Columns[1].HeaderText = wtList["MonthlyPlanQuantity"];

                //health facility grid

                gvHealthFacility.Columns[1].HeaderText = wtListHF["HealthFacilityName"];
                gvHealthFacility.Columns[2].HeaderText = wtListHF["HealthFacilityCode"];
                gvHealthFacility.Columns[3].HeaderText = wtListHF["HealthFacilityParent"];
                gvHealthFacility.Columns[4].HeaderText = wtList["MonthlyPlanPageTitle"];//wtListHF["HealthFacilityViewMonthlyPlan"];


                if (HttpContext.Current.Session["__currentDate"] == null)
                    HttpContext.Current.Session["__currentDate"] = DateTime.Today;
                if (HttpContext.Current.Session["__currentMonth"] == null)
                    HttpContext.Current.Session["__currentMonth"] = DateTime.Today.Month;
                if (HttpContext.Current.Session["__currentYear"] == null)
                    HttpContext.Current.Session["__currentYear"] = DateTime.Today.Year;
                lblActualMonth.Text = ConvertToMonth(DateTime.Today.Month);

                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                int find = 0;
                if (Request.QueryString["find"] != null)
                    find = int.Parse(Request.QueryString["find"].ToString());
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);
                else if (HttpContext.Current.Session["__planhc"] != null)
                    hfId = int.Parse(HttpContext.Current.Session["__planhc"].ToString());

                HealthFacility hc = HealthFacility.GetHealthFacilityById(hfId);
                HealthFacility hf = HealthFacility.GetHealthFacilityById(CurrentEnvironment.LoggedUser.HealthFacilityId);

                if (!hc.TopLevel)
                {
                    List<HealthFacility> hfList = HealthFacility.GetHealthFacilityByParentId(hfId);
                    bool leafs = true;
                  

                    if (hc.VaccinationPoint)
                    {
                        txtHealthcenterId.SelectedItemID = hc.Id.ToString();
                        txtHealthcenterId.SelectedItemText = hc.Name;
                        HttpContext.Current.Session["__planhc"] = hc.Id.ToString();
                        BindGrid(hc.Id, true, hc.Id.ToString());
                        gvHealthFacility.Visible = false;
                        gvVaccinationEvent.Visible = true;
                        if (hc.Id == hf.Id)
                        {
                            btnFind.Visible = false;
                            txtHealthcenterId.Enabled = false;
                        }

                    }
                    else if (hc.VaccineStore && leafs && (!hc.TopLevel))
                    {

                        string where = string.Format(@" ""PARENT_ID"" = {0} AND ""ID"" <> {0} ", hc.Id);
                        string s = HealthFacility.GetAllChildsForOneHealthFacility(hc.Id);
                        odsHealthFacility.SelectParameters.Clear();
                        odsHealthFacility.SelectParameters.Add("where", where);
                        BindGrid(hc.Id, false, s);
                        gvHealthFacility.Visible = true;
                        gvVaccinationEvent.Visible = false;
                        HttpContext.Current.Session["__planhc"] = hc.Id.ToString();
                      
                    }
                    else
                    {
                        gvHealthFacility.Visible = false;
                        gvVaccinationEvent.Visible = false;

                    }
                }
                if ((find == 0) && !hf.VaccinationPoint)
                {
                    gvHealthFacility.Visible = true;
                    gvVaccinationEvent.Visible = false;
                    if (HttpContext.Current.Session["__planhc"] != null)
                        HttpContext.Current.Session["__planhc"] = null;
                    txtHealthcenterId.SelectedItemText = string.Empty;
                }

                int month = int.Parse(HttpContext.Current.Session["__currentMonth"].ToString());
                lblActualMonth.Text = ConvertToMonth(month);
            }
            else
                Response.Redirect("Default.aspx", false);
        }
    }

    private static string ConvertToMonth(int monthNumber)
    {
        string language = CurrentEnvironment.Language;
        Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["MonthlyPlan-dictionary" + language];
        string month = string.Empty;
        switch (monthNumber)
        {
            case 1:
                month = wtList["MonthlyPlanMonth1"];
                break;
            case 2:
                month = wtList["MonthlyPlanMonth2"];
                break;
            case 3:
                month = wtList["MonthlyPlanMonth3"];
                break;
            case 4:
                month = wtList["MonthlyPlanMonth4"];
                break;
            case 5:
                month = wtList["MonthlyPlanMonth5"];
                break;
            case 6:
                month = wtList["MonthlyPlanMonth6"];
                break;
            case 7:
                month = wtList["MonthlyPlanMonth7"];
                break;
            case 8:
                month = wtList["MonthlyPlanMonth8"];
                break;
            case 9:
                month = wtList["MonthlyPlanMonth9"];
                break;
            case 10:
                month = wtList["MonthlyPlanMonth10"];
                break;
            case 11:
                month = wtList["MonthlyPlanMonth11"];
                break;
            case 12:
                month = wtList["MonthlyPlanMonth12"];
                break;
        }
        return month;
    }

    private void BindGrid(int hcId, bool vpoint, string s)
    {
        int month;
        int year;
        month = int.Parse(HttpContext.Current.Session["__currentMonth"].ToString());
        year = int.Parse(HttpContext.Current.Session["__currentYear"].ToString());
        DateTime monthDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        if (vpoint)
        {
            odsVaccinationEvent.SelectParameters.Clear();
            odsVaccinationEvent.SelectParameters.Add("hfId", hcId.ToString());
            odsVaccinationEvent.SelectParameters.Add("scheduledDate", monthDate.ToString());
        }

        odsVaccineQty.SelectParameters.Clear();
        odsVaccineQty.SelectParameters.Add("hfId", s);
        odsVaccineQty.SelectParameters.Add("scheduledDate", monthDate.ToString());
        gvTotalVaccinesRequired.DataSource = odsVaccineQty;
        gvTotalVaccinesRequired.DataBind();
    }

    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        healthCenterId = txtHealthcenterId.SelectedItemID.ToString();

        if (HttpContext.Current.Session["__currentMonth"] == null)
            HttpContext.Current.Session["__currentMonth"] = DateTime.Today.Month;
        if (HttpContext.Current.Session["__currentYear"] == null)
            HttpContext.Current.Session["__currentYear"] = DateTime.Today.Year;
        HttpContext.Current.Session["__planhc"] = healthCenterId;
        HttpContext.Current.Session["__planhcprint"] = healthCenterId;
    }

    protected void gvVaccinationEvent_DataBound(object sender, EventArgs e)
    {
        if (gvVaccinationEvent.Rows.Count > 0)
        {
            btnPrint.Visible = true;
            hlSMSPage.Visible = true;
        }
    }
    protected void gvHealthFacility_DataBound(object sender, EventArgs e)
    {
        if (gvHealthFacility.Rows.Count > 0)
        {
            hlSMSPage.Visible = true;
        }
    }
    protected void btnPreviousMonth_Click(object sender, ImageClickEventArgs e)
    {
        int month = int.Parse(HttpContext.Current.Session["__currentMonth"].ToString());
        int year = int.Parse(HttpContext.Current.Session["__currentYear"].ToString());
        if (month == 1)
        {
            HttpContext.Current.Session["__currentDate"] = new DateTime(year - 1, 12, 1);
            HttpContext.Current.Session["__currentMonth"] = 12;
            HttpContext.Current.Session["__currentYear"] = year - 1;
        }
        else
        {
            HttpContext.Current.Session["__currentDate"] = new DateTime(year, month - 1, 1);
            HttpContext.Current.Session["__currentMonth"] = month - 1;
            HttpContext.Current.Session["__currentYear"] = year;
        }
        lblActualMonth.Text = ConvertToMonth(int.Parse(HttpContext.Current.Session["__currentMonth"].ToString()));

        //if (healthCenterId != null)
        //{
        //    HttpContext.Current.Session["__planhc"] = healthCenterId;
        //    HttpContext.Current.Session["__planhcprint"] = healthCenterId;
        //}
        Response.Redirect(Request.RawUrl);
    }
    protected void btnNextMonth_Click(object sender, ImageClickEventArgs e)
    {
        int month = int.Parse(HttpContext.Current.Session["__currentMonth"].ToString());
        int year = int.Parse(HttpContext.Current.Session["__currentYear"].ToString());
        if (month == 12)
        {
            HttpContext.Current.Session["__currentDate"] = new DateTime(year + 1, 1, 1);
            HttpContext.Current.Session["__currentMonth"] = 1;
            HttpContext.Current.Session["__currentYear"] = year + 1;
        }
        else
        {
            HttpContext.Current.Session["__currentDate"] = new DateTime(year, month + 1, 1);
            HttpContext.Current.Session["__currentMonth"] = month + 1;
            HttpContext.Current.Session["__currentYear"] = year;
        }
        lblActualMonth.Text = ConvertToMonth(int.Parse(HttpContext.Current.Session["__currentMonth"].ToString()));
        //if (healthCenterId != null)
        //{
        //    HttpContext.Current.Session["__planhc"] = healthCenterId;
        //    HttpContext.Current.Session["__planhcprint"] = healthCenterId;
        //}
        Response.Redirect(Request.RawUrl);
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {
        //if (healthCenterId != null)
        //{
        //    HttpContext.Current.Session["__planhc"] = healthCenterId;
        //    HttpContext.Current.Session["__planhcprint"] = healthCenterId;
        //}
        Response.Redirect("MonthlyPlan.aspx?find=1");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int hfId = 0;
        if (HttpContext.Current.Session["__planhcprint"] != null)
            hfId = int.Parse(HttpContext.Current.Session["__planhcprint"].ToString());
        else
            hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;

        string queryString = "PrintMonthlyPlan.aspx?hfId=" + hfId;
        string newWin = "window.open('" + queryString + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        HttpContext.Current.Session["__planhcprint"] = null;
    }
    protected void gvHealthFacility_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacility.PageIndex = e.NewPageIndex;
    }
    protected void hlSMSPage_Click(object sender, EventArgs e)
    {
        string hfId = CurrentEnvironment.LoggedUser.HealthFacilityId.ToString();
        if (HttpContext.Current.Session["__planhc"] != null)
            hfId = HttpContext.Current.Session["__planhc"].ToString();

        int month = int.Parse(HttpContext.Current.Session["__currentMonth"].ToString());
        int year = int.Parse(HttpContext.Current.Session["__currentYear"].ToString());
        //string str = "../Pages/DefaulterList.aspx?hfId=" + hfId + "&month=" + month.ToString() + "&year=" + year.ToString();
        //string newWin = "window.open('" + str + "');";
        //ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
       Response.Redirect("../Pages/DefaulterList.aspx?hfId=" + hfId + "&month=" + month.ToString() + "&year=" + year.ToString());
    }

    protected void gvVaccinationEvent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVaccinationEvent.PageIndex = e.NewPageIndex;
    }

}