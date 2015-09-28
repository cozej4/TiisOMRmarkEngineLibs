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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DomicileTextbox.ascx.cs" Inherits="UserControls_DomicileTextbox" %>
 <asp:UpdatePanel runat="server" ID="TextSearchPanel3" UpdateMode="Conditional" >
     
        <ContentTemplate>
          
                 <asp:TextBox runat="server" ID="DBox" CssClass="form-control" AutoPostBack="true" BackColor="#f2f4fb" placeholder="Type for suggestions..." />
              
                <asp:Button ID="subBtn" runat="Server" OnClick="SelectAction" UseSubmitBehavior="false" Style="display:none;" />
                <asp:Panel runat="server" Height="150px" ID="myPanel"
                    ScrollBars="Vertical" style="overflow:hidden;width:50px;text-overflow:ellipsis;display:none; z-index:1100 !important;">
                </asp:Panel>
                <ajaxToolkit:AutoCompleteExtender 
                    runat="server"
                    ID="autoComplete4"   BehaviorId ="autocomplteextender4"
                    TargetControlID="DBox"
                    ServicePath="~/Pages/AutoComplete.asmx" MinimumPrefixLength="3" 
                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList" 
                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionInterval="1" 
                    EnableCaching="true" CompletionSetCount="10" CompletionListElementID="myPanel"
                    />
             
            <asp:HiddenField ID="HiddenControlID" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DBox" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
