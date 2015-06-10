<%@ Page Title="TransferOrderDetail" Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/MasterPage.master" EnableEventValidation="false" CodeFile="transferOrderDetail.aspx.cs" Inherits="Pages_TransferOrderDetail" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Stock Issue</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Transfer Order Detail" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderNum" runat="server" Text="Order Num." />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="txtOrderNum" runat="server" Font-Bold="true" />

        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderSchedReplenishDate" runat="server" Text="Replenish Date" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="txtOrderSchedReplenishDate" runat="server" Font-Bold="true" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderFacilityFrom" runat="server" Text="Facility From" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="txtOrderFacilityFrom" runat="server" Font-Bold="true" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderFacilityTo" runat="server" Text="Facility To" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="txtOrderFacilityTo" runat="server" Font-Bold="true" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderStatus" runat="server" Text="Order Status" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="txtOrderStatus" runat="server" Font-Bold="true" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderCarrier" runat="server" Text="Order Carrier" Visible="false" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="txtOrderCarrier" runat="server" Font-Bold="true" Visible="false" />
        </div>

    </div>
    <br />
    <div class="row">

        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnRelease" runat="server" Text="Release" CssClass="btn btn-success btn-raised btn-sm" OnClick="btnRelease_Click" />
        </div>

        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnPack" runat="server" Text="Pack" CssClass="btn btn-info btn-raised btn-sm" OnClick="btnPack_Click" />
        </div>

        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnShip" runat="server" Text="Ship" CssClass="btn btn-material-bluegrey btn-raised btn-sm" OnClick="btnShip_Click" />
        </div>

        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-warning btn-raised btn-sm" OnClick="btnCancel_Click" />
        </div>

        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:HyperLink ID="hlPrintPackingSlip" runat="server" Text="Packing Slip" Visible="false" CssClass="btn btn-success btn-raised btn-sm" />
        </div>

    </div>

    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
            <asp:GridView ID="gvCreateTOD" DataSourceID="odsCreateTOD" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" Visible="false" OnDataBound="gvCreateTOD_DataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <asp:Label ID="lblGtinItem" runat="server" Text='<%#Eval("ITEM")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GTIN">
                        <ItemTemplate>
                            <asp:Label ID="lblGtin" runat="server" Text='<%#Eval("GTIN")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="From Stock Level">
                        <ItemTemplate>
                            <asp:Label ID="lblDistrictStock" runat="server" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UOM">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlUom2" CssClass="form-control" runat="server"  DataTextField="Name" DataValueField="Name"></asp:DropDownList>
                            <asp:ObjectDataSource ID="odsUOM" runat="server" SelectMethod="GetUomFromGtin" TypeName="GIIS.DataLayer.ItemManufacturer">
                            </asp:ObjectDataSource>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsCreateTOD" runat="server" SelectMethod="GetGtinForTransferOrderDetail" TypeName="GIIS.DataLayer.TransferOrderDetail">
                <SelectParameters>
                    <asp:Parameter Name="healthFacilityCode" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvGtinValue" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnRowDataBound="gvGtinValue_RowDataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField HeaderText="OrderNumb" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("OrderNum")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DetailNum" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId2" runat="server" Text='<%#Eval("OrderDetailNum")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <asp:Label ID="lblItem" runat="server" Text='<%#Eval("OrderDetailDescription")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="From Stock Level">
                        <ItemTemplate>
                            <asp:Label ID="lblDistrictStock" runat="server" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="GTIN">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderGtin" runat="server" Text='<%#Eval("OrderGtin")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Lot Number">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlItemLot" runat="server" CssClass="form-control" DataTextField="LOT_NUMBER" DataValueField="LOT_NUMBER" />
                            <asp:ObjectDataSource ID="odsItemLot" runat="server" SelectMethod="GetItemLots" TypeName="GIIS.DataLayer.ItemLot">
                                <SelectParameters>
                                    <asp:Parameter Name="gtin" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValue" runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="UOM">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlUom" CssClass="form-control" runat="server" DataTextField="Name" DataValueField="Name"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsGtinValue" runat="server" SelectMethod="GetTransferOrderDetailByOrderNumAsList" TypeName="GIIS.DataLayer.TransferOrderDetail">
                <SelectParameters>
                    <asp:Parameter Name="orderNumber" Type="Int32" />
                    <asp:Parameter Name="healthFacilityCode" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </div>
    </div>
    <br />
    <div class="row">

        <div class="col-md-11 col-xs-11 col-sm-11 col-lg-11 clearfix">
            <table class="table table-striped table-bordered table-hover table-responsive" id="tbl" runat="server">
                <thead>
                    <tr>
                        <td>GTIN</td>
                        <td>Lot Number</td>
                        <td>Quantity</td>
                        <td>UOM</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <div class="form-group" style="max-width: 200px">
                                <asp:DropDownList ID="ddlGtin" CssClass="form-control" runat="server" DataTextField="GTIN" DataValueField="VALUE" AutoPostBack="True"></asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="form-group" style="min-width: 180px; max-width: 180px">
                                <asp:DropDownList ID="ddlItemLot" runat="server" CssClass="form-control" DataTextField="LOT_NUMBER" DataValueField="LOT_NUMBER" DataSourceID="odsItemLot" />
                                <asp:ObjectDataSource ID="odsItemLot" runat="server" SelectMethod="GetItemLots" TypeName="GIIS.DataLayer.ItemLot">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlGtin" Name="gtin" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </td>
                        <td>
                            <div class="form-group" style="max-width: 100px">
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" />
                            </div>
                        </td>
                        <td>
                            <div class="form-group" style="min-width: 100px">
                                <asp:DropDownList ID="ddlUom" CssClass="form-control" runat="server" DataSourceID="odsUOM" DataTextField="Name" DataValueField="Name"></asp:DropDownList>
                                <asp:ObjectDataSource ID="odsUOM" runat="server" SelectMethod="GetUomFromGtin" TypeName="GIIS.DataLayer.ItemManufacturer">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlGtin" Name="gtin" PropertyName="SelectedValue" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <%--  <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
        </div>--%>

        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix" style="margin-top: 43px">
            <asp:Button ID="btnAddDetail" runat="server" Text="Add" CssClass="btn btn-primary btn-raised btn-sm" OnClick="btnAddDetail_Click" />

        </div>

    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="An Order Detail with same data already exists, please edit on the table instead. " CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />

    <div class="row">

        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" Visible="false" />
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            &nbsp;
        </div>

    </div>






</asp:Content>
