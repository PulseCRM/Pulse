<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanCompaignEventContent.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Marketing.LoanCompaignEventContent" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <style type="text/css">
        .spanContact
        {
            vertical-align: top;
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div id="divContainer" style="border: solid 0px green; padding-left: 10px;">
            <table style="width: 700px; height: 600px">
                <tr>
                    <td width="100%" style="padding-left: 10px;">
                        <b>
                            <asp:Label ID="lbTitle" runat="server"></asp:Label></b>
                    </td>
                </tr>
                <tr>
                    <td width="100%" style="padding-left: 10px;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="100%" style="height: 100%; vertical-align: top;" align="left" valign="top">
                        <asp:Label ID="lbContent" runat="server" Style="vertical-align: top;"></asp:Label>
                        <iframe runat="server" id="ifmRes" visible="false" height="100%" width="100%">
                        </iframe>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    </form>
</body>
</html>
