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
<%@ Page Title="Weight Tally" Language="C#"  MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="StockTally.aspx.cs" Inherits="Pages_WeightTally"  EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="row">
            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                <ol class="breadcrumb">
                    <li><a href="Default.aspx">Home</a></li>
                    <li></li>
                    <li class="active">
                        <asp:Label ID="lblTitle" runat="server" Text="Stock Tally" />
                    </li>
                </ol>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <p>Health Facility:</p>
                <asp:DropDownList ID="HealthFacilityDropdown" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-4">
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                Stock Tally CSV: <asp:FileUpload ID="DocumentUpload" runat="server" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="UploadButton" runat="server" Text="Submit"/>
                <asp:Label ID="status" Visible="false" CssClass="label" runat="server"></asp:Label>
            </div>
        </div>
    <div class="row">
        <div class="col-md-12">
            <a href="../forms/sts.pdf">Download STS Template</a>
        </div>
    </div>
</asp:Content>

