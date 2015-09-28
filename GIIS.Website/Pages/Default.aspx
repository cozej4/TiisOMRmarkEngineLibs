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
<%@ Page Title="Home - GIIS" Language="C#" MasterPageFile="~/Pages/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Pages_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register Src="~/UserControls/OrderAlert.ascx" TagName="OrderAlert" TagPrefix="giis" %>
<%@ Register Src="~/UserControls/ItemLotsToExpire.ascx" TagName="ItemLotsToExpire" TagPrefix="giis" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li class="active"><a href="Default.aspx">Home</a></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:Chart ID="Chart1" runat="server" Width="850px" Palette="Pastel"                                                                                         >
                <Series>
                    <asp:Series Name="Series1"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderColor="#339966">
                         <AxisX LineColor="Gray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisX>
                    <AxisY LineColor="Gray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisY>
                    </asp:ChartArea>

                </ChartAreas>
            </asp:Chart>

        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <asp:Chart ID="Chart2" runat="server" Width="850px" Palette="SeaGreen">
                <Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderColor="#339966">
                          <AxisX LineColor="Gray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisX>
                    <AxisY LineColor="Gray">
                        <MajorGrid LineColor="LightGray" />
                    </AxisY>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
    </div>
</asp:Content>

