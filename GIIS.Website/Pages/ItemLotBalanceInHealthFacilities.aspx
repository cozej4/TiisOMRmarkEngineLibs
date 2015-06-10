<%@ Page Title="Item Lot Balance in Health Facilities" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemLotBalanceInHealthFacilities.aspx.cs" Inherits="Pages_ItemLotBalanceInHealthFacilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Item Lot Balance" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemCategoryId" runat="server" Text="ItemCategoryId" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control" DataSourceID="odsItemCategory" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsItemCategory" runat="server" SelectMethod="GetItemCategoryForList" TypeName="GIIS.DataLayer.ItemCategory"></asp:ObjectDataSource>

            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemId" runat="server" Text="Item" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItems" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlItems_SelectedIndexChanged" DataSourceID="odsItem" DataTextField="Code" DataValueField="Id" />
                <asp:ObjectDataSource ID="odsItem" runat="server" SelectMethod="GetItemByItemCategoryId" TypeName="GIIS.DataLayer.Item">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlItemCategory" Name="i" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemLot" runat="server" Text="Item Lot" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItemLot" runat="server" CssClass="form-control" DataSourceID="odsItemLot" DataTextField="LOT_NUMBER" DataValueField="ID" />
                <asp:ObjectDataSource ID="odsItemLot" runat="server" SelectMethod="GetItemLots" TypeName="GIIS.DataLayer.ItemLot">
                    <SelectParameters>
                        <asp:Parameter Name="itemId" Type="Int32" />
                        <asp:Parameter Name="hfId" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
         <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no records!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvHealthFacilityBalance" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsHealthFacilityBalance" OnDataBound="gvHealthFacilityBalance_DataBound">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                    <asp:TemplateField HeaderText="Health Facility">
                        <ItemTemplate>
                            <%#Eval("HealthFacility.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Received" HeaderText="Received" SortExpression="Received" />
                    <asp:BoundField DataField="Distributed" HeaderText="Distributed" SortExpression="Distributed" />
                    <asp:BoundField DataField="Used" HeaderText="Used" SortExpression="Used" />
                    <asp:BoundField DataField="Wasted" HeaderText="Wasted" SortExpression="Wasted" />
                    <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsHealthFacilityBalance" runat="server" SelectMethod="GetItemLotBalanceInHealthFacilities" TypeName="GIIS.DataLayer.HealthFacilityItemLotBalance">
                <SelectParameters>
                    <asp:Parameter Name="hfId" Type="Int32" />
                    <asp:Parameter Name="itemlotid" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
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
</asp:Content>

