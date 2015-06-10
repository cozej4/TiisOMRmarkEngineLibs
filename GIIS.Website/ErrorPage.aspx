<%@ Page Title="Error Page" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" style="background: url(img/migrationbg.png) no-repeat;">
        <tr>
            <td rowspan="2" style="border-right: solid 2px #DDE2E6; padding-left: 30px;" align="center">
                <br />
                <h2 style="text-align: center; font-size: 36px; color: #de7812;">Oops...</h2>
                <br />
                <img src="img/error.png" />

            </td>
            <td align="center" valign="top" style="font-size: 14px; color: #de7812; padding-right: 10px;">
                <p>
                    <br />
                    <br />
                    <br />
                    <br />
                    <b>
                        <asp:Label ID="lbl1" runat="server" Text="Something went wrong!"></asp:Label>
                    </b>
                    <br />
                    <br />
                    <br />
                    <b>
                        <asp:Label ID="lbl2" runat="server"
                            Text="Please close your internet browser and try again! "></asp:Label></b>
                </p>
                <br />

                <h2 style="text-align: center; font-size: 36px; color: #de7812;">
                    <asp:Label ID="lbl3" runat="server" Text="Thank you!"></asp:Label>

                </h2>

                <br />
            </td>
        </tr>
    </table>

</asp:Content>

