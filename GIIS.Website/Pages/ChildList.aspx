<%@ Page Title="Child List" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ChildList.aspx.cs" Inherits="_ChildList" %>

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
    <div style="overflow: auto">
        <table style="width: 100%">
            <tr>
                <td style="vertical-align: top; width: 47%;">
                    <%--                    <asp:Panel ID="pnlBasic" runat="server" GroupingText="Basic" >--%>
                    <table style="width: 100%" class="panel">
                        <tr>
                            <td>
                                <asp:Label ID="lblSystemId" runat="server" Text="SystemId"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtSystemId" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trFirstname1" runat="server">
                            <td>
                                <asp:Label ID="lblFirstname1" runat="server" Text="Firstname1"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstname1" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trLastname1" runat="server">
                            <td>
                                <asp:Label ID="lblLastname1" runat="server" Text="Lastname1"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastname1" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trFirstname2" runat="server">
                            <td>
                                <asp:Label ID="lblFirstname2" runat="server" Text="Firstname2"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFirstname2" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trLastname2" runat="server">
                            <td>
                                <asp:Label ID="lblLastname2" runat="server" Text="Lastname2"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastname2" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBirthdateFrom" runat="server" Text="Birthdate From:"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthdateFrom" runat="server" CssClass="form-control" />
                                <ajaxToolkit:CalendarExtender ID="ceBirthdateFrom" runat="server" TargetControlID="txtBirthdateFrom" />
                                <br />
                                <asp:RegularExpressionValidator ID="revBirthdateFrom" runat="server" ControlToValidate="txtBirthdateFrom" ValidationGroup="saveChild" Display="Dynamic" ForeColor="Red" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBirthdateTo" runat="server" Text="Birthdate To:"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthdateTo" runat="server" CssClass="form-control" />
                                <ajaxToolkit:CalendarExtender ID="ceBirthdateTo" runat="server" TargetControlID="txtBirthdateTo" />
                                <br />
                                <asp:RegularExpressionValidator ID="revBirthdateTo" runat="server" ControlToValidate="txtBirthdateTo" ValidationGroup="saveChild" Display="Dynamic" ForeColor="Red" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblHealthcenterId" runat="server" Text="HealthcenterId"   />
                            </td>
                            <td>
                                <uc1:AutoCompleteTextbox runat="server"
                                    ID="txtHealthcenterId"
                                    OnClickSubmit="true"
                                    ServiceMethod="VaccinationPoints"
                                    HighlightResults="true"
                                    ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trIdentificationNo1" runat="server">
                            <td>
                                <asp:Label ID="lblIdentificationNo1" runat="server" Text="IdentificationNo1"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentificationNo1" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trIdentificationNo2" runat="server">
                            <td>
                                <asp:Label ID="lblIdentificationNo2" runat="server" Text="IdentificationNo2"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentificationNo2" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trIdentificationNo3" runat="server">
                            <td>
                                <asp:Label ID="lblIdentificationNo3" runat="server" Text="IdentificationNo3"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentificationNo3" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblStatusId" runat="server" Text="StatusId"   />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" DataSourceID="odsStatus" DataTextField="Name" DataValueField="Id" CssClass="form-control" />
                                <asp:ObjectDataSource ID="odsStatus" runat="server" SelectMethod="GetStatusForList" TypeName="GIIS.DataLayer.Status"></asp:ObjectDataSource>
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trIsActive" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblIsActive" runat="server" Text="IsActive"   />
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td></td>
                        </tr>

                    </table>
                    <br />
                    <%-- </asp:Panel>--%>
                </td>
                <td style="vertical-align: top; width: 53%;">
                    <%--<asp:Panel ID="pnlAdditional" runat="server" GroupingText="Additional">--%>
                    <table style="width: 100%; empty-cells: hide" class="panel">

                        <tr id="trBirthplaceId" runat="server">
                            <td>
                                <asp:Label ID="lblBirthplaceId" runat="server" Text="BirthplaceId"   />
                            </td>
                            <td>
                                <uc3:BirthplacesTextbox ID="txtBirthplaceId"
                                    runat="server" ClearAfterSelect="false"
                                    HighlightResults="true" OnClickSubmit="true"
                                    OnValueSelected="Places_ValueSelected"
                                    ServiceMethod="BirthPlace" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trCommunityId" runat="server">
                            <td>
                                <asp:Label ID="lblCommunityId" runat="server" Text="CommunityId"   />
                            </td>
                            <td>
                                <uc2:CommunitiesTextbox ID="txtCommunityId"
                                    runat="server" ClearAfterSelect="false"
                                    HighlightResults="true" OnClickSubmit="true"
                                    OnValueSelected="Communities_ValueSelected"
                                    ServiceMethod="Community" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trDomicileId" runat="server">
                            <td>
                                <asp:Label ID="lblDomicileId" runat="server" Text="DomicileId"   />
                            </td>
                            <td>
                                <uc4:DomicileTextbox ID="txtDomicileId"
                                    runat="server" ClearAfterSelect="false"
                                    HighlightResults="true" OnClickSubmit="true"
                                    OnValueSelected="Domicile_ValueSelected"
                                    ServiceMethod="Place" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trAddress" runat="server">
                            <td>
                                <asp:Label ID="lblAddress" runat="server" Text="Address"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trPhone" runat="server">
                            <td>
                                <asp:Label ID="lblPhone" runat="server" Text="Phone"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trMobile" runat="server">
                            <td>
                                <asp:Label ID="lblMobile" runat="server" Text="Mobile"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trEmail" runat="server">
                            <td>
                                <asp:Label ID="lblEmail" runat="server" Text="Email"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trMotherId" runat="server">
                            <td>
                                <asp:Label ID="lblMotherId" runat="server" Text="MotherId"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherId" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trMotherFirstname" runat="server">
                            <td>
                                <asp:Label ID="lblMotherFirstname" runat="server" Text="MotherFirstname"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherFirstname" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trMotherLastname" runat="server">
                            <td>
                                <asp:Label ID="lblMotherLastname" runat="server" Text="MotherLastname"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherLastname" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trFatherId" runat="server">
                            <td>
                                <asp:Label ID="lblFatherId" runat="server" Text="FatherId"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherId" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trFatherFirstname" runat="server">
                            <td>
                                <asp:Label ID="lblFatherFirstname" runat="server" Text="FatherFirstname"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherFirstname" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trFatherLastname" runat="server">
                            <td>
                                <asp:Label ID="lblFatherLastname" runat="server" Text="FatherLastname"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherLastname" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trCaretakerId" runat="server">
                            <td>
                                <asp:Label ID="lblCaretakerId" runat="server" Text="CaretakerId"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCaretakerId" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trCaretakerFirstname" runat="server">
                            <td>
                                <asp:Label ID="lblCaretakerFirstname" runat="server" Text="CaretakerFirstname"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCaretakerFirstname" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trCaretakerLastname" runat="server">
                            <td>
                                <asp:Label ID="lblCaretakerLastname" runat="server" Text="CaretakerLastname"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCaretakerLastname" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                        <tr id="trNotes" runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblNotes" runat="server" Text="Notes"   />
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <%-- </asp:Panel>--%>
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" ValidationGroup="saveChild" />
                </td>
            </tr>
        </table>
        <br />
         <table style="width: 100%">
            <tr>
                <%--<td>
                    <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="labelSuccess" Visible="false" />
                </td>--%>
                <td>
                    <asp:Label ID="lblWarning" runat="server" Text="There are no child that match your search criteria!" CssClass="labelWarning" Visible="false" />
                </td>
                <%--<td>
                    <asp:Label ID="lblError" runat="server" Text="Error" CssClass="labelError" Visible="false" />
                </td>
                <td></td>--%>
            </tr>
        </table>
    </div>
    <div class="tabela" style="overflow: auto">
        <asp:GridView ID="gvChild" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnDataBound="gvChild_DataBound" OnPageIndexChanging="gvChild_PageIndexChanging">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="SystemID">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlSystemId" runat="server" NavigateUrl='<%# Eval("Id", "Child.aspx?id={0}") %>'
                            Text='<%# Eval("SystemId", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
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
        </asp:GridView>
        <asp:ObjectDataSource ID="odsChild" runat="server" EnablePaging="true" SelectMethod="GetPagedChildList" TypeName="GIIS.DataLayer.Child" SelectCountMethod="GetCountChildList"></asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
</asp:Content>
