using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _UserRole : System.Web.UI.Page
{
    ICollection<Role> _roles;
    ICollection<Role> _userRoles;

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

            if ((actionList != null) && actionList.Contains("ViewUserRole") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["UserRole-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "UserRole");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("UserRole-dictionary" + language, wtList);
                }

                //controls
                this.lblRole.Text = wtList["UserRoleRole"];
                this.lblUser.Text = wtList["UserRoleUser"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddRoleAction");
                this.btnRemove.Visible = actionList.Contains("RemoveRoleAction");
                this.btnAddAll.Visible = actionList.Contains("AddAllRoleAction");
                this.btnRemoveAll.Visible = actionList.Contains("RemoveAllRoleAction");

                //message
                this.lblSuccess.Text = wtList["UserRoleSuccessText"];
                this.lblWarning.Text = wtList["UserRoleWarningText"];
                this.lblError.Text = wtList["UserRoleErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["UserRolePageTitle"];

                //validators
                BindUsers(GIIS.DataLayer.User.GetUserByStatus(true));
                
                _roles = Role.GetLeftRolesOfUser(int.Parse(ddlUser.SelectedValue.ToString()));
                BindLeftRolesOfUser(_roles);

                _userRoles = Role.GetRolesOfUser(int.Parse(ddlUser.SelectedValue.ToString()));
                BindRolesOfUser(_userRoles);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void BindUsers(ICollection<User> user)
    {
        ddlUser.DataSource = user;
        ddlUser.DataValueField = "Id";
        ddlUser.DataTextField = "Username";

        ddlUser.DataBind();
    }
    protected void BindLeftRolesOfUser(ICollection<Role> roles)
    {
        lbRoles.DataSource = roles;
        lbRoles.DataValueField = "Id";
        lbRoles.DataTextField = "Name";

        lbRoles.DataBind();
    }
    protected void BindRolesOfUser(ICollection<Role> roles)
    {
        lbUserRoles.DataSource = roles;
        lbUserRoles.DataValueField = "Id";
        lbUserRoles.DataTextField = "Name";

        lbUserRoles.DataBind();
    }

       protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string machineName = Request.ServerVariables["REMOTE_HOST"].ToString();
                int i = 0;
                if ((lbRoles.Items.Count > 0))
                {
                    UserRole o = new UserRole();
                    o.UserId = int.Parse(ddlUser.SelectedValue);

                    for (int j = 0; j <= lbRoles.Items.Count - 1; j++)
                    {
                        if (lbRoles.Items[j].Selected)
                        {
                            int roleId = int.Parse(lbRoles.Items[j].Value.ToString());
                            if (UserRole.Exists(o.UserId, roleId) == 0)
                            {
                                o.RoleId = roleId;
                                o.IsActive = true;
                                i = UserRole.Insert(o);
                            }
                        }
                    }

                    _roles = Role.GetLeftRolesOfUser(o.UserId);
                    BindLeftRolesOfUser(_roles);

                    _userRoles = Role.GetRolesOfUser(o.UserId);
                    BindRolesOfUser(_userRoles);
                    
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
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

    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string machineName = Request.ServerVariables["REMOTE_HOST"].ToString();
                int i = 0;
                if ((lbRoles.Items.Count > 0))
                {
                    UserRole o = new UserRole();
                    o.UserId = int.Parse(ddlUser.SelectedValue);

                    for (int j = 0; j <= lbRoles.Items.Count - 1; j++)
                    {
                        int roleId = int.Parse(lbRoles.Items[j].Value.ToString());
                        if (UserRole.Exists(o.UserId, roleId) == 0)
                        {
                            o.RoleId = roleId; ;
                            o.IsActive = true;
                            i = UserRole.Insert(o);
                        }
                    }

                    _roles = Role.GetLeftRolesOfUser(o.UserId);
                    BindLeftRolesOfUser(_roles);

                    _userRoles = Role.GetRolesOfUser(o.UserId);
                    BindRolesOfUser(_userRoles);

                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
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

    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string machineName = Request.ServerVariables["REMOTE_HOST"].ToString();
                int i = 0;
                if ((lbUserRoles.Items.Count > 0))
                {
                    int ddluserId = int.Parse(ddlUser.SelectedValue);

                    for (int j = 0; j <= lbUserRoles.Items.Count - 1; j++)
                    {
                        int roleId = int.Parse(lbUserRoles.Items[j].Value.ToString());
                        i = UserRole.DeleteByUserAndRole(ddluserId, roleId);
                    }

                    _roles = Role.GetLeftRolesOfUser(ddluserId);
                    BindLeftRolesOfUser(_roles);

                    _userRoles = Role.GetRolesOfUser(ddluserId);
                    BindRolesOfUser(_userRoles);

                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
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
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string machineName = Request.ServerVariables["REMOTE_HOST"].ToString();
                int i = 0;
                if (lbUserRoles.Items.Count > 0)
                {
                    int ddluserId = int.Parse(ddlUser.SelectedValue);
                    for (int j = 0; j <= lbUserRoles.Items.Count - 1; j++)
                    {
                        if (lbUserRoles.Items[j].Selected)
                        {
                            int roleId = int.Parse(lbUserRoles.Items[j].Value.ToString());
                            i = UserRole.DeleteByUserAndRole(ddluserId, roleId);
                        }
                    }

                    _roles = Role.GetLeftRolesOfUser(ddluserId);
                    BindLeftRolesOfUser(_roles);

                    _userRoles = Role.GetRolesOfUser(ddluserId);
                    BindRolesOfUser(_userRoles);

                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
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
    //protected void ddlUser_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    if (this.ddlUser.SelectedValue != null)
    //    {
    //        _roles = Role.GetLeftRolesOfUser(int.Parse(ddlUser.SelectedValue.ToString()));
    //        BindLeftRolesOfUser(_roles);

    //        _userRoles = Role.GetRolesOfUser(int.Parse(ddlUser.SelectedValue.ToString()));
    //        BindRolesOfUser(_userRoles);
    //    }
    //}
    
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
    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddlUser.SelectedValue != null)
        {
            _roles = Role.GetLeftRolesOfUser(int.Parse(ddlUser.SelectedValue.ToString()));
            BindLeftRolesOfUser(_roles);

            _userRoles = Role.GetRolesOfUser(int.Parse(ddlUser.SelectedValue.ToString()));
            BindRolesOfUser(_userRoles);
        }
    }
}
