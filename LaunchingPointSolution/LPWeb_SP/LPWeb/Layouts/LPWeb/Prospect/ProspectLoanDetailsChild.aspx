<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="Prospect_ProspectLoanDetailChild" CodeBehind="ProspectLoanDetailsChild.aspx.cs" %>

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

            DrawTab();

            var sHasEmailView = "<%=sHasEmailViewRight %>";
            var sHasNoteView = "<%=sHasNoteViewRight %>";
            var sHasMarketingView = "<%=sHasMarketingRight %>";
            if (sHasEmailView == "0") {
                DisableLink("aEmail");
            }
            if (sHasNoteView == "0") {
                DisableLink("aNotes");
            }
//            if (sHasMarketingView == "0") {
//                DisableLink("aMarketing");
//            }

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

        function ChangeTab(tab_index) {

            // select tab
            $("#tabs10 #current").removeAttr("id");
            $("#tabs10 ul li").eq(tab_index).attr("id", "current");
        }

        function aBorrowerInfo_onclick(tab_index) {

            // common parameters
            var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // set iframe.src
            $("#tabFrame").attr("src", "BorrowerInfotab.aspx?sid=" + sid + "&FileID=" + FileID + "&LoanId=" + FileID);

            ChangeTab(tab_index);

            return false;
        }

        function aActions_onclick(tab_index) {

            // common parameters
            var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // set iframe.src
            $("#tabFrame").attr("src", "../LoanDetails/LoanDetailsTask.aspx?sid=" + sid + "&LoanID=" + FileID);

            ChangeTab(tab_index);

            return false;
        }

        function aOpportunityInfo_onclick(tab_index) {

            // common parameters
            var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // set iframe.src
            $("#tabFrame").attr("src", "OpportunityInfotab.aspx?sid=" + sid + "&FileID=" + FileID);

            ChangeTab(tab_index);

            return false;
        }

        function aMarketing_onclick(tab_index) {

            // common parameters
            var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // set iframe.src
            $("#tabFrame").attr("src", "ProspectMailChimpTab.aspx?sid=" + sid + "&FileID=" + FileID);

            ChangeTab(tab_index);

            return false;
        }

        function aSetup_onclick(tab_index) {

            // common parameters
            var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // set iframe.src
            $("#tabFrame").attr("src", "LeadSetupTab.aspx?sid=" + sid + "&FileID=" + FileID);

            ChangeTab(tab_index);

            return false;
        }
        
