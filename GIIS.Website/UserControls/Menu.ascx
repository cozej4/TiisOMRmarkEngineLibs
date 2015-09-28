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
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="UserControls_Menu" %>

<asp:TreeView ID="tvMenu" runat="server" ExpandDepth="0" Font-Bold="true" Font-Size="Medium" >    
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="nodeLevel1"  />
        <asp:TreeNodeStyle CssClass="nodeLevel2" />
        <asp:TreeNodeStyle CssClass="nodeLevel3" />
        <asp:TreeNodeStyle CssClass="nodeLevel3" />
    </LevelStyles>
   
</asp:TreeView>
