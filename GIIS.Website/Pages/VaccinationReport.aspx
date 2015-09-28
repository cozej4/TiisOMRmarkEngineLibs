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
<%@ Page Title="Vaccination Report" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="VaccinationReport.aspx.cs" Inherits="Pages_VaccinationReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3>Monthly Register for Children with Barcodes</h3>
    <asp:DropDownList ID="HealthFacilityDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UpdateHyperLink">

    </asp:DropDownList>
    <br />
    <br />
    <p>Month:</p>
    <asp:DropDownList ID="MonthDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UpdateHyperLink">

    </asp:DropDownList>
    <br />
    <br />
    <p>
    <asp:HyperLink ID="DownloadLink" runat="server" 
              NavigateUrl='#' 
              Text='Download Report'
               Target="_blank" >

    </asp:HyperLink>

        </p>

</asp:Content>

