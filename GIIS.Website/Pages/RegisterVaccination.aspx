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
<%@ Page Title="Register Vaccination" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="RegisterVaccination.aspx.cs" Inherits="Pages_RegisterVaccination" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
    <script type="text/javaScript">
        function cvHealthFacility_Validate(sender, args) {
            var text = (document.getElementById('<%=ddlHealthFacility.ClientID%>').value == '-1');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Immunization</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Register Vaccination" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lbChild" runat="server" Text="Child:"></asp:Label>
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:LinkButton ID="lnkChild" runat="server" Font-Bold="true"></asp:LinkButton>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lbChildBirthdate" runat="server" Text="Birthdate:"></asp:Label>
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Label ID="lblChildBirthdate" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lbHealthCenter" runat="server" Text="Health Center:"></asp:Label>
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Label ID="lblHealthCenter" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lbVaccineDose" runat="server" Text="Vaccine Dose:"></asp:Label>
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Label ID="lblVaccineDose" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lbScheduledDate" runat="server" Text="Scheduled Date:"></asp:Label>
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Label ID="lblScheduledDate" runat="server" Font-Bold="true"></asp:Label>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnImmunizationCard" runat="server" CssClass="btn btn-raised btn-sm btn-material-teal" Text="Immunization Card" OnClick="btnImmunizationCard_Click" />
        </div>
    </div>
    <hr style="color: gray" />
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">&nbsp;</div>
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
            <asp:Label ID="lblInfo" runat="server" Text="You can not apply this vaccine at the moment." CssClass="label label-info" Font-Size="Small" Visible="false"></asp:Label>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHealthFacilityId" runat="server" Text="Health Facility" />
             <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlHealthFacility" runat="server" CssClass="form-control" DataSourceID="odsHealthF" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlHealthFacility_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsHealthF" runat="server" SelectMethod="GetVaccinationPointList" TypeName="GIIS.DataLayer.HealthFacility">
                    <SelectParameters>
                        <asp:Parameter Name="ids" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:CheckBox ID="chbxOutreach" runat="server" Text="Outreach" Checked="true" />
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
           <%-- <asp:CustomValidator ID="cvHealthFacility" runat="server" ErrorMessage="Health Facility must be filled!"
                ClientValidationFunction="cvHealthFacility_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White"
                Display="Dynamic" ValidationGroup="saveVaccinationEvent"></asp:CustomValidator>--%>
            <asp:CustomValidator ID="cvVaccinationEvent" runat="server" ErrorMessage="All fields marked with * must be filled!" OnServerValidate="cvVaccinationEvent_ServerValidate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveVaccinationEvent"></asp:CustomValidator>
          <%--  <asp:CustomValidator ID="cvVaccinationdate" runat="server" ErrorMessage="Vaccination Date cannot be after today!"
                OnServerValidate="ValidateVaccinationDate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic"
                ValidationGroup="saveVaccinationEvent" ValidateEmptyText="false"></asp:CustomValidator>--%>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-11 col-xs-11 col-sm-11 col-lg-11 clearfix">
            <asp:GridView ID="gvVaccinationEvent" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive"
                DataSourceID="odsVaccinationEvent" OnDataBound="gvVaccinationEvent_DataBound" OnRowDataBound="gvVaccinationEvent_RowDataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                    <asp:TemplateField HeaderText="Vaccine Dose">
                        <ItemTemplate>
                            <%#Eval("Dose.Fullname")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vaccine Lot">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlVaccineLot" runat="server" CssClass="form-control"
                                DataSourceID="odsItemLot" DataTextField="LOT_NUMBER" DataValueField="ID"
                                OnSelectedIndexChanged="ddlVaccineLot_OnSelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsItemLot" runat="server" SelectMethod="GetItemLotForList" TypeName="GIIS.DataLayer.ItemLot">
                                <SelectParameters>
                                    <asp:Parameter Name="itemId" Type="Int32" />
                                    <asp:Parameter Name="hfId" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="VaccinationDate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtVaccinationDate" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceVaccinationDate" runat="server" TargetControlID="txtVaccinationDate" />
                            <asp:RegularExpressionValidator ID="revVaccinationDate" runat="server" ControlToValidate="txtVaccinationDate"
                                ValidationGroup="saveVaccinationEvent" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Done">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkStatus" runat="server" Checked='True' AutoPostBack="true" OnCheckedChanged="chkDoneStatus_OnCheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NonVaccination Reason">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlNonvaccinationReason" runat="server" CssClass="form-control" Visible="false"
                                DataSourceID="odsNonVaccinationReason" DataTextField="Name" DataValueField="Id">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="odsNonVaccinationReason" runat="server" SelectMethod="GetNonvaccinationReasonForList"
                                TypeName="GIIS.DataLayer.NonvaccinationReason"></asp:ObjectDataSource>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NonvaccinationReasonId" HeaderText="NonvaccinationReasonId" SortExpression="NonvaccinationReasonId" />
                    <asp:TemplateField HeaderText="Vaccine Dose" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsVaccinationEvent" runat="server" SelectMethod="GetVaccinationEventByAppointmentId" TypeName="GIIS.DataLayer.VaccinationEvent">
                <SelectParameters>
                    <asp:Parameter Name="appId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <table class="table table-striped table-bordered table-hover table-responsive" id="tblsupp" runat ="server">
                <tr>
                    <td>
                        <asp:Label ID="lblSupplement" runat="server" Text="Supplement"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblGiven" runat="server" Text="Given"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVitA" runat="server" Text="VitA"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblVitADate" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkVitA" Text="Yes" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMebendezol" runat="server" Text="Mebendezol"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMebendezolDate" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkMebendezol" runat="server" Text="Yes" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <br />
            <br />
            <br />
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click"
                ValidationGroup="saveVaccinationEvent" OnClientClick="if (!checkHFacility()) return false;" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <br />
            <br />
            <br />
            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-raised btn-warning" OnClick="btnRemove_Click" />
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
            <asp:GridView ID="gvChild" runat="server" Visible="false" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsChild">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="APPOINTMENT_ID" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="VACCINES" HeaderText="Next Appointment" SortExpression="Id" />
                    <asp:BoundField DataField="SCHEDULED_DATE" HeaderText="Scheduled Date" SortExpression="ScheduledDate" DataFormatString="{0:dd-MMM-yyyy}" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsChild" runat="server" SelectMethod="GetNextDueVaccinesForChild" TypeName="GIIS.DataLayer.VaccinationEvent">
                <SelectParameters>
                    <asp:Parameter Name="childId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>

