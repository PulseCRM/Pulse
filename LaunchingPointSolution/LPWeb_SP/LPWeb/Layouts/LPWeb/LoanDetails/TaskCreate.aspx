<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskCreate.aspx.cs" Inherits="LoanDetails_TaskCreate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan Task Setup</title>
    
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.tabs.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>

    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/datejs.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {

            // init tab
            $("#tabs").tabs();


            setTimeout("AfterTabInit()", 1000);

        });

        function AfterTabInit() {

            $("#tabs").show();

            AddValidators();

            // datepick
            $("#txtDueDate").datepick();

            // disaled complete date
            $("#txtCompletedDate").attr("disabled", "true");

            // only number
            $("#txtDaysToEst").onlypressnum();
            $("#txtDaysAfterCreation").onlypressnum();
            $("#txtDaysDueAfter").onlypressnum();
            $("#txtDaysDueAfterPrevStage").onlypressnum();


            // add event
            $("#ddlPrerequisite").change(ddlPrerequisite_onchange);
            $("#txtDaysToEst").blur(txtDaysToEst_onblur);
            $("#txtDaysDueAfter").blur(txtDaysDueAfter_onblur);

            // disabled
            $("#txtDaysDueAfter").attr("disabled", "true");


            // max length
            $("#txtNotes").maxlength(4000);

        }

        function AddValidators() {

            $("#form1").validate({

                rules: {

                    txtDueDate: {
                        required: true
                    }
                },
                messages: {

                    txtDueDate: {
                        required: "*"
                    }
                }
            });
        }

        function radTaskList_onclick() {

            $("#ddlTaskList").removeAttr("disabled");
            $("#txtTaskName").val("");
            $("#txtTaskName").attr("disabled", "true");
            $("#txtTaskName").rules("remove", "required");
        }

        function radTaskName_onclicik() {

            $("#ddlTaskList").attr("disabled", "true");
            $("#txtTaskName").removeAttr("disabled");
            $("#txtTaskName").rules("add", {
                required: true,
                messages: { required: "*" }
            });
        }

        function chkReminder_onclick(checkbox) {

            if (checkbox.checked == true) {

                $("#ddlReminderInterval").removeAttr("disabled");
            }
            else {

                $("#ddlReminderInterval").attr("disabled", "true");
            }
        }

        function chkCompleted_onclick(checkbox) {

            if (checkbox.checked == true) {

                $("#txtCompletedDate").removeAttr("disabled");
                var ServerNow = $("#hdnNow").val();
                $("#txtCompletedDate").val(ServerNow);
                $("#txtCompletedDate").attr("readonly", "true");
            }
            else {

                $("#txtCompletedDate").val("");
                $("#txtCompletedDate").attr("disabled", "true");
            }
        }

        function ddlPrerequisite_onchange() {

            var PrerequisiteID = $("#ddlPrerequisite").val();
            if (PrerequisiteID == 0) {

                // clear and editable
                $("#txtDaysToEst").attr("disabled", "");

                $("#txtDueDate").val("");
                $("#txtDueDate").attr("readonly", "");
                $("#txtDueDate").datepick();

                // add required
                if ($("#txtDaysToEst").val() == 0) {

                    $("#txtDueDate").rules("add", {
                        required: true,
                        messages: { required: "*" }
                    });
                }

                $("#txtDaysDueAfter").attr("disabled", "true");
                $("#txtDaysDueAfter").val("");

                // remove requred
                //$("#txtDaysDueAfter").rules("remove", "required");
            }
            else {

                // clear and readonly
                $("#txtDueDate").attr("readonly", "true");
                $("#txtDueDate").datepick('destroy')

                $("#txtDaysToEst").val("");
                $("#txtDaysToEst").attr("disabled", "true");

                // default
                $("#txtDaysDueAfter").attr("disabled", "");
                $("#txtDaysDueAfter").val("5");

                // calc due date
                CalcDueDate(PrerequisiteID);

                // add required
                $("#txtDaysDueAfter").rules("add", {
                    required: true,
                    messages: { required: "*" }
                });
            }
        }

        function CalcDueDate(PrerequisiteID) {

            // calc due date
            var TaskCompletedDate = $("#ddlPrerequisite2 option[value=" + PrerequisiteID + "]").text();
            if (TaskCompletedDate != "") {

                var DayCount = $('#txtDaysDueAfter').val();
                var NewDate = Date.parse(TaskCompletedDate).addDays(DayCount);
                var sNewDate = NewDate.toString("MM/dd/yyyy")
                $("#txtDueDate").val(sNewDate);
            }
            else {

                // alert("Cannot calculate the task due date because the selected prerequisite task is not completed.");
                // remove requred
                $("#txtDueDate").rules("remove", "required");
                $("#txtDueDate").val("");
            }
        }

        function txtDaysDueAfter_onblur() {

            var PrerequisiteID = $("#ddlPrerequisite").val();
            CalcDueDate(PrerequisiteID);
        }

        function txtDaysToEst_onblur() {

            var DaysToEst = $("#txtDaysToEst").val();
            var DaysAfterCreation = $("#txtDaysAfterCreation").val();
            if (DaysToEst != "") {

                $("#txtDueDate").datepick('destroy');

                var LoanEstCloseDate = $("#hdnEstCloseDate").val();
                if (LoanEstCloseDate != "") {

                    var DayCount = $('#txtDaysToEst').val();
                    var NewDate = Date.parse(LoanEstCloseDate).addDays(-DayCount);
                    var sNewDate = NewDate.toString("MM/dd/yyyy")
                    $("#txtDueDate").val(sNewDate);
                }
                else {

                    alert("The Estimated Close Date is missing. Default the due date to 30 days later.");
                    var ServerNow = $("#hdnNow").val();
                    var NewDate1 = Date.parse(ServerNow).addDays(30);
                    var sNewDate1 = NewDate1.toString("MM/dd/yyyy")
                    $("#txtDueDate").val(sNewDate1);
                }

                $("#ddlPrerequisite").attr("disabled", "true");
                $("#txtDaysDueAfter").attr("disabled", "true");
                $("#txtDaysDueAfter").val("");

                $("#txtDueDate").attr("readonly", "true");
            }
            else {
                if (DaysAfterCreation != "") {
                    $("#ddlPrerequisite").attr("disabled", "true");
                    $("#txtDaysDueAfter").attr("disabled", "true");
                    $("#txtDaysDueAfter").val("");

                    $("#txtDueDate").attr("readonly", "true");
                }
                else {
                    $("#ddlPrerequisite").attr("disabled", "");
                    $("#txtDaysDueAfter").attr("disabled", "");
                    $("#txtDueDate").val("");
                    $("#txtDueDate").attr("readonly", "");
                    $("#txtDueDate").datepick();
                    // add required
                    $("#txtDueDate").rules("add", {
                        required: true,
                        messages: { required: "*" }
                    });
                }
            }
        }

        // cancel
        function btnCancel_onclick() {
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            if (CloseDialogCodes == "") {
                window.parent.CloseDialog_AddTask();
            } else {
                eval(CloseDialogCodes);
            }
        }

        function BeforeSave() {

            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return false;
            }

            // require name
            if ($("#radTaskList").attr("checked") == true) {

                var SelTaskName = $("#ddlTaskList").val();
                if (SelTaskName == "-- select --") {

                    alert("Please select task name.");
                    return false;
                }
            }

            var str = $("#txtDaysDueAfterPrevStage").val();

            if (str != null && typeof (str) != "undefined" && str != "") {

                var ex = /^(\+|-)?\d+$/;
                if (ex.test(str)) {


                    if (str < -32768 || str > 32767) {
                        alert("Beyond the scope of content!");
                        $("#txtDaysDueAfterPrevStage").focus();
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    alert("Not a number!");
                    $("#txtDaysDueAfterPrevStage").focus();
                    return false;
                }

            }

            //CompletionEmail 处理



            CompletionEmailBeforeSave();

            return true;
        }


        function btnPreview2_onclick() {

            var EmailTemplateID = $("#ddlWarningEmail").val();
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }
            var LoanID = GetQueryString1("LoanID");

            OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_Preview2", 760, 580, "no", "center");

            return true;
        }

        function btnPreview3_onclick() {

            var EmailTemplateID = $("#ddlOverdueEmail").val();
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }
            var LoanID = GetQueryString1("LoanID");

            OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_Preview3", 760, 580, "no", "center");

            return true;
        }

        //#region email template list

        $(function () {

            $("#btnEmailNew").click(function () {

                var itemLen = $("#gridCompletetionEmails tbody tr").size();

                if (itemLen == 1) {
                    $("#gridCompletetionEmails").empty();
                    $("#gridCompletetionEmails").append($("#gvTmp").html());
                }
                else {
                    $("#gridCompletetionEmails").append($("#gvTmp tr").eq(1).clone());
                }

            });

            $("#btnRemove").click(function () {

                var itemLen = $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").size();

                if (itemLen == 0) {

                    alert("No email template(s) was selected.");
                    return;
                }

                var Result = confirm("This will delete the selected email template(s) for this loan task. Are you sure you want to continue?");
                if (Result == false) {

                    return;
                }

                if (itemLen == $("#gridCompletetionEmails tr:not(:first)").size()) {
                    $("#gridCompletetionEmails").empty();
                    $("#gridCompletetionEmails").append('<tr class="EmptyDataRow" align="center"><td>There is no Completion Emails.</td></tr>');
                }
                else {
                    $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").parent().parent().remove();
                }

            });

            //select all
            $("#gridCompletetionEmails input[cid='cbAll']").live("change", function () {

                if ($(this).attr("checked") == null || $(this).attr("checked") == false) {
                    $("input[cid='cbMe']").attr("checked", "");
                }
                else {
                    $("input[cid='cbMe']").attr("checked", "checked");
                }

            });


            // Preview

            $("a[cid='Preview']").live("click", function () {
                var EmailTemplateID = $(this).parent().parent().find("select").val();
                if (EmailTemplateID == "0") {

                    alert("Please select a email template.");
                    return false;
                }
                var LoanID = GetQueryString1("LoanID");

                OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_Preview7", 760, 580, "no", "center");

                return false;
            });

            $("#btnEnable").click(function () {
                var itemLen = $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").size();

                if (itemLen == 0) {

                    alert("No email template(s) was selected.");
                    return;
                }

                $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").parent().parent().find("input[cid='Enabled']").attr("checked", "checked");
            });

            $("#btnDisable").click(function () {
                var itemLen = $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").size();

                if (itemLen == 0) {

                    alert("No email template(s) was selected.");
                    return;
                }

                $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").parent().parent().find("input[cid='Enabled']").attr("checked", "");
            });
        });

        function CompletionEmailBeforeSave() {

            var itemAll = $("#gridCompletetionEmails tr:not(:first)");

            //alert(itemAll.size());
            var allStr = "";
            itemAll.each(function (i) {
                var obj = $(this);
                var Id = obj.find("input[cid='cbMe']").val();
                var emailTmpId = obj.find("select[cid='EmailTemplate']").val();
                var enable = 0;
                if (obj.find("input[cid='Enabled']:checked").size() == 1) {
                    enable = 1;
                }
                var str = Id + "," + emailTmpId + "," + enable

                if (i == 0) {
                    allStr = str;
                }
                else {
                    allStr = allStr + "|" + str;
                }
            });

            $("#hdnCompletionEmail").val(allStr);

        }

        //#endregion

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" class="margin-top-10">
        
        <div id="tabs" style="display:none;">
            <ul style="height:27px;">
                <li><a href="#tabs-1">Basic Info</a></li>
                <li><a href="#tabs-2">Alerts & Emails</a></li>
                <li><a href="#tabs-3">Due Dates</a></li>
                
            </ul>
            <div id="tabs-1">

                <table>
                    <tr>
                        <td>
                            
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <td>Owner:</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:DropDownList ID="ddlOwner" runat="server" Width="164px" DataTextField="FullName" DataValueField="UserId">
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="width:70px; vertical-align: top; padding-top:8px;">Task Name:</td>
                                    <td style="width:15px;">
                                        <input id="radTaskList" runat="server" type="radio" name="task-name-source" checked onclick="radTaskList_onclick()" />
                                    </td>
                                    <td style="width:190px;">
                                        <asp:DropDownList ID="ddlTaskList" runat="server" Width="164px" DataTextField="TaskName" DataValueField="TaskName">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                        
                                    <td>
                                        <input id="radTaskName" runat="server" type="radio" name="task-name-source" onclick="radTaskName_onclicik()" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTaskName" runat="server" Width="160px" Enabled="false" MaxLength="255"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Due Date:</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtDueDate" runat="server" Width="160px" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Due Time:</td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtDueTime" runat="server" Width="160px" Visible="false"></asp:TextBox>
                                        <asp:DropDownList ID="ddlDueTime_hour" runat="server">
                                        <asp:ListItem Value="00">12am</asp:ListItem>
                                        <asp:ListItem Value="01">01am</asp:ListItem>
                                        <asp:ListItem Value="02">02am</asp:ListItem>
                                        <asp:ListItem Value="03">03am</asp:ListItem>
                                        <asp:ListItem Value="04">04am</asp:ListItem>
                                        <asp:ListItem Value="05">05am</asp:ListItem>
                                        <asp:ListItem Value="06">06am</asp:ListItem>
                                        <asp:ListItem Value="07">07am</asp:ListItem>
                                        <asp:ListItem Value="08">08am</asp:ListItem>
                                        <asp:ListItem Value="09">09am</asp:ListItem>
                                        <asp:ListItem Value="10">10am</asp:ListItem>
                                        <asp:ListItem Value="11">11am</asp:ListItem>
                                        <asp:ListItem Value="12">12pm</asp:ListItem>
                                        <asp:ListItem Value="13">01pm</asp:ListItem>
                                        <asp:ListItem Value="14">02pm</asp:ListItem>
                                        <asp:ListItem Value="15">03pm</asp:ListItem>
                                        <asp:ListItem Value="16">04pm</asp:ListItem>
                                        <asp:ListItem Value="17">05pm</asp:ListItem>
                                        <asp:ListItem Value="18">06pm</asp:ListItem>
                                        <asp:ListItem Value="19">07pm</asp:ListItem>
                                        <asp:ListItem Value="20">08pm</asp:ListItem>
                                        <asp:ListItem Value="21">09pm</asp:ListItem>
                                        <asp:ListItem Value="22">10pm</asp:ListItem>
                                        <asp:ListItem Value="23">11pm</asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<% ddlDueTime_hour.SelectedValue = DateTime.Now.Hour.ToString(); %>--%>

                                        <asp:DropDownList ID="ddlDueTime_min" runat="server">
                                        <asp:ListItem>00</asp:ListItem>
                                        <asp:ListItem>05</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>55</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Reminder:</td>
                                    <td style="vertical-align:top; padding-top:5px;">
                                        <input id="chkReminder" runat="server" type="checkbox" checked onclick="chkReminder_onclick(this)" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlReminderInterval" runat="server" Width="164px">
                                            <asp:ListItem>5 minutes</asp:ListItem>
                                            <asp:ListItem>10 minutes</asp:ListItem>
                                            <asp:ListItem>15 minutes</asp:ListItem>
                                            <asp:ListItem>20 minutes</asp:ListItem>
                                            <asp:ListItem>30 minutes</asp:ListItem>
                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Completed:</td>
                                    <td>
                                        <input id="chkCompleted" runat="server" type="checkbox" onclick="chkCompleted_onclick(this)" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompletedDate" runat="server" Width="160px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>


                        </td>
                        <td style="vertical-align:bottom;">
                            
                            <table cellpadding="3" cellspacing="3">
                                <tr style=" height:20px;">
                                    <td>Stage:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlStage" runat="server" DataTextField="Alias" DataValueField="LoanStageId">
                                        <asp:ListItem>Contact</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
								<tr>
                                    <td colspan="2" valign="bottom">Description</td>
                                </tr>
								<tr style=" height:70px;">
                                    <td></td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" style="width:180px; height:70px;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; padding-top:8px;">Notes:</td>
                                    <td>
                                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" style="width:180px; height:80px;"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                </table>
                
                

            </div>
            <div id="tabs-2">

                <div>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 82px;">
                                Warning Email:
                            </td>
                            <td style="padding-left: 15px;">
                                <asp:DropDownList ID="ddlWarningEmail" runat="server" DataValueField="TemplEmailId" DataTextField="Name" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 20px;">
                                <input id="btnPreview2" type="button" value="Preview" class="Btn-66" onclick="btnPreview2_onclick()" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="margin-top: 9px;">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 82px;">
                                Overdue Email:
                            </td>
                            <td style="padding-left: 15px;">
                                <asp:DropDownList ID="ddlOverdueEmail" runat="server" DataValueField="TemplEmailId" DataTextField="Name" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 20px;">
                                <input id="btnPreview3" type="button" value="Preview" class="Btn-66" onclick="btnPreview3_onclick()" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="margin-top-10">

                    <div id="divToolStrip">
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a href="javascript:;" id="btnEmailNew">Add Completion Email</a><span>|</span></li>
                            <li><a href="javascript:;" id="btnRemove">Remove Completion Email</a><span>|</span></li>
                            <li><a href="javascript:;" id="btnEnable">Enable</a><span>|</span></li>
                            <li><a href="javascript:;" id="btnDisable">Disable</a></li>
                        </ul>
                    </div>
                    <div id="divLoanTask_CompletionEmails" class="ColorGrid margin-top-5">
                        <asp:GridView ID="gridCompletetionEmails" runat="server" EmptyDataText="There is no Completion Emails." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate><input cid="cbAll" type="checkbox" /></HeaderTemplate>
                                    <ItemTemplate><input cid="cbMe" type="checkbox" value="<%# Eval("TaskCompletionEmailId")%>" /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField  >
                                    <HeaderTemplate>Email Template</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlEmailTemplate" DataTextField="Name" DataValueField="TemplEmailId" runat="server" Width="370"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CheckBoxField HeaderText="Enabled" DataField="Enabled" ReadOnly="true" />
                                <asp:TemplateField>
                                    <ItemStyle Width="70" />
                                    <ItemTemplate>
                                        <a href="javascript:void(0);" cid="Preview" >Preview</a> 
                                    </ItemTemplate>    
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">&nbsp;</div>
                     </div>

                    <div id="tmpl" style=" display:none;">
                        <table class="GrayGrid" cellspacing="0" cellpadding="3" border="0" id="gvTmp" style="border-collapse:collapse;">
		                    <tr>
			                    <th scope="col" class="CheckBoxHeader"><input cid="cbAll" type="checkbox" /></th>
                                <th scope="col">Email Template</th>
                                <th scope="col">Enabled</th>
                                <th scope="col">&nbsp;</th>
		                    </tr>
        
                            <tr>
			                    <td class="CheckBoxColumn"><% cbMe.Attributes.Add("cid", "cbMe"); %><input id="cbMe" type="checkbox" runat="server" value="0" /> </td>
                                <td><% ddlEmailTemplate.Attributes.Add("cid", "EmailTemplate"); %>
                                    <asp:DropDownList ID="ddlEmailTemplate" DataTextField="Name" DataValueField="TemplEmailId" runat="server" Width="370"></asp:DropDownList>
                                </td>
                                <td style="text-align:center">
                                    <input cid="Enabled" type="checkbox" checked value="" readonly />
                                </td>
                                <td style="width:70px;">
                                    <a href="javascript:void(0);" cid="Preview" >Preview</a> 
                                </td>
		                    </tr>
	                    </table>
                    </div>

                </div>

            </div>
            <div id="tabs-3">
                
                <table cellpadding="3" cellspacing="3">
                    
                    <tr>
                        <td>Prerequisite:</td>
                        <td>
                            <asp:DropDownList ID="ddlPrerequisite" runat="server" DataValueField="LoanTaskId" DataTextField="Name" Width="200px">
                            </asp:DropDownList>
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlPrerequisite2" runat="server" DataValueField="LoanTaskId" DataTextField="Completed" Width="200px">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                </table>
                
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td>Days to Est Close:</td>
                        <td>
                            <asp:TextBox ID="txtDaysToEst" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                        </td>
                        <td style="padding-left:30px;">Days Due After Creation:</td>
                        <td>
                            <asp:TextBox ID="txtDaysAfterCreation" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td>Days due after Prerequisite completed:</td>
                        <td>
                            <asp:TextBox ID="txtDaysDueAfter" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Days due after previous stage completion date:</td>
                        <td>
                            <asp:TextBox ID="txtDaysDueAfterPrevStage" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </div>
            
            
            
        </div>
        
        <div class="margin-top-20">
            <asp:Button ID="btnSaveGoToDetail" runat="server" Text="Save and Close" CssClass="Btn-115" OnClientClick="return BeforeSave()" onclick="btnSave_Click"  />
            &nbsp;&nbsp;
            <asp:Button ID="btnCreateAnother" runat="server" Text="Save and Create Another" CssClass="Btn-160" OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="btnAddtoOutlook" runat="server" Text="Add to Outlook" CssClass="Btn-115" Enabled="false" />
            &nbsp;&nbsp;
            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
        </div>
        
        <input id="hdnNow" type="hidden" value="<%= DateTime.Now.ToString("MM/dd/yyyy hh:mm") %>" />
        <input id="hdnEstCloseDate" runat="server" type="text" style="display: none;" />
        <input id="hdnCompletionEmail" runat="server" type="text" style="display: none;" />
    </div>
    </form>
</body>
</html>