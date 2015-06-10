using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Pages_EditUser : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewUser") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["User-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "User");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("User-dictionary" + language, wtList);
                }
              
                //controls
                this.lblUsername.Text = wtList["UserUsername"];
                this.lblFirstname.Text = wtList["UserFirstname"];
                this.lblLastname.Text = wtList["UserLastname"];
                this.lblNotes.Text = wtList["UserNotes"];
                this.lblEmail.Text = wtList["UserEmail"];
                this.lblHealthFacilityId.Text = wtList["UserHealthFacility"];
                this.lblIsActive.Text = wtList["UserIsActive"];
                this.rblIsActive.Items[0].Text = wtList["UserYes"];
                this.rblIsActive.Items[1].Text = wtList["UserNo"];

                //grid header text
                gvUser.Columns[1].HeaderText = wtList["UserUsername"];
               // gvUser.Columns[2].HeaderText = wtList["UserPassword"];
                gvUser.Columns[2].HeaderText = wtList["UserHealthFacility"];
                gvUser.Columns[3].HeaderText = wtList["UserFirstname"];
                gvUser.Columns[4].HeaderText = wtList["UserLastname"];
                gvUser.Columns[7].HeaderText = wtList["UserIsActive"];
                gvUser.Columns[6].HeaderText = wtList["UserNotes"];
                gvUser.Columns[5].HeaderText = wtList["UserEmail"];

                //actions
                this.btnEdit.Visible = actionList.Contains("EditUser");
                this.btnRemove.Visible = actionList.Contains("RemoveUser");

                //validators
                cvUser.ErrorMessage = wtList["UserMandatory"];
                revHealthFacility.Text = wtList["UserHealthFacilityValidator"];
                //cvRole.ErrorMessage = wtList["UserRoleValidator"];
                revFirstname.ErrorMessage = wtList["UserNameValidator"];
                revLastname.ErrorMessage = wtList["UserLastNameValidator"];
                revEmail.ErrorMessage = wtList["UserEmailValidator"];
                revPassword.ErrorMessage = wtList["UserPasswordValidator"];
                revConfirmPassword.ErrorMessage = wtList["UserPasswordValidator"];

                //Page Title
                this.lblTitle.Text = wtList["EditUserPageTitle"];

                //message
                this.lblSuccess.Text = wtList["UserSuccessText"];
                this.lblWarning.Text = wtList["UserWarningText"];
                this.lblError.Text = wtList["UserErrorText"];
                lblSuccess.Visible = ((String)HttpContext.Current.Session["_successEditUser"] == "1");

                HttpContext.Current.Session["_successEditUser"] = "0";

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"].ToString();
                int.TryParse(_id, out id);
                GIIS.DataLayer.User o = GIIS.DataLayer.User.GetUserById(id);
                txtUsername.Text = o.Username;
                txtHealthFacilityId.SelectedItemText = o.HealthFacility.Name.ToString();
                txtEmail.Text = o.Email;
                txtFirstname.Text = o.Firstname;
                txtLastname.Text = o.Lastname;
                txtNotes.Text = o.Notes;
                rblIsActive.Items[0].Selected = o.IsActive;
                rblIsActive.Items[1].Selected = !o.IsActive;
                ddlRole.DataBind();
                ddlRole.SelectedValue = UserRole.GetUserRoleByUserId(o.Id).RoleId.ToString();
                healthCenterId = o.HealthFacilityId.ToString();
                
                //gridview databind
                odsUser.SelectParameters.Clear();
                odsUser.SelectParameters.Add("i", id.ToString());
                odsUser.DataBind();
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        else
        {
            //postback
        }
    }

    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        healthCenterId = txtHealthFacilityId.SelectedItemID.ToString();
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
                revHealthFacility.Visible = false;
                GIIS.DataLayer.User o = GIIS.DataLayer.User.GetUserById(id);

                if (usernameExists(txtUsername.Text.Replace("'", @"''")) && (o.Username != txtUsername.Text.Trim()))
                    return;

                if (healthCenterId == null)
                {
                    revHealthFacility.Visible = true;
                    return;
                }
                if (o.HealthFacilityId != int.Parse(healthCenterId))
                {
                    if (HealthFacility.GetHealthFacilityById(int.Parse(healthCenterId)) == null)
                    {
                        revHealthFacility.Visible = true;
                        return;
                    }
                }

                if (o.Id != 1)
                {
                    o.Username = txtUsername.Text.Replace("'", @"''");
                    o.HealthFacilityId = int.Parse(healthCenterId);
                    if (txtNewPassword.Text != String.Empty)
                      if (txtNewPassword.Text == txtConfirmPassword.Text)
                          o.Password = Helper.ComputeHash(txtNewPassword.Text.Replace("'", @"''"));
                }
                o.Firstname = txtFirstname.Text.Replace("'", @"''");
                o.Lastname = txtLastname.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Deleted = false;
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.Email = txtEmail.Text.Replace("'", @"''");                

                int i = GIIS.DataLayer.User.Update(o);

                if (i > 0)
                {
                    if (o.Id != 1)
                    {
                        int roleId = int.Parse(ddlRole.SelectedValue.ToString());

                        UserRole u = UserRole.GetUserRoleByUserId(id);
                        u.RoleId = roleId;
                        u.IsActive = true;
                        int j = UserRole.Update(u);
                    }
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    //gridview databind
                    odsUser.SelectParameters.Clear();
                    odsUser.SelectParameters.Add("i", id.ToString());
                    odsUser.DataBind();
              
                    HttpContext.Current.Session["_successEditUser"] = "1";
                    Response.Redirect(Request.RawUrl);
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
            string machineName = Request.ServerVariables["REMOTE_HOST"].ToString();

            GIIS.DataLayer.User o = GIIS.DataLayer.User.GetUserById(id);
            if (o.Username == "admin")
                return;

            int i = GIIS.DataLayer.User.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvUser.DataBind();
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

    protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUser.PageIndex = e.NewPageIndex;
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
    protected bool usernameExists(string name)
    {
        if (GIIS.DataLayer.User.GetUserByUsername(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
}