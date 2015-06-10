<%@ Page Title="Vaccination Report Temporary" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="VaccinationReportTemp.aspx.cs" Inherits="Pages_VaccinationReportTemp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h3>Monthly Register for Children without Barcodes</h3>
    <p>Health Facility:</p>
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

