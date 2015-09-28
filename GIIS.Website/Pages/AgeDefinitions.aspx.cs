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


public partial class _AgeDefinitions : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewAgeDefinitions") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["AgeDefinitions-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "AgeDefinitions");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("AgeDefinitions-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["AgeDefinitionsName"];
                this.lblIsActive.Text = wtList["AgeDefinitionsIsActive"];
                this.lblNotes.Text = wtList["AgeDefinitionsNotes"];
                this.lblDays.Text = wtList["AgeDefinitionsDays"];

                //grid header text
                gvAgeDefinitions.Columns[1].HeaderText = wtList["AgeDefinitionsName"];
                gvAgeDefinitions.Columns[2].HeaderText = wtList["AgeDefinitionsDays"];
                gvAgeDefinitions.Columns[4].HeaderText = wtList["AgeDefinitionsIsActive"];
                gvAgeDefinitions.Columns[3].HeaderText = wtList["AgeDefinitionsNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddAgeDefinitions");
                this.btnEdit.Visible = actionList.Contains("EditAgeDefinitions");
                this.btnRemove.Visible = actionList.Contains("RemoveAgeDefinitions");

                //buttons
                this.btnAdd.Text = wtList["AgeDefinitionsAddButton"];
                this.btnEdit.Text = wtList["AgeDefinitionsEditButton"];
                this.btnRemove.Text = wtList["AgeDefinitionsRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["AgeDefinitionsSuccessText"];
                this.lblWarning.Text = wtList["AgeDefinitionsWarningText"];
                this.lblError.Text = wtList["AgeDefinitionsErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["AgeDefinitionsPageTitle"];

                //validators
               // rfvName.ErrorMessage = wtList["AgeDefinitionsMandatory"];
               // rfvDays.ErrorMessage = wtList["AgeDefinitionsMandatory"];
                revDays.ErrorMessage = wtList["AgeDefinitionsNotValid"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    AgeDefinitions o = AgeDefinitions.GetAgeDefinitionsById(id);
                    txtName.Text = o.Name;
                    txtDays.Text = o.Days.ToString();
                    txtNotes.Text = o.Notes;
                    rblIsActive.Items[0].Selected = o.IsActive;
                    rblIsActive.Items[1].Selected = !o.IsActive;
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


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                if (nameExists(txtName.Text.Replace("'", @"''")))
                    return;
                AgeDefinitions o = new AgeDefinitions();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Days = int.Parse(txtDays.Text);
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = AgeDefinitions.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvAgeDefinitions.DataBind();
                    ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false ;
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

                AgeDefinitions o = AgeDefinitions.GetAgeDefinitionsById(id);
                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Days = int.Parse(txtDays.Text);
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");

                int i = AgeDefinitions.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvAgeDefinitions.DataBind();
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


    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int id = -1;
            string _id = Request.QueryString["id"].ToString();
            int.TryParse(_id, out id);
            int userId = CurrentEnvironment.LoggedUser.Id;

            int i = AgeDefinitions.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvAgeDefinitions.DataBind();
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

    protected void gvAgeDefinitions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAgeDefinitions.PageIndex = e.NewPageIndex;
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
        if (AgeDefinitions.GetAgeDefinitionsByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
}
