<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingCampaignDetails.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Marketing.MarketingCampaignDetails" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
  
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
     
    <script src="../js/jquery.datepick.js" type="text/javascript"></script> 
    <script src="../js/date.js" type="text/javascript"></script>
     <%--<style>
         html {     overflow:-moz-scrollbars-vertical; }

    </style>--%>
    <script type="text/javascript">
        function ShowDetails(tLS) {

            //            window.open("MarketingCampaignEventContent.aspx?campaignEventId=" + tLS + "&CloseDialogCodes=window.parent.CloseGlobalPopup()");
            //            var iFrameSrc = "MarketingCampaignEventContent.aspx?campaignEventId=" + tLS + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            //            var BaseWidth = 811
            //            var iFrameWidth = BaseWidth + 2;
            //            var divWidth = iFrameWidth + 25;

            //            var BaseHeight = 580;
            //            var iFrameHeight = BaseHeight + 2;
            //            var divHeight = iFrameHeight + 40;

            //            window.parent.parent.parent.ShowGlobalPopup("", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        $(document).ready(function () {

            if ($.browser.msie == true) {

//                alert(window.mainFrame.style.height);
            }
            else {   // not ie browser

                //alert($("#ifrGlider1", window.parent.document).height());
                // reset container iframe height
                //                 alert($(this).height());
                //                alert($(this).html());
                //$(this).parent().height($(this).height()+100);

                //window.mainFrame.style.height  = $(this).height(); 

                //alert( window.mainFrame.value );

                $("iframe", window.parent.document).each(function () {

                    window.parent.dyniframesize($(this).attr("id"));
                })

            }

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="border: solid 0px green; padding-left: 10px;">
        <table style="margin-top: 5px; width:550px">
            <tr>
                <td style="font-weight:bold; width:450px">
                    Campaign:<asp:Label ID="lbName" runat="server" ></asp:Label>
                </td>
                 <td style="font-weight:bold; width:100px">
                   Price:$<asp:Label ID="lbPrice" runat="server" ></asp:Label>
                </td>
            </tr>
        </table>
         <table style="margin-top: 5px;width:600px">
            <tr>
                <td>
                  <asp:Label ID="lbDesc" runat="server" ></asp:Label>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="3" style="margin-top: 5px; margin-left: 6px;" border="0">
            <%=GetCampaignDetailIconsInfo()%>
        </table>
	    
    </div>
    </form>
</body>
</html>
