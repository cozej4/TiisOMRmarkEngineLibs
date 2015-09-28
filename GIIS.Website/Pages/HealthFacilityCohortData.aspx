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
<%@ Page Title="HealthFacilityCohortData" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="HealthFacilityCohortData.aspx.cs" Inherits="_HealthFacilityCohortData" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Immunization</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="HealthFacilityCohortData" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHealthFacilityId" runat="server" Text="Health Facility" />
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Label ID="lblHealthFacility" runat="server" Font-Bold="true" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblYear" runat="server" Text="Year" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-----" Value="0"></asp:ListItem>
                    <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
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
                    <asp:ListItem Text="2031" Value="2031"></asp:ListItem>
                    <asp:ListItem Text="2032" Value="2032"></asp:ListItem>
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
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <%--            <asp:CompareValidator ID="cvYear" runat="server" ControlToValidate="ddlYear" ErrorMessage="Please select a year!" ValidationGroup="saveHealthFacilityCohortData" Operator="NotEqual" ForeColor="Red" ValueToCompare="0"></asp:CompareValidator>--%>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblCohort" runat="server" Text="Cohort" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtCohort" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <%--            <asp:RequiredFieldValidator ID="rfvCohort" runat="server" ErrorMessage="This field must be filled!" ControlToValidate="txtCohort" ValidationGroup="saveHealthFacilityCohortData" Display="Dynamic" ForeColor="Red" />--%>
            <asp:RegularExpressionValidator ID="revCohort" runat="server"
                ErrorMessage=" Cohort should be only numbers!"
                ControlToValidate="txtCohort" Display="Dynamic" CssClass="label label-warning" Font-Size="Small" ForeColor="White"
                ValidationExpression="^[0-9]+$" />
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
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:CustomValidator ID="cvHealthFacility" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvHealthFacility_Validate" Display="Dynamic" ValidationGroup="saveHealthFacilityCohortData" CssClass="label label-warning" Font-Size="Small" ForeColor="White"></asp:CustomValidator>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveHealthFacilityCohortData" />
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" ValidationGroup="saveHealthFacilityCohortData" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
         <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
             </div> 
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
            <asp:GridView ID="gvHealthFacilityCohortData" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsHealthFacilityCohortData" OnPageIndexChanging="gvHealthFacilityCohortData_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="Year">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlYear" runat="server" NavigateUrl='<%# Eval("Id", "HealthFacilityCohortData.aspx?id={0}") %>'
                                Text='<%# Eval("Year", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HealthFacility" Visible="false">
                        <ItemTemplate>
                            <%#Eval("HealthFacility.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Cohort" HeaderText="Cohort" SortExpression="Cohort" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsHealthFacilityCohortData" runat="server" EnablePaging="true" SelectMethod="GetPagedHealthFacilityCohortDataList" TypeName="GIIS.DataLayer.HealthFacilityCohortData" SelectCountMethod="GetCountHealthFacilityCohortDataList">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
    </div>
    <div class="row" style="overflow: auto">
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvHealthFacilityCohortData_DataBound" Visible="false">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" />
                <asp:TemplateField HeaderText="HealthFacility">
                    <ItemTemplate>
                        <%#Eval("HealthFacility.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Cohort" HeaderText="Cohort" SortExpression="Cohort" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvHealthFacility_Validate(sender, args) {
            var text = (document.getElementById('<%=txtCohort.ClientID%>').value == '') || (document.getElementById('<%=ddlYear.ClientID%>').value == '-1');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }

    </script>
</asp:Content>

