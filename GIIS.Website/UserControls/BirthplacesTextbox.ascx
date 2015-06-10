<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BirthplacesTextbox.ascx.cs" Inherits="UserControls_BirthplacesTextbox" %>
 <asp:UpdatePanel runat="server" ID="TextSearchPanel2" UpdateMode="Conditional" >
     
        <ContentTemplate>
          
                 <asp:TextBox runat="server" ID="BBox" CssClass="form-control" AutoPostBack="true" BackColor="#f2f4fb" placeholder="Type for suggestions..." />
              
                <asp:Button ID="subBtn" runat="Server" OnClick="SelectAction" UseSubmitBehavior="false" Style="display:none;" />
                <asp:Panel runat="server" ID="myPanel"
                    ScrollBars="Vertical" style="overflow:hidden;text-overflow:ellipsis;display:none;  z-index:1100 !important;">
                </asp:Panel>
                <ajaxToolkit:AutoCompleteExtender 
                    runat="server"
                    ID="autoComplete3"   BehaviorId ="autocomplteextender3"
                    TargetControlID="BBox"
                    ServicePath="~/Pages/AutoComplete.asmx" MinimumPrefixLength="3" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList" 
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionInterval="1" 
                    EnableCaching="true" CompletionSetCount="10" CompletionListElementID="myPanel"
                    />
             
            <asp:HiddenField ID="HiddenControlID" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BBox" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
