<%@ Page Title="Balance by Lot" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="HealthFacilityBalanceByLot.aspx.cs" EnableEventValidation="false" Inherits="Pages_HealthFacilityBalanceByLot" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>
        <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Stock</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Health Facility Balance" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <asp:Label ID="lblHealthfacility" runat="server" Text="" Font-Bold="true" Visible="false" />
        </div>
         <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" />
                <asp:RegularExpressionValidator ID="revDate" runat="server" ControlToValidate="txtDate" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
            </div>
        </div>
    </div>
    <br />
  
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow: auto">
            <asp:GridView ID="gvHealthFacilityBalance" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsHealthFacilityBalance" OnDataBound="gvHealthFacilityBalance_DataBound" OnRowDataBound="gvHealthFacilityBalance_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />

                    <asp:BoundField DataField="Gtin" HeaderText="GTIN" SortExpression="Gtin" />
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <%#Eval("GtinObject.ItemObject.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LotNumber" HeaderText="Lot Number" SortExpression="LotNumber" />
                    <asp:TemplateField HeaderText="Expire Date">
                        <ItemTemplate>
                            <asp:Label ID="lblExpireDate" runat="server"></asp:Label>
                            <%--   <%#Eval("ItemLot.ExpireDate")%>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
<%--                    <asp:BoundField DataField="Received" HeaderText="Received" SortExpression="Received" Visible="false" />
                    <asp:BoundField DataField="Distributed" HeaderText="Distributed" SortExpression="Distributed" Visible="false" />
                    <asp:BoundField DataField="Used" HeaderText="Used" SortExpression="Used" />--%>

                    <asp:TemplateField HeaderText="AMC">
                        <ItemTemplate>
                            <%#Math.Round((double)Eval("AvgMonthlyConsumption"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Safety Stock">
                        <ItemTemplate>
                            <%#Eval("GtinStockPolicy.SafetyStock")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Wastage Rate" SortExpression="Wasted">
                        <ItemTemplate>
                            <%#(((double)Eval("Received")) == 0 ? 100 : Math.Round(100 - (((double)Eval("Used") / ((double)Eval("Received"))) * 100))) %>%
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" />
                     <asp:TemplateField HeaderText="Days of Inventory">
                        <ItemTemplate>
                             <%#Math.Round( (double)Eval("Balance") / ((double)Eval("AvgMonthlyConsumption") / (double)20) )%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Base UOM">
                        <ItemTemplate>
                            <%#Eval("GtinObject.BaseUom")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" runat="server" Width="50px" onkeypress="return isNumber(event)"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsHealthFacilityBalance" runat="server" SelectMethod="GetHealthFacilityBalanceByHealthFacility" TypeName="GIIS.DataLayer.HealthFacilityBalance">
                <SelectParameters>
                    <asp:Parameter Name="id" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="There are no records" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />

        </div>
    </div>
    <div class="row">
        <%--   <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>--%>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnExcel" runat="server" Visible="false" Text="Excel" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" CssClass="btn btn-material-bluegrey btn-raised" />
        </div>


        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Button ID="btnUpdateAndOrder" runat="server" Text="Update Balance &amp; Create Order" CssClass="btn btn-primary btn-raised" Visible="false" OnClick="btnUpdateAndOrder_Click"/>
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnUpdateBalance" runat="server" Text="Update Balance" OnClick="btnUpdateBalance_Click" CssClass="btn btn-primary btn-raised" />
        </div>
    </div>
</asp:Content>

