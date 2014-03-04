<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectLoans.aspx.cs"
    Inherits="ProspectLoans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            DrawTab();
            SetTab("ProspectLoanOpportunitiesTab.aspx", 0);
        });

        function SetTab(src, i) {
            $("#tabFrame").attr("src", SetSrc(src));
            $("#tabs10 #current").removeAttr("id");
            $("#tabs10 ul li").eq(i).attr("id", "current");
            DrawTab();
        }

        function SetSrc(src) {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var ContactID = jQuery("#<%= this.hfContactID.ClientID %>").val();
            src = src + "?sid=" + RadomStr + "&ContactID=" + ContactID;
            return src
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divProspectTabs">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="#" onclick="SetTab('ProspectLoanOpportunitiesTab.aspx',0);return false;">
                                    <span>Leads</span></a></li>
                                <li><a href="#" onclick="SetTab('ProspectLoanActiveTab.aspx',1);return false;"><span>
                                    Loans</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine">
                    &nbsp;</div>
                <div class="TabContent">
                    <iframe id="tabFrame" src="" frameborder="0" style="border: none; height: 800px;
                        width: 100%;"></iframe>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfContactID" runat="server" />
    </form>
</body>
</html>
