<%@ Page Title="Item" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Item.aspx.cs" Inherits="_Item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Setup</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Item" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemCategoryId" runat="server" Text="ItemCategoryId" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItemCategory" runat="server" CssClass="form-control" DataSourceID="odsItems" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlItemCategory_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsItems" runat="server" SelectMethod="GetItemCategoryForListAll" TypeName="GIIS.DataLayer.ItemCategory"></asp:ObjectDataSource>

            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHl7VaccineId" runat="server" Text="Hl7VaccineId" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlHl7Vaccine" Enabled="false" runat="server" CssClass="form-control"  DataSourceID="odsHl7Vaccines" DataTextField="Code" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlHl7Vaccine_SelectedIndexChanged"></asp:DropDownList>
                <asp:ObjectDataSource ID="odsHl7Vaccines" runat="server" SelectMethod="GetHl7VaccinesForList" TypeName="GIIS.DataLayer.Hl7Vaccines"></asp:ObjectDataSource>
            </div>
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblCode" runat="server" Text="Code" />
             <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblEntryDate" runat="server" Text="EntryDate" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
               
                <asp:TextBox ID="txtEntryDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceEntryDate" runat="server" TargetControlID="txtEntryDate" />
                <asp:RegularExpressionValidator ID="revEntryDate" runat="server" ControlToValidate="txtEntryDate" ValidationGroup="saveItem" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblExitDate" runat="server" Text="ExitDate" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
              
                <asp:TextBox ID="txtExitDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceExitDate" runat="server" TargetControlID="txtExitDate" />
                <asp:RegularExpressionValidator ID="revExitDate" runat="server" ControlToValidate="txtExitDate" ValidationGroup="saveItem" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"/>
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblIsActive" runat="server" Text="IsActive" Visible ="false"  />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal" Visible ="false">
                    <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
<%--            <asp:CompareValidator ID="cvItemCategory" runat="server" ControlToValidate="ddlItemCategory" ErrorMessage="Please select an item category!" ValidationGroup="saveItem" Operator="NotEqual"  ValueToCompare="-1"></asp:CompareValidator>--%>
            <asp:CustomValidator ID="cvItem" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvItem_Validate" Display="Dynamic" ValidationGroup="saveItem" CssClass="label label-warning" Font-Size="Small" ForeColor="White" ></asp:CustomValidator>
            <%--                    <asp:CustomValidator ID="cvEntrydate" runat="server" ErrorMessage="Entry date cannot be after today!" OnServerValidate="ValidateEntrydate" CssClass="labelError" Display="Dynamic" ValidationGroup="saveItem" ValidateEmptyText="true"></asp:CustomValidator>--%>
            <asp:CustomValidator ID="cvDate" runat="server" ErrorMessage="First date cannot be after second date!" OnServerValidate="ValidateDates" Display="Dynamic" ValidationGroup="saveItem" ValidateEmptyText="false" CssClass="label label-warning" Font-Size="Small" ForeColor="White" ></asp:CustomValidator>
        </div>
    </div>
    <br />
     <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveItem" />
                <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" ValidationGroup="saveItem" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-raised btn-warning" OnClick="btnRemove_Click" Visible ="false" />
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
        <asp:GridView ID="gvItem" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnRowDataBound="gvItem_RowDatabound" DataSourceID="odsItem">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />

                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "Item.aspx?id={0}") %>'
                            Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                <asp:TemplateField HeaderText="Item Category">
                    <ItemTemplate>
                        <%#Eval("ItemCategory.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hl7Vaccine">
                    <ItemTemplate>
                        <%#Eval("Hl7Vaccine.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="ExitDate" HeaderText="ExitDate" SortExpression="ExitDate" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsItem" runat="server" SelectMethod="GetItemById" TypeName="GIIS.DataLayer.Item" >
            <SelectParameters>
                <asp:Parameter Name="i" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div></div> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
     <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
    <script type="text/javaScript">
        function cvItem_Validate(sender, args) {
            var text = (document.getElementById('<%=txtName.ClientID%>').value == '') || (document.getElementById('<%=txtCode.ClientID%>').value == '') || (document.getElementById('<%=txtEntryDate.ClientID%>').value == '') || (document.getElementById('<%=ddlItemCategory.ClientID%>').value == '-1');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
    </script>
</asp:Content>
