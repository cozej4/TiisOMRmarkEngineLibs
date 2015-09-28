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
<%@ Page Title="GtinHfStockPolicy" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="GtinHfStockPolicy.aspx.cs" Inherits="Pages_GtinHfStockPolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
      
        function cvStockPolicy_Validate(sender, args) {
            var text = (document.getElementById('<%=ddlReorderQty.ClientID%>').value == '') || (document.getElementById('<%=txtSafetyStock.ClientID%>').value == '') || (document.getElementById('<%=ddlHealthFacility.ClientID%>').value == '-1') || (document.getElementById('<%=ddlGtin.ClientID%>').value == '-1');
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
            <asp:Label ID="lblGtin" runat="server" Text="GTIN" />
            <%--<span style="color: Red">*</span> --%>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlGtin" runat="server" CssClass="form-control" DataSourceID="odsGtin" DataTextField="GTIN" DataValueField="ID" AutoPostBack="True" OnSelectedIndexChanged="ddlGtin_SelectedIndexChanged" />
                <asp:ObjectDataSource ID="odsGtin" runat="server" SelectMethod="GetItemManufacturerForList2" TypeName="GIIS.DataLayer.ItemManufacturer"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHealthFacility" runat="server" Text="Health Facility" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <%--<div class="form-group">
                    <uc1:AutoCompleteTextbox runat="server"
                        ID="txtHealthcenterId"
                        OnClickSubmit="true"
                        ServiceMethod="AllHealthFacilities"
                        HighlightResults="true"
                        ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
                </div>--%>
               <asp:DropDownList ID="ddlHealthFacility" runat="server" CssClass="form-control" DataSourceID="odsHealthF" DataTextField="NAME" DataValueField="CODE" AutoPostBack="true" OnSelectedIndexChanged="ddlHealthFacility_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsHealthF" runat="server" SelectMethod="GetHealthFacilityForList" TypeName="GIIS.DataLayer.HealthFacility">
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblReorderQty" runat="server" Text="Reorder QTY" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <%--<asp:TextBox ID="txtReorderQty" runat="server" CssClass="form-control" />--%>
                <asp:DropDownList ID="ddlReorderQty" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblSafetyStock" runat="server" Text="Safety Stock" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtSafetyStock" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" />
            </div>
        </div>

    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblLeadTime" runat="server" Text="Lead Time" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtLeadTime" runat="server" CssClass="form-control" onkeypress="return isNumber(event)" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblConsumptionLogic" runat="server" Text="Consumption Logic" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlConsumptionLogic" runat="server" CssClass="form-control" Enabled ="false">
                    <asp:ListItem Selected="True">FEFO</asp:ListItem>
                    <asp:ListItem>LIFO</asp:ListItem>
                    <asp:ListItem>FIFO</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <%--<asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveGtinHfStockPolicy" />--%>
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveStockPolicy" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:Label ID="lblSuccess" runat="server" CssClass="label label-success" Font-Size="Small" Text="Success" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" CssClass="label label-warning" Font-Size="Small" Text="Warning" Visible="false" />
            <asp:Label ID="lblError" runat="server" CssClass="label label-error" Font-Size="Small" Text="Error" Visible="false" />
            <%--<asp:CustomValidator ID="cvGtinHfStockPolicy" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvGtinHfStockPolicy_Validate" Display="Dynamic" CssClass="label label-warning" Font-Size="Small" ValidationGroup="saveGtinHfStockPolicy"></asp:CustomValidator>--%>
            <asp:CustomValidator ID="cvStockPolicy" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvStockPolicy_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveStockPolicy"></asp:CustomValidator>
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvGtinHfStockPolicy" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive"
                DataSourceID="odsGtinHfStockPolicy" AllowPaging="True" OnPageIndexChanging="gvGtinHfStockPolicy_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:TemplateField HeaderText="GTIN">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="Gtin"
                                NavigateUrl='<%# String.Format("GtinHfStockPolicy.aspx?hfId={0}&gtin={1}", Eval("HealthFacilityCode"), Eval("Gtin")) %>'
                                Text='<%# Eval("Gtin", "{0}") %>'
                                ToolTip='<%# Eval("Notes", "{0}") %>'>                                 
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HealthFacility">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="HealthFacility"
                                NavigateUrl='<%# String.Format("GtinHfStockPolicy.aspx?hfId={0}&gtin={1}", Eval("HealthFacilityCode"), Eval("Gtin")) %>'
                                Text='<%# Eval("HealthFacilityCodeObject.Name", "{0}") %>'
                                ToolTip='<%# Eval("Notes", "{0}") %>'>
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LeadTime" HeaderText="Lead Time" SortExpression="LeadTime" />
                    <asp:BoundField DataField="ReorderQty" HeaderText="Reorder QTY" SortExpression="ReorderQty" />
                    <asp:BoundField DataField="SafetyStock" HeaderText="Safety Stock" SortExpression="SafetyStock" />
                    <asp:BoundField DataField="ConsumptionLogic" HeaderText="Consumption Logic" SortExpression="ConsumptionLogic" />

                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsGtinHfStockPolicy" runat="server" EnablePaging="true" SelectMethod="GetGtinHfStockPolicyList"
                TypeName="GIIS.DataLayer.GtinHfStockPolicy" SelectCountMethod="GetCountGtinHfStockPolicyList"></asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>

