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
<%@ Page Title="Place" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Place.aspx.cs" Inherits="_Place" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Setup</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Place" /></li>
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
            <asp:Label ID="lblCode" runat="server" Text="Code" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblParentId" runat="server" Text="ParentId" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc3:BirthplacesTextbox ID="txtParentId"
                    runat="server" ClearAfterSelect="false"
                    HighlightResults="true" OnClickSubmit="true"
                    OnValueSelected="Places_ValueSelected"
                    ServiceMethod="ParentPlaces" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblLeaf" runat="server" Text="Leaf" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblLeaf" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblLeaf_SelectedIndexChanged">
                    <asp:ListItem Value="True" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
        </div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHealthFacility" runat="server" Text="Health Facility" Visible ="false" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <%-- <uc1:AutoCompleteTextbox runat="server" 
                    ID="txtHealthcenterId"
                    OnClickSubmit="true"
                    ServiceMethod="SubVaccinationPoints"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />--%>
                <asp:DropDownList ID="ddlHealthFacility" runat="server" CssClass="form-control" Visible="false" DataSourceID="odsHealthF" DataTextField="Name" DataValueField="Id">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsHealthF" runat="server" SelectMethod="GetVaccinationPointList" TypeName="GIIS.DataLayer.HealthFacility">
                      <SelectParameters>
                        <asp:Parameter Name="ids" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>

            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblIsActive" runat="server" Text="IsActive" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="True" Text="Yes&nbsp;&nbsp;&nbsp;"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:Label ID="revPlace" runat="server" CssClass="label label-warning" Font-Size="Small" Text=" The place you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
            <asp:CustomValidator ID="cvPlace" runat="server" ErrorMessage=" All fields marked with * must be filled!" ClientValidationFunction="cvPlace_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="savePlace"></asp:CustomValidator>
            <asp:RegularExpressionValidator ID="revName" runat="server"
                ErrorMessage=" Name should be at least 2 characters and not contain numbers."
                ControlToValidate="txtName" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="savePlace"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]{2,50}$" />
            <asp:Label ID="lblInfo" runat="server" CssClass="label label-warning" Font-Size="Small" Visible="false" Text="You need to select a Health Facility only for Villages (Leaf=true)"></asp:Label>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" OnClientClick="if (!checkPlace()) return false;" ValidationGroup="savePlace" />
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" OnClientClick="if (!checkPlace()) return false;" ValidationGroup="savePlace" />
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
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:GridView ID="gvPlace" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" AllowPaging="True" OnPageIndexChanging="gvPlace_PageIndexChanging" DataSourceId="odsPlace">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "Place.aspx?id={0}") %>'
                                Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                    <asp:TemplateField HeaderText="ParentId">
                        <ItemTemplate>
                            <%#Eval("Parent.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="Leaf" HeaderText="Leaf" SortExpression="Leaf" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsPlace" runat="server" SelectMethod="GetPlaceById" TypeName="GIIS.DataLayer.Place">
                <SelectParameters>
                    <asp:Parameter Name="i" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvPlace_Validate(sender, args) {
            var text = (document.getElementById('<%=txtName.ClientID%>').value == '') || (document.getElementById('ctl00_ContentPlaceHolder1_txtParentId_BBox').value == '');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }

    </script>
    <script type="text/javascript">


        var isresult1 = "test";
        var isresult2 = "test";
        function pageLoad(sender, args) {

            $find('autocomplteextender1')._onMethodComplete = function (result, context) {

                $find('autocomplteextender1')._update(context, result, /* cacheResults */false);
                webservice_callback1(result, context);
            };
            $find('autocomplteextender2')._onMethodComplete = function (result, context) {

                $find('autocomplteextender2')._update(context, result, /* cacheResults */false);
                webservice_callback2(result, context);
            };
        }
        function webservice_callback1(result, context) {

            if (result == "") {
                $find("autocomplteextender1").get_element().style.backgroundColor = "red";
                isresult2 = "";
            }
            else {
                $find("autocomplteextender1").get_element().style.backgroundColor = "white";
                isresult2 = "test";
            }
        }
        function webservice_callback2(result, context) {

            if (result == "") {
                $find("autocomplteextender2").get_element().style.backgroundColor = "red";
                isresult1 = "";
            }
            else {
                $find("autocomplteextender2").get_element().style.backgroundColor = "white";
                isresult1 = "test";
            }
        }
        function checkPlace() {
            if (isresult1 == "") {
                alert("Please choose a place from the list!");
                return false;
            }
            if (isresult2 == "") {
                alert("Please choose a health faciity from the list!");
                return false;
            }
            return true;
        }
    </script>

</asp:Content>

