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
<%@ Page Title="Actions" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Actions.aspx.cs" Inherits="_Actions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="title">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Actions" /></h1>
    </div>
    <br />
    <div style="overflow: auto">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="Name"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="This field must be filled!" ControlToValidate="txtName" ValidationGroup="saveActions" Display="Dynamic" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNotes" runat="server" Text="Notes"   />
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" OnClick="btnAdd_Click" ValidationGroup="saveActions" />
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="button" OnClick="btnEdit_Click" ValidationGroup="saveActions" />
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
        <asp:GridView ID="gvActions" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsActions" OnPageIndexChanging="gvActions_PageIndexChanging">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsActions" runat="server" EnablePaging="true" SelectMethod="GetPagedActionsList" TypeName="GIIS.DataLayer.Actions" SelectCountMethod="GetCountActionsList">
            <SelectParameters>
                <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

