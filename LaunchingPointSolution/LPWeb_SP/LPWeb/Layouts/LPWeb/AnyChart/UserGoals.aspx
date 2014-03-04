<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGoals.aspx.cs" Inherits="LPWeb.AnyChart.UserGoals" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Goals</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="js/AnyChart.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            InitGoalsChart();

            InitDateRange();

            $("#ddlDateRange").change(dllDateRange_onchange);
        });

        function InitGoalsChart() {

            var DateRange = GetQueryString1("DateRange");
            if (DateRange == "") {
                DateRange = "ThisMonth";
            }
            if (DateRange != "ThisMonth" && DateRange != "NextMonth"
                && DateRange != "ThisQuarter" && DateRange != "NextQuarter"
                    && DateRange != "ThisYear" && DateRange != "NextYear") {
                DateRange = "ThisMonth";
            }

            var GoalsChart = new AnyChart('swf/AnyChart.swf');
            GoalsChart.xmlLoadingText = "Loading data...";
            GoalsChart.noDataText = "No user goals set up";
            GoalsChart.width = "270";
            GoalsChart.wMode = "opaque";
            GoalsChart.height = "220";
            GoalsChart.setXMLFile('UserGoals_GetData.aspx?DateRange=' + DateRange);
            GoalsChart.write('divUserGoalsChart');
        }

        function InitDateRange() {

            // DateRange
            var DateRange = GetQueryString1("DateRange");
            if (DateRange == "ThisMonth") {
                $("#ddlDateRange").val("This month");
            }
            else if (DateRange == "NextMonth") {
                $("#ddlDateRange").val("Next month");
            }
            else if (DateRange == "ThisQuarter") {
                $("#ddlDateRange").val("This quarter");
            }
            else if (DateRange == "NextQuarter") {
                $("#ddlDateRange").val("Next quarter");
            }
            else if (DateRange == "ThisYear") {
                $("#ddlDateRange").val("This year");
            }
            else if (DateRange == "NextYear") {
                $("#ddlDateRange").val("Next year");
            }
        }

        function dllDateRange_onchange() {

            var Range = $("#ddlDateRange").val();

            if (Range == "This month") {

                window.location.href = window.location.pathname + "?DateRange=ThisMonth";
            }
            else if (Range == "Next month") {

                window.location.href = window.location.pathname + "?DateRange=NextMonth";
            }
            else if (Range == "This quarter") {

                window.location.href = window.location.pathname + "?DateRange=ThisQuarter";
            }
            else if (Range == "Next quarter") {

                window.location.href = window.location.pathname + "?DateRange=NextQuarter";
            }
            else if (Range == "This year") {

                window.location.href = window.location.pathname + "?DateRange=ThisYear";
            }
            else if (Range == "Next year") {

                window.location.href = window.location.pathname + "?DateRange=NextYear";
            }
        }
// ]]>
    </script>
</head>
<body>
    
    <div id="divUserGoalsPanel">
        <div>
            <select id="ddlDateRange" style="width: 150px;">
                <option>This month</option>
                <option>Next month</option>
                <option>This quarter</option>
                <option>Next quarter</option>
                <option>This year</option>
                <option>Next year</option>
            </select>
        </div>
        <div id="divUserGoalsChart" style="margin-top: 7px;"></div>
    </div>
    
</body>
</html>
