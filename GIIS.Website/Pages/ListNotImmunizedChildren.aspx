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
<%@ Page Title="Not Vaccinated Children" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ListNotImmunizedChildren.aspx.cs" Inherits="Pages_ListNotImmunizedChildren" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var isresult = "test";
        function pageLoad(sender, args) {

            $find('autocomplteextender1')._onMethodComplete = function (result, context) {

                $find('autocomplteextender1')._update(context, result, /* cacheResults */false);
                webservice_callback(result, context, sender);

            };

        }
        function webservice_callback(result, context, sender) {

            if (result == "") {
                $find("autocomplteextender1").get_element().style.backgroundColor = "red";
                isresult = "";
            }
            else {
                $find("autocomplteextender1").get_element().style.backgroundColor = "white";
                isresult = "test";

            }
        }
        function checkHFacility() {
            if (isresult == "") {
                alert("Please choose a health facility from the list!");
                // alert(text);
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
                <li><a href="#">Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="List not Immunized Children" /></li>
            </ol>
        </div>
    </div>
   <%-- <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHealthFacility" runat="server" Text="Health Facility" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc1:AutoCompleteTextbox runat="server"
                    ID="txtHealthFacility"
                    OnClickSubmit="true"
                    ServiceMethod="HealthCentersUser"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
            </div>
        </div>
          <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm btn-raised" OnClick="btnSearch_Click" OnClientClick="if (!checkHFacility()) return false;" />
        </div>
    </div>
   
    <br />
    <br />--%>
    <div class="row">
        <%--<div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>--%>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <asp:Label ID="lblVaccine" runat="server" Text="Vaccine: ">:</asp:Label>
            &nbsp;&nbsp;
                     <asp:Label ID="txtVaccine" runat="server" Text="" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <br />
     <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no not-immunized children with this vaccine in this health facility!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix" style="overflow:auto">
        <asp:GridView ID="gvChild" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsChild" OnDataBinding="gvChild_DataBinding">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Notes" HeaderText="Reason" SortExpression="Notes" />
                <asp:TemplateField HeaderText="SystemID">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlSystemId" runat="server" NavigateUrl='<%# Eval("Id", "Child.aspx?id={0}") %>'
                            Text='<%# Eval("SystemId", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--  <asp:BoundField DataField="SystemId" HeaderText="SystemId" SortExpression="SystemId" />--%>
                <asp:BoundField DataField="Firstname1" HeaderText="Firstname1" SortExpression="Firstname1" />
                <asp:BoundField DataField="Lastname1" HeaderText="Lastname1" SortExpression="Lastname1" />
                   <asp:BoundField DataField="MotherFirstname" HeaderText="MotherFirstname" SortExpression="MotherFirstname" />
                <asp:BoundField DataField="MotherLastname" HeaderText="MotherLastname" SortExpression="MotherLastname" />
                <asp:BoundField DataField="Birthdate" HeaderText="Birthdate" SortExpression="Birthdate" />
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
                <asp:TemplateField HeaderText="Domicile">
                    <ItemTemplate>
                        <%#Eval("Domicile.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
             
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsChild" runat="server" EnablePaging="true" SelectMethod="GetNotImmunizedChildren" TypeName="GIIS.DataLayer.Child" SelectCountMethod="GetCountNotImmunizedChildren">
            <SelectParameters>
                <asp:Parameter Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div></div> 
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
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvChild_DataBinding" Visible="false">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Notes" HeaderText="Reason" SortExpression="Notes" />
                <asp:BoundField DataField="SystemId" HeaderText="SystemId" SortExpression="SystemId" />
                <asp:BoundField DataField="Firstname1" HeaderText="Firstname1" SortExpression="Firstname1" />
                <asp:BoundField DataField="Lastname1" HeaderText="Lastname1" SortExpression="Lastname1" />
                   <asp:BoundField DataField="MotherFirstname" HeaderText="MotherFirstname" SortExpression="MotherFirstname" />
                <asp:BoundField DataField="MotherLastname" HeaderText="MotherLastname" SortExpression="MotherLastname" />
                <asp:BoundField DataField="Birthdate" HeaderText="Birthdate" SortExpression="Birthdate" />
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
                <asp:TemplateField HeaderText="Domicile">
                    <ItemTemplate>
                        <%#Eval("Domicile.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
             
               
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

