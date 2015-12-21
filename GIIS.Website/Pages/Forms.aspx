<%-- 
*******************************************************************************
  Copyright 2015 TIIS - Tanzania Immunization Information System

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 ******************************************************************************
--%>

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
                <li><h4 runat="server" id="frms">Scan Forms</h4>
                    <ul>
                        <li><a runat="server" id="tz02" href="/forms/TZ02.pdf">Form #2 - Registration</a></li>
                        <li><a runat="server" id="tz03" href="/forms/TZ03.pdf">Form #3 - Weigh Tally (New)</a></li>
                        <li><a runat="server" id="tz04" href="/forms/TZ04.pdf">Form #4 - Stock Tally (New)</a></li>
                        <li><a runat="server" id="tz05" href="/forms/TZ05.pdf">Facility Monthly Stock Status (New)</a></li>
                    </ul>
                </li>
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

