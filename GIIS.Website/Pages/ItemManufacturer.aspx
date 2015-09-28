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
<%@ Page Title="ItemManufacturer" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ItemManufacturer.aspx.cs" Inherits="Pages_ItemManufacturer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                  <li><a href="#">Stock</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="GTIN Setup" />
                </li>
            </ol>
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
                <asp:TextBox ID="txtGTIN" runat="server" CssClass="form-control" />
            </div>
        </div>
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
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblManufacturer" runat="server" Text="Manufacturer" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlManufacturer" runat="server" CssClass="form-control" DataSourceID="odsManufacturer"
                    DataTextField="Name" DataValueField="Id" />
                <asp:ObjectDataSource ID="odsManufacturer" runat="server" SelectMethod="GetManufacturerForList"
                    TypeName="GIIS.DataLayer.Manufacturer"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItem" runat="server" Text="Item" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control" DataSourceID="odsItem" DataTextField="Code" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" />
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
            <asp:Label ID="lblBaseUOM" runat="server" Text="Base UOM" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlBaseUOM" runat="server" CssClass="form-control" DataSourceID="odsUOM" DataTextField="Name" DataValueField="Name" />
                <asp:ObjectDataSource ID="odsUOM" runat="server" SelectMethod="GetUomForList" TypeName="GIIS.DataLayer.Uom"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblPrice" runat="server" Text="Price" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAlt1UOM" runat="server" Text="ALT 1 UOM" />
             <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlAlt1UOM" runat="server" CssClass="form-control" DataSourceID="odsAlt1Uom" DataTextField="Name" DataValueField="Name" />
                <asp:ObjectDataSource ID="odsAlt1Uom" runat="server" SelectMethod="GetUomForList" TypeName="GIIS.DataLayer.Uom"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAlt1Qty" runat="server" Text="QTY ALT 1 UOM" />
             <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtAlt1Qty" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAlt2UOM" runat="server" Text="ALT 2 UOM" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlAlt2UOM" runat="server" CssClass="form-control"  DataSourceID="odsAlt2Uom" DataTextField="Name" DataValueField="Name" />
                <asp:ObjectDataSource ID="odsAlt2Uom" runat="server" SelectMethod="GetUomForList" TypeName="GIIS.DataLayer.Uom"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAlt2Qty" runat="server" Text="QTY ALT 2 UOM" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtAlt2Qty" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblGTINParent" runat="server" Text="KIT Items" />
        </div>
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
            <div class="form-group">
                <asp:ListBox SelectionMode="Multiple" ID="ddlGTINParent" runat="server" CssClass="form-control"  DataSourceID="odsGTINParent" DataTextField="GTIN" DataValueField="ID" OnDataBound="ddlGTINParent_DataBound"   AutoPostBack ="true" OnSelectedIndexChanged="ddlGTINParent_SelectedIndexChanged"/>
                <asp:ObjectDataSource ID="odsGTINParent" runat="server" SelectMethod="GetItemManufacturerForList2" TypeName="GIIS.DataLayer.ItemManufacturer" ></asp:ObjectDataSource>
            </div>
        </div>
<%--        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblBaseUOMChild" runat="server" Text="Base UOM Child" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtBaseUOMChild" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
<%--                <asp:DropDownList ID="ddlBaseUOMChild" runat="server" CssClass="form-control" DataSourceID="odsBaseUOMChild" DataTextField="Name" DataValueField="Name" />
                <asp:ObjectDataSource ID="odsBaseUOMChild" runat="server" SelectMethod="GetUomForList" TypeName="GIIS.DataLayer.Uom"></asp:ObjectDataSource>
            </div>
        </div>--%>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblStorageSpace" runat="server" Text="Storage Space" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtStorageSpace" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblNotes" runat="server" Text="Notes" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" Enabled="false" />
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
            
            <asp:CustomValidator ID="cvGtin" runat="server" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" 
                ErrorMessage="This GTIN alredy exists! Please enter another GTIN." 
                ControlToValidate="txtGTIN" OnServerValidate="cvGtin_ServerValidate" ValidationGroup="saveItemManufacturer"></asp:CustomValidator>

            <asp:RegularExpressionValidator ID="revPrice" runat="server" ForeColor="White" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" 
                ControlToValidate="txtPrice" ValidationExpression="^[0-9.]+$" ValidationGroup="saveItemManufacturer" 
                ErrorMessage="Price is not valid! It should be a numeric value!" > </asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="revAlt1Qty" runat="server" ForeColor="White" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" 
                ControlToValidate="txtAlt1Qty" ValidationExpression="^[0-9.]+$" ValidationGroup="saveItemManufacturer" 
                ErrorMessage="ALT 1 QTY is not valid! It should be a numeric value!" > </asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="revAlt2Qty" runat="server" ForeColor="White" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" 
                ControlToValidate="txtAlt2Qty" ValidationExpression="^[0-9.]+$" ValidationGroup="saveItemManufacturer" 
                ErrorMessage="ALT 2 QTY is not valid! It should be a numeric value!" > </asp:RegularExpressionValidator>
