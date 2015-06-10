<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="UserControls_Menu" %>

<asp:TreeView ID="tvMenu" runat="server" ExpandDepth="0" Font-Bold="true" Font-Size="Medium" >    
    <LevelStyles>
        <asp:TreeNodeStyle CssClass="nodeLevel1"  />
        <asp:TreeNodeStyle CssClass="nodeLevel2" />
        <asp:TreeNodeStyle CssClass="nodeLevel3" />
        <asp:TreeNodeStyle CssClass="nodeLevel3" />
    </LevelStyles>
   
</asp:TreeView>
