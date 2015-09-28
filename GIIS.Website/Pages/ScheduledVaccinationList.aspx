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
<%@ Page Title="Scheduled Vaccinationt List" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ScheduledVaccinationList.aspx.cs" Inherits="Pages_ScheduledVaccinationList" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblName" runat="server" Text="Name" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblCode" runat="server" Text="Code" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no scheduled vaccinations that match your search criteria!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btn btn-material-bluegrey btn-raised btn-sm" Visible="false"  OnClick="btnAddNew_Click" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvScheduledVaccination" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnDataBound="gvScheduledVaccination_DataBound" OnRowDataBound="gvScheduledVaccination_RowDatabound" OnPageIndexChanging="gvScheduledVaccination_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "ScheduledVaccination.aspx?id={0}") %>'
                                Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />

                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <%#Eval("Item.Code")%>
                        </ItemTemplate>
                    </asp:TemplateField>
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
            <asp:ObjectDataSource ID="odsScheduledVaccination" runat="server" EnablePaging="true" SelectMethod="GetPagedScheduledVaccinationList" TypeName="GIIS.DataLayer.ScheduledVaccination" SelectCountMethod="GetCountScheduledVaccinationList">
                <SelectParameters>
                    <asp:Parameter Name="name" Type="String" />
                    <asp:Parameter Name="code" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnExcel" runat="server" Text="Excel" Visible="false" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
    </div>
    <div class="row" style="overflow: auto">
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvScheduledVaccination_DataBound" Visible="false">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("Item.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
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
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


