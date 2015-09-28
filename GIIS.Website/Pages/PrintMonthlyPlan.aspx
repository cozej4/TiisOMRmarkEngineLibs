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
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintMonthlyPlan.aspx.cs" Inherits="Pages_PrintMonthlyPlan" %>

<!DOCTYPE html>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Monthly Plan</title>
    <link type="text/css" rel="stylesheet" href="../css/style.css" media="print" />
    <link type="text/css" rel="stylesheet" href="../css/ciis.css" media="print" />
    <style type="text/css">
        @media print
        {
            th
            {
                color: black;
                background-color: white;
            }

            tHead
            {
                display: table-header-group;
            }

            tFoot
            {
                display: table-footer-group;
            }
        }
    </style>

    <script type="text/javascript">
        //this is a very tricky method which is loaded with <body>
        function AddTHEAD(tableName) {
            var table = document.getElementById(tableName);//table name is your gridview name
            if (table != null) {
                var head = document.createElement("THEAD");
                head.style.display = "table-header-group";
                head.appendChild(table.rows[0]);
                table.insertBefore(head, table.childNodes[0]);
            }
        }
        //this method will print the current window
        function CallPrint() {
            // var btn = document.getElementById('btnPrint');//make print button invisible before print
            // btn.style.display = "none";
            window.print();
        }
    </script>

    <%--<script language="javascript" type="text/javascript">
           window.print();
    </script>
    --%>
</head>
<body onload="window.print()">
    <form id="form2" runat="server">
        <div>
            <div class="table" style="overflow: auto;">
                <img id="banner" alt="banner" src="../img/logoiis.jpg" />

                <br />
                <div id="title" style="text-align: center; font-size: larger; background-color: white">
                    <h2>
                        <b>
                        <asp:Label ID="lblTitle" runat="server" Text="Monthly Plan - "></asp:Label></b>
                        <asp:Label ID="lblHealthfacility" runat="server" Text="H.F"></asp:Label>
                        <asp:Label ID="lblMonth" runat="server" Text=" "></asp:Label>
                    </h2>
                </div>
                <br />
                <br />
                <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" />
                
                <asp:GridView ID="gvVaccinationEvent" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" RowStyle-BackColor="White" OnDataBinding="gvVaccinationEvent_DataBound">
                    <RowStyle CssClass="gridviewRow" />
                    <AlternatingRowStyle CssClass="gridviewRowAlt" />
                    <Columns>
                       <asp:BoundField DataField="ID" HeaderText="Id" SortExpression="Id" Visible="False" />
                            <asp:BoundField DataField="APPOINTMENT_ID" HeaderText="Id" SortExpression="Id" Visible="False" />
                            <asp:TemplateField HeaderText="Child">
                                <ItemTemplate>
                                  <%# Eval("NAME", "{0}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vaccines">
                                <ItemTemplate>
                                   <%# Eval("VACCINES", "{0}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SCHEDULE" HeaderText="Schedule" SortExpression="SCHEDULE" />
                            <asp:BoundField DataField="SCHEDULED_DATE" HeaderText="Scheduled Date" SortExpression="SCHEDULED_DATE" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:BoundField DataField="DOMICILE" HeaderText="Village/Domicile" SortExpression="DOMICILE" />

                        <asp:TemplateField HeaderText="Vaccination Date">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lot Number">
                        <ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>

                    </Columns>
                    <HeaderStyle CssClass="gridviewHeader" />
                </asp:GridView>
            </div>
        </div>
        <br />
        <br />
        <div class="table" style="overflow: auto">
                <table cellspacing="10px">
                    <tr>
                        <asp:GridView AutoGenerateColumns="False" ID="gvTotalVaccinesRequired" runat="server">
                            <HeaderStyle CssClass="gridviewHeader" />
                            <AlternatingRowStyle CssClass="gridviewRowAlt" />
                            <RowStyle CssClass="gridviewRow" />

                            <Columns>
                                <asp:TemplateField HeaderText="Item">
                                    <ItemTemplate>
                                        <%#Eval("vaccineCode")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity">
                                    <ItemTemplate>
                                        <%#Eval("quantity")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                    </tr>
                </table>
            </div>

    </form>
</body>
</html>
