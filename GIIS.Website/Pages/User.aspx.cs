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


public partial class _User : System.Web.UI.Page
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
                int languageId = int.Parse(language); //Language.GetLanguageByName(language).Id;
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
                this.lblPassword.Text = wtList["UserPassword"];
                this.lblFirstname.Text = wtList["UserFirstname"];
                this.lblLastname.Text = wtList["UserLastname"];
                this.lblNotes.Text = wtList["UserNotes"];
                this.lblEmail.Text = wtList["UserEmail"];
                this.lblHealthFacilityId.Text = wtList["UserHealthFacility"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddUser");

                //validators
                cvUser.ErrorMessage = wtList["UserMandatory"];
                revHealthFacility.Text = wtList["UserHealthFacilityValidator"];
                revPassword.ErrorMessage = wtList["UserPasswordValidator"];
                //cvRole.ErrorMessage = wtList["UserRoleValidator"];
                revFirstname.ErrorMessage = wtList["UserNameValidator"];
                revLastname.ErrorMessage = wtList["UserLastNameValidator"];
                revEmail.ErrorMessage = wtList["UserEmailValidator"];

                //Page Title
                this.lblTitle.Text = wtList["UserPageTitle"];

                //message
                this.lblSuccess.Text = wtList["UserSuccessText"];
                this.lblWarning.Text = wtList["UserWarningText"];
                this.lblError.Text = wtList["UserErrorText"];
                if ((String)HttpContext.Current.Session["_successUser"] == "1")
                    lblSuccess.Visible = true;
                else
                    lblSuccess.Visible = false;

                HttpContext.Current.Session["_successUser"] = "0";

                //gridview databind
                if (HttpContext.Current.Session["_lastUser"] != null)
                {
                    int i = (int)HttpContext.Current.Session["_lastUser"];
                    odsUser.SelectParameters.Clear();
                    odsUser.SelectParameters.Add("i", i.ToString());
                    odsUser.DataBind();
                   
                    HttpContext.Current.Session["_lastUser"] = null;
                    txtUsername.Text = String.Empty;
                    txtPassword.Text = String.Empty;
                }
                if (Request.QueryString["hfId"] != null)
                {
                    int hfId = int.Parse(Request.QueryString["hfId"].ToString());
                    HealthFacility o = HealthFacility.GetHealthFacilityById(hfId);
                    //txtHealthFacilityId.SelectedItemID = hfId;
                    
                    txtHealthFacilityId.SelectedItemText =o.Name;
                    healthCenterId = hfId.ToString();
                    txtHealthFacilityId.Enabled = false;
                }
            }
            else
            {
                Response.Redirect("Default.aspx", false);
            }
        }
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
    protected void HealthCenters_ValueSelected(object sender, System.EventArgs e)
    {
        healthCenterId = txtHealthFacilityId.SelectedItemID.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                revHealthFacility.Visible = false;
                if (usernameExists(txtUsername.Text.Trim().Replace("'", @"''")))
                    return;
                User o = new User();
                if (healthCenterId == null)
                {
                    revHealthFacility.Visible = true;
                    return;
                }
                if (HealthFacility.GetHealthFacilityById(int.Parse(healthCenterId)) == null)
                {
                    revHealthFacility.Visible = true;
                    return;
                }
                o.Username = txtUsername.Text.Replace("'", @"''");
                o.Password = Helper.ComputeHash(txtPassword.Text);
                o.Firstname = txtFirstname.Text.Replace("'", @"''");
                o.Lastname = txtLastname.Text.Replace("'", @"''");
                o.HealthFacilityId = int.Parse(healthCenterId);
                o.IsActive = true;
                o.Deleted = false;
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.Email = txtEmail.Text.Replace("'", @"''");
                o.Failedlogins = 0;
                o.Isloggedin = false;
                o.Lastip = "";

                int i = GIIS.DataLayer.User.Insert(o);

                if (i > 0)
                {
                    int roleId = int.Parse(ddlRole.SelectedValue.ToString());
                    UserRole u = new UserRole();

                    u.UserId = i;
                    u.RoleId = roleId;
                    u.IsActive = true;
                    int j = UserRole.Insert(u);
                    if (j > 0)
                    {
                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        ClearControls(this);
                        revHealthFacility.Visible = false;
                        HttpContext.Current.Session["_successUser"] = "1";
                        HttpContext.Current.Session["_lastUser"] = i;
                        Response.Redirect(Request.RawUrl, false);
                    }
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


    protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUser.PageIndex = e.NewPageIndex;
    }

    protected void gvUser_Databound(object sender, System.EventArgs e)
    {
        if (gvUser.Rows.Count > 0)
        {
            string language = CurrentEnvironment.Language;
            //put here name assignment for gridview column headers
            Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["User-dictionary" + language];
            if (wtList != null)
            {
                //grid header text
                gvUser.Columns[1].HeaderText = wtList["UserUsername"];
              //  gvUser.Columns[2].HeaderText = wtList["UserPassword"];
                gvUser.Columns[2].HeaderText = wtList["UserFirstname"];
                gvUser.Columns[3].HeaderText = wtList["UserLastname"];
                gvUser.Columns[7].HeaderText = wtList["UserIsActive"];
                gvUser.Columns[6].HeaderText = wtList["UserNotes"];
                gvUser.Columns[5].HeaderText = wtList["UserEmail"];
                gvUser.Columns[4].HeaderText = wtList["UserHealthFacility"];
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
    protected void ddlRole_DataBound(object sender, EventArgs e)
    {
        int userid = CurrentEnvironment.LoggedUser.Id;
        int roleid = UserRole.GetUserRoleByUserId(userid).RoleId;
        if (roleid != 1)
        {
            foreach (ListItem item in ddlRole.Items)
            {
                if (item.Value == "1")
                    item.Enabled = false;
            }
        }
    }
  
}
