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
<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemManufacturerList.aspx.cs" Inherits="Pages_ItemManufacturerList" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Stock</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Item Manufacturer List" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblName" runat="server" Text="Item" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" DataSourceID="odsItem" DataTextField="Name" DataValueField="Id" />
                <asp:ObjectDataSource ID="odsItem" runat="server" SelectMethod="GetItemForList" TypeName="GIIS.DataLayer.Item"></asp:ObjectDataSource>
            </div>
        </div>
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
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnAddNew_Click" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvItemManufacturer" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsItemManufacturer" OnDataBound="gvItemManufacturer_DataBound" OnPageIndexChanging="gvItemManufacturer_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField HeaderText="GTIN" SortExpression="Gtin">
                        <ItemTemplate>
                            <asp:HyperLink ID="Gtin" runat="server" NavigateUrl='<%# Eval("Gtin", "ItemManufacturer.aspx?gtin={0}") %>'
                                Text='<%# Eval("Gtin", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <%#Eval("ItemObject.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Manufacturer">
                        <ItemTemplate>
                            <%#Eval("ManufacturerObject.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BaseUom" HeaderText="BaseUom" SortExpression="BaseUom" />
                    <asp:BoundField DataField="Alt1Uom" HeaderText="Alt1Uom" SortExpression="Alt1Uom" />
                    <asp:BoundField DataField="Alt1QtyPer" HeaderText="Alt1QtyPer" SortExpression="Alt1QtyPer" />
                    <asp:BoundField DataField="Alt2Uom" HeaderText="Alt2Uom" SortExpression="Alt2Uom" />
                    <asp:BoundField DataField="Alt2QtyPer" HeaderText="Alt2QtyPer" SortExpression="Alt2QtyPer" />
                    <asp:TemplateField HeaderText="KitItems">
                        <ItemTemplate>
                                <%# (int)Eval("KitItems.Count") > 0 ? "<ul style=\"margin:0px 0px 0px 7px; padding:0px\">" + ((List<GIIS.DataLayer.ItemManufacturer>)Eval("KitItems")).Select(o=>o.ItemObject.Code).Aggregate((a,b)=>String.Format("<li>{0}</li><li>{1}</li>", a, b)) + "</ul>" : ""%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />

                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsItemManufacturer" runat="server" EnablePaging="True" SelectMethod="GetPagedItemManufacturerList" TypeName="GIIS.DataLayer.ItemManufacturer" SelectCountMethod="GetCountItemManufacturerList">
                <SelectParameters>
                    <asp:Parameter Name="itemId" Type="Int32" />
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
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvItemManufacturer_DataBound" Visible="false">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:TemplateField HeaderText="GTIN" SortExpression="Gtin">
                    <ItemTemplate>
                        <%#Eval("Gtin")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("ItemObject.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Manufacturer">
                    <ItemTemplate>
                        <%#Eval("ManufacturerObject.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BaseUom" HeaderText="BaseUom" SortExpression="BaseUom" />
                <asp:BoundField DataField="Alt1Uom" HeaderText="Alt1Uom" SortExpression="Alt1Uom" />
                <asp:BoundField DataField="Alt1QtyPer" HeaderText="Alt1QtyPer" SortExpression="Alt1QtyPer" />
                <asp:BoundField DataField="Alt2Uom" HeaderText="Alt2Uom" SortExpression="Alt2Uom" />
                <asp:BoundField DataField="Alt2QtyPer" HeaderText="Alt2QtyPer" SortExpression="Alt2QtyPer" />
                <asp:BoundField DataField="GtinParent" HeaderText="GtinParent" SortExpression="GtinParent" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

