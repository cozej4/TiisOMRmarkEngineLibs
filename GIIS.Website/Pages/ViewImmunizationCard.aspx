<%@ Page Title="View Immunization Card" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ViewImmunizationCard.aspx.cs" Inherits="Pages_ViewImmunizationCard" %>

<%@ Register Src="~/UserControls/ChildData.ascx" TagName="Child" TagPrefix="giis" %>
<%@ Register Src="~/UserControls/ChildVaccinationData.ascx" TagName="VaccinationData" TagPrefix="giis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Immunization</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="View Immunization Card" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <giis:Child ID="child1" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" CssClass="btn btn-material-bluegrey btn-raised" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <giis:VaccinationData ID="vaccinations" runat="server"></giis:VaccinationData>
        </div>
    </div>
</asp:Content>

