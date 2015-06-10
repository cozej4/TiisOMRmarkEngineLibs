using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Hl7Vaccines : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHl7Vaccines") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Hl7Vaccines-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Hl7Vaccines");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Hl7Vaccines-dictionary" + language, wtList);
                }

                //controls
                this.lblCvxCode.Text = wtList["Hl7VaccinesCvxCode"];
                this.lblCode.Text = wtList["Hl7VaccinesCode"];
                this.lblFullname.Text = wtList["Hl7VaccinesFullname"];
                this.lblNotes.Text = wtList["Hl7VaccinesNotes"];
                this.lblVaccineStatus.Text = wtList["Hl7VaccinesVaccineStatus"];
                this.lblInternalId.Text = wtList["Hl7VaccinesInternal"];
                this.lblNonVaccine.Text = wtList["Hl7VaccinesNonVaccine"];
                this.lblUpdateDate.Text = wtList["Hl7VaccinesUpdateDate"];

                //grid header text
                gvHl7Vaccines.Columns[1].HeaderText = wtList["Hl7VaccinesCvxCode"];
                gvHl7Vaccines.Columns[2].HeaderText = wtList["Hl7VaccinesCode"];
                gvHl7Vaccines.Columns[3].HeaderText = wtList["Hl7VaccinesFullname"];
                gvHl7Vaccines.Columns[4].HeaderText = wtList["Hl7VaccinesNotes"];
                gvHl7Vaccines.Columns[5].HeaderText = wtList["Hl7VaccinesVaccineStatus"];
                gvHl7Vaccines.Columns[6].HeaderText = wtList["Hl7VaccinesInternal"];
                gvHl7Vaccines.Columns[7].HeaderText = wtList["Hl7VaccinesNonVaccine"];
                gvHl7Vaccines.Columns[8].HeaderText = wtList["Hl7VaccinesUpdateDate"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddHl7Vaccines");
                this.btnEdit.Visible = actionList.Contains("EditHl7Vaccines");

                //buttons
                this.btnAdd.Text = wtList["Hl7VaccinesAddButton"];
                this.btnEdit.Text = wtList["Hl7VaccinesEditButton"];

                //message
                this.lblSuccess.Text = wtList["Hl7VaccinesSuccessText"];
                this.lblWarning.Text = wtList["Hl7VaccinesWarningText"];
                this.lblError.Text = wtList["Hl7VaccinesErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["Hl7VaccinesPageTitle"];

                //validators
                ceUpdateDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revUpdateDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revUpdateDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;
                //cvHl7Vaccines.ErrorMessage = wtList["Hl7VaccinesMandatory"];
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

                Hl7Vaccines o = new Hl7Vaccines();

                o.CvxCode = txtCvxCode.Text;
                o.Code = txtCode.Text.Replace("'", @"''");
                o.Fullname = txtFullname.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.VaccineStatus = bool.Parse(rblVaccineStatus.SelectedValue);
                o.InternalId = int.Parse(txtInternalId.Text);
                o.NonVaccine = bool.Parse(rblNonVaccine.SelectedValue);
                o.UpdateDate = DateTime.Parse(txtUpdateDate.Text);

                int i = Hl7Vaccines.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvHl7Vaccines.DataBind();
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

                Hl7Vaccines o = Hl7Vaccines.GetHl7VaccinesById(id);

                o.CvxCode = txtCvxCode.Text;
                o.Code = txtCode.Text.Replace("'", @"''");
                o.Fullname = txtFullname.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.VaccineStatus = bool.Parse(rblVaccineStatus.SelectedValue);
                o.InternalId = int.Parse(txtInternalId.Text);
                o.NonVaccine = bool.Parse(rblNonVaccine.SelectedValue);
                o.UpdateDate = DateTime.Parse(txtUpdateDate.Text);

                int i = Hl7Vaccines.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvHl7Vaccines.DataBind();
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

    protected void gvHl7Vaccines_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHl7Vaccines.PageIndex = e.NewPageIndex;
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
    protected void gvHl7Vaccines_Databound(object sender, System.EventArgs e)
    {
        if (gvHl7Vaccines.Rows.Count > 0)
        {
            string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
            ((BoundField)gvHl7Vaccines.Columns[8]).DataFormatString = "{0:" + dateformat + "}";

        }
    }
}
