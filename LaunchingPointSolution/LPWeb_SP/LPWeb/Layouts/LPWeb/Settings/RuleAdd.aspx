<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleAdd.aspx.cs" Inherits="Settings_RuleAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rule Setup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
	.ui-autocomplete-input { width: 220px; }
	.ui-autocomplete-loading { background: white url('../images/loading.gif') right center no-repeat; }
	.ui-autocomplete {
		max-height: 430px;
		overflow-y: auto;
		/* prevent horizontal scrollbar */
		overflow-x: hidden;
		/* add padding to account for vertical scrollbar */
		padding-right: 20px;
	}
	/* IE 6 doesn't support max-height
	 * we use height instead, but this forces the menu to always be this tall
	 */
	* html .ui-autocomplete {
		height: 430px;
	}
	</style>

    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.progressbar.min.js"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>

 
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.bestupper.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            AddValidators();

            // upper
            $('#txtFormula').bestupper();

            // maxlength
            $("#txtDesc").maxlength(500);

        });

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {
                    txtRuleName: {
                        required: true
                    }
                },
                messages: {
                    txtRuleName: {
                        required: "<div>Please enter Rule Name.</div>"
                    }
                }
            });
        }

        //#region Add/Remove Conditions

        function aAdd_onclick() {

            var TrCount = $("#divConditionList tr").length;

            if (TrCount == 21) {

                alert("A rule cannot have more than 20 conditions.");
                return;
            }

            // add th
            if (TrCount == 1) {

                // clear tr
                $("#gridConditionList").empty();

                // add th
                $("#gridConditionList").append($("#gridConditionList1 tr").eq(0).clone());
            }

            //#region Add Tr

            // next index
            var NowIndex = new Number($("#hdnCounter").val());
            var NextIndex = NowIndex + 1;

            // clone tr
            var TrCopy = $("#gridConditionList1 tr").eq(1).clone(true);

            // ddlSeq
            TrCopy.find("#ddlSeq").val(TrCount);

            // txtPointField
            var txtPointField_NewID = "txtPointField" + NextIndex;
            var hdnPointField_NewID = "hdnPointField" + NextIndex;
            var txtPointField_Code = "<input id='" + txtPointField_NewID + "' name='" + txtPointField_NewID + "' type='text' class='txtPointField' style='width:280px;' />"
                                   + "<input id='" + hdnPointField_NewID + "' type='hidden' class='hdnPointField' />";
            // replace
            TrCopy.find("#txtPointField").replaceWith(txtPointField_Code);

            // add tr
            $("#gridConditionList").append(TrCopy);

            // set counter
            $("#hdnCounter").val(NextIndex);

            // add validate
            $("#" + txtPointField_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please select Point Field.</div>"
                }
            });

            // add auto complete
            AddAutoComplete(txtPointField_NewID, hdnPointField_NewID);

            //#endregion
        }

        function aRemove_onclick() {

            var SelectedCount = $("#gridConditionList tr td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No condition was selected.");
                return;
            }

            var Result = confirm("This operation may affect the existing rules that are in effect and is not reversible. Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // clear formula
            var Formular = $.trim($("#txtFormula").val());
            $("#gridConditionList tr:not(:first) td :checkbox:checked").parent().parent().find(":input[id='ddlSeq']").each(function () {

                var Seq = $(this).val();
                var Pos = Formular.indexOf(Seq.toString());
                if (Pos != -1) {

                    $("#txtFormula").val("");
                }
            });

            // remove row
            if ($("#gridConditionList tr:not(:first) td :checkbox:checked").length == $("#gridConditionList tr:not(:first)").length) {

                $("#gridConditionList").empty();
                $("#gridConditionList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no condition, please add.</td></tr>");
            }
            else {

                $("#gridConditionList tr:not(:first) td :checkbox:checked").parent().parent().remove();
            }

            // reset sequence
            ResetSequence();
        }

        function AddAutoComplete(txtID, hdnID) {

            $("#" + txtID).autocomplete({

                source: "GetPointField_Background.aspx",
                minLength: 2,
                search: function (event, ui) {

                    // clear point field id
                    $("#" + hdnID).val("");
                },
                select: function (event, ui) {

                    // set point filed id
                    $("#" + hdnID).val(ui.item.id);

                    //                    alert(ui.item.datatype);

                    //#region add/remove Condition options

                    $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").attr("disabled", "");
                    $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").attr("title", "");
                    if (ui.item.datatype == "1") {  // string

                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").empty();
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='1'>equals</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='2'>not equal to</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='10'>contains</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='11'>does not contain</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='12'>starts with</option>");
                    }
                    else if (ui.item.datatype == "2") { // numeric

                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").empty();
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='1'>equals</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='2'>not equal to</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='3'>less than</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='4'>greater than</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='5'>less or equal</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='6'>greater or equal</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='7'>increases by more than</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='8'>decreases by more than</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='9'>changes by more than</option>");
                    }
                    else if (ui.item.datatype == "3") { // boolean

                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").empty();
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='1'>equals</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='2'>not equal to</option>");
                    }
                    else if (ui.item.datatype == "4") { // date

                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").empty();
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='13'>Today</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='14'>Yesterday</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='15'>Within last x days</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='16'>Within next x days</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='17'>Older than x days</option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").append("<option value='18'>After x days from now</option>");


                        $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").change(function () {
                            var sConditionVal = $(this).children('option:selected').val();  //弹出select的值
                            if (sConditionVal == "13" || sConditionVal == "14") {
                                $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").removeAttr("value");
                                $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").attr("disabled", "true");
                                $("#" + txtID).parent().parent().find(":input[class='txtTolerance valid']").removeAttr("value");
                                $("#" + txtID).parent().parent().find(":input[class='txtTolerance valid']").attr("disabled", "true");

                                $("#" + txtID).parent().parent().find(":input[id='ddlType']").empty();
                                $("#" + txtID).parent().parent().find(":input[id='ddlType']").append("<option value=''></option>");
                                $("#" + txtID).parent().parent().find(":input[id='ddlType']").attr("disabled", "true");


                            }
                            else {

                                $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").attr("disabled", "");
                                $("#" + txtID).parent().parent().find(":input[class='txtTolerance valid']").attr("disabled", "");

                                $("#" + txtID).parent().parent().find(":input[id='ddlType']").empty();
                                $("#" + txtID).parent().parent().find(":input[id='ddlType']").append("<option value='Days'>Days</option>");
                                $("#" + txtID).parent().parent().find(":input[id='ddlType']").attr("disabled", "true");
                            }
                        });


                    }

                    //#endregion

                    //                    alert("Value=" + $("#" + txtID).parent().parent().find(":input[id='ddlCondition']").val());

                    //#region replace Tolerance

                    // next index
                    var NowIndex = new Number($("#hdnCounter").val());
                    var NextIndex = NowIndex + 1;

                    if (ui.item.datatype == "3") { // boolean

                        var ddlTolerance_NewID = "ddlTolerance" + NextIndex;
                        var ddlTolerance_Code = "<select id='" + ddlTolerance_NewID + "' class='txtTolerance' style='width: 128px;'>"
                                            + "<option value='Yes'>Yes</option>"
                                            + "<option value='No'>No</option>"
                                            + "</select>";
                        // replace
                        $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").replaceWith(ddlTolerance_Code);
                    }
                    else {

                        var txtTolerance_NewID = "txtTolerance" + NextIndex;
                        var txtTolerance_Code = "";
                        if (ui.item.datatype == "1") {  // string

                            txtTolerance_Code = "<input id='" + txtTolerance_NewID + "' name='" + txtTolerance_NewID + "' type='text' maxlength='64' class='txtTolerance' />";
                            // replace
                            $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").replaceWith(txtTolerance_Code);
                        }
                        else if (ui.item.datatype == "2") { // numeric 

                            txtTolerance_Code = "<input id='" + txtTolerance_NewID + "' name='" + txtTolerance_NewID + "' type='text' maxlength='8' class='txtTolerance' />";

                            // replace
                            $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").replaceWith(txtTolerance_Code);

                            // only number
                            $("#" + txtTolerance_NewID).onlypressnum();

                        }
                        else if (ui.item.datatype == "4") { // Date

                            txtTolerance_Code = "<input id='" + txtTolerance_NewID + "' name='" + txtTolerance_NewID + "' type='text' maxlength='8' class='txtTolerance' />";

                            // replace
                            $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").replaceWith(txtTolerance_Code);

                            // only number
                            $("#" + txtTolerance_NewID).onlypressnum();

                            $("#" + txtID).parent().parent().find(":input[class='txtTolerance']").attr("disabled", "true");
                        }

                        // add validate
                        $("#" + txtTolerance_NewID).rules("add", {
                            required: true,
                            messages: {
                                required: "<div>Please enter Tolerance.</div>"
                            }
                        });
                    }

                    // set counter
                    $("#hdnCounter").val(NextIndex);

                    //#endregion

                    //#region enable/disable Tolerance Type

                    $("#" + txtID).parent().parent().find(":input[id='ddlType']").attr("title", "");
                    if (ui.item.datatype == "1") {  // string

                        $("#" + txtID).parent().parent().find(":input[id='ddlType']").attr("disabled", "true");
                    }
                    else if (ui.item.datatype == "2") { // numeric

                        $("#" + txtID).parent().parent().find(":input[id='ddlType']").attr("disabled", "");
                    }
                    else if (ui.item.datatype == "3") { // boolean

                        $("#" + txtID).parent().parent().find(":input[id='ddlType']").attr("disabled", "true");
                    }
                    else if (ui.item.datatype == "4") { // boolean

                        $("#" + txtID).parent().parent().find(":input[id='ddlType']").empty();
                        $("#" + txtID).parent().parent().find(":input[id='ddlType']").append("<option value=''></option>");
                        $("#" + txtID).parent().parent().find(":input[id='ddlType']").attr("disabled", "true");

                    }

                    //#endregion
                }
            });

            $("#" + txtID).blur(function () {

                // if non-standard input, clear
                if ($("#" + hdnID).val() == "") {

                    $("#" + txtID).val("");
                }
            });
        }

        function ResetSequence() {

            for (var i = 1; i < $("#gridConditionList tr").length; i++) {

                $("#gridConditionList tr").eq(i).find(":input[id='ddlSeq']").val(i);
            }
        }

        //#endregion

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#divConditionList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#divConditionList tr td :checkbox").attr("checked", "");
            }
        }

        function BeforeSave() {

            // call validate
            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return false;
            }

            // no condition
            if ($("#gridConditionList tr").length == 1) {

                alert("Please add condition(s).");
                return false;
            }

            //#region validate sequence duplicate

            $("#gridConditionList tr td :input[id='ddlSeq']").each(function (i) {

                var ThisSeq = $(this).val();

                for (var j = i + 1; j < $("#divConditionList tr td :input[id='ddlSeq']").length; j++) {

                    var OtherSeq = $("#divConditionList tr td :input[id='ddlSeq']").eq(j).val();

                    if (ThisSeq == OtherSeq) {

                        alert("The sequence in condition list can not be duplicated.");
                        return false;
                    }
                }
            });

            //#endregion

            var f = $.trim($("#txtFormula").val());
            if (f != "") {

                //#region validate formula

                var f = $.trim($("#txtFormula").val());

                // valid formula
//                var regex2 = new RegExp($("#hdnFormulaRegex").val());
//                if (regex2.test(f) == false) {

//                    alert("The Advanced Formula is invalid.");
//                    return false;
//                }

                var x = f.split('');
                var SeqArray = new Array();
                var regex = new RegExp("^[1-9][0-9]?$");

                var j = 0;
                for (var i = 0; i < x.length; i++) {

                    var Is1to5 = regex.test(x[i]);
                    if (Is1to5 == true) {

                        SeqArray[j] = x[i];
                        j++;
                    }
                }

                var ConditionSeqs = "";
                for (var m = 0; m < SeqArray.length; m++) {

                    var Seq = SeqArray[m];

                    if ($("#divConditionList tr td :input[id='ddlSeq'][value='" + Seq + "']").length == 0) {

                        if (ConditionSeqs == "") {

                            ConditionSeqs = Seq;
                        }
                        else {

                            ConditionSeqs += ", " + Seq;
                        }
                    }
                }

                if (ConditionSeqs != "") {

                    alert("There is no condition(Sequence=" + ConditionSeqs + ") in condition list referenced by Advanced Formula.");
                    return false;
                }

                //#endregion
            }

            //#region build condition list data string

            //#region Sequences

            var Sequences = "";
            $("#gridConditionList tr td :input[id='ddlSeq']").each(function (i) {

                var ThisSeq = $(this).val();

                if (Sequences == "") {

                    Sequences = ThisSeq
                }
                else {

                    Sequences += "," + ThisSeq;
                }
            });

            $("#hdnSequences").val(Sequences);

            //#endregion

            //#region Point Field IDs

            var PointFieldIDs = "";
            $("#gridConditionList tr td :input[class='hdnPointField']").each(function (i) {

                var PointFieldID = $(this).val();

                if (PointFieldID != "") {

                    if (PointFieldIDs == "") {

                        PointFieldIDs = PointFieldID
                    }
                    else {

                        PointFieldIDs += "," + PointFieldID;
                    }
                }
            });

            $("#hdnPointFieldIDs").val(PointFieldIDs);

            //#endregion

            //#region Conditions

            var Conditions = "";
            $("#gridConditionList tr td :input[id='ddlCondition']").each(function (i) {

                var ConditionID = $(this).val();

                if (Conditions == "") {

                    Conditions = ConditionID
                }
                else {

                    Conditions += "," + ConditionID;
                }
            });

            $("#hdnConditions").val(Conditions);

            //#endregion

            //#region Tolerances

            var Tolerances = "";
            $("#gridConditionList tr td :input[class*='txtTolerance']").each(function (i) {

                var Tol = $(this).val();

                if (Tolerances == "") {

                    Tolerances = "[$" + Tol + "$]";
                }
                else {

                    Tolerances += ",[$" + Tol + "$]";
                }
            });

            $("#hdnTolerances").val(Tolerances);

            //#endregion

            //#region Tolerance Types

            var ToleranceTypes = "";
            $("#gridConditionList tr td :input[id='ddlType']").each(function (i) {

                var disabled = $(this).attr("disabled");

                var Type = "";
                if (disabled == true) {

                    if ($(this).val() == "Days") {
                        Type = $(this).val();
                    }
                    else {
                        Type = "#null#";
                    }
                }
                else {

                    Type = $(this).val();
                    if (Type == "") {

                        Type = "#empty#";
                    }
                }

                if (ToleranceTypes == "") {

                    ToleranceTypes = Type
                }
                else {

                    ToleranceTypes += "," + Type;
                }
            });

            $("#hdnToleranceTypes").val(ToleranceTypes);

            //#endregion

            //#endregion

            return true;
        }

        //#region Preview

        function btnPreviewRecom_onclick() {

            var EmailTemplateID = $("#ddlRecomActionTemplate").val();
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }

            OpenWindow("EmailTemplatePreview.aspx?id=" + EmailTemplateID, "_PreviewEmailTemplate3", 760, 700, "no", "center");
            return true;
        }

        function btnPreviewAlert_onclick() {

            var EmailTemplateID = $("#ddlAlertEmailTemplate").val();
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }

            // under ../Settings/
            OpenWindow("EmailTemplatePreview.aspx?id=" + EmailTemplateID, "_PreviewEmailTemplate4", 760, 700, "no", "center");
            return true;
        }

        //#endregion

        //#region add by peter 20110106
        function addSuccessfully(sNewId) {
            $('#divContainer').hide();
            alert('Create rule successfully.');
            if ('<%=strNeedReturn %>' === "1" && typeof (window.parent.newRuleAdded) === "function") {
                window.parent.newRuleAdded(sNewId);
            }
            else
                window.parent.location.href = window.parent.location.href;
        }
        //#endregion

        function Cancel() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            if (CloseDialogCodes == "") {

                window.parent.CloseDialog_AddRule();
            }
            else {

                eval(CloseDialogCodes);
            }
        }

        function ConditionChange(ddlCondition,tbTolerance, ddlType) {

            Alert("ddlType=" + ddlType.val());
//            if (ddlCondition.val() == "18") {
//                Alert("ddlType=" + ddlType.val());
//            }
        }

        function ConditionChange1() {

            Alert("ddlType=" + ddlType.val());
            //            if (ddlCondition.val() == "18") {
            //                Alert("ddlType=" + ddlType.val());
            //            }
        }


        function AddMarketing() {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var f = document.getElementById('ifrMarketingAdd');
            f.src = "../Marketing/SelectMarketingCampaignForRule.aspx?type=1&t=" + RadomStr;
            $('#divAddMarketing').dialog({
                height: 560,
                width: 640,
                modal: true,
                resizable: false
            });
        }

        function getCampaignSelectionReturn(result) {

            var SelID = result.substring(0, result.indexOf("^"));
            var sSelText = result.substring(result.indexOf("^")+1 , result.length);
            $("#<%=hfSelCampaigns.ClientID %>").val(SelID);

            $("#ifrMarketingAdd").dialog("destroy");
            if ($("#<%=hfSelCampaigns.ClientID %>").val() != "") {
                $("#divAddMarketing").dialog("close");
            }
        }
     

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div id="divAddMarketing" title="Select Marketing Campaign" style="display: none;">
        <iframe id="ifrMarketingAdd" frameborder="0" scrolling="no" width="580px" height="500px">
        </iframe>
    </div>
    <div id="divContainer" style="width: 805px; height: 690px; margin-top: 5px; border: solid 0px red; overflow: auto;" >
        <div id="divRuleDetails">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 130px; vertical-align: top; padding-top: 2px;">Rule Name:</td>
                    <td style="width: 350px;">
                        <asp:TextBox ID="txtRuleName" runat="server" width="300px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:CheckBox ID="chkEnabled" runat="server" Text=" Enabled" Checked="true" Enabled="false" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
                <tr>
                    <td style="width: 130px;">Description:</td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Width="650px" Height="50px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
                <tr>
                    <td style="width: 130px;">Scope:</td>
                    <td style="width: 305px;">
                        <asp:DropDownList ID="ddlScope" runat="server" Width="100px">
                            <asp:ListItem Value="0">Loan</asp:ListItem>
                            <asp:ListItem Value="1">Company</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 45px;">Target:</td>
                    <td>
                        <asp:DropDownList ID="ddlTarget" runat="server">
                            <asp:ListItem Value="0">Processing</asp:ListItem>
                            <asp:ListItem Value="1">Prospect</asp:ListItem>
                            <asp:ListItem Value="2">Processing and Prospect</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" style="margin-top: 15px;">
                <tr>
                    <td style="width: 130px;">Recommended Action <br />Template:</td>
                    <td style="vertical-align: top;">
                        <asp:DropDownList ID="ddlRecomActionTemplate" runat="server" Width="300px" DataValueField="TemplEmailId" DataTextField="Name">
                        </asp:DropDownList>
                    </td>
                    <td style="vertical-align: top; padding-left: 10px;">
                        <input id="btnPreviewRecom" type="button" value="Preview" class="Btn-66" onclick="btnPreviewRecom_onclick()" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" style="margin-top: 5px;">
                <tr>
                    <td style="width: 130px;">Alert Email Template:</td>
                    <td>
                        <asp:DropDownList ID="ddlAlertEmailTemplate" runat="server" Width="300px" DataValueField="TemplEmailId" DataTextField="Name">
                        </asp:DropDownList>
                    </td>
                    <td style=" padding-left: 10px;">
                        <input id="btnPreviewAlert" type="button" value="Preview" class="Btn-66" onclick="btnPreviewAlert_onclick()" />
                    </td>
                    <td style="padding-left: 30px;">
                        <asp:CheckBox ID="chkReqAck" runat="server" Text=" Requires Acknowledgement" />
                    </td>
                </tr>
            </table>
