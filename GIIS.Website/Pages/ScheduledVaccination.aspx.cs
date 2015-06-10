using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;


public partial class _ScheduledVaccination : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Validator
        //DateTime date = DateTime.ParseExact(txtEntryDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
        //cvEntryDate.ControlToValidate = date;
        //cvEntryDate.ValueToCompare = DateTime.Today.ToString(ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat);

        #endregion

        if (!this.Page.IsPostBack)
        {
            Page.DataBind();

            List<string> actionList = null;
            string sessionNameAction = "";
            if (CurrentEnvironment.LoggedUser != null)
            {
                sessionNameAction = "__GIS_actionList_" + CurrentEnvironment.LoggedUser.Id;
                actionList = (List<string>)Session[sessionNameAction];
            }

            if ((actionList != null) && actionList.Contains("ViewScheduledVaccination") && (CurrentEnvironment.LoggedUser != null))
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
                // this.lblHl7VaccineId.Text = wtList["ScheduledVaccinationHl7Vaccine"];
                this.lblItemId.Text = wtList["ScheduledVaccinationItem"];
                this.lblEntryDate.Text = wtList["ScheduledVaccinationEntryDate"];
                this.lblExitDate.Text = wtList["ScheduledVaccinationExitDate"];
                // this.lblStatus.Text = wtList["ScheduledVaccinationStatus"];
                this.lblDeseases.Text = wtList["ScheduledVaccinationDeseases"];
                this.lblNotes.Text = wtList["ScheduledVaccinationNotes"];
                this.lblIsActive.Text = wtList["ScheduledVaccinationIsActive"];
                this.rblIsActive.Items[0].Text = wtList["ScheduledVaccinationYes"];
                this.rblIsActive.Items[1].Text = wtList["ScheduledVaccinationNo"];

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
                //this.btnAdd.Visible = actionList.Contains("AddScheduledVaccination");
                //this.btnEdit.Visible = actionList.Contains("EditScheduledVaccination");
                //this.btnRemove.Visible = actionList.Contains("RemoveScheduledVaccination");

                //buttons
                this.btnAdd.Text = wtList["ScheduledVaccinationAddButton"];
                this.btnEdit.Text = wtList["ScheduledVaccinationEditButton"];
                this.btnRemove.Text = wtList["ScheduledVaccinationRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["ScheduledVaccinationSuccessText"];
                this.lblWarning.Text = wtList["ScheduledVaccinationWarningText"];
                this.lblError.Text = wtList["ScheduledVaccinationErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["ScheduledVaccinationPageTitle"];

                //validators
                ceEntryDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revEntryDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revEntryDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;
                ceExitDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revExitDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revExitDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;
                cvScheduledVaccination.ErrorMessage = wtList["ScheduledVaccinationMandatory"];
               // cvItem.ErrorMessage = wtList["ScheduledVaccinationItemValidator"];
                // cvtxtEntryDate.ErrorMessage = wtList["EntryDateValidator"];
                //cvEntryDate.ErrorMessage = wtList["EntryDateFieldValidator"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    ScheduledVaccination o = ScheduledVaccination.GetScheduledVaccinationById(id);
                    //ddlHl7Vaccine.SelectedValue = o.Hl7VaccineId.ToString();
                    ddlItem.SelectedValue = o.ItemId.ToString();
                    txtName.Text = o.Name.ToString();
                    txtCode.Text = o.Code.ToString();
                    txtNotes.Text = o.Notes;
                    txtEntryDate.Text = o.EntryDate.ToString(ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString());
                    txtExitDate.Text = o.ExitDate.ToString(ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString());
                    if (o.ExitDate.ToString("yyyy-MM-dd").Equals("0001-01-01"))
                        txtExitDate.Text = String.Empty;
                    rblIsActive.Items[0].Selected = o.IsActive;
                    rblIsActive.Items[1].Selected = !o.IsActive;
                    txtDeseases.Text = o.Deseases;
                    gridview_Databind(id);
                    btnAdd.Visible = false;
                }
                else
                {
                    btnEdit.Visible = false;
                    btnRemove.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void gridview_Databind(int id)
    {
        //gridview databind
        odsScheduledVaccination.SelectParameters.Clear();
        odsScheduledVaccination.SelectParameters.Add("i", id.ToString());
        odsScheduledVaccination.DataBind();
      
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                ScheduledVaccination o = new ScheduledVaccination();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                //  o.Hl7VaccineId = int.Parse(ddlHl7Vaccine.SelectedValue);
                o.ItemId = int.Parse(ddlItem.SelectedValue);
                DateTime date = DateTime.ParseExact(txtEntryDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                o.EntryDate = date;
                if (txtExitDate.Text != String.Empty)
                {
                    DateTime exitdate = DateTime.ParseExact(txtExitDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                    o.ExitDate = exitdate;
                }
                // o.Status = bool.Parse(rblStatus.SelectedValue);
                o.Deseases = txtDeseases.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (o.ExitDate < DateTime.Now)
                {
                    o.Status = false;
                }
                else
                { o.Status = true; }

                int i = ScheduledVaccination.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(i);
                    ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
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

                ScheduledVaccination o = ScheduledVaccination.GetScheduledVaccinationById(id);

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                //  o.Hl7VaccineId = int.Parse(ddlHl7Vaccine.SelectedValue);
                o.ItemId = int.Parse(ddlItem.SelectedValue);
                DateTime date = DateTime.ParseExact(txtEntryDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                o.EntryDate = date;
                if (txtExitDate.Text != String.Empty)
                {
                    DateTime exitdate = DateTime.ParseExact(txtExitDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                    o.ExitDate = exitdate;
                }
                else
                {
                    o.ExitDate = DateTime.Parse("0001-01-01");
                }
                // o.Status = bool.Parse(rblStatus.SelectedValue);
                o.Deseases = txtDeseases.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (o.ExitDate < DateTime.Now)
                {
                    o.Status = false;
                }
                else
                { o.Status = true; }
                int i = ScheduledVaccination.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(id);

                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
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

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int id = -1;
            string _id = Request.QueryString["id"].ToString();
            int.TryParse(_id, out id);
            int userId = CurrentEnvironment.LoggedUser.Id;

            int i = ScheduledVaccination.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gridview_Databind(id);
                ClearControls(this);
            }
            else
            {
                lblSuccess.Visible = false;
                lblWarning.Visible = true;
                lblError.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvScheduledVaccination_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvScheduledVaccination.PageIndex = e.NewPageIndex;
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

                if (c is DropDownList)
                    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }

    protected void gvScheduledVaccination_RowDatabound(object sender, GridViewRowEventArgs e)
    {

        //string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        //((BoundField)gvScheduledVaccination.Columns[4]).DataFormatString = "{0:" + dateformat + "}";
        //((BoundField)gvScheduledVaccination.Columns[5]).DataFormatString = "{0:" + dateformat + "}";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string exitDate = e.Row.Cells[5].Text;
            DateTime date = DateTime.ParseExact(exitDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture);
            if (date.ToString("yyyy-MM-dd").Equals("0001-01-01"))
                e.Row.Cells[5].Text = String.Empty;
        }
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItem.SelectedIndex > 0)
        {
            int id = int.Parse(ddlItem.SelectedValue);
            Item vaccine = Item.GetItemById(id);
            txtName.Text = vaccine.Name;
            txtCode.Text = vaccine.Code;
        }
    }

    protected void ValidateDates(object sender, ServerValidateEventArgs e)
    {
        ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
        if (txtEntryDate.Text != String.Empty || txtExitDate.Text != String.Empty)
        {
            DateTime datefrom = DateTime.MinValue;
            if (txtEntryDate.Text != String.Empty)
                datefrom = DateTime.ParseExact(txtEntryDate.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);
            DateTime dateto = DateTime.MaxValue;
            if (txtExitDate.Text != String.Empty)
                dateto = DateTime.ParseExact(txtExitDate.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);
            e.IsValid = datefrom <= dateto; //&& datefrom <= DateTime.Today.Date;
        }
    }
}