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
