<%@ Page Title="Search Children" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ChildListNew.aspx.cs" Inherits="Pages_ChildListNew" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Child</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Search Children" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <div class="row">
                <asp:Panel ID="pnlSystemId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblSystemId" runat="server" Text="System ID" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtSystemId" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="pnlBarcode" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBarcode" runat="server" Text="Barcode" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel runat="server" ID="trFirstname1">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblFirstname1" runat="server" Text="Firstname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtFirstname1" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel runat="server" ID="pnlMotherFirstname">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMotherFirstname" runat="server" Text="Mother Firstname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMotherFirstname" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div> 
            <div class="row">
                <asp:Panel ID="trBirthdateFrom" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthdateFrom" runat="server" Text="Birthdate From:" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBirthdateFrom" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceBirthdateFrom" runat="server" TargetControlID="txtBirthdateFrom" />
                            <asp:RegularExpressionValidator ID="revBirthdateFrom" runat="server" ControlToValidate="txtBirthdateFrom" ValidationGroup="saveChild" Display="Dynamic" CssClass="label label-danger" Font-Size ="Small" ForeColor="White"  />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="pnlHealthcenter" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix" >
                        <asp:Label ID="lblHealthcenterId" runat="server" Text="HealthcenterId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group"">
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
                <asp:Panel ID="trDomicileId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblDomicileId" runat="server" Text="DomicileId" />
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
           <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
            <div class="row">
                <asp:Panel ID="pnlIdFields" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblIdentification" runat="server" Text="ID Fields" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtIdFields" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="pnlTempId" runat="server">
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
                <asp:Panel runat="server" ID="trLastname1">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblLastname1" runat="server" Text="Lastname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtLastname1" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel runat="server" ID="pnlMotherLastname">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblMotherLastname" runat="server" Text="Mother Lastname" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtMotherLastname" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trBirthdateTo" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthdateTo" runat="server" Text="Birthdate To:" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <asp:TextBox ID="txtBirthdateTo" runat="server" CssClass="form-control" />
                            <ajaxToolkit:CalendarExtender ID="ceBirthdateTo" runat="server" TargetControlID="txtBirthdateTo" />
                            <asp:RegularExpressionValidator ID="revBirthdateTo" runat="server" ControlToValidate="txtBirthdateTo" ValidationGroup="saveChild" Display="Dynamic" CssClass="label label-danger" Font-Size ="Small" ForeColor="White"  />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trBirthplaceId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblBirthplaceId" runat="server" Text="BirthplaceId" />
                    </div>
                    <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
                        <div class="form-group">
                            <%--<uc3:BirthplacesTextbox ID="txtBirthplaceId" runat="server" ClearAfterSelect="false" HighlightResults="true" OnClickSubmit="true" OnValueSelected="Places_ValueSelected" ServiceMethod="BirthPlace" />--%>
                            <asp:DropDownList ID="ddlBirthplace" runat="server" DataSourceID="odsBirthplace" DataTextField="Name" DataValueField="Id" CssClass="form-control" />
                            <asp:ObjectDataSource ID="odsBirthplace" runat="server" SelectMethod="GetBirthplaceListNew" TypeName="GIIS.DataLayer.Birthplace"></asp:ObjectDataSource>
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div class="row">
                <asp:Panel ID="trCommunityId" runat="server">
                    <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                        <asp:Label ID="lblCommunityId" runat="server" Text="CommunityId" />
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
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
        </div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:CustomValidator ID="cvBirthdate" runat="server" ErrorMessage="First date cannot be after second date!" OnServerValidate="ValidateBirthdate" CssClass="label label-warning" Font-Size="Small" Display="Dynamic" ForeColor="White" ValidationGroup="saveChild" ValidateEmptyText="false"></asp:CustomValidator>
            <asp:Label ID="lblWarning" runat="server" Text="There are no children that match your search criteria!" CssClass="label label-warning" Font-Size="Small" Visible="false" />

        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveChild" />
        </div>
    </div>
    <br />

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
     <style type="text/css">
         .ajax__calendar_container {
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

