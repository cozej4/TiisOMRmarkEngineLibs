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


public partial class _ItemCategory : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItemCategory") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ItemCategory-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ItemCategory");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ItemCategory-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["ItemCategoryName"];
                this.lblCode.Text = wtList["ItemCategoryCode"];
                this.lblIsActive.Text = wtList["ItemCategoryIsActive"];
                this.lblNotes.Text = wtList["ItemCategoryNotes"];

                //grid header text
                gvItemCategory.Columns[1].HeaderText = wtList["ItemCategoryName"];
                gvItemCategory.Columns[2].HeaderText = wtList["ItemCategoryCode"];
                gvItemCategory.Columns[4].HeaderText = wtList["ItemCategoryIsActive"];
                gvItemCategory.Columns[3].HeaderText = wtList["ItemCategoryNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddItemCategory");
                this.btnEdit.Visible = actionList.Contains("EditItemCategory");
                this.btnRemove.Visible = actionList.Contains("RemoveItemCategory");

                //buttons
                this.btnAdd.Text = wtList["ItemCategoryAddButton"];
                this.btnEdit.Text = wtList["ItemCategoryEditButton"];
                this.btnRemove.Text = wtList["ItemCategoryRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["ItemCategorySuccessText"];
                this.lblWarning.Text = wtList["ItemCategoryWarningText"];
                this.lblError.Text = wtList["ItemCategoryErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["ItemCategoryPageTitle"];

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    ItemCategory o = ItemCategory.GetItemCategoryById(id);
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

                ItemCategory o = new ItemCategory();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = ItemCategory.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvItemCategory.DataBind();
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

                ItemCategory o = ItemCategory.GetItemCategoryById(id);

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;
                if (id == 1 || id == 2 || id == 3)
                    o.IsActive = true;
                int i = ItemCategory.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gvItemCategory.DataBind();
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
            if (id != 1 && id !=2 && id!=3)
             i = ItemCategory.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvItemCategory.DataBind();
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

    protected void gvItemCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemCategory.PageIndex = e.NewPageIndex;
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
