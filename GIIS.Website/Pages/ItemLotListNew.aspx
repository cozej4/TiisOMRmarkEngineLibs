<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemLotListNew.aspx.cs" Inherits="Pages_ItemLotListNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Stock</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Item Lot List" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
       <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblItem" runat="server" Text="Item" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" DataSourceID="odsItem" DataTextField="Name" DataValueField="Id" AutoPostBack="True" />
                <asp:ObjectDataSource ID="odsItem" runat="server" SelectMethod="GetItemForList" TypeName="GIIS.DataLayer.Item">
                </asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblGtin" runat="server" Text="GTIN" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlGtin" runat="server" CssClass="form-control" DataSourceID="odsGtin" DataTextField="Gtin" DataValueField="Gtin" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsGtin" runat="server" SelectMethod="GetGtinByItemId" TypeName="GIIS.DataLayer.ItemManufacturer">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlItem" Name="itemId" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised btn-sm" OnClick="btnSearch_Click" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no GTINs that match your search criteria!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnAddNew_Click" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvItemLotNew" runat="server" DataSourceID="odsItemLotNew" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" 
                OnDataBound="gvItemLot_DataBound" OnPageIndexChanging="gvItemLot_PageIndexChanging" >
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField HeaderText="GTIN">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="GTIN"
                                NavigateUrl='<%# String.Format("ItemLotNew.aspx?gtin={0}&lotno={1}", Eval("Gtin"), Eval("LotNumber")) %>'
                                Text='<%# Eval("Gtin", "{0}") %>'
                                ToolTip='<%# Eval("Notes", "{0}") %>'>                                 
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lot Number">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="LotNumber"
                                NavigateUrl='<%# String.Format("ItemLotNew.aspx?gtin={0}&lotno={1}", Eval("Gtin"), Eval("LotNumber")) %>'
                                Text='<%# Eval("LotNumber", "{0}") %>'
                                ToolTip='<%# Eval("Notes", "{0}") %>'>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <%#Eval("ItemObject.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ExpireDate" HeaderText="ExpireDate" SortExpression="ExpireDate" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                     <asp:CheckBoxField DataField="IsActive" HeaderText="Is Active" SortExpression="IsActive" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsItemLotNew" runat="server" EnablePaging="True" SelectMethod="GetPagedItemLotList" TypeName="GIIS.DataLayer.ItemLot" SelectCountMethod="GetCountItemLotList">
                <SelectParameters>
                    <asp:Parameter Name="itemId" Type="Int32" />
                    <asp:Parameter Name="gtin" Type="String" />
                </SelectParameters>
                
            </asp:ObjectDataSource>
        </div>
    </div>
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnExcel" runat="server" Text="Excel" Visible="false" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
    </div>
    <div class="row" style="overflow: auto">
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvItemLot_DataBound" 
            Visible="false">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:TemplateField HeaderText="GTIN">
                    <ItemTemplate>
                        <%# Eval("Gtin") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lot Number">
                    <ItemTemplate>
                       <%# Eval("LotNumber") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("ItemObject.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ExpireDate" HeaderText="ExpireDate" SortExpression="ExpireDate" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

