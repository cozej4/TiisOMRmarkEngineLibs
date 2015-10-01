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
<%@ Page Title="Uom" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Uom.aspx.cs" Inherits="_Uom" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Setup</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Uom" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblName" runat="server" Text="Name" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblNotes" runat="server" Text="Notes" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:Label ID="revUom" runat="server" CssClass="label label-warning" Font-Size="Small" Text=" The Uom you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
                       <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name must be filled!" ControlToValidate="txtName" ValidationGroup="saveUom" Display="Dynamic" CssClass="label label-warning" Font-Size="Small" ForeColor="White" />
            <%-- <asp:CustomValidator ID="cvUom" runat="server" ErrorMessage=" All fields marked with * must be filled!" ClientValidationFunction="cvUom_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveUom"></asp:CustomValidator>--%>
            <asp:RegularExpressionValidator ID="revName" runat="server"
                ErrorMessage=" Name should be at least 2 characters and not contain numbers."
                ControlToValidate="txtName" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveUom"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:\,;@ª^\*<>=&-]{2,50}$" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveUom" />
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" ValidationGroup="saveUom" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">

            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-raised btn-warning" OnClick="btnRemove_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvUom" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive">

                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "Uom.aspx?id={0}") %>'
                                Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

