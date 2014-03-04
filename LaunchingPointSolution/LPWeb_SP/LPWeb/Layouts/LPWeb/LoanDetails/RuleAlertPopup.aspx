<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleAlertPopup.aspx.cs" Inherits="LoanDetails_RuleAlertPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rule Alert</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        var hdnSendEmail = "#<%= hdnSendEmail.ClientID %>";

        $(document).ready(function () {

            // event
            $("#ddlEmailSelector").change(ddlEmailSelector_onchange);

            // left click context menu
            $("#btnAction").contextMenu({ menu: 'divActionMenu', leftButton: true }, function (action, el, pos) { contextMenuWork(action, el, pos); });

            if ($(hdnSendEmail).val() == "0") {
                $("#btnSendEmail").attr("enable", "false");
            }

            //#region enable/disable link buttons

            var AlertStatus = $("#hdnAlertStatus").val();
            //alert(AlertStatus);
            if (AlertStatus == "") {

                $("#divActionMenu").enableContextMenuItems('#Acknowledge');
                $("#divActionMenu").enableContextMenuItems('#Dismiss');

                $("#divActionMenu").enableContextMenuItems('#Accept');
                $("#divActionMenu").enableContextMenuItems('#Decline');
            }
            else if (AlertStatus == "Acknowledged") {

                $("#divActionMenu").disableContextMenuItems('#Acknowledge');
                $("#divActionMenu").disableContextMenuItems('#Dismiss');

                $("#divActionMenu").enableContextMenuItems('#Accept');
                $("#divActionMenu").enableContextMenuItems('#Decline');
            }
            else {

                $("#divActionMenu").disableContextMenuItems('#Acknowledge');
                $("#divActionMenu").disableContextMenuItems('#Dismiss');

                $("#divActionMenu").disableContextMenuItems('#Accept');
                $("#divActionMenu").disableContextMenuItems('#Decline');
            }

            //#endregion
        });

        function ddlEmailSelector_onchange(EmailType) {

            var EmailType = $("#ddlEmailSelector").val();
            var EmailContent = "";
            if (EmailType == "Alert Email Content") {

                EmailContent = $("#txtAlertEmail").text();
            }
            else {

                EmailContent = $("#txtRecommEmail").text();
            }

            $("#divEmailContent").html(EmailContent);
        }

        function WindowOpen_SendEmail() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            OpenWindow("../LoanDetails/EmailSendWindowOpen.aspx?sid=" + RadomStr + "&LoanID=" + LoanID, "_SendMail10", 670, 610, "no", "center");

            return true;
        }

        // left click context menu
        function contextMenuWork(action, el, pos) {

            switch (action) {
                case "Acknowledge":
                    {
                        aAcknowledge_onclick();
                        break;
                    }
                case "Accept":
                    {
                        aAccept_onclick();
                        break;
                    }

                case "Decline":
                    {
                        aDecline_onclick();
                        break;
                    }
                case "Dismiss":
                    {
                        aDismiss_onclick();
                        break;
                    }
            }
        }

        //#region show/close waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {

            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(SuccessMsg);
            $('#aClose').show();

        }

        //#endregion

        //#region button click

        function aAcknowledge_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Acknowledging Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = GetQueryString1("AlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Acknowledge&AlertID=" + AlertID, AfterAcknowledge);
        }

        function AfterAcknowledge(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Acknowledge successfully.')", 2000);
        }

        function aDismiss_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Dismissing Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = GetQueryString1("AlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Dismiss&AlertID=" + AlertID, AfterDismiss);
        }

        function AfterDismiss(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Dismiss successfully.')", 2000);
        }

        function aAccept_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Accepting Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = GetQueryString1("AlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Accept&AlertID=" + AlertID, AfterAccept);
        }

        function AfterAccept(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Accept successfully.')", 2000);

            window.parent.location = window.parent.location;
        }

        function aDecline_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Declining Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = GetQueryString1("AlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Decline&AlertID=" + AlertID, AfterDecline);
        }

        function AfterDecline(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Decline successfully.')", 2000);
        }

        //#endregion
        function Cancel() {
            var CloseDialogCodes = $("#hdnCloseDialogCodes").val();
//            alert("CloseDialogCodes=" + CloseDialogCodes);
            if (CloseDialogCodes == "") {
                window.parent.CloseDialog_RuleAlert();
                return;
            }
            if (CloseDialogCodes == "sharepoint") {
                window.frameElement.commitPopup(); 
                return false;
            }
           eval(CloseDialogCodes);      
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 500px; height: 600px; border: solid 0px red; padding-left: 10px; padding-top: 10px;">
        
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 22px;">
                    <img id="imgAlertIcon" runat="server" alt="AlertIcon" src="../images/alert/Alert-Red.gif" />
                </td>
                <td>
                    <span id="lbAlertDesc" style="position: relative; top: -1px;">
                        Rule Name: <asp:Label ID="lbRuleName" runat="server" Text="Label"></asp:Label>
                    </span>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;" border="0">
            <tr>
                <td style="width: 270px;">Detected: <asp:Label ID="lbDetected" runat="server" Text="Label"></asp:Label></td>
                <td>Due Date: <asp:Label ID="lbDueDate" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
            <tr>
                <td style="width: 270px;">Borrower: <asp:Label ID="lbBorrower" runat="server" Text="Label"></asp:Label></td>
                <td>Loan Officer: <asp:Label ID="lbLoanOfficer" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
            <tr>
                <td style="width: 270px;">Coborrower: <asp:Label ID="lbCoborrower" runat="server" Text="Label"></asp:Label></td>
                <td>Loan Amount: $<asp:Label ID="lbLoanAmount" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;" border="0">
            <tr>
                <td style="width: 48px; vertical-align: top;">Property:</td>
                <td style="width: 212px;"><asp:Label ID="lbProperty" runat="server" Text="Label"></asp:Label></td>
                <td style="padding-left: 10px; width: 68px; vertical-align: top;">Interest Rate:</td>
                <td style="vertical-align: top;"><asp:Label ID="lbInterestRate" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 20px;">
            <tr>
                <td style="width: 65px;">View Email:</td>
                <td>
                    <select id="ddlEmailSelector">
                        <option>Alert Email Content</option>
                        <option>Recommended Email Content</option>
                    </select>
                </td>
                            
            </tr>
        </table>

        <div id="divEmailContent" style="height: 260px; margin-top: 10px; padding: 10px; overflow: auto; border: solid 1px #e4e7ef;">
            <asp:Literal ID="ltEmailContent" runat="server"></asp:Literal>      
        </div>
        <div id="divButtons" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <input id="btnAction" type="button" value="Action" class="Btn-66" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnSendEmail" type="button" value="Send Email" class="Btn-91" onclick="WindowOpen_SendEmail()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="Cancel()" />
                    </td>
                </tr>
            </table>
        </div>
        
    </div>
    <div id="divHiddenFields" style="display: none;">
        <textarea id="txtAlertEmail" runat="server" cols="20" rows="2"></textarea>
        <textarea id="txtRecommEmail" runat="server" cols="20" rows="2"></textarea>
        <input id="hdnLoginUserID" runat="server" type="text" />
        <asp:HiddenField ID="hdnSendEmail" runat="server" />
        <asp:HiddenField ID="hdnAlertStatus" runat="server" />
    </div>
    <ul id="divActionMenu" class="contextMenu">
        <li><a href="#Acknowledge">Acknowledge</a></li>
        <li><a href="#Accept">Accept</a></li>
        <li><a href="#Decline">Decline</a></li>
        <li><a href="#Dismiss">Dismiss</a></li>
    </ul>
    <div id="divWaiting" style="display: none; padding: 5px;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>&nbsp;&nbsp;
					<a id="aClose" href="javascript:window.parent.IsRefreshPage=true;window.location.href=window.location.href" style="font-weight: bold; color: #6182c1;">[Close]</a>
				</td>
			</tr>
		</table>
	</div>
    <asp:HiddenField ID="hdnCloseDialogCodes" runat="server" />
    </form>
</body>
</html>