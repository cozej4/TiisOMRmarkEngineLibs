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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChildData.ascx.cs" Inherits="UserControls_ChildData" %>
<asp:DataList ID="DataList1" runat="server" DataSourceID="odsChild" Width="100%" OnItemDataBound="DataList1_ItemDataBound">
    <ItemTemplate>
        <table>
            <tr>
                <td>&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbSystemID" runat="server" Text="SystemID:"></asp:Label>
                </td>
                <td>
                    <div style="float: left">
                         <asp:LinkButton ID="lnkChild" runat="server" Text='<%# Eval("SystemId") %>' Font-Bold="true" PostBackUrl='<%# Eval("Id", "/Pages/Child.aspx?id={0}") %>'></asp:LinkButton>
                      
                    </div>
                </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;
                </td>

                <td>

                    <asp:Label ID="lbIdentificationNo1" runat="server" Text="Identification No1:"></asp:Label>
                </td>
                <td>
                    <div style="float: left">
                        <b>
                            <asp:Label ID="lblIdentificationNo1" runat="server" Text='<%# Eval("IdentificationNo1") %>' /></b>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbFirstName" runat="server" Text="FirstName: "></asp:Label>
                </td>
                <td>
                    <div style="float: left">
                        <b>
                            <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("Firstname1") + " " + Eval("Firstname2") %>' /></b>
                    </div>
                </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbLastName" runat="server" Text="LastName:"></asp:Label>
                </td>
                <td>
                    <div style="float: left">
                        <b>
                            <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("Lastname1") + " " + Eval("Lastname2") %>' /></b>
                    </div>
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbBirthdate" runat="server" Text="Birthdate:"></asp:Label>
                </td>
                <td>
                    <b>
                        <asp:Label ID="lblBirthdate" runat="server" Text='<%# Eval("BirthDate", "{0:dd/MM/yyyy}") %>' /></b>
                </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbBirthplace" runat="server" Text="Birthplace:"></asp:Label>
                </td>
                <td>
                    <b>
                        <asp:Label ID="lblBirthplace" runat="server" Text='<%# Eval("BirthPlace.Name") %>' /></b>
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbGender" runat="server" Text="Gender:"></asp:Label>
                </td>
                <td>
                    <b>
                        <asp:Label ID="lblGender" runat="server" Text='<%# (bool)Eval("Gender") == true? "M" : "F" %>' /></b>
                </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbHealthFacility" runat="server" Text="Health Center:"></asp:Label>
                </td>
                <td>
                    <b>
                        <asp:Label ID="lblHealthFacility" runat="server" Text='<%# Eval("Healthcenter.Name") %>' /></b>
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;&nbsp;
                </td>
                <td>

                    <asp:Label ID="lbMotherName" runat="server" Text="Mother:"></asp:Label>
                </td>
                <td>
                    <b>
                        <asp:Label ID="lblMotherName" runat="server" Text='<%# Eval("MotherFirstname") + " " + Eval("MotherLastname") %>' /></b>
                </td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
               
                </tr>
            
          
        </table>
    </ItemTemplate>

</asp:DataList>
<asp:ObjectDataSource ID="odsChild" runat="server" SelectMethod="GetChildById" TypeName="GIIS.DataLayer.Child"
    OldValuesParameterFormatString="original_{0}">
    <SelectParameters>
        <asp:QueryStringParameter Name="i" QueryStringField="id" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
