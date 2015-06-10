<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintVaccinationActivityReport.aspx.cs" Inherits="Pages_PrintVaccinationActivityReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>View Lot Tracking</title>
    <link type="text/css" rel="stylesheet" href="../css/controls_style.css" media="print" />


    <script type="text/javascript">
        window.print();
    </script>
</head>
<body onload="window.print()">
    <form id="form2" runat="server">
        <div style="overflow: auto; font-size: large;">
            <img id="banner" alt="banner" src="../img/logoiis.jpg" />
            <br />
            <br />
            <div id="Div1" style="text-align: center; font-size: larger; background-color: white">
                <h2>
                    <b>
                        <asp:Label ID="lblTitle" runat="server" Text="View Vaccination Activity Report"></asp:Label></b>

                </h2>
            </div>
            <br />

            <table style="width: 100%">
                 <tr>
                     <td>
                        <b>
                            <asp:Label ID="lbYear" runat="server" Text="Year:"></asp:Label></b>
                        
                            <asp:Label ID="lblYear" runat="server" Text=" "></asp:Label>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                <td>
                        <b>
                            <asp:Label ID="lbItem" runat="server" Text="Item:"></asp:Label></b>
                        
                            <asp:Label ID="lblItem" runat="server" Text=" "></asp:Label>
                    </td>
                </tr>
                               
            </table>
           
            <br />
            <div class="table"  font-size: large;" >
                <asp:GridView ID="gvReport" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive" AutoGenerateColumns="true" OnDataBound="gvReport_DataBound">
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
        </asp:GridView>
            </div>
            <br />
        <asp:Literal ID="iActivityReport" runat="server" />
        </div>

    </form>
</body>
</html>

