<%@ Page Title="Health Facility Balance" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="HealthFacilityBalance.aspx.cs" Inherits="_HealthFacilityBalance" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Reports</a></li>
                 <li><a href="#">Stock Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Health Facility Balance" /></li>
            </ol>
        </div>
    </div>
     <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no records!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>
   
    <br />
     <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
        <asp:GridView ID="gvHealthFacilityBalance" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsHealthFacilityBalance" OnDataBound="gvHealthFacilityBalance_DataBound">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("Item.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Received" HeaderText="Received" SortExpression="Received" />
                <asp:BoundField DataField="Distributed" HeaderText="Distributed" SortExpression="Distributed" />
                <asp:BoundField DataField="Used" HeaderText="Used" SortExpression="Used" />
                <asp:BoundField DataField="Wasted" HeaderText="Wasted" SortExpression="Wasted" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsHealthFacilityBalance" runat="server" SelectMethod="GetCurrentStockBalance" TypeName="GIIS.DataLayer.HealthFacilityBalance">
            <SelectParameters>
                <asp:Parameter Name="hfId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
         </div> 
    <br />
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
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
    </script>
</asp:Content>

