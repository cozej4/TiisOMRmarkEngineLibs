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
<%@ Page Title="Defaulter List" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="DefaulterList.aspx.cs" Inherits="Pages_DefaulterList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Immunization</a></li>
                <li><a href="#">Monthly Plan</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Defaulter List" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <br />
            <asp:Label ID="lblName" runat="server" Font-Bold="true" Text="Health Center:" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <br />
                <asp:Label ID="lblHealthCenter" Font-Bold="true" runat="server" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <br />
            <asp:Label ID="lblDomicileId" runat="server" Text="Domicile" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlDomicile" runat="server" DataSourceID="odsDomicile" DataTextField="Name" DataValueField="Id" CssClass="form-control" OnSelectedIndexChanged="ddlDomicile_SelectedIndexChanged" AutoPostBack="True" />
                <asp:ObjectDataSource ID="odsDomicile" runat="server" SelectMethod="GetPlaceListbyHealthFacility" TypeName="GIIS.DataLayer.Place">
                    <SelectParameters>
                        <asp:SessionParameter Name="hfId" SessionField="__healthFacilityId_DefaulterList" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix" style="align-content: center">
            <asp:Label ID="lblActualMonth" Text=" " runat="server" Font-Bold="True"></asp:Label>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <%--            <asp:Button ID="btnSendSMS" runat="server" Text="Send SMS" Visible="false" OnClick="btnSendSMS_Click" CssClass="btn btn-sm btn-material-bluegrey btn-raised" />--%>
        </div>
    </div>
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnExcel" runat="server" Visible="false" Text="Excel" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvVaccinationEvent" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsVaccinationEvent" OnDataBound="gvVaccinationEvent_DataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="Child">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlChild" runat="server" NavigateUrl='<%# Eval("ID", "Child.aspx?id={0}") %>'
                                Text='<%# Eval("NAME", "{0}") %>' Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VACCINES" HeaderText="Vaccines" SortExpression="VACCINES" />
                    <asp:BoundField DataField="SCHEDULED_DATE" HeaderText="Scheduled Date" SortExpression="SCHEDULED_DATE" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="DOMICILE" HeaderText="Village/Domicile" SortExpression="DOMICILE" />
                    <asp:BoundField DataField="DOMICILE_ID" HeaderText="Id" SortExpression="DOMICILE_ID" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsVaccinationEvent" runat="server" SelectMethod="GetDefaultersList" TypeName="GIIS.DataLayer.VaccinationEvent">
                <SelectParameters>
                    <asp:Parameter Name="healthFacilityId" Type="String" />
                    <asp:Parameter Name="domicileId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>    
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsExport" OnDataBound="gvVaccinationEvent_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Child">
                        <ItemTemplate>
                            <asp:Label ID="hlChild" runat="server" Text='<%# Eval("NAME", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VACCINES" HeaderText="Vaccines" SortExpression="VACCINES" />
                    <asp:BoundField DataField="SCHEDULED_DATE" HeaderText="Scheduled Date" SortExpression="SCHEDULED_DATE" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="DOMICILE" HeaderText="Village/Domicile" SortExpression="DOMICILE" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsExport" runat="server" SelectMethod="GetDefaultersList" TypeName="GIIS.DataLayer.VaccinationEvent">
                <SelectParameters>
                    <asp:Parameter Name="healthFacilityId" Type="String" />
                    <asp:Parameter Name="domicileId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>  
</asp:Content>
