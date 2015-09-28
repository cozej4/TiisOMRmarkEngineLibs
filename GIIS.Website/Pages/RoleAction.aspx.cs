//*******************************************************************************
//Copyright 2015 TIIS - Tanzania Immunization Information System
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
 //******************************************************************************
using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _RoleAction : System.Web.UI.Page
{
    ICollection<Actions> _actions;
    ICollection<Actions> _roleActions;

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

            if ((actionList != null) && actionList.Contains("ViewRoleAction") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language); //Language.GetLanguageByName(language).Id;
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["RoleAction-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "RoleAction");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("RoleAction-dictionary" + language, wtList);
                }

               // controls
                this.lblRole.Text = wtList["RoleActionRole"];
                this.lblAction.Text = wtList["RoleActionAction"];
              
                //actions
                this.btnAdd.Visible = actionList.Contains("AddRoleAction");
                this.btnRemove.Visible = actionList.Contains("RemoveRoleAction");
                this.btnAddAll.Visible = actionList.Contains("AddAllRoleAction");
                this.btnRemoveAll.Visible = actionList.Contains("RemoveAllRoleAction");

                //Page Title
                this.lblTitle.Text = wtList["RoleActionPageTitle"];

                //validators
                BindRoles(Role.GetRoleByStatus(true));

                _actions = Actions.GetLeftActionsOfRole(int.Parse(ddlRole.SelectedValue.ToString()));
                BindLeftActionsOfRole(_actions);

                _roleActions = Actions.GetActionsOfRole(int.Parse(ddlRole.SelectedValue.ToString()));
                BindActionsOfRole(_roleActions);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    protected void BindRoles(ICollection<Role> role)
    {
        ddlRole.DataSource = role;
        ddlRole.DataValueField = "Id";
        ddlRole.DataTextField = "Name";

        ddlRole.DataBind();
    }
    protected void BindLeftActionsOfRole(ICollection<Actions> actions)
    {
        lbActions.DataSource = actions;
        lbActions.DataValueField = "Id";
        lbActions.DataTextField = "Name";

        lbActions.DataBind();
    }
    protected void BindActionsOfRole(ICollection<Actions> actions)
    {
        lbRoleActions.DataSource = actions;
        lbRoleActions.DataValueField = "Id";
        lbRoleActions.DataTextField = "Name";

        lbRoleActions.DataBind();
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string machineName = Request.ServerVariables["REMOTE_HOST"].ToString();
                int i = 0;
                if ((lbActions.Items.Count > 0))
                {
                    RoleAction o = new RoleAction();
                    o.RoleId = int.Parse(ddlRole.SelectedValue);

                    for (int j = 0; j <= lbActions.Items.Count - 1; j++)
                    {
                        if (lbActions.Items[j].Selected)
                        {
                            int actionId = int.Parse(lbActions.Items[j].Value.ToString());
                            if (RoleAction.Exists(o.RoleId, actionId) == 0)   
                            {
                                o.ActionId = actionId;
                                o.IsActive = true;
                                i = RoleAction.Insert(o);
                            }
                        }
                    }

                    _actions = Actions.GetLeftActionsOfRole(o.RoleId);
                    BindLeftActionsOfRole(_actions);

                    _roleActions = Actions.GetActionsOfRole(o.RoleId);
                    BindActionsOfRole(_roleActions);
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
                if ((lbActions.Items.Count > 0))
                {
                    RoleAction o = new RoleAction();
                    o.RoleId = int.Parse(ddlRole.SelectedValue);

                    for (int j = 0; j <= lbActions.Items.Count - 1; j++)
                    {
                        int actionId = int.Parse(lbActions.Items[j].Value.ToString());
                        if (RoleAction.Exists(o.RoleId, actionId) == 0)   
                        {
                            o.ActionId = actionId;
                            o.IsActive = true;
                            i = RoleAction.Insert(o);
                        }
                    }

                    _actions = Actions.GetLeftActionsOfRole(o.RoleId);
                    BindLeftActionsOfRole(_actions);

                    _roleActions = Actions.GetActionsOfRole(o.RoleId);
                    BindActionsOfRole(_roleActions);
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
                if ((lbRoleActions.Items.Count > 0))
                {
                    int roleId = int.Parse(ddlRole.SelectedValue);
                    for (int j = 0; j <= lbRoleActions.Items.Count - 1; j++)
                    {
                        int actionId = int.Parse(lbRoleActions.Items[j].Value.ToString());
                        i = RoleAction.DeleteByActionAndRole(roleId, actionId);
                    }

                    _actions = Actions.GetLeftActionsOfRole(roleId);
                    BindLeftActionsOfRole(_actions);

                    _roleActions = Actions.GetActionsOfRole(roleId);
                    BindActionsOfRole(_roleActions);
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
                if (lbRoleActions.Items.Count > 0)
                {
                    int roleId = int.Parse(ddlRole.SelectedValue);
                    for (int j = 0; j <= lbRoleActions.Items.Count - 1; j++)
                    {
                        if (lbRoleActions.Items[j].Selected)
                        {
                            int actionId = int.Parse(lbRoleActions.Items[j].Value.ToString());
                            i = RoleAction.DeleteByActionAndRole(roleId, actionId);
                        }
                    }

                    _actions = Actions.GetLeftActionsOfRole(roleId);
                    BindLeftActionsOfRole(_actions);

                    _roleActions = Actions.GetActionsOfRole(roleId);
                    BindActionsOfRole(_roleActions);
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

    protected void ddlRole_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (this.ddlRole.SelectedValue != null)
        {
           _actions = Actions.GetLeftActionsOfRole(int.Parse(ddlRole.SelectedValue.ToString()));
            BindLeftActionsOfRole(_actions);

            _roleActions = Actions.GetActionsOfRole(int.Parse(ddlRole.SelectedValue.ToString()));
            BindActionsOfRole(_roleActions);
        }
    }




}
