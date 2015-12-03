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
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RunReport.aspx.cs" EnableEventValidation="false" Inherits="Pages_RunReport" MasterPageFile="~/Pages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .ajax__calendar_container { z-index : 1000 ; }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12 clearfix">
            <ol class="breadcrumb">
                <li><a href="Default.aspx">Home</a></li>
                <li class="active">
                    <a href="Report.aspx">Reports</a></li>
                <li class="active">
                    <asp:Label ID="lblTitle" runat="server" Text="Run Report" /></li>
            </ol>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <h2><asp:Label ID="lblReportName" Text="ReportNameHere" runat="server" /></h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <em><asp:Label runat="server" ID="lblReportDescription" /></em>
        </div>
    </div>
    </form>
    <form target="_blank" onsubmit="scrubNullValues()" class="form" method="get" action="<%= this.jasperAction %>" id="launchReport" >
    <div class="row">
            <div class="col-md-12">
                    <asp:TextBox ID="hack" runat="server" Visible="false" />
                    <input type="hidden" name="j_username" value="<%=ConfigurationManager.AppSettings["JasperUser"]%>" />
                    <input type="hidden" name="j_password" value="<%=ConfigurationManager.AppSettings["JasperPassword"]%>" />
                    <div class="container-fluid" runat="server" id="reportInputs">
                    </div>
            </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <input type="submit" class="btn btn-primary" value="View (HTML)" onclick="$('#launchReport').attr('action','<%=this.jasperAction%>.html');" />
            <input type="submit" class="btn btn-success" value="Download (PDF)" onclick="$('#launchReport').attr('action','<%=this.jasperAction%>.pdf');" />
            <input type="submit" class="btn btn-success" value="Download (XLS)" onclick="$('#launchReport').attr('action','<%=this.jasperAction%>.xls');" />
        </div>
    </div>
    </form>
    <script type="text/javascript">
        function scrubNullValues() {
            $('input').each(function(i) {
                var $input = $(this);
                if ($input.val() == '')
                    $input.attr('disabled', 'disabled');
            });
            $('select').each(function(i) {
                var $input = $(this);
                if ($input.val() == '')
                    $input.attr('disabled', 'disabled');
            });
        }
    </script>
    <ajaxToolkit:CalendarExtender TargetControlID="hack" ID="ceMain" runat="server" />
</asp:Content>