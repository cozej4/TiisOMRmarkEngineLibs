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
<%@ Page Title="Language" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Language.aspx.cs" Inherits="_Language" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Configuration</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Languages" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblName" runat="server" Text="Name" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAbbrevation" runat="server" Text="Abbrevation" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtAbbrevation" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblWritingDirection" runat="server" Text="WritingDirection" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtWritingDirection" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblNameEnglish" runat="server" Text="NameEnglish" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtNameEnglish" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblNotes" runat="server" Text="Notes" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblIsActive" runat="server" Text="IsActive" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" ValidationGroup="saveLanguage" />
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" ValidationGroup="saveLanguage" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            
            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-raised btn-warning" OnClick="btnRemove_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix"></div>
        <div class="col-md-7 col-xs-7 col-sm-7 col-lg-7 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
            <asp:CustomValidator ID="cvLanguage" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvLanguage_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveLanguage"></asp:CustomValidator>
        </div>
    </div>
    <br />
  <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
        <asp:GridView ID="gvLanguage" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnPageIndexChanging="gvLanguage_PageIndexChanging">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "Language.aspx?id={0}") %>'
                            Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Abbrevation" HeaderText="Abbrevation" SortExpression="Abbrevation" />
                <asp:BoundField DataField="WritingDirection" HeaderText="WritingDirection" SortExpression="WritingDirection" />
                <asp:BoundField DataField="NameEnglish" HeaderText="NameEnglish" SortExpression="NameEnglish" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />

            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsLanguage" runat="server" EnablePaging="true" SelectMethod="GetPagedLanguageList" TypeName="GIIS.DataLayer.Language" SelectCountMethod="GetCountLanguageList">
            <SelectParameters>
                <asp:Parameter DefaultValue="1 = 1" Name="where" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div></div> 
    <div class="row">
        <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            <asp:Button ID="btnExcel" runat="server" Text="Excel" Visible="false" CssClass="btn btn-success btn-raised" OnClick="btnExcel_Click" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
    </div>
    <div class="row" style="overflow: auto">
        <asp:GridView ID="gvExport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBinding="gvLanguage_DataBound" Visible="false">
            <RowStyle CssClass="gridviewRow" />
            <HeaderStyle CssClass="gridviewHeader" />
            <AlternatingRowStyle CssClass="gridviewRowAlt" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Abbrevation" HeaderText="Abbrevation" SortExpression="Abbrevation" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                <asp:BoundField DataField="WritingDirection" HeaderText="WritingDirection" SortExpression="WritingDirection" />
                <asp:BoundField DataField="NameEnglish" HeaderText="NameEnglish" SortExpression="NameEnglish" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvLanguage_Validate(sender, args) {
            var text = (document.getElementById('<%=txtName.ClientID%>').value == '') || (document.getElementById('<%=txtAbbrevation.ClientID%>').value == '') || (document.getElementById('<%=txtWritingDirection.ClientID%>').value == '');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
    </script>
</asp:Content>

