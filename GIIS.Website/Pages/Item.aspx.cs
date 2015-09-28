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
using System.Globalization;

public partial class _Item : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItem") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["Item-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "Item");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("Item-dictionary" + language, wtList);
                }

                //controls
                this.lblItemCategoryId.Text = wtList["ItemItemCategory"];
                this.lblHl7VaccineId.Text = wtList["ItemHl7Vaccine"];
                this.lblName.Text = wtList["ItemName"];
                this.lblCode.Text = wtList["ItemCode"];
                this.lblEntryDate.Text = wtList["ItemEntryDate"];
                this.lblExitDate.Text = wtList["ItemExitDate"];
                this.lblIsActive.Text = wtList["ItemIsActive"];
                this.lblNotes.Text = wtList["ItemNotes"];
                this.rblIsActive.Items[0].Text = wtList["ItemYes"];
                this.rblIsActive.Items[1].Text = wtList["ItemNo"];


                //grid header text
                gvItem.Columns[1].HeaderText = wtList["ItemName"];
                gvItem.Columns[2].HeaderText = wtList["ItemCode"];
                gvItem.Columns[3].HeaderText = wtList["ItemItemCategory"];
                gvItem.Columns[4].HeaderText = wtList["ItemHl7Vaccine"];
                gvItem.Columns[5].HeaderText = wtList["ItemEntryDate"];
                gvItem.Columns[6].HeaderText = wtList["ItemExitDate"];
                gvItem.Columns[8].HeaderText = wtList["ItemIsActive"];
                gvItem.Columns[7].HeaderText = wtList["ItemNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddItem");
                this.btnEdit.Visible = actionList.Contains("EditItem");
                // this.btnRemove.Visible = actionList.Contains("RemoveItem");

                //buttons
                this.btnAdd.Text = wtList["ItemAddButton"];
                this.btnEdit.Text = wtList["ItemEditButton"];
                this.btnRemove.Text = wtList["ItemRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["ItemSuccessText"];
                this.lblWarning.Text = wtList["ItemWarningText"];
                this.lblError.Text = wtList["ItemErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["ItemPageTitle"];

                //validators
                ceEntryDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revEntryDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revEntryDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;
                cvItem.ErrorMessage = wtList["ItemMandatory"];
                //cvItemCategory.ErrorMessage = wtList["ItemCategoryValidator"];
                ceExitDate.Format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revExitDate.ErrorMessage = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                revExitDate.ValidationExpression = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;

                //selected object
                int id = -1;
                string _id = Request.QueryString["id"];
                if (!String.IsNullOrEmpty(_id))
                {
                    int.TryParse(_id, out id);
                    GIIS.DataLayer.Item o = GIIS.DataLayer.Item.GetItemById(id);
                    ddlItemCategory.SelectedValue = o.ItemCategoryId.ToString();
                    if (o.ItemCategoryId == 1)
                        ddlHl7Vaccine.Enabled = true;
                    if (o.Hl7VaccineId != 0)
                        ddlHl7Vaccine.SelectedValue = o.Hl7VaccineId.ToString();
                    txtName.Text = o.Name;
                    txtCode.Text = o.Code;
                    txtEntryDate.Text = o.EntryDate.ToString(ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString());
                    txtExitDate.Text = o.ExitDate.ToString(ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString());
                    if (o.ExitDate.ToString("yyyy-MM-dd").Equals("0001-01-01"))
                        txtExitDate.Text = String.Empty;
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
        odsItem.SelectParameters.Clear();
        odsItem.SelectParameters.Add("i", id.ToString());
        odsItem.DataBind();

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
                Item o = new Item();
                o.ItemCategoryId = int.Parse(ddlItemCategory.SelectedValue);
                if (ddlHl7Vaccine.SelectedIndex > 0)
                    o.Hl7VaccineId = int.Parse(ddlHl7Vaccine.SelectedValue);
                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");

                DateTime date = DateTime.ParseExact(txtEntryDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                o.EntryDate = date;
                if (txtExitDate.Text != String.Empty)
                {
                    DateTime exitdate = DateTime.ParseExact(txtExitDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                    o.ExitDate = exitdate;
                }
                o.IsActive = true;// bool.Parse(rblIsActive.SelectedValue);
                o.Notes = txtNotes.Text;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = Item.Insert(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(i);
                    ClearControls(this);

                    if (o.ItemCategoryId == 1)
                    { //add Scheduled Vaccination
                        ScheduledVaccination sv = new ScheduledVaccination();
                        sv.Name = o.Name;
                        sv.Code = o.Code;
                        sv.ItemId = i;
                        sv.EntryDate = o.EntryDate;
                        if (txtExitDate.Text != String.Empty)
                            sv.ExitDate = o.ExitDate;
                        sv.IsActive = o.IsActive;
                        sv.Notes = o.Notes;
                        sv.ModifiedOn = DateTime.Now;
                        sv.ModifiedBy = userId;
                        int j = ScheduledVaccination.Insert(sv);
                        // end 
                    }
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

                Item o = Item.GetItemById(id);
                if (nameExists(txtName.Text.Replace("'", @"''")) && (o.Name != txtName.Text))
                    return;
                o.ItemCategoryId = int.Parse(ddlItemCategory.SelectedValue);
                if (ddlHl7Vaccine.SelectedIndex > 0)
                    o.Hl7VaccineId = int.Parse(ddlHl7Vaccine.SelectedValue);
                else
                    o.Hl7VaccineId = null;
                o.Name = txtName.Text.Replace("'", @"''");
                o.Code = txtCode.Text.Replace("'", @"''");
                DateTime date = DateTime.ParseExact(txtEntryDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                o.EntryDate = date;
                if (txtExitDate.Text != String.Empty)
                {
                    DateTime exitdate = DateTime.ParseExact(txtExitDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                    o.ExitDate = exitdate;
                    if (o.ExitDate < DateTime.Today.Date)
                        o.IsActive = false;
                }
                else
                {
                    o.ExitDate = DateTime.Parse("0001-01-01");
                    o.IsActive = true;
                }

                o.Notes = txtNotes.Text;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = Item.Update(o);

                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    gridview_Databind(id);


                    if (o.ItemCategoryId == 1)
                    { //add Scheduled Vaccination
                        ScheduledVaccination sv = ScheduledVaccination.GetScheduledVaccinationByItemId(o.Id);
                       if (sv != null)
                       {
                        sv.Name = o.Name;
                        sv.Code = o.Code;
                        sv.EntryDate = o.EntryDate;
                        if (txtExitDate.Text != String.Empty)
                            sv.ExitDate = o.ExitDate;
                        sv.IsActive = o.IsActive;
                        sv.Notes = o.Notes;
                        sv.ModifiedOn = DateTime.Now;
                        sv.ModifiedBy = userId;
                        int j = ScheduledVaccination.Update(sv);
                        // end 
                       }
                    }
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

            int i = Item.Remove(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gridview_Databind(id);
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

    protected void gvItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItem.PageIndex = e.NewPageIndex;
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
        if (Item.GetItemByName(name) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected void ddlHl7Vaccine_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHl7Vaccine.SelectedIndex > 0)
        {
            int id = int.Parse(ddlHl7Vaccine.SelectedValue);
            Hl7Vaccines vaccine = Hl7Vaccines.GetHl7VaccinesById(id);
            txtName.Text = vaccine.Fullname;
            txtCode.Text = vaccine.Code;
        }
    }
    protected void gvItem_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
        // ((BoundField)gvItem.Columns[5]).DataFormatString = "{0:" + dateformat + "}";
        // ((BoundField)gvItem.Columns[6]).DataFormatString = "{0:" + dateformat + "}";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string exitDate = e.Row.Cells[6].Text;
            // DateTime date = DateTime.ParseExact(exitDate, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
            DateTime date = DateTime.ParseExact(exitDate, "dd-MMM-yyyy", CultureInfo.CurrentCulture);

            if (date.ToString("yyyy-MM-dd").Equals("0001-01-01"))
                e.Row.Cells[6].Text = String.Empty;
        }
    }

    protected void ddlItemCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItemCategory.SelectedIndex != 0)
        {
            if (int.Parse(ddlItemCategory.SelectedValue) == 1)
                ddlHl7Vaccine.Enabled = true;
        }
    }
    protected void ValidateDates(object sender, ServerValidateEventArgs e)
    {
        ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
        if (txtEntryDate.Text != String.Empty || txtExitDate.Text != String.Empty)
        {
            DateTime datefrom = DateTime.MinValue;
            if (txtEntryDate.Text != String.Empty)
                datefrom = DateTime.ParseExact(txtEntryDate.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);
            DateTime dateto = DateTime.MaxValue;
            if (txtExitDate.Text != String.Empty)
                dateto = DateTime.ParseExact(txtExitDate.Text, dateformat.DateFormat, CultureInfo.CurrentCulture);
            e.IsValid = datefrom <= dateto; //&& datefrom <= DateTime.Today.Date;
        }
    }
}