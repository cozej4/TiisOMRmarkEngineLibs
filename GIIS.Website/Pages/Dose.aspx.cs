using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Pages_Dose : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewDose") && (CurrentEnvironment.LoggedUser != null))
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
                this.lblDoseNumber.Text = wtList["DoseDoseNumber"];
                this.lblAgeDefinitionId.Text = wtList["DoseAgeDefinition"];
                this.lblFullname.Text = wtList["DoseFullname"];
                this.lblNotes.Text = wtList["DoseNotes"];
                this.lblIsActive.Text = wtList["DoseIsActive"];
               // this.lblFromAgeDef.Text = wtList["DoseFromAgeDefinition"];
               // this.lblToAgeDef.Text = wtList["DoseToAgeDefinition"];

                //grid header text
                gvDose.Columns[1].HeaderText = wtList["DoseFullname"];
                gvDose.Columns[2].HeaderText = wtList["DoseAgeDefinition"];
                gvDose.Columns[3].HeaderText = wtList["DoseScheduledVaccination"];
                gvDose.Columns[4].HeaderText = wtList["DoseDoseNumber"];
                gvDose.Columns[5].HeaderText = wtList["DoseNotes"];
                gvDose.Columns[6].HeaderText = wtList["DoseIsActive"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddDose");
                this.btnEdit.Visible = actionList.Contains("EditDose");
                this.btnRemove.Visible = actionList.Contains("RemoveDose");

                //buttons
                this.btnAdd.Text = wtList["DoseAddButton"];
                this.btnEdit.Text = wtList["DoseEditButton"];
                this.btnRemove.Text = wtList["DoseRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["DoseSuccessText"];
                this.lblWarning.Text = wtList["DoseWarningText"];
                this.lblError.Text = wtList["DoseErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["DosePageTitle"];

                //validators
                cvDose.ErrorMessage = wtList["DoseMandatory"];
               // cvScheduledVaccination.ErrorMessage = wtList["DoseScheduledVaccinationValidator"];
                //cvAgeDefinition.ErrorMessage = wtList["DoseAgeDefinitionValidator"];
               
                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    Dose o = Dose.GetDoseById(id);
                    ddlScheduledVaccination.SelectedValue = o.ScheduledVaccinationId.ToString();
                    ddlAgeDefinition.SelectedValue = o.AgeDefinitionId.ToString();
                    if (o.FromAgeDefinitionId > 0)
                        ddlFromAgeDef.SelectedValue = o.FromAgeDefinitionId.ToString();
                    if (o.ToAgeDefinitionId > 0)
                        ddlToAgeDef.SelectedValue = o.ToAgeDefinitionId.ToString();

                    txtDoseNumber.Text = o.DoseNumber.ToString();
                    txtFullname.Text = o.Fullname.ToString();
                    txtNotes.Text = o.Notes;
                    rblIsActive.Items[0].Selected = o.IsActive;
                    rblIsActive.Items[1].Selected = !o.IsActive;
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
        odsDose.SelectParameters.Clear();
        odsDose.SelectParameters.Add("i", id.ToString());
        odsDose.DataBind();
      
    }

    protected bool Exists(int schVaccinationId, int dosenumber)
    {
        if (Dose.GetDoseBySchVaccinationAndDoseNumber(schVaccinationId,dosenumber) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected bool NameExists(string fullname)
    {
        if (Dose.GetDoseByFullname(fullname) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                if (Exists(int.Parse(ddlScheduledVaccination.SelectedValue), int.Parse(txtDoseNumber.Text)))
                    return;
                if (NameExists(txtFullname.Text.Replace("'", @"''")))
                    return;
                Dose o = new Dose();
               
                o.ScheduledVaccinationId = int.Parse(ddlScheduledVaccination.SelectedValue);
                o.AgeDefinitionId = int.Parse(ddlAgeDefinition.SelectedValue);
                o.DoseNumber = int.Parse(txtDoseNumber.Text);
                o.Fullname = txtFullname.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                if (ddlFromAgeDef.SelectedIndex > 0)
                    o.FromAgeDefinitionId = int.Parse(ddlFromAgeDef.SelectedValue);
                if (ddlToAgeDef.SelectedIndex > 0)
                    o.ToAgeDefinitionId = int.Parse(ddlToAgeDef.SelectedValue);

                int i = Dose.Insert(o);

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

                Dose o = Dose.GetDoseById(id);
                if (Exists(ddlScheduledVaccination.SelectedIndex, int.Parse(txtDoseNumber.Text)) && (o.ScheduledVaccinationId != int.Parse(ddlScheduledVaccination.SelectedValue)) && (o.DoseNumber != int.Parse(txtDoseNumber.Text)))
                    return;
                if (NameExists(txtFullname.Text.Replace("'", @"''")) && (o.Fullname != txtFullname.Text))
                    return;
                o.ScheduledVaccinationId = int.Parse(ddlScheduledVaccination.SelectedValue);
                o.AgeDefinitionId = int.Parse(ddlAgeDefinition.SelectedValue);
                o.DoseNumber = int.Parse(txtDoseNumber.Text);
                o.Fullname = txtFullname.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (ddlFromAgeDef.SelectedIndex > 0)
                    o.FromAgeDefinitionId = int.Parse(ddlFromAgeDef.SelectedValue);
                else
                    o.FromAgeDefinitionId = null;
                if (ddlToAgeDef.SelectedIndex > 0)
                    o.ToAgeDefinitionId = int.Parse(ddlToAgeDef.SelectedValue);
                else
                    o.ToAgeDefinitionId = null;

                int i = Dose.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(id);
                    //ClearControls(this);
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

            int i = Dose.Remove(id);

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

    protected void gvDose_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDose.PageIndex = e.NewPageIndex;
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
}
