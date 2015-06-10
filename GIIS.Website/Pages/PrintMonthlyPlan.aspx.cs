using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.DataLayer;
using System.IO;
using System.Text;

public partial class Pages_PrintMonthlyPlan : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("PrintMonthlyPlan") && (CurrentEnvironment.LoggedUser != null))
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

                this.lblTitle.Text = wtList["MonthlyPlanTitle"];

                //buttons
                this.btnPrint.Text = wtList["MonthlyPlanPrintButton"];

                //grid header text
                gvVaccinationEvent.Columns[2].HeaderText = wtList["MonthlyPlanChild"];
                gvVaccinationEvent.Columns[3].HeaderText = wtList["MonthlyPlanVaccineDose"];
                gvVaccinationEvent.Columns[4].HeaderText = wtList["MonthlyPlanAge"];
                gvVaccinationEvent.Columns[5].HeaderText = wtList["MonthlyPlanScheduleDate"];
                gvVaccinationEvent.Columns[6].HeaderText = wtList["MonthlyPlanAddress"];
                gvVaccinationEvent.Columns[7].HeaderText = wtList["MonthlyPlanVaccinationDate"];
                gvVaccinationEvent.Columns[8].HeaderText = wtList["MonthlyPlanLotNumber"];
                gvTotalVaccinesRequired.Columns[0].HeaderText = wtList["MonthlyPlanItem"];
                gvTotalVaccinesRequired.Columns[1].HeaderText = wtList["MonthlyPlanQuantity"];


                int month;
                int year;
                int healthfacilityid = -1;
                string _id = Request.QueryString["hfId"].ToString();
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out healthfacilityid);
                    month = int.Parse(HttpContext.Current.Session["__currentMonth"].ToString());
                    year = int.Parse(HttpContext.Current.Session["__currentYear"].ToString());
                    HealthFacility o = HealthFacility.GetHealthFacilityById(healthfacilityid);
                    lblHealthfacility.Text = o.Name;
                    lblMonth.Text = ", " + ConvertToMonth(month) + " " + year;
                    BindByHealthFacilityMonthYear(healthfacilityid, month, year);
                }

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

    private void BindByHealthFacilityMonthYear(int healthfacilityid, int month, int year)
    {
        DateTime monthDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        int maximumRows = int.MaxValue;
        int startRowIndex = 0;
        gvVaccinationEvent.DataSource = VaccinationEvent.GetMonthlyPlan(ref maximumRows, ref startRowIndex, healthfacilityid, monthDate);
        gvVaccinationEvent.DataBind();
        string s = HealthFacility.GetAllChildsForOneHealthFacility(healthfacilityid);
        gvTotalVaccinesRequired.DataSource = VaccineQuantity.GetQuantityMonthlyPlan(s, monthDate);
        gvTotalVaccinesRequired.DataBind();

    }
    protected void gvVaccinationEvent_DataBound(object sender, EventArgs e)
    {
       
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //Verifies that the control is rendered
    }
    protected void btnPrint_Click(object sender, System.EventArgs e)
    {
        gvVaccinationEvent.UseAccessibleHeader = true;
        gvVaccinationEvent.HeaderRow.TableSection = TableRowSection.TableHeader;
        gvVaccinationEvent.FooterRow.TableSection = TableRowSection.TableFooter;
        gvVaccinationEvent.Attributes["style"] = "border-collapse:separate";
        foreach (GridViewRow row in gvVaccinationEvent.Rows)
        {
            if (row.RowIndex % 20 == 0 && row.RowIndex != 0)
            {
                row.Attributes["style"] = "page-break-after:always;";
            }
        }
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        gvVaccinationEvent.RenderControl(hw);
        string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload = new function(){");
        sb.Append("var printWin = window.open('', '', 'left=0");
        sb.Append(",top=0,width=1000,height=600,status=0');");
        sb.Append("printWin.document.write(\"");
        string style = "<style type = 'text/css'>thead {display:table-header-group;} tfoot{display:table-footer-group;}</style>";
        sb.Append(style + gridHTML);
        sb.Append("\");");
        sb.Append("printWin.document.close();");
        sb.Append("printWin.focus();");
        sb.Append("printWin.print();");
        sb.Append("printWin.close();");
        sb.Append("};");
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
        gvVaccinationEvent.DataBind();
    }

}
