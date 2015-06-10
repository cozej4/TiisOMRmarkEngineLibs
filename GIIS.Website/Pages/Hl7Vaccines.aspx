<%@ Page Title="Hl7Vaccines" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Hl7Vaccines.aspx.cs" Inherits="_Hl7Vaccines" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="title">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Hl7Vaccines" /></h1>
    </div>
    <br />
    <div style="overflow: auto">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblCvxCode" runat="server" Text="CvxCode"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtCvxCode" runat="server" CssClass="form-control" />
                </td>
                <td></td>
                <td>
                    <asp:Label ID="lblCode" runat="server" Text="Code"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFullname" runat="server" Text="Fullname"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtFullname" runat="server" CssClass="form-control" />
                </td>
                <td></td>
                <td>
                    <asp:Label ID="lblNotes" runat="server" Text="Notes"   />
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVaccineStatus" runat="server" Text="VaccineStatus"   />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblVaccineStatus" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="False" Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td></td>
                <td>
                    <asp:Label ID="lblInternalId" runat="server" Text="InternalId"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtInternalId" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNonVaccine" runat="server" Text="NonVaccine"   />
                </td>
                <td>
                    <asp:RadioButtonList ID="rblNonVaccine" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="False" Text="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td></td>
                <td>
                    <asp:Label ID="lblUpdateDate" runat="server" Text="UpdateDate"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtUpdateDate" runat="server" CssClass="form-control" />
                    <ajaxToolkit:CalendarExtender ID="ceUpdateDate" runat="server" TargetControlID="txtUpdateDate" />
                    <br />
                    <asp:RegularExpressionValidator ID="revUpdateDate" runat="server" ControlToValidate="txtUpdateDate" ValidationGroup="saveHl7Vaccines" Display="Dynamic" ForeColor="Red" />
                </td>
                <td></td>
            </tr>
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" OnClick="btnAdd_Click" ValidationGroup="saveHl7Vaccines" />
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="button" OnClick="btnEdit_Click" ValidationGroup="saveHl7Vaccines" />
                </td>
                <td></td>
            </tr>
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="labelSuccess" Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="labelWarning" Visible="false" />
                </td>
                <td>
                    <asp:Label ID="lblError" runat="server" Text="Error" CssClass="labelError" Visible="false" />
                </td>
                <td>
                    <asp:CustomValidator ID="cvHl7Vaccines" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvHl7Vaccines_Validate" CssClass="labelError" Display="Dynamic" ValidationGroup="saveHl7Vaccines"></asp:CustomValidator>
                </td>
            </tr>
        </table>
    </div>

    <br />
    <div class="tabela" style="overflow: auto">
        <asp:GridView ID="gvHl7Vaccines" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsHl7Vaccines" OnDataBound="gvHl7Vaccines_Databound"  OnPageIndexChanging="gvHl7Vaccines_PageIndexChanging">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="CvxCode" HeaderText="CvxCode" SortExpression="CvxCode" />
                <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                <asp:BoundField DataField="Fullname" HeaderText="Fullname" SortExpression="Fullname" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:CheckBoxField DataField="VaccineStatus" HeaderText="VaccineStatus" SortExpression="VaccineStatus" />
                <asp:BoundField DataField="InternalId" HeaderText="InternalId" SortExpression="InternalId" />
                <asp:CheckBoxField DataField="NonVaccine" HeaderText="NonVaccine" SortExpression="NonVaccine" />
                <asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" SortExpression="UpdateDate" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsHl7Vaccines" runat="server" EnablePaging="true" SelectMethod="GetPagedHl7VaccinesList" TypeName="GIIS.DataLayer.Hl7Vaccines" SelectCountMethod="GetCountHl7VaccinesList">
            <SelectParameters>
                <asp:Parameter DefaultValue="10" Name="maximumRows" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="startRowIndex" Type="Int32" />
                <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />
                
                
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvHl7Vaccines_Validate(sender, args) {
            var text = (document.getElementById('<%=txtCvxCode.ClientID%>').value == '') || (document.getElementById('<%=txtCode.ClientID%>').value == '') || (document.getElementById('<%=txtFullname.ClientID%>').value == '') || (document.getElementById('<%=txtInternalId.ClientID%>').value == '') || (document.getElementById('<%=txtUpdateDate.ClientID%>').value == '');
    if (text) {
        args.IsValid = false;
        return;
    }
    args.IsValid = true;
}
    </script>
</asp:Content>

