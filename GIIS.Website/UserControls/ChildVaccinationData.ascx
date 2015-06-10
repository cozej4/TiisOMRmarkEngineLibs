<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChildVaccinationData.ascx.cs" Inherits="UserControls_ChildVaccinationData" %>
<div class="tabela" style="overflow: auto">
    <asp:GridView ID="gvVaccinationEvent" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" DataSourceID="odsVaccinationEvent" OnDataBound="gvVaccinationEvent_DataBound">
        <PagerSettings Position="Top" Mode="NumericFirstLast" />
        <RowStyle CssClass="gridviewRow" />
        <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
        <HeaderStyle CssClass="gridviewHeader" />
        <AlternatingRowStyle CssClass="gridviewRowAlt" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
            <asp:TemplateField HeaderText="Vaccine Dose">
                <ItemTemplate>
                       <asp:HyperLink ID="hlDose" runat="server"  NavigateUrl='<%# Eval("Id", "../Pages/RegisterVaccination.aspx?id={0}") %>'
                        Text='<%# Eval("Dose.Fullname", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>' ></asp:HyperLink>
                 <%--   <%# Eval("Dose.Fullname", "{0}") %>--%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Vaccine Lot">
                <ItemTemplate>
                    <%#Eval("VaccineLot")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Health Center">
                <ItemTemplate>
                    <%#Eval("HealthFacility.Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="VaccinationDate" HeaderText="VaccinationDate" SortExpression="VaccinationDate" DataFormatString="{0:dd-MMM-yyyy}" />
            <asp:TemplateField HeaderText="Done">
                <ItemTemplate>
                    <asp:CheckBox ID="chkStatus" runat="server" Enabled="false" Checked='<%# bool.Parse(Eval("VaccinationStatus").ToString()) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="NonVaccination Reason">
                <ItemTemplate>
                    <%#Eval("NonVaccinationReason.Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="NonvaccinationReasonId" HeaderText="NonvaccinationReasonId" SortExpression="NonvaccinationReasonId" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsVaccinationEvent" runat="server" SelectMethod="GetImmunizationCard" TypeName="GIIS.DataLayer.VaccinationEvent">
        <SelectParameters>
            <asp:QueryStringParameter Name="i" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>
