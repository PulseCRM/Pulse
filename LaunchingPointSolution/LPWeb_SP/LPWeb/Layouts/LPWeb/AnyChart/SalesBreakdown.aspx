<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesBreakdown.aspx.cs" Inherits="LPWeb.AnyChart.SalesBreakdown" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Breakdown</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="js/AnyChart.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            InitGoalsChart();
        });

        function InitGoalsChart() {

            var GoalsChart = new AnyChart('swf/AnyChart.swf');
            GoalsChart.xmlLoadingText = "Loading data...";
            GoalsChart.noDataText = "No data for sales breakdown";
            GoalsChart.width = "280";
            GoalsChart.wMode = "opaque";
            GoalsChart.height = "300";

            var XmlUrl = 'SalesBreakdown_GetData.aspx' + window.location.search;
            GoalsChart.setXMLFile(XmlUrl);
            GoalsChart.write('divSalesBreakdown');

            //alert(XmlUrl);
        }
// ]]>
    </script>
</head>
<body>
    <div>
        <div id="divSalesBreakdown"></div>
    </div>
</body>
</html>
