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


public partial class _Manufacturer : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewManufacturer") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Manufacturer-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Manufacturer");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Manufacturer-dictionary" + language, wtList);
                }

                //controls
                this.lblName.Text = wtList["ManufacturerName"];
                this.lblCode.Text = wtList["ManufacturerCode"];
                this.lblHl7ManufacturerId.Text = wtList["ManufacturerHl7Manufacturer"];
                this.lblIsActive.Text = wtList["ManufacturerIsActive"];
                this.lblNotes.Text = wtList["ManufacturerNotes"];

                //grid header text
                gvManufacturer.Columns[1].HeaderText = wtList["ManufacturerName"];
                gvManufacturer.Columns[2].HeaderText = wtList["ManufacturerCode"];
                //gvManufacturer.Columns[3].HeaderText = wtList["ManufacturerHl7Manufacturer"];
                gvManufacturer.Columns[4].HeaderText = wtList["ManufacturerIsActive"];
                gvManufacturer.Columns[3].HeaderText = wtList["ManufacturerNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddManufacturer");
                this.btnEdit.Visible = actionList.Contains("EditManufacturer");
                this.btnRemove.Visible = actionList.Contains("RemoveManufacturer");

                //buttons
                this.btnAdd.Text = wtList["ManufacturerAddButton"];
                this.btnEdit.Text = wtList["ManufacturerEditButton"];
                this.btnRemove.Text = wtList["ManufacturerRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["ManufacturerSuccessText"];
                this.lblWarning.Text = wtList["ManufacturerWarningText"];
                this.lblError.Text = wtList["ManufacturerErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["ManufacturerPageTitle"];

                //validators
               // rfvCode.ErrorMessage = wtList["ManufacturerMandatory"];
                rfvName.ErrorMessage = wtList["ManufacturerMandatory"];
                revCode.ErrorMessage = wtList["ManufacturerCodeValidator"];
                revName.ErrorMessage = wtList["ManufacturerNameValidator"];

                    //Selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    Manufacturer o = Manufacturer.GetManufacturerById(id);
                    if (o.Hl7ManufacturerId != 0)
                        ddlHl7Manufacturer.SelectedValue = o.Hl7ManufacturerId.ToString();
                    
                    txtName.Text = o.Name;
                    txtCode.Text = o.Code;
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
        odsManufacturer.SelectParameters.Clear();
        odsManufacturer.SelectParameters.Add("i", id.ToString());
        odsManufacturer.DataBind();
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

                if (!String.IsNullOrEmpty(txtCode.Text))
                    if (codeExists(txtCode.Text.Replace("'", @"''")))
                        return;
                Manufacturer o = new Manufacturer();

                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.Hl7ManufacturerId = int.Parse(ddlHl7Manufacturer.SelectedValue);
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = Manufacturer.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    lblWarningRemove.Visible = false;
                    gridview_Databind(i);
                    ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
                    lblWarningRemove.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
            lblWarningRemove.Visible = false;
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

                Manufacturer o = Manufacturer.GetManufacturerById(id);
                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text.Trim()))
                    return;
                if (codeExists(txtCode.Text.Replace("'", @"''")) && (o.Code != txtCode.Text.Trim()))
                    return;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                o.Hl7ManufacturerId = int.Parse(ddlHl7Manufacturer.SelectedValue );
                o.IsActive = bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text.Replace("'", @"''");
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = Manufacturer.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    lblWarningRemove.Visible = false;
                    gridview_Databind(id);
                    ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = false;
                    lblError.Visible = true;
                    lblWarningRemove.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
            lblWarningRemove.Visible = false;
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

            int count = ItemManufacturer.GetCountManufacturer(id);
            if (count > 0)
            {
                lblWarningRemove.Visible = true;
                lblWarning.Visible = false;
                lblSuccess.Visible = false;
                lblError.Visible = false;
            }
            else
            {
                int i = Manufacturer.Remove(id);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    lblWarningRemove.Visible = false;
                    gvManufacturer.DataBind();
                    ClearControls(this);
                }
                else
                {
                    lblSuccess.Visible = false;
                    lblWarning.Visible = true;
                    lblError.Visible = false;
                    lblWarningRemove.Visible = false;
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
    protected void ddlHl7Manufacturer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHl7Manufacturer.SelectedIndex != 0)
        {
            Hl7Manufacturers o = Hl7Manufacturers.GetHl7ManufacturersById(int.Parse(ddlHl7Manufacturer.SelectedValue));
            txtName.Text = o.Name;
            txtCode.Text = o.MvxCode;
        }

    }
    protected bool nameExists(string name)
    {
        if (Manufacturer.GetManufacturerByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected bool codeExists(string code)
    {
        if (Manufacturer.GetManufacturerByCode(code) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
}
