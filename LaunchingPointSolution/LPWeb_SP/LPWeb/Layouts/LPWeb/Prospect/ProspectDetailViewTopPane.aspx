<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectDetailViewTopPane.aspx.cs"
    Inherits="ProspectDetailViewTopPane" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Client Detail</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        jQuery(document).ready(function () {
            DrawTab();

            SetTab("ProspectLoans.aspx", 0);
            var sHasEmailView = "<%=sHasEmailViewRight %>";
            var sHasNoteView = "<%=sHasNoteViewRight %>";
            var sHasMarketingView = "<%=sHasMarketingRight %>";
            if (sHasEmailView == "0") {
                DisableLink("aEmail");
            }
            if (sHasNoteView == "0") {
                DisableLink("aNotes");
            }
            if (sHasMarketingView == "0") {
                DisableLink("aMarketing");
            }
        });

        function SetTab(src, i) {

            var ContactID = $("#hfContactID").val();

            if (i == 2) {
                // Start: add by peter
                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "ProspectDetailEmailTab.aspx?ProspectID=" + ContactID + "&sid=" + RadomStr);
                // End : add by peter

            }
            else if (i == 3) {

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "ProspectNotesTab.aspx?ContactID=" + ContactID + "&sid=" + RadomStr);
            }
            else if (i == 4) {

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "ProspectMailChimpTab.aspx?ContactID=" + ContactID + "&sid=" + RadomStr);
            }
            else if (i == 5) {

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "ProspectActivityTab.aspx?ContactID=" + ContactID + "&sid=" + RadomStr);
            }
            else if (i == 7) {

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "EmploymentTab.aspx?ContactID=" + ContactID + "&PageFrom=ClientDetail&sid=" + RadomStr);
            }
            else if (i == 8) {
                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "ProspectDetailIncomeTab.aspx?ProspectID=" + ContactID + "&sid=" + RadomStr);
                
            }
             else if (i == 9) {
                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "ProspectDetailAssetTab.aspx?ProspectID=" + ContactID + "&sid=" + RadomStr);
            }
            else if (i == 10) {
                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#tabFrame").attr("src", "");//"EmploymentTab.aspx?ProspectID=" + ContactID + "&sid=" + RadomStr);
            }
            else {
                $("#tabFrame").attr("src", SetSrc(src));
            }
            $("#tabs10 #current").removeAttr("id");
            $("#tabs10 ul li").eq(i).attr("id", "current");
            DrawTab();
        }

        function SetSrc(src) {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var ContactID = GetQueryString1("ContactID");
            src = src + "?sid=" + RadomStr + "&ContactID=" + ContactID;
            return src
        }

        //2011-08-06 Wangxiao Add Start
        function ShowGlobalPopup(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divGlobalPopup").attr("title", Title);
            $("#ifrGlobalPopup").attr("src", "");
            $("#ifrGlobalPopup").attr("src", iFrameSrc);
            $("#ifrGlobalPopup").width(iFrameWidth);
            $("#ifrGlobalPopup").height(iFrameHeight);

            // show modal
            $("#divGlobalPopup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) {
                    $("#divGlobalPopup").dialog("destroy");
                    $("#ifrGlobalPopup").attr("src", "about:blank");
                }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
        }

        function CloseGlobalPopup() {
            $("#divGlobalPopup").dialog("close");
            RefreshPage();
        }

        var RefreshFlag = ""
        function CloseDialog_SendCompletionEmail() {
            $("#divGlobalPopup").dialog("close");
            if (RefreshFlag == "SendCompletionEmail") {
                RefreshPage();
            }
        }

        function ShowWaitingDialog(WaitingMsg, TaskIDs, loanIDs) {
            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            $("#hfTaskIds").val("");
            $("#hfTaskIds").val(TaskIDs);
            $("#hfLoanIds").val("");
            $("#hfLoanIds").val(loanIDs);

            $("#btnOK").show();
            $("#btnSkip").show();
            $("#btnCancel").show();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function ShowWaitingDialogSingleTask(WaitingMsg) {
            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();

            $("#btnOK").hide();
            $("#btnSkip").hide();
            $("#btnCancel").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function ComplateError(ErrorMsg) {
            $("#divContainer").unblock();
            alert(ErrorMsg);
            RefreshPage();
        }

        function ShowConfirmWindow() {
            $("#divContainer").block({ message: $('#ConfirmWindow'), css: { width: '650px'} });
        }

        var SendEmail = "Cancel";
        function CloseConfirmWindow(returnValue) {
            SendEmail = returnValue;
            $("#btnOK").hide();
            $("#btnSkip").hide();
            $("#btnCancel").hide();
            $("#aClose").hide();
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            if (SendEmail == "OK" || SendEmail == "Skip") {
                $.getJSON("ProspectTasksComplate_Background.aspx?sid=" + Radom + "&TaskIDs=" + $("#hfTaskIds").val() + "&SendEmail=" + SendEmail + "&LoanIDs=" + $("#hfLoanIds").val(), AfterTaskComplete);
            }
            else {

                $('#divContainer').unblock();

            }
            //$("#divContainer").unblock(); 
        }

        function AfterTaskComplete(data) {
            if (data.ExecResult == "Failed") {

                ComplateError(data.ErrorMsg);
                return;
            }

            $("#hdnErrMsg").val(data.ErrorMsg);
            CloseLoan(data);
        }

        function CloseWaitingDialog(SuccessMsg) {
            var Msg = $('#hdnErrMsg').val();

            if (Msg.length <= 0)
                Msg = SuccessMsg;
            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(Msg);
            $('#aClose').show();
        }
        function CloseWaitingDialog1() {

            $('#divContainer').unblock();
        }


        function CloseLoan(data) {
            var LoanClosed = data.LoanClosed;
            var ErrMsg = data.ErrorMsg;

            if (LoanClosed == "Yes") {
                RefreshFlag = "CloseLoan";
                ShowCloseLoanDialog(data.LoanID);
            }
            else {
                RefreshPage();
            }

            //setTimeout("CloseWaitingDialog('Completed tasks successfully.')", 500);
        }

        function ShowCloseLoanDialog(sloanid) {
            var f = document.getElementById('iframePF');
            f.src = "../LoanDetails/CloseLoanPopup.aspx?fid=" + sloanid + "&sid=" + Math.random().toString();
            $("#dialog1").dialog({
                height: 650,
                width: 630,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
            return false;
        }

        function CloseDialog_CloseLoan() {
            $("#dialog1").dialog("close");
            if (RefreshFlag == "CloseLoan") {
                RefreshPage();
            }
        }

        function RefreshPage() {
            $('#divContainer').unblock();
            if ($.browser.msie == true) {   // ie
                window.document.frames("ifrLoanInfo").location.href = window.document.frames("ifrLoanInfo").location.href;
            }
            else {   // firefox
                window.document.getElementById('ifrLoanInfo').contentWindow.location.href = window.document.getElementById('ifrLoanInfo').contentWindow.location.href;
            }

            if ($.browser.msie == true) {   // ie
                window.document.frames("tabFrame").location.href = window.document.frames("tabFrame").location.href;
            }
            else {   // firefox
                window.document.getElementById('tabFrame').contentWindow.location.href = window.document.getElementById('tabFrame').contentWindow.location.href;
            }
        }
        //2011-08-06 Wangxiao Add End

        function ShowUpdatePointWait(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();

            $("#btnOK").hide();
            $("#btnSkip").hide();
            $("#btnCancel").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function HideUpdatePointWait() {
            $("#divContainer").unblock();

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 1060px; border: solid 0px red; padding: 0px;">
        <div id="content">
            <iframe id="ifrLoanInfo" frameborder="0" scrolling="no" style="width: 1056px; height: 255px;
                border: solid 0px red;" src="ProspectDetailInfo.aspx?ContactID=<%= this.iContactID %>">
            </iframe>
        </div>
        <div class="JTab" style="margin-top: 0px; border: solid 0px red;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="" onclick="SetTab('ProspectLoans.aspx',0);return false;"><span>
                                    Loans</span></a></li>
                                <li><a href="" onclick="SetTab('ProspectTasksTab.aspx',1);return false;"><span>Tasks</span></a></li>
                                <li><a id="aEmail" href="" onclick="SetTab('ProspectDetailEmailTab.aspx',2);return false;">
                                    <span>Emails</span></a></li>
                                <li><a id="aNotes" href="" onclick="SetTab('ProspectNotesTab.aspx',3);return false;">
                                    <span>Notes</span></a></li>
                                <li><a id="aMarketing" href="" onclick="SetTab('ProspectMailChimpTab.aspx',4);return false;">
                                    <span>Marketing</span></a></li>
                                <li><a href="" onclick="SetTab('ProspectActivityTab.aspx',5);return false;"><span>Activity
                                    History</span></a></li>
                                <li><a href="" onclick="SetTab('ProspectDetailRelationshipTab.aspx',6);return false;">
                                    <span>Relationship</span></a></li>
                                <li><a href="" onclick="SetTab('EmploymentTab.aspx',7);return false;"><span>Employment</span></a></li>
                                <li><a href="" onclick="SetTab('ProspectDetailIncomeTab.aspx',8);return false;"><span>Income/Expenses</span></a></li>
                                <li><a href="" onclick="SetTab('ProspectDetailAssetTab.aspx',9);return false;"><span>Assets</span></a></li>
                                <!--<li><a href="" onclick="SetTab('',10);return false;"><span>Liabilities</span></a></li>-->
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
                    <iframe id="tabFrame" scrolling="no" frameborder="0" style="border: solid 0px blue;
                        height: 610px; width: 1037px;"></iframe>
                </div>
            </div>
            <asp:HiddenField ID="hfContactID" runat="server" />
        </div>
    </div>
        <div id="divGlobalPopup" title="Global Popup" style="display: none;">
        <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="100px" height="100px">
        </iframe>
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
                    &nbsp;&nbsp; <a id="aClose" href="javascript:RefreshPage()" style="font-weight: bold;
                        color: #6182c1;">[Close]</a>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfTaskIds" runat="server" />
        <asp:HiddenField ID="hfLoanIds" runat="server" />
        <input id="hdnErrMsg" runat="server" type="text" style="display: none;" />
        <input id="hdnLoanStatus" runat="server" type="text" style="display: none;" />
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td style="width: 200px">
                    <input id="btnOK" type="button" value="OK" class="Btn-66" onclick="CloseConfirmWindow('OK')" />
                </td>
                <td style="width: 200px">
                    <input id="btnSkip" type="button" value="Skip" class="Btn-66" onclick="CloseConfirmWindow('Skip')" />
                </td>
                <td style="width: 200px">
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="CloseConfirmWindow('Cancel')" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dialog1" title="Select Closed Loan Folder" style="display: none;">
        <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
        </iframe>
    </div>
    <div id="ConfirmWindow" style="display: none; padding: 5px;">
    </div>

    </form>

</body>
</html>
