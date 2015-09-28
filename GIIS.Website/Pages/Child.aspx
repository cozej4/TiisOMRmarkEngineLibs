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
<%@ Page Title="Child" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Child.aspx.cs" Inherits="_Child" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Child</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Child" /></li>
            </ol>
        </div>
    </div>
    <%-- <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <div class="row">
                <asp:Panel ID="pnlSystem" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblSystemId" runat="server" Text="SystemId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtSystemId" runat="server" CssClass="form-control" Enabled="false" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
             
            <div class="row">
                <asp:Panel runat="server" ID="trFirstname1">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblFirstname1" runat="server" Text="Firstname1" />
                        <span id="spanFirstname1" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtFirstname1" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
          
            <div class="row">
                <asp:Panel ID="trMotherFirstname" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMotherFirstname" runat="server" Text="MotherFirstname" />
                        <span id="spanMotherFirstname" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMotherFirstname" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trBirthdate" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthdate" runat="server" Text="Birthdate" />
                        <span id="spanBirthdate" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBirthdate" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceBirthdate" runat="server" TargetControlID="txtBirthdate" />
                            <asp:RegularExpressionValidator ID="revBirthdate" runat="server" ControlToValidate="txtBirthdate" ValidationGroup="saveChild" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trBirthplaceId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthplaceId" runat="server" Text="BirthplaceId" />
                        <span id="spanBirthplaceId" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <uc3:BirthplacesTextbox ID="txtBirthplaceId"
                                runat="server" ClearAfterSelect="false"
                                HighlightResults="true" OnClickSubmit="true"
                                OnValueSelected="Places_ValueSelected"
                                ServiceMethod="BirthPlace" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
           
           
            <div class="row">
                <asp:Panel ID="pnlHealthcenter" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblHealthcenterId" runat="server" Text="HealthcenterId" />
                        <span id="spanHealthcenterId" runat="server" style="color: Red">*</span>
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
                <asp:Panel ID="trIdentificationNo1" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblIdentificationNo1" runat="server" Text="IdentificationNo1" />
                        <span id="spanIdentificationNo1" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtIdentificationNo1" runat="server" CssClass="form-control" />
                            <asp:RegularExpressionValidator ID="revIdentification1" runat="server"
                                ErrorMessage="Identification 1 not correct!"
                                ControlToValidate="txtIdentificationNo1" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveChild" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trIdentificationNo2" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblIdentificationNo2" runat="server" Text="IdentificationNo2" />
                        <span id="spanIdentificationNo2" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtIdentificationNo2" runat="server" CssClass="form-control" />
                            <asp:RegularExpressionValidator ID="revIdentification2" runat="server"
                                ErrorMessage="Identification 2 not correct!"
                                ControlToValidate="txtIdentificationNo2" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveChild" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trIdentificationNo3" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblIdentificationNo3" runat="server" Text="IdentificationNo3" />
                        <span id="spanIdentificationNo3" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group"></div>
                        <asp:TextBox ID="txtIdentificationNo3" runat="server" CssClass="form-control" />
                        <asp:RegularExpressionValidator ID="revIdentification3" runat="server"
                            ErrorMessage="Identification 3 not correct!"
                            ControlToValidate="txtIdentificationNo3" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveChild" />
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
        <%--left column div--%>
    <%--      right column div--%>
    <%--  <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
               <div class="row">
                <asp:Panel ID="Panel2" runat="server">
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
                <asp:Panel ID="Panel1" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBarcode" runat="server" Text="Barcode ID" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
             <div class="row">
                <asp:Panel runat="server" ID="trLastname1">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblLastname1" runat="server" Text="Lastname1" />
                        <span id="spanLastname1" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtLastname1" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
              <div class="row">
                <asp:Panel ID="trMotherLastname" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMotherLastname" runat="server" Text="MotherLastname" />
                        <span id="spanMotherLastname" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMotherLastname" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
             <div class="row">
                <asp:Panel ID="pnlGender" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblGender" runat="server" Text="Gender" />
                        <span id="Span1" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="True" Text="Male"></asp:ListItem>
                                <asp:ListItem Value="False" Text="Female"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trDomicileId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblDomicileId" runat="server" Text="DomicileId" />
                        <span id="spanDomicileId" runat="server" style="color: Red">*</span>
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
                <asp:Panel ID="trCommunityId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblCommunityId" runat="server" Text="CommunityId" />
                        <span id="spanCommunityId" runat="server" style="color: Red">*</span>
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
            <div class="row">
                <asp:Panel ID="trAddress" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblAddress" runat="server" Text="Address" />
                        <span id="spanAddress" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trPhone" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblPhone" runat="server" Text="Phone" />
                        <span id="spanPhone" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trMobile" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMobile" runat="server" Text="Mobile" />
                        <span id="spanMobile" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
             <div class="row">
                <asp:Panel ID="trMotherId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMotherId" runat="server" Text="MotherId" />
                        <span id="spanMotherId" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMotherId" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
           
            <div class="row">
                <asp:Panel ID="trNotes" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblNotes" runat="server" Text="Notes" />
                        <span id="spanNotes" runat="server" style="color: Red">*</span>
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>--%>

    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblSystemId" runat="server" Text="SystemId" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtSystemId" runat="server" CssClass="form-control" Enabled="false" BackColor="#f3f3f3" />
            </div>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblBarcodeID" runat="server" Text="Barcode ID" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtBarcodeId" runat="server" CssClass="form-control" />
            </div>
        </div>

    </div>
       <div class="row">

        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblMotherFirstname" runat="server" Text="MotherFirstname" />
            <span id="spanMotherFirstname" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtMotherFirstname" runat="server" CssClass="form-control" />
            </div>
        </div>

        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblMotherLastname" runat="server" Text="MotherLastname" />
            <span id="spanMotherLastname" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtMotherLastname" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblFirstname1" runat="server" Text="Firstname" />
            <span id="spanFirstname1" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtFirstname1" runat="server" CssClass="form-control" />
            </div>
        </div>

        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblLastname1" runat="server" Text="Lastname1" />
            <span id="spanLastname1" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtLastname1" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
  <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblFirstname2" runat="server" Text="Second name" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtFirstname2" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblBirthdate" runat="server" Text="Birthdate" />
            <span id="spanBirthdate" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtBirthdate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceBirthdate" runat="server" TargetControlID="txtBirthdate" />
                <asp:RegularExpressionValidator ID="revBirthdate" runat="server" ControlToValidate="txtBirthdate" ValidationGroup="saveChild" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />

            </div>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblGender" runat="server" Text="Gender" />
            <span id="Span1" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True" Text="Male"></asp:ListItem>
                    <asp:ListItem Value="False" Text="Female"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblBirthplaceId" runat="server" Text="BirthplaceId" />
            <span id="spanBirthplaceId" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <%--<uc3:BirthplacesTextbox ID="txtBirthplaceId" runat="server" ClearAfterSelect="false" HighlightResults="true" OnClickSubmit="true" OnValueSelected="Places_ValueSelected" ServiceMethod="BirthPlace" />--%>
                <asp:DropDownList ID="ddlBirthplace" runat="server" DataSourceID="odsBirthplace" DataTextField="Name" DataValueField="Id" CssClass="form-control" />
                <asp:ObjectDataSource ID="odsBirthplace" runat="server" SelectMethod="GetBirthplaceListNew" TypeName="GIIS.DataLayer.Birthplace"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblDomicileId" runat="server" Text="DomicileId" />
            <span id="spanDomicileId" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc4:DomicileTextbox ID="txtDomicileId"
                    runat="server" ClearAfterSelect="false"
                    HighlightResults="true" OnClickSubmit="true"
                    OnValueSelected="Domicile_ValueSelected"
                    ServiceMethod="Place" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblHealthcenterId" runat="server" Text="HealthcenterId" />
            <span id="spanHealthcenterId" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc1:AutoCompleteTextbox runat="server"
                    ID="txtHealthcenterId"
                    OnClickSubmit="true"
                    ServiceMethod="VaccinationPoints"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
            </div>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblCommunityId" runat="server" Text="CommunityId" />
            <span id="spanCommunityId" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc2:CommunitiesTextbox ID="txtCommunityId"
                    runat="server" ClearAfterSelect="false"
                    HighlightResults="true" OnClickSubmit="true"
                    OnValueSelected="Communities_ValueSelected"
                    ServiceMethod="Community" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblStatusId" runat="server" Text="StatusId" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="odsStatus" DataTextField="Name" DataValueField="Id" CssClass="form-control" />
                <asp:ObjectDataSource ID="odsStatus" runat="server" SelectMethod="GetStatusForList" TypeName="GIIS.DataLayer.Status"></asp:ObjectDataSource>

            </div>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblPhone" runat="server" Text="Phone" />
            <span id="spanPhone" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            </div>
        </div>

    </div>
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblNotes" runat="server" Text="Notes" />
            <span id="spanNotes" runat="server" style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblTempId" runat="server" Text="Temp ID" Visible="false" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtTempId" runat="server" CssClass="form-control" Visible="false" />
            </div>
        </div>

    </div>
    <div class="row">
        <asp:Panel ID="trIdentificationNo1" runat="server">
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <asp:Label ID="lblIdentificationNo1" runat="server" Text="IdentificationNo1" />
                <span id="spanIdentificationNo1" runat="server" style="color: Red">*</span>
            </div>
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <div class="form-group">
                    <asp:TextBox ID="txtIdentificationNo1" runat="server" CssClass="form-control" />
                    <asp:RegularExpressionValidator ID="revIdentification1" runat="server"
                        ErrorMessage="Identification 1 not correct!"
                        ControlToValidate="txtIdentificationNo1" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveChild" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="trIdentificationNo2" runat="server">
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <asp:Label ID="lblIdentificationNo2" runat="server" Text="IdentificationNo2" />
                <span id="spanIdentificationNo2" runat="server" style="color: Red">*</span>
            </div>
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <div class="form-group">
                    <asp:TextBox ID="txtIdentificationNo2" runat="server" CssClass="form-control" />
                    <asp:RegularExpressionValidator ID="revIdentification2" runat="server"
                        ErrorMessage="Identification 2 not correct!"
                        ControlToValidate="txtIdentificationNo2" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveChild" />
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <asp:Panel ID="trIdentificationNo3" runat="server">
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <asp:Label ID="lblIdentificationNo3" runat="server" Text="IdentificationNo3" />
                <span id="spanIdentificationNo3" runat="server" style="color: Red">*</span>
            </div>
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <div class="form-group"></div>
                <asp:TextBox ID="txtIdentificationNo3" runat="server" CssClass="form-control" />
                <asp:RegularExpressionValidator ID="revIdentification3" runat="server"
                    ErrorMessage="Identification 3 not correct!"
                    ControlToValidate="txtIdentificationNo3" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveChild" />
            </div>
        </asp:Panel>
        <asp:Panel ID="trAddress" runat="server">
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <asp:Label ID="lblAddress" runat="server" Text="Address" />
                <span id="spanAddress" runat="server" style="color: Red">*</span>
            </div>
            <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                <div class="form-group">
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
        </div>
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
            <asp:CustomValidator ID="cvChild" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvChild_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveChild"></asp:CustomValidator>
            <asp:Label ID="revBirthplace" runat="server" CssClass="label label-warning" Font-Size="Small" Text=" The birthplace you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
            <asp:Label ID="revDomicile" runat="server" CssClass="label label-warning" Font-Size="Small" Text=" The domicile area you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
            <asp:Label ID="revHealthcenter" runat="server" CssClass="label label-warning" Font-Size="Small" Text=" The health center you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
            <asp:Label ID="revCommunity" runat="server" CssClass="label label-warning" Font-Size="Small" Text=" The community you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
            <%--            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="The email is not valid!" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveChild" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"> </asp:RegularExpressionValidator>--%>
            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" ForeColor="White"
                ErrorMessage="The phone number is not valid!" ValidationGroup="saveChild" CssClass="label label-danger" Font-Size="Small" ValidationExpression="^[0-9 -()+]{4,30}$"> </asp:RegularExpressionValidator>
            <%--            <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" ForeColor="White"
                ErrorMessage="The mobile number is not valid!" ValidationGroup="saveChild" CssClass="label label-danger" Font-Size="Small" ValidationExpression="^[0-9 -]{4,30}$"> </asp:RegularExpressionValidator>--%>
            <asp:RegularExpressionValidator ID="revFirstname1" runat="server" ForeColor="White"
                ErrorMessage=" First name should be at least 2 characters long and should not contain numbers!"
                ControlToValidate="txtFirstname1" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ValidationGroup="saveChild"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]{2,50}$" />
                        <asp:RegularExpressionValidator ID="revFirstname2" runat="server" ForeColor="White"
                ErrorMessage="First name should be at least 2 characters long and should not contain numbers!"
                ControlToValidate="txtFirstname2" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ValidationGroup="saveChild"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]{2,50}$" />
            <asp:RegularExpressionValidator ID="revLastname1" runat="server" ForeColor="White"
                ErrorMessage="Last name should be at least 2 characters long and should not contain numbers!"
                ControlToValidate="txtLastname1" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ValidationGroup="saveChild"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]{2,50}$" />
            <%--<asp:RegularExpressionValidator ID="revLastname2" runat="server" ForeColor="White"
                ErrorMessage="Last name should be at least 2 characters long and should not contain numbers!"
                ControlToValidate="txtLastname2" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ValidationGroup="saveChild"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]{2,50}$" />--%>
            <asp:CustomValidator ID="cvBirthdate" runat="server" ErrorMessage="Birthdate cannot be after today!" ForeColor="White" OnServerValidate="ValidateBirthdate" CssClass="label label-danger" Font-Size="Small" Display="Dynamic" ValidationGroup="saveChild" ValidateEmptyText="false"></asp:CustomValidator>
            <%--<asp:CompareValidator ID="cvBirthdate" runat="server" ControlToValidate="txtBirthdate" Operator="LessThan" Display="Dynamic" ValueToCompare="<%# DateTime.Today.ToShortDateString() %>" Type="Date" Text="Birthdate cannot be after today!" ErrorMessage="Birthdate cannot be after today!" ValidationGroup="saveChild"></asp:CompareValidator>--%>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveChild" />
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveChild" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAddNew" runat="server" Text="Add New" CssClass="btn btn-material-bluegrey btn-sm btn-raised" ValidationGroup="saveChild" OnClick="btnAddNew_Click" />
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:Label ID="lblSuccess" runat="server" CssClass="label label-success" Font-Size="Small" Text="Success" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" CssClass="label label-warning" Font-Size="Small" Text="Warning" Visible="false" />
            <asp:Label ID="lblWarningBarcode" runat="server" CssClass="label label-warning" Font-Size="Small" Text="This barcode is already assigned, please choose another one." Visible="false" />
            <asp:LinkButton ID="lnbContinue" runat="server" Text="Continue with registration" OnClick="lnbContinue_Click" Visible="false"></asp:LinkButton>
            <asp:Label ID="lblError" runat="server" CssClass="label label-danger" Font-Size="Small" Text="Error" Visible="false" />
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvVaccinationEvent" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsVaccinationEvent" OnDataBound="gvVaccinationEvent_DataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="APPOINTMENT_ID" HeaderText="Id" SortExpression="Id" />
                    <asp:TemplateField HeaderText="Vaccine Dose">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlDose" runat="server"
                                NavigateUrl='<%# Eval("APPOINTMENT_ID", "RegisterVaccination.aspx?appId={0}") %>'
                                Text='<%# Eval("VACCINES", "{0}") %>' Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SCHEDULE" HeaderText="Schedule" SortExpression="SCHEDULE" />
                    <asp:BoundField DataField="SCHEDULED_DATE" HeaderText="Scheduled Date" SortExpression="ScheduledDate" DataFormatString="{0:dd-MMM-yyyy}" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsVaccinationEvent" runat="server" SelectMethod="GetDueVaccinesForChild" TypeName="GIIS.DataLayer.VaccinationEvent">
                <SelectParameters>
                    <asp:Parameter Name="childId" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnWeight" runat="server" Text="Weight" CssClass="btn btn-sm btn-material-bluegrey btn-raised" OnClick="btnWeight_Click" Visible="false" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAefi" runat="server" Text="AEFI" CssClass="btn btn-material-grey btn-sm btn-raised" OnClick="btnAefi_Click" Visible="false" />

        </div>

        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Button ID="btnImmunizationCard" runat="server" Text="Immunization Card" CssClass="btn btn-material-teal btn-sm btn-raised" OnClick="btnImmunizationCard_Click" />

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ajax__calendar_container
        {
            z-index: 1000;
        }
    </style>
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

</asp:Content>
