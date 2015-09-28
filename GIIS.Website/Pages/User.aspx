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
<%@ Page Title="User" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="_User" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Setup</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="User" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblUsername" runat="server" Text="Username" />
            <span style="color: Red">&nbsp;*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" AutoCompleteType="Disabled"  />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblPassword" runat="server" Text="Password" />
            <span style="color: Red">&nbsp; *</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" AutoCompleteType="Disabled" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblHealthFacilityId" runat="server" Text="HealthFacilityId" />
            <span style="color: Red">&nbsp; *</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc1:AutoCompleteTextbox runat="server"
                    ID="txtHealthFacilityId"
                    OnClickSubmit="true"
                    ServiceMethod="HealthCentersUser"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblRole" runat="server" Text="Role"></asp:Label>
            <span style="color: Red">&nbsp; *</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" DataSourceID="odsRole" DataTextField="Name" DataValueField="Id" OnDataBound="ddlRole_DataBound"></asp:DropDownList>
                <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="GetRoleForList" TypeName="GIIS.DataLayer.Role"></asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblFirstname" runat="server" Text="Firstname" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtFirstname" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblLastname" runat="server" Text="Lastname" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtLastname" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblEmail" runat="server" Text="Email" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="The email is not valid!" ForeColor="Red" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"> <%--^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$--%></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblNotes" runat="server" Text="Notes" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">&nbsp;</div>
        <div class="col-md-8 col-xs-8 col-sm-8 col-lg-8 clearfix">
            <asp:Label ID="revHealthFacility" runat="server" CssClass="label label-warning" Font-Size="Small" Text="The health facility you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
            <asp:CustomValidator ID="cvUser" runat="server" ErrorMessage="All fields marked with * must be filled!" ClientValidationFunction="cvUser_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveUser"></asp:CustomValidator>
            <asp:RegularExpressionValidator ID="revPassword" runat="server"
                ErrorMessage="Password must be at least 6 characters, no more than 15 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit."
                ControlToValidate="txtPassword" Display="Dynamic" ForeColor="Red"
                ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,15}$" />
            <asp:RegularExpressionValidator ID="revFirstname" runat="server"
                ErrorMessage="First name should be at least 2 characters and not contain numbers."
                ControlToValidate="txtFirstname" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]{2,50}$" />
            <asp:RegularExpressionValidator ID="revLastname" runat="server"
                ErrorMessage="Last name should be at least 2 characters and not contain numbers."
                ControlToValidate="txtLastname" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"
                ValidationExpression="^[^0-9_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&-]{2,50}$" />
<%--            <asp:CompareValidator ID="cvRole" runat="server" ControlToValidate="ddlRole" ErrorMessage="Please select a role for the user!" ValidationGroup="saveUser" Operator="NotEqual" CssClass="labelError" ValueToCompare="-1"></asp:CompareValidator>--%>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary btn-raised" OnClick="btnAdd_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveUser" />
        </div>
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
        <asp:GridView ID="gvUser" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBound="gvUser_Databound" DataSourceID="odsUser">
            <PagerSettings Position="Top" Mode="NumericFirstLast" />
            <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                <%--                <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />--%>
                <asp:TemplateField HeaderText="Username">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlUsername" runat="server" NavigateUrl='<%# Eval("Id", "EditUser.aspx?id={0}") %>'
                            Text='<%# Eval("Username", "{0}") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="HealthFacilityId" HeaderText="HealthFacilityId" SortExpression="HealthFacilityId" />--%>
                <asp:TemplateField HeaderText="HealthFacility">
                    <ItemTemplate>
                        <%#Eval("HealthFacility.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Firstname" HeaderText="Firstname" SortExpression="Firstname" />
                <asp:BoundField DataField="Lastname" HeaderText="Lastname" SortExpression="Lastname" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsUser" runat="server" SelectMethod="GetUserById" TypeName="GIIS.DataLayer.User">
            <SelectParameters>
                <asp:Parameter Name="i" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div></div> 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvUser_Validate(sender, args) {
            var text = (document.getElementById('<%=txtUsername.ClientID%>').value == '') || (document.getElementById('<%=txtPassword.ClientID%>').value == '') || (document.getElementById('ctl00_ContentPlaceHolder1_txtHealthFacilityId_SBox').value == '') || (document.getElementById('<%=ddlRole.ClientID%>').value == '-1');
            if (text) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
    </script>
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

