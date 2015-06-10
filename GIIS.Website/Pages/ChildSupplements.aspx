<%@ Page Title="ChildSupplements" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ChildSupplements.aspx.cs" Inherits="_ChildSupplements" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li class="active">Child</li>
                <li class="active">Child Supplements</li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
             </div> 
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
            <table class="table table-striped table-bordered table-hover table-responsive">
                <tr>
                    <td>
                        <asp:Label ID="lblSupplement" runat="server" Text="Supplement" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text="Date" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblGiven" runat="server" Text="Given" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVitA" runat="server" Text="VitA"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblVitADate" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkVitA" Text="Yes" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMebendezol" runat="server" Text="Mebendezol"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMebendezolDate" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkMebendezol" runat="server" Text="Yes" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveChildSupplements" />
        </div>
        </div> 
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" CssClass="label label-success" Font-Size="Small" Text="Success" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" CssClass="label label-warning" Font-Size="Small" Text="Warning" Visible="false" />
            <asp:Label ID="lblError" runat="server" CssClass="label label-error" Font-Size="Small" Text="Error" Visible="false" />
        </div>
    </div>

    <br />
    <div class="row">
         <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
             </div> 
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
            <asp:GridView ID="gvChildSupplements" Visible="False" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsChildSupplements" OnPageIndexChanging="gvChildSupplements_PageIndexChanging" EnableModelValidation="True">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="ChildId" HeaderText="ChildId" SortExpression="ChildId" Visible="False"/>
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:dd-MMM-yyyy}"/>
                    <asp:CheckBoxField DataField="Vita" HeaderText="Vita" SortExpression="Vita" />
                    <asp:CheckBoxField DataField="Mebendezol" HeaderText="Mebendezol" SortExpression="Mebendezol" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsChildSupplements" runat="server"
                SelectMethod="GetChildSupplementsByChild" TypeName="GIIS.DataLayer.ChildSupplements"  >
                <SelectParameters>
                    <asp:QueryStringParameter DefaultValue="" Name="childId" QueryStringField="id" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

