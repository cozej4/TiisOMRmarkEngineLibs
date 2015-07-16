<%@ Page Title="RIVO Receipts" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="RivoReceipt.aspx.cs" Inherits="Pages_Adjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">

        function openItemLotWindow() {
            var popup = window.open('/Pages/ItemLotNewPopup.aspx', 'itemLot', 'width=800,height=600,status=0,toolbar=0');
            var beforeunload = function (e) {

                document.location.reload();
                //(e || window.event).returnValue = confirmationMessage; //Gecko + IE
                //return confirmationMessage;                            //Webkit, Safari, Chrome
                popup.onbeforeunload = beforeunload;
            };
            popup.onbeforeunload= beforeunload;
        }
        function cvAdjustment_Validate(sender, args) {
            var text = (document.getElementById('<%=ddlItems.ClientID%>').value == '-1' || document.getElementById('<%=ddlGtin.ClientID%>').value == '-1'
                     || document.getElementById('<%=ddlItemLot.ClientID%>').value == '-1' || document.getElementById('<%=txtDate.ClientID%>').value == ''
                     || document.getElementById('<%=txtQuantity.ClientID%>').value == '');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
    </script>
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
        .ajax__calendar_container {
            z-index: 1000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Stock</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="RIVO Receipt" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHealthFacilityId" runat="server" Text="Health Facility" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlHealthFacility" runat="server" CssClass="form-control" DataSourceID="odsHealthF" DataTextField="Name" DataValueField="Code">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsHealthF" runat="server" SelectMethod="GetHealthFacilityByParentIdForList" TypeName="GIIS.DataLayer.HealthFacility">
                    <SelectParameters>
                        <asp:Parameter Name="i" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblDate" runat="server" Text="Date" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemCategoryId" runat="server" Text="ItemCategoryId" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control" DataSourceID="odsItemCategory" DataTextField="Name" DataValueField="Id" AutoPostBack="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsItemCategory" runat="server" SelectMethod="GetItemCategoryForList" TypeName="GIIS.DataLayer.ItemCategory"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemId" runat="server" Text="Item" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItems" runat="server" CssClass="form-control" DataSourceID="odsItem" DataTextField="Code" DataValueField="Id" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsItem" runat="server" SelectMethod="GetItemByItemCategoryId" TypeName="GIIS.DataLayer.Item">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlItemCategory" Name="i" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblGtin" runat="server" Text="GTIN" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlGtin" runat="server" CssClass="form-control" DataSourceID="odsGtin" DataTextField="GTIN" DataValueField="GTIN" AutoPostBack="true" OnSelectedIndexChanged="ddlGtin_SelectedIndexChanged" />
                <asp:ObjectDataSource ID="odsGtin" runat="server" SelectMethod="GetItemManufacturerByItemId" TypeName="GIIS.DataLayer.ItemManufacturer">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlItems" Name="itemId" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemLot" runat="server" Text="Item Lot" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItemLot" runat="server" CssClass="form-control" DataTextField="LOT_NUMBER" DataValueField="LOT_NUMBER" />
                <asp:ObjectDataSource ID="odsItemLot" runat="server" SelectMethod="GetItemLots" TypeName="GIIS.DataLayer.ItemLot">
                    <SelectParameters>
                        <asp:Parameter Name="gtin" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblQuantity" runat="server" Text="Quantity" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" />
            </div>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <a href="#" onclick="openItemLotWindow()" class="btn btn-success btn-sm">New Lot...</a>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblUom" runat="server" Text="UOM" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlUom" CssClass="form-control" DataSourceID="odsUOM" runat="server" DataTextField="Name" DataValueField="Name"></asp:DropDownList>
                <asp:ObjectDataSource ID="odsUOM" runat="server" SelectMethod="GetUomFromGtin" TypeName="GIIS.DataLayer.ItemManufacturer">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlGtin" Name="gtin" PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
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
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblLotInfo" runat="server" Text="Item Lot Balance: " Visible="false"></asp:Label>
            <asp:Label ID="lblLotInfoQty" runat="server" Text="" Visible="false"></asp:Label>
            <asp:CustomValidator ID="cvAdjustment" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvAdjustment_Validate" Display="Dynamic" ValidationGroup="saveWastedItems" CssClass="label label-warning" Font-Size="Small" ForeColor="White"></asp:CustomValidator>
            <asp:RegularExpressionValidator ID="revQuantity" runat="server" ControlToValidate="txtQuantity"
                ErrorMessage="The quantity is not valid!" ValidationGroup="saveWastedItems" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationExpression="^[0-9]{1,10}$"> </asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="revDate" runat="server" ControlToValidate="txtDate" ValidationGroup="saveWastedItems" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />

        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveWastedItems" />
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" ValidationGroup="saveWastedItems" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
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
            <asp:GridView ID="gvTransactionLines" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsTransactionLines" OnPageIndexChanging="gvTransactionLines_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <%--   <asp:TemplateField HeaderText="Gtin">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlGtin" runat="server" NavigateUrl='<%# "Adjustment.aspx?id=" + Eval("Id","{0}") %>'
                                Text='<%# Eval("Gtin", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="Gtin" HeaderText="Gtin" SortExpression="Gtin" />
                    <asp:BoundField DataField="GtinLot" HeaderText="Lot Number" SortExpression="GtinLot" />
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <%#Eval("GtinObject.ItemObject.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TransactionDate" HeaderText="Date" SortExpression="TransactionDate" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="TransactionQtyInBaseUom" HeaderText="Quantity" SortExpression="TransactionQtyInBaseUom" />
                    <asp:TemplateField HeaderText="Wastage Reason">
                        <ItemTemplate>
                            <%#Eval("AdjustmentObject.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsTransactionLines" runat="server" SelectMethod="GetItemTransactionById" TypeName="GIIS.DataLayer.ItemTransaction">
                <SelectParameters>
                    <asp:Parameter Name="i" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>

</asp:Content>
