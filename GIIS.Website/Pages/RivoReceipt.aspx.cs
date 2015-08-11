using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.BusinessLogic;

public partial class Pages_Adjustment : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("RivoReceipts") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["ItemTransaction-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "ItemTransaction");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("ItemTransaction-dictionary" + language, wtList);
                }

                lblTitle.Text = wtList["RivoReceiptsTitle"];

                //controls
                lblHealthFacilityId.Text = wtList["ItemTransactionHealthFacility"];
                lblItemCategoryId.Text = wtList["ItemTransactionItemCategory"];
                lblGtin.Text = wtList["ItemTransactionGtin"];
                lblItemId.Text = wtList["ItemTransactionItem"];
                lblItemLot.Text = wtList["ItemTransactionItemLot"];
                lblDate.Text = wtList["ItemTransactionDate"];
                lblQuantity.Text = wtList["ItemTransactionQuantity"];
                lblNotes.Text = wtList["ItemTransactionNotes"];

               //grid header text
                gvTransactionLines.Columns[1].HeaderText = wtList["ItemTransactionGtin"];
                gvTransactionLines.Columns[2].HeaderText = wtList["ItemTransactionItemLot"];
                gvTransactionLines.Columns[3].HeaderText = wtList["ItemTransactionItem"];
                gvTransactionLines.Columns[4].HeaderText = wtList["ItemTransactionDate"];
                gvTransactionLines.Columns[5].HeaderText = wtList["ItemTransactionQuantity"];
                gvTransactionLines.Columns[6].HeaderText = wtList["ItemTransactionAdjustment"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddWastedItems");
                this.btnEdit.Visible = actionList.Contains("EditWastedItems");
                //this.btnRemove.Visible = actionList.Contains("RemoveWastedItems");

                //buttons
                this.btnAdd.Text = wtList["ItemTransactionAddButton"];
                this.btnEdit.Text = wtList["ItemTransactionEditButton"];
                //this.btnRemove.Text = wtList["TransactionLinesRemoveButton"];

                //message
                this.lblSuccess.Text = wtList["ItemTransactionSuccessText"];
                //this.lblWarning.Text = wtList["ItemTransactionWarningText"];
                this.lblError.Text = wtList["ItemTransactionErrorText"];

                //Page Title

                //validators
                cvAdjustment.ErrorMessage = wtList["ItemTransactionMandatory"];
                revQuantity.ErrorMessage = wtList["ItemTransactionQuantityValidator"];

                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceDate.Format = dateformat.DateFormat;
                revDate.ErrorMessage = dateformat.DateFormat;
                revDate.ValidationExpression = dateformat.DateExpresion;

                odsHealthF.SelectParameters.Clear();
                odsHealthF.SelectParameters.Add("i", CurrentEnvironment.LoggedUser.HealthFacilityId.ToString());
                odsHealthF.DataBind();

                ddlHealthFacility.SelectedValue = CurrentEnvironment.LoggedUser.HealthFacility.Code;

                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"].ToString());
                    ItemTransaction o = ItemTransaction.GetItemTransactionById(id);
                    ddlHealthFacility.SelectedValue = o.HealthFacilityCode;
                    ddlItemCategory.SelectedValue = o.GtinObject.ItemObject.ItemCategoryId.ToString();
                    ddlItemCategory.DataBind();
                    odsItem.DataBind();
                    ddlItems.SelectedValue = o.GtinObject.ItemId.ToString();
                    ddlItems.DataBind();
                    odsGtin.DataBind();
                    ddlGtin.SelectedValue = o.Gtin;
                    ddlGtin.DataBind();
                    odsItemLot.SelectParameters.Clear();
                    odsItemLot.SelectParameters.Add("gtin", o.Gtin);
                    //odsItemLot.SelectParameters.Add("hfId", o.HealthFacilityCode);
                    odsItemLot.DataBind();
                    ddlItemLot.DataSource = odsItemLot;
                    ddlItemLot.DataBind();
                    ddlItemLot.SelectedValue = o.GtinLot;
                    //use uom ddl
                    txtQuantity.Text = o.TransactionQtyInBaseUom.ToString();
                    txtDate.Text = o.TransactionDate.ToString(dateformat.DateFormat);
                    txtNotes.Text = o.Notes;
                    btnEdit.Visible = true;
                    btnAdd.Visible = false;

                    odsTransactionLines.SelectParameters.Clear();
                    odsTransactionLines.SelectParameters.Add("i", id.ToString());
                    odsTransactionLines.DataBind();
                }
                else
                {
                    btnEdit.Visible = false;
                    btnAdd.Visible = true;
                    txtDate.Text = DateTime.Today.Date.ToString(dateformat.DateFormat);
                }
              
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
    
    protected void ddlGtin_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gtin = ddlGtin.SelectedValue;
        string hfcode = ddlHealthFacility.SelectedValue;
       // HealthFacility hf = CurrentEnvironment.LoggedUser.HealthFacility;
        
            odsItemLot.SelectParameters.Clear();
            odsItemLot.SelectParameters.Add("gtin", gtin);
            //odsItemLot.SelectParameters.Add("hfId", hfcode);
            odsItemLot.DataBind();
            ddlItemLot.DataSource = odsItemLot;
            ddlItemLot.DataBind();

            //ItemManufacturer gtinObject = ItemManufacturer.GetItemManufacturerByGtin(gtin);
            //ddlUom.SelectedValue = gtinObject.BaseUom;
        //uom ddl should be loaded with the base, alt1, alt2 uoms from item manufacturer

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int userId = CurrentEnvironment.LoggedUser.Id;

                StockManagementLogic sml = new StockManagementLogic();
                DateTime date = DateTime.ParseExact(txtDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                if (!String.IsNullOrEmpty(txtQuantity.Text))
                {
                    int qty = int.Parse(txtQuantity.Text);
                    AdjustmentReason adr = AdjustmentReason.GetAdjustmentReasonById(int.Parse(System.Configuration.ConfigurationManager.AppSettings["ReceiptAdjustmentReason"]));
                    HealthFacility hf = HealthFacility.GetHealthFacilityByCode(ddlHealthFacility.SelectedValue);

                    string uom = ddlUom.SelectedValue;
                    int quantity = 0;
                    ItemManufacturer im = ItemManufacturer.GetItemManufacturerByGtin(ddlGtin.SelectedValue);
                    if (uom.Equals(im.BaseUom))
                        quantity = qty;
                    else if (uom.Equals(im.Alt1Uom))
                        quantity = qty * im.Alt1QtyPer;
                    else if (uom.Equals(im.Alt2Uom))
                        quantity = qty * im.Alt2QtyPer;

                    //have base uom logic

                    string lot = string.Empty;
                    if (ddlItemLot.SelectedIndex != 0)
                        lot = ddlItemLot.SelectedValue;
                    ItemTransaction st = sml.Adjust(hf, ddlGtin.SelectedValue, lot, quantity, adr, CurrentEnvironment.LoggedUser.Id, date);
                    int i = st.Id;

                    if (i > 0)
                    {
                        ClearControls(this);

                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        
                        odsTransactionLines.SelectParameters.Clear();
                        odsTransactionLines.SelectParameters.Add("i", i.ToString());
                        odsTransactionLines.DataBind();
                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = false;
                        lblError.Visible = true;
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

                ItemTransaction o = ItemTransaction.GetItemTransactionById(id);
                DateTime date = DateTime.ParseExact(txtDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                if (!String.IsNullOrEmpty(txtQuantity.Text))
                {
                    o.Gtin = ddlGtin.SelectedValue;
                    string lot = string.Empty;
                    if (ddlItemLot.SelectedIndex != 0)
                        lot = ddlItemLot.SelectedValue;
                    o.GtinLot = lot;
                    o.TransactionDate = date;
                    double qty = double.Parse(txtQuantity.Text);
                    int diff = 0;
                    if (o.TransactionQtyInBaseUom != qty)
                        diff = (int)(qty - o.TransactionQtyInBaseUom);
                    o.TransactionQtyInBaseUom = qty;
                    o.AdjustmentId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ReceiptAdjustmentReason"]);
                    o.Notes = txtNotes.Text;
                    o.ModifiedOn = DateTime.Now;
                    o.ModifiedBy = userId;

                    int i = ItemTransaction.Update(o);
                    if (i > 0)
                    {
                        UpdateBalance(o, diff);
                        
                        ClearControls(this);

                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        
                        odsTransactionLines.SelectParameters.Clear();
                        odsTransactionLines.SelectParameters.Add("i", o.Id.ToString());
                        odsTransactionLines.DataBind();
                        
                        btnEdit.Visible = false;
                        btnAdd.Visible = true;
                    }
                    else
                    {
                        lblSuccess.Visible = false;
                        lblWarning.Visible = false;
                        lblError.Visible = true;
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
    
    private int UpdateBalance(ItemTransaction it, int qty)
    {
        HealthFacilityBalance hb = HealthFacilityBalance.GetHealthFacilityBalance(it.HealthFacilityCode, it.Gtin, it.GtinLot);
        int i = -1;
        if (hb != null)
        {
            if (it.AdjustmentObject.Positive)
                hb.Balance += qty;
            else
            {
                hb.Balance -= qty;
                hb.Wasted += qty;
            }
            i = HealthFacilityBalance.Update(hb);
        }
        return i;
    }
    
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            int id = -1;
            string _id = Request.QueryString["id"].ToString();
            int.TryParse(_id, out id);
            int userId = CurrentEnvironment.LoggedUser.Id;

            int i = TransactionLines.Delete(id);

            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblWarning.Visible = false;
                lblError.Visible = false;
                gvTransactionLines.DataBind();
                ClearControls(this);
            }
            else
            {
                lblSuccess.Visible = false;
                lblWarning.Visible = true;
                lblError.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = false;
            lblError.Visible = true;
        }
    }

    protected void gvTransactionLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransactionLines.PageIndex = e.NewPageIndex;
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

                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));

                ddlItemCategory.SelectedIndex = 0;
                //ddlAdjustmentReason.SelectedIndex = 0;
                //ddlUom.SelectedIndex = 0;
                ddlGtin.SelectedIndex = 0;
                ddlItemLot.SelectedIndex = 0;
                txtDate.Text = DateTime.Today.Date.ToString(dateformat.DateFormat);

                //if (c is DropDownList)
                //    (c as DropDownList).SelectedIndex = 0;

            }
        }
    }
}
