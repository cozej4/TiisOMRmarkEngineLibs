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
<%@ Page Title="Vaccination Activity Report" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="VaccinationActivityReport.aspx.cs" Inherits="Pages_VaccinationActivityReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Vaccination Activity Report" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblYear" runat="server" Text="Year"></asp:Label>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-----" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                    <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                    <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                    <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                    <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                    <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                    <asp:ListItem Text="2021" Value="2021"></asp:ListItem>
                    <asp:ListItem Text="2022" Value="2022"></asp:ListItem>
                    <asp:ListItem Text="2023" Value="2023"></asp:ListItem>
                    <asp:ListItem Text="2024" Value="2024"></asp:ListItem>
                    <asp:ListItem Text="2025" Value="2025"></asp:ListItem>
                    <asp:ListItem Text="2026" Value="2026"></asp:ListItem>
                    <asp:ListItem Text="2027" Value="2027"></asp:ListItem>
                    <asp:ListItem Text="2028" Value="2028"></asp:ListItem>
                    <asp:ListItem Text="2029" Value="2029"></asp:ListItem>
                    <asp:ListItem Text="2030" Value="2030"></asp:ListItem>
                    <asp:ListItem Text="2031" Value="2032"></asp:ListItem>
                    <asp:ListItem Text="2033" Value="2033"></asp:ListItem>
                    <asp:ListItem Text="2034" Value="2034"></asp:ListItem>
                    <asp:ListItem Text="2035" Value="2035"></asp:ListItem>
                    <asp:ListItem Text="2036" Value="2036"></asp:ListItem>
                    <asp:ListItem Text="2037" Value="2037"></asp:ListItem>
                    <asp:ListItem Text="2038" Value="2038"></asp:ListItem>
                    <asp:ListItem Text="2039" Value="2039"></asp:ListItem>
                    <asp:ListItem Text="2040" Value="2040"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix"  style="overflow: auto">
            <asp:GridView ID="gvReport" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBound="gvReport_DataBound">
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" CssClass="btn btn-material-bluegrey btn-raised" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnExcel" runat="server" Visible="false" Text="Excel" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>

    </div>
     <div class="row" style="overflow: auto">
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBound="gvReport_DataBound" Visible="false">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
        </asp:GridView>
    </div>
    <br />
    <br />
    <div class="row">
    <%--  <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>--%>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
              <asp:Label ID="lblItem" runat="server" Visible="false" Text="Item"></asp:Label>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                   <asp:ObjectDataSource ID="odsItem" runat="server" SelectMethod="GetVaccinesList"
                    TypeName="GIIS.DataLayer.Item"></asp:ObjectDataSource>
                <asp:DropDownList ID="ddlItem" runat="server" Visible="False" AutoPostBack="True" CssClass="form-control" DataSourceID="odsItem"
                    DataTextField="Code" DataValueField="Id" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
    </div>
   
    <br />
    <asp:Literal ID="iActivityReport" runat="server" />
</asp:Content>
