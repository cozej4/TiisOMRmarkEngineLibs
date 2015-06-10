<%@ Page Title="Monthly Plan" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="MonthlyPlan.aspx.cs" Inherits="Pages_MonthlyPlan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var isresult = "test";
        function pageLoad(sender, args) {

            $find('autocomplteextender1')._onMethodComplete = function (result, context) {

                $find('autocomplteextender1')._update(context, result, /* cacheResults */false);
                webservice_callback1(result, context);
            };
        }
        function webservice_callback1(result, context) {

            if (result == "") {
                $find("autocomplteextender1").get_element().style.backgroundColor = "red";
                isresult = "";
            }
            else {
                $find("autocomplteextender1").get_element().style.backgroundColor = "white";
                isresult = "test";
            }
        }
        function checkHFacility() {
            if (isresult == "") {
                alert("Please choose a health facility from the list!");
                // alert(text);
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Immunization</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Monthly Plan" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix" style="text-align: right">
            <asp:Label ID="lblHealthCenter" runat="server" Text="Health Center"></asp:Label>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc1:AutoCompleteTextbox runat="server"
                    ID="txtHealthcenterId"
                    OnClickSubmit="true"
                    ServiceMethod="SubVaccinationPoints"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" OnClientClick="if (!checkHFacility()) return false;" CssClass="btn btn-primary btn-xs btn-raised" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix" style="text-align: right">
            <asp:ImageButton ID="btnPreviousMonth" runat="server" ImageUrl="~/img/arrow_left_blue.png" OnClick="btnPreviousMonth_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix" style="align-content: center">
            <asp:Label ID="lblActualMonth" Text=" " runat="server" Font-Bold="True"></asp:Label>

        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            &nbsp; &nbsp; &nbsp; 
            <asp:ImageButton ID="btnNextMonth" runat="server" ImageUrl="~/img/arrow_right_blue.png" OnClick="btnNextMonth_Click" />
        </div>

    </div>

    <br />
    <div class="row">
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" CssClass="btn btn-material-bluegrey btn-raised" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Button ID="hlSMSPage" runat="server" Text="Defaulter List" Visible="false" OnClick="hlSMSPage_Click"  CssClass="btn btn-sm btn-material-bluegrey btn-raised"  OnClientClick="document.forms[0].target = '_blank'"></asp:Button>
        </div>

    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvHealthFacility" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsHealthFacility" OnPageIndexChanging="gvHealthFacility_PageIndexChanging" OnDataBound="gvHealthFacility_DataBound">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" Visible ="false" />
                    <asp:TemplateField HeaderText="ParentId">
                        <ItemTemplate>
                            <%#Eval("Parent.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View Monthly Plan">
                        <ItemTemplate>
                            <a href='<%# Eval("Id", "MonthlyPlan.aspx?hfId={0}&find=1") %>' target="_blank">
                                <img alt='View Monthly Plan' src="../img/DispatchLines.png" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsHealthFacility" runat="server" EnablePaging="true" SelectMethod="GetPagedHealthFacilityList" TypeName="GIIS.DataLayer.HealthFacility" SelectCountMethod="GetCountHealthFacilityList">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>


    <asp:UpdatePanel ID="resultPanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtHealthcenterId" EventName="ValueSelected" />
        </Triggers>

        <ContentTemplate>
            <div class="row">
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                    <asp:GridView ID="gvVaccinationEvent" runat="server"
                        AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive"
                        DataSourceID="odsVaccinationEvent" OnDataBound="gvVaccinationEvent_DataBound" OnPageIndexChanging="gvVaccinationEvent_PageIndexChanging"
                        EnableModelValidation="True" AllowPaging="true" PageSize="30">
                        <PagerSettings Position="Top" Mode="NumericFirstLast" />
                        <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="Id" SortExpression="Id" Visible="False" />
                            <asp:BoundField DataField="APPOINTMENT_ID" HeaderText="Id" SortExpression="Id" Visible="False" />
                            <asp:TemplateField HeaderText="Child">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlChild" runat="server" NavigateUrl='<%# Eval("ID", "Child.aspx?id={0}") %>'
                                        Text='<%# Eval("NAME", "{0}") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vaccines">
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlDose" runat="server"
                                        NavigateUrl='<%# Eval("APPOINTMENT_ID", "RegisterVaccination.aspx?appId={0}") %>'
                                        Text='<%# Eval("VACCINES", "{0}") %>' Target="_blank"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SCHEDULE" HeaderText="Schedule" SortExpression="SCHEDULE" />
                            <asp:BoundField DataField="SCHEDULED_DATE" HeaderText="Scheduled Date" SortExpression="SCHEDULED_DATE" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:BoundField DataField="DOMICILE" HeaderText="Village/Domicile" SortExpression="DOMICILE" />
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsVaccinationEvent" runat="server" EnablePaging="true" SelectCountMethod="GetCountMonthlyPlan"
                        SelectMethod="GetMonthlyPlan" TypeName="GIIS.DataLayer.VaccinationEvent">
                        <SelectParameters>
                            <asp:Parameter Name="hfId" Type="Int32" />
                            <asp:Parameter Name="scheduledDate" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>

            <br />
            <br />
            <div class="row">
                <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
                    <asp:GridView AutoGenerateColumns="False" ID="gvTotalVaccinesRequired" runat="server" CssClass="table table-striped table-bordered table-hover table-responsive">
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
                    <asp:ObjectDataSource ID="odsVaccineQty" runat="server" SelectMethod="GetQuantityMonthlyPlan" TypeName="GIIS.DataLayer.VaccineQuantity">
                        <SelectParameters>
                            <asp:Parameter Name="hfId" Type="String" />
                            <asp:Parameter Name="scheduledDate" Type="DateTime" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    </tr>
                </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

