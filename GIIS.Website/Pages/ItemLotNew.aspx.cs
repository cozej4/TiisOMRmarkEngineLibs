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
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ItemLotNew : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItemLot") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ItemLot-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ItemLot");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ItemLot-dictionary" + language, wtList);
                }

                //controls                
                lblItemCategory.Text = wtList["ItemLotItemCategory"];
                lblItem.Text = wtList["ItemLotItem"];
                lblGTIN.Text = wtList["ItemLotGTIN"];
                lblLotNumber.Text = wtList["ItemLotLotNumber"];
                lblExpireDate.Text = wtList["ItemLotExpireDate"];
                lblNotes.Text = wtList["ItemLotNotes"];

                //grid header text
                gvItemLotNew.Columns[0].HeaderText = wtList["ItemLotGTIN"];
                gvItemLotNew.Columns[1].HeaderText = wtList["ItemLotLotNumber"];
                gvItemLotNew.Columns[2].HeaderText = wtList["ItemLotItem"];
                gvItemLotNew.Columns[3].HeaderText = wtList["ItemLotExpireDate"];
                gvItemLotNew.Columns[4].HeaderText = wtList["ItemLotNotes"];

                //validators
                string format = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                string expresion = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateExpresion;
                ceExpireDate.Format = format;
                revExpireDate.ErrorMessage = format;
                revExpireDate.ValidationExpression = expresion;

                //actions
                btnEdit.Visible = actionList.Contains("EditItemLot");
                //btnRemove.Visible = actionList.Contains("RemoveItemLot");

                //buttons
                btnEdit.Text = wtList["ItemLotEditButton"];
                //btnRemove.Text = wtList["ItemLotRemoveButton"];

                ////message
                lblSuccess.Text = wtList["ItemLotSuccessText"];
                lblWarning.Text = wtList["ItemLotWarningText"];
                lblError.Text = wtList["ItemLotErrorText"];
                cvItemLotNew.ErrorMessage = wtList["ItemLotMandatory"];
                cvDate.ErrorMessage = wtList["ItemLotDateValidator"];
                cvGtinLotNumber.ErrorMessage = wtList["ItemLotDuplicateCheck"];

                //Page Title
                lblTitle.Text = wtList["ItemLotPageTitle"];

                //selected object
                string _gtin = Request.QueryString["gtin"];
                string _lotNo = Request.QueryString["lotno"];

                if (!String.IsNullOrEmpty(_gtin) && !String.IsNullOrEmpty(_lotNo))
                {
                    ItemLot o = ItemLot.GetItemLotByGtinAndLotNo(_gtin, _lotNo);

                    if (o != null)
                    {
                        ddlItemCategory.DataBind();
                        ddlItemCategory.SelectedValue = o.ItemObject.ItemCategoryId.ToString();
                        ddlItem.DataBind();
                        ddlItem.SelectedValue = o.ItemId.ToString();
                        ddlGtin.DataBind();
                        ddlGtin.SelectedValue = o.Gtin;
                        txtLotNumber.Text = o.LotNumber;
                        txtExpireDate.Text = o.ExpireDate.ToString(ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat);
                        txtNotes.Text = o.Notes;
                        rblIsActive.SelectedValue = o.IsActive.ToString();

                        gvItemLotNew.Visible = true;
                        odsItemLotNew.SelectParameters.Clear();
                        odsItemLotNew.SelectParameters.Add("gtin", _gtin);
                        odsItemLotNew.SelectParameters.Add("lotNumber", _lotNo);
                        odsItemLotNew.DataBind();
                        gvItemLotNew.DataBind();

                        btnEdit.Visible = true;
                    }
                }
                else
                {
                    gvItemLotNew.Visible = false;
                    //btnRemove.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int i = 0;
                string gtin = Request.QueryString["gtin"];
                string lotno = Request.QueryString["lotno"];

                if (!String.IsNullOrEmpty(gtin) && !String.IsNullOrEmpty(lotno))
                {
                    ItemLot o = ItemLot.GetItemLotByGtinAndLotNo(gtin, lotno);

                    int itemId = -1;
                    if (ddlItem.SelectedIndex != -1)
                    {
                        int.TryParse(ddlItem.SelectedValue, out itemId);
                        o.ItemId = itemId;
                    }
                    if (!String.IsNullOrEmpty(ddlGtin.SelectedValue))
                        o.Gtin = ddlGtin.SelectedValue;
                    if (!String.IsNullOrEmpty(txtLotNumber.Text))
                        o.LotNumber = txtLotNumber.Text;
                    if (!String.IsNullOrEmpty(txtExpireDate.Text))
                        o.ExpireDate = DateTime.ParseExact(txtExpireDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                    if (!String.IsNullOrEmpty(txtNotes.Text))
                        o.Notes = txtNotes.Text;
                    o.IsActive = bool.Parse(rblIsActive.SelectedValue);

                    i = ItemLot.Update(o);
                }
                else
                {
                    ItemLot o = new ItemLot();

                    int itemId = -1;
                    if (ddlItem.SelectedIndex != -1)
                    {
                        int.TryParse(ddlItem.SelectedValue, out itemId);
                        o.ItemId = itemId;
                    }
                    if (!String.IsNullOrEmpty(ddlGtin.SelectedValue))
                    {
                        o.Gtin = ddlGtin.SelectedValue;
                        gtin = o.Gtin;
                    }
                    if (!String.IsNullOrEmpty(txtLotNumber.Text))
                    {
                        o.LotNumber = txtLotNumber.Text;
                        lotno = o.LotNumber;
                    }
                    if (!String.IsNullOrEmpty(txtExpireDate.Text))
                        o.ExpireDate = DateTime.ParseExact(txtExpireDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                    if (!String.IsNullOrEmpty(txtNotes.Text))
                        o.Notes = txtNotes.Text;

                    i = ItemLot.Insert(o);
                }

                if (i > 0)
                {
                    ClearControls(this);
                    BindGrid(gtin, lotno);
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
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
            int userId = CurrentEnvironment.LoggedUser.Id;
            string gtin = Request.QueryString["gtin"];
            string lotno = Request.QueryString["lotno"];

            if (!String.IsNullOrEmpty(gtin) && !String.IsNullOrEmpty(lotno))
            {
                ItemLot o = ItemLot.GetItemLotByGtinAndLotNo(gtin, lotno);

                int i = ItemLot.Remove(o.Id);
                if (i > 0)
                {
                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    ClearControls(this);
                    gvItemLotNew.Visible = false;
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

    private void BindGrid(string gtin, string lotno)
    {
        gvItemLotNew.Visible = true;
        odsItemLotNew.SelectParameters.Clear();
        odsItemLotNew.SelectParameters.Add("gtin", gtin);
        odsItemLotNew.SelectParameters.Add("lotNumber", lotno);
        odsItemLotNew.DataBind();
        gvItemLotNew.DataBind();
    }
    protected void cvGtinLotNumber_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        string _gtin = "";
        string _lotno = "";
        string gtin = Request.QueryString["gtin"];
        string lotno = Request.QueryString["lotno"];

        if (String.IsNullOrEmpty(gtin) && String.IsNullOrEmpty(lotno))
        {
            if (!String.IsNullOrEmpty(ddlGtin.SelectedValue) && !String.IsNullOrEmpty(txtLotNumber.Text))
            {
                _gtin = ddlGtin.SelectedValue;
                _lotno = txtLotNumber.Text;
                if (ItemLot.GetItemLotByGtinAndLotNo(_gtin, _lotno) != null)
                    args.IsValid = false;
                lblSuccess.Visible = false;


            }
        }
    }

    protected void ValidateDate(object sender, ServerValidateEventArgs e)
    {
        ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));

        DateTime date = DateTime.ParseExact(txtExpireDate.Text, dateformat.DateFormat, CultureInfo.InvariantCulture);

        e.IsValid = date > DateTime.Today;
    }

}