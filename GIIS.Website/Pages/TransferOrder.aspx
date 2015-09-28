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
<%@ Page Title="TransferOrder" Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/MasterPage.master" CodeFile="TransferOrder.aspx.cs" Inherits="Pages_TransferOrder" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
               <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Stock Issue" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">

            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                <asp:Button ID="btnRelease" runat="server" Text="Release" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnRelease_Click" />
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                <asp:Button ID="btnPack" runat="server" Text="Pack" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnPack_Click" />
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                <asp:Button ID="btnShip" runat="server" Text="Ship" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnShip_Click" />
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnCancel_Click" />
            </div>

        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:GridView ID="gvTransferOrderStatus" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive" 
                DataSourceID="odsTransferOrderStatus" OnDataBound="gvTransferOrderStatus_DataBound" >
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:GridView>
            <asp:ObjectDataSource ID="odsTransferOrderStatus" runat="server" SelectMethod="GetTransferStatus" TypeName="GIIS.DataLayer.TransferOrderHeader">
                <SelectParameters>
                    <asp:Parameter Name="OrderFacilityFrom" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            &nbsp;
        </div>
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
            <asp:GridView ID="gvTransferOrderHeader" runat="server" AllowPaging="true" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnRowDataBound="gvTransferOrderHeader_RowDataBound" DataSourceID="odsTransferOrderHeader" OnDataBound="gvTransferOrderHeader_DataBound" >
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <%--<asp:BoundField DataField="OrderNum" HeaderText="OrderNum" SortExpression="OrderNum" />--%>
                    <asp:TemplateField HeaderText="Order Number">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="Id"
                                NavigateUrl='<%# String.Format("TransferOrderDetail.aspx?OrderNum={0}&flag=1", Eval("OrderNum")) %>'
                                Text='<%# Eval("OrderNum", "{0}") %>'>                                 
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderSchedReplenishDate" HeaderText="Date" SortExpression="OrderSchedReplenishDate" />
                    <asp:TemplateField HeaderText="Health Center From" Visible="false">
                        <ItemTemplate>
                            <%#Eval("OrderFacilityFromObject.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Health Center To">
                        <ItemTemplate>
                            <%#Eval("OrderFacilityToObject.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderCarrier" HeaderText="Carrier" SortExpression="OrderCarrier" />
                    <asp:BoundField DataField="OrderStatusName" HeaderText="Status" SortExpression="OrderStatusName" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsTransferOrderHeader" runat="server" EnablePaging="true" TypeName="GIIS.DataLayer.TransferOrderHeader"
                SelectMethod="GetPagedTransferOrderHeaderList" SelectCountMethod="GetCountTransferOrderHeaderList" >
                <SelectParameters>
                    <asp:Parameter Name="OrderFacilityFrom" Type="String" />
                    <asp:Parameter Name="orderStatus" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </div>
    </div>

    <br />
    <%--<div class="row">
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">&nbsp;</div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <asp:GridView ID="gvTransferOrderDetails" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsTransferOrderDetails">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:GridView>
            <asp:ObjectDataSource ID="odsTransferOrderDetails" runat="server" SelectMethod="GetTransferOrderDetailsByHealthFacilityId" TypeName="GIIS.DataLayer.TransferOrderDetail">
                <SelectParameters>
                    <asp:Parameter Name="OrderFacilityFrom" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </div>
    </div>
    <br />--%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
