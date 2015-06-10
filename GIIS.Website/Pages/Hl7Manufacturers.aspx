<%@ Page Title="Hl7Manufacturers" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Hl7Manufacturers.aspx.cs" Inherits="_Hl7Manufacturers" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="title">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text="Hl7Manufacturers" /></h1>
    </div>
    <br />
    <div style="overflow: auto">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblMvxCode" runat="server" Text="MvxCode"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtMvxCode" runat="server" CssClass="form-control" />
                </td>
                <td></td>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="Name"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNotes" runat="server" Text="Notes"   />
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
                </td>
                <td></td>
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
            <tr>
                <td>
                    <asp:Label ID="lblLastUpdated" runat="server" Text="LastUpdated"   />
                    <span style="color: Red">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtLastUpdated" runat="server" CssClass="form-control" />
                    <ajaxToolkit:CalendarExtender ID="ceLastUpdated" runat="server" TargetControlID="txtLastUpdated" />
                    <br />
                    <asp:RegularExpressionValidator ID="revLastUpdated" runat="server" ControlToValidate="txtLastUpdated" ValidationGroup="saveHl7Manufacturers" Display="Dynamic" ForeColor="Red" />
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
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" OnClick="btnAdd_Click" ValidationGroup="saveHl7Manufacturers" />
                </td>
                <td>
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="button" OnClick="btnEdit_Click" ValidationGroup="saveHl7Manufacturers" />
                </td>
                <td>
                    <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="button" OnClick="btnRemove_Click" />
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
                    <asp:CustomValidator ID="cvHl7Manufacturers" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvHl7Manufacturers_Validate" CssClass="labelError" Display="Dynamic" ValidationGroup="saveHl7Manufacturers"></asp:CustomValidator>
                </td>
            </tr>
        </table>
    </div>

    <br />
    <div class="tabela" style="overflow: auto">
        <asp:GridView ID="gvHl7Manufacturers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsHl7Manufacturers" OnPageIndexChanging="gvHl7Manufacturers_PageIndexChanging">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <RowStyle CssClass="gridviewRow" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="MvxCode" HeaderText="MvxCode" SortExpression="MvxCode" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="LastUpdated" HeaderText="LastUpdated" SortExpression="LastUpdated" />
                <asp:BoundField DataField="InternalId" HeaderText="InternalId" SortExpression="InternalId" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsHl7Manufacturers" runat="server" EnablePaging="true" SelectMethod="GetPagedHl7ManufacturersList" TypeName="GIIS.DataLayer.Hl7Manufacturers" SelectCountMethod="GetCountHl7ManufacturersList">
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
        function cvHl7Manufacturers_Validate(sender, args) {
            var text = (document.getElementById('<%=txtMvxCode.ClientID%>').value == '') || (document.getElementById('<%=txtName.ClientID%>').value == '') || (document.getElementById('<%=txtLastUpdated.ClientID%>').value == '') || (document.getElementById('<%=txtInternalId.ClientID%>').value == '');
    if (text) {
        args.IsValid = false;
        return;
    }
    args.IsValid = true;
}
    </script>
</asp:Content>

