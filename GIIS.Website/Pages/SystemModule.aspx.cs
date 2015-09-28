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


public partial class _SystemModule : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewSystemModule") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["SystemModule-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "SystemModule");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("SystemModule-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["SystemModuleName"];
                this.lblCode.Text = wtList["SystemModuleCode"];
                this.lblIsUsed.Text = wtList["SystemModuleIsUsed"];
                this.lblNotes.Text = wtList["SystemModuleNotes"];

                //grid header text
                gvSystemModule.Columns[1].HeaderText = wtList["SystemModuleName"];
                gvSystemModule.Columns[2].HeaderText = wtList["SystemModuleCode"];
                gvSystemModule.Columns[3].HeaderText = wtList["SystemModuleIsUsed"];
                gvSystemModule.Columns[4].HeaderText = wtList["SystemModuleNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddSystemModule");
                this.btnEdit.Visible = actionList.Contains("EditSystemModule");

                //buttons
                this.btnAdd.Text = wtList["SystemModuleAddButton"];
                this.btnEdit.Text = wtList["SystemModuleEditButton"];

                //message
                this.lblSuccess.Text = wtList["SystemModuleSuccessText"];
                this.lblWarning.Text = wtList["SystemModuleWarningText"];
                this.lblError.Text = wtList["SystemModuleErrorText"];

                //validators

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    SystemModule o = SystemModule.GetSystemModuleById(id);
                    txtName.Text = o.Name;
                    txtCode.Text = o.Code;
                    txtNotes.Text = o.Notes;
                    rblIsUsed.SelectedValue = o.IsUsed.ToString();
                    //rblIsUsed.Items[0].Selected = o.IsUsed;
                    //rblIsUsed.Items[1].Selected = !o.IsUsed;
                    btnAdd.Visible = false;
                }
                else
                    btnEdit.Visible = false;
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

                SystemModule o = new SystemModule();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.IsUsed = bool.Parse(rblIsUsed.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = SystemModule.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvSystemModule.DataBind();
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

                SystemModule o = SystemModule.GetSystemModuleById(id);

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.IsUsed = bool.Parse(rblIsUsed.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = SystemModule.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvSystemModule.DataBind();
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

    protected void gvSystemModule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSystemModule.PageIndex = e.NewPageIndex;
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
