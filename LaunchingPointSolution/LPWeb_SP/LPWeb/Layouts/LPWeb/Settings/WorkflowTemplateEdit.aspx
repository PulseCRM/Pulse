<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowTemplateEdit.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.WorkflowTemplateEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Workflow Template Setup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            AddValidators();

            $("#txtDesc").maxlength(500);

            // disable ddlWorkflowType
            if ($("#gridStageList tr").length > 1) {

                $("#ddlWorkflowType").attr("disabled", "true");
                $("#ddlCalcDueDateMethod").attr("disabled", "true");
            }

            //#region add validator to gridStageList

            $("#gridStageList tr td :input[id^='ddlStage']").each(function (i) {

                $(this).rules("add", {
                    required: true,
                    messages: {
                        required: "<div>Please select Stage.</div>"
                    }
                });

                // add event
                var RowIndex = i + 1;
                $(this).change(function () {

                    var SelStageID = $(this).val();
                    var StageAlias = $("#ddlStageTemplateList").find("option[value='" + SelStageID + "']").text();
                    $("#txtDisplayedAs" + RowIndex).val(StageAlias);
                });
            });

            $("#gridStageList tr td :text[id^='txtDisplayedAs']").each(function (i) {

                $(this).rules("add", {
                    required: true,
                    messages: {
                        required: "<div>Please enter Displayed As.</div>"
                    }
                });
            });


            var CalcMethod = $("#ddlCalcDueDateMethod").val();
            var SelWorkflowType = $("#ddlWorkflowType").val();

            //Rocky20110515 Set DaysFromEstCloseDate/DaysAfterCreationDate is required based on stage calcduedateMethod
            var TmpCalcMethod = "1";
            if (CalcMethod == "Creation Date") {
                TmpCalcMethod = "2";
            }

            $("#gridStageList tr td :text[id^='txtDaysFromEstCloseDate']").each(function (i) {
                var myCalcMethod = "1";
                if ($(this).attr("myCalculationMethod") != "" && $(this).attr("myCalculationMethod") != "0") {
                    myCalcMethod = $(this).attr("myCalculationMethod");
                }
                else {
                    myCalcmethod = TmpCalcMethod;
                }
                if (myCalcMethod == "1") {
                    $(this).rules("add", {
                        required: true,
                        range: [-365, 365],
                        messages: {
                            required: "<div>Required.</div>",
                            range: "<div>-365 to 365.</div>"
                        }
                    });

                    // only int
                    $(this).OnlyInt();
                }
                else {
                    // readonly
                    $(this).attr("readonly", "true");
                }

                // set parent.iframe.height
                var xx = $("#divContainer").height() + 10;
                //alert(xx);
                if (xx > 700) {

                    $(window.parent.document).find("#ifrWflTemplateSetup").height(xx);
                }
            });

            $("#gridStageList tr td :text[id^='txtDaysAfterCreationDate']").each(function (i) {
                var myCalcMethod = "2";
                if ($(this).attr("myCalculationMethod1") != "" && $(this).attr("myCalculationMethod1") != "0") {
                    myCalcMethod = $(this).attr("myCalculationMethod1");
                }
                else {
                    myCalcmethod = TmpCalcMethod;
                }
                if (myCalcMethod == "2") {
                    $(this).rules("add", {
                        required: true,
                        range: [-365, 365],
                        messages: {
                            required: "<div>Required.</div>",
                            range: "<div>-365 to 365.</div>"
                        }
                    });

                    // only int
                    $(this).OnlyInt();
                }
                else {
                    // readonly
                    $(this).attr("readonly", "true");
                }
            });


            //#endregion

            //#endregion

            //#region if not custom, disable all inputs

            var IsCustom = $("#hdnIsCustom").val();
            if (IsCustom == "False") {

                $(":input").attr("disabled", true);
                $("a").attr("disabled", true);
                $("a").removeAttr("href");
                $("a").css("text-decoration", "none");

                $("#btnClone").attr("disabled", "");
                $("#btnCancel").attr("disabled", "");
            }

            //#endregion

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflTemplateSetup").height(xx);
            }
        });

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {
                    txtWorkflowTemplate: {
                        required: true
                    },
                    ddlWorkflowType: {
                        required: true
                    }
                },
                messages: {
                    txtWorkflowTemplate: {
                        required: "<div>Please enter Workflow Template.</div>"
                    },
                    ddlWorkflowType: {
                        required: "<div>Please select Workflow Type.</div>"
                    }
                }
            });
        }

        //#region Add/Remove Stage

        function aAddStage_onclick() {

            var SelWorkflowType = $("#ddlWorkflowType").val();
            if (SelWorkflowType == "") {

                alert("Please select Workflow Type at first.");
                return;
            }

            var TrCount = $("#gridStageList tr").length;

            // add th
            if (TrCount == 1) {

                // clear tr
                $("#gridStageList").empty();

                // add th
                $("#gridStageList").append($("#gridStageList1 tr").eq(0).clone());
            }

            // next index
            var NowIndex = new Number($("#hdnCounter").val());
            var NextIndex = NowIndex + 1;

            // clone tr
            var TrCopy = $("#gridStageList1 tr").eq(1).clone(true);

            //#region Add Tr

            // ddlSeq
            var last_seq_value = "0";
            if ($("#gridStageList tr td #ddlSeq:last").length > 0) {

                last_seq_value = $("#gridStageList tr td #ddlSeq:last").val();
            }
            var xSeq = new Number(last_seq_value) + 10;
            if (xSeq > 999) {

                xSeq = 999;
            }
            TrCopy.find("#ddlSeq").val(xSeq);

            //#region ddlStage

            var ddlStage_NewID = "ddlStage" + NextIndex;

            var ddlStageTemp = "";
            var ddlStageCode = "";

            if (SelWorkflowType == "Processing") {

                ddlStageTemp = $.trim($("#hdnStageProcessing").text());
                ddlStageCode = ddlStageTemp.replace(/ddlStageProcessing/g, ddlStage_NewID);
            }
            else {

                ddlStageTemp = $.trim($("#hdnStageProspect").text());
                ddlStageCode = ddlStageTemp.replace(/ddlStageProspect/g, ddlStage_NewID);
            }

            //alert(ddlStageTemp);
            //alert(ddlStageCode);

            TrCopy.find("#ddlStage").replaceWith(ddlStageCode);

            //#endregion

            //#region txtDisplayedAs

            var txtDisplayedAs_NewID = "txtDisplayedAs" + NextIndex;
            var txtDisplayedAsTemp = $.trim($("#hdnDisplayAsTemp").val());
            //alert(txtDisplayedAsTemp);
            var txtDisplayedAsCode = txtDisplayedAsTemp.replace(/txtDisplayedAs/g, txtDisplayedAs_NewID);
            //alert(txtDisplayedAsCode);

            TrCopy.find("#txtDisplayedAs").replaceWith(txtDisplayedAsCode);

            //#endregion

            //#region txtDaysFromEstCloseDate

            var txtDaysFromEstCloseDate_NewID = "txtDaysFromEstCloseDate" + NextIndex;
            var txtDaysFromEstCloseDateTemp = $.trim($("#hdnDaysFromEstCloseDateTemp").val());
            //alert(txtDaysFromEstCloseDateTemp);
            var txtDaysFromEstCloseDateCode = txtDaysFromEstCloseDateTemp.replace(/txtDaysFromEstCloseDate/g, txtDaysFromEstCloseDate_NewID);
            //alert(txtDaysFromEstCloseDateCode);

            TrCopy.find("#txtDaysFromEstCloseDate").replaceWith(txtDaysFromEstCloseDateCode);

            //#endregion

            //#region txtDaysAfterCreationDate

            var txtDaysAfterCreationDate_NewID = "txtDaysAfterCreationDate" + NextIndex;
            var txtDaysAfterCreationDateTemp = $.trim($("#hdnDaysAfterCreationDateTemp").val());
            //alert(txtDaysAfterCreationDateTemp);
            var txtDaysAfterCreationDateCode = txtDaysAfterCreationDateTemp.replace(/txtDaysAfterCreationDate/g, txtDaysAfterCreationDate_NewID);
            //alert(txtDaysAfterCreationDateCode);

            TrCopy.find("#txtDaysAfterCreationDate").replaceWith(txtDaysAfterCreationDateCode);

            //#endregion

            // append tr
            $("#gridStageList").append(TrCopy);

            // set counter
            $("#hdnCounter").val(NextIndex);

            // disable Workflow Type
            $("#ddlWorkflowType").attr("disabled", "true");

            // disable Calc. Method
            $("#ddlCalcDueDateMethod").attr("disabled", "true");

            //#region add validate

            $("#" + ddlStage_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please select Stage.</div>"
                }
            });

            $("#" + txtDisplayedAs_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please enter Displayed As.</div>"
                }
            });
            //$("#" + txtDisplayedAs_NewID).alphanumeric({ ichars: "$" });

            // add event
            $("#" + ddlStage_NewID).change(function () {

                var SelStageID = $(this).val();
                var StageAlias = $("#ddlStageTemplateList").find("option[value='" + SelStageID + "']").text();
                $("#" + txtDisplayedAs_NewID).val(StageAlias);
            });

            //#region DaysFromEstClose DaysAfterCreateion

            var CalcMethod = $("#ddlCalcDueDateMethod").val();
            if (CalcMethod == "Est Close Date") {

                $("#" + txtDaysFromEstCloseDate_NewID).rules("add", {
                    required: true,
                    range: [-365, 365],
                    messages: {
                        required: "<div>Required.</div>",
                        range: "<div>-365 to 365.</div>"
                    }
                });

                // readonly
                $("#" + txtDaysFromEstCloseDate_NewID).attr("readonly", "");
                $("#" + txtDaysAfterCreationDate_NewID).attr("readonly", "true");
            }
            else {

                $("#" + txtDaysAfterCreationDate_NewID).rules("add", {
                    required: true,
                    range: [-365, 365],
                    messages: {
                        required: "<div>Required.</div>",
                        range: "<div>-365 to 365.</div>"
                    }
                });

                // readonly
                $("#" + txtDaysFromEstCloseDate_NewID).attr("readonly", "true");
                $("#" + txtDaysAfterCreationDate_NewID).attr("readonly", "");
            }

            //#endregion

            // only number
            $("#" + txtDaysFromEstCloseDate_NewID).OnlyInt();
            $("#" + txtDaysAfterCreationDate_NewID).OnlyInt();

            //#endregion

            //#endregion

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflTemplateSetup").height(xx);
            }
        }

        function aRemoveStage_onclick() {

            var SelectedCount = $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No stage was selected.");
                return;
            }

            //#region check is referenced

            var IsRef = false;
            $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").each(function (i) {

                var WflStageId = $(this).attr("myWlfStageID");
                //alert(WflStageId);
                var TaskCount = $(this).parent().parent().find("a[id='aTaskCount']").text();
                //alert(TaskCount);

                if (WflStageId > 0 && TaskCount > 0) {

                    IsRef = true;
                }
            });

            //#endregion

            var ConfirmMsg = "";
            if (IsRef == true) {

                ConfirmMsg = "The selected stage(s) has been referenced in workflow tasks. Deleting the stage(s) will also remove the associated tasks and loan task references. \r\n\r\nAre you sure you want to continue?";

            }
            else {

                ConfirmMsg = "Are you sure you want to continue?";
            }

            var Result = confirm(ConfirmMsg);
            if (Result == false) {

                return;
            }

            //#region store removed stage ids

            $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").each(function (i) {

                var WflStageId = $(this).attr("myWlfStageID");
                if (WflStageId > 0) {

                    var StageIDsNow = $("#hdnRemovedStageIDs").val();
                    if (StageIDsNow == "") {

                        $("#hdnRemovedStageIDs").val(WflStageId);
                    }
                    else {

                        $("#hdnRemovedStageIDs").val(StageIDsNow + "," + WflStageId);
                    }
                }
            });

            //#endregion

            // remove row
            if (SelectedCount == $("#gridStageList tr:not(:first)").length) {

                $("#gridStageList").empty();
                $("#gridStageList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no stage, please add.</td></tr>");

                // enable Workflow Type
                $("#ddlWorkflowType").attr("disabled", "");

                $("#ddlCalcDueDateMethod").attr("disabled", "");
            }
            else {

                $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").parent().parent().remove();
            }

            // reset sequence
            //ResetSeq();

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflTemplateSetup").height(xx);
            }
            else {

                $(window.parent.document).find("#ifrWflTemplateSetup").height(700);
            }
        }

        function ResetSeq() {

            for (var i = 1; i < $("#gridStageList tr").length; i++) {

                var xSeq = i * 10;
                if (xSeq > 999) {

                    xSeq = 999;
                }
                $("#gridStageList tr").eq(i).find(":input[id='ddlSeq']").val(xSeq);
            }
        }

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridStageList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridStageList tr td :checkbox").attr("checked", "");
            }
        }

        //#endregion

        function aCreateStage_onclick() {
//            var result = confirm("Your changes have not been saved, if you leave this page now, you will lose the changes you have made so far. You probably want to save the changes first.\r\n\r\nDo you still want to continue and lose your changes?");
//            if (result == false) {

//                return;
//            }
            CreateStage();  //Create Stage
        }

        function CreateStage() {


            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrStageAdd").attr("src", "StageTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&stageid=0&sid=" + radomStr);
            $("#divAddStage").dialog({
                height: 400,
                width: 730,
                title: "Stage template setup",
                modal: true,
                resizable: false
            });

            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            $("body>div[role=dialog]").appendTo("#aspnetForm");
            return false;
        }

        //#region Update Stage

        function aUpdateStage_onclick() {

            var SelectedCount = $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").length;
            if (SelectedCount == 0) {

                alert("No stage was selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one stage can be selected.");
                return;
            }

//            var result = confirm("Your changes have not been saved, if you leave this page now, you will lose the changes you have made so far. You probably want to save the changes first.\r\n\r\nDo you still want to continue and lose your changes?");
//            if (result == false) {

//                return;
//            }

            var WflStageID = $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").attr("myWlfStageID");
            //alert(WflStageID);

            GoToWflStageSetup(WflStageID);
        }



        function GoToWflStageSetup(WflStageID) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");

            window.parent.location.href = "WorkflowStageSetup2.aspx?sid=" + sid + "&WflStageID=" + WflStageID + "&WorkflowTemplateID=" + WorkflowTemplateID + "&from=" + encodeURIComponent(window.parent.location.href);
        }

        //#endregion



        //#region before save

        function DoValidate() {

            // call validate
            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return;
            }

            //#region validate stage list - sequence duplication

            var IsDuplicated = false;
            $("#gridStageList tr td :input[id='ddlSeq']").each(function (i) {

                var ThisSeq = $(this).val();

                for (var j = i + 1; j < $("#gridStageList tr td :input[id='ddlSeq']").length; j++) {

                    var OtherSeq = $("#gridStageList tr td :input[id='ddlSeq']").eq(j).val();

                    if (ThisSeq == OtherSeq) {

                        IsDuplicated = true;
                        return;
                    }
                }
            });

            if (IsDuplicated == true) {

                alert("The Sequence in stage list can not be duplicated.");
                return;
            }

            //#endregion

            //#region validate stage list - stage template id

            IsDuplicated = false;
            $("#gridStageList tr td :input[id^='ddlStage']").each(function (i) {

                var ThisID = $(this).val();

                for (var j = i + 1; j < $("#gridStageList tr td :input[id^='ddlStage']").length; j++) {

                    var OtherID = $("#gridStageList tr td :input[id^='ddlStage']").eq(j).val();

                    if (ThisID == OtherID) {

                        IsDuplicated = true;
                        return;
                    }
                }
            });

            if (IsDuplicated == true) {

                alert("The Stage Name in stage list can not be duplicated.");
                return;
            }

            //#endregion

            CheckDuplication();
        }

        function CheckDuplication() {

            //#region check duplication of workflow template name

            var WorkflowTemplateName = $.trim($("#txtWorkflowTemplate").val());

            // show waiting dialog
            window.parent.ShowWaitingDialog("Checking duplicates...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);
            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");

            // Ajax
            $.getJSON("CheckWflTempNameDuplicate.ashx?sid=" + Radom + "&WflTempID=" + WorkflowTemplateID + "&WflTempName=" + encodeURIComponent(WorkflowTemplateName), AfterCheckDuplication);

            //#endregion
        }

        function AfterCheckDuplication(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    window.parent.CloseWaitingDialog()
                    return;
                }

                if (data.IsDuplicated == "true") {

                    window.parent.CloseWaitingDialog()
                    alert("The workflow template name was used by others, please change another one.");
                    return;
                }

                //$('#divContainer').unblock();

                //#region check default

                var IsDefault = $("#chkDefault").attr("checked");
                if (IsDefault == true) {

                    CheckDefault();
                }
                else {

                    CallPostBack();
                }

                //#endregion

            }, 2000);
        }

        function CheckDefault() {

            // show waiting dialog
            window.parent.ShowWaitingDialog("Checking defaults...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var WorkflowType = $("#ddlWorkflowType").val();
            //alert(WorkflowType);

            // Ajax
            $.getJSON("GetDefaultWflTempCount.ashx?sid=" + Radom + "&WorkflowType=" + encodeURIComponent(WorkflowType), AfterCheckDefault);

        }

        function AfterCheckDefault(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    window.parent.CloseWaitingDialog()
                    return;
                }

                window.parent.CloseWaitingDialog()

                var CountNum = new Number(data.DefaultCount);
                //alert(CountNum);
                if (CountNum > 1) {

                    var WorkflowType = $("#ddlWorkflowType").val();
                    var result = confirm("More than one default Workflow Template has been configured for Workflow Type " + WorkflowType + ". Do you want to make this one the default Worklfow Template for Workflow Type " + WorkflowType + "?");
                    if (result == false) {

                        return;
                    }
                }

                CallPostBack();

            }, 2000);
        }

        function CallPostBack() {

            //#region WflStageIDs

            var WflStageIDs = "";
            $("#gridStageList tr td :input[id='chkChecked']").each(function (i) {

                var value = $(this).attr("myWlfStageID");

                if (WflStageIDs == "") {

                    WflStageIDs = value;
                }
                else {

                    WflStageIDs += "," + value;
                }
            });

            //alert(WflStageIDs);
            $("#hdnWflStageIDs").val(WflStageIDs);

            //#endregion

            //#region Sequences

            var Sequences = "";
            $("#gridStageList tr td :input[id='ddlSeq']").each(function (i) {

                var ThisSeq = $(this).val();

                if (Sequences == "") {

                    Sequences = ThisSeq;
                }
                else {

                    Sequences += "," + ThisSeq;
                }
            });

            //alert(Sequences);
            $("#hdnSequences").val(Sequences);

            //#endregion

            //#region StageTempIDs

            var StageTempIDs = "";
            $("#gridStageList tr td :input[id^='ddlStage']").each(function (i) {

                var value = $(this).val();

                if (StageTempIDs == "") {

                    StageTempIDs = value;
                }
                else {

                    StageTempIDs += "," + value;
                }
            });

            //alert(StageTempIDs);
            $("#hdnStageTemplateIDs").val(StageTempIDs);

            //#endregion

            //#region Stage Names

            var StageNames = "";
            $("#gridStageList tr td :input[id^='ddlStage']").each(function (i) {

                var value = $(this).val();
                var StageName = $(this).find("option[value='" + value + "']").text();

                if (StageNames == "") {

                    StageNames = "[$" + StageName + "$]";
                }
                else {

                    StageNames += ",[$" + StageName + "$]";
                }
            });

            //alert(StageNames);
            $("#hdnStageNames").val(StageNames);

            //#endregion

            //#region StageEnableds

            var StageEnableds = "";
            $("#gridStageList tr td :checkbox[id='chkStageEnabled']").each(function (i) {

                var CheckedValue = $(this).attr("checked");

                if (i == 0) {

                    StageEnableds = CheckedValue;
                }
                else {

                    StageEnableds += "," + CheckedValue;
                }
            });

            //alert("StageEnableds: " + StageEnableds);
            $("#hdnEnableds").val(StageEnableds);

            //#endregion

            //#region Days from Est Close Date

            var DaysFromEstCloseDates = "";
            $("#gridStageList tr td :input[id^='txtDaysFromEstCloseDate']").each(function (i) {

                var value = $(this).val();
                if (value == "") {

                    value = "null";
                }

                if (DaysFromEstCloseDates == "") {

                    DaysFromEstCloseDates = value;
                }
                else {

                    DaysFromEstCloseDates += "," + value;
                }
            });

            //alert(DaysFromEstCloseDates);
            $("#hdnDaysFromEstCloseDates").val(DaysFromEstCloseDates);

            //#endregion

            //#region Days after Creation Date

            var DaysAfterCreationDates = "";
            $("#gridStageList tr td :input[id^='txtDaysAfterCreationDate']").each(function (i) {

                var value = $(this).val();
                if (value == "") {

                    value = "null";
                }

                if (DaysAfterCreationDates == "") {

                    DaysAfterCreationDates = value;
                }
                else {

                    DaysAfterCreationDates += "," + value;
                }
            });

            //alert(DaysAfterCreationDates);
            $("#hdnDaysAfterCreationDates").val(DaysAfterCreationDates);

            //#endregion

            // submit form
            $("#ddlWorkflowType").attr("disabled", "");
            $("#ddlCalcDueDateMethod").attr("disabled", "");
            __doPostBack("btnSave", "");
        }

        //#endregion

        function BeforeDelete() {

            var result = confirm("This will delete the workflow template, stages, and the tasks. If this workflow template has been applied in the loans, the workflow information and history will be removed as well. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }

            return true;
        }

        function btnClone_onclick() {

            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.parent.location.href = "WorkflowTemplateClone2.aspx?sid=" + sid + "&WorkflowTemplateID=" + WorkflowTemplateID;
        }

        function btnRegen_onclick() {

            var result = confirm("This will apply the workflow template to all the loans that currently have this workflow template selected. \r\n\r\nThis will have a significant impact on the system performance.  It is highly recommended that you perform this task during the off hours or when the system is not busy. \r\n\r\nAre you sure you want to do it now? ");
            if (result == false) {

                return;
            }

            // show waiting dialog
            window.parent.ShowWaitingDialog("Regenerating workflows...");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");

            // Ajax
            $.getJSON("RegenerateWorkflowAjax.aspx?sid=" + sid + "&WorkflowTemplateID=" + WorkflowTemplateID, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        window.parent.CloseWaitingDialog()

                        return;
                    }

                    window.parent.CloseWaitingDialog()

                }, 2000);
            });
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 1000px;border: solid 0px red;">
        <input id="btnGoToList" type="button" value="Back to Workflow Template List" onclick="javascript:window.parent.location.href='WorkflowTemplateList.aspx'" class="Btn-250" />
        <br />
        <br />
        <div id="divWorkflowDetails">
            <table>
                <tr>
                    <td style="width: 110px;">Workflow Template</td>
                    <td style="width: 350px;">
                        <asp:TextBox ID="txtWorkflowTemplate" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 85px;">Workflow Type</td>
                    <td style="width: 160px;">
                        <asp:DropDownList ID="ddlWorkflowType" runat="server" Width="110px">
                            <asp:ListItem Value="">-- select --</asp:ListItem>
                            <asp:ListItem>Processing</asp:ListItem>
                            <asp:ListItem>Prospect</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
                    </td>
                    <td><label for="chkEnabled">Enabled</label></td>
                    
                    <td style="padding-left:15px;"><asp:CheckBox ID="chkDefault" runat="server" /></td>
                     <td><label for="chkDefault">Default</label></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">Description</td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Width="750px" Height="50px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="margin-top: 2px;">
                <tr>
                    <td style="width: 110px;">Calculate Due Dates<br>based on</td>
                    <td style="width: 170px; vertical-align: top;">
                        <asp:DropDownList ID="ddlCalcDueDateMethod" runat="server" Width="250px">
                            <asp:ListItem>Est Close Date</asp:ListItem>
                            <asp:ListItem>Creation Date</asp:ListItem>
                            <asp:ListItem>Completion Date of the previous Stage</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 175px;">
                        
                        
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="DoValidate(); return false;" onclick="btnSave_Click" />
                        <input id="btnClone" type="button" value="Clone" class="Btn-66" onclick="return btnClone_onclick()" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDelete()" OnClick="btnDelete_Click" />
                        <input id="btnRegen" runat="server" type="button" value="Regenerate Workflows" onclick="return btnRegen_onclick()" class="Btn-140" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-top: 20px;">
            <div id="divToolBar">
                <ul class="ToolStrip" style="margin-left: 0px;">
                    <li><a id="aAdd" href="javascript:aAddStage_onclick()">Add Stage</a><span>|</span></li>
                    <li><a id="aRemove" href="javascript:aRemoveStage_onclick()">Remove Stage</a><span>|</span></li>
                    <li><a id="aCreate" href="javascript:aCreateStage_onclick()">Create Stage</a><span>|</span></li>
                    <li><a id="aUpdate" href="javascript:aUpdateStage_onclick()">Update Stage</a></li>
                </ul>
            </div>
            <div">
            <div id="divStageList" class="ColorGrid" style="margin-top: 5px;">
                <asp:GridView ID="gridStageList" runat="server" EmptyDataText="There is no stage, please add." AutoGenerateColumns="False" CellPadding="4" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" myWlfStageID="<%# Eval("WflStageId") %>"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sequence" ItemStyle-Width="60px">
                            <ItemTemplate>
                                <select id="ddlSeq" style="width: 50px;">
                                    <%# this.GetOptions_ddlSeq(Eval("SequenceNumber").ToString())%>
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stage">
                            <ItemTemplate>
                                <select name="ddlStage<%# Eval("RowIndex") %>" id="ddlStage<%# Eval("RowIndex") %>" style="width:245px;">
                                    <%# this.GetOptions_ddlStage(Eval("TemplStageId").ToString())%>
                                </select>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Displayed As" ItemStyle-Width="230px">
                            <ItemTemplate>
                                <input id="txtDisplayedAs<%# Eval("RowIndex") %>" name="txtDisplayedAs<%# Eval("RowIndex") %>" type="text" value="<%# Eval("Alias") %>" maxlength="50" readonly style="width: 165px;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enabled" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <input id="chkStageEnabled" type="checkbox" <%# Eval("Enabled").ToString() == "False" ? "" : "checked" %> />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Days from<br />Est Close" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <input id="txtDaysFromEstCloseDate<%# Eval("RowIndex") %>" name="txtDaysFromEstCloseDate<%# Eval("RowIndex") %>" type="text" value="<%# Eval("DaysFromEstClose") %>" maxlength="4" style="width: 60px;" myCalculationMethod="<%# Eval("CalculationMethodCode") %>"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Days after<br />Creation date" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <input id="txtDaysAfterCreationDate<%# Eval("RowIndex") %>" name="txtDaysAfterCreationDate<%# Eval("RowIndex") %>" type="text" value="<%# Eval("DaysFromCreation") %>" maxlength="4" style="width: 60px;"  myCalculationMethod1="<%# Eval("CalculationMethodCode") %>"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tasks" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <a id="aTaskCount" href="javascript:GoToWflStageSetup('<%# Eval("WflStageId") %>')"><%# Eval("TaskCount") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
            </div>
            <div id="divStageList1" class="ColorGrid" style="margin-top: 5px; display: none;">
                <div>
                    <table id="gridStageList1" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                        <tr>
	                        <th class="CheckBoxHeader" scope="col"><input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" /></th>
	                        <th scope="col">Sequence</th>
	                        <th scope="col">Stage</th>
	                        <th scope="col">Displayed As</th>
                            <th scope="col">Enabled</th>
	                        <th scope="col">Days from<br />Est Close</th>
	                        <th scope="col">Days after<br />Creation date</th>
                            <th scope="col">Tasks</th>
                        </tr>
                        <tr>
	                        <td class="CheckBoxColumn">
                                <input id="chkChecked" type="checkbox" myWlfStageID="0" />
                            </td>
	                        <td style="width: 60px;">
                                <select id="ddlSeq" style="width: 50px;">
                                    <asp:Literal ID="ltrSeqOptions" runat="server"></asp:Literal>
                                </select>
                            </td>
                            <td>
                                <select id="ddlStage" style="width: 245px;">
                                </select>
                            </td>
	                        <td style="width: 230px;">
                                <input id="txtDisplayedAs" type="text" maxlength="50" readonly style="width: 165px;" />
                            </td>
	                        <td style="width: 50px; text-align: center;">
                                <input id="chkStageEnabled" type="checkbox" checked />
                            </td>
	                        <td style="width: 100px;">
                                <input id="txtDaysFromEstCloseDate" type="text" maxlength="4" style="width: 60px;" />
                            </td>
                            <td style="width: 100px;">
                                <input id="txtDaysAfterCreationDate" type="text" maxlength="4" style="width: 60px;" />
                            </td>
                            <td style="width: 40px; text-align: center">
                                <a id="aTaskCount" href="javascript:alert('Please save this workflow template at first.')">0</a>
                            </td>
                        </tr>
                        
                    </table>
                </div>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        
    </div>
    <div id="divCodeTemplates" style="display: none;">
        <dl>
            <dd>
                <textarea id="hdnStageProcessing" cols="20" rows="2">
                    <asp:DropDownList ID="ddlStageProcessing" runat="server" DataValueField="TemplStageId" DataTextField="Name" Width="245px"></asp:DropDownList>
                </textarea>
            </dd>
            <dd>
                <textarea id="hdnStageProspect" cols="20" rows="2">
                    <asp:DropDownList ID="ddlStageProspect" runat="server" DataValueField="TemplStageId" DataTextField="Name" Width="245px"></asp:DropDownList>
                </textarea>
            </dd>
            <dd><input id="hdnDisplayAsTemp" type="text" value="<input id='txtDisplayedAs' name='txtDisplayedAs' type='text' maxlength='50' readonly style='width: 165px;' />" /></dd>
            <dd><input id="hdnDaysFromEstCloseDateTemp" type="text" value="<input id='txtDaysFromEstCloseDate' name='txtDaysFromEstCloseDate' type='text' maxlength='4' style='width: 60px;' />" /></dd>
            <dd><input id="hdnDaysAfterCreationDateTemp" type="text" value="<input id='txtDaysAfterCreationDate' name='txtDaysAfterCreationDate' type='text' maxlength='4' style='width: 60px;' />" /></dd>
        </dl>
    </div>
    <div id="divHiddenFields" style="display: none;">
        <input id="hdnCounter" runat="server" type="hidden" value="0" />
        <asp:TextBox ID="hdnSequences" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnStageTemplateIDs" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnStageNames" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnEnableds" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnDaysFromEstCloseDates" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnDaysAfterCreationDates" runat="server"></asp:TextBox>
        <asp:DropDownList ID="ddlStageTemplateList" runat="server" DataValueField="TemplStageId" DataTextField="Alias"></asp:DropDownList>
        <asp:TextBox ID="hdnRemovedStageIDs" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnWflStageIDs" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnIsCustom" runat="server"></asp:TextBox>
    </div>
    
    <div id="divStageSetup" title="Workflow Stage Setup" style="display: none;">
        <iframe id="ifrStageSetup" frameborder="0" scrolling="no" width="872px" height="470px"></iframe>
    </div>
     <div id="divAddStage" title="Create a new stage template" style="display: none;">
        <iframe id="ifrStageAdd" frameborder="0" scrolling="no" width="700px" height="360px">
        </iframe>
    </div>
    </form>
</body>
</html>
