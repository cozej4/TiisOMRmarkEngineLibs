using GIIS.DataLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using GIIS.BusinessLogic;
using System.Data;
using System.Configuration;

public partial class Pages_TransferOrderDetail : System.Web.UI.Page
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

            if ((actionList != null) && actionList.Contains("ViewTransferOrderDetails") && (CurrentEnvironment.LoggedUser != null))
            {
                int userId = CurrentEnvironment.LoggedUser.Id;
                string language = CurrentEnvironment.Language;
                int languageId = int.Parse(language);
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["TransferOrderDetail-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "TransferOrderDetail");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("TransferOrderDetail-dictionary" + language, wtList);
                }

                this.lblOrderNum.Text = wtList["TransferOrderDetailOrderNum"];
                //this.lblOrderDetailNum.Text = wtList["TransferOrderDetailOrderDetailNum"];
                //this.lblOrderGtin.Text = wtList["TransferOrderDetailOrderGtin"];
                //this.lblOrderGtinLotnum.Text = wtList["TransferOrderDetailOrderGtinLotnum"];
                //this.lblOrderCustomItem.Text = wtList["TransferOrderDetailOrderCustomItem"];
                //this.lblOrderDetailDescription.Text = wtList["TransferOrderDetailOrderDetailDescription"];
                //this.lblOrderUom.Text = wtList["TransferOrderDetailOrderUom"];
                //this.lblOrderQtyInBaseUom.Text = wtList["TransferOrderDetailOrderQtyInBaseUom"];
                //this.lblOrderDetailStatus.Text = wtList["TransferOrderDetailOrderDetailStatus"];
                //this.lblRevNum.Text = wtList["TransferOrderDetailRevNum"];

              
                //gvGtinValue.Columns[2].HeaderText = wtList["TransferOrderDetailOrderItem"];
                gvGtinValue.Columns[4].HeaderText = wtList["TransferOrderDetailOrderGtin"];
                gvGtinValue.Columns[5].HeaderText = wtList["TransferOrderDetailOrderGtinLotnum"];
                gvGtinValue.Columns[6].HeaderText = wtList["TransferOrderDetailOrderQtyInBaseUom"];
                gvGtinValue.Columns[7].HeaderText = wtList["TransferOrderDetailOrderUom"];

              
                //    this.btnAdd.Visible = actionList.Contains("AddTransferOrderDetail");
                //    this.btnAdd.Text = wtList["TransferOrderDetailAddButton"];

                btnRelease.Visible = false;
                btnPack.Visible = false;
                btnShip.Visible = false;
                btnCancel.Visible = false;

                if (Request.QueryString["orderNum"] != null)
                {
                    string orderNumber = Request.QueryString["orderNum"].ToString();

                    //Requested = 0,
                    //Released = 1,
                    //Packed = 2,
                    //Shipped = 3,
                    //Cancelled = -1

                    TransferOrderHeader orderheader = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));

                    txtOrderNum.Text = orderheader.OrderNum.ToString();
                    txtOrderSchedReplenishDate.Text = orderheader.OrderSchedReplenishDate.ToString("dd-MMM-yyyy");
                    txtOrderFacilityFrom.Text = orderheader.OrderFacilityFromObject.Name;
                    txtOrderFacilityTo.Text = orderheader.OrderFacilityToObject.Name;
                    int ostatus = orderheader.OrderStatus;
                    switch (ostatus)
                    {
                        case 0:
                            txtOrderStatus.Text = "Requested";
                            break;
                        case 1:
                            txtOrderStatus.Text = "Released";
                            break;
                        case 2:
                            txtOrderStatus.Text = "Packed";
                            break;
                        case 3:
                            txtOrderStatus.Text = "Shipped";
                            break;
                        case -1:
                            txtOrderStatus.Text = "Canceled";
                            break;
                    }
                    txtOrderCarrier.Text = orderheader.OrderCarrier;

                    List<TransferOrderDetail> orderDetailList = TransferOrderDetail.GetTransferOrderDetailByOrderNumAsList(int.Parse(orderNumber), CurrentEnvironment.LoggedUser.HealthFacility.Code);

                    if (orderDetailList.Count >= 1)
                    {
                        if (orderheader.OrderStatus == 0)
                        {
                            btnRelease.Visible = true;
                            btnCancel.Visible = true;
                            gvCreateTOD.Visible = false;
                            gvGtinValue.Visible = true;
                            ddlItemLot.Visible = false;
                        }
                        else if (orderheader.OrderStatus == 1)
                        {
                            btnPack.Visible = true;
                            btnCancel.Visible = true;
                            gvCreateTOD.Visible = false;
                            gvGtinValue.Visible = true;
                        }
                        else if (orderheader.OrderStatus == 2)
                        {
                            btnShip.Visible = true;
                            btnCancel.Visible = true;
                            gvCreateTOD.Visible = false;
                            gvGtinValue.Visible = true;
                        }
                        else if (orderheader.OrderStatus == 3)
                        {
                            btnAdd.Visible = false;
                            ddlGtin.Visible = false;
                            ddlItemLot.Visible = false;
                            ddlUom.Visible = false;
                            txtQuantity.Visible = false;
                            btnEdit.Visible = false;
                            btnAddDetail.Visible = false;
                            tbl.Visible = false;
                        }
                        else if (orderheader.OrderStatus == -1)
                        {
                            btnAdd.Visible = false;
                            ddlGtin.Visible = false;
                            ddlItemLot.Visible = false;
                            ddlUom.Visible = false;
                            txtQuantity.Visible = false;
                            btnEdit.Visible = false;
                            btnAddDetail.Visible = false;
                            tbl.Visible = false;
                        }
                        hlPrintPackingSlip.Visible = orderheader.OrderStatus > 0;
                        
                        hlPrintPackingSlip.NavigateUrl = String.Format("{0}/Pack_Order_Report.pdf?j_username={1}&j_password={2}&OrderNum={3}",
                            ConfigurationManager.AppSettings["JasperServer"],
                            ConfigurationManager.AppSettings["JasperUser"],
                            ConfigurationManager.AppSettings["JasperPassword"],
                            orderheader.OrderNum);
                        odsGtinValue.SelectParameters.Clear();
                        odsGtinValue.SelectParameters.Add("orderNumber", orderNumber);
                        odsGtinValue.SelectParameters.Add("healthFacilityCode", CurrentEnvironment.LoggedUser.HealthFacility.Code);
                        gvGtinValue.DataSourceID = "odsGtinValue";
                        gvGtinValue.DataBind();

                        //List<Uom> uomList = Uom.GetUomList();
                        //ddlUom.DataSource = uomList;
                        //ddlUom.DataBind();

                        DataTable dt = HealthFacilityBalance.GetItemManufacturerBalanceForDropDown(CurrentEnvironment.LoggedUser.HealthFacilityId);
                        ddlGtin.DataSource = dt;
                        ddlGtin.DataBind();

                    }
                    else
                    {
                        gvCreateTOD.Visible = true;
                        gvGtinValue.Visible = false;
                        odsCreateTOD.SelectParameters.Clear();
                        odsCreateTOD.SelectParameters.Add("healthFacilityCode", CurrentEnvironment.LoggedUser.HealthFacility.Code);
                        gvCreateTOD.DataSourceID = "odsCreateTOD";
                        gvCreateTOD.DataBind();
                        btnAdd.Visible = true;

                        //hide cotrols
                        tbl.Visible = false;
                        ddlGtin.Visible = false;
                        ddlItemLot.Visible = false;
                        ddlUom.Visible = false;
                        txtQuantity.Visible = false;
                        btnEdit.Visible = false;
                        btnAddDetail.Visible = false;
                    }
                }

            }
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

    protected void btnAddDetail_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (Page.IsValid)
            {
                string orderNumber = "";
                if (Request.QueryString["orderNum"] != null)
                {
                    orderNumber = Request.QueryString["orderNum"].ToString();
                }
                TransferOrderHeader toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));

                OrderManagementLogic oml = new OrderManagementLogic();
                string gtin = ddlGtin.SelectedValue;
                string lot = "*";
                if (ddlItemLot.SelectedIndex > 0)
                    lot = ddlItemLot.SelectedValue;

                int qty = 0;
                int quantity = 0;
                if (!String.IsNullOrEmpty(txtQuantity.Text))
                {
                    qty = int.Parse(txtQuantity.Text);
                    string uom = ddlUom.SelectedValue;
                                      
                    ItemManufacturer im = ItemManufacturer.GetItemManufacturerByGtin(gtin);
                    if (uom.Equals(im.BaseUom))
                        quantity = qty;
                    else if (uom.Equals(im.Alt1Uom))
                        quantity = qty * im.Alt1QtyPer;
                    else if (uom.Equals(im.Alt2Uom))
                        quantity = qty * im.Alt2QtyPer;
                }
                Uom u = Uom.GetUomByName(ddlUom.SelectedValue); //GetuomByName
                
                TransferOrderDetail tod = TransferOrderDetail.GetTransferOrderDetailByOrderNumGtinLotNumber(int.Parse(orderNumber), gtin, lot);
                if (tod == null)
                {
                    TransferOrderDetail td = oml.AddOrderLine(toh, gtin, lot, quantity, u, CurrentEnvironment.LoggedUser.Id);

                    if (td != null)
                    {
                        //ClearControls(this);
                        gvGtinValue.DataBind();
                        ddlGtin.SelectedIndex = 0;
                        //ddlItemLot.SelectedIndex = 0;
                        txtQuantity.Text = string.Empty;
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
                int index = 0;
                for (int rowIndex = 0; rowIndex < gvGtinValue.Rows.Count; rowIndex++)
                {
                    //extract the TextBox values

                    Label lblOrderGtin = (Label)gvGtinValue.Rows[rowIndex].Cells[3].FindControl("lblOrderGtin");
                    TextBox txtValue = (TextBox)gvGtinValue.Rows[rowIndex].Cells[5].FindControl("txtValue");
                    Label lblId = (Label)gvGtinValue.Rows[rowIndex].Cells[0].FindControl("lblId");
                    Label lblId2 = (Label)gvGtinValue.Rows[rowIndex].Cells[1].FindControl("lblId2");
                    Label lblItem = (Label)gvGtinValue.Rows[rowIndex].Cells[2].FindControl("lblItem");
                    //DropDownList ddlHealthFacilityFrom = (DropDownList)gvGtinValue.Rows[rowIndex].Cells[1].FindControl("ddlHealthFacilityFrom");
                    //DropDownList ddlHealthFacilityTo = (DropDownList)gvGtinValue.Rows[rowIndex].Cells[1].FindControl("ddlHealthFacilityTo");
                    DropDownList ddlUom = (DropDownList)gvGtinValue.Rows[rowIndex].Cells[6].FindControl("ddlUom");
                    DropDownList ddlGtinLotNumber = (DropDownList)gvGtinValue.Rows[rowIndex].Cells[4].FindControl("ddlItemLot");

                    string id = lblId.Text;
                    int id2 = int.Parse(lblId2.Text);

                    string orderNumber = "";
                    if (Request.QueryString["orderNum"] != null)
                    {
                        orderNumber = Request.QueryString["orderNum"].ToString();
                    }

                    TransferOrderDetail tod = TransferOrderDetail.GetTransferOrderDetailByOrderNumAndOrderDetail(int.Parse(id), id2);

                    if (tod != null)
                    {
                        if (!String.IsNullOrEmpty(txtValue.Text))
                        {
                            tod.OrderNum = int.Parse(orderNumber);
                            tod.OrderGtin = lblOrderGtin.Text;
                            tod.OrderQty = double.Parse(txtValue.Text);
                            tod.OrderQtyInBaseUom = tod.OrderQty;
                            tod.OrderUom = ddlUom.SelectedValue;
                            tod.OrderGtinLotnum = ddlGtinLotNumber.SelectedValue;
                            tod.OrderDetailStatus = GetStatusId(txtOrderStatus.Text);
                         
                            OrderManagementLogic oml = new OrderManagementLogic();
                            TransferOrderDetail t = oml.UpdateOrderLine(tod, CurrentEnvironment.LoggedUser.Id);
                            int updated = TransferOrderDetail.Update(tod);
                            index = updated;
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(txtValue.Text))
                        {
                            tod = new TransferOrderDetail();
                            tod.OrderNum = int.Parse(orderNumber);
                            tod.OrderGtin = lblOrderGtin.Text;
                            tod.OrderGtinLotnum = "*";
                            tod.OrderQty = double.Parse(txtValue.Text);
                            tod.OrderQtyInBaseUom = tod.OrderQty;
                            tod.OrderUom = ddlUom.SelectedValue;
                            tod.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
                            tod.ModifiedOn = DateTime.Now;
                            tod.OrderDetailDescription = lblItem.Text;
                            tod.OrderDetailStatus = 0; //requested
                            //employ gtinparent logic + uom logic 

                            int updated = TransferOrderDetail.Insert(tod);
                            index = updated;
                        }
                    }
                }
                if (index > 0)
                {
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

    protected void gvGtinValue_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;

        TransferOrderHeader toh = null;
        string orderNumber = Request.QueryString["OrderNum"];
        if (!string.IsNullOrEmpty(orderNumber))
        {
            toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));
            if (toh.OrderStatus == 0)
                e.Row.Cells[4].Visible = false;
        }


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOrderGtin = (Label)e.Row.FindControl("lblOrderGtin");
            TextBox txtValue = (TextBox)e.Row.FindControl("txtValue");
            Label lblId = (Label)e.Row.FindControl("lblId");
            Label lblId2 = (Label)e.Row.FindControl("lblId2");
            Label lblItem = (Label)e.Row.FindControl("lblItem");
            Label lblFromStock = (Label)e.Row.FindControl("lblDistrictStock");

            DropDownList ddlUom = (DropDownList)e.Row.FindControl("ddlUom");
            DataTable dt = ItemManufacturer.GetUomFromGtin(lblOrderGtin.Text);
           // List<Uom> uomList = Uom.GetUomList();
            ddlUom.DataSource = dt;
            ddlUom.DataBind();


            //if (toh != null) // && toh.OrderStatus == 0)
            //    ddlUom.Enabled = true;
            //else ddlUom.Enabled = false;

            string id = lblId.Text;
            int id2 = int.Parse(lblId2.Text);

            TransferOrderDetail tod = TransferOrderDetail.GetTransferOrderDetailByOrderNumAndOrderDetail(int.Parse(id), id2);

            lblFromStock.Text = GIIS.DataLayer.HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode(toh.OrderFacilityFrom).Find(o=>o.Gtin == tod.OrderGtin).Balance.ToString();
            lblFromStock.Text += " " + tod.OrderUom;

            if (tod != null)
            {
                txtValue.Text = tod.OrderQty.ToString();
                ddlUom.SelectedValue = tod.OrderUom;
                ddlUom.Enabled = false;
            }


            DropDownList ddlItemLot = (DropDownList)e.Row.FindControl("ddlItemLot");
            ObjectDataSource odsItemLot = (ObjectDataSource)e.Row.FindControl("odsItemLot");

            odsItemLot.SelectParameters.Clear();
            odsItemLot.SelectParameters.Add("gtin", lblOrderGtin.Text);
            odsItemLot.DataBind();
            ddlItemLot.DataSource = odsItemLot;
            ddlItemLot.DataBind();
            if (tod.OrderDetailStatus > 0)
            {
                ddlItemLot.SelectedValue = tod.OrderGtinLotnum;
            }
            if (tod.OrderDetailStatus == 3)
            {
                txtValue.Enabled = false;
                ddlItemLot.Enabled = false;
            }
        }

    }


    protected void btnRelease_Click(object sender, EventArgs e)
    {
        string orderNumber = Request.QueryString["OrderNum"];
        if (!string.IsNullOrEmpty(orderNumber))
        {
            TransferOrderHeader toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));

            OrderManagementLogic oml = new OrderManagementLogic();
            TransferOrderHeader tohCancel = oml.ReleaseOrder(toh, CurrentEnvironment.LoggedUser.Id);

            string url = string.Format("TransferOrderDetail.aspx?OrderNum={0}", orderNumber);
            Response.Redirect(url, false);
        }
    }
    protected void btnPack_Click(object sender, EventArgs e)
    {
        string orderNumber = Request.QueryString["OrderNum"];
        if (!string.IsNullOrEmpty(orderNumber))
        {
            TransferOrderHeader toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));

            OrderManagementLogic oml = new OrderManagementLogic();
            TransferOrderHeader tohCancel = oml.PackOrder(toh, CurrentEnvironment.LoggedUser.Id);

            string url = string.Format("TransferOrderDetail.aspx?OrderNum={0}", orderNumber);
            Response.Redirect(url, false);
        }
    }
    protected void btnShip_Click(object sender, EventArgs e)
    {
        string orderNumber = Request.QueryString["OrderNum"];
        if (!string.IsNullOrEmpty(orderNumber))
        {
            TransferOrderHeader toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));

            OrderManagementLogic oml = new OrderManagementLogic();
            TransferOrderHeader tohCancel = oml.ShipOrder(toh, CurrentEnvironment.LoggedUser.Id);

            string url = string.Format("TransferOrderDetail.aspx?OrderNum={0}", orderNumber);
            Response.Redirect(url, false);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string orderNumber = Request.QueryString["OrderNum"];
        if (!string.IsNullOrEmpty(orderNumber))
        {
            TransferOrderHeader toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));

            OrderManagementLogic oml = new OrderManagementLogic();
            TransferOrderHeader tohCancel = oml.CancelOrder(toh, CurrentEnvironment.LoggedUser.Id);

            string url = string.Format("TransferOrderDetail.aspx?OrderNum={0}", orderNumber);
            Response.Redirect(url, false);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                string orderNumber = "";
                if (Request.QueryString["orderNum"] != null)
                {
                    orderNumber = Request.QueryString["orderNum"].ToString();
                }
                int index = 0;
                for (int rowIndex = 0; rowIndex < gvCreateTOD.Rows.Count; rowIndex++)
                {
                    //extract the TextBox values

                    Label lblOrderGtin = (Label)gvCreateTOD.Rows[rowIndex].Cells[0].FindControl("lblGtin");
                    TextBox txtValue = (TextBox)gvCreateTOD.Rows[rowIndex].Cells[2].FindControl("txtQty");
                    Label lblItem = (Label)gvCreateTOD.Rows[rowIndex].Cells[1].FindControl("lblGtinItem");
                    DropDownList ddlUom = (DropDownList)gvCreateTOD.Rows[rowIndex].Cells[3].FindControl("ddlUom2");

                    if (!String.IsNullOrEmpty(txtValue.Text))
                    {

                        string gtin = lblOrderGtin.Text;
                        string uom = ddlUom.SelectedValue;
                        double qty = double.Parse(txtValue.Text);
                        string item = lblItem.Text;

                        index = NewOrderDetail(orderNumber, gtin, qty, uom, item);

                        if (index > 0)
                        {
                            List<ItemManufacturer> imlist = ItemManufacturer.GetItemManufacturerByParentGtin(gtin);
                            if (imlist.Count > 0)
                            {
                                foreach (ItemManufacturer imo in imlist)
                                {
                                    index = NewOrderDetail(orderNumber, imo.Gtin, qty, uom, imo.ItemObject.Code);
                                }
                            }

                        }
                    }
                }
                    if (index > 0)
                    {
                        lblSuccess.Visible = true;
                        lblWarning.Visible = false;
                        lblError.Visible = false;
                        gvCreateTOD.Visible = false;
                        gvGtinValue.Visible = true;
                        odsGtinValue.SelectParameters.Clear();
                        odsGtinValue.SelectParameters.Add("orderNumber", orderNumber);
                        odsGtinValue.SelectParameters.Add("healthFacilityCode", CurrentEnvironment.LoggedUser.HealthFacility.Code);
                        gvGtinValue.DataSourceID = "odsGtinValue";
                        gvGtinValue.DataBind();
                        tbl.Visible = true;
                        ddlGtin.Visible = true;
                        ddlUom.Visible = true;
                        txtQuantity.Visible = true;
                        btnAdd.Visible = false;
                        btnEdit.Visible = true;
                        btnAddDetail.Visible = true;
                        btnRelease.Visible = true;
                        btnCancel.Visible = true;

                        ////bind ddl
                        //List<Uom> uomList = Uom.GetUomList();
                        //ddlUom.DataSource = uomList;
                        //ddlUom.DataBind();

                        DataTable dt = HealthFacilityBalance.GetItemManufacturerBalanceForDropDown(CurrentEnvironment.LoggedUser.HealthFacilityId);
                        ddlGtin.DataSource = dt;
                        ddlGtin.DataBind();
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
    private int NewOrderDetail(string orderNumber, string gtin, double qty, string uom, string item)
    {
        TransferOrderDetail tod = new TransferOrderDetail();
        tod.OrderNum = int.Parse(orderNumber);
        tod.OrderGtin = gtin;
        tod.OrderGtinLotnum = "*";

        int quantity = 0;

        ItemManufacturer im = ItemManufacturer.GetItemManufacturerByGtin(gtin);
        if (uom.Equals(im.BaseUom))
            quantity = (int)qty;
        else if (uom.Equals(im.Alt1Uom))
            quantity = (int)(qty * im.Alt1QtyPer);
        else if (uom.Equals(im.Alt2Uom))
            quantity = (int)(qty * im.Alt2QtyPer);
        tod.OrderQty = qty;
        tod.OrderQtyInBaseUom = (double)quantity;
        tod.OrderUom = uom;
        tod.ModifiedBy = CurrentEnvironment.LoggedUser.Id;
        tod.ModifiedOn = DateTime.Now;
        tod.OrderDetailDescription = item;
        tod.OrderDetailStatus = 0; //requested

        int i = TransferOrderDetail.Insert(tod);
        return i;
    }

    private int GetStatusId(string status)
    {
        int result = 0;

        switch (status)
        {
            case "Requested":
                result = 0;
                break;
            case "Released":
                result = 1;
                break;
            case "Packed":
                result = 2;
                break;
            case "Shipped":
                result = 3;
                break;
            case "Canceled":
                result = -1;
                break;
        }

        return result;
    }

   
    protected void gvCreateTOD_DataBound(object sender, EventArgs e)
    {
        if (gvCreateTOD.Rows.Count > 0)
        {

            TransferOrderHeader toh = null;
            string orderNumber = Request.QueryString["OrderNum"];
            if (!string.IsNullOrEmpty(orderNumber))
            {
                toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));
            }

            foreach (GridViewRow gvr in gvCreateTOD.Rows)
            {
                DropDownList ddlUom = (DropDownList)gvr.Cells[3].FindControl("ddlUom2");
                ObjectDataSource odsUom = (ObjectDataSource)gvr.Cells[3].FindControl("odsUom");
                Label lblGtin = (Label)gvr.Cells[1].FindControl("lblGtin");

                // District stock
                // HACK: If you see it, you know why it needs to be changed, if not... 
                Label lblFromStock = (Label)gvr.FindControl("lblDistrictStock");
                var bal = GIIS.DataLayer.HealthFacilityBalance.GetHealthFacilityBalanceByHealthFacilityCode(toh.OrderFacilityFrom).Find(o => o.Gtin == lblGtin.Text);
                if(bal != null)
                    lblFromStock.Text = String.Format("{0} {1}", bal.Balance, bal.GtinObject.BaseUom);

                odsUom.SelectParameters.Clear();
                odsUom.SelectParameters.Add("gtin", lblGtin.Text);
                odsUom.DataBind();
                ddlUom.DataSourceID = "odsUom";
                ddlUom.DataBind();
            }
        }
    }

}