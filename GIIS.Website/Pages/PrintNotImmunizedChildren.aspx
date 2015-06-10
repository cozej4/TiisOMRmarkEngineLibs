<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintNotImmunizedChildren.aspx.cs" Inherits="Pages_PrintNotImmunizedChildren" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Not Immunized Children</title>
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
                        <asp:Label ID="lblTitle" runat="server" Text="Not Vaccinated Children"></asp:Label></b>

                </h2>
            </div>
            <br />

            <table style="width: 100%">
                 <tr>
                     <td>
                        <b>
                            <asp:Label ID="lbHealthFacility" runat="server" Text="Health Facility:"></asp:Label></b>
                        <u>
                            <asp:Label ID="lblHealthFacility" runat="server" Text=" "></asp:Label></u>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <b>
                            <asp:Label ID="lbVaccine" runat="server" Text="Vaccine:"></asp:Label></b>
                        <u>
                            <asp:Label ID="lblVaccine" runat="server" Text=" "></asp:Label></u>
                    </td>

                </tr>
              
                               
            </table>
           
            <br />
            <div class="table" style="overflow: auto; font-size: large;" >
                <asp:GridView ID="gvChild" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" RowStyle-BackColor="White" OnDataBinding="gvChild_DataBound" >
                    <RowStyle CssClass="gridviewRow" />
                    <AlternatingRowStyle CssClass="gridviewRowAlt" />
                   <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                 <asp:BoundField DataField ="Notes" HeaderText="Reason" SortExpression="Notes" />
                <asp:TemplateField HeaderText="SystemID">
                    <ItemTemplate>
                        <%#Eval("Id", "Child.aspx?id={0}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--  <asp:BoundField DataField="SystemId" HeaderText="SystemId" SortExpression="SystemId" />--%>
                <asp:BoundField DataField="Firstname1" HeaderText="Firstname1" SortExpression="Firstname1" />
                <asp:BoundField DataField="Firstname2" HeaderText="Firstname2" SortExpression="Firstname2" />
                <asp:BoundField DataField="Lastname1" HeaderText="Lastname1" SortExpression="Lastname1" />
                <asp:BoundField DataField="Lastname2" HeaderText="Lastname2" SortExpression="Lastname2" />
                <asp:BoundField DataField="Birthdate" HeaderText="Birthdate" SortExpression="Birthdate" />
                <asp:TemplateField HeaderText="Gender">
                    <ItemTemplate>
                        <%# (bool)Eval("Gender") == true? "M" : "F" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:CheckBoxField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />--%>
                <asp:TemplateField HeaderText="Health Center">
                    <ItemTemplate>
                        <%#Eval("Healthcenter.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Birthplace">
                    <ItemTemplate>
                        <%#Eval("Birthplace.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MotherFirstname" HeaderText="MotherFirstname" SortExpression="MotherFirstname" />
                <asp:BoundField DataField="MotherLastname" HeaderText="MotherLastname" SortExpression="MotherLastname" />
                <asp:BoundField DataField="FatherFirstname" HeaderText="FatherFirstname" SortExpression="FatherFirstname" />
                <asp:BoundField DataField="FatherLastname" HeaderText="FatherLastname" SortExpression="FatherLastname" />
               
            </Columns>
                    <HeaderStyle CssClass="gridviewHeader" />
                </asp:GridView>
            </div>
        </div>

    </form>
</body>
</html>


