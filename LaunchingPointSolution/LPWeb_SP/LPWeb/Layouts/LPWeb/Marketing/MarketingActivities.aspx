<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingActivities.aspx.cs" Inherits="Marketing_MarketingActivities" MasterPageFile="~/_layouts/LPWeb/MasterPage/Marketing.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<link href="../css/style.web.css" rel="stylesheet" type="text/css" />
     
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
// <![CDATA[


        $(document).ready(function () {
            DrawTab();

            SetTab('MarketingActiviteCompain.aspx', 0);

        });

        function SetTab(src, i, CampaignId) {

            $("#tabFrame").attr("src", SetSrc(src, CampaignId));
            $("#tabs10 #current").removeAttr("id");
            $("#tabs10 ul li").eq(i).attr("id", "current");
            DrawTab();
        }

        function SetSrc(src, CampaignId) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            if (CampaignId != "") {
                src = src + "?CampaignId=" + CampaignId + "&sid=" + RadomStr;
            }
            else 
                src = src + "?sid=" + RadomStr;

            return src
        }
// ]]>
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">

        <div class="JTab" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="" onclick="SetTab('MarketingActiviteCompain.aspx',0);return false;"><span>Campaigns</span></a></li>
                                <li><a href="" onclick="SetTab('MarketingActivitiesEvents.aspx',1); return false;"><span>Events</span></a></li>
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
                    <iframe id="tabFrame" frameborder="0" style="border: solid 0px blue;height: 710px; width: 1035px;"></iframe>
                </div>
            </div>
        </div>
</asp:Content>

