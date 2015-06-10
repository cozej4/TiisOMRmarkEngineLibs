<%@ Page Title="Child Configuration" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true"
    CodeFile="ChildConfiguration.aspx.cs" Inherits="_Child" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li><a href="#">Configuration</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Child Configuration" /></li>
            </ol>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ajaxToolkit:TabContainer ID="TabContainer1" runat="server"
                ActiveTabIndex="0" CssClass="MyTabStyle" >
                <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" 
                    HeaderText="Visible Fields" style="height:20px;">
                    <ContentTemplate>
                        <div class="row">
                             <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbSystemId" runat="server" Text="SystemId" Enabled="False" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbFirstname1" runat="server" Text="Firstname1" Checked="True" Enabled="False" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbFirstname2" runat="server" Text="Firstname2" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbLastname1" runat="server" Text="Lastname1" Checked="True" Enabled="False" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbLastname2" runat="server" Text="Lastname2" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbBirthdate" runat="server" Text="Birthdate" Checked="True" Enabled="False" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbGender" runat="server" Text="Gender" Checked="True" Enabled="False" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbHealthcenterId" runat="server" Text="HealthcenterId" Checked="True" Enabled="False" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbBirthplaceId" runat="server" Text="BirthplaceId" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbCommunityId" runat="server" Text="CommunityId" />
                            </div>
                        </div>
                        <div class="row">
                           <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbDomicileId" runat="server" Text="DomicileId" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbStatusId" runat="server" Text="StatusId" Checked="True" Enabled="False" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbAddress" runat="server" Text="Address" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbPhone" runat="server" Text="Phone" />
                            </div>
                        </div>
                        <div class="row">
                          <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbMobile" runat="server" Text="Mobile" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbEmail" runat="server" Text="Email" />
                            </div>
                        </div>
                        <div class="row">
                           <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbMotherId" runat="server" Text="MotherId" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbMotherFirstname" runat="server" Text="MotherFirstname" />
                            </div>
                        </div>
                        <div class="row">
                         <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbMotherLastname" runat="server" Text="MotherLastname" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbFatherId" runat="server" Text="FatherId" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbFatherFirstname" runat="server" Text="FatherFirstname" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbFatherLastname" runat="server" Text="FatherLastname" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbCaretakerId" runat="server" Text="CaretakerId" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbCaretakerFirstname" runat="server" Text="CaretakerFirstname" />
                            </div>
                        </div>
                        <div class="row">
                             <div class="col-md-1 col-xs-1 col-sm-1 col-lg-1 clearfix"></div> 
                            <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
                                <asp:CheckBox ID="chbCaretakerLastname" runat="server" Text="CaretakerLastname" />
                            </div>
                            <div class="col-md-6 col-xs-6 col-sm-6 col-lg-6 clearfix">
                                <asp:CheckBox ID="chbNotes" runat="server" Text="Notes" />
                            </div>
                        </div>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Mandatory Fields">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                 <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatorySystemId" runat="server" Text="SystemId" Enabled="False" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryFirstname1" runat="server" Text="Firstname1" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryFirstname2" runat="server" Text="Firstname2" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryLastname1" runat="server" Text="Lastname1" Checked="True" Enabled="False" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryLastname2" runat="server" Text="Lastname2" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryBirthdate" runat="server" Text="Birthdate" Checked="True" Enabled="False" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryGender" runat="server" Text="Gender" Checked="True" Enabled="False" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryHealthcenterId" runat="server" Text="HealthcenterId" Checked="True" Enabled="False" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryBirthplaceId" runat="server" Text="BirthplaceId" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryCommunityId" runat="server" Text="CommunityId" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryDomicileId" runat="server" Text="DomicileId" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryStatusId" runat="server" Text="StatusId" Checked="True" Enabled="False" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryAddress" runat="server" Text="Address" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryPhone" runat="server" Text="Phone" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryMobile" runat="server" Text="Mobile" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryEmail" runat="server" Text="Email" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryMotherId" runat="server" Text="MotherId" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryMotherFirstname" runat="server" Text="MotherFirstname" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryMotherLastname" runat="server" Text="MotherLastname" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryFatherId" runat="server" Text="FatherId" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryFatherFirstname" runat="server" Text="FatherFirstname" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryFatherLastname" runat="server" Text="FatherLastname" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryCaretakerId" runat="server" Text="CaretakerId" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryCaretakerFirstname" runat="server" Text="CaretakerFirstname" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryCaretakerLastname" runat="server" Text="CaretakerLastname" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="chbMandatoryNotes" runat="server" Text="Notes" />
                                </td>
                                <td></td>
                            </tr>

                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Extra Fields">
                    <ContentTemplate>
                        <div class="row">
                             <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                 </div> 
                              <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                   <asp:Label ID="lblFieldLabel" runat="server" Text="Field Label" Font-Underline="true"></asp:Label>
                                 </div> 
                             <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                  <asp:Label ID="lblFieldFormat" runat="server" Text="Field Format" Font-Underline="true"></asp:Label>
                                 </div> 
                        </div>
                     <div class="row">
                           <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                               <asp:Label ID="lblIdentificationNo1" runat="server" Text="IdentificationNo1" />
                                 </div>
                           <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                <asp:TextBox ID="txtIdentificationNo1" runat="server" CssClass="form-control" />
                           </div>
                           <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                               <asp:TextBox ID="txtValidatorIdentificationNo1" runat="server" CssClass="form-control" />
                           </div> 
                           <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                               <asp:CheckBox ID="chbIdentificationNo1" runat="server" Text="Visible" />
                           </div> 
                          <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                               <asp:CheckBox ID="chbMandatoryIdentificationNo1" runat="server" Text="Mandatory" />
                          </div> 
                     </div>
                          <div class="row">
                              <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                   <asp:Label ID="lblIdentificationNo2" runat="server" Text="IdentificationNo2" />
                              </div> 
                                 <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                      <asp:TextBox ID="txtIdentificationNo2" runat="server" CssClass="form-control" />
                                 </div> 
                               <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                    <asp:TextBox ID="txtValidatorIdentificationNo2" runat="server" CssClass="form-control" />
                               </div> 
                               <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                    <asp:CheckBox ID="chbIdentificationNo2" runat="server" Text="Visible" />
                               </div>
                                <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                     <asp:CheckBox ID="chbMandatoryIdentificationNo2" runat="server" Text="Mandatory" />
                                </div> 
                          </div>
                          <div class="row">
                               <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                    <asp:Label ID="lblIdentificationNo3" runat="server" Text="IdentificationNo3" />
                               </div> 
                               <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                   <asp:TextBox ID="txtIdentificationNo3" runat="server" CssClass="form-control" />
                               </div> 
                               <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix">
                                   <asp:TextBox ID="txtValidatorIdentificationNo3" runat="server" CssClass="form-control" />
                               </div> 
                               <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                    <asp:CheckBox ID="chbIdentificationNo3" runat="server" Text="Visible" />
                               </div> 
                               <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
                                   <asp:CheckBox ID="chbMandatoryIdentificationNo3" runat="server" Text="Mandatory" />
                               </div> 
                          </div>
                           
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    <br />
    <div class="row">
         <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix"></div>
          <div class="col-md-2 col-xs-2 col-sm-2 col-lg-2 clearfix">
               <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary btn-raised" OnClick="btnSave_Click" />
              </div> 
    </div>
     <br />
     <div class="row">
        <div class="col-md-3 col-xs-3 col-sm-3 col-lg-3 clearfix"></div>
        <div class="col-md-5 col-xs-5 col-sm-5 col-lg-5 clearfix">
            <asp:Label ID="lblSuccess" runat="server" Text="Success" CssClass="label label-success" Font-Size="Small"  Visible="false" />
            <asp:Label ID="lblWarning" runat="server" Text="Warning" CssClass="label label-warning" Font-Size="Small"  Visible="false" />
            <asp:Label ID="lblError" runat="server" Text="Error" CssClass="label label-danger" Font-Size="Small" Visible="false" />
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
