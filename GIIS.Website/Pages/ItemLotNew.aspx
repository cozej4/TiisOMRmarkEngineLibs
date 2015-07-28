<%@ Page Title="ItemLot" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemLotNew.aspx.cs" Inherits="Pages_ItemLotNew" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                  <li><a href="#">Stock</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Item Lot" />
                </li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemCategory" runat="server" Text="Item Category" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control" AutoPostBack="True" DataSourceID="odsItemCategory" DataTextField="Name" DataValueField="Id" />
                <asp:ObjectDataSource ID="odsItemCategory" runat="server" SelectMethod="GetItemCategoryForList" TypeName="GIIS.DataLayer.ItemCategory"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItem" runat="server" Text="Item" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" DataSourceID="odsItem" DataTextField="Code" DataValueField="Id" AutoPostBack="true" />
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
            <asp:Label ID="lblGTIN" runat="server" Text="GTIN" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlGtin" runat="server" CssClass="form-control" DataSourceID="odsGtin" DataTextField="Gtin" DataValueField="Gtin" AutoPostBack="true" />
                <asp:ObjectDataSource ID="odsGtin" runat="server" SelectMethod="GetGtinByItemId" TypeName="GIIS.DataLayer.ItemManufacturer">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlItem" Name="itemId" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblLotNumber" runat="server" Text="Lot Number" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtLotNumber" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblExpireDate" runat="server" Text="Expire Date" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtExpireDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceExpireDate" runat="server" TargetControlID="txtExpireDate" />
                <asp:RegularExpressionValidator ID="revExpireDate" runat="server" ControlToValidate="txtExpireDate"
                    ValidationGroup="saveItemLotNew" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblIsActive" runat="server" Text="Is Active" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:CustomValidator ID="cvGtinLotNumber" runat="server" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic"
                ErrorMessage="This GTIN-Lot Number combination alredy exists!"
                OnServerValidate="cvGtinLotNumber_ServerValidate" ValidationGroup="saveItemLotNew"></asp:CustomValidator>
            <asp:CustomValidator ID="cvDate" runat="server" ErrorMessage="Expire Date cannot be before today!" ForeColor="White" OnServerValidate="ValidateDate" CssClass="label label-warning" Font-Size="Small" Display="Dynamic" ValidationGroup="saveItemLotNew"></asp:CustomValidator>

        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <%--<asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveItemLotNew" />--%>
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-raised"
                ValidationGroup="saveItemLotNew" OnClick="btnEdit_Click" />
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
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
            <asp:CustomValidator ID="cvItemLotNew" runat="server" ErrorMessage="All fields marked with * must be filled!"
                ClientValidationFunction="cvItemLotNew_Validate" Display="Dynamic" CssClass="label label-warning" ForeColor="white" Font-Size="Small" ValidationGroup="saveItemLotNew"></asp:CustomValidator>
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvItemLotNew" runat="server" DataSourceID="odsItemLotNew" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive">
                <Columns>
                    <%-- Fields are: GTIN, lot no, ITEM , expire date, note --%>
                    <asp:TemplateField HeaderText="GTIN">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="GTIN"
                                NavigateUrl='<%# String.Format("ItemLotNew.aspx?gtin={0}&lotno={1}", Eval("Gtin"), Eval("LotNumber")) %>'
                                Text='<%# Eval("Gtin", "{0}") %>'
                                ToolTip='<%# Eval("Notes", "{0}") %>'>                                 
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lot Number">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="LotNumber"
                                NavigateUrl='<%# String.Format("ItemLotNew.aspx?gtin={0}&lotno={1}", Eval("Gtin"), Eval("LotNumber")) %>'
                                Text='<%# Eval("LotNumber", "{0}") %>'
                                ToolTip='<%# Eval("Notes", "{0}") %>'>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <%#Eval("ItemObject.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ExpireDate" HeaderText="ExpireDate" SortExpression="ExpireDate" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="Is Active" SortExpression="IsActive" />

                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsItemLotNew" runat="server" SelectMethod="GetItemLotByGtinAndLotNo" TypeName="GIIS.DataLayer.ItemLot">
                <SelectParameters>
                    <asp:Parameter Name="gtin" Type="String" />
                    <asp:Parameter Name="lotNumber" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvItemLotNew_Validate(sender, args) {
            var text = (document.getElementById('<%=ddlItemCategory.ClientID%>').value == '-1') ||
                        (document.getElementById('<%=ddlItem.ClientID%>').value == '-1') ||
                        (document.getElementById('<%=ddlGtin.ClientID%>').value == '-1') ||
                        (document.getElementById('<%=txtLotNumber.ClientID%>').value == '') ||
                        (document.getElementById('<%=txtExpireDate.ClientID%>').value == '');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
    </script>
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
</asp:Content>
