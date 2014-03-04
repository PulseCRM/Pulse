<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupAlertDetail.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Pipeline.PopupAlertDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rate Lock Alert Detail</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.thickbox.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
        div.opt
        {
            margin-top: 20px;
        }
        div.opt input
        {
            margin-right: 10px;
        }
        form
        {
            padding-left: 10px;
        }
        .detail
        {
            margin-top:20px;
        }
        .detail td
        {
            line-height:20px;
            white-space:nowrap;
        }
    </style>
    <script type="text/javascript">
       $().ready(function () {
           $("#BtnClose").click(CloseCurrentWindowHandller);
       });

       function OpenExtendRateLockWindow() {
           
           var bShowRateLockPopup = $("#<%= hdnShowLockRatePopup.ClientID %>").val();
           if (bShowRateLockPopup != "true") {
               alert("You do not have the privilege to extend rate lock.");
               return false;
           }

           var RadomNum = Math.random();
           var sid = RadomNum.toString().substr(2);
           var FileId = $("#<%= hfdFileId.ClientID %>").val();

           var iFrameSrc = "../LoanDetails/LockRatePopup.aspx?FileId=" + FileId + "&sid=" + sid;

           var BaseWidth = 750;
           var iFrameWidth = BaseWidth + 2;
           var divWidth = iFrameWidth + 25;

           var BaseHeight = 620;
           var iFrameHeight = BaseHeight + 2;
           var divHeight = iFrameHeight + 40;

           window.parent.ShowGlobalPopup("Lock Rate", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

           // close self
           CloseCurrentWindowHandller();
       }

       function CloseCurrentWindowHandller() {
           if (parent != null) {
               parent.closeDialogR();
           }
       }

       function CloseTheUpdateExtendWindow()
       {
            $("#tbxExtendDays").val("");
            $("#divExtendRateLock").dialog("close");
        }

       function WindowOpen_SendEmail() {

           var LoanID = GetQueryString1("fileId");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            OpenWindow("../LoanDetails/EmailSendWindowOpen.aspx?sid=" + RadomStr + "&LoanID=" + LoanID, "_SendMail3", 670, 610, "no", "center");

            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfdFileId" runat="server" />
    <asp:HiddenField ID="hfdExpDate" runat="server" />
    <asp:HiddenField ID="hdnShowLockRatePopup" runat="server" />
    <div id="divExtendRateLock" title="Extend the rate lock" style="display: none;">
        <iframe id="ifrExtendRateLock" frameborder="0" scrolling="no" width="450px" height="110px"></iframe>
    </div>
    <div id="divDetail">
        <table class="detail">
            <tr>
                <td width="20px">
                    <img id="imgIcon" runat="server" alt="" />
                </td>
                <td style="padding-right: 80px;">
                    Rate Lock Expiration:
                    <asp:Label ID="lblRateLockExp" runat="server"></asp:Label>
                </td>
                <td>
                    Current Stage:
                    <asp:Label ID="lblCurrentState" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Borrower:
                    <asp:Label ID="lblBorrower" runat="server"></asp:Label>
                </td>
                <td>
                    Est Close Date:
                    <asp:Label ID="lblEstCloseDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Coborrower:
                    <asp:Label ID="lblCoborrower" runat="server"></asp:Label>
                </td>
                <td>
                    Loan Officer:
                    <asp:Label ID="lblLoanOfficer" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td rowspan="2">
                    Property:
                    <asp:Label ID="lblPropertyAddress" runat="server"></asp:Label>
                </td>
                <td>
                    Loan Amount:
                    <asp:Label ID="lblLoanAmount" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Interest Rate:
                    <asp:Label ID="lblInterestRate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Lender:<asp:Label ID="lblLender" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div id="divButtons" class="opt">
        <input id="BtnExtendRateLock" type="button" value="Extend Ratelock" onclick="OpenExtendRateLockWindow();" class="Btn-91" runat="server" />
        <asp:Button ID="BtnSendEmail" runat="server" Text="Send Email" class="Btn-91" OnClientClick="WindowOpen_SendEmail(); return false;"/>
        <input id="BtnClose" type="button" value="Close" class="Btn-91"/>
    </div>
    
    </form>
</body>
</html>
