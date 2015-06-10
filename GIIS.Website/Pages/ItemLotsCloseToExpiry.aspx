<%@ Page Title="Item Lots Close to Expiry" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemLotsCloseToExpiry.aspx.cs" Inherits="Pages_ItemLotsCloseToExpiry" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                  <li><a href="#">Reports</a></li>
                <li><a href="#">Stock Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Item Lots Close to Expiry" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItem" runat="server" Text="Item" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" DataSourceID="odsItems" DataTextField="Code" DataValueField="Id">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsItems" runat="server" SelectMethod="GetItemsList" TypeName="GIIS.DataLayer.Item"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblEntryDateFrom" runat="server" Text="Expire Date:" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtExpireDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceExpireDate" runat="server" TargetControlID="txtExpireDate" />
                <asp:RegularExpressionValidator ID="revExpireDate" runat="server" ControlToValidate="txtExpireDate" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no item lots that match your search criteria!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
     <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
        <asp:GridView ID="gvItemLot" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsItemLot" OnDataBound="gvItemLot_DataBound" OnRowDataBound="gvItemLot_RowDataBound">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                <asp:TemplateField HeaderText="Item Lot">
                    <ItemTemplate>
                        <%#Eval("ItemLot.LotNumber")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("ItemLot.Item.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Arrival Date">
                    <ItemTemplate>
                        <asp:Label ID="lblEntryDate" runat="server"></asp:Label>
                        <%-- <%#Eval("ItemLot.EntryDate")%>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expire Date">
                    <ItemTemplate>
                        <asp:Label ID="lblExpireDate" runat="server"></asp:Label>
                        <%--   <%#Eval("ItemLot.ExpireDate")%>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Received" HeaderText="Received" SortExpression="Received" />
                <asp:BoundField DataField="Distributed" HeaderText="Distributed" SortExpression="Distributed" />
                <asp:BoundField DataField="Used" HeaderText="Used" SortExpression="Used" />
                <asp:BoundField DataField="Wasted" HeaderText="Wasted" SortExpression="Wasted" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" />
                <asp:BoundField DataField="ItemLotId" HeaderText="ItemLotId" SortExpression="ItemLotId" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsItemLot" runat="server" SelectMethod="GetExpiringLots" TypeName="GIIS.DataLayer.HealthFacilityItemLotBalance">
            <SelectParameters>
                <asp:Parameter Name="hfId" Type="Int32" />
                <asp:Parameter Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div></div> 
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

