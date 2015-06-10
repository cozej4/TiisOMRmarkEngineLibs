<%@ Page Title="Role Action" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="RoleAction.aspx.cs" Inherits="_RoleAction" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Setup</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Role Action" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
              <asp:Label ID="lblRole" runat="server" Text="Choose Role" Font-Bold="true"></asp:Label>
            <br />
            <div class="form-group">
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAction" runat="server" Text="Choose Actions" Font-Bold="true"></asp:Label>
            <br />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <div class="form-group">
                <asp:ListBox ID="lbActions" runat="server" Height="400px"
                    CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
            </div>
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <br /><br />
            <asp:Button ID="btnAddAll" runat="server" Text=">>" CssClass="btn btn-primary btn-raised" Width="70px" Font-Bold="true" OnClick="btnAddAll_Click" /><br />
            <br />
            <asp:Button ID="btnAdd" runat="server" Text=">" CssClass="btn btn-primary btn-raised" Width="70px" Font-Bold="true" OnClick="btnAdd_Click" /><br />
            <br />
            <asp:Button ID="btnRemove" runat="server" Text="<" CssClass="btn btn-raised btn-warning" Width="70px" Font-Bold="true" OnClick="btnRemove_Click" /><br />
            <br />
            <asp:Button ID="btnRemoveAll" runat="server" Text="<<" CssClass="btn btn-raised btn-warning" Width="70px" Font-Bold="true" OnClick="btnRemoveAll_Click" /><br />

        </div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <div class="form-group">  <asp:ListBox ID="lbRoleActions" runat="server"  Height="400px"
            CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
            </div>
        </div>
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

