using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class Pages_ListNotImmunizedChildren : System.Web.UI.Page
{
    static String healthCenterId;
    string vaccineId;
    int planned;
    DateTime fromdate;
    DateTime todate;
    DateTime enddate;

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

            if ((CurrentEnvironment.LoggedUser != null)) //actionList.Contains("ViewNotImmunizedChildren") &&
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Child-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Child");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Child-dictionary" + language, wtList);
                }

                #region Person Configuration

                List<PersonConfiguration> pcList = PersonConfiguration.GetPersonConfigurationList();

                foreach (PersonConfiguration pc in pcList)
                {
                    if (pc.IsVisible == false)
                    {
                        //Control lbl = FindMyControl(this, "lbl" + pc.ColumnName);
                        //Control txt = FindMyControl(this, "txt" + pc.ColumnName);
                        ////Control rbl = FindMyControl(this, "rbl" + pc.ColumnName);
                        //Control tr = FindMyControl(this, "tr" + pc.ColumnName);

                        //if (lbl != null)
                        //    lbl.Visible = false;

                        //if (txt != null)
                        //    txt.Visible = false;

                        //if (rbl != null)
                        //    rbl.Visible = false;

                        //if (tr != null)
                        //    tr.Visible = false;

                        for (int i = 1; i < gvChild.Columns.Count; i++)
                        {
                            if (gvChild.Columns[i].HeaderText == pc.ColumnName)
                            {
                                gvChild.Columns[i].Visible = false;
                                break;
                            }
                        }
                    }
                }

                #endregion

                //controls
                #region Controls

                //this.lblHealthFacility.Text = wtList["ChildHealthcenter"];
                this.lblVaccine.Text = wtList["ChildVaccine"];
                this.lblTitle.Text = wtList["ChildListNotImmunizedChildrenPageTitle"];
                // this.lblWarning.Text = wtList["ChildListNotImmunizedChildrenWarningText"];

                #endregion

                //grid header text
                #region Grid Columns
                gvChild.Columns[1].HeaderText = wtList["ChildNonVaccinationReason"];
                gvChild.Columns[2].HeaderText = wtList["ChildSystem"];
                gvChild.Columns[3].HeaderText = wtList["ChildFirstname1"];
                gvChild.Columns[4].HeaderText = wtList["ChildLastname1"];
                gvChild.Columns[5].HeaderText = wtList["ChildMotherFirstname"];
                gvChild.Columns[6].HeaderText = wtList["ChildMotherLastname"];
                gvChild.Columns[7].HeaderText = wtList["ChildBirthdate"];
                gvChild.Columns[8].HeaderText = wtList["ChildGender"];
                gvChild.Columns[9].HeaderText = wtList["ChildHealthcenter"];
                gvChild.Columns[10].HeaderText = wtList["ChildDomicile"];

                gvExport.Columns[1].HeaderText = wtList["ChildNonVaccinationReason"];
                gvExport.Columns[2].HeaderText = wtList["ChildSystem"];
                gvExport.Columns[3].HeaderText = wtList["ChildFirstname1"];
                gvExport.Columns[4].HeaderText = wtList["ChildLastname1"];
                gvExport.Columns[5].HeaderText = wtList["ChildMotherFirstname"];
                gvExport.Columns[6].HeaderText = wtList["ChildMotherLastname"];
                gvExport.Columns[7].HeaderText = wtList["ChildBirthdate"];
                gvExport.Columns[8].HeaderText = wtList["ChildGender"];
                gvExport.Columns[9].HeaderText = wtList["ChildHealthcenter"];
                gvExport.Columns[10].HeaderText = wtList["ChildBirthplace"];


                #endregion

                //buttons
               // this.btnSearch.Text = wtList["ChildSearchButton"];
                this.btnExcel.Text = wtList["ChildExcelButton"];
                this.btnPrint.Text = wtList["ChildPrintButton"];

                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                    healthCenterId = _hfId;
                string _vaccineId = (string)Request.QueryString["vaccineId"];
                if (!String.IsNullOrEmpty(_vaccineId))
                    vaccineId = _vaccineId;
                txtVaccine.Text = Dose.GetDoseById(int.Parse(vaccineId)).Fullname;
                HttpContext.Current.Session["vaccine"] = txtVaccine.Text;

                string _planned = (string)Request.QueryString["planned"];
                if (!String.IsNullOrEmpty(_planned))
                    planned = int.Parse(_planned);

                string dateFormat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString();

                fromdate = DateTime.ParseExact(HttpContext.Current.Session["fromdate"].ToString(), dateFormat, CultureInfo.CurrentCulture);
                todate = DateTime.ParseExact(HttpContext.Current.Session["todate"].ToString(), dateFormat, CultureInfo.CurrentCulture);
                enddate = DateTime.ParseExact(HttpContext.Current.Session["enddate"].ToString(), dateFormat, CultureInfo.CurrentCulture);

                string s = HealthFacility.GetAllChildsForOneHealthFacility(int.Parse(healthCenterId));
                string where = string.Format(" AND (\"HEALTHCENTER_ID\" in ({0})) ", s);

                if (planned == 1)
                    where += String.Format(@" AND ""DOSE_ID"" = {0} AND ""SCHEDULED_DATE"" between '{1}' and '{2}' AND ""VACCINATION_DATE"" between '{1}' and '{3}' ", vaccineId, fromdate.ToString("yyyy-MM-dd"), todate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                else
                    where += String.Format(@" AND ""DOSE_ID"" = {0} AND ""BIRTHDATE"" between '{1}' and '{2}' AND ""VACCINATION_DATE"" between '{1}' and '{3}' ", vaccineId, fromdate.ToString("yyyy-MM-dd"), todate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));

                odsChild.SelectParameters.Clear();
                odsChild.SelectParameters.Add("where", where);
                odsChild.DataBind();
                HttpContext.Current.Session["whereExcelNotImmunized"] = where; //use for excel

                if (HttpContext.Current.Session["whereNotImmunized"] != null)
                {
                    string swhere = HttpContext.Current.Session["whereNotImmunized"].ToString();
                    odsChild.SelectParameters.Clear();
                    odsChild.SelectParameters.Add("where", swhere);
                    odsChild.DataBind();
                    HttpContext.Current.Session["whereExcelNotImmunized"] = swhere;
                    HttpContext.Current.Session["whereNotImmunized"] = null;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    //private static Control FindMyControl(Control Root, string Id)
    //{
    //    if (Id == "txtIsActive")
    //        return null;

    //    if (Root.ID == Id)
    //        return Root;

    //    foreach (Control Ctl in Root.Controls)
    //    {
    //        Control FoundCtl = FindMyControl(Ctl, Id);
    //        if (FoundCtl != null)
    //            return FoundCtl;
    //    }

    //    return null;
    //}
    //protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    //{
    //    healthCenterId = txtHealthFacility.SelectedItemID.ToString();
    //}
    protected void gvChild_DataBinding(object sender, System.EventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        ((BoundField)gvChild.Columns[7]).DataFormatString = "{0:" + dateformat + "}";
        ((BoundField)gvExport.Columns[7]).DataFormatString = "{0:" + dateformat + "}";
        if (gvChild.Rows.Count <= 0)
        {
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            //lblWarning.Visible = true;
        }
        else
        {
            btnExcel.Visible = true;
            btnPrint.Visible = true;
            //lblWarning.Visible = false;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (healthCenterId != null)
        {
            vaccineId = Page.Request.QueryString["vaccineId"].ToString();
            txtVaccine.Text = Dose.GetDoseById(int.Parse(vaccineId)).Fullname;

            //fromdate = DateTime.Parse(HttpContext.Current.Session["fromdate"].ToString());
            //todate = DateTime.Parse(HttpContext.Current.Session["todate"].ToString());
            //enddate = DateTime.Parse(HttpContext.Current.Session["enddate"].ToString());

            string s = HealthFacility.GetAllChildsForOneHealthFacility(int.Parse(healthCenterId));
            string where = string.Format(" AND (\"HEALTHCENTER_ID\" in ({0})) ", s);
      
            where += String.Format(@" AND ""DOSE_ID"" = {0} AND ""BIRTHDATE"" between '{1}' and '{2}' AND ""VACCINATION_DATE"" between '{1}' and '{3}' ", vaccineId, fromdate.ToString("yyyy-MM-dd"), todate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));

            odsChild.SelectParameters.Clear();
            odsChild.SelectParameters.Add("where", where);
            odsChild.DataBind();
           // HttpContext.Current.Session["hfId"] = healthCenterId;
            HttpContext.Current.Session["whereNotImmunized"] = where;
            Response.Redirect(Request.RawUrl);
        }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["whereExcelNotImmunized"] != null)
        {
            string where = HttpContext.Current.Session["whereExcelNotImmunized"].ToString();

            int maximumRows = int.MaxValue;
            int startRowIndex = 0;

            List<Child> list = Child.GetNotImmunizedChildren(ref maximumRows, ref startRowIndex, where);
            gvExport.DataSource = list;
            gvExport.DataBind();
            gvExport.Visible = true;

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=NotImmunizedChildren.xls");
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

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (healthCenterId != null)
        {
            int hfId = int.Parse(healthCenterId);

            string queryString = "PrintNotImmunizedChildren.aspx?hfId=" + hfId;
            string newWin = "window.open('" + queryString + "');";
            ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        }
    }
}