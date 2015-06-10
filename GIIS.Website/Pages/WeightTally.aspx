<%@ Page Title="Weight Tally" Language="C#"  MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="WeightTally.aspx.cs" Inherits="Pages_WeightTally"  EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="row">
            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                <ol class="breadcrumb">
                    <li><a href="Default.aspx">Home</a></li>
                    <li></li>
                    <li class="active">
                        <asp:Label ID="lblTitle" runat="server" Text="Weigh Tally" />
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
                <p>Month:</p>
                <asp:DropDownList ID="MonthDropdown" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-4">
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                Weight Tally CSV: <asp:FileUpload ID="DocumentUpload" runat="server" />
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
                <a href="../forms/wts.oxps">Download WTS Template</a>
            </div>
        </div>
</asp:Content>

