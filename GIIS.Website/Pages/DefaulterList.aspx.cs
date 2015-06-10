using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net;

public partial class Pages_DefaulterList : System.Web.UI.Page
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

                //Page Title
                this.lblTitle.Text = wtList["MonthlyPlanDefaultersList"];
                lblName.Text = wtList["MonthlyPlanHealthCenter"];
                lblDomicileId.Text = wtList["MonthlyPlanAddress"]; 


                //grid header text
                gvVaccinationEvent.Columns[1].HeaderText = wtList["MonthlyPlanChild"];
                gvVaccinationEvent.Columns[2].HeaderText = wtList["MonthlyPlanVaccineDose"];
                gvVaccinationEvent.Columns[3].HeaderText = wtList["MonthlyPlanScheduleDate"];
                gvVaccinationEvent.Columns[4].HeaderText = wtList["MonthlyPlanAddress"];

                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    int.TryParse(_hfId, out hfId);
                Session["__healthFacilityId_DefaulterList"] = hfId;


                int domicileId = -1;
                if (ddlDomicile.SelectedValue != "")
                 domicileId = int.Parse(ddlDomicile.SelectedValue);
                
                HealthFacility hc = HealthFacility.GetHealthFacilityById(hfId);
                lblHealthCenter.Text = hc.Name.ToString();
                if (hc.VaccinationPoint)
                {
                    BindGrid(hfId.ToString(), domicileId);
                }
                else
                {
                    string s = HealthFacility.GetAllChildsForOneHealthFacility(hc.Id, true);
                    BindGrid(s, domicileId);

                }

                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                if (HttpContext.Current.Session["__currentMonth"] != null)
                    month = int.Parse(HttpContext.Current.Session["__currentMonth"].ToString());
                if (HttpContext.Current.Session["__currentYear"] != null)
                    year = int.Parse(HttpContext.Current.Session["__currentYear"].ToString());

                lblActualMonth.Text = ConvertToMonth(month) + ", " + year.ToString();

            }
            else
                Response.Redirect("Default.aspx", false);
        }
    }

    private void BindGrid(string hcId, int domicileId)
    {
      
            odsVaccinationEvent.SelectParameters.Clear();
            odsVaccinationEvent.SelectParameters.Add("healthFacilityId", hcId);
            odsVaccinationEvent.SelectParameters.Add("domicileId", domicileId.ToString());
            gvVaccinationEvent.Visible = true;
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
    
    protected void gvVaccinationEvent_DataBound(object sender, EventArgs e)
    {
        if (gvVaccinationEvent.Rows.Count > 0)
            btnExcel.Visible = true;
        else
            btnExcel.Visible = false;       
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
        
        if (HttpContext.Current.Session["__healthFacilityId_DefaulterList"] != null)
            hfId = int.Parse(HttpContext.Current.Session["__healthFacilityId_DefaulterList"].ToString());
        
        HealthFacility hc = HealthFacility.GetHealthFacilityById(hfId);
        
        int domicileId = -1;
        if (ddlDomicile.SelectedValue != "")
            domicileId = int.Parse(ddlDomicile.SelectedValue);
        
        if (hc.VaccinationPoint)
        {
            odsExport.SelectParameters.Clear();
            odsExport.SelectParameters.Add("healthFacilityId", hfId.ToString());
            odsExport.SelectParameters.Add("domicileId", domicileId.ToString());
            gvExport.DataBind();
        }
        
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=DefaulterList.xls");
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
    }
    
    protected void ddlDomicile_SelectedIndexChanged(object sender, EventArgs e)
    {
        int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
        if (HttpContext.Current.Session["__healthFacilityId_DefaulterList"] != null)
            hfId = int.Parse(HttpContext.Current.Session["__healthFacilityId_DefaulterList"].ToString());
        
        HealthFacility hc = HealthFacility.GetHealthFacilityById(hfId);

        int domicileId = int.Parse(ddlDomicile.SelectedValue);

        if (hc.VaccinationPoint)
        {
            BindGrid(hfId.ToString(), domicileId);
        }
        else
        {
            string s = HealthFacility.GetAllChildsForOneHealthFacility(hc.Id, true);
            BindGrid(s, domicileId);

        }
    }
}