<%--              <table cellpadding="0" cellspacing="0" style="margin-top: 15px;">
                <tr>
                    <td style="width: 130px;">Marketing Campaign: </td>
                    <td style="vertical-align: top;">
                        <asp:TextBox ID="txtMarketingCampaign" runat="server" Width="295px" ReadOnly="true" ></asp:TextBox>
                    </td>
                    <td style="vertical-align: top; padding-left: 10px;">
                        <input id="btnShowMarketingCamp" type="button" value="Select" class="Btn-66"  onclick="javascript:AddMarketing()" />
                    </td>
                </tr>
            </table>--%>
            
        </div>
        <div id="divCondition" style="margin-top: 25px;">
            <div id="divToolBar">
                <ul class="ToolStrip" style="margin-left: 0px;">
                    <li><a id="aAdd" href="javascript:aAdd_onclick()">Add Condition</a><span>|</span></li>
                    <li><a id="aRemove" href="javascript:aRemove_onclick()">Remove Condition</a></li>
                </ul>
            </div>
            <div id="divConditionList" class="ColorGrid" style="margin-top: 5px;">
                <div>
                    <table id="gridConditionList" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                        <tr class="EmptyDataRow" align="center">
                            <td colspan="2">
                                There is no condition, please add.
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
            <div id="divConditionList1" class="ColorGrid" style="margin-top: 5px; display: none;">
                <div>
                    <table id="gridConditionList1" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                        <tr>
	                        <th class="CheckBoxHeader" scope="col"><input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" /></th>
	                        <th scope="col">Sequence</th>
	                        <th scope="col">Point Field</th>
	                        <th scope="col">Condition</th>
	                        <th scope="col">Tolerance</th>
	                        <th scope="col">Type</th>
                        </tr>
                        <tr>
	                        <td class="CheckBoxColumn">
                                <input id="chkSelected" type="checkbox" />
                            </td>
	                        <td style="width: 55px;">
                                <select id="ddlSeq" disabled>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                    <option value="13">13</option>
                                    <option value="14">14</option>
                                    <option value="15">15</option>
                                    <option value="16">16</option>
                                    <option value="17">17</option>
                                    <option value="18">18</option>
                                    <option value="19">19</option>
                                    <option value="20">20</option>
                                </select>
                            </td>
                            <td>
                                <input id="txtPointField" type="text" style="width:280px;" />
                            </td>
	                        <td style="width: 170px;">
                                <select id="ddlCondition" title="Please select Point Field first." style="width: 150px;" disabled>
                                    <option value="1">equals</option>
                                    <option value="2">not equal to</option>
                                    <option value="3">less than</option>
                                    <option value="4">greater than</option>
                                    <option value="5">less or equal</option>
                                    <option value="6">greater or equal</option>
                                    <option value="7">increases by more than</option>
                                    <option value="8">decreases by more than</option>
                                    <option value="9">changes by more than</option>
                                    <option value="10">contains</option>
                                    <option value="11">does not contain</option>
                                    <option value="12">starts with</option>
                                </select>
                            </td>
	                        <td style="width: 150px;">
                                <input id="txtTolerance" type="text" class="txtTolerance" title="Please select Point Field first." disabled />
                            </td>
	                        <td style="width: 50px;">
                                <select id="ddlType" title="Please select Point Field first." disabled>
                                    <option value=""></option>
                                    <option value="$">$</option>
                                    <option value="%">%</option>
                                </select>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
            <div id="divFormula" style="margin-top: 15px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 130px;">Advanced Formula:</td>
                        <td>
                            <asp:TextBox ID="txtFormula" runat="server" Width="300px" MaxLength="200" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divButtons" style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave();" onclick="btnSave_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="Cancel();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divHiddenFields" style="display: none;">
        <input id="hdnCounter" type="hidden" value="0" />
        <asp:TextBox ID="hdnSequences" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnPointFieldIDs" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnConditions" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnTolerances" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnToleranceTypes" runat="server"></asp:TextBox>
         <asp:HiddenField ID="hfSelCampaigns" runat="server" />
        <input id="hdnFormulaRegex" type="text" value="^(\(*[1-5])*(\s*(and|or|not|AND|OR|NOT)\s*\(*[1-5]\)*)*$" />
    </div>
    </form>
</body>
</html>
