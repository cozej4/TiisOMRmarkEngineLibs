<%@ Page Title="Child Weight" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="ChildWeight.aspx.cs" Inherits="_ChildWeight" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Child</a></li>
                <li class="active">Child Weight</li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-1 col-xs-2 col-sm-1 col-lg-1 clearfix">
            <asp:Label ID="lbChild" runat="server" Text="Child:"></asp:Label>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
              <asp:LinkButton ID="lnkChild" runat="server" Font-Bold="true"></asp:LinkButton>
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix" style="text-align: right">
            <asp:Label ID="lbChildBirthdate" runat="server" Text="Birthdate:"></asp:Label>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:Label ID="lblChildBirthdate" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            <asp:Label ID="lbDate" runat="server" Text="Date:" />
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblDate" runat="server" Font-Bold="true" />
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix" style="text-align: right">
            <asp:Label ID="lblWeight" runat="server" Text="Weight" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Save" CssClass="btn btn-primary btn-sm btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveChildWeight" />
            <asp:Button ID="btnEdit" runat="server" Text="Save" CssClass="btn btn-primary btn-sm btn-raised" OnClick="btnEdit_Click" ValidationGroup="saveChildWeight" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix"></div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:RegularExpressionValidator ID="revWeight" runat="server" ControlToValidate="txtWeight" ForeColor="White" Display="Dynamic"
                ErrorMessage="The weight is not valid!" ValidationGroup="saveChildWeight" CssClass="label label-danger" Font-Size="Small" ValidationExpression="^[0-9.]*$"> </asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="rfvWeight" runat="server" ErrorMessage="Weight must be filled!" ControlToValidate="txtWeight" ValidationGroup="saveChildWeight" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:Label ID="lblSuccess" runat="server" CssClass="label label-success" Font-Size="Small" ForeColor="White" Text="Success" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Text="Warning" Visible="false" />
            <asp:Label ID="lblError" runat="server" CssClass="label label-danger" Font-Size="Small" ForeColor="White" Text="Error" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
      <%--  <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>--%>
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:Label ID="lblWeightMessage" runat="server" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Visible="false" />
        </div>
    </div>
    <br />
      <div class="row">
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
          
        </div>
         <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix"></div>        
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
              <asp:Button ID="btnSupplement" runat="server" Text="Supplements" CssClass="btn btn-sm btn-material-bluegrey btn-raised" 
                OnClick="btnSupplement_Click" Visible ="false"/>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-11 col-xs-11 col-sm-11 col-lg-11 clearfix">
            <asp:GridView ID="gvChildWeight" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" DataSourceID="odsChildWeight" OnPageIndexChanging="gvChildWeight_PageIndexChanging">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="ChildId" HeaderText="ChildId" SortExpression="ChildId" Visible="False" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:dd-MMMM-yyyy}" />
                    <asp:BoundField DataField="Weight" HeaderText="Weight (kg)" SortExpression="Weight" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsChildWeight" runat="server" EnablePaging="true" SelectMethod="GetPagedChildWeightList" TypeName="GIIS.DataLayer.ChildWeight" SelectCountMethod="GetCountChildWeightList">
                <SelectParameters>
                    <asp:QueryStringParameter Name="id" QueryStringField="id" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>

  

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

