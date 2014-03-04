<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpportunityInfotab.aspx.cs" Inherits="OpportunityInfotab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
     <script language="javascript" type="text/javascript">
// <![CDATA[


         $(document).ready(function () {
             SetTabByName();
         });

         function SetTabByName() {
             var sSpeTab = GetQueryString1("tab");   // get the specified tab name
             var tabToOpen = $("a:first-child", "#tabs10 #current"); // get the default tab

             // find the specified tab
             $("li", "#tabs10").each(function () {
                 if (sSpeTab == jQuery.trim($(this).text())) {
                     tabToOpen = $("a:first-child", this);
                 }
             });

             // here, tabToOpen should not be null
             tabToOpen.trigger("click");
         }

         function SetTab(src, i) {

             $("#tabFrame").attr("src", SetSrc(src));
             $("#tabs10 #current").removeAttr("id");
             $("#tabs10 ul li").eq(i).attr("id", "current");
             DrawTab();
         }

         function SetSrc(src) {


             var RadomNum = Math.random();
             var RadomStr = RadomNum.toString().substr(2);

             var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
             if (src.indexOf("LoanDetailsTask.aspx") != -1 || src.indexOf("LoanDetailsAlertTab.aspx") != -1) {

                 src = src + "?sid=" + RadomStr + "&LoanID=" + FileID + "&ref=lead";
             }
             else {
                 src = src + "?sid=" + RadomStr + "&FileID=" + FileID + "&LoanID=" + FileID + "&ref=lead";
             }
             return src
         }

         function SetWinHeight(obj) {

             var win = obj;

             if (document.getElementById) {

                 if (win && !window.opera) {

                     if (win.contentDocument && win.contentDocument.body.offsetHeight)

                         win.height = win.contentDocument.body.offsetHeight;

                     else if (win.Document && win.Document.body.scrollHeight)

                         win.height = win.Document.body.scrollHeight;

                 }

             }

         }

// ]]>
    </script>
    <style>
    *{ margin:0px;}
    </style>
</head>
<body>
<form  id="form1" runat="server">
<div class="JTab" style="margin-top: 10px; width:1020px">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a  href="" onclick="SetTab('LeadLoanInfoTab.aspx',0);return false;"><span>Loan Info</span></a></li>
                                <li><a href="" onclick="SetTab('EmploymentIncomeTab.aspx',1);return false;"><span>Income/Employment</span></a></li>
                                <li><a href="" onclick="SetTab('LeadOtherIncome_Comments.aspx',2);return false;"><span>Other Income/Comments</span></a></li>
                                <li><a href="" onclick="SetTab('ProspectNotesTab.aspx',3);return false;"><span>Notes</span></a></li>

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
                    <iframe id="tabFrame" frameborder="0" style="border: solid 0px blue; height: 510px;
                        width: 1005px; overflow:hidden;" scrolling="no" onload="Javascript:SetWinHeight(this)" ></iframe>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfFileID" runat="server" />
</form>
</body>
</html>