<%--            <asp:RegularExpressionValidator ID="revBaseUOMChild" runat="server" ForeColor="White" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" 
                ControlToValidate="txtBaseUOMChild" ValidationExpression="^[0-9.]+$" ValidationGroup="saveItemManufacturer" 
                ErrorMessage="Base UOM Child is not valid! It should be a numeric value!" > </asp:RegularExpressionValidator>--%>
            <asp:RegularExpressionValidator ID="revStorageSpace" runat="server" ForeColor="White" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" 
                ControlToValidate="txtStorageSpace" ValidationExpression="^[0-9.]+$" ValidationGroup="saveItemManufacturer" 
                ErrorMessage="Storage Space is not valid! It should be a numeric value!" > </asp:RegularExpressionValidator>
        </div>
    </div>

    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <%--<asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveItemManufacturer" />--%>
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" 
               ValidationGroup="saveItemManufacturer" OnClick="btnEdit_Click" />
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
            <asp:CustomValidator ID="cvItemManufacturer" runat="server" ErrorMessage="All fields marked with * must be filled!" 
                ClientValidationFunction="cvItemManufacturer_Validate" Display="Dynamic" CssClass="label label-warning" Font-Size="Small" ValidationGroup="saveItemManufacturer" ForeColor="White" ></asp:CustomValidator>
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvItemManufacturer" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsItemManufacturer">
                <Columns>
                    <asp:TemplateField HeaderText="GTIN" SortExpression="Gtin">
                        <ItemTemplate>
                            <asp:HyperLink ID="Gtin" runat="server" NavigateUrl='<%# Eval("Gtin", "ItemManufacturer.aspx?gtin={0}") %>'
                                Text='<%# Eval("Gtin", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <%#Eval("ItemObject.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Manufacturer">
                        <ItemTemplate>
                            <%#Eval("ManufacturerObject.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BaseUom" HeaderText="BaseUom" SortExpression="BaseUom" />
                    <asp:BoundField DataField="Alt1Uom" HeaderText="Alt1Uom" SortExpression="Alt1Uom" />
                    <asp:BoundField DataField="Alt1QtyPer" HeaderText="Alt1QtyPer" SortExpression="Alt1QtyPer" />
                    <asp:BoundField DataField="Alt2Uom" HeaderText="Alt2Uom" SortExpression="Alt2Uom" />
                    <asp:BoundField DataField="Alt2QtyPer" HeaderText="Alt2QtyPer" SortExpression="Alt2QtyPer" />
                    <asp:BoundField DataField="GtinParent" HeaderText="GtinParent" SortExpression="GtinParent" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsItemManufacturer" runat="server" SelectMethod="GetItemManufacturerByGtin"
                TypeName="GIIS.DataLayer.ItemManufacturer">
                 <SelectParameters>
                    <asp:Parameter Name="s" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvItemManufacturer_Validate(sender, args) {
            var text = (document.getElementById('<%=txtGTIN.ClientID%>').value == '') || (document.getElementById('<%=ddlItem.ClientID%>').value == '-1') || (document.getElementById('<%=ddlManufacturer.ClientID%>').value == '-1') || (document.getElementById('<%=ddlBaseUOM.ClientID%>').value == '-1') || (document.getElementById('<%=ddlAlt1UOM.ClientID%>').value == '-1') || (document.getElementById('<%=txtAlt1Qty.ClientID%>').value == '');
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
</asp:Content>
