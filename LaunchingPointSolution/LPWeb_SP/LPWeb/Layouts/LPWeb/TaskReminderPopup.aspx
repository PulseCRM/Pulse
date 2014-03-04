<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskReminderPopup.aspx.cs" Inherits="LPWeb_TaskReminderPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Task Reminder Popup</title>
    <link href="css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/urlparser.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridTaskList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridTaskList tr td :checkbox").attr("checked", "");
            }
        }

        //#region complete task

        function BeforeComplete() {

            var SelectedCount = $("#gridTaskList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return false;
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
                return false;
            }

            var TaskIDs = GetSelectedTaskIDs();
            $("#hdnSelTaskIDs").val(TaskIDs);

            return true;

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

            var bSelectAll = false;
            if ($("#gridTaskList tr:not(:first) td :checkbox").length == SelectedCount) {

                bSelectAll = true;
            }

            var TaskIDs = GetSelectedTaskIDs();

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "";
            if (bSelectAll == false) {

                iFrameSrc = "LoanDetails/LoanTaskDefer.aspx?sid=" + sid + "&TaskIDs=" + TaskIDs;
            }
            else {

                iFrameSrc = "LoanDetails/LoanTaskDefer.aspx?sid=" + sid + "&TaskIDs=" + TaskIDs + "&All=true";
            }

            var BaseWidth = 310;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 130;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Defer Task", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function CloseDialog_DeferTask() {

            CloseGlobalPopup();
        }

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

        function RefreshPage() {

            // select all
            if ($("#gridTaskList tr:not(:first) td :checkbox").length == $("#gridTaskList tr:not(:first) td :checkbox:checked").length) {

                window.parent.CloseGlobalPopup();
            }
            else {

                window.location.href = window.location.href;
            }
        }

        //#endregion

        //#region snooze task

        function ShowDialog_SnoozeTask() {

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

            var bSelectAll = false;
            if ($("#gridTaskList tr:not(:first) td :checkbox").length == SelectedCount) {

                bSelectAll = true;
            }

            var TaskIDs = GetSelectedTaskIDs();

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "";
            if (bSelectAll == false) {

                iFrameSrc = "TaskReminderSnooze.aspx?sid=" + sid + "&TaskIDs=" + TaskIDs + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";
            }
            else {

                iFrameSrc = "TaskReminderSnooze.aspx?sid=" + sid + "&TaskIDs=" + TaskIDs + "&CloseDialogCodes=window.parent.CloseGlobalPopup()&All=true";
            }

            var BaseWidth = 200;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 100;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Snooze Task", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }


        //#endregion

        //#region Show/Close Global Popup

        function ShowGlobalPopup(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divGlobalPopup").attr("title", Title);

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
                    $("#ifrGlobalPopup").attr("src", "");
                }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseGlobalPopup() {

            $("#divGlobalPopup").dialog("close");
        }

        //#endregion

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        
        <div id="divToolBar">
            <ul class="ToolStrip" style="margin-left: 0px;">
                <li><asp:LinkButton ID="lnkComplete" runat="server" OnClientClick="return BeforeComplete()" OnClick="lnkComplete_Click">Complete</asp:LinkButton><span>|</span></li>
                <li><a id="aSnooze" href="javascript:ShowDialog_SnoozeTask()">Snooze</a><span>|</span></li>
                <li><a id="aDefer" href="javascript:ShowDialog_DeferTask()">Defer</a></li>
                
            </ul>
        </div>
        <div id="divTaskList" class="ColorGrid" style="margin-top: 5px; width:700px;">

            <asp:GridView ID="gridTaskList" runat="server" DataKeyNames="LoanTaskId" EmptyDataText="There is no reminder task."
                AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkSelected" type="checkbox" mytaskid="<%# Eval("LoanTaskID")%>"
                                                myprerequisiteid="<%# Eval("PrerequisiteTaskId")%>" mytaskowner="<%# Eval("Owner")%>"
                                                mycompleteddate="<%# Eval("Completed")%>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Borrower" DataField="BorrowerName" HeaderStyle-Width="150px" ItemStyle-Width="150px" />
                    <asp:BoundField HeaderText="Task Name" DataField="TaskName" />
                    <asp:BoundField HeaderText="Owner" DataField="OwnerName" HeaderStyle-Width="130px" ItemStyle-Width="130px" />
                    <asp:BoundField HeaderText="Due" DataField="DueDateTime" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        
        <div id="divGlobalPopup" title="Global Popup" style="display: none;">
            <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="100px" height="100px">
            </iframe>
        </div>

        <asp:HiddenField ID="hdnSelTaskIDs" runat="server" />

    </div>
    </form>
</body>
</html>