<%@ Page Title="WordTranslate" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="WordTranslate.aspx.cs" Inherits="_WordTranslate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Configuration</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Translation" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblLanguageId" runat="server" Text="LanguageId" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlLanguages" runat="server" CssClass="form-control" DataSourceID="odsLanguage" DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlLanguages_SelectedIndexChanged"></asp:DropDownList>
                <asp:ObjectDataSource ID="odsLanguage" runat="server" SelectMethod="GetLanguageList" TypeName="GIIS.DataLayer.Language"></asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblPageName" runat="server" Text="PageName" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlPage" runat="server" DataSourceID="odsPage" DataTextField="PageName" DataValueField="PageName" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged"></asp:DropDownList>
                <asp:ObjectDataSource ID="odsPage" runat="server" SelectMethod="GetAllPages" TypeName="GIIS.DataLayer.WordTranslate"></asp:ObjectDataSource>
            </div>
        </div>
    </div>
   <%-- <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblCode" runat="server" Text="Code" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-raised" OnClick="btnSearch_Click" />
        </div>
    </div>--%>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
        </div>
    </div>
    <br />
    <div class="row">
        <asp:UpdatePanel ID="resultPanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvWordTranslate"  />
            </Triggers>

            <ContentTemplate>
                <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
                    <asp:GridView ID="gvWordTranslate" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnPageIndexChanging="gvWordTranslate_PageIndexChanging" EnableModelValidation="True" OnRowEditing="gvWordTranslate_RowEditing" OnRowUpdating="gvWordTranslate_RowUpdating" OnRowCancelingEdit="gvWordTranslate_RowCancelingEdit">
                        <PagerSettings Position="Top" Mode="NumericFirstLast" />
                        <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                        <Columns>
                            <asp:TemplateField HeaderText="Id" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="txtId" runat="server" Text='<%# Bind("Id") %>' Enabled="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PageCode">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPageCode" runat="server" Text='<%# Bind("Code") %>' Enabled="false"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%# Eval("Code") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="English">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCode" runat="server" Text='<%# Bind("EnglishVersion") %>' Enabled="false"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%# Eval("EnglishVersion") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Translated">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%# Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

