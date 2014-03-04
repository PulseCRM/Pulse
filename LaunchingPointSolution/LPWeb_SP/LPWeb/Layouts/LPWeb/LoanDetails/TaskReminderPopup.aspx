<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page  Language="C#" AutoEventWireup="true" CodeBehind="TaskReminderPopup.aspx.cs" Inherits="TaskReminderPopup" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Task Reminder Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type"text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
   
     <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />

    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />

    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.tab.js" type="text/javascript"></script>

    <script src="../js/jquery.validate.js" type="text/javascript"></script>

    <script src="../js/jquery.tablesorter.js" type="text/javascript"></script>

    <script src="../js/common.js" type="text/javascript"></script>


       <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

        <script language="javascript" type="text/javascript">

            var taskBaseParent = window.parent;

            //alert(taskBaseParent);
            var RefreshFlag = "";

        // check/decheck all
        function CheckAll(CheckBox) {
            //CheckAllClicked(CheckBox);
            if (CheckBox.checked) {
                $("#" + '<%=gridList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#" + '<%=gridList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

                function ShowDialog_DeferTask() {

                    var SelectedCount = $("#gridList tr:not(:first) td :checkbox:checked").length;
                    if (SelectedCount == 0) {

                        alert("No task has been selected.");
                        return;
                    }

                    var HasCompleteTask = false;
                    $("#gridList tr:not(:first) td :checkbox:checked").each(function () {

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
                    $("#ifrDeferTask").attr("src", "../LoanDetails/LoanTaskDefer.aspx?sid=" + RadomStr + "&TaskIDs=" + TaskIDs);

                    // show modal
                    $("#divDeferTask").dialog({
                        height: 130,
                        width: 310,
                        modal: true,
                        resizable: false
                    });
                    $(".ui-dialog").css("border", "solid 3px #aaaaaa")
                }

                function GetSelectedTaskIDs() {

                    var TaskIDs = "";
                    $("#gridList tr:not(:first) td :checkbox:checked").each(function (i) {

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



                ///////////////////////////////////////////////////////

        function aComplete_onclick() {

            var flag = false;
            var SelectedCount = $("#gridList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }

            // if completed, can't re-complete task
            $("#gridList tr:not(:first) td :checkbox:checked").each(function () {
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

            $("#gridList tr:not(:first) td :checkbox:checked").each(function () {
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
            $("#gridList tr:not(:first) td :checkbox").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("myTaskID"));
                }
            });
            return selctedItems;
        }

        function CompleteTask1(TaskID, StageID) {

          
            // show waiting dialog
            ShowWaitingDialog("Completing task...", StageID, TaskID);

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = GetQueryString1("LoanID");

          
            // Ajax
            $.getJSON("../LoanDetails/LoanTaskComplete_Background.aspx?sid=" + Radom + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterTaskComplete1);

           
           
        }

        function AfterTaskComplete1(data) {
            RefreshFlag = "";
            if (data.ExecResult == "Failed") {
                //alert(data.ErrorMsg);
                return;
            }

            //taskBaseParent.CompleteStageID = GetQueryString1("Stage");

            var LoanClosed = data.LoanClosed;
            var LoanStatus = $("#hdnLoanStatus").val();


            //alert('LoanStatus');

            if (LoanClosed == "Yes" && LoanStatus == "Processing") {
                //alert('ssssssssssssssssssssssss');
              
                RefreshFlag = "CloseLoan";
            }

            // show send completion email
            if (isID(data.EmailTemplateID) == true) {

                //alert('1');
                if (RefreshFlag == "") {
                    RefreshFlag = "SendCompletionEmail";
                }


                taskBaseParent.RefreshFlag = RefreshFlag;

                //alert('2');
                ShowDialog_SendCompletionEmail(data.EmailTemplateID, data.TaskID);
            }

            //if (RefreshFlag == "") {
            //    taskBaseParent.RefreshPage();
            //}

            //alert(RefreshFlag);
            taskBaseParent.RefreshFlag = RefreshFlag;

            //alert(data.ErrorMsg);
            $("#hdnErrMsg").val(data.ErrorMsg);
            taskBaseParent.CloseLoan(data);

           

            if (RefreshFlag == "CloseLoan") {
               
                CloseLoan(data);
            }
        }

        function CloseLoan(data) {

            var LoanClosed = data.LoanClosed;
            var ErrMsg = data.ErrorMsg;
            var LoanStatus = $("#hdnLoanStatus").val();


            //alert(LoanClosed);


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

           ShowGlobalPopup("CloseLoan", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

            return false;
        }

        function ShowDialog_SendCompletionEmail(EmailTemplateID, TaskID) {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/EmailSendCompletionPopup.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&EmailTemplateID=" + EmailTemplateID + "&TaskID=" + TaskID + "&CloseDialogCodes=taskBaseParent.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 380;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Send Completion Email", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }


        function CompleteTask(TaskIDs, LoanStatus, StageID) {

            //            taskBaseParent.ShowWaitingDialog("Completing task...", TaskIDs, LoanStatus);
            ShowWaitingDialog("Multiple tasks selected have completion emails.  Please press OK to continue, Cancel to exit or Skip to bypass the completion emails but still complete the tasks.", TaskIDs, LoanStatus, StageID);

        }

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            $.blockUI({ message: $('#divWaiting'), css: { width: '300px'} });
            //$("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {
            //alert(SuccessMsg);
            var Msg = $('#hdnErrMsg').val();

            if (Msg.length <= 0)
                Msg = SuccessMsg;
            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(Msg);
            $('#aClose').show();
        }

        function ShowDialog_Snooze() {

            var SnoozeTask = $("#hdnTaskReminder").val();
            var flag = 1;
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var SelectedCount = $("#gridList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }
            else if (SelectedCount < SnoozeTask) {
                //说明没有全部选择 指刷新Task Reminder
                flag = 0;
            }
            else {
                //说明全部选择,关闭Task Reminder
                flag = 1;
            }

            var TaskIDs = GetSelectedTaskIDs();

            //alert(TaskIDs);

            var iFrameSrc = "../LoanDetails/TaskReminderSnoozePopup.aspx?sid=" + RadomStr + "&TaskIDs=" + TaskIDs + "&flag=" + flag;

          
            var BaseWidth = 200;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 100;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            
            ShowGlobalPopup("Task Reminder Snooze Popup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

            //window.showModalDialog(iFrameSrc,window,'dialogWidth:150px;dialogHeight:100px;toolbar=no; menubar=no; scrollbars=no; resizable=no;location=no; status=no');

        }

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
          
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function checkLeave() {
            //$("#divGlobalPopup").dialog("destroy");
            //$("#ifrGlobalPopup").attr("src", "about:blank");

            this.window.opener = null;
            window.close(); 
          
        }

        </script>
       </head>
<%--<body onbeforeunload="checkLeave()">--%>
<body>

    <form id="form1" runat="server">
    <div id="divContainer" style="width: 700px;">
     <div class="JTab" style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>                                      
                                        <td style="width:600px;">
                                            <ul class="ToolStrip">
                                               <li><a id="aComplete" href="javascript:aComplete_onclick()">Complete</a><span>|</span></li>
                                                <li>
                                                   <a id="a1" href="javascript:ShowDialog_Snooze()">Snooze</a><span>|</span></li>
                                                <li>
                                             <a id="aDefer" href="javascript:ShowDialog_DeferTask()">Defer</a></li>
                                               
                                            </ul>
                                        </td>
                                     <%--  <td style="text-align: right;">
                                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                                                OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                                                FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                                                ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                                                LayoutType="Table">
                                            </webdiyer:AspNetPager>
                                        </td>--%>
                                    </tr>
                                </table>
                                
                             <div id="divDivision" class="ColorGrid" style="width: 580px; margin-top: 5px;">
                            <asp:GridView ID="gridList" runat="server" DataKeyNames="LoanTaskId" EmptyDataText="There is no email template in database."
                                AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound" CellPadding="3" OnPreRender="gridList_PreRender"
                                CssClass="GrayGrid tablesorter" GridLines="None" OnSorting="gridList_Sorting" AllowSorting="true">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" ToolTip='<%# Eval("LoanTaskId") %>' />
                                        </ItemTemplate>
                                        <ItemTemplate>
                                            <input id="Checkbox2" type="checkbox" mytaskname="<%# Eval("Name") %>" mytaskid="<%# Eval("LoanTaskID")%>"
                                                myprerequisiteid="<%# Eval("PrerequisiteTaskId")%>" mytaskowner="<%# Eval("Owner")%>"
                                                mycompleteddate="<%# Eval("Completed")%>" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                        <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Borrower" SortExpression="Borrower" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBorrower" runat="server" Text='<%# Eval("FileId")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Task Name" SortExpression="Name" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                         <img style=" top: 2px;" src="../images/task/<%# this.GetLoanTaskIconFileName(Convert.ToInt32(Eval("LoanTaskId"))) %>" />
                                           
                                                <%# Eval("Name")%>
                                           <%-- <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Owner" SortExpression="Owner"  ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOwner" runat="server" Text='<%# Eval("Owner")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due" SortExpression="Due"  ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDue" runat="server" Text='<%# Eval("Due")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="DueTime" SortExpression="DueTime"  ItemStyle-Width="60px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueTime" runat="server" Text='<%# Eval("DueTime")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                            <asp:HiddenField ID="hiAllIds" runat="server" />
                            <asp:HiddenField ID="hiCheckedIds" runat="server" />
                            <asp:HiddenField ID="hiReferenced" runat="server" />
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

        <asp:HiddenField ID="hdnTaskReminder" runat="server" />
    </div>

    <div id="divDeferTask" title="Defer Loan Task" style="display: none;">
        <iframe id="ifrDeferTask" frameborder="0" scrolling="auto" width="280px" height="90px">
        </iframe>
    </div>
     <div id="divEditTask" title="Loan Task Setup" style="display: none;">
        <iframe id="ifrEditTask" frameborder="0" scrolling="no" width="580px" height="400px">
        </iframe>
    </div>

 <div id="divGlobalPopup" title="Global Popup" style="display: none;">
        <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="400px" height="300px">
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
   </div>
	
   </form>

  
</body>
</html>