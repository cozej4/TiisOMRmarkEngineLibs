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
<%@ Page Title="Health Facility" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="HealthFacility.aspx.cs" Inherits="_HealthFacility" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Setup</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Health Facility" /></li>
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
            <span style="color: Red">*</span>
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
            <asp:Label ID="lblParentId" runat="server" Text="Parent" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <uc1:AutoCompleteTextbox runat="server"
                    ID="txtParentId"
                    OnClickSubmit="true"
                    ServiceMethod="HealthCenters"
                    HighlightResults="true"
                    ClearAfterSelect="false" OnValueSelected="HealthCenters_ValueSelected" />
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
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblTopLevel" runat="server" Text="TopLevel" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblTopLevel" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True" Text="Yes&nbsp;&nbsp;&nbsp;"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblLeaf" runat="server" Text="Leaf" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblLeaf" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="True&nbsp;&nbsp;&nbsp;" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblVaccinationPoint" runat="server" Text="VaccinationPoint" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblVaccinationPoint" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="True&nbsp;&nbsp;&nbsp;" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblVaccineStore" runat="server" Text="Vaccine Store" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:RadioButtonList ID="rblVaccineStore" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Selected="True" Value="True&nbsp;&nbsp;&nbsp;" Text="Yes"></asp:ListItem>
                    <asp:ListItem Value="False" Text="No"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblAddress" runat="server" Text="Address" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblContact" runat="server" Text="Contact" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblType" runat="server" Text="Type" />
            <span style="color: Red">*</span>
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" AutoPostBack="True" DataSourceID="odsHealthFacilityType" DataTextField="Name" DataValueField="Id">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="odsHealthFacilityType" runat="server" SelectMethod="GetHealthFacilityTypeForList" TypeName="GIIS.DataLayer.HealthFacilityType"></asp:ObjectDataSource>
            </div>
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblOwnership" runat="server" Text="Ownership" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:DropDownList ID="ddlOwnership" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-----" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Government" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Private" Value="2"></asp:ListItem>
                    <asp:ListItem Text="FBO" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Parastatal" Value="4"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Label ID="lblColdStorageCapacity" runat="server" Text="Cold Storage Cap. (lit)" />
        </div>
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
            <div class="form-group">
                <asp:TextBox ID="txtColdStorageCapacity" runat="server" CssClass="form-control" />
            </div>
        </div>
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
            <asp:Label ID="revHealthFacility" runat="server" CssClass="label label-warning" Font-Size="Small" Text=" The health facility you have chosen is not valid! Please choose from the options!" Visible="false"></asp:Label>
            <asp:CustomValidator ID="cvHealthFacility" runat="server" ErrorMessage=" All fields marked with * must be filled!" ClientValidationFunction="cvHealthFacility_Validate" 
                CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveHealthFacility"></asp:CustomValidator>
            <asp:RegularExpressionValidator ID="revName" runat="server"
                ErrorMessage=" Name should be at least 2 characters and not contain numbers."
                ControlToValidate="txtName" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White" ValidationGroup="saveHealthFacility"
                ValidationExpression="^[^_\|°¬!#\$%/\()\?¡¿+{}[\]:.\,;@ª^\*<>=&]{2,50}$" />
         <%--   <asp:RegularExpressionValidator ID="revCode" runat="server"
                ErrorMessage=" Code should be 5 characters long and contain only numbers!"
                ControlToValidate="txtCode" Display="Dynamic" CssClass="label label-danger" Font-Size="Small" ForeColor="White"
                ValidationExpression="^[0-9]{5}$" />--%>
            <asp:RegularExpressionValidator ID="revCapacity" runat="server" ControlToValidate="txtColdStorageCapacity" ForeColor="White" Display="Dynamic"
                ErrorMessage="The cold storage capacity is not valid!" ValidationGroup="saveHealthFacility" CssClass="label label-danger" Font-Size="Small" ValidationExpression="^[0-9.]+$"> </asp:RegularExpressionValidator>
            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtContact" ForeColor="White"
                ErrorMessage="The contact number is not valid!" ValidationGroup="saveHealthFacility" CssClass="label label-danger" Font-Size="Small" ValidationExpression="^[0-9 -]{4,30}$"> </asp:RegularExpressionValidator>
            <asp:CustomValidator ID="cvLevels" runat="server" ErrorMessage=" A health facility can not be both top level and leaf level!" OnServerValidate="cvRadioButtons_ServerValidate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic" ValidationGroup="saveHealthFacility"></asp:CustomValidator>
            <asp:Label ID="lblDelete" runat="server" CssClass="label label-danger" Font-Size="Small" Text=" You can not delete this health facility" Visible="false"></asp:Label>

        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary  btn-raised" OnClick="btnAdd_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveHealthFacility" />
            <br />
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-raised" OnClick="btnEdit_Click" OnClientClick="if (!checkHFacility()) return false;" ValidationGroup="saveHealthFacility" />
        </div>
        <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">&nbsp;</div>
        <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
            <br />
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
            <asp:GridView ID="gvHealthFacility" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-responsive" OnDataBound="gvHealthFacility_Databound" OnPageIndexChanging="gvHealthFacility_PageIndexChanging" DataSourceID="odsHealthFacility">
                <PagerSettings Position="Top" Mode="NumericFirstLast" />
                <RowStyle CssClass="gridviewRow" />
                <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Top" />
                <HeaderStyle CssClass="gridviewHeader" />
                <AlternatingRowStyle CssClass="gridviewRowAlt" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlName" runat="server" NavigateUrl='<%# Eval("Id", "HealthFacility.aspx?id={0}") %>'
                                Text='<%# Eval("Name", "{0}") %>' ToolTip='<%# Eval("Notes", "{0}") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" />
                    <%-- <asp:BoundField DataField="ParentId" HeaderText="ParentId" SortExpression="ParentId" />--%>
                    <asp:TemplateField HeaderText="ParentId">
                        <ItemTemplate>
                            <%#Eval("Parent.Name")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField DataField="TopLevel" HeaderText="TopLevel" SortExpression="TopLevel" />
                    <asp:CheckBoxField DataField="Leaf" HeaderText="Leaf" SortExpression="Leaf" />
                    <asp:CheckBoxField DataField="VaccinationPoint" HeaderText="VaccinationPoint" SortExpression="VaccinationPoint" />
                    <asp:CheckBoxField DataField="VaccineStore" HeaderText="Vaccine Store" SortExpression="VaccineStore" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" Visible="false" />
                    <asp:CheckBoxField DataField="IsActive" HeaderText="IsActive" SortExpression="IsActive" />
                    <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" Visible="False" />
                    <asp:BoundField DataField="ModifiedBy" HeaderText="ModifiedBy" SortExpression="ModifiedBy" Visible="False" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" Visible="false" />
                    <asp:BoundField DataField="Population" HeaderText="Population" SortExpression="Population" Visible="false" />
                    <asp:TemplateField HeaderText="AddUser">
                        <ItemTemplate>
                            <a href='<%# Eval("Id", "User.aspx?hfId={0}") %>' target="_blank">
                                <img alt='Add User' src="../img/add.png" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsHealthFacility" runat="server" SelectMethod="GetHealthFacilityById" TypeName="GIIS.DataLayer.HealthFacility">
                <SelectParameters>
                    <asp:Parameter Name="i" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javaScript">
        function cvHealthFacility_Validate(sender, args) {
            var text = (document.getElementById('<%=ddlType.ClientID%>').value == -1) || (document.getElementById('<%=txtName.ClientID%>').value == '') || (document.getElementById('<%=txtCode.ClientID%>').value == '') || (document.getElementById('ctl00_ContentPlaceHolder1_txtParentId_SBox').value == '');
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
                webservice_callback(result, context, sender);
            };
        }
        function webservice_callback(result, context, sender) {

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

        function ClientPopulated(source, eventArgs) {
            if (source._currentPrefix != null) {
                var list = source.get_completionList();
                var search = source._currentPrefix.toLowerCase();
                for (var i = 0; i < list.childNodes.length; i++) {
                    var text = list.childNodes[i].innerHTML;
                    var index = text.toLowerCase().indexOf(search);
                    if (index != -1) {
                        var value = text.substring(0, index);
                        value += '<span class="AutoComplete_ListItemHiliteText">';
                        value += text.substr(index, search.length);
                        value += '</span>';
                        value += text.substring(index + search.length);
                        list.childNodes[i].innerHTML = value;
                    }
                }
            }
        }
    </script>
</asp:Content>

