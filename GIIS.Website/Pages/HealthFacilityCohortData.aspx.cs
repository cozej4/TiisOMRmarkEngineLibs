using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _HealthFacilityCohortData : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHealthFacilityCohortData") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacilityCohortData-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacilityCohortData");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacilityCohortData-dictionary" + language, wtList);
                }

                //controls
                this.lblHealthFacilityId.Text = wtList["HealthFacilityCohortDataHealthFacility"];
                this.lblYear.Text = wtList["HealthFacilityCohortDataYear"];
                this.lblCohort.Text = wtList["HealthFacilityCohortDataCohort"];
                this.lblNotes.Text = wtList["HealthFacilityCohortDataNotes"];

                //grid header text
                gvHealthFacilityCohortData.Columns[2].HeaderText = wtList["HealthFacilityCohortDataHealthFacility"];
                gvHealthFacilityCohortData.Columns[1].HeaderText = wtList["HealthFacilityCohortDataYear"];
                gvHealthFacilityCohortData.Columns[3].HeaderText = wtList["HealthFacilityCohortDataCohort"];
                gvHealthFacilityCohortData.Columns[4].HeaderText = wtList["HealthFacilityCohortDataNotes"];
                gvExport.Columns[2].HeaderText = wtList["HealthFacilityCohortDataHealthFacility"];
                gvExport.Columns[1].HeaderText = wtList["HealthFacilityCohortDataYear"];
                gvExport.Columns[3].HeaderText = wtList["HealthFacilityCohortDataCohort"];
                gvExport.Columns[4].HeaderText = wtList["HealthFacilityCohortDataNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddHealthFacilityCohortData");
                this.btnEdit.Visible = actionList.Contains("EditHealthFacilityCohortData");

                //buttons
                this.btnAdd.Text = wtList["HealthFacilityCohortDataAddButton"];
                this.btnEdit.Text = wtList["HealthFacilityCohortDataEditButton"];
                //this.btnExcel.Text = wtList["HealthFacilityCohortDataExcelButton"];

                //message
                this.lblSuccess.Text = wtList["HealthFacilityCohortDataSuccessText"];
                this.lblWarning.Text = wtList["HealthFacilityCohortDataWarningText"];
                this.lblError.Text = wtList["HealthFacilityCohortDataErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["HealthFacilityCohortDataPageTitle"];

                //validators
                //cvYear.ErrorMessage = wtList["HealthFacilityCohortDataMandatory"];
               // rfvCohort.ErrorMessage = wtList["HealthFacilityCohortDataMandatory"];
                //rfvHealthFacilityId.ErrorMessage = wtList["HealthFacilityCohortDataMandatory"];
                cvHealthFacility.ErrorMessage = wtList["HealthFacilityCohortDataMandatory"];

                //gridview databind
                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                {
                    int.TryParse(_hfId, out hfId);
                    HealthFacility h = HealthFacility.GetHealthFacilityById(hfId);
                    lblHealthFacility.Text = h.Name;

                    string where = string.Format(" \"HEALTH_FACILITY_ID\" = {0} ", hfId);
                    odsHealthFacilityCohortData.SelectParameters.Clear();
                    odsHealthFacilityCohortData.SelectParameters.Add("where", where);
                    Session["HealthFacilityCohortData-Where"] = where;
                    btnEdit.Visible = false;
                }
                string _id = (string)Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int id = int.Parse(_id);
                    HealthFacilityCohortData o = HealthFacilityCohortData.GetHealthFacilityCohortDataById(id);
                    
                    ddlYear.SelectedValue = o.Year.ToString();
                    txtCohort.Text = o.Cohort.ToString();
                    txtNotes.Text = o.Notes;
                    healthCenterId = o.HealthFacilityId.ToString();
                    string where = string.Format(" \"ID\" = {0} ", id);
                    odsHealthFacilityCohortData.SelectParameters.Clear();
                    odsHealthFacilityCohortData.SelectParameters.Add("where", where);
                    Session["HealthFacilityCohortData-Where"] = where;
                    btnAdd.Visible = false;
                }
              

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    //protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    //{
    //    healthCenterId = txtHealthFacilityId.SelectedItemID.ToString();
    //}

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                int hfId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                string _hfId = (string)Request.QueryString["hfId"];
                if (!String.IsNullOrEmpty(_hfId))
                {
                    hfId = int.Parse(_hfId);
                    HealthFacilityCohortData o = new HealthFacilityCohortData();
                    if (yearExists(int.Parse(ddlYear.SelectedValue)))
                        return;

                    o.HealthFacilityId = hfId;
                    o.Year = int.Parse(ddlYear.SelectedValue);
                    o.Cohort = int.Parse(txtCohort.Text);
                    o.Notes = txtNotes.Text.Replace("'", @"''");
                    o.ModifiedOn = DateTime.Now;
                    o.ModifiedBy = userId;

                    int i = HealthFacilityCohortData.Insert(o);

                    if (i > 0)
                    {
                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        gvHealthFacilityCohortData.DataBind();
                        ClearControls(this);

                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = true;
                        lblError.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }


    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int id = -1;
                string _id = Request.QueryString["id"].ToString();
                int.TryParse(_id, out id);
                int userId = CurrentEnvironment.LoggedUser.Id;

                HealthFacilityCohortData o = HealthFacilityCohortData.GetHealthFacilityCohortDataById(id);

                if (yearExists(int.Parse(ddlYear.SelectedValue)) && (o.Year != int.Parse(ddlYear.SelectedValue)))
                    return;

               // o.HealthFacilityId = CurrentEnvironment.LoggedUser.HealthFacilityId;
                o.Year = int.Parse(ddlYear.SelectedValue);
                o.Cohort = int.Parse(txtCohort.Text);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = HealthFacilityCohortData.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvHealthFacilityCohortData.DataBind();
                    ClearControls(this);

                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = true;
                    lblError.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvHealthFacilityCohortData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacilityCohortData.PageIndex = e.NewPageIndex;
    }

    private void ClearControls(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
                ClearControls(c);
            else
            {
                if (c is TextBox)
                    (c as TextBox).Text = "";
                //if (c is CheckBox)
                //    (c as CheckBox).Checked = false;
                if (c is DropDownList)
                    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }
    protected bool yearExists(int year)
    {
        if (HealthFacilityCohortData.GetHealthFacilityCohortDataByYear(year) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected void gvHealthFacilityCohortData_DataBound(object sender, System.EventArgs e)
    {
        if (gvHealthFacilityCohortData.Rows.Count > 0)
            btnExcel.Visible = true;
        else
            btnExcel.Visible = true;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (Session["HealthFacilityCohortData-Where"] != null)
        {
            string where = Session["HealthFacilityCohortData-Where"].ToString();
            if (!string.IsNullOrEmpty(where))
            {
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                //List<HealthFacilityCohortData> list = HealthFacilityCohortData.GetPagedHealthFacilityCohortDataList(ref maximumRows, ref startRowIndex, where);
                //gvExport.DataSource = list;
                //gvExport.DataBind();
                //gvExport.Visible = true;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=HealthFacilityCohortData.xls");
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
