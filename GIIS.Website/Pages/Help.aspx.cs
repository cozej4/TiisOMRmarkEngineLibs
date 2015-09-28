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
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Help : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHelp") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Help-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Help");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Help-dictionary" + language, wtList);
                }

                lblLanguageId.Text = wtList["HelpLanguage"];
                lblPageName.Text = wtList["HelpPage"];
                //grid header text
                gvHelp.Columns[1].HeaderText = wtList["HelpPage"];
                gvHelp.Columns[3].HeaderText = wtList["HelpHelpText"];
                gvHelp.Columns[2].HeaderText = wtList["HelpLanguage"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddHelp");
                //this.btnEdit.Visible = actionList.Contains("EditHelp");

                //buttons
                this.btnAdd.Text = wtList["HelpAddButton"];
                //this.btnEdit.Text = wtList["HelpEditButton"];

                //message
                this.lblSuccess.Text = wtList["HelpSuccessText"];
                this.lblWarning.Text = wtList["HelpWarningText"];
                this.lblError.Text = wtList["HelpErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["HelpPageTitle"];

                if (Session["__languageid"] != null)
                    ddlLanguage.SelectedValue = Session["__languageid"].ToString();

                odsLanguages.DataBind();
                ddlLanguage.DataSourceID = "odsLanguages";
                ddlLanguage.DataBind();

                ddlLanguage.SelectedValue = languageId.ToString();
            
                //gvHelp.DataSource = Help.GetHelpByLanguage(languageId);
                //gvHelp.DataBind();

                if (Page.Request.QueryString["menuid"] != null)
                {
                    string menuid = Page.Request.QueryString["menuid"].ToString();
                    Help help = Help.GetHelpByPageAndLanguage(menuid, int.Parse(ddlLanguage.SelectedValue));
                    if (Page.Request.QueryString["text"] != null)
                        lblPage.Text = Page.Request.QueryString["text"].ToString();
                    if (help != null)
                        txtHelpText.Text = help.HelpText;
                }
                else
                    btnAdd.Visible = false;
                
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
                if (Page.Request.QueryString["menuid"] != null)
                {
                    int userId = CurrentEnvironment.LoggedUser.Id;
                    string page = Page.Request.QueryString["menuid"].ToString();
                    Help o = Help.GetHelpByPageAndLanguage(page, int.Parse(ddlLanguage.SelectedValue));
                    int i = 0;
                    if (o != null)
                    {
                        o.HelpText = txtHelpText.Text.Replace("'", @"''");
                        o.ModifiedOn = DateTime.Now;
                        o.ModifiedBy = userId;

                        i = Help.Update(o);
                    }
                    else
                    {
                         o = new Help();
                        o.Page = page;
                        o.HelpText = txtHelpText.Text.Replace("'", @"''");
                        o.LanguageId = int.Parse(ddlLanguage.SelectedValue);
                        o.ModifiedOn = DateTime.Now;
                        o.ModifiedBy = userId;

                        i = Help.Insert(o);
                    }
                    if (i > 0)
                    {
                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        
                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = true;
                        lblError.Visible = false;
                    }
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

   

    protected void gvHelp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gvHelp.PageIndex = e.NewPageIndex;
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

    protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["__languageid"] = ddlLanguage.SelectedValue;
        if (Page.Request.QueryString["menuid"] != null)
        {
            string page = Page.Request.QueryString["menuid"].ToString();
            Help o = Help.GetHelpByPageAndLanguage(page, int.Parse(ddlLanguage.SelectedValue));
            if (o != null)
                txtHelpText.Text = o.HelpText;
            else
                txtHelpText.Text = string.Empty;
        }
       
    }
}
