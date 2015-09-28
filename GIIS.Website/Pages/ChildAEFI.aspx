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
<%@ Page Title="Child AEFI" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ChildAEFI.aspx.cs" Inherits="Pages_ChildAEFI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Child</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Child AEFI" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">&nbsp;</div>
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
            <asp:Label ID="lblInfo" runat="server" Text="This baby does not have a new vaccination encounter to register AEFI for!" CssClass="label label-info" Font-Size="Small" Visible="false"></asp:Label>
        </div>
    </div>
    <div class="row">

        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvVaccinationAppointment" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive"
                DataSourceID="odsVaccinationAppointment" OnDataBound="gvVaccinationAppointment_DataBound"
                OnRowDataBound="gvVaccinationAppointment_RowDataBound" EnableModelValidation="True">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="APPOINTMENT_ID" HeaderText="Id" SortExpression="APPOINTMENT_ID" />
                    <asp:BoundField DataField="VACCINES" HeaderText="Vaccines" SortExpression="VACCINES" />
                    <asp:BoundField DataField="VACCINATION_DATE" HeaderText="Vaccination Date" SortExpression="VACCINATION_DATE" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="HEALTH_FACILITY" HeaderText="Health Facility" SortExpression="HEALTH_FACILITY" />
                    <asp:CheckBoxField DataField="DONE" HeaderText="Done" SortExpression="DONE" ReadOnly="true" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsVaccinationAppointment" runat="server"
                SelectMethod="getVaccinationEventforAEFI" TypeName="GIIS.DataLayer.VaccinationEvent">
                <SelectParameters>
                    <asp:Parameter Name="childId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <asp:CheckBox ID="chbxChildAEFI" runat="server" Checked="true" Text="Child had AEFI on this encounter" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblDate" runat="server" Text="Date" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" />
                <asp:RegularExpressionValidator ID="revDate" runat="server" ControlToValidate="txtDate" ValidationGroup="saveAefi" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lblNotes" runat="server" Text="Notes" />
        </div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Height="70px" />
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
            <asp:CustomValidator ID="cvAEFIdate" runat="server" ErrorMessage="AEFI Date cannot be before Vaccination Date and in the future!"
                OnServerValidate="ValidateAEFIDate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic"
                ValidationGroup="saveAefi" ValidateEmptyText="false"></asp:CustomValidator>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnSave_Click"
                ValidationGroup="saveAefi" />
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click"
                ValidationGroup="saveAefi" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning btn-raised" OnClick="btnClear_Click"  />
            
        </div>
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
            <asp:GridView ID="gvVccEvent" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive"
                DataSourceID="odsVccEvent" OnDataBound="gvVaccinationAppointment_DataBound"
                OnRowDataBound="gvVccEvent_RowDataBound" EnableModelValidation="True">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="APPOINTMENT_ID" HeaderText="Id" SortExpression="APPOINTMENT_ID" Visible="False" />
                    <asp:TemplateField HeaderText="Vaccines">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlDose" runat="server"
                                NavigateUrl='<%# Eval("APPOINTMENT_ID", "ChildAEFI.aspx?appId={0}") %>'
                                Text='<%# Eval("VACCINES", "{0}") %>' Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VACCINATION_DATE" HeaderText="Vaccination Date" SortExpression="VACCINATION_DATE" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="HEALTH_FACILITY" HeaderText="Health Facility" SortExpression="HEALTH_FACILITY" />
                    <asp:CheckBoxField DataField="DONE" HeaderText="Done" SortExpression="DONE" ReadOnly="true" />
                    <asp:CheckBoxField DataField="AEFI" HeaderText="AEFI" SortExpression="AEFI" ReadOnly="true" />
                    <asp:BoundField DataField="AEFI_DATE" HeaderText="AEFI Date" SortExpression="AEFI_DATE" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="NOTES" HeaderText="Notes" SortExpression="NOTES" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsVccEvent" runat="server"
                SelectMethod="getVaccinationEventforAEFINew" TypeName="GIIS.DataLayer.VaccinationEvent">
                <SelectParameters>
                    <asp:Parameter Name="childId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <br />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
</asp:Content>
