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
<%@ Page Title="Planned Coverage Report" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="PlannedCoverageReport.aspx.cs" Inherits="Pages_PlannedCoverageReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
    <script type="text/javascript">
        var isresult1 = "test";
        var isresult2 = "test";
        var isresult3 = "test";
        var isresult4 = "test";
        function pageLoad(sender, args) {
            $find('autocomplteextender4')._onMethodComplete = function (result, context) {
                $find('autocomplteextender4')._update(context, result, /* cacheResults */false);
                webservice_callback4(result, context);
            };
        }

        function webservice_callback4(result, context) {
            if (result == "") {
                $find("autocomplteextender4").get_element().style.backgroundColor = "red";
                isresult4 = "";
            }
            else {
                $find("autocomplteextender4").get_element().style.backgroundColor = "white";
                isresult4 = "test";
            }
        }

        function checkHFacility() {
            if (isresult4 == "") {
                alert("Please choose a domicile area from the list!");
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Planned Coverage Report" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
         <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
       <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblFromDate" runat="server" Text="From Date" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate" />
                <asp:RegularExpressionValidator ID="revFromDate" runat="server" ControlToValidate="txtFromDate" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate" ValidationGroup="SearchCohortCoverage" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblToDate" runat="server" Text="To Date" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate" />
                <asp:RegularExpressionValidator ID="revToDate" runat="server" ControlToValidate="txtToDate" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate" ValidationGroup="SearchCohortCoverage" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblEndDate" runat="server" Text="End Date" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceEndDate" runat="server" TargetControlID="txtEndDate" />
                <asp:RegularExpressionValidator ID="revEndDate" runat="server" ControlToValidate="txtEndDate" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" ValidationGroup="SearchCohortCoverage" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"></asp:RequiredFieldValidator>

            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblVaccine" runat="server" Text="Vaccine"></asp:Label>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlVaccine" runat="server" CssClass="form-control" DataSourceID="odsScheduledVaccination" DataTextField="Fullname" DataValueField="Id">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsScheduledVaccination" runat="server" SelectMethod="GetDoseListForReport" TypeName="GIIS.DataLayer.Dose"></asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <%--<div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>--%>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblDomicileId" runat="server" Text="Domicile" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc4:DomicileTextbox ID="txtDomicileId"
                    runat="server" ClearAfterSelect="false"
                    HighlightResults="true" OnClickSubmit="true"
                    OnValueSelected="Domicile_ValueSelected"
                    ServiceMethod="Place" />
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="btn btn-primary btn-raised" ValidationGroup="SearchCohortCoverage" OnClick="btnSearch_Click" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnNewSearch" Text="Clear" runat="server" CssClass="btn btn-default btn-raised" OnClick="btnNewSearch_Click" />
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvPlannedCoverage" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive" OnRowDataBound="gvPlannedCoverage_RowDataBound" OnDataBound="gvPlannedCoverage_DataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:GridView>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" CssClass="btn btn-material-bluegrey btn-raised" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnExcel" runat="server" Visible="false" Text="Excel" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>

    </div>
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:Literal ID="iCoverageReport" runat="server" />
        </div>
    </div>

    <br />

</asp:Content>

