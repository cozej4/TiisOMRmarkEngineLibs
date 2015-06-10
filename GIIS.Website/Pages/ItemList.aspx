<%@ Page Title="Item List" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemList.aspx.cs" Inherits="_ItemList" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Setup</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Item List" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <%-- <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>--%>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix" style="text-align: right">
            <asp:Label ID="lblItemCategoryId" runat="server" Text="ItemCategoryId" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control" DataSourceID="odsItems" DataTextField="Name" DataValueField="Id">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsItems" runat="server" SelectMethod="GetItemCategoryForListAll" TypeName="GIIS.DataLayer.ItemCategory"></asp:ObjectDataSource>

            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblName" runat="server" Text="Name" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
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
            <asp:Label ID="lblWarning" runat="server" Text="There are no items that match your search criteria!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnAddNew_Click" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnDataBound="gvItem_Databound" OnPageIndexChanging="gvItem_PageIndexChanging" OnRowDataBound="gvItem_RowDataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />

                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "Item.aspx?id={0}") %>'
                                Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                    <asp:TemplateField HeaderText="Item Category">
                        <ItemTemplate>
                            <%#Eval("ItemCategory.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                 <%--   <asp:TemplateField HeaderText="Hl7Vaccine">
                        <ItemTemplate>
                            <%#Eval("Hl7Vaccine.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="ExitDate" HeaderText="ExitDate" SortExpression="ExitDate" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />

                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsItem" runat="server" EnablePaging="true" SelectMethod="GetPagedItemList" TypeName="GIIS.DataLayer.Item" SelectCountMethod="GetCountItemList">
                <SelectParameters>
                    <asp:Parameter Name="name" Type="String" />
                    <asp:Parameter Name="itemCategoryId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnExcel" runat="server" Text="Excel" Visible="false" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
    </div>

    <div class="row" style="overflow: auto">
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvItem_Databound" Visible="false" OnRowDataBound="gvItem_RowDataBound">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                <asp:TemplateField HeaderText="Item Category">
                    <ItemTemplate>
                        <%#Eval("ItemCategory.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hl7Vaccine">
                    <ItemTemplate>
                        <%#Eval("Hl7Vaccine.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="ExitDate" HeaderText="ExitDate" SortExpression="ExitDate" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

