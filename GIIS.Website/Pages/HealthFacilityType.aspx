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
<%@ Page Title="HealthFacilityType" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="HealthFacilityType.aspx.cs" Inherits="Pages_HealthFacilityType" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                 <li><a href="#">Setup</a></li>
                <li class="active"> <asp:Label ID="lblTitle" runat="server" Text="Health Facility Type"/> </li>
            </ol>
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
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name must be filled!" ControlToValidate="txtName" ValidationGroup="saveHealthFacilityType" Display="Dynamic" ForeColor="Red" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblCode" runat="server" Text="Code" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
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
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblIsActive" runat="server" Text="IsActive" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveHealthFacilityType" />
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" ValidationGroup="saveHealthFacilityType" />
        </div>
      <%--  <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>--%>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-raised btn-warning" OnClick="btnRemove_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" CssClass="label label-success" Font-Size="Small" Text="Success" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" CssClass="label label-warning" Font-Size="Small" Text="Warning" Visible="false" />
            <asp:Label ID="lblError" runat="server" CssClass="label label-error" Font-Size="Small" Text="Error" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvHealthFacilityType" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" 
                AllowPaging="True" DataSourceID="odsHealthFacilityType" OnPageIndexChanging="gvHealthFacilityType_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "HealthFacilityType.aspx?id={0}") %>'
                                Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="Active" SortExpression="IsActive" />      
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />              
                    <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" SortExpression="ModifiedBy" Visible="false"/>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsHealthFacilityType" runat="server" EnablePaging="true" TypeName="GIIS.DataLayer.HealthFacilityType"
                SelectMethod="GetPagedHealthFacilityTypeList"  
                SelectCountMethod="GetCountHealthFacilityTypeList">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
