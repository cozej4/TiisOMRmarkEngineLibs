using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GIIS.BusinessLogic;

public partial class Pages_StockCount : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewStockCount") && (CurrentEnvironment.LoggedUser != null))
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
                //controls
                lblItemCategoryId.Text = wtList["ItemTransactionItemCategory"];
                lblGtin.Text = wtList["ItemTransactionGtin"];
                lblItemId.Text = wtList["ItemTransactionItem"];
                lblItemLot.Text = wtList["ItemTransactionItemLot"];
                lblStockCountDate.Text = wtList["ItemTransactionDate"];
                lblQuantity.Text = wtList["ItemTransactionQuantity"];
                lblNotes.Text = wtList["ItemTransactionNotes"];

                //grid header text
                gvTransactionLines.Columns[1].HeaderText = wtList["ItemTransactionItemLot"];
                gvTransactionLines.Columns[2].HeaderText = wtList["ItemTransactionGtin"];
                gvTransactionLines.Columns[3].HeaderText = wtList["ItemTransactionItem"];
                gvTransactionLines.Columns[4].HeaderText = wtList["ItemTransactionDate"];
                gvTransactionLines.Columns[5].HeaderText = wtList["ItemTransactionQuantity"];
                gvTransactionLines.Columns[6].HeaderText = wtList["ItemTransactionNotes"];

                //actions
                this.btnAdd.Visible = actionList.Contains("AddStockCount");
                this.btnEdit.Visible = actionList.Contains("EditStockCount");

                //buttons
                this.btnAdd.Text = wtList["ItemTransactionAddButton"];
                this.btnEdit.Text = wtList["ItemTransactionEditButton"];
                this.btnExcel.Text = wtList["ItemTransactionExcelButton"];

                //message
                this.lblSuccess.Text = wtList["ItemTransactionSuccessText"];
                //this.lblWarning.Text = wtList["ItemTransactionWarningText"];
                this.lblError.Text = wtList["ItemTransactionErrorText"];

                //Page Title
                this.lblTitle.Text = wtList["StockCountPageTitle"];

                //validators
                cvStock.ErrorMessage = wtList["ItemTransactionMandatory"];
                revQuantity.ErrorMessage = wtList["ItemTransactionQuantityValidator"];

                ConfigurationDate dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value));
                ceStockCountDate.Format = dateformat.DateFormat;
                revStockCountDate.ErrorMessage = dateformat.DateFormat;
                revStockCountDate.ValidationExpression = dateformat.DateExpresion;
                ceStockCountDate.EndDate = DateTime.Today.Date;

                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"].ToString());
                    ItemTransaction o = ItemTransaction.GetItemTransactionById(id);
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
                    odsItemLot.SelectParameters.Add("hfId", o.HealthFacilityCode);
                    odsItemLot.DataBind();
                    ddlItemLot.DataSource = odsItemLot;
                    ddlItemLot.DataBind();
                    ddlItemLot.SelectedValue = o.GtinLot;
                    txtQuantity.Text = o.TransactionQtyInBaseUom.ToString();
                    txtStockCountDate.Text = o.TransactionDate.ToString(dateformat.DateFormat);
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
                    txtStockCountDate.Text = DateTime.Today.Date.ToString(dateformat.DateFormat);
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
                //check for duplicates
               // ItemTransaction st = StockManagementLogic.StockCount(CurrentEnvironment.LoggedUser.HealthFacility.Code, ddlGtin.SelectedValue, ddlItemLot.SelectedValue, txtQuantity.Text, CurrentEnvironment.LoggedUser.Id);
                StockManagementLogic sml = new StockManagementLogic();
                UInt32 qty = UInt32.Parse(txtQuantity.Text);

                DateTime date = DateTime.ParseExact(txtStockCountDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                ItemTransaction st = sml.StockCount(CurrentEnvironment.LoggedUser.HealthFacility, ddlGtin.SelectedValue, ddlItemLot.SelectedValue, qty, CurrentEnvironment.LoggedUser.Id, date );

                int i = st.Id; //  ItemTransaction.Insert(o);
             

                if (i > 0)
                {
                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        odsTransactionLines.SelectParameters.Clear();
                        odsTransactionLines.SelectParameters.Add("i", i.ToString());
                        odsTransactionLines.DataBind();

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

                ItemTransaction o = ItemTransaction.GetItemTransactionById(id);

                DateTime date = DateTime.ParseExact(txtStockCountDate.Text, ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat.ToString(), CultureInfo.CurrentCulture);
                o.TransactionDate = date;
                o.Gtin = ddlGtin.SelectedValue;
                if (ddlItemLot.SelectedIndex != 0)
                    o.GtinLot = ddlItemLot.SelectedValue;
                o.TransactionTypeId = 3; //Stock Count

                double qty = double.Parse(txtQuantity.Text);
                int diff = 0;
                if (o.TransactionQtyInBaseUom != qty)
                    diff = (int)(qty - o.TransactionQtyInBaseUom);

                o.TransactionQtyInBaseUom = double.Parse(txtQuantity.Text);
                o.Notes = txtNotes.Text;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedBy = userId;

                int i = ItemTransaction.Update(o);


                if (i > 0)
                {
                    UpdateBalance(o, diff);

                    lblSuccess.Visible = true;
                    lblWarning.Visible = false;
                    lblError.Visible = false;
                    odsTransactionLines.SelectParameters.Clear();
                    odsTransactionLines.SelectParameters.Add("i", o.Id.ToString());
                    odsTransactionLines.DataBind();
                    ClearControls(this);
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

    private int UpdateBalance(ItemTransaction it, int qty)
    {
        HealthFacilityBalance hb = HealthFacilityBalance.GetHealthFacilityBalance(it.HealthFacilityCode, it.Gtin, it.GtinLot);
        int i = -1;
        if (hb != null)
        {
            hb.StockCount += qty; //??
            hb.Balance += qty;
            WastageTransaction(it, qty);
            i = HealthFacilityBalance.Update(hb);
        }
        return i;
    }
    private int UpdateBalance(ItemTransaction it)
    {
        HealthFacilityBalance hb = HealthFacilityBalance.GetHealthFacilityBalance(it.HealthFacilityCode, it.Gtin, it.GtinLot);
        int i = -1;
        if (hb != null)
        {

            hb.StockCount = it.TransactionQtyInBaseUom;
            int qty = (int)(hb.Balance - it.TransactionQtyInBaseUom);
            if (qty > 0)
                WastageTransaction(it, qty);
            hb.Balance = it.TransactionQtyInBaseUom;
            i = HealthFacilityBalance.Update(hb);

        }
        else
        {
            HealthFacilityBalance hbnew = new HealthFacilityBalance();
            hbnew.HealthFacilityCode = it.HealthFacilityCode;
            hbnew.Gtin = it.Gtin;
            hbnew.LotNumber = it.GtinLot;
            hbnew.StockCount = it.TransactionQtyInBaseUom;
            hbnew.Balance = it.TransactionQtyInBaseUom;
            i = HealthFacilityBalance.Insert(hbnew);

        }
        return i;
    }
    private void WastageTransaction(ItemTransaction o, int diff)
    {
        ItemTransaction it = new ItemTransaction();
        it.TransactionDate = o.TransactionDate;
        it.Gtin = o.Gtin;
        it.GtinLot = o.GtinLot;
        it.HealthFacilityCode = o.HealthFacilityCode;
        it.TransactionTypeId = 4; //Adjustment
        it.RefId = o.Id.ToString();
        it.TransactionQtyInBaseUom = diff;
        it.Notes = o.Notes;
        it.ModifiedOn = DateTime.Now;
        it.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
        it.AdjustmentId = 99; //From stock count - open vial wastage

        int i = ItemTransaction.Insert(it);
        //do we need to update used column?

    }
    protected void ddlGtin_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gtin = ddlGtin.SelectedValue;
        HealthFacility hf = CurrentEnvironment.LoggedUser.HealthFacility;
        if (hf.Type.Code == "1")
        {
            odsItemLot2.SelectParameters.Clear();
            odsItemLot2.SelectParameters.Add("gtin", gtin);
            odsItemLot2.DataBind();
            ddlItemLot.DataSource = odsItemLot2;
            ddlItemLot.DataBind();
        }
        else
        {
            odsItemLot.SelectParameters.Clear();
            odsItemLot.SelectParameters.Add("gtin", gtin);
            odsItemLot.SelectParameters.Add("hfId", hf.Code);
            odsItemLot.DataBind();
            ddlItemLot.DataSource = odsItemLot;
            ddlItemLot.DataBind();
        }
    }

    protected void gvTransactionLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransactionLines.PageIndex = e.NewPageIndex;
    }

    protected void gvTransactionLines_DataBound(object sender, EventArgs e)
    {
        if (gvTransactionLines.Rows.Count > 0)
            btnExcel.Visible = true;
        else
            btnExcel.Visible = false;
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
                ddlItemCategory.SelectedIndex = 0;
                //odsItemLot.SelectParameters.Clear();
                //odsItemLot.SelectParameters.Add("itemId", "-1");
                //odsItemLot.SelectParameters.Add("hfId", CurrentEnvironment.LoggedUser.Id.ToString());
                //ddlItemLot.SelectedIndex = 0;

            }
        }
    }

    protected bool tlExists(int transactionId, int lotid)
    {
        if (TransactionLines.GetTransactionLinesByTransactionIdLotId(transactionId, lotid) != null)
        {
            lblSuccess.Visible = false;
            lblWarning.Visible = true;
            lblError.Visible = false;
            return true;
        }
        return false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=StockCount.xls");
        Response.Charset = "";

        // If you want the option to open the Excel file without saving then
        // comment out the line below
        // Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/ms-excel";
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        gvTransactionLines.RenderControl(htmlWrite);
        Response.Write(stringWrite.ToString());
        Response.End();
    }

    protected override void Render(HtmlTextWriter writer)
    {
        // Ensure that the control is nested in a server form.
        if ((Page != null))
        {
            Page.VerifyRenderingInServerForm(this);
        }

        base.Render(writer);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //     server control at run time. 
    }


}