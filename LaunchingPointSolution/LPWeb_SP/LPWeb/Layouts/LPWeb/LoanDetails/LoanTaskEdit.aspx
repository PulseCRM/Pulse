<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanTaskEdit.aspx.cs" Inherits="LoanDetails_LoanTaskEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan Task Setup</title>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/datejs.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            AddValidators();

            InitControls();

            InitInputControl();

            // if completed, disabled all
            var CompletedDate = $("#hdnCompleted").val();
            if (CompletedDate != "") {

                $(":input").attr("disabled", "true");
                $("a[cid='btn']").attr("disabled", "true");
            }
        });

        function InitControls() {

            // only number
            $("#txtDaysToEst").onlypressnum();
            $("#txtDaysDueAfter").onlypressnum();

            // add event
            $("#ddlPrerequisite").change(ddlPrerequisite_onchange);
            $("#txtDaysToEst").blur(txtDaysToEst_onblur);
            $("#ddlStage").change(ddlStage_onchange);
            $("#txtDaysDueAfter").blur(txtDaysDueAfter_onblur);

            var PrerequisiteID = $("#ddlPrerequisite").val();
            var DaysToEstClose = $("#txtDaysToEst").val();

            if (PrerequisiteID == 0 && DaysToEstClose == "") {

                $("#txtDueDate").datepick();
                $("#txtDaysDueAfter").attr("disabled", "true");
            }
            else if (PrerequisiteID == 0 && DaysToEstClose != "") {

                $("#ddlPrerequisite").attr("disabled", "true");
                $("#txtDaysDueAfter").attr("disabled", "true");

                //$("#txtDueDate").attr("readonly", "true");
                //$("#txtDueDate").datepick('destroy');
                $("#txtDueDate").datepick();
                //$("#txtDueDate").rules("remove", "required");
            }
            else if (PrerequisiteID != 0 && DaysToEstClose == "") {

                $("#txtDaysToEst").attr("disabled", "true");

                $("#txtDueDate").attr("readonly", "true");
                //$("#txtDueDate").datepick('destroy');
                $("#txtDueDate").datepick();
                $("#txtDueDate").rules("remove", "required");

                // add required
                $("#txtDaysDueAfter").rules("add", {
                    required: true,
                    messages: { required: "*" }
                });
            }

            // if prerequisite, disabled Prerequisite
            var IsPrerequisite = $("#hndIsPrerequisite").val();
            if (IsPrerequisite == "True") {

                $("#ddlPrerequisite").attr("disabled", "true");
                $("#txtDaysDueAfter").attr("disabled", "true");
            }
        }

        // add jQuery Validators
        function AddValidators() {

            jQueryValidator = $("#form1").validate({

                rules: {
                    txtTaskName: {
                        required: true
                    },
                    txtDueDate: {
                        required: true
                    }
                },
                messages: {
                    txtTaskName: {
                        required: "*"
                    },
                    txtDueDate: {
                        required: "*"
                    }
                }
            });
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
                $("#txtDueDate").rules("add", {
                    required: true,
                    messages: { required: "*" }
                });

                $("#txtDaysDueAfter").attr("disabled", "true");
                $("#txtDaysDueAfter").val("");

                // remove requred
                $("#txtDaysDueAfter").rules("remove", "required");
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

                alert("Could not calculate the task due date because the prerequisite task is not completed.");
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
            var DaysFromCreation = $("#txtDaysAfterCreation").val();
            var DateCompleted = $("#hdnCompleted").val();
            if (DaysToEst != "") {
                $("#txtDueDate").datepick('destroy');
                var LoanEstCloseDate = $("#hdnEstCloseDate").val();
                if (LoanEstCloseDate != "") {
                    var DayCount = $('#txtDaysToEst').val();
                    var NewDate = Date.parse(LoanEstCloseDate).addDays(-DayCount);
                    var sNewDate = NewDate.toString("MM/dd/yyyy")
                    $("#txtDueDate").val(sNewDate);
                }
                else if (DaysFromCreation == "")
                {                                    
                    alert("Missing Estimated Close Date, default the task due date to 30 days later.");

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

                $("#ddlPrerequisite").attr("disabled", "");
                $("#txtDaysDueAfter").attr("disabled", "");

                //                $("#txtDueDate").val("");
                $("#txtDueDate").attr("readonly", "");
                $("#txtDueDate").datepick();
                $("#txtDueDate").rules("add", {
                    required: true,
                    messages: { required: "*" }
                });
            }

            // if prerequisite, disabled Prerequisite
            var IsPrerequisite = $("#hndIsPrerequisite").val();
            if (IsPrerequisite == "True") {

                $("#ddlPrerequisite").attr("disabled", "true");
                $("#txtDaysDueAfter").attr("disabled", "true");
            }
        }

        // cancel
        function btnCancel_onclick() {

            window.parent.CloseDialog_EditTask();
        }

        function BeforeDeleteTask() {

            var Result = confirm("This operation will not be reversible. Are you sure you want to continue?");
            if (Result == false) {
                return false;
            }

            return true;
        }

        function ddlStage_onchange() {

            //#region on stage changed
            var LoanID = GetQueryString1("LoanID");
            var TaskID = GetQueryString1("TaskID");
            var QueryString = "?LoanID=" + LoanID + "&TaskID=" + TaskID;


            var TaskName = $.trim($("#txtTaskName").val());
            if (TaskName != "") {

                QueryString += "&TaskName=" + encodeURIComponent(TaskName);
            }

            var Stage = $("#ddlStage").val();
            if (Stage != "") {

                QueryString += "&Stage=" + Stage;
            }

            var Owner = $("#ddlOwner").val();
            if (Owner != "0") {

                QueryString += "&Owner=" + Owner;
            }

            var DueDate = $("#txtDueDate").val();
            if (DueDate != "") {

                QueryString += "&DueDate=" + DueDate;
            }

            var DaysToEst = $("#txtDaysToEst").val();
            if (DaysToEst != "") {

                QueryString += "&DaysToEst=" + DaysToEst;
            }

            var CompletionEmail = $("#ddlCompletionEmail").val();
            if (CompletionEmail != "0") {

                QueryString += "&CompletionEmail=" + CompletionEmail;
            }

            var WarningEmail = $("#ddlWarningEmail").val();
            if (WarningEmail != "0") {

                QueryString += "&WarningEmail=" + WarningEmail;
            }

            var OverdueEmail = $("#ddlOverdueEmail").val();
            if (OverdueEmail != "0") {

                QueryString += "&OverdueEmail=" + OverdueEmail;
            }

            window.location.href = window.location.pathname + QueryString;
            //#endregion
        }

        function InitInputControl() {

            var TaskName = GetQueryString1("TaskName");
            if (TaskName != "") {

                $("#txtTaskName").val(TaskName);
            }

            var Stage = GetQueryString1("Stage");
            if (Stage != "") {

                $("#ddlStage").val(Stage);
            }

            var Owner = GetQueryString1("Owner");
            if (Owner != "") {

                $("#ddlOwner").val(Owner);
            }

            var DueDate = GetQueryString1("DueDate");
            if (DueDate != "") {

                $("#txtDueDate").val(DueDate);
            }

            var DaysToEst = GetQueryString1("DaysToEst");
            if (DaysToEst != "") {

                $("#txtDaysToEst").val(DaysToEst);
                txtDaysToEst_onblur();
            }

            // clear
            var Prerequiste = $("#ddlPrerequisite").val();
            if (Prerequiste == "0") {

                $("#txtDaysDueAfter").val("");
            }

            var CompletionEmail = GetQueryString1("CompletionEmail");
            if (CompletionEmail != "") {

                $("#ddlCompletionEmail").val(CompletionEmail);
            }

            var WarningEmail = GetQueryString1("WarningEmail");
            if (WarningEmail != "") {

                $("#ddlWarningEmail").val(WarningEmail);
            }

            var OverdueEmail = GetQueryString1("OverdueEmail");
            if (OverdueEmail != "") {

                $("#ddlOverdueEmail").val(OverdueEmail);
            }
        }

        function BeforeSave() {

            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return false;
            }

            // check stage
            var StageID = $("#ddlStage").val();
            var StageCompletedDate = $("#ddlStage2 option[value=" + StageID + "]").text();
            var TaskCompletedDate = $("#hdnCompleted").val();
            if ((StageCompletedDate != "") && (TaskCompletedDate == "")) {

                var Result = confirm("The stage that you selected is already completed. To add a task to the stage will make the stage un-completed. Are you sure you want to continue?");
                if (Result == false) {

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

        function btnPreview1_onclick() {

            var EmailTemplateID = $("#ddlCompletionEmail").val();
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }
            var LoanID = GetQueryString1("LoanID");

            OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_Preview4", 760, 580, "no", "center");

            return true;
        }

        function btnPreview2_onclick() {

            var EmailTemplateID = $("#ddlWarningEmail").val();
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }
            var LoanID = GetQueryString1("LoanID");

            OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_Preview5", 760, 580, "no", "center");
            
            return true;
        }

        function btnPreview3_onclick() {

            var EmailTemplateID = $("#ddlOverdueEmail").val();
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }
            var LoanID = GetQueryString1("LoanID");

            OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_Preview6", 760, 580, "no", "center");

            return true;
        }


        ///region MailTemplate

        $(function () {

            $("#btnEmailNew").click(function () {

                var CompletedDate = $("#hdnCompleted").val();
                if (CompletedDate != "") {
                    return;
                }
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

                var CompletedDate = $("#hdnCompleted").val();
                if (CompletedDate != "") {
                    return;
                }
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

                var CompletedDate = $("#hdnCompleted").val();
                if (CompletedDate != "") {
                    return;
                }

                if ($(this).attr("checked") == null || $(this).attr("checked") == false) {
                    $("input[cid='cbMe']").attr("checked", "");
                }
                else {
                    $("input[cid='cbMe']").attr("checked", "checked");
                }

            });


            // Preview

            $("a[cid='Preview']").live("click", function () {
                var CompletedDate = $("#hdnCompleted").val();
                if (CompletedDate != "") {
                    return;
                }

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
                var CompletedDate = $("#hdnCompleted").val();
                if (CompletedDate != "") {
                    return;
                }
                var itemLen = $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").size();

                if (itemLen == 0) {

                    alert("No email template(s) was selected.");
                    return;
                }

                $("#gridCompletetionEmails tr:not(:first) td :checkbox[cid='cbMe']:checked").parent().parent().find("input[cid='Enabled']").attr("checked", "checked");
            });

            $("#btnDisable").click(function () {
                var CompletedDate = $("#hdnCompleted").val();
                if (CompletedDate != "") {
                    return;
                }

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

            var checkStr = "";

            var allStr = "";
            itemAll.each(function (i) {
                var obj = $(this);
                var Id = obj.find("input[cid='cbMe']").val();
                var emailTmpId = obj.find("select[cid='EmailTemplate']").val();
                var enable = 0;
                if (obj.find("input[cid='Enabled']:checked").size() == 1) {
                    enable = 1;
                }

                //check same
                //Have the same template, the same template, select only one

                if ((checkStr + ",").indexOf("," + emailTmpId + ",") >= 0) {

                    alert("The same template, select only one !");
                    allStr = "{=}";
                    return false;
                }


                var str = Id + "," + emailTmpId + "," + enable
                checkStr = "," + emailTmpId;

                if (i == 0) {
                    allStr = str;
                }
                else {
                    allStr = allStr + "|" + str;
                }
            });

            if (allStr == "{=}") {
                return false;
            }

            $("#hdnCompletionEmail").val(allStr);

            return true;
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 580px;">
        <div>
         <div style="margin-top: 8px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave();" onclick="btnSave_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDeleteTask();"
                                onclick="btnDelete_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Borrower:
                        </td>
                        <td style="padding-left: 15px; width: 150px;">
                            <asp:Label ID="lbBorrower" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            Property:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:Label ID="lbProperty" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 15px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            <asp:Image ID="imgTaskIcon" runat="server" /> Task Name:
                        </td>
                        <td style="padding-left: 15px; width: 219px;">
                            <asp:TextBox ID="txtTaskName" runat="server" Width="196px" MaxLength="255"></asp:TextBox>
                        </td>
                        <td>
                            Stage:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlStage" runat="server" DataValueField="LoanStageID" DataTextField="StageName" Width="150px">
                            </asp:DropDownList>
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlStage2" runat="server" DataValueField="LoanStageID" DataTextField="Completed">
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Owner:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlOwner" runat="server" DataValueField="UserID" DataTextField="FullName" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td  style="padding-left: 20px;">
                            <asp:CheckBox ID="chbExternalViewing" runat="server" Text=" External Viewing" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Due:
                        </td>
                        <td style="padding-left: 15px; width: 131px;">
                            <asp:TextBox ID="txtDueDate" runat="server" CssClass="DateField"></asp:TextBox>
                        </td>
                        
                        <%--<td style="padding-left: 15px; width: 90px;">
                            <asp:TextBox ID="txtCompleted" runat="server" CssClass="DateField" ReadOnly="true"></asp:TextBox>
                        </td>--%>
                       
                        <td style="padding-left: 10px;">
                            Days to Est Close:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtDaysToEst" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                        </td>
                        <td style="padding-left: 10px;">
                            Days Due After Creation:
                        </td>
                        <td style ="padding-left: 10px;">
                        <asp:TextBox ID="txtDaysAfterCreation" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                    </tr>
                </table>
            </div>

            
             <div >
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Days due after completion Date of Previous Stage:
                        </td>
                        <td style="padding-left: 15px; width: 80px;">
                            <asp:TextBox ID="txtDaysDueAfterPrevStage" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                        </td>                        
                    </tr>
                </table>
            </div>

            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Prerequisite:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlPrerequisite" runat="server" DataValueField="LoanTaskId" DataTextField="Name" Width="200px">
                            </asp:DropDownList>
                            <div style="display: none;">
                                <asp:DropDownList ID="ddlPrerequisite2" runat="server" DataValueField="LoanTaskId" DataTextField="Completed" Width="200px">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td style="padding-left: 20px;">
                            Days due after Prerequisite completed:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txtDaysDueAfter" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px; display:none;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Completion Email:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlCompletionEmail" runat="server" DataValueField="TemplEmailId" DataTextField="Name" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 30px;">
                            <input id="btnPreview1" type="button" value="Preview" class="Btn-66" onclick="btnPreview1_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Warning Email:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlWarningEmail" runat="server" DataValueField="TemplEmailId" DataTextField="Name" Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 30px;">
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
                        <td style="padding-left: 30px;">
                            <input id="btnPreview3" type="button" value="Preview" class="Btn-66" onclick="btnPreview3_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divLoanTask_CompletionEmails" class="ColorGrid" style="margin-top: 10px; width: 520px;">
            <div id="div1">
                <ul class="ToolStrip" style="margin-left: 0px;">
                    <li><a href="javascript:;" cid="btn" id="btnEmailNew">Add Completion Email</a><span>|</span></li>
                    <li><a href="javascript:;" cid="btn" id="btnRemove">Remove Completion Email</a><span>|</span></li>
                    <li><a href="javascript:;" cid="btn" id="btnEnable">Enable</a><span>|</span></li>
                    <li><a href="javascript:;" cid="btn" id="btnDisable">Disable</a></li>
                </ul>
            </div>
            <div style=" height:125px; width:539px; overflow:auto">
            <asp:GridView ID="gridCompletetionEmails" runat="server" Width="519" OnRowDataBound="gridCompletetionEmails_RowDataBound" EmptyDataText="There is no Completion Emails." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="20" />
                        <HeaderTemplate><input cid="cbAll" type="checkbox" /></HeaderTemplate>
                        <ItemTemplate><input cid="cbMe" type="checkbox" value="<%# Eval("TaskCompletionEmailId")%>" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  >
                        <HeaderTemplate>Email Template</HeaderTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlEmailTemplate" DataTextField="Name" DataValueField="TemplEmailId" runat="server" Width="370"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <HeaderTemplate>Enabled</HeaderTemplate>
                        <ItemTemplate>
                        <input cid="Enabled" type="checkbox" value="" <%# (Eval("Enabled") != null && Convert.ToBoolean(Eval("Enabled")) == true) ? "checked" : ""%>  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="70" />
                        <ItemTemplate>
                            <a href="javascript:void(0);" cid="Preview" >Preview</a> 
                        </ItemTemplate>    
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
            <div id="tmpl" style=" display:none;">
                <table class="GrayGrid" cellspacing="0" cellpadding="3" border="0" id="gvTmp" style="border-collapse:collapse;">
		            <tr>
			            <th scope="col"><input cid="cbAll" type="checkbox" /></th>
                        <th scope="col">Email Template</th>
                        <th scope="col">Enabled</th>
                        <th scope="col">&nbsp;</th>
		            </tr>
        
                    <tr>
			            <td style="width:20px;"><% cbMe.Attributes.Add("cid", "cbMe"); %><input id="cbMe" type="checkbox" runat="server" value="0" /> </td>
                        <td><% ddlEmailTemplate.Attributes.Add("cid", "EmailTemplate"); %>
                            <asp:DropDownList ID="ddlEmailTemplate" DataTextField="Name" DataValueField="TemplEmailId" runat="server" Width="370"></asp:DropDownList>
                        </td>
                        <td>
                        <input cid="Enabled" type="checkbox" checked value="" />
                        </td>
                        <td style="width:70px;">
                            <a href="javascript:void(0);" cid="Preview" >Preview</a> 
                        </td>
		            </tr>
	            </table>


            </div>
            </div>
            </div>

           <%-- <div style="margin-top: 8px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDeleteTask();"
                                onclick="btnDelete_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>--%>
        </div>
        <input id="hdnEstCloseDate" runat="server" type="text" style="display: none;" />
        <input id="hndIsPrerequisite" runat="server" type="text" style="display: none;" />
        <input id="hdnNow" runat="server" type="text" style="display: none;" />
        <asp:HiddenField ID="hdnCompleted" runat=server />
        <input id="hdnCompletionEmail" runat="server" type="text" style="display: none;" />

    </div>
    </form>
</body>
</html>