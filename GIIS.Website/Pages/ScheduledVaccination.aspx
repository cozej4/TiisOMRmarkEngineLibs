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
<%@ Page Title="Scheduled Vaccination" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ScheduledVaccination.aspx.cs" Inherits="_ScheduledVaccination" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Vaccination Schedule</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Scheduled Vaccinations" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblItemId" runat="server" Text="ItemId" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-control"  DataSourceID="odsItem" DataTextField="Code" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" Enabled="false" ></asp:DropDownList>
                <asp:ObjectDataSource ID="odsItem" runat="server" SelectMethod="GetVaccinesList" TypeName="GIIS.DataLayer.Item"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblDeseases" runat="server" Text="Deseases" Enabled="false" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtDeseases" runat="server" CssClass="form-control" />
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
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Enabled="false" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblCode" runat="server" Text="Code" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Enabled="false"/>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblEntryDate" runat="server" Text="EntryDate" Enabled="false" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtEntryDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceEntryDate" runat="server" TargetControlID="txtEntryDate" />
                <asp:RegularExpressionValidator ID="revEntryDate" runat="server" ControlToValidate="txtEntryDate" ValidationGroup="saveScheduledVaccination" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblExitDate" runat="server" Text="ExitDate" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtExitDate" runat="server" CssClass="form-control" Enabled="false"/>
                <ajaxToolkit:CalendarExtender ID="ceExitDate" runat="server" TargetControlID="txtExitDate" />
                <asp:RegularExpressionValidator ID="revExitDate" runat="server" ControlToValidate="txtExitDate" ValidationGroup="saveScheduledVaccination" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
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
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" Enabled="false" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblIsActive" runat="server" Text="IsActive" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal" Enabled="false">
                    <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
<%--            <asp:CompareValidator ID="cvItem" runat="server" ControlToValidate="ddlItem" ErrorMessage="Please select an item!" ValidationGroup="saveScheduledVaccination" Operator="NotEqual" CssClass="labelError" ValueToCompare="-1"></asp:CompareValidator>--%>
            <asp:CustomValidator ID="cvScheduledVaccination" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvScheduledVaccination_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveScheduledVaccination"></asp:CustomValidator>
            <asp:CustomValidator ID="cvDate" runat="server" ErrorMessage="First date cannot be after second date!" OnServerValidate="ValidateDates" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveScheduledVaccination" ValidateEmptyText="false"></asp:CustomValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" Visible="false" OnClick="btnAdd_Click" ValidationGroup="saveScheduledVaccination" />
             <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" Visible="false"  OnClick="btnEdit_Click" ValidationGroup="saveScheduledVaccination" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
           <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-raised btn-warning" Visible="false" OnClick="btnRemove_Click" />
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
        <asp:GridView ID="gvScheduledVaccination" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive"  OnRowDataBound="gvScheduledVaccination_RowDatabound" DataSourceID="odsScheduledVaccination">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "ScheduledVaccination.aspx?id={0}") %>'
                            Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                <%--   <asp:BoundField DataField="Hl7VaccineId" HeaderText="Hl7VaccineId" SortExpression="Hl7VaccineId" />--%>

                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("Item.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--  <asp:BoundField DataField="ItemId" HeaderText="ItemId" SortExpression="ItemId" />--%>
                <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="ExitDate" HeaderText="ExitDate" SortExpression="ExitDate" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" Visible="false" />
                <asp:BoundField DataField="Deseases" HeaderText="Deseases" SortExpression="Deseases" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsScheduledVaccination" runat="server" SelectMethod="GetScheduledVaccinationById" TypeName="GIIS.DataLayer.ScheduledVaccination" >
            <SelectParameters>
                <asp:Parameter Name="i" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div></div> 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
    <script type="text/javaScript">
        function cvScheduledVaccination_Validate(sender, args) {
            var text = (document.getElementById('<%=txtName.ClientID%>').value == '') || (document.getElementById('<%=txtCode.ClientID%>').value == '') || (document.getElementById('<%=txtEntryDate.ClientID%>').value == '') || (document.getElementById('<%=ddlItem.ClientID%>').value == '-1');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
    </script>
</asp:Content>

