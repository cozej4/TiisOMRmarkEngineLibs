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


public partial class _Birthplace : System.Web.UI.Page
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

            if ((actionList != null) && (CurrentEnvironment.LoggedUser != null) && actionList.Contains("ViewBirthplace"))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Birthplace-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Birthplace");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Birthplace-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["BirthplaceName"];
                this.lblNotes.Text = wtList["BirthplaceNotes"];
                this.lblIsActive.Text = wtList["BirthplaceIsActive"];
                this.rblIsActive.Items[0].Text = wtList["BirthplaceYes"];
                this.rblIsActive.Items[1].Text = wtList["BirthplaceNo"];

                //grid header text
                gvBirthplace.Columns[1].HeaderText = wtList["BirthplaceName"];
                gvBirthplace.Columns[2].HeaderText = wtList["BirthplaceNotes"];
                gvBirthplace.Columns[3].HeaderText = wtList["BirthplaceIsActive"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddBirthplace");
                this.btnEdit.Visible = actionList.Contains("EditBirthplace");
                this.btnRemove.Visible = actionList.Contains("RemoveBirthplace");

                //buttons
                this.btnAdd.Text = wtList["BirthplaceAddButton"];
                this.btnEdit.Text = wtList["BirthplaceEditButton"];
                this.btnRemove.Text = wtList["BirthplaceRemoveButton"];

                //Page Title
                this.lblTitle.Text = wtList["BirthplacePageTitle"];

                //message
                this.lblSuccess.Text = wtList["BirthplaceSuccessText"];
                this.lblWarning.Text = wtList["BirthplaceWarningText"];
                this.lblError.Text = wtList["BirthplaceErrorText"];

                //validators
                cvBirthplace.ErrorMessage = wtList["BirthplaceMandatory"];
                revName.Text = wtList["BirthplaceNameValidator"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    Birthplace o = Birthplace.GetBirthplaceById(id);
                    txtName.Text = o.Name;
                    txtNotes.Text = o.Notes;
                    rblIsActive.SelectedValue = o.IsActive.ToString();
                   // gridview_Databind(id);
                    btnAdd.Visible = false;
                }
                else
                {
                    btnEdit.Visible = false;
                    btnRemove.Visible = false;
                    lblIsActive.Visible = false;
                    rblIsActive.Visible = false;
                }

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void gridview_Databind()
    {
        odsBirthplace.SelectParameters.Clear();
       // odsBirthplace.SelectParameters.Add("i", id.ToString());
        odsBirthplace.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                Birthplace o = new Birthplace();

                if (nameExists(txtName.Text.Replace("'", @"''")))
                    return;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = Birthplace.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind();
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

                Birthplace o = Birthplace.GetBirthplaceById(id);

                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;

                o.Name = txtName.Text.Trim();
                o.Notes = txtNotes.Text.Trim();
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                if (id == 1)
                    o.IsActive = true;
                int i = Birthplace.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    ClearControls(this);
                    gridview_Databind();
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
            int i = 0;
            if (id != 1)
                i = Birthplace.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gridview_Databind();
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

    protected void gvBirthplace_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBirthplace.PageIndex = e.NewPageIndex;
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

                //if (c is DropDownList)
                //    (c as DropDownList).SelectedIndex = 0;
            }
        }
    }
    protected bool nameExists(string name)
    {
        if (Birthplace.GetBirthplaceByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
}