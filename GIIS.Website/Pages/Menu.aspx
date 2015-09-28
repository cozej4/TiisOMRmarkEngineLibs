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
<%@ Page Title="Menu" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="_Menu" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="title">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Menu" /></h1>
    </div>
    <br />
    <div style="overflow: auto">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblParentId" runat="server" Text="ParentId"   />
                </td>
                <td>
                    <asp:TextBox ID="txtParentId" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMenuTitle" runat="server" Text="Title"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="This field must be filled!" ControlToValidate="txtTitle" ValidationGroup="saveMenu" Display="Dynamic" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNavigateUrl" runat="server" Text="NavigateUrl"   />
                </td>
                <td>
                    <asp:TextBox ID="txtNavigateUrl" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblIsActive" runat="server" Text="IsActive"   />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="False" Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDisplayOrder" runat="server" Text="DisplayOrder"   />
                </td>
                <td>
                    <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" OnClick="btnAdd_Click" ValidationGroup="saveMenu" />
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="button" OnClick="btnEdit_Click" ValidationGroup="saveMenu" />
                </td>
                <td>
                    <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="button" OnClick="btnRemove_Click" />
                </td>
                <td></td>
            </tr>
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="labelSuccess" Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="labelWarning" Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblError" runat="server" Text="Error" CssClass="labelError" Visible="false" />
                </td>
            </tr>
        </table>
    </div>

    <br />
    <div class="tabela" style="overflow: auto">
        <asp:GridView ID="gvMenu" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsMenu" OnPageIndexChanging="gvMenu_PageIndexChanging">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="ParentId" HeaderText="ParentId" SortExpression="ParentId" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="NavigateUrl" HeaderText="NavigateUrl" SortExpression="NavigateUrl" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="DisplayOrder" HeaderText="DisplayOrder" SortExpression="DisplayOrder" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsMenu" runat="server" EnablePaging="true" SelectMethod="GetPagedMenuList" TypeName="GIIS.DataLayer.Menu" SelectCountMethod="GetCountMenuList">
            <SelectParameters>
                <asp:Parameter DefaultValue="10" Name="maximumRows" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="startRowIndex" Type="Int32" />
                <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />


            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

