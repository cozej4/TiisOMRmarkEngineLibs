<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AutoCompleteTextbox.ascx.cs" Inherits="UserControls_AutoCompleteTextbox" %>
  <asp:UpdatePanel runat="server" ID="TextSearchPanel" UpdateMode="Conditional" >
     
        <ContentTemplate>
          
                 <asp:TextBox runat="server" ID="SBox" CssClass="form-control" AutoPostBack="true" BackColor="#f2f4fb" placeholder="Type for suggestions..." />
              
                <asp:Button ID="subBtn" runat="Server" OnClick="SelectAction" UseSubmitBehavior="false" Style="display:none;" />
                <asp:Panel runat="server"  ID="myPanel"
                    ScrollBars="Vertical" style="overflow:hidden;text-overflow:ellipsis;display:none; z-index:1100 !important;">
                </asp:Panel>
                <ajaxToolkit:AutoCompleteExtender 
                    runat="server"
                    ID="autoComplete1"   BehaviorId ="autocomplteextender1"
                    TargetControlID="SBox"
                    ServicePath="~/Pages/AutoComplete.asmx" MinimumPrefixLength="3" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList" 
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionInterval="1" 
                    EnableCaching="true" CompletionSetCount="10" CompletionListElementID="myPanel"
                    
                    />
             
            <asp:HiddenField ID="HiddenControlID" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="SBox" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>

