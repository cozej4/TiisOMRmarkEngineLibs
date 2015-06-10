using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Role : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewRole") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Role-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Role");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Role-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["RoleName"];
                this.lblIsActive.Text = wtList["RoleIsActive"];
                this.lblNotes.Text = wtList["RoleNotes"];

                //grid header text
                gvRole.Columns[1].HeaderText = wtList["RoleName"];
                gvRole.Columns[3].HeaderText = wtList["RoleIsActive"];
                gvRole.Columns[2].HeaderText = wtList["RoleNotes"];

                gvExport.Columns[1].HeaderText = wtList["RoleName"];
                gvExport.Columns[3].HeaderText = wtList["RoleIsActive"];
                gvExport.Columns[2].HeaderText = wtList["RoleNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddRole");
                this.btnEdit.Visible = actionList.Contains("EditRole");
                this.btnRemove.Visible = actionList.Contains("RemoveRole");

                //buttons
                this.btnAdd.Text = wtList["RoleAddButton"];
                this.btnEdit.Text = wtList["RoleEditButton"];
                this.btnRemove.Text = wtList["RoleRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["RoleSuccessText"];
                this.lblWarning.Text = wtList["RoleWarningText"];
                this.lblError.Text = wtList["RoleErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["RolePageTitle"];

                //validators
                rfvName.ErrorMessage = wtList["RoleMandatory"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    Role o = Role.GetRoleById(id);
                    txtName.Text = o.Name;
                    txtNotes.Text = o.Notes;
                    rblIsActive.Items[0].Selected = o.IsActive;
                    rblIsActive.Items[1].Selected = !o.IsActive;
                    btnAdd.Visible = false;
                }
                else
                {
                    btnEdit.Visible = false;
                    btnRemove.Visible = false;
                    Session["Role-Where"] = "1 = 1";
                }
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
                if (nameExists(txtName.Text.Replace("'", @"''")))
                    return;
                Role o = new Role();

                o.Name = txtName.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = Role.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvRole.DataBind();
                    Session["Role-Where"] = "1 = 1";
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

                Role o = Role.GetRoleById(id);
                if (o.Id == 1)
                    return;
                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;

                o.Name = txtName.Text;
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = Role.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    Session["Role-Where"] = "1 = 1";
                    gvRole.DataBind();
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

            Role o = Role.GetRoleById(id);
            if (o.Name == "Administrator")
                return;

            int i = Role.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvRole.DataBind();
                Session["Role-Where"] = "1 = 1";
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

    protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRole.PageIndex = e.NewPageIndex;
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

    protected bool nameExists(string name)
    {
        if (Role.GetRoleByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
   
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        if (Session["Role-Where"] != null)
        {
            string where = Session["Role-Where"].ToString();
            if (!string.IsNullOrEmpty(where))
            {
                int maximumRows = int.MaxValue;
                int startRowIndex = 0;

                List<Role> list = Role.GetPagedRoleList(ref maximumRows, ref startRowIndex, where);
                gvExport.DataSource = list;
                gvExport.DataBind();
                gvExport.Visible = true;
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Roles.xls");
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
