<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.ascx.cs" Inherits="UserControls_ResetPassword" %>

<asp:Panel ID="pnlResetPassword" runat="server">
    <div class="log-in">
        <div class="row">&nbsp;</div>
        <div class="row">
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
            <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <i class="glyphicon glyphicon-user"></i>
                        </span>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username" AutoCompleteType="Disabled" />
                    </div>
                </div>
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
        </div>
        <div class="row">
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
            <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <i class="glyphicon glyphicon-lock"></i>
                        </span>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" MaxLength="15" TextMode="Password" AutoCompleteType="Disabled" placeholder="New Password" ValidationGroup="resetPassword" />
                    </div>
                </div>
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
                <ajaxToolkit:PasswordStrength ID="PS" runat="server"
                    TargetControlID="txtNewPassword"
                    DisplayPosition="BelowLeft"
                    StrengthIndicatorType="Text"
                    PreferredPasswordLength="6"
                    PrefixText="Strength:"
                    TextCssClass="TextIndicator_txtNewPassword"
                    MinimumNumericCharacters="1"
                    MinimumSymbolCharacters="0"
                    RequiresUpperAndLowerCaseCharacters="True"
                    TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
                    TextStrengthDescriptionStyles="cssClass1;cssClass2;cssClass3;cssClass4;cssClass5"
                    CalculationWeightings="50;15;15;20" />


            </div>
        </div>
        <div class="row">
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
            <div class="col-md-10 col-xs-10 col-sm-10 col-lg-10 clearfix">
                <div class="form-group">
                    <div class="input-group">
                        <span class="input-group-addon">
                            <i class="glyphicon glyphicon-lock"></i>
                        </span>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" MaxLength="15" TextMode="Password" AutoCompleteType="Disabled" placeholder="Confirm New Password" ValidationGroup="resetPassword" />
                    </div>
                </div>
            </div>
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div>
            <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
                <ajaxToolkit:PasswordStrength ID="PasswordStrength1" runat="server"
                    TargetControlID="txtConfirmPassword"
                    DisplayPosition="BelowLeft"
                    StrengthIndicatorType="Text"
                    PreferredPasswordLength="6"
                    PrefixText="Strength:"
                    TextCssClass="TextIndicator_txtConfirmPassword"
                    MinimumNumericCharacters="1"
                    MinimumSymbolCharacters="0"
                    RequiresUpperAndLowerCaseCharacters="True"
                    TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
                    TextStrengthDescriptionStyles="cssClass1;cssClass2;cssClass3;cssClass4;cssClass5"
                    CalculationWeightings="50;15;15;20" />
        </div>
        <div class="row">
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
        </div>
        <div class="row">
                <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            </div>
                <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
                    <asp:Button ID="btnResetPassword" runat="server" Text="Reset" CssClass="btn btn-block btn-sm btn-primary"
                        ValidationGroup="resetPassword" OnClick="btnResetPassword_Click" />
            </div>
                <div class="col-md-4 col-xs-4 col-sm-4 col-lg-4 clearfix">
            </div>
        </div>
        <div class="row">
            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix">
            </div>
            <div class="col-md-11 col-xs-11 col-sm-11 col-lg-11 clearfix">
                    <asp:CustomValidator ID="cvRequiredFields" runat="server" ErrorMessage="All fields marked with * must be filled!"
                        ClientValidationFunction="cvRequiredFields_Validate" CssClass="label label-warning" Font-Size="Small" ForeColor="White" Display="Dynamic"
                        ValidationGroup="resetPassword"></asp:CustomValidator>

                    <asp:CompareValidator ID="comparePasswords"
                        runat="server"
                        ControlToCompare="txtNewPassword"
                        ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Your passwords do not match!"
                        CssClass="label label-warning" Font-Size="Small" ForeColor="White"
                        ValidationGroup="password"
                        Display="Dynamic" />

                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ValidationGroup="resetPassword"
                        ErrorMessage="Password must be at least 6 characters, no more than 15 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit."
                        ControlToValidate="txtNewPassword" Display="Dynamic" ForeColor="Red"
                        ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,15}$" />

                    <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small" Visible="false" />
                    <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small" Visible="false" />
                    <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
                 <asp:Label ID="lblUsernameExists" runat="server" Text="The username does not exist!" CssClass="label label-warning" Font-Size="Small" Visible="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
