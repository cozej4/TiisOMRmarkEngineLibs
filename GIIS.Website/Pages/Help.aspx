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
<%@ Page Title="Help" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Help.aspx.cs" Inherits="_Help" ValidateRequest="false" EnableViewState="true" %>

<%@ Register Src="~/UserControls/Menu.ascx" TagName="Menu" TagPrefix="giis" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Configuration</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Help" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <giis:Menu ID="Menu1" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix"></div>
                <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                    <asp:Label ID="lblPageName" runat="server" Text="Page" />
                </div>
                <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
                    <asp:Label ID="lblPage" runat="server" Text="" Font-Bold="true" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix"></div>
                <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                    <asp:Label ID="lblLanguageId" runat="server" Text="Language" />
                </div>
                <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control" AutoPostBack="True" DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged"></asp:DropDownList>
                        <asp:ObjectDataSource ID="odsLanguages" runat="server" SelectMethod="GetLanguageList" TypeName="GIIS.DataLayer.Language"></asp:ObjectDataSource>
                    </div>
                </div>
                <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                    <asp:RequiredFieldValidator ID="rfvHelpText" runat="server" ControlToValidate="txtHelpText" ValidationGroup="saveHelp" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix"></div>
                <div class="col-md-9 col-xs-9 col-sm-9 col-lg-9 clearfix">
                  
                   <ftb:freetextbox ID="txtHelpText" runat="Server" Width="350px"
                        EnableHtmlMode="False" AssemblyResourceHandlerPath=""
                        AutoGenerateToolbarsFromString="True"
                        AutoParseStyles="True" BackColor="158, 190, 245" BaseUrl=""
                        BreakMode="Paragraph" ButtonFileExtention="gif"
                        ButtonFolder="Images" ButtonHeight="20" ButtonImagesLocation="InternalResource"
                        ButtonSet="Office2003" ButtonWidth="21"
                        ClientSideTextChanged="" ConvertHtmlSymbolsToHtmlCodes="False"
                        DesignModeBodyTagCssClass="" DesignModeCss="" DisableIEBackButton="False"
                        DownLevelCols="50" DownLevelMessage="" DownLevelMode="TextArea"
                        DownLevelRows="10" EditorBorderColorDark="128, 128, 128"
                        EditorBorderColorLight="128, 128, 128" EnableSsl="False"
                        EnableToolbars="True" Focus="False" FormatHtmlTagsToXhtml="True"
                        GutterBackColor="129, 169, 226" GutterBorderColorDark="128, 128, 128"
                        GutterBorderColorLight="255, 255, 255" Height="350px"
                        HtmlModeCssClass="" HtmlModeDefaultsToMonoSpaceFont="True"
                        ImageGalleryPath="~/images/"
                        ImageGalleryUrl="ftb.imagegallery.aspx?rif={0}&amp;cif={0}"
                        InstallationErrorMessage="InlineMessage" JavaScriptLocation="InternalResource"
                        Language="en-US" PasteMode="Default" ReadOnly="False"
                        RemoveScriptNameFromBookmarks="True" RemoveServerNameFromUrls="True"
                        RenderMode="NotSet" ScriptMode="External" ShowTagPath="False" SslUrl="/."
                        StartMode="DesignMode" StripAllScripting="False"
                        SupportFolder="/aspnet_client/FreeTextBox/" TabIndex="-1"
                        TabMode="InsertSpaces" Text="" TextDirection="LeftToRight"
                        ToolbarBackColor="Transparent" ToolbarBackgroundImage="True"
                        ToolbarImagesLocation="InternalResource"
                        ToolbarLayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule|Cut,Copy,Paste;Undo,Redo,Print"
                        ToolbarStyleConfiguration="NotSet" UpdateToolbar="True"
                        UseToolbarBackGroundImage="True" />
               
                   
                </div>
                    <span style="color: Red">*</span>
              
            </div>
            <br />
            <div class="row">
                <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
                <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" Visible="false" ValidationGroup="saveHelp" />
                    
                </div>
                <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
            </div>
        </div>
    </div>
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
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
        <asp:GridView ID="gvHelp" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" Visible="false">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="Page">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlSystemId" runat="server" NavigateUrl='<%# Eval("Page", "help.aspx?menuid={0}") %>'
                            Text='<%# Eval("Page", "{0}") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="LanguageId">
                    <ItemTemplate>
                        <%#Eval("Language.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="HelpText" HeaderText="HelpText" SortExpression="HelpText" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
            </Columns>
        </asp:GridView>
        <%--<asp:ObjectDataSource ID="odsHelp" runat="server" EnablePaging="true" SelectMethod="GetPagedHelpList" TypeName="GIIS.DataLayer.Help" SelectCountMethod="GetCountHelpList">
            <SelectParameters>
                <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>--%>
    </div></div> 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