// ]]>
    </script>
    
    <script language="javascript" type="text/javascript">
        // <!CDATA[

        //#region Show/Close Global Popup

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
        function CloseGlobalPopupNoRefreshPage() {

            $("#divGlobalPopup").dialog("close");
        }

        var RefreshFlag = ""
        function CloseDialog_SendCompletionEmail() {

            $("#divGlobalPopup").dialog("close");
            if (RefreshFlag == "SendCompletionEmail") {
                RefreshPage();
            }
        }

        function ShowWaitingDialog(WaitingMsg, TaskIds, LoanStatus, StageID) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            $("#hfTaskIds").val("");
            $("#hfTaskIds").val(TaskIds);
            $("#hdnLoanStatus").val(LoanStatus);

            $("#btnOK").show();
            $("#btnSkip").show();
            $("#btnCancel").show();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });

            CompleteStageID = StageID;
        }

        function ShowWaitingDialog1(WaitingMsg, StageID, taskID) {

            CompleteStageID = StageID;
            this.sTaskID = taskID;

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

            var LoanID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            // Ajax
            if (SendEmail == "OK" || SendEmail == "Skip") {
                $.getJSON("../LoanDetails/LoanTasksComplete_Background.aspx?sid=" + Radom + "&TaskIDs=" + $("#hfTaskIds").val() + "&SendEmail=" + SendEmail + "&LoanID=" + LoanID, AfterTaskComplete);
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
        function CloseWaitingWindow() {
            $('#divContainer').unblock();
        }
        function CloseLoan(data) {

            var LoanClosed = data.LoanClosed;
            var ErrMsg = data.ErrorMsg;
            var LoanStatus = $("#hdnLoanStatus").val();

            if (LoanClosed == "Yes" && LoanStatus == "Processing") {
                ShowCloseLoanDialog();
                $("#hdnLoanStatus").val("Closed");

            }
            else {
                RefreshPage();
            }

            //setTimeout("CloseWaitingDialog('Completed task successfully.')", 500);
        }

        function ShowCloseLoanDialog() {
            var LoanID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            var f = document.getElementById('iframePF');
            //              alert("ShowCloseLoanDialog before calling the popup....Frame=" + f.name.toString());
            f.src = "../LoanDetails/CloseLoanPopup.aspx?fid=" + LoanID + "&sid=" + Math.random().toString();
            //              $('#dialog1').dialog('open');
            // show modal
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

        function UnCompleteTask(TaskID, StageID) {

            ShowWaitingDialog1("Uncompleting task...", StageID, TaskID);
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            CompleteStageID = StageID;
            this.sTaskID = TaskID;

            var LoanID = jQuery("#<%= this.hfFileID.ClientID %>").val();
            //            var LoanID = GetQueryString1("LoanID");

            // Ajax
            $.getJSON("../LoanDetails/LoanTaskUncomplete_Background.aspx?sid=" + Radom + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterTaskUncomplete);
        }

        function AfterTaskUncomplete(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
            }
            else {
                //setTimeout("CloseWaitingDialog('Uncomplete selected task successfully.')", 500);
                RefreshPage();
            }
        }
        var CompleteStageID = "";
        var sTaskID = "";
        var task_StageScrollTop = 0;
        function RefreshPage() {

            //            if ($.browser.msie == true) {   // ie

            //                window.document.frames("ifrLoanInfo").location.reload();
            //            }
            //            else {   // firefox

            //                window.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
            //            }

            //            if ($.browser.msie == true) {   // ie

            //                window.document.frames("tabFrame").location.reload();
            //            }
            //            else {   // firefox

            //                window.document.getElementById('tabFrame').contentWindow.location.reload();
            //            }
            //            $('#divContainer').unblock();



            if ($.browser.msie == true) {   // ie

                window.document.frames("ifrLoanInfo").location.reload();
            }
            else {   // firefox

                window.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
            }

            //alert("xx: "+CompleteStageID);
            var taskframe = null; //gdc 20111202 find Task Page   localpage ->iframe tabFrame->LoanActions.aspx ->iframe tabFrame -> LoanDetailsTask.aspx (find this)
            if ($.browser.msie == true) {
                taskframe = window.document.frames("tabFrame");
                //taskframe = tabFrame.document.frames("tabFrame");//cr48
            }
            else {
                taskframe = window.document.getElementById('tabFrame').contentWindow;
                //taskframe = taskframe.document.getElementById('tabFrame').contentWindow; //cr48

            }

            if (taskframe.StageScrollTop != null) {
                this.task_StageScrollTop = taskframe.StageScrollTop();
            }

            if (CompleteStageID == "") {
                taskframe.location.reload();
            }
            else {
                var url = taskframe.location.href;
                var StageID = GetQueryString2("Stage", url);
                if (StageID == "") {
                    url = url + "&Stage=" + CompleteStageID;
                }
                else {
                    url = url.replace(/Stage=\d+/, "Stage=" + CompleteStageID);
                }

                if (sTaskID != undefined && sTaskID != "") {
                    var urlTaskID = GetQueryString2("TaskID", url);
                    if (urlTaskID == "") {
                        url = url + "&TaskID=" + sTaskID;
                    }
                    else {
                        url = url.replace(/TaskID=\d+/, "TaskID=" + sTaskID);
                    }
                }


                taskframe.location.href = url;
            }

            CompleteStageID = "";
            this.sTaskID = "";
            $('#divContainer').unblock();

        }

        function RefreshTopAndBottomIframe() {

            if ($.browser.msie == true) {   // ie

                window.document.frames("ifrLoanInfo").location.reload();
                window.document.frames("tabFrame").location.reload();
            }
            else {   // firefox

                window.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
                window.document.getElementById('tabFrame').contentWindow.location.reload();
            }
        }

        //#endregion

        // ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        <div id="content">
            <iframe id="ifrLoanInfo" frameborder="0" scrolling="no" style="width: 1056px; height: 320px;" src="ProspectLoanDetailsInfo.aspx?LoanID=<%= this.iFileID %>"></iframe>
        </div>
        <div class="JTab" style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 10px;">&nbsp;</td>
                    <td>
                        <div id="tabs10">
                            <ul> 
                                <li><a id="aBorrowerInfo" href="" onclick="return aBorrowerInfo_onclick(0)"><span>Borrower Info</span></a></li>
                                <li id="current"><a id="aActions" href="" onclick="return aActions_onclick(1)"><span>Actions</span></a></li>
                                <li><a id="aOpportunityInfo" href="" onclick="return aOpportunityInfo_onclick(2)"><span>Opportunity Info</span></a></li>
                                <li><a id="aMarketing" href="" onclick="return aMarketing_onclick(3)"><span>Marketing</span></a></li>
                                <li><a id="aSetup" href="" onclick="return aSetup_onclick(4)"><span>Setup</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
                <div id="TabLine2" class="TabRightLine">&nbsp;</div>
                <div class="TabContent">
                    <iframe id="tabFrame" frameborder="0" style="border: solid 0px blue; height: 720px; width: 1037px;"></iframe>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfFileID" runat="server" />
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
                <td style="padding-left: 5px; width:320px;">
                    <label id="WaitingMsg" style="color: #818892; font-weight: bold;">
                    </label>
                    &nbsp;&nbsp; <a id="aClose" href="javascript:RefreshPage()" style="font-weight: bold;
                        color: #6182c1;">[Close]</a>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfTaskIds" runat="server" />
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