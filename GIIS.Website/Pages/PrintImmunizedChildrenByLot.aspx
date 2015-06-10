<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintImmunizedChildrenByLot.aspx.cs" Inherits="Pages_PrintImmunizedChildrenByLot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Immunized Children By Lot</title>
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
                        <asp:Label ID="lblTitle" runat="server" Text="Immunized Children By Lot"></asp:Label></b>

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
                            <asp:Label ID="lbLotId" runat="server" Text="Item Lot:"></asp:Label></b>
                        <u>
                            <asp:Label ID="lblLotId" runat="server" Text=" "></asp:Label></u>
                    </td>

                </tr>
                <tr>
                     <td>
                        <b>
                            <asp:Label ID="lbChildNo" runat="server" Text="No. of Vaccinated Children:"></asp:Label></b>
                        <u>
                            <asp:Label ID="lblChildNo" runat="server" Text=" "></asp:Label></u>
                    </td>
                    <td>
                    </td>
                    <td></td>
                </tr>
                               
            </table>
           
            <br />
            <div class="table" style="overflow: auto; font-size: large;" >
                <asp:GridView ID="gvChild" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" RowStyle-BackColor="White" OnDataBinding="gvChild_DataBound" >
                    <RowStyle CssClass="gridviewRow" />
                    <AlternatingRowStyle CssClass="gridviewRowAlt" />
                   <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
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
                <asp:TemplateField HeaderText="Community">
                    <ItemTemplate>
                        <%#Eval("Community.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Domicile">
                    <ItemTemplate>
                        <%#Eval("Domicile.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%#Eval("Status.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="MotherId" HeaderText="MotherId" SortExpression="MotherId" />
                <asp:BoundField DataField="MotherFirstname" HeaderText="MotherFirstname" SortExpression="MotherFirstname" />
                <asp:BoundField DataField="MotherLastname" HeaderText="MotherLastname" SortExpression="MotherLastname" />
                <asp:BoundField DataField="FatherId" HeaderText="FatherId" SortExpression="FatherId" />
                <asp:BoundField DataField="FatherFirstname" HeaderText="FatherFirstname" SortExpression="FatherFirstname" />
                <asp:BoundField DataField="FatherLastname" HeaderText="FatherLastname" SortExpression="FatherLastname" />
                <asp:BoundField DataField="CaretakerId" HeaderText="CaretakerId" SortExpression="CaretakerId" />
                <asp:BoundField DataField="CaretakerFirstname" HeaderText="CaretakerFirstname" SortExpression="CaretakerFirstname" />
                <asp:BoundField DataField="CaretakerLastname" HeaderText="CaretakerLastname" SortExpression="CaretakerLastname" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                <asp:BoundField DataField="IdentificationNo1" HeaderText="IdentificationNo1" SortExpression="IdentificationNo1" />
                <asp:BoundField DataField="IdentificationNo2" HeaderText="IdentificationNo2" SortExpression="IdentificationNo2" />
                <asp:BoundField DataField="IdentificationNo3" HeaderText="IdentificationNo3" SortExpression="IdentificationNo3" />
            </Columns>
                    <HeaderStyle CssClass="gridviewHeader" />
                </asp:GridView>
            </div>
        </div>

    </form>
</body>
</html>


