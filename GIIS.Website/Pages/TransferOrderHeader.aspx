<%@ Page Title="TransferOrderHeader" Language="C#" AutoEventWireup="true" MasterPageFile="~/Pages/MasterPage.master" CodeFile="TransferOrderHeader.aspx.cs" Inherits="_TransferOrderHeader" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Stock Issue</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Header" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderFacilityFrom" runat="server" Text="Order Facility From" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <%--<asp:TextBox ID="txtOrderFacilityFrom" runat="server" CssClass="form-control" />--%>
                <%--<uc1:AutoCompleteTextbox runat="server"
                    ID="txtOrderFacilityFrom"
                    OnClickSubmit="true"
                    ServiceMethod="HealthCenters"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="OrderFacilityFrom_ValueSelected" />--%>
                <asp:DropDownList ID="ddlHealthFacilityFrom" CssClass="form-control" runat="server" AutoPostBack="True" DataTextField="Name" DataValueField="Code" OnSelectedIndexChanged="ddlHealthFacilityFrom_SelectedIndexChanged"></asp:DropDownList>

            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderFacilityTo" runat="server" Text="Order Facility To" />
            <!--<span id="spanLastname1" runat="server" style="color: Red">*</span>-->
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <%--<asp:TextBox ID="txtOrderFacilityTo" runat="server" CssClass="form-control" />--%>
                <%--<uc1:AutoCompleteTextbox runat="server"
                    ID="txtOrderFacilityTo"
                    OnClickSubmit="true"
                    ServiceMethod="HealthCenters"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="OrderFacilityTo_ValueSelected" />--%>
                <asp:DropDownList ID="ddlHealthFacilityTo" CssClass="form-control" runat="server" AutoPostBack="True" DataTextField="Name" DataValueField="Code"></asp:DropDownList>

            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>

        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderSchedReplenishDate" runat="server" Text="Order Replenish Date" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtOrderSchedReplenishDate" runat="server" CssClass="form-control" ValidationGroup="saveTrasferOrderHeader" />
                <ajaxToolkit:CalendarExtender ID="ceOrderSchedReplenishDate" runat="server" TargetControlID="txtOrderSchedReplenishDate" />
                <asp:RegularExpressionValidator ID="revOrderSchedReplenishDate" runat="server" ControlToValidate="txtOrderSchedReplenishDate" ValidationGroup="saveTrasferOrderHeader" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
            </div>
        </div>

        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOrderCarrier" runat="server" Text="Order Carrier"  Visible="false" />
            <!--<span id="spanMotherLastname" runat="server" style="color: Red">*</span>-->
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtOrderCarrier" runat="server" CssClass="form-control" Visible="false"/>
            </div>
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
        </div>
        <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
            <asp:CustomValidator ID="cvOrderSchedReplenishDate" runat="server" ErrorMessage="Order Scheduled Replenishment Date cannot be after today!" ForeColor="White" OnServerValidate="ValidateOrderSchedReplenishDate" CssClass="label label-danger" Font-Size="Small" Display="Dynamic" ValidationGroup="saveTrasferOrderHeader" ValidateEmptyText="false"></asp:CustomValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveTrasferOrderHeader" />
            <asp:Button ID="btnAddDetails" runat="server" Text="Add Details" CssClass="btn btn-primary btn-raised" OnClick="btnAddDetails_Click" />

        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="The action completed succesfully!" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
