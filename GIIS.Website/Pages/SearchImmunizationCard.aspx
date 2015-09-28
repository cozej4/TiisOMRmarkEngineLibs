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
<%@ Page Title="Search Children for Immunization Card" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="SearchImmunizationCard.aspx.cs" Inherits="_SearchImmunizationCard" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var isresult1 = "test";
        var isresult2 = "test";
        var isresult3 = "test";
        var isresult4 = "test";
        function pageLoad(sender, args) {

            $find('autocomplteextender1')._onMethodComplete = function (result, context) {

                $find('autocomplteextender1')._update(context, result, /* cacheResults */false);
                webservice_callback1(result, context);
            };
            $find('autocomplteextender2')._onMethodComplete = function (result, context) {

                $find('autocomplteextender2')._update(context, result, /* cacheResults */false);
                webservice_callback2(result, context);
            };
            $find('autocomplteextender3')._onMethodComplete = function (result, context) {

                $find('autocomplteextender3')._update(context, result, /* cacheResults */false);
                webservice_callback3(result, context);
            };
            $find('autocomplteextender4')._onMethodComplete = function (result, context) {

                $find('autocomplteextender4')._update(context, result, /* cacheResults */false);
                webservice_callback4(result, context);
            };

        }
        function webservice_callback1(result, context) {

            if (result == "") {
                $find("autocomplteextender1").get_element().style.backgroundColor = "red";
                isresult1 = "";
            }
            else {
                $find("autocomplteextender1").get_element().style.backgroundColor = "white";
                isresult1 = "test";
            }
        }
        function webservice_callback2(result, context) {

            if (result == "") {
                $find("autocomplteextender2").get_element().style.backgroundColor = "red";
                isresult2 = "";
            }
            else {
                $find("autocomplteextender2").get_element().style.backgroundColor = "white";
                isresult2 = "test";
            }
        }
        function webservice_callback3(result, context) {

            if (result == "") {
                $find("autocomplteextender3").get_element().style.backgroundColor = "red";
                isresult3 = "";
            }
            else {
                $find("autocomplteextender3").get_element().style.backgroundColor = "white";
                isresult3 = "test";
            }
        }
        function webservice_callback4(result, context) {

            if (result == "") {
                $find("autocomplteextender4").get_element().style.backgroundColor = "red";
                isresult4 = "";
            }
            else {
                $find("autocomplteextender4").get_element().style.backgroundColor = "white";
                isresult4 = "test";
            }
        }
        function checkHFacility() {
            if (isresult1 == "") {
                alert("Please choose a health facility from the list!");
                return false;
            }
            if (isresult2 == "") {
                alert("Please choose a community from the list!");
                return false;
            }
            if (isresult3 == "") {
                alert("Please choose a birthplace from the list!");
                return false;
            }
            if (isresult4 == "") {
                alert("Please choose a domicile area from the list!");
                return false;
            }
            return true;
        }

    </script>
        <style type="text/css">
         .ajax__calendar_container {
             z-index: 1000;
         }
     </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Immunization</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Search Immunization Card" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
      
       <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <div class="row">
                <asp:Panel ID="pnlSystemId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblSystemId" runat="server" Text="System ID" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtSystemId" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="pnlBarcode" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBarcode" runat="server" Text="Barcode" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel runat="server" ID="trFirstname1">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblFirstname1" runat="server" Text="Firstname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtFirstname1" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel runat="server" ID="pnlMotherFirstname">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMotherFirstname" runat="server" Text="Mother Firstname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMotherFirstname" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trBirthdateFrom" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthdateFrom" runat="server" Text="Birthdate From:" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBirthdateFrom" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceBirthdateFrom" runat="server" TargetControlID="txtBirthdateFrom" />
                            <asp:RegularExpressionValidator ID="revBirthdateFrom" runat="server" ControlToValidate="txtBirthdateFrom" ValidationGroup="saveChild" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="pnlHealthcenter" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblHealthcenterId" runat="server" Text="HealthcenterId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <uc1:AutoCompleteTextbox runat="server"
                                ID="txtHealthcenterId"
                                OnClickSubmit="true"
                                ServiceMethod="AllHealthFacilities"
                                HighlightResults="true"
                                ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trDomicileId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblDomicileId" runat="server" Text="DomicileId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <uc4:DomicileTextbox ID="txtDomicileId"
                                runat="server" ClearAfterSelect="false"
                                HighlightResults="true" OnClickSubmit="true"
                                OnValueSelected="Domicile_ValueSelected"
                                ServiceMethod="Place" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="pnlStatus" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblStatusId" runat="server" Text="StatusId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="odsStatus" DataTextField="Name" DataValueField="Id" CssClass="form-control" />
                            <asp:ObjectDataSource ID="odsStatus" runat="server" SelectMethod="GetStatusForList" TypeName="GIIS.DataLayer.Status"></asp:ObjectDataSource>

                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
       
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <div class="row">
                <asp:Panel ID="pnlIdFields" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblIdentification" runat="server" Text="ID Fields" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtIdFields" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="pnlTempId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblTempId" runat="server" Text="Temp ID" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtTempId" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel runat="server" ID="trLastname1">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblLastname1" runat="server" Text="Lastname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtLastname1" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel runat="server" ID="pnlMotherLastname">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMotherLastname" runat="server" Text="Mother Lastname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMotherLastname" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trBirthdateTo" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthdateTo" runat="server" Text="Birthdate To:" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBirthdateTo" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceBirthdateTo" runat="server" TargetControlID="txtBirthdateTo" />
                            <asp:RegularExpressionValidator ID="revBirthdateTo" runat="server" ControlToValidate="txtBirthdateTo" ValidationGroup="saveChild" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trBirthplaceId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthplaceId" runat="server" Text="BirthplaceId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <%--<uc3:BirthplacesTextbox ID="txtBirthplaceId" runat="server" ClearAfterSelect="false" HighlightResults="true" OnClickSubmit="true" OnValueSelected="Places_ValueSelected" ServiceMethod="BirthPlace" />--%>
                            <asp:DropDownList ID="ddlBirthplace" runat="server" DataSourceID="odsBirthplace" DataTextField="Name" DataValueField="Id" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlBirthplace_SelectedIndexChanged" />
                            <asp:ObjectDataSource ID="odsBirthplace" runat="server" SelectMethod="GetBirthplaceListNew" TypeName="GIIS.DataLayer.Birthplace"></asp:ObjectDataSource>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trCommunityId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblCommunityId" runat="server" Text="CommunityId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <uc2:CommunitiesTextbox ID="txtCommunityId"
                                runat="server" ClearAfterSelect="false"
                                HighlightResults="true" OnClickSubmit="true"
                                OnValueSelected="Communities_ValueSelected"
                                ServiceMethod="Community" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
        </div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <%--            <asp:CustomValidator ID="cvBirthdate" runat="server" ErrorMessage="First date cannot be after second date!" OnServerValidate="ValidateBirthdate" CssClass="label label-warning" Font-Size="Small" Display="Dynamic" ForeColor="White" ValidationGroup="saveChild" ValidateEmptyText="false"></asp:CustomValidator>--%>
            <asp:Label ID="lblWarning" runat="server" Text="There are no children that match your search criteria!" CssClass="label label-warning" Font-Size="Small" Visible="false" />

        </div>
    </div>

    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveChild" />
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow: auto">
            <asp:GridView ID="gvChild" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnDataBound="gvChild_DataBound" OnPageIndexChanging="gvChild_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <RowStyle CssClass="gridviewRow" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <HeaderStyle CssClass="gridviewHeader" />
                <AlternatingRowStyle CssClass="gridviewRowAlt" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="SystemID">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlSystemId" runat="server" NavigateUrl='<%# Eval("Id", "ViewImmunizationCard.aspx?id={0}") %>'
                                Text='<%# Eval("SystemId", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--  <asp:BoundField DataField="SystemId" HeaderText="SystemId" SortExpression="SystemId" />--%>
                    <asp:BoundField DataField="Firstname1" HeaderText="Firstname1" SortExpression="Firstname1" />
                    <asp:BoundField DataField="Firstname2" HeaderText="Firstname2" SortExpression="Firstname2" />
                    <asp:BoundField DataField="Lastname1" HeaderText="Lastname1" SortExpression="Lastname1" />
                    <asp:BoundField DataField="Lastname2" HeaderText="Lastname2" SortExpression="Lastname2" />
                    <asp:BoundField DataField="Birthdate" HeaderText="Birthdate" SortExpression="Birthdate" DataFormatString="{0:dd-MMM-yyyy}" />
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
            <asp:ObjectDataSource ID="odsChild" runat="server" EnablePaging="true" SelectMethod="GetPagedChildList" TypeName="GIIS.DataLayer.Child" SelectCountMethod="GetCountChildList">
                <SelectParameters>
                    <asp:Parameter Name="statusId" Type="Int32" />
                    <asp:Parameter Name="birthdateFrom" Type="DateTime" />
                    <asp:Parameter Name="birthdateTo" Type="DateTime" />
                    <asp:Parameter Name="firstname1" Type="String" />
                    <asp:Parameter Name="lastname1" Type="String" />
                    <asp:Parameter Name="motherFirstname" Type="String" />
                    <asp:Parameter Name="motherLastname" Type="String" />
                    <asp:Parameter Name="idFields" Type="String" />
                    <asp:Parameter Name="systemId" Type="String" />
                    <asp:Parameter Name="barcodeId" Type="String" />
                    <asp:Parameter Name="tempId" Type="String" />
                    <asp:Parameter Name="healthFacilityId" Type="String" />
                    <asp:Parameter Name="birthplaceId" Type="Int32" />
                    <asp:Parameter Name="communityId" Type="Int32" />
                    <asp:Parameter Name="domicileId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>


</asp:Content>

