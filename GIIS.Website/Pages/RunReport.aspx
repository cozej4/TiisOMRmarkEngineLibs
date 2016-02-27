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
    <form onsubmit="scrubNullValues()" class="form" method="get" action="RenderReport.aspx" id="launchReport" >
    <div class="row">
            <div class="col-md-12">
                    <asp:TextBox ID="hack" runat="server" Visible="false" />
                    <input type="hidden" name="reportId" value="<%=Request.QueryString["reportId"]%>" />
                    <input type="hidden" id="reportFormat" name="format" value="html" />
                    <div class="container-fluid" runat="server" id="reportInputs">
                    </div>
            </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <a class="btn btn-primary" onclick="previewReport()" href="#preview-panel">Preview (HTML)</a>
            <input type="submit" class="btn btn-success" value="Download (PDF)" onclick="$('#reportFormat').val('pdf')" />
            <input type="submit" class="btn btn-success" value="Download (XLS)" onclick="$('#reportFormat').val('csv')" />
        </div>
    </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default" id="preview-panel">
                    <div class="panel-heading"><h2 class="panel-title"><a data-toggle="collapse" href="#panel-collapse">Report Preview</a></h2></div>
                    <div class="panel-collapse collapse" id="panel-collapse">
                        <div class="panel-body" id="preview-body" style="height:400px; overflow:scroll">

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>
    <script type="text/javascript">
        function previewReport() {
            $.ajaxSetup({ cache: false });
            $("#panel-collapse").collapse(false);
            $('#preview-body').html('<center><img src="/img/294.gif"/><br/>Generating Preview</center>');
            setTimeout(function () {
                var url = 'RenderReport.aspx?';
                $("#reportFormat").val('html');
                // Serialize the form as kv
                $('#launchReport input').each(function (i) {
                    if ($(this).val() != '')
                        url += $(this).attr('name') + '=' + $(this).val() + "&";
                });
                $('#launchReport select').each(function (i) {
                    if ($(this).val() != '')
                        url += $(this).attr('name') + '=' + $(this).val() + "&";
                });

                $.ajax({
                    url: url,
                    async: true
                }).success(function (e) {
                    $('#preview-body').html(e);
                }).error(function (x, s, e) {
                    $('#preview-body').html('<div class="label label-danger center-block">An error occurred generating the report: ' + e + '</div>')
                });
            }, 500);
                return false;
        }
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
            setTimeout(resetNullValues, 500);
        }

        function resetNullValues() {
            $('input[disabled=disabled]').each(function(i) {
                var $input = $(this);
                if ($input.val() == '')
                    $input.removeAttr('disabled');
            });
            $('select[disabled=disabled]').each(function(i) {
                var $input = $(this);
                if ($input.val() == '')
                    $input.removeAttr('disabled');
            });
        }
    </script>
    <ajaxToolkit:CalendarExtender TargetControlID="hack" ID="ceMain" runat="server" />
</asp:Content>