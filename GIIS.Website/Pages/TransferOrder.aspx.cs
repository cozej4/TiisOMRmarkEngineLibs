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

using GIIS.BusinessLogic;

public partial class Pages_TransferOrder : System.Web.UI.Page
{
    static bool first;
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
                Dictionary<string, string> wtList = (Dictionary<string, string>)HttpContext.Current.Cache["TransferOrderHeader-dictionary" + language];
                if (wtList == null)
                {
                    List<WordTranslate> wordTranslateList = WordTranslate.GetWordByLanguage(languageId, "TransferOrderHeader");
                    wtList = new Dictionary<string, string>();
                    foreach (WordTranslate vwt in wordTranslateList)
                        wtList.Add(vwt.Code, vwt.Name);
                    HttpContext.Current.Cache.Insert("TransferOrderHeader-dictionary" + language, wtList);
                }

                //controls

                //actions
                //btnRelease.Visible = actionList.Contains("ReleaseTransferOrderDetail");
                //btnPack.Visible = actionList.Contains("PackTransferOrderDetail");
                //btnShip.Visible = actionList.Contains("ShipTransferOrderDetail");
                //btnCancel.Visible = actionList.Contains("CancelTransferOrderDetail");

                //buttons
                //btnRelease.Text = wtList["TransferOrderDetailReleaseButton"];
                //btnPack.Text = wtList["TransferOrderDetailPackButton"];
                //btnShip.Text = wtList["TransferOrderDetailShipButton"];
                //btnCancel.Text = wtList["TransferOrderDetailCancelButton"];

                //messages

                //Page Title
                lblTitle.Text = wtList["TransferOrderHeaderListPageTitle"];

                btnRelease.Visible = false;
                btnPack.Visible = false;
                btnShip.Visible = false;
                btnCancel.Visible = false;

                string orderNumber = Request.QueryString["OrderNum"];
                if (!string.IsNullOrEmpty(orderNumber))
                {
                    //Requested = 0,
                    //Released = 1,
                    //Packed = 2,
                    //Shipped = 3,
                    //Cancelled = -1

                    TransferOrderHeader toh = TransferOrderHeader.GetTransferOrderHeaderByOrderNum(int.Parse(orderNumber));
                    if (toh.OrderStatus == 0)
                    {
                        btnRelease.Visible = true;
                        btnCancel.Visible = true;
                    }
                    else if (toh.OrderStatus == 1)
                    {
                        btnPack.Visible = true;
                        btnCancel.Visible = true;
                    }
                    else if (toh.OrderStatus == 2)
                    {
                        btnShip.Visible = true;
                        btnCancel.Visible = true;
                    }
                }
                string orderStatus = Request.QueryString["Status"];

                odsTransferOrderHeader.SelectParameters.Clear();
                odsTransferOrderHeader.SelectParameters.Add("OrderFacilityFrom", CurrentEnvironment.LoggedUser.HealthFacility.Code);
                odsTransferOrderHeader.SelectParameters.Add("orderStatus", orderStatus);
                gvTransferOrderHeader.DataSourceID = "odsTransferOrderHeader";
                gvTransferOrderHeader.DataBind();

                //odsTransferOrderDetails.SelectParameters.Clear();
                //odsTransferOrderDetails.SelectParameters.Add("OrderFacilityFrom", CurrentEnvironment.LoggedUser.HealthFacility.Code);
                //gvTransferOrderDetails.DataSourceID = "odsTransferOrderDetails";
                //gvTransferOrderDetails.DataBind();

                odsTransferOrderStatus.SelectParameters.Clear();
                odsTransferOrderStatus.SelectParameters.Add("OrderFacilityFrom", CurrentEnvironment.LoggedUser.HealthFacility.Code);
                gvTransferOrderStatus.DataSourceID = "odsTransferOrderStatus";
                gvTransferOrderStatus.DataBind();

                //grid header text
                //gvTransferOrderStatus.Columns[0].HeaderText = wtList["TransferOrderDetailOrderStatus"];
                //gvTransferOrderStatus.Columns[1].HeaderText = wtList["TransferOrderDetailTotal"];

                gvTransferOrderHeader.Columns[0].HeaderText = wtList["TransferOrderHeaderOrderNum"];
                gvTransferOrderHeader.Columns[1].HeaderText = wtList["TransferOrderHeaderOrderSchedReplenishDate"];
                gvTransferOrderHeader.Columns[2].HeaderText = wtList["TransferOrderHeaderOrderFacilityFrom"];
                gvTransferOrderHeader.Columns[3].HeaderText = wtList["TransferOrderHeaderOrderFacilityTo"];
                //gvTransferOrderHeader.Columns[4].HeaderText = wtList["TransferOrderDetailCarrier"];
                gvTransferOrderHeader.Columns[5].HeaderText = wtList["TransferOrderHeaderOrderIsActive"];


            }
        }
    }

    protected void gvTransferOrderHeader_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string dateformat = ConfigurationDate.GetConfigurationDateById(int.Parse(Configuration.GetConfigurationByName("DateFormat").Value)).DateFormat;
                ((BoundField)gvTransferOrderHeader.Columns[1]).DataFormatString = "{0:" + dateformat + "}";
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

            string url = string.Format("TransferOrder.aspx?OrderNum={0}", orderNumber);
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

            string url = string.Format("TransferOrder.aspx?OrderNum={0}", orderNumber);
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

            string url = string.Format("TransferOrder.aspx?OrderNum={0}", orderNumber);
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

            string url = string.Format("TransferOrder.aspx?OrderNum={0}", orderNumber);
            Response.Redirect(url, false);
        }
    }

    protected void gvTransferOrderStatus_DataBound(object sender, EventArgs e)
    {
        //foreach (GridViewRow row in gvTransferOrderStatus.Rows)
        //{
        //    HyperLink hp = new HyperLink();
        //    hp.Text = row.Cells[1].Text;
        //    hp.NavigateUrl = string.Format("TransferOrder.aspx?Status={0}", row.Cells[0].Text);
        //    row.Cells[1].Controls.Add(hp);
        //}
    }
    protected void gvTransferOrderHeader_DataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvTransferOrderStatus.Rows)
        {
            HyperLink h = (HyperLink)row.Cells[1].FindControl("hl");
            if (h == null)
            {
                HyperLink hp = new HyperLink();
                hp.Text = row.Cells[1].Text;
                hp.ID = "hl";
                hp.NavigateUrl = string.Format("TransferOrder.aspx?Status={0}", row.Cells[0].Text);
                row.Cells[1].Controls.Add(hp);
            }
        }
    }
}