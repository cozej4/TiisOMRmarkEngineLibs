<%@ Page Title="ItemLot" Language="C#" AutoEventWireup="true" CodeFile="ItemLotNewPopup.aspx.cs" Inherits="Pages_ItemLotNew" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Immunization Information System</title>
    <!-- Bootstrap -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="../css/material-wfont.min.css" rel="stylesheet" />
    <link href="../css/nprogress.css" rel="stylesheet" />
    <link href="../css/ripples.css" rel="stylesheet" />
    <link href="../css/material.css" rel="stylesheet" />
    <style type="text/css">
        .ajax__calendar_container {
            z-index: 1000;
        }
    </style>

    <script type="text/javascript" src="../js/AutoCompletescript.js"></script>

    <%-- <script type="text/javascript" src="../js/AutoCompleteCheck.js"></script>--%>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->


</head>
<body>
    <form id="aspnetform" runat="server">
        <h2>New Item Lot</h2>
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <!-- container -->
        <div class="container">
            <div class="form-group">
                <div class="row">
                    <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
                    <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                        <asp:Label ID="lblItemCategory" runat="server" Text="Item Category" />
                        <span style="color: Red">*</span>
                    </div>
                    <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                        <div class="form-group">
                            <asp:DropDownList ID="ddlItemCategory" runat="server" AutoPostBack="True" DataSourceID="odsItemCategory" DataTextField="Name" DataValueField="Id" />
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
                            <asp:DropDownList ID="ddlItem" runat="server" DataSourceID="odsItem" DataTextField="Code" DataValueField="Id" AutoPostBack="true" />
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
                            <asp:DropDownList ID="ddlGtin" runat="server" DataSourceID="odsGtin" DataTextField="Gtin" DataValueField="Gtin" AutoPostBack="true" />
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
                            <asp:TextBox ID="txtLotNumber" runat="server" />
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
                            <asp:TextBox ID="txtExpireDate" runat="server" />
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
                            <asp:TextBox ID="txtNotes" runat="server"  />
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
                        <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn-sm btn btn-primary btn-raised"
                            ValidationGroup="saveItemLotNew" OnClick="btnEdit_Click" />
                    </div>
                    <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
                    <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                        <%--<asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-raised btn-warning" OnClick="btnRemove_Click" />--%>
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
            </div>

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
        </div>

    </form>
</body>
</html>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>--%>
