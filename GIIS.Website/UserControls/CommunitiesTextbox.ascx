<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommunitiesTextbox.ascx.cs" Inherits="UserControls_CommunitiesTextbox" %>
  <asp:UpdatePanel runat="server" ID="TextSearchPanel1" UpdateMode="Conditional" >
     
        <ContentTemplate>
          
                 <asp:TextBox runat="server" ID="CBox" CssClass="form-control" AutoPostBack="true" BackColor="#f2f4fb" placeholder="Type for suggestions..." />
              
                <asp:Button ID="subBtn" runat="Server" OnClick="SelectAction" UseSubmitBehavior="false" Style="display:none;" />
                <asp:Panel runat="server" Height="150px" ID="myPanel"
                    ScrollBars="Vertical" style="overflow:hidden;width:50px;text-overflow:ellipsis;display:none;">
                </asp:Panel>
                <ajaxToolkit:AutoCompleteExtender 
                    runat="server"
                    ID="autoComplete2"   BehaviorId ="autocomplteextender2"
                    TargetControlID="CBox"
                    ServicePath="~/Pages/AutoComplete.asmx" MinimumPrefixLength="3" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList" 
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionInterval="10" 
                    EnableCaching="true" CompletionSetCount="10" CompletionListElementID="myPanel"
                    />
             
            <asp:HiddenField ID="HiddenControlID" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="CBox" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
