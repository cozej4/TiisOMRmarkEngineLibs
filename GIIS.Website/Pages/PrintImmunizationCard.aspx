<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintImmunizationCard.aspx.cs" Inherits="Pages_PrintImmunizationCard" %>

<%@ Register Src="~/UserControls/ChildVaccinationData.ascx" TagName="VaccinationData" TagPrefix="giis" %>
<%@ Register Src="~/UserControls/ChildData.ascx" TagName="Child" TagPrefix="giis" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Immunization Card</title>
<link type="text/css" rel="stylesheet" href="../css/style.css" media="print" />
    <link type="text/css" rel="stylesheet" href="../css/ciis.css" media="print" />

    <script type="text/javascript">
        window.print();
    </script>
</head>
<body onload="window.print()">
    <form id="form2" runat="server">
        <div>
            <div class="table" style="overflow: auto;">
            
                <br />
                <div id="title" style="text-align: center; font-size: larger; background-color: white">
                    <h2>
                        <b>
                        <asp:Label ID="lblTitle" runat="server" Text="Immunization Card"></asp:Label></b>
                       
                    </h2>
                </div>
                
                <br />
                    <giis:Child ID="child1" runat="server" />
                <br />
               
     <giis:VaccinationData ID="vaccinations" runat="server"></giis:VaccinationData>
            </div>
        </div>
    </form>
</body>
</html>
