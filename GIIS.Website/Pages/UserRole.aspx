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
<%@ Page Title="User Role" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="UserRole.aspx.cs" Inherits="_UserRole" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="title">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="User Role" /></h1>
    </div>
    <br />
    <div style="overflow: auto">
        <table style="width: 100%">
            <tr>
                <td style="width: 260px">
                    <b>
                    <asp:Label ID="lblUser" runat="server" Text="Choose User"   />
                    <br />
                    <br />
                    <asp:DropDownList ID="ddlUser" runat="server" CssClass="form-control" AutoPostBack="True" Width="250px" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
            <td>&nbsp;
                </td>

            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Label ID="lblRole" runat="server" Text="Choose Roles" Font-Bold="true"></asp:Label>
                    <br />
                    <br />
                    <asp:ListBox ID="lbRoles" runat="server" Width="250px" Height="400px"
                        CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                </td>
                <td style="text-align: left">
                    <asp:Button ID="btnAddAll" runat="server" Text=">>" CssClass="button" Width="70px" Font-Bold="true" OnClick="btnAddAll_Click" /><br />
                    <br />
                    <asp:Button ID="btnAdd" runat="server" Text=">" CssClass="button" Width="70px" Font-Bold="true" OnClick="btnAdd_Click" /><br />
                    <br />
                    <asp:Button ID="btnRemove" runat="server" Text="<" CssClass="button" Width="70px" Font-Bold="true" OnClick="btnRemove_Click" /><br />
                    <br />
                    <asp:Button ID="btnRemoveAll" runat="server" Text="<<" CssClass="button" Width="70px" Font-Bold="true" OnClick="btnRemoveAll_Click" /><br />
                </td>
                <td>&nbsp;
                </td>
                <td style="vertical-align: bottom">
                    <asp:ListBox ID="lbUserRoles" runat="server" Width="250px" Height="400px"
                        CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
                </td>
            </tr>

        </table>
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

