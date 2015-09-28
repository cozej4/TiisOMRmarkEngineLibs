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


public partial class _Community : System.Web.UI.Page
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
            
            if ((actionList != null) && actionList.Contains("ViewCommunity") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Community-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Community");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Community-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["CommunityName"];
                this.lblIsActive.Text = wtList["CommunityIsActive"];
                this.lblNotes.Text = wtList["CommunityNotes"];

                //grid header text
                gvCommunity.Columns[1].HeaderText = wtList["CommunityName"];
                gvCommunity.Columns[3].HeaderText = wtList["CommunityIsActive"];
                gvCommunity.Columns[2].HeaderText = wtList["CommunityNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddCommunity");
                this.btnEdit.Visible = actionList.Contains("EditCommunity");
                this.btnRemove.Visible = actionList.Contains("RemoveCommunity");

                //buttons
                this.btnAdd.Text = wtList["CommunityAddButton"];
                this.btnEdit.Text = wtList["CommunityEditButton"];
                this.btnRemove.Text = wtList["CommunityRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["CommunitySuccessText"];
                this.lblWarning.Text = wtList["CommunityWarningText"];
                this.lblError.Text = wtList["CommunityErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["CommunityPageTitle"];

                //validators
                rfvName.ErrorMessage = wtList["CommunityMandatory"];
                revName.ErrorMessage = wtList["CommunityNotValid"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    GIIS.DataLayer.Community o = GIIS.DataLayer.Community.GetCommunityById(id);
                    txtName.Text = o.Name;
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
       // string where = @" ""ID"" = " + id;
        odsCommunity.SelectParameters.Clear();
        odsCommunity.SelectParameters.Add("i", id.ToString());
        odsCommunity.DataBind();
        gvCommunity.DataSource = odsCommunity;
        gvCommunity.DataBind();
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
                Community o = new Community();

                o.Name = txtName.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = Community.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    //gridview databind
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

                Community o = Community.GetCommunityById(id);
                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;
                o.Name = txtName.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = Community.Update(o);
             
                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    //gridview databind
                    gridview_Databind(i);
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
            //Community o = Community.GetCommunityById(id);
            int i = Community.Remove(id);
            
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
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvCommunity_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCommunity.PageIndex = e.NewPageIndex;
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
    protected bool nameExists(string name)
    {
        if (Community.GetCommunityByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
}
