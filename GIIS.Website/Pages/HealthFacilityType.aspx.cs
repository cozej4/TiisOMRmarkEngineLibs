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
using System;
using GIIS.DataLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_HealthFacilityType : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewHealthFacilityType") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["HealthFacilityType-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "HealthFacilityType");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("HealthFacilityType-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["HealthFacilityTypeName"];
                this.lblCode.Text = wtList["HealthFacilityTypeCode"];
                this.lblIsActive.Text = wtList["HealthFacilityTypeIsActive"];
                this.lblNotes.Text = wtList["HealthFacilityTypeNotes"];

                //grid header text
                gvHealthFacilityType.Columns[1].HeaderText = wtList["HealthFacilityTypeName"];
                gvHealthFacilityType.Columns[2].HeaderText = wtList["HealthFacilityTypeCode"];
                gvHealthFacilityType.Columns[3].HeaderText = wtList["HealthFacilityTypeNotes"];
                gvHealthFacilityType.Columns[4].HeaderText = wtList["HealthFacilityTypeIsActive"];

                // actions
                this.btnAdd.Visible = actionList.Contains("AddHealthFacilityType");
                this.btnEdit.Visible = actionList.Contains("EditHealthFacilityType");
                this.btnRemove.Visible = actionList.Contains("RemoveHealthFacilityType");

                //buttons
                this.btnAdd.Text = wtList["HealthFacilityTypeAddButton"];
                this.btnEdit.Text = wtList["HealthFacilityTypeEditButton"];
                this.btnRemove.Text = wtList["HealthFacilityTypeRemoveButton"];

                // message
                this.lblSuccess.Text = wtList["HealthFacilityTypeSuccessText"];
                this.lblWarning.Text = wtList["HealthFacilityTypeWarningText"];
                this.lblError.Text = wtList["HealthFacilityTypeErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["HealthFacilityTypePageTitle"];

                //Selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    HealthFacilityType o = HealthFacilityType.GetHealthFacilityTypeById(id);
                    txtName.Text = o.Name;
                    txtCode.Text = o.Code;
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
                Response.Redirect("Default.aspx", false);
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

                HealthFacilityType o = new HealthFacilityType();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = HealthFacilityType.Insert(o);
                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvHealthFacilityType.DataBind();
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

                HealthFacilityType o = HealthFacilityType.GetHealthFacilityTypeById(id);

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (id == 1)
                    o.IsActive = true;
                int i = HealthFacilityType.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvHealthFacilityType.DataBind();
                    //ClearControls(this);
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
            int i = 0;
            if (id != 1)
                i = HealthFacilityType.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvHealthFacilityType.DataBind();
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

    protected void gvHealthFacilityType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvHealthFacilityType.PageIndex = e.NewPageIndex;
    }

}