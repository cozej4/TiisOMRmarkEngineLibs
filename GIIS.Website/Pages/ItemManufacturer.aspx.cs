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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_ItemManufacturer : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewItemManufacturer") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ItemManufacturer-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ItemManufacturer");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ItemManufacturer-dictionary" + language, wtList);
                }

                //controls
                this.lblGTIN.Text = wtList["ItemManufacturerGTIN"];
                this.lblItemCategory.Text = wtList["ItemManufacturerItemCategory"];
                this.lblManufacturer.Text = wtList["ItemManufacturerManufacturer"];
                this.lblItem.Text = wtList["ItemManufacturerItem"];
                this.lblBaseUOM.Text = wtList["ItemManufacturerBaseUOM"];
                this.lblPrice.Text = wtList["ItemManufacturerPrice"];
                this.lblAlt1UOM.Text = wtList["ItemManufacturerAlt1UOM"];
                this.lblAlt1Qty.Text = wtList["ItemManufacturerAlt1Qty"];
                this.lblAlt2UOM.Text = wtList["ItemManufacturerAlt2UOM"];
                this.lblAlt2Qty.Text = wtList["ItemManufacturerAlt2Qty"];
                this.lblGTINParent.Text = wtList["ItemManufacturerGTINParent"];
                //this.lblBaseUOMChild.Text = wtList["ItemManufacturerBaseUOMChild"];
                this.lblStorageSpace.Text = wtList["ItemManufacturerStorageSpace"];
                this.lblNotes.Text = wtList["ItemManufacturerNotes"];
                this.lblIsActive.Text = wtList["ItemManufacturerIsActive"];

                //grid header text
                gvItemManufacturer.Columns[0].HeaderText = wtList["ItemManufacturerGTIN"];
                gvItemManufacturer.Columns[1].HeaderText = wtList["ItemManufacturerItem"];
                gvItemManufacturer.Columns[2].HeaderText = wtList["ItemManufacturerManufacturer"];
                gvItemManufacturer.Columns[3].HeaderText = wtList["ItemManufacturerBaseUOM"];
                gvItemManufacturer.Columns[4].HeaderText = wtList["ItemManufacturerAlt1UOM"];
                gvItemManufacturer.Columns[5].HeaderText = wtList["ItemManufacturerAlt1Qty"];
                gvItemManufacturer.Columns[6].HeaderText = wtList["ItemManufacturerAlt2UOM"];
                gvItemManufacturer.Columns[7].HeaderText = wtList["ItemManufacturerAlt2Qty"];
                gvItemManufacturer.Columns[8].HeaderText = wtList["ItemManufacturerGTINParent"];
                gvItemManufacturer.Columns[9].HeaderText = wtList["ItemManufacturerIsActive"];
                gvItemManufacturer.Columns[10].HeaderText = wtList["ItemManufacturerNotes"];
                gvItemManufacturer.Columns[11].HeaderText = wtList["ItemManufacturerModifiedBy"];

                //actions
                this.btnEdit.Visible = actionList.Contains("EditItemManufacturer");
                this.btnRemove.Visible = actionList.Contains("RemoveItemManufacturer");

                //buttons
                this.btnEdit.Text = wtList["ItemManufacturerEditButton"];
                this.btnRemove.Text = wtList["ItemManufacturerRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["ItemManufacturerSuccessText"];
                this.lblWarning.Text = wtList["ItemManufacturerWarningText"];
                this.lblError.Text = wtList["ItemManufacturerErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["ItemManufacturerTitle"];

                //selected object
                string gtin = Request.QueryString["gtin"];
                if (!String.IsNullOrEmpty(gtin))
                {
                    ItemManufacturer o = ItemManufacturer.GetItemManufacturerByGtin(gtin);

                    txtGTIN.Text = o.Gtin;
                    ddlItemCategory.DataBind();
                    ddlItemCategory.SelectedValue = o.ItemObject.ItemCategoryId.ToString();
                    odsItemCategory.DataBind();
                    odsItem.DataBind();
                    ddlItem.SelectedValue = o.ItemId.ToString();
                    ddlManufacturer.SelectedValue = o.ManufacturerId.ToString();
                    ddlBaseUOM.SelectedValue = o.BaseUom;
                    txtPrice.Text = o.Price.ToString();
                    ddlAlt1UOM.SelectedValue = o.Alt1Uom;
                    txtAlt1Qty.Text = o.Alt1QtyPer.ToString();
                    if(!string.IsNullOrEmpty(o.Alt2Uom))
                    {ddlAlt2UOM.SelectedValue = o.Alt2Uom;
                    txtAlt2Qty.Text = o.Alt2QtyPer.ToString();
                    }
                    rblIsActive.SelectedValue = o.IsActive.ToString();
                    txtStorageSpace.Text = o.StorageSpace.ToString();
                    txtNotes.Text = o.Notes;
                    gridview_Databind(gtin);
                }
                else
                {
                    btnRemove.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    protected void gridview_Databind(string s)
    {
        odsItemManufacturer.SelectParameters.Clear();
        odsItemManufacturer.SelectParameters.Add("s", s);
        odsItemManufacturer.DataBind();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int i = 0;

                string gtin = Request.QueryString["gtin"];
                if (!String.IsNullOrEmpty(gtin))
                {
                    ItemManufacturer o = ItemManufacturer.GetItemManufacturerByGtin(gtin);
                    if (!String.IsNullOrEmpty(txtGTIN.Text))
                        o.Gtin = txtGTIN.Text;
                    o.ItemId = int.Parse(ddlItem.SelectedValue.ToString());
                    o.ManufacturerId = int.Parse(ddlManufacturer.SelectedValue.ToString());
                    o.BaseUom = ddlBaseUOM.SelectedValue.ToString();
                    if (!String.IsNullOrEmpty(txtPrice.Text))
                        o.Price = double.Parse(txtPrice.Text);
                    o.Alt1Uom = ddlAlt1UOM.SelectedValue.ToString();
                    if (!String.IsNullOrEmpty(txtAlt1Qty.Text))
                        o.Alt1QtyPer = int.Parse(txtAlt1Qty.Text);
                    o.Alt2Uom = ddlAlt2UOM.SelectedValue.ToString();
                    if (!String.IsNullOrEmpty(txtAlt2Qty.Text))
                        o.Alt2QtyPer = int.Parse(txtAlt2Qty.Text);
                    if (ddlGTINParent.GetSelectedIndices().Length > 0)
                    {
                        // Set kit items to the list
                        o.KitItems.Clear();
                        foreach(var idx in ddlGTINParent.GetSelectedIndices())
                        {
                            o.KitItems.Add(ItemManufacturer.GetItemManufacturerByGtin(ddlGTINParent.Items[idx].Value));
                        }
                    }
                    //if (!String.IsNullOrEmpty(txtBaseUOMChild.Text))
                    //    o.BaseUomChildPerBaseUomParent = double.Parse(txtBaseUOMChild.Text);
                    double storagespace = -1;
                    if (!String.IsNullOrEmpty(txtStorageSpace.Text))
                    {
                        double.TryParse(txtStorageSpace.Text, out storagespace);
                        o.StorageSpace = storagespace;
                    }
                    if (!String.IsNullOrEmpty(txtNotes.Text))
                        o.Notes = txtNotes.Text;

                    o.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
                    o.ModifiedOn = DateTime.Now;
                    o.IsActive = bool.Parse(rblIsActive.SelectedValue);

                    i = ItemManufacturer.Update(o);
                }
                else
                {
                    ItemManufacturer o = new ItemManufacturer();

                    if (!String.IsNullOrEmpty(txtGTIN.Text))
                        o.Gtin = txtGTIN.Text;
                    o.ItemId = int.Parse(ddlItem.SelectedValue.ToString());
                    o.ManufacturerId = int.Parse(ddlManufacturer.SelectedValue.ToString());
                    o.BaseUom = ddlBaseUOM.SelectedValue.ToString();
                    if (!String.IsNullOrEmpty(txtPrice.Text))
                        o.Price = double.Parse(txtPrice.Text);
                    if (ddlAlt1UOM.SelectedIndex != 0)
                        o.Alt1Uom = ddlAlt1UOM.SelectedValue.ToString();
                    if (!String.IsNullOrEmpty(txtAlt1Qty.Text))
                        o.Alt1QtyPer = int.Parse(txtAlt1Qty.Text);
                    if (ddlAlt2UOM.SelectedIndex != 0)
                        o.Alt2Uom = ddlAlt2UOM.SelectedValue.ToString();
                    if (!String.IsNullOrEmpty(txtAlt2Qty.Text))
                        o.Alt2QtyPer = int.Parse(txtAlt2Qty.Text);
                    if (ddlGTINParent.GetSelectedIndices().Length > 0)
                    {
                        // Set kit items to the list
                        o.KitItems.Clear();
                        foreach (var idx in ddlGTINParent.GetSelectedIndices())
                        {
                            o.KitItems.Add(ItemManufacturer.GetItemManufacturerByGtin(ddlGTINParent.Items[idx].Value));
                        }
                    }
                    //if (!String.IsNullOrEmpty(txtBaseUOMChild.Text))
                    //    o.BaseUomChildPerBaseUomParent = double.Parse(txtBaseUOMChild.Text);
                    double storagespace = -1;
                    if (!String.IsNullOrEmpty(txtStorageSpace.Text))
                    {
                        double.TryParse(txtStorageSpace.Text, out storagespace);
                        o.StorageSpace = storagespace;
                    }
                    if (!String.IsNullOrEmpty(txtNotes.Text))
                        o.Notes = txtNotes.Text;

                    o.IsActive = true;
                    o.ModifiedOn = DateTime.Now;
                    o.ModifiedBy = CurrentEnvironment.LoggedUser.Id;

                    i = ItemManufacturer.Insert(o);
                    gtin = o.Gtin;
                }

                if (i > 0)
                {
                    List<HealthFacility> hfList = HealthFacility.GetHealthFacilityList();
                    foreach(HealthFacility hf in hfList)
                    {
                        GtinHfStockPolicy o = new GtinHfStockPolicy();
                        if (!String.IsNullOrEmpty(txtGTIN.Text))
                            o.Gtin = txtGTIN.Text;
                        o.HealthFacilityCode = hf.Code;
                        
                        o.ReorderQty = 1;
                        o.SafetyStock = 0;
                        o.AvgDailyDemandRate = 0;
                        o.LeadTime = 1;
                        o.ConsumptionLogic = "FEFO";
                        o.ForecastPeriodDemand = 0;

                        GtinHfStockPolicy.Insert(o);
                    }

                    ClearControls(this);
                    gvItemManufacturer.Visible = true;
                    gridview_Databind(gtin);

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
            if (Page.IsValid)
            {
                int i = 0;

                string gtin = Request.QueryString["gtin"];
                if (!String.IsNullOrEmpty(gtin))
                {
                    ItemManufacturer o = ItemManufacturer.GetItemManufacturerByGtin(gtin);
                    o.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
                    o.ModifiedOn = DateTime.Now;

                    i = ItemManufacturer.Remove(gtin);
                    List<ItemManufacturer> imlist = o.KitItems;
                    foreach (ItemManufacturer im in imlist)
                    {
                        ItemManufacturer.Remove(im.Gtin);
                    }
                }

                if (i > 0)
                {
                    ClearControls(this);
                    gvItemManufacturer.Visible = true;
                    odsItemManufacturer.DataBind();
                    gvItemManufacturer.DataBind();

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

    protected void cvGtin_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        string _gtin = "";
        if (!String.IsNullOrEmpty(txtGTIN.Text))
        {
            _gtin = txtGTIN.Text;
            if (ItemManufacturer.GetItemManufacturerByGtin(_gtin) != null)
                args.IsValid = false;
            string gtin = Request.QueryString["gtin"];
            if (!String.IsNullOrEmpty(gtin))
                args.IsValid = true;
        }
    }
    protected bool gtinExists(string gtin)
    {
        if (HealthFacility.GetHealthFacilityByName(gtin) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }
    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItem.SelectedIndex != 0)
        {
            txtNotes.Text = Item.GetItemById(int.Parse(ddlItem.SelectedValue)).Code;
        }
    }
    protected void ddlGTINParent_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlGTINParent.SelectedIndex > 0)
        //    txtBaseUOMChild.Enabled = true;
        //else
        //    txtBaseUOMChild.Enabled = false;
    }

    /// <summary>
    /// Set the selected items
    /// </summary>
    protected void ddlGTINParent_DataBound(object sender, EventArgs e)
    {
        string gtin = Request.QueryString["gtin"];
        if (!String.IsNullOrEmpty(gtin))
        {
            var o = ItemManufacturer.GetItemManufacturerByGtin(gtin);
            foreach (var itm in o.KitItems)
                ddlGTINParent.Items.FindByValue(itm.Gtin).Selected = true;
        }
        // HACK: Kit Items

    }
}