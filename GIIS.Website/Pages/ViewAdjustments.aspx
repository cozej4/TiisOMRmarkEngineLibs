<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ViewAdjustments.aspx.cs" Inherits="Pages_ViewAdjustments" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Reports</a></li>
                <li><a href="#">Stock Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="View Adjustments" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblFirstDate" runat="server" Text="First Date" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtFirstDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceFirstDate" runat="server" TargetControlID="txtFirstDate" />
                <asp:RegularExpressionValidator ID="revFirstDate" runat="server" ControlToValidate="txtFirstDate" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"/>

            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblEndDate" runat="server" Text="End Date" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" />
                <ajaxToolkit:CalendarExtender ID="ceEndDate" runat="server" TargetControlID="txtEndDate" />
                <asp:RegularExpressionValidator ID="revEndDate" runat="server" ControlToValidate="txtEndDate" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAdjustmentReason" runat="server" Text="Wastage Reason"></asp:Label>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlAdjustmentReason" runat="server" CssClass="form-control" DataSourceID="odsAdjustmentReason" DataTextField="Name" DataValueField="Id" AutoPostBack="true">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsAdjustmentReason" runat="server" SelectMethod="GetAdjustmentReasonForList" TypeName="GIIS.DataLayer.AdjustmentReason"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:CustomValidator ID="cvDates" runat="server" ErrorMessage="First date cannot be after second date!" OnServerValidate="ValidateDates" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveDispatch" ValidateEmptyText="false"></asp:CustomValidator>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblWarning" runat="server" Text="There are no records for this date interval!" Visible="false" CssClass="label label-warning" Font-Size="Small"></asp:Label>
        </div>
    </div>
    <br />
  
     <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
        <asp:GridView ID="gvTransactionLines" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvTransactionLines_DataBinding" OnDataBound="gvTransactionLines_DataBound" AllowPaging="True" DataSourceID="odsTransactionLines" EnableModelValidation="True" OnPageIndexChanging="gvTransactionLines_PageIndexChanging">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="Health Facility">
                    <ItemTemplate>
                        <%#Eval("Transaction.HealthFacility.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ModifiedOn" HeaderText="Transaction Date" SortExpression="Transaction Date" />
                <asp:TemplateField HeaderText="Item Lot">
                    <ItemTemplate>
                        <%#Eval("ItemLot.LotNumber")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("ItemLot.Item.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                <asp:TemplateField HeaderText="Adjustments Reason">
                    <ItemTemplate>
                        <%#Eval("Adjustment.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsTransactionLines" runat="server" EnablePaging="true" SelectMethod="GetPagedAdjustmentsByDateInterval" TypeName="GIIS.DataLayer.TransactionLines" SelectCountMethod="GetCountAdjustmentsByDateInterval">
            <SelectParameters>
                <asp:Parameter DefaultValue="and 1 = 1" Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
      </div></div>
       <div class="row">
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
        </div>
          <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false" OnClick="btnPrint_Click" CssClass="btn btn-material-bluegrey btn-raised" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnExcel" runat="server" Visible="false" Text="Excel" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
       
    </div>
    <div class="row">
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvTransactionLines_DataBinding" Visible="false">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="Health Facility">
                    <ItemTemplate>
                        <%#Eval("Transaction.HealthFacility.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ModifiedOn" HeaderText="Transaction Date" SortExpression="Transaction Date" />
                <asp:TemplateField HeaderText="Item Lot">
                    <ItemTemplate>
                        <%#Eval("ItemLot.LotNumber")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <%#Eval("ItemLot.Item.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />
                <asp:TemplateField HeaderText="Adjustments Reason">
                    <ItemTemplate>
                        <%#Eval("Adjustment.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
