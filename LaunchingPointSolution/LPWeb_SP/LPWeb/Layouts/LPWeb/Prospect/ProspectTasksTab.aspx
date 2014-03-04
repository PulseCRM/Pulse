<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectTasksTab.aspx.cs"
    Inherits="ProspectTasksTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.treeview.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
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
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>
    <style type="text/css" >
         a.taskDetails:link, :visited, :active
        {
            color: #818892;
        }
        a.taskDetails:hover
        {
            color: Blue;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {

            // left click context menu
            $("#btnNewTask").contextMenu({ menu: 'divNewTaskMenu', leftButton: true, onShowMenu: function () { $("#divNewTaskMenu").css({ top: 79, left: 25 }).fadeIn(-1); } }, function (action, el, pos) { contextMenuWork(action, el, pos); });
        });

        function contextMenuWork(action, el, pos) {

            if (action == "ProspectTask") {

                ProspectTaskAdd();
            }
            else {

                var LoanID = action;
                //alert("LoanID: " + LoanID);

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                //$("#ifrProspectTaskAdd").attr("src", "../LoanDetails/LoanTaskAdd.aspx?sid=" + RadomStr + "&LoanID=" + LoanID);
                $("#ifrProspectTaskAdd").attr("src", "../LoanDetails/TaskCreate.aspx?sid=" + RadomStr + "&LoanID=" + LoanID);
                // show modal
                $("#divAddTask").attr("title", "Loan Task Setup");
                $("#divAddTask").dialog({
                    height: 435,
                    width: 610,
                    modal: true,
                    resizable: false,
                    close: function (event, ui) { $("#divAddTask").dialog("destroy"); }
                });
                $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            }
        }

        function CloseDialog_AddTask() {

            $("#divAddTask").dialog("close");
        }

        function CloseDialog_EditTask() {

            $("#divAddTask").dialog("close");
        }
        
        function DialogContactAddClose() {
            $("#divAddContact").dialog('destroy');
        }
        function DialogContactEditClose() {
            $("#divEditContact").dialog('destroy');
        }
        function DialogLoanAssignClose() {
            $("#divLoanReassign").dialog('destroy');
        }
        function DialogContactAssignClose() {
            $("#divContactReassign").dialog('destroy');
        }

        function ProspectTaskAdd() {
            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            var fileId = $("#<%= hfdFileId.ClientID %>").val();
            var contactid = "<%=sContactID %>";
            $("#ifrProspectTaskAdd").attr("src", "ProspectTaskSetup.aspx?mode=0&ContactId=" + "<%=sContactID %>" + "&sid=" + radomStr);

            $("#divAddTask").attr("title", "Client Task Setup");
            $("#divAddTask").dialog({
                height: 420,
                width: 740,
                modal: true,
                resizable: false,
                close: function (event, ui) { $("#divAddTask").dialog("destroy"); }
            });

            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            return false;
        }

        function closeProspectTaskSetupWin() {
            $("#divAddTask").dialog('destroy');
            RefreshPage();
        }

        function ProspectTaskUpdate() {
            var checkedIds = getSelectedItems2();
            if (checkedIds.length == 0) {
                alert("No record has been selected.");
                return;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            var TaskType = $("#gvTasks tr td :checkbox:checked").attr("TaskType");

            if (TaskType == "Client Task") {

                var radomStr = Math.random().toString().substr(2);

                $("#ifrProspectTaskAdd").attr("src", "ProspectTaskSetup.aspx?mode=1&id=" + checkedIds + "&sid=" + radomStr);

                $("#divAddTask").attr("title", "Client Task Setup");
                $("#divAddTask").dialog({
                    height: 420,
                    width: 740,
                    modal: true,
                    resizable: false,
                    close: function (event, ui) { $("#divAddTask").dialog("destroy"); }
                });

                $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            }
            else {

                var LoanID = $("#gvTasks tr td :checkbox:checked").attr("LoanID"); ;
                //alert("LoanID: " + LoanID);

                if (TaskType != "Opportunity Task" && TaskType != "Active Loan Task") {

                    alert("You cannot modify a task that’s attached to an archived loan.");
                    return false;
                }

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                //$("#ifrProspectTaskAdd").attr("src", "../LoanDetails/LoanTaskEdit.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&TaskID=" + checkedIds);
                $("#ifrProspectTaskAdd").attr("src", "../LoanDetails/TaskEdit.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&TaskID=" + checkedIds)
                // show modal
                $("#divAddTask").attr("title", "Loan Task Setup");
                $("#divAddTask").dialog({
                    height: 420,
                    width: 610,
                    modal: true,
                    resizable: false,
                    close: function (event, ui) { $("#divAddTask").dialog("destroy"); }
                });
                $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            }

            return false;
        }

        function UpdateProspectTask(ProspectTaskID, TaskType, LoanID) {

            var sHasUpdate = "<%=sHasUpdateRight %>";
            if (sHasUpdate != "1") {

                alert("You are not authorized to perform this operation.");
                return;
            }
            
            if (TaskType == "Client Task") {

                var radomStr = Math.random().toString().substr(2);

                $("#ifrProspectTaskAdd").attr("src", "ProspectTaskSetup.aspx?mode=1&id=" + ProspectTaskID + "&sid=" + radomStr);

                $("#divAddTask").attr("title", "Client Task Setup");
                $("#divAddTask").dialog({
                    height: 420,
                    width: 740,
                    modal: true,
                    resizable: false,
                    close: function (event, ui) { $("#divAddTask").dialog("destroy"); }
                });

                $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            }
            else {

                if (TaskType != "Lead Task" && TaskType != "Active Loan Task") {

                    alert("You cannot modify a task that’s attached to an archived loan.");
                    return;
                }

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                $("#ifrProspectTaskAdd").attr("src", "../LoanDetails/LoanTaskEdit.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&TaskID=" + ProspectTaskID);

                // show modal
                $("#divAddTask").attr("title", "Loan Task Setup");
                $("#divAddTask").dialog({
                    height: 420,
                    width: 610,
                    modal: true,
                    resizable: false,
                    close: function (event, ui) { $("#divAddTask").dialog("destroy"); }
                });
                $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            }
        }

        function getSelectedItems2() {
            var selctedItems = new Array();
            $("#<%=gvTasks.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gvTasks.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        function BeforeDelete() {

            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("Please select one row at least .");
                return false;
            }
            else if (checkedIds.length > 1) {

                alert("Only one record can be selected.");
                return false;
            }

            var HasCompleteTask = false;
            $("#gvTasks tr:not(:first) td :checkbox:checked").each(function () {

                // if completed, can't re-complete task
                var CompleteDate = $(this).attr("CompletedDate");
                if (CompleteDate != "") {

                    HasCompleteTask = true;
                }
            });

            if (HasCompleteTask == true) {

                alert("You cannot delete a completed task.");
                return false;
            }

            var r1 = confirm("Deleting the selected tasks will also delete the associated task alerts and email notifications. Are you sure you want to continue?");
            if (r1 == false) {

                return false;
            }

            var TaskType = $("#gvTasks tr td :checkbox:checked").attr("TaskType");

            $("#<%= hfContactIDs.ClientID %>").val(checkedIds);
            $("#hfSelectedTaskType").val(TaskType);
            return true;
        }

        function ProspectTaskComplate() {
            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("No task has been selected.");
                return false;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one task can be selected for this operation.");
                return false;
            }
            $("#<%= hfContactIDs.ClientID %>").val(checkedIds);
            return true;
        }

        // check/decheck all
        function CheckAll(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

     
        function RefreshPage() {
//            if ($.browser.msie == true) {   // ie 
//                window.parent.document.frames("tabFrame").location.reload();
//            }
//            else if ($.browser.mozilla == true) {   // firefox 
//                window.parent.document.getElementById('tabFrame').contentWindow.location.reload();
//            }
//            else {  // others 
//                var LoanID = GetQueryString1("LoanID");
//                var RadomNum = Math.random();
//                var RadomStr = RadomNum.toString().substr(2);
//                $(window.parent.document).find("#tabFrame").attr("src", "LoanDetailsInfo.aspx?sid=" + RadomStr + "&FileID=" + LoanID);
//            }
            // refresh current page

            window.location.href = window.location.href;

        }

        function aComplete_onclick() {
            // 2011-08-06 WangXiao Comments Start
            /*
            var SelectedCount = $("#gvTasks tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one task can be selected.");
                return;
            }

            var TaskType = $("#gvTasks tr:not(:first) td :checkbox:checked").attr("TaskType");
            var TaskID = $("#gvTasks tr:not(:first) td :checkbox:checked").attr("tag");

            if (TaskType == "Client Task") {

                CompleteTask(TaskID);
            }
            else {

                // if completed, can't re-complete task
                var CompleteDate = $("#gvTasks tr:not(:first) td :checkbox:checked").attr("CompletedDate");
                if (CompleteDate != "") {

                    alert("The selected task has been completed, can't re-complete.");
                    return;
                }

                var LoanID = $("#gvTasks tr td :checkbox:checked").attr("LoanID");
                CompleteLoanTask(TaskID, LoanID);
            }

            return false;
            */
            // 2011-08-06 WangXiao Comments End
            // 2011-08-06 WangXiao Add Start
            var flag = false;
            var SelectedCount = $("#gvTasks tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {
                alert("No task has been selected.");
                return;
            }

            var taskIDs = new Array();
            var loanIDs = new Array();
            var tempLoanID = "";
            $("#gvTasks tr:not(:first) td :checkbox:checked").each(function () {
                var CompleteDate = $(this).attr("CompletedDate");
                if (CompleteDate != "") {

                    alert(" One of the selected task has been completed, can't re-complete.");
                    flag = true;
                    return false;
                }

                var TaskType = $("#gvTasks tr:not(:first) td :checkbox:checked").attr("TaskType");
                if (TaskType == "Client Task") {
                    taskIDs.push($(this).attr("tag"));
                    loanIDs.push(-1);
                }
                else {
                    var IsPrerequisite = $(this).attr("PrerequisiteID");
                    if (IsPrerequisite != "") {
                        alert("The selected task has unfinished prerequisite, can't be completed.");
                        flag = true;
                        return false;
                    }
                    if (jQuery.trim(tempLoanID) == "") {
                        tempLoanID = $(this).attr("LoanID");
                    }
                    if (tempLoanID != $(this).attr("LoanID")) {
                        alert("The tasks you selected are under different loan files. You can only complete multiple tasks within the same loan file. Please check your selection and try again.");
                        flag = true;
                        return false;
                    }
                    taskIDs.push($(this).attr("tag"));
                    loanIDs.push($(this).attr("LoanID"));
                }
            });

            if (flag) {
                return;
            }
            CompleteTasks(taskIDs, loanIDs);
            // 2011-08-06 WangXiao Add End
        }

        // 2011-08-06 WangXiao Add Start
        function CompleteTasks(taskIDs, loanIDs) {
            if (taskIDs.length == 1) {
                CompleteSingleTask(taskIDs, loanIDs)
            }
            else {
                window.parent.ShowWaitingDialog("Multiple tasks selected have completion emails.  Please press OK to continue, Cancel to exit or Skip to bypass the completion emails but still complete the tasks.", taskIDs, loanIDs);
             }
        }
        // 2011-08-06 WangXiao Add End

        function CompleteSingleTask(TaskID, LoanID) {
            // show waiting dialog
            window.parent.ShowWaitingDialogSingleTask("Completing task...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("ProspectTaskComplate_Background.aspx?sid=" + Radom + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterSingleTaskComplete);
        }

        function AfterTaskComplete(data) {
            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
                return;
            }
            // show send completion email
            if (isID(data.EmailTemplateID) == true) {

                ShowDialog_SendCompletionEmail(data.EmailTemplateID);
            }
            setTimeout("CloseWaitingDialog('Complete selected task successfully.')", 2000);
        }

        var RefreshFlag = "";
        function AfterSingleTaskComplete(data) {
            RefreshFlag = "";
            if (data.ExecResult == "Failed") {
                window.parent.ComplateError(data.ErrorMsg);
                return;
            }

            var LoanClosed = data.LoanClosed;

            if (LoanClosed == "Yes") {
                RefreshFlag = "CloseLoan";
            }

            // show send completion email
            if (isID(data.EmailTemplateID) == true) {

                if (RefreshFlag == "") {
                    RefreshFlag = "SendCompletionEmail";
                }
                window.parent.RefreshFlag = RefreshFlag;
                ShowDialog_SendCompletionEmail(data.EmailTemplateID, data.TaskID);
            }

            if (RefreshFlag == "") {
                window.parent.RefreshPage();
            }
            window.parent.RefreshFlag = RefreshFlag;

            $("#hdnErrMsg").val(data.ErrorMsg);
            window.parent.CloseLoan(data);
        }

        function CompleteLoanTask(TaskID, LoanID) {

            // show waiting dialog
            ShowWaitingDialog("Completing loan task...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("../LoanDetails/LoanTaskComplete_Background.aspx?sid=" + Radom + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterLoanTaskComplete);
        }

        function AfterLoanTaskComplete(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
                return;
            }

            // show send completion email
            if (isID(data.EmailTemplateID) == true) {

                ShowDialog_SendCompletionEmail(data.EmailTemplateID);
            }

            setTimeout("CloseWaitingDialog('Complete selected loan task successfully.')", 2000);
        }

        // show popup for send completion email
        function ShowDialog_SendCompletionEmail(EmailTemplateID) {

            var checkedIds = getSelectedItems();

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "ProspectTaskEmailSendCompletionPopup.aspx?sid=" + RadomStr + "&prospectID=" + checkedIds + "&EmailTemplateID=" + EmailTemplateID + "&CloseDialogCodes=window.parent.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 380;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.ShowGlobalPopup("Send Completion Email", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function CloseDialog_SendCompletionEmail() {

            $("#divSendCompletionEmail").dialog("close");
        }


        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {

            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(SuccessMsg);
            $('#aClose').show();
        }

        $(document).ready(function () {
            $(".alert img[src$='Unknown.png']").each(function () {
                $(this).hide();
            });

        });
    </script>
</head>
<body style="width: 700px">
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfdFileId" runat="server" />
    <asp:HiddenField ID="hfContactIDs" runat="server" />
    <asp:HiddenField ID="hfSelectedTaskType" runat="server" />
    <div id="divContainer" style="width: 1035px; height: 600px; border: solid 0px red;
        overflow: auto;">
        <div id="divAddTask" title="Client Task Setup" style="display: none;">
            <iframe id="ifrProspectTaskAdd" frameborder="0" scrolling="no" width="100%" height="100%">
            </iframe>
        </div>
        <div id="divEditTask" title="Client Task Setup" style="display: none;">
            <iframe id="ifrProspectTaskEdit" frameborder="0" scrolling="no" width="100%" height="100%">
            </iframe>
        </div>
  <%--      <div id="divLoanReassign" title="Loan Reassign" style="display: none;">
            <iframe id="ifrLoanReassign" frameborder="0" scrolling="no" width="600px" height="280px">
            </iframe>
        </div>
        <div id="divContactReassign" title="Contact Reassign" style="display: none;">
            <iframe id="ifrContactReassign" frameborder="0" width="630px" height="440px"></iframe>
        </div>--%>
        <div style="padding-left: 10px; padding-right: 10px;">
            <div id="divFilter" style="margin-top: 10px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 330px;">
                            <asp:DropDownList ID="ddlTaskTypeFilter" runat="server" DataValueField="TaskTypeID" DataTextField="TaskType" Width="300px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 15px;">
                            Task Status:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Uncompleted" Value="Uncompleted"></asp:ListItem>
                                <asp:ListItem Text="Completed " Value="Completed"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 15px;">
                            Task Owner:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlOwner" runat="server" DataTextField="TaskOwner" DataValueField="OwnerId">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 15px;">
                            Due:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlDue" runat="server">
                                <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Due in 30 days" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Due in two weeks" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Due in one week" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Due tomorrow" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Due today" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Overdue" Value="6"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                            </asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divToolBar" style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 950px;">
                    <tr>
                        <td>
                            <div id="div1" style="margin-left: 10px;">
                                <ul class="ToolStrip" style="margin-left: 0px;">
                                    <li><input id="btnNewTask" runat="server" type="button" value="New" class="Btn-66" /><span>&nbsp;&nbsp;</span></li>
                                    <li>
                                        <asp:Button ID="btnComplate" runat="server" Text="Complete" CssClass="Btn-66" OnClientClick="javascript:return  aComplete_onclick();" />
                                        <span>&nbsp;&nbsp;</span> </li>
                                    <li>
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="Btn-66" OnClientClick="javascript:return ProspectTaskUpdate();" />
                                        <span>&nbsp;&nbsp;</span> </li>
                                    <li>
                                        <asp:Button ID="btnRemove" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDelete();"
                                            OnClick="btnRemove_Click" />
                                        <span>&nbsp;&nbsp;</span> </li>
                                </ul>
                            </div>
                        </td>
                        <td style="text-align: right; ">
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                                OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                                FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                                ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                                LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divGrid" class="ColorGrid" style="width: 950px; margin-top: 5px; margin-left: 10px;">
            <asp:GridView ID="gvTasks" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false" AllowSorting="true" EmptyDataText="There is no data in database."
                OnSorting="gvTasks_Sorting" CellPadding="3" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" tag='<%# Eval("TaskId") %>' TaskType="<%# Eval("TaskType") %>" LoanID="<%# Eval("LoanID") %>" CompletedDate="<%# Eval("Completed") %>" PrerequisiteID="<%# Eval("PrerequisiteTaskId")%>"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<div style='text-align: center;'>Status<div>" SortExpression="Status" ItemStyle-Wrap="false" ItemStyle-Width="35px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <span class="alert">
                                <img id="imgRateLockIcon"  tag='<%# Eval("TaskId") %>' src='../images/task/<%# Eval("Status")%>'
                                    width="16" height="16" />
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TaskType" SortExpression="TaskType" HeaderText="Type" ItemStyle-Width="110" />
                    <asp:BoundField DataField="LoanFile" SortExpression="LoanFile" HeaderText="Loan File" />
                    <asp:TemplateField HeaderText="Task Name" SortExpression="TaskName">
                        <ItemTemplate>
                             <a href="javascript:UpdateProspectTask('<%# Eval("TaskId") %>', '<%# Eval("TaskType") %>', '<%# Eval("LoanID") %>')" class="taskDetails" tag='<%# Eval("TaskId") %>'><%# Eval("TaskName")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TaskOwner" SortExpression="OwnerId" HeaderText="Owner" ItemStyle-Width="150" />
                    <asp:BoundField DataField="Due" SortExpression="Due" HeaderText="Due" ItemStyle-Width="80" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Completed" SortExpression="Completed" HeaderText="Completed" ItemStyle-Width="80" DataFormatString="{0:d}" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">
                &nbsp;</div>
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
        <div id="divSendCompletionEmail" title="Send Completion Email" style="display: none;">
        <iframe id="ifrSendCompletionEmail" frameborder="0" scrolling="no" width="605px" height="340px"></iframe>
        </div>
    </div>
    <ul id="divNewTaskMenu" class="contextMenu" style="width: 280px;">
        <li><a href="#ProspectTask">Create Client Task</a></li>
        <asp:Literal ID="ltrContextMenuItems" runat="server"></asp:Literal>
    </ul>
    </form>
</body>
</html>
