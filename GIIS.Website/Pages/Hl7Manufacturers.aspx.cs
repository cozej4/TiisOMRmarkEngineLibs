using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Hl7Manufacturers : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHl7Manufacturers") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Hl7Manufacturers-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Hl7Manufacturers");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Hl7Manufacturers-dictionary" + language, wtList);
                }

                //controls
                this.lblMvxCode.Text = wtList["Hl7ManufacturersMvxCode"];
                this.lblName.Text = wtList["Hl7ManufacturersName"];
                this.lblNotes.Text = wtList["Hl7ManufacturersNotes"];
                this.lblIsActive.Text = wtList["Hl7ManufacturersIsActive"];
                this.lblLastUpdated.Text = wtList["Hl7ManufacturersLastUpdated"];
                this.lblInternalId.Text = wtList["Hl7ManufacturersInternal"];

                //grid header text
                gvHl7Manufacturers.Columns[1].HeaderText = wtList["Hl7ManufacturersMvxCode"];
                gvHl7Manufacturers.Columns[2].HeaderText = wtList["Hl7ManufacturersName"];
                gvHl7Manufacturers.Columns[3].HeaderText = wtList["Hl7ManufacturersNotes"];
                gvHl7Manufacturers.Columns[4].HeaderText = wtList["Hl7ManufacturersIsActive"];
                gvHl7Manufacturers.Columns[5].HeaderText = wtList["Hl7ManufacturersLastUpdated"];
                gvHl7Manufacturers.Columns[6].HeaderText = wtList["Hl7ManufacturersInternal"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddHl7Manufacturers");
                this.btnEdit.Visible = actionList.Contains("EditHl7Manufacturers");
                this.btnRemove.Visible = actionList.Contains("RemoveHl7Manufacturers");

                //buttons
                this.btnAdd.Text = wtList["Hl7ManufacturersAddButton"];
                this.btnEdit.Text = wtList["Hl7ManufacturersEditButton"];
                this.btnRemove.Text = wtList["Hl7ManufacturersRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["Hl7ManufacturersSuccessText"];
                this.lblWarning.Text = wtList["Hl7ManufacturersWarningText"];
                this.lblError.Text = wtList["Hl7ManufacturersErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["Hl7ManufacturersPageTitle"];

                //validators
                ceLastUpdated.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revLastUpdated.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revLastUpdated.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;
                //cvHl7Manufacturers.ErrorMessage = wtList["Hl7ManufacturersMandatory"];
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                Hl7Manufacturers o = new Hl7Manufacturers();

                o.MvxCode = txtMvxCode.Text;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.LastUpdated = DateTime.Parse(txtLastUpdated.Text);
                o.InternalId = int.Parse(txtInternalId.Text);

                int i = Hl7Manufacturers.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvHl7Manufacturers.DataBind();
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

                Hl7Manufacturers o = Hl7Manufacturers.GetHl7ManufacturersById(id);

                o.MvxCode = txtMvxCode.Text;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.LastUpdated = DateTime.Parse(txtLastUpdated.Text);
                o.InternalId = int.Parse(txtInternalId.Text);

                int i = Hl7Manufacturers.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvHl7Manufacturers.DataBind();
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


    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int id = -1;
            string _id = Request.QueryString["id"].ToString();
            int.TryParse(_id, out id);
            int userId = CurrentEnvironment.LoggedUser.Id;

            int i = Hl7Manufacturers.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvHl7Manufacturers.DataBind();
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

    protected void gvHl7Manufacturers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHl7Manufacturers.PageIndex = e.NewPageIndex;
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
}
