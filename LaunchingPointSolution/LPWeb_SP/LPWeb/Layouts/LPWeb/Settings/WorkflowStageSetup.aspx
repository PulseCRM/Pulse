<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowStageSetup.aspx.cs" Inherits="Settings_WorkflowStageSetup" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Workflow Stage Setup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            var CalcDueDateMethod = $("#hdnCalcDueDateMethod").val();
            if (CalcDueDateMethod == "Est Close Date") {
                AddValidators(true, false);
                $("#ddlCalcDueDateMethod").val(CalcDueDateMethod)
            }
            else if (CalcDueDateMethod == "Completion Date of the previous Stage") {
                AddValidators(false, false);
                $("#ddlCalcDueDateMethod").val(CalcDueDateMethod)
            }
            else {
                AddValidators(false, true);
                $("#ddlCalcDueDateMethod").val(CalcDueDateMethod)
            }
            // only int
            $("#txtSeq").onlypressnum();
            $("#txtDaysFromEstClose").OnlyInt();
            $("#txtDaysAfterCreation").OnlyInt();

            // add event
            $("#ddlStage").change(ddlStage_onchange);

            var WflStageID = GetQueryString1("WflStageID");
            if (WflStageID == "0") {

                $("#chkEnabled").attr("disabled", "true");
                $("#txtSeq").attr("disabled", "true");
                $("#txtDaysFromEstClose").attr("disabled", "true");
                $("#txtDaysAfterCreation").attr("disabled", "true");
                $("#btnSave").attr("disabled", "true");

                $("#aCreateTask").attr("disabled", true);
                $("#aCreateTask").removeAttr("href");
                $("#aCreateTask").css("text-decoration", "none");
            }

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflStageSetup").height(xx);
            }
        });

        // add jQuery Validators
        function AddValidators(bEstClose, bCreation) {
            $("#form1").validate({

                rules: {
                    ddlWorkflowType: {
                        required: true
                    },
                    txtSeq: {
                        required: true,
                        range: [0, 100]
                    },
                    txtDaysFromEstClose: {
                        required: bEstClose,
                        range: [-365, 365]
                    },
                    txtDaysAfterCreation: {
                        required: bCreation,
                        range: [-365, 365]
                    }
                },
                messages: {
                    ddlWorkflowType: {
                        required: "<div>Please select Workflow Type.</div>"
                    },
                    txtSeq: {
                        required: "Required.",
                        range: "from 0 to 100."
                    },
                    txtDaysFromEstClose: {
                    
                        required: "Required.",
                        range: "<div>-365 0 to 365.</div>"
                    },
                    txtDaysAfterCreation: {
                     
                        required: "Required.",
                        range: "<div>-365 0 to 365.</div>"
                    }
                }
            });
        }

        function ddlStage_onchange() {

            var SelStageID = $("#ddlStage").val();
            if (SelStageID == "") {

                SelStageID = "0";
            }

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");
            var from = GetQueryString1("from");

            window.parent.location.href = "WorkflowStageSetup2.aspx?sid=" + sid + "&WflStageID=" + SelStageID + "&WorkflowTemplateID=" + WorkflowTemplateID + "&from=" + encodeURIComponent(from);
        }

        function ddlCalcDueDateMethod_onChange() {


            var Calcduedate = $("#ddlCalcDueDateMethod").val();
            //alert(Calcduedate);

            var StageSequence = parseInt($("#txtSeq").val());
            var MinSeqNo = parseInt($("#hdnMinStagesSeq").val());
            var SecSeqNo = parseInt($("#hdnSecStagesSeq").val());


            if (Calcduedate == "Completion Date of the previous Stage" && (StageSequence == 1 || StageSequence < SecSeqNo)) {
                alert("The selected stage does not have a previous stage.");
                $("#ddlCalcDueDateMethod").val("Est Close Date");
                return false;
            }
          
                if (Calcduedate == "") {
                    Calcduedate = "Est Close Date";
                }
                $("#hdnCalcDueDateMethod").val(Calcduedate);
                $("#txtDaysFromEstClose").val("");
                $("#txtDaysAfterCreation").val("");

                if (Calcduedate == "Est Close Date") {
                    $("#txtDaysFromEstClose").removeAttr("Disabled");
                    $("#txtDaysAfterCreation").attr("Disabled", "disabled");

                    $("#txtDaysFromEstClose").rules("add", {
                        required: true,
                        range: [-365, 365],
                        messages: {
                            required: "Required.",
                            range: "<div>-365 0 to 365.</div>"
                        }
                    });
                    $("#txtDaysAfterCreation").rules("remove");
                }
                else if (Calcduedate == "Creation Date") {
                    $("#txtDaysFromEstClose").attr("Disabled", "disabled");
                    $("#txtDaysAfterCreation").removeAttr("Disabled");

                    $("#txtDaysAfterCreation").rules("add", {
                        required: true,
                        range: [-365, 365],
                        messages: {
                            required: "Required.",
                            range: "<div>-365 0 to 365.</div>"
                        }
                    });
                    $("#txtDaysFromEstClose").rules("remove");
                }
                else  {
                    $("#txtDaysFromEstClose").attr("Disabled", "disabled");
                    $("#txtDaysAfterCreation").removeAttr("Disabled");

                    $("#txtDaysAfterCreation").rules("add", {
                        required: false,
                        range: [-365, 365],
                        messages: {
                            required: "",
                            range: ""
                        }
                    });

                    $("#txtDaysFromEstClose").removeAttr("Disabled");
                    $("#txtDaysAfterCreation").attr("Disabled", "disabled");

                    $("#txtDaysFromEstClose").rules("add", {
                        required: false,
                        range: [-365, 365],
                        messages: {
                            required: "",
                            range: "</div>"
                        }
                    });

                    $("#txtDaysFromEstClose").rules("remove");
                    $("#txtDaysAfterCreation").rules("remove");
                }


            }
            //TxtSeq Value Change
            function txtSeq_onChange() {

                var Calcduedate = $("#ddlCalcDueDateMethod").val();
                //alert(Calcduedate);

                var StageSequence = parseInt($("#txtSeq").val());
                var MinSeqNo = parseInt($("#hdnMinStagesSeq").val());
                var SecSeqNo = parseInt($("#hdnSecStagesSeq").val());
                var SeqNo = $("#hdnSeqNos").val();

                if (Calcduedate == "Completion Date of the previous Stage" && (StageSequence == 1 || StageSequence < SecSeqNo)) {
                    alert("The selected stage does not have a previous stage.");
//                    $("#txtSeq").val(SeqNo);
                    return false;
                }
            }


        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridWorkflowTaskList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridWorkflowTaskList tr td :checkbox").attr("checked", "");
            }
        }

        //#region Create/Update

        function ShowDialog_CreateTask() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");
            var WflStageID = GetQueryString1("WflStageID");

            window.location.href = "WorkflowTaskTemplateAdd.aspx?sid=" + RadomStr + "&StageID=" + WflStageID + "&TemplateID=" + WorkflowTemplateID + "&FromPage=WorkflowStageSetup.aspx";

        }

        function ShowDialog_UpdateWorkflowTask() {

            var SelectedCount = $("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No workflow task was selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one workflow task can be selected.");
                return;
            }

            var WorkflowTaskID = $("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").attr("myWflTaskID");
            //            alert(WorkflowTaskID);

            UpdateWorkflowTask(WorkflowTaskID);
        }

        function UpdateWorkflowTask(WorkflowTaskID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");
            var WflStageID = GetQueryString1("WflStageID");

            window.location.href = "WorkflowTaskTemplateEdit.aspx?sid=" + RadomStr + "&TaskID=" + WorkflowTaskID + "&StageID=" + WflStageID + "&TemplateID=" + WorkflowTemplateID + "&FromPage=WorkflowStageSetup.aspx";
        }

        function ClosePopupTask() {

            $("#divWflTaskSetup").dialog("close");
            $("#divWflTaskSetup").dialog("destroy");
        }

        //#endregion

        //#region show/close waiting dialog

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

        //#region Disable Task

        function aDisable_Task() {

            if ($("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No task has been selected.");
                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Disabling selected task(s)...");

            // selected task ids
            var WflTaskIDs = "";
            $("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").each(function (i) {

                var WflTaskID = $(this).attr("myWflTaskID");
                if (WflTaskIDs == "") {

                    WflTaskIDs = WflTaskID;
                }
                else {

                    WflTaskIDs += "," + WflTaskID;
                }
            });

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("DisabledWorkflowTask.ashx?sid=" + Radom + "&WflTaskIDs=" + encodeURIComponent(WflTaskIDs), AfterDisableTask);
        }

        function AfterDisableTask(data) {

            if (data.ExecResult == "Failed") {
                $("#divContainer").unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Disabled selected task(s) successfully.')", 2000);
        }

        //#endregion

        //#region Delete Task

        function aDelete_Task() {

            if ($("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No task has been selected.");
                return;
            }

            var result = confirm("This will delete the task record(s), the loan tasks, and the workflow history for the tasks. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Deleting selected task(s)...");

            // selected task ids
            var WflTaskIDs = "";
            $("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").each(function (i) {

                var WflTaskID = $(this).attr("myWflTaskID");
                if (WflTaskIDs == "") {

                    WflTaskIDs = WflTaskID;
                }
                else {

                    WflTaskIDs += "," + WflTaskID;
                }
            });

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("DeleteWorkflowTask.ashx?sid=" + Radom + "&WflTaskIDs=" + encodeURIComponent(WflTaskIDs), AfterDeleteTask);
        }

        function AfterDeleteTask(data) {

            if (data.ExecResult == "Failed") {
                $("#divContainer").unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Deleted selected task(s) successfully.')", 2000);
        }

        //#endregion

        function btnGoToList_onclick() {

            var from = GetQueryString1("from");
            window.parent.location.href = from;
        }

        //#region Clone Task

        function aCloneTask_onclick() {

            if ($("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No task has been selected.");
                return;
            }
            else if ($("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").length > 1) {

                alert("Only one task can be selected.");
                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Cloning task...");

            // selected task ids
            var WflTaskID = $("#gridWorkflowTaskList tr:not(:first) td :checkbox:checked").attr("myWflTaskID");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("CloneWorkflowTaskAjax.aspx?sid=" + sid + "&WflTaskID=" + WflTaskID, function (data) {

                if (data.ExecResult == "Failed") {

                    $("#divContainer").unblock();
                    alert(data.ErrorMsg);
                    return;
                }

                setTimeout("CloseWaitingDialog('Cloned task successfully.')", 2000);

            });
        }

        //#endregion

        function BeforeSave() {

            //#region validate task list - sequence duplication

            var IsDuplicated = false;
            $("#gridWorkflowTaskList .ddlSeqNo").each(function (i) {

                var ThisSeq = $(this).val();

             

                for (var j = i + 1; j < $("#gridWorkflowTaskList .ddlSeqNo").length; j++) {

                    var OtherSeq = $("#gridWorkflowTaskList .ddlSeqNo").eq(j).val();

                  
                    if (ThisSeq == OtherSeq) {

                        IsDuplicated = true;
                        return;
                    }
                }
            });

            if (IsDuplicated == true) {

                alert("The Task Sequence Number must be unique.");
                return false;
            }

            //#endregion


            var Calcduedate = $("#ddlCalcDueDateMethod").val();

            var StageSequence = parseInt($("#txtSeq").val());
            var MinSeqNo = parseInt($("#hdnMinStagesSeq").val());
            var SecSeqNo = parseInt($("#hdnSecStagesSeq").val());

            if (Calcduedate == "Completion Date of the previous Stage" && (StageSequence == 1 || StageSequence < SecSeqNo)) {
                alert("The selected stage does not have a previous stage.");
                return false;
            }


            var SeqNos = "";

            $("#gridWorkflowTaskList .ddlSeqNo").each(function (i) {

                var taskidx = $(this).attr("taskid");
                var seq = $(this).val();

                if (i == 0) {

                    SeqNos = taskidx + ":" + seq;
                }
                else {

                    SeqNos += "," + taskidx + ":" + seq;
                }
            });

            $("#hdnSeqNos").val(SeqNos);
            //alert($("#hdnSeqNos").val());

            return true;
        }
        
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 870px; border: solid 0px red;">
        <input id="btnGoToList" type="button" value="Back to Workflow Template Setup" onclick="btnGoToList_onclick()" class="Btn-250" />
        <br />
        <br />
        <div id="divStageDetails">
            <table>
                <tr>
                    <td style="width: 388px;">Workflow Template: <asp:Label ID="lbWorkflowTemplate" runat="server" Text="Label"></asp:Label></td>
                    <td>Workflow Type: <asp:Label ID="lbWorkflowType" runat="server" Text="Label"></asp:Label></td>
                </tr>
            </table>
            <table style="margin-top: 10px;">
                <tr>
                    <td style="width: 70px;">Stage:</td>
                    <td style="width: 315px;">
                        <asp:DropDownList ID="ddlStage" runat="server" DataValueField="WflStageId" DataTextField="Name" Width="230px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnabled" runat="server" />
                    </td>
                    <td style="width: 100px;">Enabled</td>
                    <td style="width: 60px;">Sequence:</td>
                    <td>
                        <asp:TextBox ID="txtSeq" runat="server" Width="50px" MaxLength="3" onchange="return txtSeq_onChange();"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="margin-top: 10px;">
                <tr>
                    <td style="width: 150px;">Calculate Due Dates based on: </td>
                    <td style="width: 170px; vertical-align: top;">
                        <select id="ddlCalcDueDateMethod" onchange="return ddlCalcDueDateMethod_onChange();" style="width:300px;">
                            <option>Est Close Date</option>
                            <option>Creation Date</option>
                            <option>Completion Date of the previous Stage</option>	
                        </select>
<%--                        <asp:DropDownList ID="ddlCalcDueDateMethod" runat="server" Width="120px">
                            <asp:ListItem>Est Close Date</asp:ListItem>
                            <asp:ListItem>Creation Date</asp:ListItem>
                        </asp:DropDownList>--%>
                    </td>
                </tr>
            </table>
            <table style="margin-top: 10px;">
                <tr>
                    <td style="width: 150px;">Days From Est Close:</td>
                    <td style="width: 80px;">
                        <asp:TextBox ID="txtDaysFromEstClose" runat="server" Width="60px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td style="width: 100px;"></td>
                    <td style="width: 150px;">Days After Creation Date:</td>
                    <td style="width: 50px;">
                        <asp:TextBox ID="txtDaysAfterCreation" runat="server" Width="60px" MaxLength="4"></asp:TextBox>
                    </td>
                    
                </tr>
            </table>
            <table style="margin-top: 10px;">
                <tr>
                  
                  <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
                        
                    </td>
                </tr>
            </table>
        </div>
        <div id="divTaskList" style="margin-top: 20px;">
            <div id="divToolBar">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 450px;">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aCreateTask" href="javascript:ShowDialog_CreateTask()">Create Task</a><span>|</span></li>
                                <li><a id="aCloneTask" href="javascript:aCloneTask_onclick()">Clone Task</a><span>|</span></li>
                                <li><a id="aUpdateTask" href="javascript:ShowDialog_UpdateWorkflowTask()">Update Task</a><span>|</span></li>
                                <li><a id="aDisableTask" href="javascript:aDisable_Task()">Disable Task</a><span>|</span></li>
                                <li><a id="aDeleteTask" href="javascript:aDelete_Task()">Delete Task</a></li>
                            </ul>
                        </td>
                        <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>
            </div>
             <div id="divWorkflowTaskList" class="ColorGrid" style="margin-top: 5px;">
                <asp:GridView ID="gridWorkflowTaskList" runat="server" EmptyDataText="There is no workflow task." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" myWflTaskID="<%# Eval("TemplTaskId") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Seq No" ItemStyle-Width="60px">
                            <ItemTemplate>
                                <select class="ddlSeqNo" style="width:55px;" taskid="<%# Eval("TemplTaskId")%>">
                                   <%# this.GetSeqNoOptions(Eval("SequenceNumber").ToString()) %>
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Task">
                            <ItemTemplate>
                                <a href="javascript:UpdateWorkflowTask('<%# Eval("TemplTaskId")%>')"><%# Eval("Name")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prerequisite Task" ItemStyle-Width="200px">
                            <ItemTemplate>
                                <a href="#" class="TextEllipsis" style="width: 195px;" title="<%# Eval("PrerequisiteTaskid")%>"><%# this.GetPrerequisiteTaskName(Eval("PrerequisiteTaskid").ToString())%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DaysDueAfterPrerequisite" HeaderText="Days After<br />Prereq" ItemStyle-Width="55px" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" />
                        <asp:BoundField DataField="DaysDueFromCoe" HeaderText="Days From<br />Est Close" ItemStyle-Width="55px" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" />
                        <asp:BoundField DataField="DaysFromCreation" HeaderText="Days After<br />Creation" ItemStyle-Width="55px" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" />
                        <%--<asp:BoundField DataField="TaskEnabled" HeaderText="Enabled" ItemStyle-Width="40px" />--%>
                        <asp:BoundField DataField="WflTemplId" HeaderText="Enabled" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="StageName" HeaderText="Stage" ItemStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
    </div>
    <div id="divWflTaskSetup" title="Workflow Task Setup" style="display: none;">
        <iframe id="ifrWflTaskSetup" frameborder="0" scrolling="no" width="750px" height="450px"></iframe>
    </div>
    <div id="divWaiting" style="display: none; padding: 5px;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>&nbsp;&nbsp;
					<a id="aClose" href="javascript:window.location.href=window.location.href" style="font-weight: bold; color: #6182c1;">[Close]</a>
				</td>
			</tr>
		</table>
	</div>
    <asp:HiddenField ID="hdnCalcDueDateMethod" runat="server" />
    <asp:HiddenField ID="hdnSeqNos" runat="server" Value="" />
    <asp:HiddenField ID="hdnMinStagesSeq" runat="server" Value="" />
    <asp:HiddenField ID="hdnSecStagesSeq" runat="server" Value="" />
    </form>
</body>
</html>