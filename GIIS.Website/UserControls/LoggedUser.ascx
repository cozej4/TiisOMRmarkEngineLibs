<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoggedUser.ascx.cs" Inherits="UserControls_LoggedUser" %>
<link type="text/css" rel="stylesheet" href="css/logged_in.css" />
<asp:Panel ID="pnlLoggedId" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--        <div style="border: 1px solid #ccc; -moz-border-radius: 6px 6px 6px 6px; -webkit-border-radius: 6px 6px 6px 6px; border-radius: 6px 6px 6px 6px; margin-top: 30px; margin-left: 3px; padding: 8px; position: relative; width: auto; height: auto; -moz-box-shadow: 0 5px 4px rgba(0, 0, 0, 0.2); -webkit-box-shadow: 0 5px 3px rgba(0, 0, 0, 0.2); box-shadow: 0 5px 3px rgba(0, 0, 0, 0.2)">
            --%>
            <div class="log-in">
                 <div class="row">&nbsp;</div>
                <div class="row">
                    <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                        &nbsp;
                    </div>
                    <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
                        <asp:Label ID="lblUser" runat="server" Text="Logged User:"></asp:Label>
                    &nbsp;&nbsp;
                        <asp:Label ID="lblLoggedUser" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                   
                </div>
                <div class="row">
                   <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                        &nbsp;
                    </div>
                     <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
                        <asp:Label ID="lblHealthFacility" runat="server" Text="Health Facility: "></asp:Label>
                   &nbsp;
                        <asp:Label ID="lblCurrentHealthFacility" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                   
                </div>
                <div class="row">
                    <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                        &nbsp;
                    </div>
                    <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
                        <br />
                        <div class="form-group">
                            <div class="input-group">
                                <span class="glyphicon glyphicon-user"></span>&nbsp;
                            <asp:LinkButton ID="lnkUserProfile" runat="server" OnClick="lnkUserProfile_Click" Text="User Profile"></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                              <span class="glyphicon glyphicon-log-out"></span>&nbsp;
                                <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click" Text="Log out"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
