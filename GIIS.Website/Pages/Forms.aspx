
<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Forms.aspx.cs" Inherits="_Report" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Forms" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <ul runat="server" id="ulReports">

            </ul>
            <br />
            <br />
        </div>
    </div>
    <div class="row">
           <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
               <br />
           </div>
        
    </div>
    <!--
   <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <asp:Label ID="lblStockReports" runat="server" Text="Stock Reports" Font-Size="Large" ForeColor="LightSlateGray" Font-Bold="true" Font-Underline="true" />
            <br />
            <br />
            <ul>
                <li><a runat="server" id="aRunningBalance" href="HealthFacilityListForRunningBalance.aspx" target="_blank"><span id="mRunningBalance" runat="server">Running Balance</span></a></li>
                <li><a runat="server" id="aItemInHealthFacility" href="ItemBalanceInHealthFacilities.aspx" target="_blank"><span id="mItemInHealthFacility" runat="server">Item Balance in H.Facilities</span></a></li>
                <li><a runat="server" id="aItemLotInHealthFacility" href="ItemLotBalanceInHealthFacilities.aspx" target="_blank"><span id="mItemLotInHealthFacility" runat="server">Item Lot Balance in H.Facilities</span></a></li>
                <li><a runat="server" id="aStockCountList" href="HealthFacilityListForStockCount.aspx" target="_blank"><span id="mStockCountList" runat="server">View Stock Counts</span></a></li>
                <li><a runat="server" id="aAdjustmentsList" href="HealtFacilityListForAdjustments.aspx" target="_blank"><span id="mAdjustmentsList" runat="server">View Adjustments</span></a></li>
              
                <li><a runat="server" id="aLotTracking" href="LotTracking.aspx" target="_blank"><span id="mLotTracking" runat="server">Lot Tracking</span></a> </li>
                <li><a runat="server" id="aItemLotsCloseToExpiry" href="ItemLotsCloseToExpiry.aspx" target="_blank"><span id="mItemLotsCloseToExpiry" runat="server">Item Lots Close to Expiry</span></a></li>
                <li><a runat="server" id="aClosedVialWastage" href="ClosedVialWastage.aspx" target="_blank"><span id="mClosedVialWastage" runat="server">Closed Vial Wastage</span></a></li>
                <li><a runat="server" id="aConsumption" href="Consumption.aspx" target="_blank"><span id="mConsumption" runat="server">Consumption</span></a></li>
            </ul>
        </div>
    </div>-->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

