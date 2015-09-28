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
<%@ Page Title="Search Children" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ChildListNewSearch.aspx.cs" Inherits="Pages_ChildListNewSearch" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Child</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Search Children" /></li>
            </ol>
        </div>
    </div>
    <br />
    <div class="row">
      <%--  <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
        </div>--%>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search Again" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click"/>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
        </div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no children that match your search criteria!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
            <asp:GridView ID="gvChild" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" 
                AllowPaging="True" OnDataBound="gvChild_DataBound" OnPageIndexChanging="gvChild_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                    <asp:TemplateField HeaderText="SystemID">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlSystemId" runat="server" NavigateUrl='<%# Eval("Id", "Child.aspx?id={0}") %>'
                                Text='<%# Eval("SystemId", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--  <asp:BoundField DataField="SystemId" HeaderText="SystemId" SortExpression="SystemId" />--%>
                    <asp:BoundField DataField="Firstname1" HeaderText="Firstname1" SortExpression="Firstname1" />
                    <asp:BoundField DataField="Firstname2" HeaderText="Firstname2" SortExpression="Firstname2" />
                    <asp:BoundField DataField="Lastname1" HeaderText="Lastname1" SortExpression="Lastname1" />
                    <asp:BoundField DataField="Lastname2" HeaderText="Lastname2" SortExpression="Lastname2" />
                    <asp:BoundField DataField="Birthdate" HeaderText="Birthdate" SortExpression="Birthdate"  DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:TemplateField HeaderText="Gender">
                        <ItemTemplate>
                            <%# (bool)Eval("Gender") == true? "M" : "F" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- <asp:CheckBoxField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />--%>
                    <asp:TemplateField HeaderText="Health Center">
                        <ItemTemplate>
                            <%#Eval("Healthcenter.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Birthplace">
                        <ItemTemplate>
                            <%#Eval("Birthplace.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Community" Visible ="false">
                        <ItemTemplate>
                            <%#Eval("Community.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Domicile">
                        <ItemTemplate>
                            <%#Eval("Domicile.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%#Eval("Status.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                    <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                    <asp:BoundField DataField="MotherId" HeaderText="MotherId" SortExpression="MotherId" />
                    <asp:BoundField DataField="MotherFirstname" HeaderText="MotherFirstname" SortExpression="MotherFirstname" />
                    <asp:BoundField DataField="MotherLastname" HeaderText="MotherLastname" SortExpression="MotherLastname" />
                    <asp:BoundField DataField="FatherId" HeaderText="FatherId" SortExpression="FatherId" />
                    <asp:BoundField DataField="FatherFirstname" HeaderText="FatherFirstname" SortExpression="FatherFirstname" />
                    <asp:BoundField DataField="FatherLastname" HeaderText="FatherLastname" SortExpression="FatherLastname" />
                    <asp:BoundField DataField="CaretakerId" HeaderText="CaretakerId" SortExpression="CaretakerId" />
                    <asp:BoundField DataField="CaretakerFirstname" HeaderText="CaretakerFirstname" SortExpression="CaretakerFirstname" />
                    <asp:BoundField DataField="CaretakerLastname" HeaderText="CaretakerLastname" SortExpression="CaretakerLastname" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                    <asp:BoundField DataField="IdentificationNo1" HeaderText="IdentificationNo1" SortExpression="IdentificationNo1" />
                    <asp:BoundField DataField="IdentificationNo2" HeaderText="IdentificationNo2" SortExpression="IdentificationNo2" />
                    <asp:BoundField DataField="IdentificationNo3" HeaderText="IdentificationNo3" SortExpression="IdentificationNo3" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsChild" runat="server" EnablePaging="true" SelectMethod="GetPagedChildList" TypeName="GIIS.DataLayer.Child" 
                SelectCountMethod="GetCountChildList">
                <SelectParameters>
                    <asp:Parameter  Name="statusId" Type="Int32" />
                    <asp:Parameter  Name="birthdateFrom" Type="DateTime" />
                    <asp:Parameter  Name="birthdateTo" Type="DateTime" />
                    <asp:Parameter  Name="firstname1" Type="String" />
                    <asp:Parameter  Name="lastname1" Type="String" />
                    <asp:Parameter  Name="motherFirstname" Type="String" />
                    <asp:Parameter  Name="motherLastname" Type="String" />
                    <asp:Parameter  Name="idFields" Type="String" />
                    <asp:Parameter  Name="systemId" Type="String" />
                    <asp:Parameter  Name="barcodeId" Type="String" />
                    <asp:Parameter  Name="tempId" Type="String" />
                    <asp:Parameter  Name="healthFacilityId" Type="String" />
                    <asp:Parameter  Name="birthplaceId" Type="Int32" />
                    <asp:Parameter  Name="communityId" Type="Int32" />
                    <asp:Parameter  Name="domicileId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnExcel" runat="server" Text="Excel" Visible="false" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />

        </div>
    </div>

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvExel" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" Visible="false">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="SystemID">
                        <ItemTemplate>
                            <%#Eval("Id", "Child.aspx?id={0}") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Firstname1" HeaderText="Firstname1" SortExpression="Firstname1" />
                    <asp:BoundField DataField="Firstname2" HeaderText="Firstname2" SortExpression="Firstname2" />
                    <asp:BoundField DataField="Lastname1" HeaderText="Lastname1" SortExpression="Lastname1" />
                    <asp:BoundField DataField="Lastname2" HeaderText="Lastname2" SortExpression="Lastname2" />
                    <asp:BoundField DataField="Birthdate" HeaderText="Birthdate" SortExpression="Birthdate"  DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:TemplateField HeaderText="Gender">
                        <ItemTemplate>
                            <%# (bool)Eval("Gender") == true? "M" : "F" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Health Center">
                        <ItemTemplate>
                            <%#Eval("Healthcenter.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Birthplace">
                        <ItemTemplate>
                            <%#Eval("Birthplace.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Community">
                        <ItemTemplate>
                            <%#Eval("Community.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Domicile">
                        <ItemTemplate>
                            <%#Eval("Domicile.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%#Eval("Status.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                    <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                    <asp:BoundField DataField="MotherId" HeaderText="MotherId" SortExpression="MotherId" />
                    <asp:BoundField DataField="MotherFirstname" HeaderText="MotherFirstname" SortExpression="MotherFirstname" />
                    <asp:BoundField DataField="MotherLastname" HeaderText="MotherLastname" SortExpression="MotherLastname" />
                    <asp:BoundField DataField="FatherId" HeaderText="FatherId" SortExpression="FatherId" />
                    <asp:BoundField DataField="FatherFirstname" HeaderText="FatherFirstname" SortExpression="FatherFirstname" />
                    <asp:BoundField DataField="FatherLastname" HeaderText="FatherLastname" SortExpression="FatherLastname" />
                    <asp:BoundField DataField="CaretakerId" HeaderText="CaretakerId" SortExpression="CaretakerId" />
                    <asp:BoundField DataField="CaretakerFirstname" HeaderText="CaretakerFirstname" SortExpression="CaretakerFirstname" />
                    <asp:BoundField DataField="CaretakerLastname" HeaderText="CaretakerLastname" SortExpression="CaretakerLastname" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                    <asp:BoundField DataField="IdentificationNo1" HeaderText="IdentificationNo1" SortExpression="IdentificationNo1" />
                    <asp:BoundField DataField="IdentificationNo2" HeaderText="IdentificationNo2" SortExpression="IdentificationNo2" />
                    <asp:BoundField DataField="IdentificationNo3" HeaderText="IdentificationNo3" SortExpression="IdentificationNo3" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

