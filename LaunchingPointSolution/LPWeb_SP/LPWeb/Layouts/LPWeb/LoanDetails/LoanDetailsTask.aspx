<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailsTask.aspx.cs"
    Inherits="LoanDetails_LoanDetailsTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan Details - Tasks Tab</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.treeview.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.treeview.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
// <![CDATA[
        // who is parent

        var taskBaseParent = window.parent;

        if (parent.document.location.href.indexOf("LoanActions.aspx") != -1) {
            taskBaseParent = window.parent.parent;
        }



        $(document).ready(function () {

            DrawTab();

            // add event
            $("#ddlTaskStatus").change(ddlTaskStatus_onchange);
            $("#ddlTaskOwner").change(ddlTaskOwner_onchange);
            $("#ddlStage").change(ddlStage_onchange);
            $("#ddlDue").change(ddlDue_onchange);
            $("#ulStageTree :checkbox").click(CheckToCompleteTask);

            InitFilter();

            InitStageTree();

            SetTaskTreeAndListHeight();

            InitTableSorter();

            //#region Disabled

            var LoanStatus = $("#hdnLoanStatus").val();
            if (LoanStatus != "Processing" && LoanStatus != "Prospect") {

                if (LoanStatus == "Closed") {

                    var IsPostCloseUncompleted = $("#hdnIsPostCloseUncompleted").val();
                    if (IsPostCloseUncompleted == "true") {

                        DisableLink("aChangeEstClose");
                    }
                    else {

                        DisableAll();
                    }
                }
                else if (LoanStatus == "Canceled" || LoanStatus == "Denied" || LoanStatus == "Suspended") {

                    DisableAll();
                }
            }

            //#endregion

            //#region Privilege

            if ($("#hdnCustomTask").val().indexOf('1') == -1) {

                DisableLink("aAdd");
            }
            if ($("#hdnCustomTask").val().indexOf('2') == -1) {

                DisableLink("aUpdate");
                DisableLink("aDefer");
            }
            if ($("#hdnCustomTask").val().indexOf('3') == -1) {

                DisableLink("aDelete");
            }
            if ($("#hdnAssignTask").val() == "0") {

                DisableLink("aReassign");
            }

            //#endregion


            //task_StageScrollTop
            $("#StageTreemain").scrollTop(taskBaseParent.task_StageScrollTop);

        });

        function StageScrollTop() {

            return $("#StageTreemain").scrollTop();
        }

        function SetTaskTreeAndListHeight() {

            // set height of left/right div
            var TaskTreeDivHeight = $("#divLeftTaskTree").height();
            var TaskListDivHeight = $("#divRightTaskList").height();
            //            alert(TaskTreeDivHeight);
            //            alert(TaskListDivHeight);
            if (TaskTreeDivHeight > TaskListDivHeight) {

                $("#divRightTaskList").height(TaskTreeDivHeight - 9);
            }

            if (TaskTreeDivHeight < TaskListDivHeight) {

                $("#divLeftTaskTree").height(TaskListDivHeight + 9);
            }
        }

        function InitStageTree() {

            $("#ulStageTree").treeview({
                persist: "location",
                collapsed: true,
                unique: true
            });

            if ($.browser.msie == true) {

                if ($.browser.version == "8.0") {

                    $("#ulStageTree :checkbox").attr("class", "TaskCheckbox8");

                    $("#ulStageTree img[class=TaskIcon]").attr("class", "TaskIcon8");
                }
            }

            $("#ulStageTree").show();
        }

        function InitTableSorter() {

            $("#gridTaskList").tablesorter({

                headers: {
                    0: { sorter: false }
                },
                widgets: ['zebra']
            });
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridTaskList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridTaskList tr td :checkbox").attr("checked", "");
            }
        }

        //#region Add Task

        function ShowDialog_AddTask() {

            var QueryString = "";
            var LoanID = GetQueryString1("LoanID");
            QueryString = "&LoanID=" + LoanID

            var Stage = GetQueryString1("Stage");
            if (Stage != "") {

                QueryString += "&Stage=" + Stage
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrAddTask").attr("src", "TaskCreate.aspx?sid=" + RadomStr + QueryString);

            // show modal
            $("#divAddTask").dialog({
                height: 435,
                width: 610,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_AddTask() {

            $("#divAddTask").dialog("close");
        }

        //#endregion

        //#region Edit Task

        function ShowDialog_EditTask() {

            var LoanID = GetQueryString1("LoanID");
            if (LoanID == "") {

                alert("Missing required query string, quit.");
                return;
            }

            var SelectedCount = $("#gridTaskList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one task can be selected.");
                return;
            }

            var TaskID = $("#gridTaskList tr:not(:first) td :checkbox:checked").attr("myTaskID");

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrEditTask").attr("src", "TaskEdit.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&TaskID=" + TaskID);

            // show modal
            $("#divEditTask").dialog({
                height: 435,
                width: 610,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // show popup for edit task
        function ShowDialog_EditTask1(LoanID, TaskID) {

            if (LoanID == "") {

                alert("Missing required query string, quit.");
                return;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrEditTask").attr("src", "TaskEdit.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&TaskID=" + TaskID);

            // show modal
            $("#divEditTask").dialog({
                height: 435,
                width: 610,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_EditTask() {

            $("#divEditTask").dialog("close");
        }

        //#endregion

        //#region Reassign Task

        function ShowDialog_ReassignTask() {

            var SelectedCount = $("#gridTaskList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }

            var HasCompleteTask = false;
            $("#gridTaskList tr:not(:first) td :checkbox:checked").each(function () {

                // if completed, can't re-complete task
                var CompleteDate = $(this).attr("myCompletedDate");
                if (CompleteDate != "") {

                    HasCompleteTask = true;
                }
            });

            if (HasCompleteTask == true) {

                alert("One of selected tasks has been completed, can't be reassign.");
                return;
            }

            var TaskIDs = GetSelectedTaskIDs();
            var LoanID = GetQueryString1("LoanID");

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrReassignTask").attr("src", "LoanTaskReassign.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&TaskIDs=" + TaskIDs);

            // show modal
            $("#divReassignTask").dialog({
                height: 340,
                width: 610,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
        }

        function CloseDialog_ReassignTask() {

            $("#divReassignTask").dialog("close");
        }

        //#endregion

        //#region Filters

        function BuildQueryString_Filter() {

            var QueryString = "";
            var LoanID = GetQueryString1("LoanID");
            QueryString = "LoanID=" + LoanID;

            // Task Status
            var TaskStatus = $("#ddlTaskStatus").val();
            if (TaskStatus != "All") {

                QueryString += "&TaskStatus=" + TaskStatus;
            }

            // Task Owner
            var TaskOwner = $("#ddlTaskOwner").val();
            if (TaskOwner != "0") {

                QueryString += "&TaskOwner=" + TaskOwner;
            }

            // Stage
            var Stage = $("#ddlStage").val();
            if (Stage != "0") {

                QueryString += "&StageFilter=" + Stage;
            }

            // Due
            var Due = $("#ddlDue").val();
            if (Due != "All") {

                QueryString += "&Due=" + Due;
            }

            window.location.href = window.location.pathname + "?" + QueryString;
        }

        function ddlTaskStatus_onchange() {

            BuildQueryString_Filter();
        }

        function ddlTaskOwner_onchange() {

            BuildQueryString_Filter();
        }

        function ddlStage_onchange() {

            BuildQueryString_Filter();
        }

        function ddlDue_onchange() {

            BuildQueryString_Filter();
        }

        function InitFilter() {

            // TaskStatus
            var TaskStatus = GetQueryString1("TaskStatus");
            if (TaskStatus != "") {
                $("#ddlTaskStatus").val(TaskStatus);
            }

            // TaskOwner
            var TaskOwner = GetQueryString1("TaskOwner");
            if (TaskOwner != "") {
                $("#ddlTaskOwner").val(TaskOwner);
            }

            // StageFilter
            var Stage = GetQueryString1("StageFilter");
            if (Stage != "") {
                $("#ddlStage").val(Stage);
            }

            // Due
            var Due = GetQueryString1("Due");
            if (Due != "") {
                $("#ddlDue").val(Due);
            }
        }

        //#endregion

        function Stage_onclick(alink) {

            // parse self href
            var QueryString = "";
            var LoanID = GetQueryString2("LoanID", alink.href);
            QueryString = "LoanID=" + LoanID;

            var Stage = GetQueryString2("Stage", alink.href);
            QueryString += "&Stage=" + Stage;

            // parse url query-strings

            // Task Status
            var TaskStatus = GetQueryString1("TaskStatus");
            if (TaskStatus != "") {

                QueryString += "&TaskStatus=" + TaskStatus;
            }

            // Task Owner
            var TaskOwner = GetQueryString1("TaskOwner");
            if (TaskOwner != "") {

                QueryString += "&TaskOwner=" + TaskOwner;
            }

            // Stage
            var StageFilter = GetQueryString1("StageFilter");
            if (StageFilter != "") {

                QueryString += "&StageFilter=" + StageFilter;
            }

            // Due
            var Due = GetQueryString1("Due");
            if (Due != "") {

                QueryString += "&Due=" + Due;
            }

            window.location.href = "LoanDetailsTask.aspx?" + QueryString;
            return false;
        }

        function Task_onclick(alink) {

            // parse self href
            var QueryString = "";
            var LoanID = GetQueryString2("LoanID", alink.href);
            QueryString = "LoanID=" + LoanID;

            var Stage = GetQueryString2("Stage", alink.href);
            QueryString += "&Stage=" + Stage;

            var TaskID = GetQueryString2("TaskID", alink.href);
            QueryString += "&TaskID=" + TaskID;

            // parse url query-strings

            // Task Status
            var TaskStatus = GetQueryString1("TaskStatus");
            if (TaskStatus != "") {

                QueryString += "&TaskStatus=" + TaskStatus;
            }

            // Task Owner
            var TaskOwner = GetQueryString1("TaskOwner");
            if (TaskOwner != "") {

                QueryString += "&TaskOwner=" + TaskOwner;
            }

            // StageFilter
            var StageFilter = GetQueryString1("StageFilter");
            if (StageFilter != "") {

                QueryString += "&StageFilter=" + StageFilter;
            }

            // Due
            var Due = GetQueryString1("Due");
            if (Due != "") {

                QueryString += "&Due=" + Due;
            }

            window.location.href = "LoanDetailsTask.aspx?" + QueryString;
            return false;
        }

        function DisableAll() {

            $("#gridTaskList :checkbox").attr("disabled", "true");

            $(".ToolStrip a").each(function () {

                $(this).attr("disabled", "true");
                $(this).removeAttr("href");
                $(this).css("text-decoration", "none");
            });
        }

        //#region Show/Close Waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
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

        //#endregion

        //#region Change Est. Close

        function ShowDialog_ChangeEstDate() {

            // show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            // check point file locked
            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + LoanID, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        $("#txtNewDate").val("");
                        $("#txtNewDate").datepick('destroy');
                        // show modal
                        $("#divChangeEstDate").dialog({
                            height: 150,
                            width: 350,
                            modal: true,
                            resizable: false
                        });
                        $(".ui-dialog").css("border", "solid 3px #aaaaaa")
                        $("#txtNewDate").datepick();
                        $("#txtNewDate").focus();
                    }
                }
            });


        }

        function CloseDialog_ChangeEstDate() {

            $("#divChangeEstDate").dialog("close");
        }

        function ChangeEstCloseDate() {

            // validate
            var NewDate = $("#txtNewDate").val();
            if (NewDate == "") {

                alert("Please enter New Est Close Date.");
                return;
            }

            if (isDate(NewDate, "MM/dd/yyyy") == false) {

                alert("please enter a valid date，e.g." + formatDate(new Date(), 'MM/dd/yyyy') + ".");
                return;
            }

            // close date pick
            CloseDialog_ChangeEstDate();

            // show waiting dialog
            ShowWaitingDialog("Changing Est Close Date...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = GetQueryString1("LoanID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("ChangeEstClose_Background.aspx?sid=" + Radom + "&LoanID=" + LoanID + "&NewDate=" + encodeURIComponent(NewDate) + "&LoginUserID=" + LoginUserID, AfterChangeEstCloseDate);
        }

        function AfterChangeEstCloseDate(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
                return;
            }

            var Result = confirm("Changed est close date successfully. Do you want to recalculate the due dates of all the pending tasks?");
            if (Result == false) {
                RefreshPage();
                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Recalculating due dates...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = GetQueryString1("LoanID");
            var LoginUserID = $("#hdnLoginUserID").val();
            var NewDate = $("#txtNewDate").val();

            // Ajax
            $.getJSON("RecalculateDueDate_Background.aspx?sid=" + Radom + "&LoanID=" + LoanID + "&NewDate=" + encodeURIComponent(NewDate) + "&LoginUserID=" + LoginUserID, AfterRecalculateDueDate);
        }

        //#endregion

        function AfterRecalculateDueDate(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
                return;
            }

            // close waiting dialog
            setTimeout("CloseWaitingDialog('Recalculated due dates successfully.')", 500);
        }

        //#region Regnerate

        function Regenerate() {

            var Result = confirm("This will delete all pending tasks and regenerate tasks specified in the workflow template in the current loan. This operation is not reversible. \r\n\r\nAre you sure you want to continue?");
            if (Result == false) {

                return;
            }

            var LoanWflTempID = $("#hdnLoanWflTempID").val();
            var DefaultWflTempID = $("#hdnDefaultWflTempID").val();
            var DefaultWflTempName = $("#hdnDefaultWflTempName").val();

            var WorkflowTemplateID = "";
            if (LoanWflTempID == "") {

                if (DefaultWflTempID == "") {

                    alert("The workflow template for this loan has not yet been selected. To select and apply a workflow template for your loan, please click the Workflow Setup tab.");
                    return;
                }
                else {

                    var xRe = confirm("The workflow template for this loan has not yet been selected. \r\n\r\nDo you want to apply the default workflow template, <" + DefaultWflTempName + ">?");
                    if (xRe == false) {

                        return;
                    }

                    WorkflowTemplateID = DefaultWflTempID;
                }
            }
            else {

                WorkflowTemplateID = LoanWflTempID;
            }


            // show waiting dialog
            ShowWaitingDialog("Regenerating...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = GetQueryString1("LoanID");

            // Ajax
            $.getJSON("Regenerate_Background.aspx?sid=" + Radom + "&LoanID=" + LoanID + "&WorkflowTemplateID=" + WorkflowTemplateID, AfterRegenerate);
        }

        function AfterRegenerate(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
                return;
            }

            setTimeout("CloseWaitingDialog('Regenerated successfully.')", 500);
        }

        //#endregion

        function GetSelectedTaskIDs() {

            var TaskIDs = "";
            $("#gridTaskList tr:not(:first) td :checkbox:checked").each(function (i) {

                var TaskID = $(this).attr("myTaskID");
                if (i == 0) {

                    TaskIDs = TaskID;
                }
                else {

                    TaskIDs += "," + TaskID;
                }
            });

            return TaskIDs;
        }

        //#region Complate Task

        function aComplete_onclick() {

            var flag = false;
            var SelectedCount = $("#gridTaskList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }

            // if completed, can't re-complete task
            $("#gridTaskList tr:not(:first) td :checkbox:checked").each(function () {
                var CompleteDate = $(this).attr("myCompletedDate");
                if (CompleteDate != "") {

                    alert(" One of the selected task has been completed, can't re-complete.");
                    flag = true;
                    return false;
                }
            });

            if (flag) {
                return;
            }

            //            var TaskIDs = $("#gridTaskList tr:not(:first) td :checkbox:checked").attr("myTaskID");
            var TaskIDs = getSelectedItemsIDs();

            $("#gridTaskList tr:not(:first) td :checkbox:checked").each(function () {
                var item = $(this);
                var TaskID = item.attr("myTaskID");

                // complete others' task
                var TaskOwner = item.attr("myTaskOwner");
                var LoginUserID = $("#hdnLoginUserID").val();
                var CompleteOtherTask = $("#hdnCompleteOtherTask").val();
                if (CompleteOtherTask == "False") {

                    if ((LoginUserID != TaskOwner) && (TaskOwner)) {

                        alert("You do not have the privilege to complete tasks that are not assigned to you.");

                        flag = true;

                        return false;
                    }
                }

            });
            var StageID = GetQueryString1("Stage");

            if (flag) {
                return;
            }
            else if (SelectedCount > 1) {

                CompleteTask(TaskIDs, $("#hdnLoanStatus").val(), StageID);
            }
            else {

                CompleteTask1(TaskIDs, StageID);
            }
        }

        function getSelectedItemsIDs() {
            var selctedItems = new Array();
            $("#gridTaskList tr:not(:first) td :checkbox").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("myTaskID"));
                }
            });
            return selctedItems;
        }

        function CompleteTask(TaskIDs, LoanStatus, StageID) {

            //            taskBaseParent.ShowWaitingDialog("Completing task...", TaskIDs, LoanStatus);
            taskBaseParent.ShowWaitingDialog("Multiple tasks selected have completion emails.  Please press OK to continue, Cancel to exit or Skip to bypass the completion emails but still complete the tasks.", TaskIDs, LoanStatus, StageID);

        }

        function CompleteTask1(TaskID, StageID) {

            // show waiting dialog
            taskBaseParent.ShowWaitingDialog1("Completing task...", StageID, TaskID);

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = GetQueryString1("LoanID");
            // Ajax
            $.getJSON("LoanTaskComplete_Background.aspx?sid=" + Radom + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterTaskComplete1);
        }

        function AfterTaskComplete(data) {

            if (data.ExecResult == "Failed") {

                taskBaseParent.ComplateError(data.ErrorMsg);
                return;
            }

            $("#hdnErrMsg").val(data.ErrorMsg);
            CloseLoan(data);
        }
        var RefreshFlag = "";

        function AfterTaskComplete1(data) {
            RefreshFlag = "";
            if (data.ExecResult == "Failed") {
                taskBaseParent.ComplateError(data.ErrorMsg);
                return;
            }

            //taskBaseParent.CompleteStageID = GetQueryString1("Stage");

            var LoanClosed = data.LoanClosed;
            var LoanStatus = $("#hdnLoanStatus").val();

            if (LoanClosed == "Yes" && LoanStatus == "Processing") {
                RefreshFlag = "CloseLoan";
            }

            // show send completion email
            if (isID(data.EmailTemplateID) == true) {

                if (RefreshFlag == "") {
                    RefreshFlag = "SendCompletionEmail";
                }
                taskBaseParent.RefreshFlag = RefreshFlag;
                ShowDialog_SendCompletionEmail(data.EmailTemplateID, data.TaskID);
            }

            //if (RefreshFlag == "") {
            //    taskBaseParent.RefreshPage();
            //}
            taskBaseParent.RefreshFlag = RefreshFlag;

            $("#hdnErrMsg").val(data.ErrorMsg);
            taskBaseParent.CloseLoan(data);
            if (RefreshFlag == "CloseLoan") {
                CloseLoan(data);
            }
        }

        function CheckToCompleteTask() {

            TaskID = $(this).attr("myTaskID");
            StageID = $(this).attr("myStageID");
            var Checked = $(this).attr("checked");

            //alert(TaskID);

            // complete others' task
            var TaskOwner = $("#gridTaskList tr td :checkbox[mytaskid=" + TaskID + "]").attr("mytaskowner");
            //alert(TaskOwner);
            var LoginUserID = $("#hdnLoginUserID").val();
            var CompleteOtherTask = $("#hdnCompleteOtherTask").val();
            //CompleteOtherTask = "False";
            if (CompleteOtherTask == "False") {

                if ((LoginUserID != TaskOwner) && (TaskOwner)) {

                    alert("You do not have the privilege to complete/uncomplete tasks that are not assigned to you.");

                    if (Checked == true) {

                        $(this).attr("checked", "");
                    }
                    else {

                        $(this).attr("checked", "true");
                    }
                    return;
                }
            }

            if (Checked == true) {

                CompleteTask1(TaskID, StageID);
            }
            else {

                UnCompleteTask(TaskID, StageID);
            }
        }

        //#endregion

        //#region Uncomplete Task

        function UnCompleteTask(TaskID, StageID) {

            // show waiting dialog
            taskBaseParent.UnCompleteTask(TaskID, StageID);
            //            ShowWaitingDialog("Uncompleting task...");

            //            var RadomNum = Math.random();
            //            var Radom = RadomNum.toString().substr(2);

            //            var LoanID = GetQueryString1("LoanID");

            //            // Ajax
            //            $.getJSON("LoanTaskUncomplete_Background.aspx?sid=" + Radom + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterTaskUncomplete);
        }

        function AfterTaskUncomplete(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
            }
            setTimeout("CloseWaitingDialog('Uncompleted selected task successfully.')", 500);
        }

        //#endregion

        //#region Delete Task

        function aDelete_onclick() {

            var SelectedCount = $("#gridTaskList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }

            var HasCompleteTask = false;
            $("#gridTaskList tr:not(:first) td :checkbox:checked").each(function () {

                // if completed, can't re-complete task
                var CompleteDate = $(this).attr("myCompletedDate");
                if (CompleteDate != "") {

                    HasCompleteTask = true;
                }
            });

            if (HasCompleteTask == true) {

                alert("One of selected tasks has been completed, can't be deleted.");
                return;
            }

            var Result = confirm("This operation will not be reversible. Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Deleting task...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var TaskIDs = GetSelectedTaskIDs();
            var LoanID = GetQueryString1("LoanID");

            // Ajax
            $.getJSON("LoanTaskDelete_Background.aspx?sid=" + Radom + "&TaskIDs=" + encodeURIComponent(TaskIDs) + "&LoanID=" + LoanID, AfterTaskDelete);
        }

        function AfterTaskDelete(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
                return;
            }

            CloseLoan(data);
            //            setTimeout("CloseWaitingDialog('Delete selected task(s) successfully.')", 2000);
        }

        //#endregion

        //#region Defer Task

        function ShowDialog_DeferTask() {

            var SelectedCount = $("#gridTaskList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }

            var HasCompleteTask = false;
            $("#gridTaskList tr:not(:first) td :checkbox:checked").each(function () {

                // if completed, can't re-complete task
                var CompleteDate = $(this).attr("myCompletedDate");
                if (CompleteDate != "") {

                    HasCompleteTask = true;
                }
            });

            if (HasCompleteTask == true) {

                alert("One of selected tasks has been completed, can't be defered.");
                return;
            }

            var TaskIDs = GetSelectedTaskIDs();

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrDeferTask").attr("src", "LoanTaskDefer.aspx?sid=" + RadomStr + "&TaskIDs=" + TaskIDs);

            // show modal
            $("#divDeferTask").dialog({
                height: 130,
                width: 310,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_DeferTask() {

            $("#divDeferTask").dialog("close");
        }

        //#endregion

        //#region Send Completion Email

        function ShowDialog_SendCompletionEmail(EmailTemplateID, TaskID) {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/EmailSendCompletionPopup.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&EmailTemplateID=" + EmailTemplateID + "&TaskID=" + TaskID + "&CloseDialogCodes=window.parent.CloseDialog_SendCompletionEmail()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 380;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            taskBaseParent.ShowGlobalPopup("Send Completion Email", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function CloseDialog_SendCompletionEmail() {
            taskBaseParent.CloseGlobalPopup();
            //$("#divSendCompletionEmail").dialog("close");
        }

        //#endregion

        function CloseLoan(data) {

            var LoanClosed = data.LoanClosed;
            var ErrMsg = data.ErrorMsg;
            var LoanStatus = $("#hdnLoanStatus").val();

            if (LoanClosed == "Yes" && LoanStatus == "Processing") {
                ShowCloseLoanDialog();
                $("#hdnLoanStatus").val("Closed");

            }

            setTimeout("CloseWaitingDialog('Completed task successfully.')", 500);
        }
        function ShowCloseLoanDialog() {


            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "../LoanDetails/CloseLoanPopup.aspx?sid=" + RadomStr + "&fid=" + LoanID + "&CloseDialogCodes=taskBaseParent.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 650;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            taskBaseParent.ShowGlobalPopup("CloseLoan", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

            return false;
        }
        function CloseDialog_CloseLoan() {

            taskBaseParent.CloseGlobalPopup();
            //$("#dialog1").dialog("close");
        }

        var CompleteStageID = "";
        function RefreshPage() {

            if ($.browser.msie == true) {   // ie

                taskBaseParent.document.frames("ifrLoanInfo").location.reload();
            }
            else {   // firefox

                taskBaseParent.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
            }

            // refresh current page
            if (CompleteStageID == "") {

                window.location.href = window.location.href;
            }
            else {

                var StageID = GetQueryString1("Stage");
                if (StageID == "") {

                    window.location.href = window.location.href + "&Stage=" + StageID;
                }
            }
        }

        //#region CR48 fake tab navigation

        function aTasks_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsTask.aspx?sid=" + sid + "&LoanID=" + LoanID + "&ref=" + sRef;
        }

        function aAlerts_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsAlertTab.aspx?sid=" + sid + "&ref=" + sRef + "&LoanID=" + LoanID;
        }

        function aEmails_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../Prospect/LoanDetailEmailTab.aspx?from=2&itemid=" + LoanID + "&sid=" + sid + "&ref=" + sRef;

        }

        function aNotes_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanNoteList.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }

        function aActivityHistory_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/Activity.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }


        //#endregion

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="JTab" style="margin-top:10px;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <%
                            bool isOnlytab = false;

                            //string rUrl = Request.UrlReferrer.AbsoluteUri;

                            //if (rUrl.Contains("LoanDetail.aspx"))
                            //{
                            //    isOnlytab = true;
                            //}
                            if (this.Request.QueryString["ref"] != null && this.Request.QueryString["ref"].ToString() == "loan")
                            {
                                isOnlytab = true;
                            }
                         %>
                        <ul>
                            <li id="current"><a id="aTasks" href="javascript:aTasks_onclick()"><span>Tasks</span></a></li>
                            <%if (!isOnlytab)
                              { %>
                            <li sid="otherTab"><a id="aAlerts" href="javascript:aAlerts_onclick()"><span>Rule Alerts</span></a></li>
                            <li sid="otherTab"><a id="aEmails" href="javascript:aEmails_onclick()"><span>Emails</span></a></li>
                            <li sid="otherTab"><a id="aNotes" href="javascript:aNotes_onclick()"><span>Notes</span></a></li>
                            <li sid="otherTab"><a id="aActivityHistory" href="javascript:aActivityHistory_onclick()"><span>Activity History</span></a></li>
                            <%} %>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <div id="TabBody" style="margin-bottom:10px; padding-bottom:10px;">
            <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
            <div id="TabLine2" class="TabRightLine">&nbsp;</div>
            <div class="TabContent">



    <div id="divContainer" style="width: 1000px; border: solid 0px red;">
        <div id="divFilter" style="margin-top: 5px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Task Status
                    </td>
                    <td style="padding-left: 10px;">
                        <select id="ddlTaskStatus" style="width: 100px;">
                            <option>All</option>
                            <option>Complete</option>
                            <option>Incomplete</option>
                        </select>
                    </td>
                    <td style="padding-left: 25px;">
                        Task Owner
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlTaskOwner" runat="server" DataValueField="UserID" DataTextField="FullName"
                            Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 25px;">
                        Stage
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlStage" runat="server" DataValueField="LoanStageID" DataTextField="StageName"
                            Width="100px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 25px;">
                        Due
                    </td>
                    <td style="padding-left: 10px;">
                        <select id="ddlDue" style="width: 100px;">
                            <option value="All">All</option>
                            <option value="In30">Due in 30 days</option>
                            <option value="In14">Due in two weeks</option>
                            <option value="In7">Due in one week</option>
                            <option value="In1">Due tomorrow</option>
                            <option value="In0">Due today</option>
                            <option value="Overdue">Overdue</option>
                        </select>
                    </td>
                </tr>
            </table>
        </div>
        <table cellpadding="0" cellspacing="0" style="width: 100%; margin-top: 15px;">
            <tr>
                <td style="vertical-align: top; width: 300px;">
                    <div id="divLeftTaskTree" style="border: solid 1px #e4e7ef;">
                        <div style="border-bottom: solid 1px #e4e7ef;">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aChangeEstClose" href="javascript:ShowDialog_ChangeEstDate()">Change Est
                                    Close</a><span>|</span></li>
                                <li><a id="aRegenerate" href="javascript:Regenerate()">Regenerate</a></li>
                            </ul>
                        </div>
                        <div id="StageTreemain" style="margin-left: 5px; margin-top: 10px; height:470px; overflow:auto;">
                            <ul id="ulStageTree" style="list-style: none; display: none;">
                                <%--<li><img class="StageIcon" src="../images/stage/complete.png" /><a onclick="return Stage_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106"><span>Open 05/02/2010</span></a>
			                        <ul>
                                        <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/task/complete.png" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=1"><span title="Obtain bank statements 05/20/2010">Obtain bank statements 05/20/2010</span></a></li>
				                        <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/task/pending.png" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=2"><span title="Order Appraisal Report">Order Appraisal Report</span></a></li>
				                        <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/task/unfinished.png" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=3"><span title="Home Owners Insurance deck page">Home Owners Insurance deck page</span></a>
                                            <ul>
                                                <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/task/overdue.png" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=4"><span title="Obtain bank statements">Obtain bank statements</span></a></li>
                                            </ul>
                                        </li>
			                        </ul>
		                        </li>
		                        <li><img class="StageIcon" src="../images/stage/pending.png" /><a href="LoanDetailsTask.aspx?LoanID=283&Stage=Submit"><span>Submit</span></a>
			                        <ul>
				                        <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/treeview/file.gif" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=5"><span>Item 2.0</span></a>
                                        <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/treeview/file.gif" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=6"><span>Item 2.1</span></a>
			                        </ul>
		                        </li>
		                        <li><img class="StageIcon" src="../images/stage/overdue.png" /><a href="LoanDetailsTask.aspx?LoanID=283&Stage=Approve"><span>Approve</span></a>
			                        <ul>
				                        <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/treeview/file.gif" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=7"><span>Item 3.0</span></a>
                                        <li><input type="checkbox" class="TaskCheckbox" /><img class="TaskIcon" src="../images/treeview/file.gif" /><a onclick="return Task_onclick(this);" href="LoanDetailsTask.aspx?LoanID=283&Stage=1106&TaskID=8"><span>Item 3.1</span></a>
			                        </ul>
		                        </li>--%>
                                <asp:Literal ID="ltrStageTaskNodes" runat="server"></asp:Literal>
                            </ul>
                        </div>
                    </div>
                </td>
                <td style="vertical-align: top; padding-left: 10px;">
                    <div id="divRightTaskList" style="border: solid 1px #e4e7ef; padding: 0px 10px 10px 10px;">
                        <div id="divToolBar">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aComplete" href="javascript:aComplete_onclick()">Complete</a><span>|</span></li>
                                <li><a id="aReassign" href="javascript:ShowDialog_ReassignTask()">Assign</a><span>|</span></li>
                                <li><a id="aAdd" href="javascript:ShowDialog_AddTask()">Add</a><span>|</span></li>
                                <li><a id="aUpdate" href="javascript:ShowDialog_EditTask()">Update</a><span>|</span></li>
                                <li><a id="aDefer" href="javascript:ShowDialog_DeferTask()">Defer</a><span>|</span></li>
                                <li><a id="aDelete" href="javascript:aDelete_onclick()">Delete</a></li>
                            </ul>
                        </div>
                        <div id="divTaskList" class="ColorGrid" style="margin-top: 5px; height:470px; overflow:auto;  ">
                            <asp:GridView ID="gridTaskList" runat="server" DataKeyNames="LoanTaskId" EmptyDataText="There is no loan task."
                                AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid tablesorter" GridLines="None" Width ="650">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="Checkbox2" type="checkbox" mytaskname="<%# Eval("Name") %>" mytaskid="<%# Eval("LoanTaskID")%>"
                                                myprerequisiteid="<%# Eval("PrerequisiteTaskId")%>" mytaskowner="<%# Eval("Owner")%>"
                                                mycompleteddate="<%# Eval("Completed")%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <img style=" top: 2px;" src="../images/task/<%# this.GetLoanTaskIconFileName(Convert.ToInt32(Eval("LoanTaskId"))) %>" />
                                            <a href="javascript:ShowDialog_EditTask1('<%# Eval("FileID")%>', '<%# Eval("LoanTaskId")%>')">
                                                <%# Eval("Name")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# this.FormatDueDate(Eval("Due")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Assigned/Completed By" ItemStyle-Width="130px"  ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                        <%# (Eval("CompletedBy") == DBNull.Value || Eval("CompletedBy").ToString() == "0") ? Eval("OwnerName") : Eval("CompletedByName")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

        </div>
        </div>
    </div>


    <div id="divHiddenFields" style="display: none;">
        <input id="hdnLoanStatus" runat="server" type="text" style="display: none;" />
        <input id="hdnLoginUserID" runat="server" type="text" style="display: none;" />
        <input id="hdnCustomTask" runat="server" type="text" style="display: none;" />
        <input id="hdnAssignTask" runat="server" type="text" style="display: none;" />
        <asp:HiddenField ID="hdnCompleteOtherTask" runat="server" />
        <asp:HiddenField ID="hdnIsPostCloseUncompleted" runat="server" />
        <input id="hdnErrMsg" runat="server" type="text" style="display: none;" />
        <asp:HiddenField ID="hdnLoanWflTempID" runat="server" />
        <asp:HiddenField ID="hdnDefaultWflTempID" runat="server" />
        <asp:HiddenField ID="hdnDefaultWflTempName" runat="server" />
    </div>
    <div id="divAddTask" title="Loan Task Setup" style="display: none;">
        <iframe id="ifrAddTask" frameborder="0" scrolling="no" width="580px" height="400px">
        </iframe>
    </div>
    <div id="divEditTask" title="Loan Task Setup" style="display: none;">
        <iframe id="ifrEditTask" frameborder="0" scrolling="no" width="580px" height="400px">
        </iframe>
    </div>
    <div id="divReassignTask" title="Reassign Loan Task" style="display: none;">
        <iframe id="ifrReassignTask" frameborder="0" scrolling="auto" width="580px" height="300px">
        </iframe>
    </div>
    <div id="divDeferTask" title="Defer Loan Task" style="display: none;">
        <iframe id="ifrDeferTask" frameborder="0" scrolling="auto" width="280px" height="90px">
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
    </div>
    <div id="divChangeEstDate" title="Change Est Close Date" style="display: none;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    New Est Close Date:
                </td>
                <td style="padding-left: 10px;">
                    <asp:TextBox ID="txtNewDate" runat="server" CssClass="DateField" Width="120px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0" style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>
                        <input id="btnChange" type="button" value="Change" class="Btn-66" onclick="ChangeEstCloseDate()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel8" type="button" value="Cancel" class="Btn-66" onclick="CloseDialog_ChangeEstDate()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divSendCompletionEmail" title="Send Completion Email" style="display: none;">
        <iframe id="ifrSendCompletionEmail" frameborder="0" scrolling="no" width="605px"
            height="340px"></iframe>
    </div>
    <div id="dialog1" title="Select Closed Loan Folder" style="display: none;">
        <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
        </iframe>
    </div>
    </form>
</body>
</html>
