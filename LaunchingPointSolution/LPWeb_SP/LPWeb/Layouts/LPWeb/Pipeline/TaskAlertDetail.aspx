﻿<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskAlertDetail.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Pipeline.TaskAlertDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Task Alert Detail</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
        div.opt
        {
            margin-top: 20px;
        }
        div.opt input
        {
            margin-right: 5px;
        }
        form
        {
            padding-left: 10px;
        }
        .detail
        {
            margin-top: 20px;
        }
        .detail td
        {
            line-height: 20px;
            white-space: nowrap;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("img[src$='Unknown.png']").each(function () {
                $(this).hide();
            });

            $("#<%= btnClose.ClientID %>").click(function () {
                if (parent != null) {
                    parent.closeDialog();
                }
                return false;
            });

            $(".detail tr").each(function () {
                $(this).find("td:eq(0)").css({ "padding-right": "0px", "width": "20px" });
                $(this).find("td:eq(1)").css({ "padding-right": "80px" });
                $(this).find("td:eq(2)").css({ "padding-right": "40px" });
            })
        });

        function PopupDefer() {
            var TaskIDs = $("#<%= hfdAllTaskIds.ClientID %>").val();

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrDeferTask").attr("src", "../LoanDetails/LoanTaskDefer.aspx?sid=" + RadomStr + "&TaskIDs=" + TaskIDs);

            // show modal
            $("#divDeferTask").dialog({
                height: 130,
                width: 320,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_DeferTask() {

            $("#divDeferTask").dialog("close");
        }

        function WindowOpen_SendEmail() {

            var LoanID = GetQueryString1("fileID");
            var TaskId = GetQueryString1("LoanTaskId");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            OpenWindow("../LoanDetails/EmailSendWindowOpen.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&LoanTaskId=" + TaskId, "_SendMail4", 670, 610, "no", "center");

            return true;
        }

        //#region complete task

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {
            var Msg = $('#hdnErrMsg').val();

            if (Msg.length <= 0)
                Msg = SuccessMsg;
            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(SuccessMsg);
            $('#aClose').show();
        }

        function aComplete_onclick() {

            var HasUncompletePrerequisite = $("#hdnHasUncompletePrerequisite").val();

            if (HasUncompletePrerequisite == "True") {

                alert("The task has unfinished prerequisite task, can't be completed");
                return;
            }

            var hdnTaskOwner = $("#hdnTaskOwner").val();
            var LoginUserID = $("#hdnLoginUserID").val();
            var hdnMakeOtherTaskComp = $("#hdnMakeOtherTaskComp").val();
            if (hdnMakeOtherTaskComp == "0") {
                if ((LoginUserID != hdnTaskOwner) && (hdnTaskOwner)) {
                    alert("You do not have the privilege to complete tasks that are not assigned to you.");
                    return;
                }
            }

            var TaskID = $("#hfdTaskId").val();

            CompleteTask(TaskID);
        }

        function CompleteTask(TaskID) {

            // show waiting dialog
            ShowWaitingDialog("Completing task...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = $("#hfdFileId").val();

            // Ajax
            $.getJSON("../LoanDetails/LoanTaskComplete_Background.aspx?sid=" + Radom + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterTaskComplete);
        }

        function AfterTaskComplete(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                window.parent.closeDialog(false);
                return;
            }

            // show send completion email
            if (isID(data.EmailTemplateID) == true) {

                WindowOpen_SendCompletionEmail(data.EmailTemplateID);
            }

            $("#hdnErrMsg").val(data.ErrorMsg);
            CloseLoan(data);
        }

        function WindowOpen_SendCompletionEmail(EmailTemplateID) {

            var LoanID = GetQueryString1("fileID");
            var TaskId = GetQueryString1("LoanTaskId");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            OpenWindow("../LoanDetails/EmailSendCompletionWindowOpen.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&EmailTemplateID=" + EmailTemplateID + "&LoanTaskId=" + TaskId, "_SendCompletionMail5", 630, 380, "no", "center");
            return true;
        }
        //#endregion
        function CloseLoan(data) {

            var LoanClosed = data.LoanClosed;
            var ErrMsg = data.ErrorMsg;
            var LoanStatus = $("#hdnLoanStatus").val();
            //            alert("LoanStatus=" + LoanStatus + ", LoanClosed=" + LoanClosed + ", ErrMsg=" + ErrMsg);
            if (LoanClosed == "Yes" && LoanStatus == "Processing") {
                ShowCloseLoanDialog();
                $("#hdnLoanStatus").val("Closed");
            }

            setTimeout("CloseWaitingDialog('Completed task successfully.')", 500);
        }
        function ShowCloseLoanDialog() {
            var LoanID = GetQueryString1("fileID");
            OpenWindow("../LoanDetails/CloseLoanPopupWindowOpen.aspx?fid=" + LoanID + "&sid=" + Math.random().toString(), "_CloseLoanFolderPopupWindowOpen", 650, 630, "no", "center");

            return true;
        }
        function CloseDialog_CloseLoan() {

            $("#dialog1").dialog("close");
        }
        function RefreshPage() {

            if ($.browser.msie == true) {   // ie

                window.parent.document.frames("taskAlertDetail").location.reload();
            }
            else {   // firefox

                window.parent.document.getElementById('taskAlertDetail').contentWindow.location.reload();
            }

            // refresh current page
            window.location.href = window.location.href;
        }
        function Close(bIRefresh) {
            var CloseDialogCodes = $("#hdnCloseDialogCodes").val();
            //            alert("CloseDialogCodes=" + CloseDialogCodes);
            if (CloseDialogCodes == "") {
                window.parent.closeDialog(bIRefresh);
                return;
            }
            if (CloseDialogCodes == "sharepoint") {
                window.frameElement.commitPopup();
                return false;
            }
            eval(CloseDialogCodes);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 560px; height: 270px;">
        <div id="divDeferTask" title="Defer Loan Task" style="display: none;">
            <iframe id="ifrDeferTask" frameborder="0" scrolling="auto" width="280px" height="90px">
            </iframe>
        </div>
        <table class="detail" runat="server" id="tblNoDataMessage" visible="false">
            <tr>
                <td>
                    The loan task cannot be found in the database. It may have been deleted.
                </td>
            </tr>
        </table>
        <asp:FormView ID="fvTaskAlertDetail" runat="server" DefaultMode="ReadOnly">
            <ItemTemplate>
                <table class="detail">
                    <tr>
                        <td width="20px">
                            <img id="imgRateLockIcon" src='../images/loan/<%# Eval("AlertIcon")%>' width="16"
                                height="16" />
                        </td>
                        <td style="padding-right: 80px;">
                            Task Name:
                            <%# Eval("TaskName")%>
                        </td>
                        <td>
                            Stage:
                            <%# Eval("Stage") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            Borrower:
                            <%# Eval("Borrower") %>
                        </td>
                        <td>
                            Due Date:
                            <%# Convert.ToDateTime(Eval("DueDate").ToString()).ToString("MM/dd/yyyy") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            Coborrower:<%# Eval("Coborrower") %>
                        </td>
                        <td>
                            Owner:
                            <%# Eval("Owner") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            Property:
                            <%# Eval("Property") %>
                        </td>
                        <td>
                            Loan Officer:
                            <%# Eval("Loan Officer") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            Loan Amount:
                            <%# Eval("LoanAmount","{0:c0}")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            Interest Rate:
                            <%# Eval("Rate", "{0:n4}") %>%
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:FormView>
        <asp:HiddenField ID="hfdFileId" runat="server" />
        <asp:HiddenField ID="hfdTaskId" runat="server" />
        <asp:HiddenField ID="hfdAllTaskIds" runat="server" />
        <div class="opt">
            <input id="btnComplete" type="button" value="Complete" class="Btn-91" onclick="aComplete_onclick()" />
            <asp:Button ID="btnDefer" runat="server" Text="Defer" CssClass="Btn-91" OnClientClick="PopupDefer();return false;" />
            <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" CssClass="Btn-91"
                OnClientClick="WindowOpen_SendEmail(); return false;" />
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="Btn-91" OnClientClick="Close(false)" />
        </div>
    </div>
    <div id="divWaiting" style="display: none; padding: 5px;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
                </td>
                <td style="padding-left: 5px;">
                    <label id="WaitingMsg" style="color: #818892; font-weight: bold;">
                    </label>
                    &nbsp;&nbsp; <a id="aClose" href="javascript:Close(true)"
                        style="font-weight: bold; color: #6182c1;">[Close]</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="divSendCompletionEmail" title="Send Completion Email" style="display: none;">
        <iframe id="ifrSendCompletionEmail" frameborder="0" scrolling="no" width="605px"
            height="340px"></iframe>
    </div>
    <div id="dialog1" title="Select Closed Loan Folder" style="display: none;">
        <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
        </iframe>
    </div>
    <div id="divHiddenFields" style="display: none;">
        <asp:TextBox ID="hdnHasUncompletePrerequisite" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnTaskOwner" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnMakeOtherTaskComp" runat="server"></asp:TextBox>
        <asp:HiddenField ID="hdnCloseDialogCodes" runat="server" />
        <asp:HiddenField ID="hdnUserId" runat="server" />
        <asp:HiddenField ID="hdnLoanStatus" runat="server" />
        <asp:HiddenField ID="hdnErrMsg" runat="server" />
    </div>
    </form>
</body>
</html>
