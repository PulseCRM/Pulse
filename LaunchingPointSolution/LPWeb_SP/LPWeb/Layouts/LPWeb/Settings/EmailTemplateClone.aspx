<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailTemplateClone.aspx.cs" Inherits="Settings_EmailTemplateClone" ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Email Template Setup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
	.ui-autocomplete-input { width: 220px; }
	.ui-autocomplete-loading { background: white url('../images/loading.gif') right center no-repeat; }
	.ui-icon-triangle-1-sx { background-position: -64px -20px; }
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
    
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ScriptManager1" runat="server" EnableTheming="True">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>

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
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.cleditor.js" type="text/javascript"></script>
    <script src="../js/jquery.autoCompleteCombox.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/ZeroClipboard.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        var rad_editor = null;

        var editor_selection = null

        function OnClientLoad(editor, args) {

            rad_editor = editor;

            editor.attachEventHandler("click", function (e) {

                var x = editor.getSelection().getRange();
                if (x.duplicate) { // ie

                    editor_selection = x.duplicate();
                }
                else {

                    editor_selection = x.cloneRange();
                }
            });

            editor.attachEventHandler("keyup", function (e) {

                var x = editor.getSelection().getRange();
                if (x.duplicate) { // ie

                    editor_selection = x.duplicate();
                }
                else {

                    editor_selection = x.cloneRange();
                }
            });
        }

        $(document).ready(function () {

            // init tab
            $("#tabs").tabs();

            // set attach iframe src
            var src = "EmailTemplateAttachmentList.aspx?EmailTemplateID=0";
            $("#ifrAttachments").attr("src", src);

            // init auto complete
            InitAutoComplete();

            // maxlength
            $("#txtDesc").maxlength(500);

            AddValidators();

            // add events
            $("#ddlFromUserRoles").change(ddlFromUserRoles_onchange);
            $("#txtFromEmail").blur(txtFromEmail_onblur);
            ddlFromUserRoles_onchange();
            txtFromEmail_onblur();

            //#region ZeroClipboard
            var zeroClipboardClient = new ZeroClipboard($("#btnPreviousValueOfPointField_Copy, #btnCurrentValueOfPointField_Copy, #btnValueOfPulseField_Copy"));

            zeroClipboardClient.on("load", function (client) {
                //alert("ZeroClipboard's flash movie loaded and ready.");

                client.on("complete", function (client, args) {
                    if (args.text == "") {
                        alert("Please select a field.");
                    }
                });
            });

            zeroClipboardClient.on("noFlash", function (client) {
                zeroClipboardClient_ReportProblem(false, client);
            });

            zeroClipboardClient.on("wrongFlash", function (client, args) {
                zeroClipboardClient_ReportProblem(true, client, args);
            });

            function zeroClipboardClient_ReportProblem(isFlashFound, client, args) {
                //if (!isFlashFound) {
                //    alert("Flash not found or is disabled.");
                //} else {
                //    alert("Flash 10.0.0+ is required but found running Flash " + args.flashVersion.replace(/,/g, ".") + ".");
                //}

                $("#btnPreviousValueOfPointField_Copy, #btnCurrentValueOfPointField_Copy, #btnValueOfPulseField_Copy").hide();
                $("#divPreviousValueOfPointField, #divCurrentValueOfPointField, #divValueOfPulseField").show();
            }
            //#endregion

            // is post back
            var IsPostBack = $("#hdnIsPostBack").val();
            if (IsPostBack == "True") {

                InitToList();
                InitCCList();
            }
        });



        function InitAutoComplete() {

            //#region previous point field
            $("#txtPrevPointField").autocomplete({
                source: "GetPointField_Background.aspx",
                minLength: 2,
                search: function (event, ui) {
                    $("#hdnSelPrevPointFieldID").val("");
                },
                select: function (event, ui) {
                    var fieldLabel = ui.item.value;

                    $("#hdnSelPrevPointFieldID").val(ui.item.id);

                    if (fieldLabel != "") {
                        $("#txtPreviousValueOfPointField").val("<@Previous-" + fieldLabel + "@>");
                    }
                }
            });

            $("#txtPrevPointField").blur(function () {
                if ($("#hdnSelPrevPointFieldID").val() == "") {
                    $("#txtPrevPointField").val("");
                    $("#txtPreviousValueOfPointField").val("");
                } else if ($("#txtPrevPointField").val() == "") {
                    $("#hdnSelPrevPointFieldID").val("");
                    $("#txtPreviousValueOfPointField").val("");
                }
            });
            //#endregion

            //#region current point field
            $("#txtCurrentPointField").autocomplete({
                source: "GetPointField_Background.aspx",
                minLength: 2,
                search: function (event, ui) {
                    $("#hdnSelCurrentPointFieldID").val("");
                },
                select: function (event, ui) {
                    var fieldLabel = ui.item.value;

                    $("#hdnSelCurrentPointFieldID").val(ui.item.id);

                    if (fieldLabel != "") {
                        $("#txtCurrentValueOfPointField").val("<@" + fieldLabel + "@>");
                    }
                }
            });

            $("#txtCurrentPointField").blur(function () {
                if ($("#hdnSelCurrentPointFieldID").val() == "") {
                    $("#txtCurrentPointField").val("");
                    $("#txtCurrentValueOfPointField").val("");
                } else if ($("#txtCurrentPointField").val() == "") {
                    $("#hdnSelCurrentPointFieldID").val("");
                    $("#txtCurrentValueOfPointField").val("");
                }
            });
            //#endregion

            //#region pulse database field
            $("#ddlInfoHubFields").combobox({
                'selected': function (selectedIndex, selectedVale) {
                    var fieldLabel = selectedVale.item.value;

                    $("#txtValueOfPulseField").val((fieldLabel != "0" && fieldLabel != "-- select a field --" ? "<@DB-" + fieldLabel + "@>" : ""));
                }
            });

            $("input[role='textbox'][id^='ddlInfoHubFields_']").data('invalidValueHandler', function (event) {
                if ($(this).val() == "" || $(this).val() == "-- select a field --") {
                    $("#txtValueOfPulseField").val("");
                }
            });
            //#endregion

            $("#toggle").click(function () {
                $("#ddlInfoHubFields").toggle();
            });
        }

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {
                    txtEmailTemplateName: {
                        required: true
                    },
                    txtSubject: {
                        required: true
                    },
                    txtFromEmail: {
                        email: true
                    }
                },
                messages: {
                    txtEmailTemplateName: {
                        required: "*"
                    },
                    txtSubject: {
                        required: "*"
                    },
                    txtFromEmail: {
                        email: "<div>Please enter a valid email address.</div>"
                    }
                }
            });
        }

        function ddlFromUserRoles_onchange() {

            var RoleID = $("#ddlFromUserRoles").val();
            if (RoleID == "0") {

                $("#txtFromEmail").attr("readonly", "");
            }
            else {

                $("#txtFromEmail").attr("readonly", "true");
            }
        }

        function txtFromEmail_onblur() {

            var UserDefinedEmail = $("#txtFromEmail").val();
            if (UserDefinedEmail == "") {

                $("#ddlFromUserRoles").attr("disabled", "");

                $("#txtSenderName").val("");
                $("#txtSenderName").attr("disabled", "true");
            }
            else {

                $("#ddlFromUserRoles").attr("disabled", "true");

                $("#txtSenderName").attr("disabled", "");
            }
        }

        function BeforeSave() {

            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                $("#tabs").tabs("select", 0);
                return false;
            }

            // check from
            var FromUserRole = $("#ddlFromUserRoles").val();
            var UserDefinedEmail = $("#txtFromEmail").val();
            if (FromUserRole == "0" && UserDefinedEmail == "") {

                alert("Please select a From role or enter a User-defined email.");
                $("#tabs").tabs("select", 0);
                return false;
            }

            // check to
            if ($("#gridToList tr td :checkbox").length == 0) {

                alert("You must specify To recipient(s).");
                $("#tabs").tabs("select", 0);
                return false;
            }

            return true;
        }

        // show popup for select recipient
        function ShowDialog_SelectRecipient_To() {

            // clear
            $("#txtEmails").val("")
            $("#gridRecipientRoleList tr td :checkbox:checked").attr("checked", "");

            $("#txtEmails").val($("#hdnToEmailList").val());

            //#region init Task Owner checkbox

            if ($("#hdnToTaskOwnerChecked").val() == "False") {

                $("#chkTaskOwner").attr("checked", "");
            }
            else {

                $("#chkTaskOwner").attr("checked", "true");
            }

            //#endregion

            // check exists
            var ExistEmailList = "";
            $("#gridToList tr td :checkbox").each(function () {

                var RoleType = $(this).attr("myRoleType");
                var RoleID = $(this).attr("myRoleID");
                var RoleName = $(this).attr("myRoleName");

                if (RoleType == "Contact") {

                    $("#gridRecipientRoleList tr td :checkbox[myRoleType='Contact'][myRoleID='" + RoleID + "']").attr("checked", "true");
                }
                else if (RoleType == "User") {

                    $("#gridRecipientRoleList tr td :checkbox[myRoleType='User'][myRoleID='" + RoleID + "']").attr("checked", "true");
                }
            });

            // set flag
            $("#hdnFromWho").val("To");

            // show modal
            $("#divEmailRecipientSelection").dialog({
                height: 580,
                width: 560,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // show popup for select recipient
        function ShowDialog_SelectRecipient_CC() {

            // clear
            $("#txtEmails").val("")
            $("#gridRecipientRoleList tr td :checkbox:checked").attr("checked", "");

            $("#txtEmails").val($("#hdnCCEmailList").val());

            //#region init Task Owner checkbox

            if ($("#hdnCCTaskOwnerChecked").val() == "False") {

                $("#chkTaskOwner").attr("checked", "");
            }
            else {

                $("#chkTaskOwner").attr("checked", "true");
            }

            //#endregion

            // check exists
            var ExistEmailList = "";
            $("#gridCCList tr td :checkbox").each(function () {

                var RoleType = $(this).attr("myRoleType");
                var RoleID = $(this).attr("myRoleID");
                var RoleName = $(this).attr("myRoleName");

                if (RoleType == "Contact") {

                    $("#gridRecipientRoleList tr td :checkbox[myRoleType='Contact'][myRoleID='" + RoleID + "']").attr("checked", "true");
                }
                else if (RoleType == "User") {

                    $("#gridRecipientRoleList tr td :checkbox[myRoleType='User'][myRoleID='" + RoleID + "']").attr("checked", "true");
                }
            });

            // set flag
            $("#hdnFromWho").val("CC");

            // show modal
            $("#divEmailRecipientSelection").dialog({
                height: 580,
                width: 560,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_SelectRecipient() {

            $("#divEmailRecipientSelection").dialog("close");
        }

        function chkUserDefinedEmail_click(CheckBox) {

            if (CheckBox.checked) {

                $("#txtEmails").attr("readonly", "");
            }
            else {

                $("#txtEmails").val("");
                $("#txtEmails").attr("readonly", "true");
            }
        }

        // check/decheck all
        function CheckAll_RecipientRoleList(CheckBox) {

            if (CheckBox.checked) {

                $("#gridRecipientRoleList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridRecipientRoleList tr td :checkbox").attr("checked", "");
            }
        }

        // check/decheck all
        function CheckAll_ToList(CheckBox) {

            if (CheckBox.checked) {

                $("#gridToList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridToList tr td :checkbox").attr("checked", "");
            }
        }

        // check/decheck all
        function CheckAll_CCList(CheckBox) {

            if (CheckBox.checked) {

                $("#gridCCList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridCCList tr td :checkbox").attr("checked", "");
            }
        }

        function btnAdd_click() {

            var FromWho = $("#hdnFromWho").val();

            if (FromWho == "") {

                return;
            }

            if (FromWho == "To") {

                AddEmailRole_To();
            }
            else {

                AddEmailRole_CC();
            }
        }

        //#region To and CC List

        //#region init To/CC List

        function InitToList() {

            // clear tr
            $("#gridToList").empty();

            // add th
            $("#gridToList").append("<tr><th class='CheckBoxHeader' scope='col'><input type='checkbox' onclick='CheckAll_ToList(this)' /></th><th style='width:50px;' scope='col'>Type</th><th scope='col'>Email / Role</th></tr>");

            var EmailListTo = $("#hdnToEmailList").val();
            var ContactListTo = $("#hdnToContactList").val();
            var UserRoleListTo = $("#hdnToUserRoleList").val();

            if (EmailListTo == "" && ContactListTo == "" && UserRoleListTo == "" && $("#hdnCCTaskOwnerChecked").val() == "False") {

                $("#gridToList").empty();
                $("#gridToList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no recipient.</td></tr>");
            }
            else {

                if (EmailListTo != "") {

                    // split
                    var EmailToArray = EmailListTo.split(";");

                    for (var j = 0; j < EmailToArray.length; j++) {

                        var EmailAddr = EmailToArray[j];
                        var Tr = BuildTrString("Email", "", EmailAddr, "");
                        $("#gridToList").append(Tr);
                    }
                }

                if (ContactListTo != "") {

                    // split
                    var ContactToArray = ContactListTo.split(";");

                    for (var j = 0; j < ContactToArray.length; j++) {

                        var ContactIDAndRole = ContactToArray[j];
                        var ContactInfo = ContactIDAndRole.split(":");
                        var ContactRoleID = ContactInfo[0];
                        var ContactRoleName = ContactInfo[1];

                        var Tr = BuildTrString("Contact", ContactRoleID, "", ContactRoleName);
                        $("#gridToList").append(Tr);
                    }
                }

                if (UserRoleListTo != "") {

                    // split
                    var UserRoleToArray = UserRoleListTo.split(";");

                    for (var j = 0; j < UserRoleToArray.length; j++) {

                        var UserRoleIDAndRole = UserRoleToArray[j];
                        var UserRoleInfo = UserRoleIDAndRole.split(":");
                        var UserRoleID = UserRoleInfo[0];
                        var UserRoleName = UserRoleInfo[1];

                        var Tr = BuildTrString("User", UserRoleID, "", UserRoleName);
                        $("#gridToList").append(Tr);
                    }
                }

                if ($("#hdnToTaskOwnerChecked").val() == "True") {

                    var Tr = BuildTrString("User", "0", "", "Task Owner");
                    $("#gridToList").append(Tr);
                }

                // set even
                $("#gridToList tr:not(:first):odd").attr("class", "EvenRow");
                $("#gridToList tr:not(:first):even").removeAttr("class");
            }
        }

        function InitCCList() {

            // clear tr
            $("#gridCCList").empty();

            // add th
            $("#gridCCList").append("<tr><th class='CheckBoxHeader' scope='col'><input type='checkbox' onclick='CheckAll_CCList(this)' /></th><th style='width:50px;' scope='col'>Type</th><th scope='col'>Email / Role</th></tr>");

            var EmailListCC = $("#hdnCCEmailList").val();
            var ContactListCC = $("#hdnCCContactList").val();
            var UserRoleListCC = $("#hdnCCUserRoleList").val();

            if (EmailListCC == "" && ContactListCC == "" && UserRoleListCC == "") {

                $("#gridCCList").empty();
                $("#gridCCList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no recipient.</td></tr>");
            }
            else {

                if (EmailListCC != "") {

                    // split
                    var EmailCCArray = EmailListCC.split(";");

                    for (var j = 0; j < EmailCCArray.length; j++) {

                        var EmailAddr = EmailCCArray[j];
                        var Tr = BuildTrString("Email", "", EmailAddr, "");
                        $("#gridCCList").append(Tr);
                    }
                }

                if (ContactListCC != "") {

                    // split
                    var ContactCCArray = ContactListCC.split(";");

                    for (var j = 0; j < ContactCCArray.length; j++) {

                        var ContactIDAndRole = ContactCCArray[j];
                        var ContactInfo = ContactIDAndRole.split(":");
                        var ContactRoleID = ContactInfo[0];
                        var ContactRoleName = ContactInfo[1];

                        var Tr = BuildTrString("Contact", ContactRoleID, "", ContactRoleName);
                        $("#gridCCList").append(Tr);
                    }
                }

                if (UserRoleListCC != "") {

                    // split
                    var UserRoleCCArray = UserRoleListCC.split(";");

                    for (var j = 0; j < UserRoleCCArray.length; j++) {

                        var UserRoleIDAndRole = UserRoleCCArray[j];
                        var UserRoleInfo = UserRoleIDAndRole.split(":");
                        var UserRoleID = UserRoleInfo[0];
                        var UserRoleName = UserRoleInfo[1];

                        var Tr = BuildTrString("User", UserRoleID, "", UserRoleName);
                        $("#gridCCList").append(Tr);
                    }
                }

                if ($("#hdnCCTaskOwnerChecked").val() == "True") {

                    var Tr = BuildTrString("User", "0", "", "Task Owner");
                    $("#gridCCList").append(Tr);
                }

                // set even
                $("#gridCCList tr:not(:first):odd").attr("class", "EvenRow");
                $("#gridCCList tr:not(:first):even").removeAttr("class");
            }
        }

        //#endregion

        //#region add email roles

        function AddEmailRole_To() {

            // clear
            $("#hdnToEmailList").val("");
            $("#hdnToContactList").val("");
            $("#hdnToUserRoleList").val("");

            // user-defined emails
            var EmailArray;
            if ($("#chkUserDefinedEmail").attr("checked") == true) {

                var EmailListString = $.trim($("#txtEmails").val());
                if (EmailListString != "") {

                    // valid email list
                    var Pattern = /^([a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)(\;[a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)*$/;
                    var IsValid = Pattern.test(EmailListString);
                    if (IsValid == false) {

                        alert("Please enter a valid email with the correct format, for example techsupport@a.com or tech.support@b.com.");
                        return;
                    }

                    // split
                    EmailArray = EmailListString.split(";");

                    // check duplicate
                    for (var k = 0; k < EmailArray.length; k++) {

                        var EmailStd = EmailArray[k];

                        for (var n = k + 1; n < EmailArray.length; n++) {

                            if (EmailStd == EmailArray[n]) {

                                alert("Duplicate To user/contact roles are not allowed.");
                                return;
                            }
                        }
                    }

                    // copy email list
                    $("#hdnToEmailList").val(EmailListString);
                }
            }

            // role list
            var SelectedCount = $("#gridRecipientRoleList tr td :checkbox:checked").length;
            if (EmailArray == undefined
                    && $("#chkTaskOwner").attr("checked") == false
                    && SelectedCount == 0) {

                alert("Please enter a user-defined email,  Task Owner, or select user/contact roles.");
                return;
            }

            // clear tr
            $("#gridToList").empty();

            // add th
            $("#gridToList").append("<tr><th class='CheckBoxHeader' scope='col'><input type='checkbox' onclick='CheckAll_ToList(this)' /></th><th style='width:50px;' scope='col'>Type</th><th scope='col'>Email / Role</th></tr>");

            if (EmailArray != undefined) {

                for (var j = 0; j < EmailArray.length; j++) {

                    var Tr = BuildTrString("Email", "", EmailArray[j], "");
                    $("#gridToList").append(Tr);
                }
            }

            var ContactList = "";
            var UserRoleList = "";
            $("#gridRecipientRoleList tr:not(:first) td :checkbox:checked").each(function (i) {

                var RoleType = $(this).attr("myRoleType");
                var RoleID = $(this).attr("myRoleID");
                var RoleName = $(this).attr("myRoleName");

                var Tr = BuildTrString(RoleType, RoleID, "", RoleName);
                $("#gridToList").append(Tr);

                if (RoleType == "Contact") {

                    if (ContactList == "") {

                        ContactList = RoleID + ":" + RoleName;
                    }
                    else {

                        ContactList += ";" + RoleID + ":" + RoleName;
                    }
                }
                else {

                    if (UserRoleList == "") {

                        UserRoleList = RoleID + ":" + RoleName;
                    }
                    else {

                        UserRoleList += ";" + RoleID + ":" + RoleName;
                    }
                }

            });

            $("#hdnToContactList").val(ContactList);
            $("#hdnToUserRoleList").val(UserRoleList);

            //#region set task owner checked

            if ($("#chkTaskOwner").attr("checked") == true) {

                $("#hdnToTaskOwnerChecked").val("True");

                var Tr = BuildTrString("User", "0", "", "Task Owner");
                $("#gridToList").append(Tr);
            }
            else {

                $("#hdnToTaskOwnerChecked").val("False");
            }

            //#endregion

            // set even
            $("#gridToList tr:not(:first):odd").attr("class", "EvenRow");
            $("#gridToList tr:not(:first):even").removeAttr("class");

            CloseDialog_SelectRecipient();
        }

        function AddEmailRole_CC() {

            // clear
            $("#hdnCCEmailList").val("");
            $("#hdnCCContactList").val("");
            $("#hdnCCUserRoleList").val("");

            // user-defined emails
            var EmailArray;
            if ($("#chkUserDefinedEmail").attr("checked") == true) {

                var EmailListString = $.trim($("#txtEmails").val());
                if (EmailListString != "") {

                    // valid email list
                    var Pattern = /^([a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)(\;[a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)*$/;
                    var IsValid = Pattern.test(EmailListString);
                    if (IsValid == false) {

                        alert("Please enter a valid email with the correct format, for example techsupport@a.com or tech.support@b.com.");
                        return;
                    }

                    // split
                    EmailArray = EmailListString.split(";");

                    // check duplicate
                    for (var k = 0; k < EmailArray.length; k++) {

                        var EmailStd = EmailArray[k];

                        for (var n = k + 1; n < EmailArray.length; n++) {

                            if (EmailStd == EmailArray[n]) {

                                alert("Duplicate CC user/contact roles are not allowed.");
                                return;
                            }
                        }
                    }

                    // copy email list
                    $("#hdnCCEmailList").val(EmailListString);
                }
            }

            // role list
            var SelectedCount = $("#gridRecipientRoleList tr td :checkbox:checked").length;
            if (EmailArray == undefined
                    && $("#chkTaskOwner").attr("checked") == false
                    && SelectedCount == 0) {

                alert("Please enter a user-defined email, or select user/contact roles.");
                return;
            }

            // clear tr
            $("#gridCCList").empty();

            // add th
            $("#gridCCList").append("<tr><th class='CheckBoxHeader' scope='col'><input type='checkbox' onclick='CheckAll_CCList(this)' /></th><th style='width:50px;' scope='col'>Type</th><th scope='col'>Email / Role</th></tr>");

            if (EmailArray != undefined) {

                for (var j = 0; j < EmailArray.length; j++) {

                    var Tr = BuildTrString("Email", "", EmailArray[j], "");
                    $("#gridCCList").append(Tr);
                }
            }

            var ContactList = "";
            var UserRoleList = "";
            $("#gridRecipientRoleList tr:not(:first) td :checkbox:checked").each(function (i) {

                var RoleType = $(this).attr("myRoleType");
                var RoleID = $(this).attr("myRoleID");
                var RoleName = $(this).attr("myRoleName");

                var Tr = BuildTrString(RoleType, RoleID, "", RoleName);
                $("#gridCCList").append(Tr);

                if (RoleType == "Contact") {

                    if (ContactList == "") {

                        ContactList = RoleID + ":" + RoleName;
                    }
                    else {

                        ContactList += ";" + RoleID + ":" + RoleName;
                    }
                }
                else {

                    if (UserRoleList == "") {

                        UserRoleList = RoleID + ":" + RoleName;
                    }
                    else {

                        UserRoleList += ";" + RoleID + ":" + RoleName;
                    }
                }

            });

            $("#hdnCCContactList").val(ContactList);
            $("#hdnCCUserRoleList").val(UserRoleList);

            //#region set task owner checked

            if ($("#chkTaskOwner").attr("checked") == true) {

                $("#hdnCCTaskOwnerChecked").val("True");

                var Tr = BuildTrString("User", "0", "", "Task Owner");
                $("#gridCCList").append(Tr);
            }
            else {

                $("#hdnCCTaskOwnerChecked").val("False");
            }

            //#endregion

            // set even
            $("#gridCCList tr:not(:first):odd").attr("class", "EvenRow");
            $("#gridCCList tr:not(:first):even").removeAttr("class");

            CloseDialog_SelectRecipient();
        }

        //#endregion

        function BuildTrString(RoleType, RoleID, Email, RoleName) {

            var ShowText = "";
            if (RoleType == "Email") {

                ShowText = Email;
            }
            else {

                ShowText = RoleName;
            }

            return "<tr><td class='CheckBoxColumn'><input type='checkbox' myRoleType='" + RoleType + "' myRoleID='" + RoleID + "' myEmail='" + Email + "' myRoleName='" + RoleName + "' /></td><td style='width:50px;'>" + RoleType + "</td><td>" + ShowText + "</td></tr>";

        }

        //#region remove To/CC

        function RemoveTo() {

            var SelectedCount = $("#gridToList tr td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No To recipient was selected.");
                return;
            }

            var Result = confirm("Do you want to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if ($("#gridToList tr td :checkbox:checked").length == $("#gridToList tr td :checkbox").length) {

                $("#gridToList").empty();
                $("#gridToList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no recipient.</td></tr>");

                $("#hdnToTaskOwnerChecked").val("False");
            }
            else {

                var RemovedTaskOwner = $("#gridToList tr td :checkbox:checked[myRoleName='Task Owner']").length;
                if (RemovedTaskOwner == 1) {

                    $("#hdnToTaskOwnerChecked").val("False");
                }

                $("#gridToList tr td :checkbox:checked").parent().parent().remove();

                // set even
                $("#gridToList tr:not(:first):odd").attr("class", "EvenRow");
                $("#gridToList tr:not(:first):even").removeAttr("class");
            }

            // text box
            var EmailList = "";
            var ContactList = "";
            var UserRoleList = "";
            $("#gridToList tr td :checkbox").each(function (i) {

                var RoleType = $(this).attr("myRoleType");
                var Email = $(this).attr("myEmail");
                var RoleID = $(this).attr("myRoleID");
                var RoleName = $(this).attr("myRoleName");

                if (RoleType == "Email") {

                    if (EmailList == "") {

                        EmailList = Email;
                    }
                    else {

                        EmailList += ";" + Email;
                    }
                }
                else if (RoleType == "Contact") {

                    if (ContactList == "") {

                        ContactList = RoleID + ":" + RoleName;
                    }
                    else {

                        ContactList += ";" + RoleID + ":" + RoleName;
                    }
                }
                else if (RoleType == "User") {

                    if (UserRoleList == "") {

                        UserRoleList = RoleID + ":" + RoleName;
                    }
                    else {

                        UserRoleList += ";" + RoleID + ":" + RoleName;
                    }
                }
            });

            $("#hdnToEmailList").val(EmailList);
            $("#hdnToContactList").val(ContactList);
            $("#hdnToUserRoleList").val(UserRoleList);
        }

        function RemoveCC() {

            var SelectedCount = $("#gridCCList tr td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No CC recipient was selected.");
                return;
            }

            var Result = confirm("Are you sure to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if ($("#gridCCList tr td :checkbox:checked").length == $("#gridCCList tr td :checkbox").length) {

                $("#gridCCList").empty();
                $("#gridCCList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no recipient.</td></tr>");

                $("#hdnCCTaskOwnerChecked").val("False");
            }
            else {

                var RemovedTaskOwner = $("#gridCCList tr td :checkbox:checked[myRoleName='Task Owner']").length;
                if (RemovedTaskOwner == 1) {

                    $("#hdnCCTaskOwnerChecked").val("False");
                }

                $("#gridCCList tr td :checkbox:checked").parent().parent().remove();

                // set even
                $("#gridCCList tr:not(:first):odd").attr("class", "EvenRow");
                $("#gridCCList tr:not(:first):even").removeAttr("class");
            }

            // text box
            var EmailList = "";
            var ContactList = "";
            var UserRoleList = "";
            $("#gridCCList tr td :checkbox").each(function (i) {

                var RoleType = $(this).attr("myRoleType");
                var Email = $(this).attr("myEmail");
                var RoleID = $(this).attr("myRoleID");
                var RoleName = $(this).attr("myRoleName");

                if (RoleType == "Email") {

                    if (EmailList == "") {

                        EmailList = Email;
                    }
                    else {

                        EmailList += ";" + Email;
                    }
                }
                else if (RoleType == "Contact") {

                    if (ContactList == "") {

                        ContactList = RoleID + ":" + RoleName;
                    }
                    else {

                        ContactList += ";" + RoleID + ":" + RoleName;
                    }
                }
                else if (RoleType == "User") {

                    if (UserRoleList == "") {

                        UserRoleList = RoleID + ":" + RoleName;
                    }
                    else {

                        UserRoleList += ";" + RoleID + ":" + RoleName;
                    }
                }
            });

            $("#hdnToEmailList").val(EmailList);
            $("#hdnCCContactList").val(ContactList);
            $("#hdnCCUserRoleList").val(UserRoleList);
        }

        //#endregion

        //#endregion

        //#region insert fields

        // insert previous point fields
        function btnInsert_Preview_onclick() {

            var FieldID = $("#hdnSelPrevPointFieldID").val();

            if (FieldID == "") {

                alert("Please select a field.");
                return;
            }

            if (FieldID != "") {

                var FieldLabel = $("#txtPrevPointField").val();

                //restore cursor position
                rad_editor.getSelection().selectRange(editor_selection);

                // paste html
                rad_editor.pasteHtml("<@Previous-" + FieldLabel + "@>");
            }
        }

        // insert current point fields
        function btnInsert_Current_onclick() {

            var FieldID = $("#hdnSelCurrentPointFieldID").val();

            if (FieldID == "") {

                alert("Please select a field.");
                return;
            }

            if (FieldID != "") {

                var FieldLabel = $("#txtCurrentPointField").val();

                //restore cursor position
                rad_editor.getSelection().selectRange(editor_selection);

                // paste html
                rad_editor.pasteHtml("<@" + FieldLabel + "@>");
            }
        }

        // insert InfoHub database fields
        function btnInsert_InfoHub_onclick() {

            var FieldID = $("#ddlInfoHubFields").val();
            if (FieldID == "0") {

                alert("Please select a field.");
                return;
            }

            //restore cursor position
            rad_editor.getSelection().selectRange(editor_selection);

            // paste html
            rad_editor.pasteHtml("<@DB-" + FieldID + "@>");
        }

        //#endregion

        function btnPreview_onclick() {

            var HtmlBody = rad_editor.get_html();
            var HtmlBody_Encode = $.base64Encode(HtmlBody);

            OpenWindow("EmailTemplatePreview.aspx?html=" + encodeURIComponent(HtmlBody_Encode), "_PreviewEmailTemplate3", 760, 700, "no", "center");

            return true;
        }

        function Cancel() {

            window.parent.location.href = "EmailTemplateList.aspx";
        }

        function btnImport_onclick() {

            var FakeFilePath = $("#FileUpload1").val();
            var IsValid = ValidateFileExt(FakeFilePath);
            if (IsValid == false) {

                alert("Please select a valid html file.");

                $("#FileUpload1").val("");

                return false;
            }

            return true;
        }

        function ValidateFileExt(FilePath) {

            var ext = FilePath.substr(FilePath.lastIndexOf('.'));
            ext = ext.replace(".", "");
            var AllowExt = "html|htm";
            var ExtOK = false;
            var ArrayExt;
            if (AllowExt.indexOf('|') != -1) {
                ArrayExt = AllowExt.split('|');
                for (i = 0; i < ArrayExt.length; i++) {
                    if (ext.toLowerCase() == ArrayExt[i]) {
                        ExtOK = true;
                        break;
                    }
                }
            }
            else {
                ArrayExt = AllowExt;
                if (ext.toLowerCase() == ArrayExt) {
                    ExtOK = true;
                }
            }
            return ExtOK;
        }

// ]]>
    </script>

    <div id="divContainer">
        <h6>Email Template Setup</h6>
        <div class="SplitLine" style="margin-top: 7px;"></div>
        <br />
        <br />
        <input id="btnCancel2" type="button" value="Back to Email Template List" class="Btn-250" onclick="Cancel();" />
        <br />
        <br />
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 115px;">Email Template Name:</td>
                <td style="width: 350px;">
                    <asp:TextBox ID="txtEmailTemplateName" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="chkEnabled" runat="server" Text=" Enabled" Checked="true" />
                </td>
            </tr>
        </table>
          <table cellpadding="0" cellspacing="0" style="margin-top: 9px;">
            <tr>
                <td style="vertical-align: top; padding-top: 5px;">
                    <asp:CheckBox ID="chkLeadCreated" runat="server" Text=" Sent upon lead creation by Lead Service" />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 9px;">
            <tr>
                <td style="width: 115px; vertical-align: top; padding-top: 5px;">Description:</td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Height="45px" Width="450px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 9px;">
            <tr>
                <td style="width: 115px; vertical-align: top; padding-top: 5px;">
                    Email Skin:
                </td>
                <td>
                    <asp:DropDownList ID="ddlEmailSkin" runat="server" Width="460px" DataValueField="EmailSkinID" DataTextField="Name"></asp:DropDownList>
                </td>
            </tr>
        </table>
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave();" onclick="btnSave_Click" />
                    </td>
                    <td>
                        <input id="btnPreview" type="button" value="Preview" class="Btn-66" onclick="btnPreview_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="divTabContainer" style="margin-top: 20px;">
            <div id="tabs">
                <ul>
                    <li><a href="#tabs-General">General</a></li>
                    <li><a href="#tabs-Body">Body</a></li>
                    <li><a href="#tabs-Attachments">Attachments</a></li>
                </ul>
                <div id="tabs-General">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 50px;">
                                Subject:
                            </td>
                            <td style="width: 350px;">
                                <asp:TextBox ID="txtSubject" runat="server" Width="710px" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" style="margin-top: 9px;">
                        <tr>
                            <td style="width: 50px;">
                                From:
                            </td>
                            <td style="width: 350px;">
                                <asp:DropDownList ID="ddlFromUserRoles" runat="server" Width="200px" DataValueField="RoleID" DataTextField="Name">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 120px;">
                                Or user-defined email:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromEmail" runat="server" Width="240px" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" style="margin-top: 9px;">
                        <tr>
                            <td style="vertical-align: top; padding-top: 5px;" rowspan="2">
                                To:
                            </td>
                            <td style="vertical-align: top;" rowspan="2">
                                <div id="divToList" class="ColorGrid" style="margin-top: 5px; width: 303px;">
                                    <asp:GridView ID="gridToList" runat="server" EmptyDataText="There is no recipient." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="EvenRow" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                                <HeaderTemplate>
                                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll_ToList(this)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input id="Checkbox2" type="checkbox" myRoleType="<%# Eval("Type") %>" myRoleID="<%# Eval("RoleID") %>" myEmail="<%# Eval("Email") %>" myRoleName="<%# Eval("RoleName") %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Width="50px" />
                                            <asp:TemplateField HeaderText="Email / Role">
                                                <ItemTemplate>
                                                    <%# this.GetEmailOrRole(Eval("Type").ToString(), Eval("Email").ToString(), Eval("RoleName").ToString()) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="GridPaddingBottom">&nbsp;</div>
                                </div>
                            </td>
                            <td>Sender Name: </td>
                        		<td>
                        				<asp:TextBox ID="txtSenderName" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                        		</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; padding-top: 12px; text-align: right; padding-right: 10px;">
                            		CC:
                            </td>
                            <td style="vertical-align: top; padding-top: 5px;">
                                <div id="divCCList" class="ColorGrid" style="margin-top: 5px; width: 303px;">
                                    <asp:GridView ID="gridCCList" runat="server" EmptyDataText="There is no recipient." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="EvenRow" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                                <HeaderTemplate>
                                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll_CCList(this)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input id="Checkbox2" type="checkbox" myRoleType="<%# Eval("Type") %>" myRoleID="<%# Eval("RoleID") %>" myEmail="<%# Eval("Email") %>" myRoleName="<%# Eval("RoleName") %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Width="50px" />
                                            <asp:TemplateField HeaderText="Email/Role">
                                                <ItemTemplate>
                                                    <%# this.GetEmailOrRole(Eval("Type").ToString(), Eval("Email").ToString(), Eval("RoleName").ToString()) %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="GridPaddingBottom">&nbsp;</div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50px;">&nbsp;</td>
                            <td style="width: 340px;">
                                <ul class="ToolStrip" style="margin-left: 0px;">
                                    <li><a id="aAddTo" href="javascript:ShowDialog_SelectRecipient_To()">Add Row</a><span>|</span></li>
                                    <li><a id="aRemoveTo" href="javascript:RemoveTo()">Remove</a></li>
                                </ul>
                            </td>
                            <td style="width: 80px;">&nbsp;</td>
                            <td>
                                <ul class="ToolStrip" style="margin-left: 0px;">
                                    <li><a id="aAddCC" href="javascript:ShowDialog_SelectRecipient_CC()">Add Row</a><span>|</span></li>
                                    <li><a id="aRemoveCC" href="javascript:RemoveCC()">Remove</a></li>
                                </ul>
                            </td>
                        </tr>
                    </table>      
                </div>
                <div id="tabs-Body">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                        <tr>
                            <td style="width: 260px; vertical-align: top;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 180px;">Previous value of Point Field:</td>
                                        <td>
                                            <input id="btnInsert_Preview" type="button" value="Insert" onclick="btnInsert_Preview_onclick()" style="display: none; border: none; background-color: White;" />
                                            <input id="btnPreviousValueOfPointField_Copy" type="button" value="Copy" data-clipboard-target="txtPreviousValueOfPointField" class="Btn-66" />
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <input id="txtPrevPointField" type="text" /><br />
                                    <div id="divPreviousValueOfPointField" style="display: none;">
                                        Merge tag for searched Point Field:
                                        <input id="txtPreviousValueOfPointField" type="text" readonly="readonly" class="ui-autocomplete-input" style="background-color: lightgrey;" />
                                    </div>
                                </div>
                                <br />
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 180px;">Current value of Point Field:</td>
                                        <td>
                                            <input id="btnInsert_Current" type="button" value="Insert" onclick="btnInsert_Current_onclick()" style="display: none; border: none; background-color: White;" />
                                            <input id="btnCurrentValueOfPointField_Copy" type="button" value="Copy" data-clipboard-target="txtCurrentValueOfPointField" class="Btn-66" />
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <input id="txtCurrentPointField" type="text" /><br />
                                    <div id="divCurrentValueOfPointField" style="display: none;">
                                        Merge tag for searched Point Field:
                                        <input id="txtCurrentValueOfPointField" type="text" readonly="readonly" class="ui-autocomplete-input" style="background-color: lightgrey;" />
                                    </div>
                                </div>
                                <br />
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 180px;">Pulse database Field:</td>
                                        <td>
                                            <input id="btnInsert_InfoHub" type="button" value="Insert" onclick="btnInsert_InfoHub_onclick()" style="display: none; border: none; background-color: White;" />
                                            <input id="btnValueOfPulseField_Copy" type="button" value="Copy" data-clipboard-target="txtValueOfPulseField" class="Btn-66" />
                                        </td>
                                    </tr>
                                </table>
                                <div>
                                    <asp:DropDownList ID="ddlInfoHubFields" runat="server" Width="250px">
                                        <asp:ListItem Value="0" Text="-- select a field --"></asp:ListItem>
                                        <asp:ListItem>Company Name</asp:ListItem>
                                        <asp:ListItem>Company Address</asp:ListItem>
                                        <asp:ListItem>Company City</asp:ListItem>
                                        <asp:ListItem>Company State</asp:ListItem>
                                        <asp:ListItem>Company Zip</asp:ListItem>
                                        <asp:ListItem>Company Homepage Logo</asp:ListItem>
                                        <asp:ListItem>Company Subpage Logo</asp:ListItem>
                                        <asp:ListItem>Branch Name</asp:ListItem>
                                        <asp:ListItem>Branch Address</asp:ListItem>
                                        <asp:ListItem>Branch City</asp:ListItem>
                                        <asp:ListItem>Branch State</asp:ListItem>
                                        <asp:ListItem>Branch Zip</asp:ListItem>
                                        <asp:ListItem>Branch Logo</asp:ListItem>
                                        <asp:ListItem>Loan Officer Picture</asp:ListItem>
                                        <asp:ListItem>Loan Officer Signature</asp:ListItem>
                                        <asp:ListItem>Loan Officer First Name</asp:ListItem>
                                        <asp:ListItem>Loan Officer Last Name</asp:ListItem>
                                        <asp:ListItem>Loan Officer Email</asp:ListItem>
                                        <asp:ListItem>Processor Picture</asp:ListItem>
                                        <asp:ListItem>Processor Signature</asp:ListItem>
                                        <asp:ListItem>Processor First Name</asp:ListItem>
                                        <asp:ListItem>Processor Last Name</asp:ListItem>
                                        <asp:ListItem>Processor Email</asp:ListItem>
                                        <asp:ListItem>Underwriter Picture</asp:ListItem>
                                        <asp:ListItem>Underwriter Signature</asp:ListItem>
                                        <asp:ListItem>Underwriter First Name</asp:ListItem>
                                        <asp:ListItem>Underwriter Last Name</asp:ListItem>
                                        <asp:ListItem>Underwriter Email</asp:ListItem>
                                        <asp:ListItem>Loan Officer Assistant Picture</asp:ListItem>
                                        <asp:ListItem>Loan Officer Assistant Signature</asp:ListItem>
                                        <asp:ListItem>Loan Officer Assistant First Name</asp:ListItem>
                                        <asp:ListItem>Loan Officer Assistant Last Name</asp:ListItem>
                                        <asp:ListItem>Loan Officer Assistant Email</asp:ListItem>
                                        <asp:ListItem>Doc Prep Picture</asp:ListItem>
                                        <asp:ListItem>Doc Prep Signature</asp:ListItem>
                                        <asp:ListItem>Doc Prep First Name</asp:ListItem>
                                        <asp:ListItem>Doc Prep Last Name</asp:ListItem>
                                        <asp:ListItem>Doc Prep Email</asp:ListItem>
                                        <asp:ListItem>Closer Picture</asp:ListItem>
                                        <asp:ListItem>Closer Signature</asp:ListItem>
                                        <asp:ListItem>Closer First Name</asp:ListItem>
                                        <asp:ListItem>Closer Last Name</asp:ListItem>
                                        <asp:ListItem>Closer Email</asp:ListItem>
                                        <asp:ListItem>Shipper Picture</asp:ListItem>
                                        <asp:ListItem>Shipper Signature</asp:ListItem>
                                        <asp:ListItem>Shipper First Name</asp:ListItem>
                                        <asp:ListItem>Shipper Last Name</asp:ListItem>
                                        <asp:ListItem>Shipper Email</asp:ListItem>
                                        <asp:ListItem>Accept Alert Button</asp:ListItem>
                                        <asp:ListItem>Decline Alert Button</asp:ListItem>
                                        <asp:ListItem>Sender Signature</asp:ListItem>
                                        <asp:ListItem>Sender Picture</asp:ListItem>
                                        <asp:ListItem>Client First Name</asp:ListItem>
                                        <asp:ListItem>Client Last Name</asp:ListItem>
                                        <asp:ListItem>Client Middle Name</asp:ListItem>
                                        <asp:ListItem>Client Nick Name</asp:ListItem>
                                        <asp:ListItem>Client Title</asp:ListItem>
                                        <asp:ListItem>Client Home Phone</asp:ListItem>
                                        <asp:ListItem>Client Cell Phone</asp:ListItem>
                                        <asp:ListItem>Client Business Phone</asp:ListItem>
                                        <asp:ListItem>Client Fax</asp:ListItem>
                                        <asp:ListItem>Client Email</asp:ListItem>
                                        <asp:ListItem>Client DOB</asp:ListItem>
                                        <asp:ListItem>Client Mailing Address</asp:ListItem>
                                        <asp:ListItem>Client Mailing City</asp:ListItem>
                                        <asp:ListItem>Client Mailing State</asp:ListItem>
                                        <asp:ListItem>Client Mailing Zip</asp:ListItem>
                                        <asp:ListItem>Task Owner First name</asp:ListItem>
                                        <asp:ListItem>Task Owner Last name</asp:ListItem>
                                        <asp:ListItem>Task Owner Full name</asp:ListItem>
                                        <asp:ListItem>Task Owner Email</asp:ListItem>
                                        <asp:ListItem>Task Owner Signature</asp:ListItem>
                                        <asp:ListItem>Task Description</asp:ListItem>
                                        <asp:ListItem>Task Due Date</asp:ListItem>
                                        <asp:ListItem>Task Alert Link</asp:ListItem>
                                        <asp:ListItem>Loan Link</asp:ListItem>
                                        <asp:ListItem>Rule Alert Link</asp:ListItem>
                                    </asp:DropDownList><br />
                                    <div id="divValueOfPulseField" style="display: none;">
                                        Merge tag for searched Pulse database Field:
                                        <input id="txtValueOfPulseField" type="text" readonly="readonly" class="ui-autocomplete-input" style="background-color: lightgrey;" />
                                    </div>
                                </div>
                            </td>
                            <td>
                                <table>
                                		<tr>
                                			<td>
                                				<asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Btn-66" onclick="btnExport_Click" />
                                			</td>
                                			<td style="padding-left: 20px;">
                                				<asp:FileUpload ID="FileUpload1" runat="server" />
                                			</td>
                                			<td>
                                				<asp:Button ID="btnImport" runat="server" Text="Import" CssClass="Btn-66" OnClientClick="return btnImport_onclick()" onclick="btnImport_Click" />
                                			</td>
                                            
                                		</tr>
                                </table>
                                <telerik:RadEditor ID="RadEditor1" runat="server" ContentAreaMode="Div" OnClientLoad="OnClientLoad" NewLineMode="P" Width="700px" Height="650px" EditModes="Design,Html">
                                    <Modules>
                                        <telerik:EditorModule Name="RadEditorStatistics" Visible="false" Enabled="false" />
                                        <telerik:EditorModule Name="RadEditorDomInspector" Visible="false" Enabled="false" />
                                        <telerik:EditorModule Name="RadEditorNodeInspector" Visible="false" Enabled="false" />
                                        <telerik:EditorModule Name="RadEditorHtmlInspector" Visible="false" Enabled="false" />
                                    </Modules>
                                    <Tools>
                                        <telerik:EditorToolGroup Tag="MainToolbar">
                                            <telerik:EditorTool Name="Print" ShortCut="CTRL+P" />
                                            <telerik:EditorTool Name="SelectAll" ShortCut="CTRL+A" />
                                            <telerik:EditorTool Name="Cut" />
                                            <telerik:EditorTool Name="Copy" ShortCut="CTRL+C" />
                                            <telerik:EditorTool Name="Paste" ShortCut="CTRL+V" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorSplitButton Name="Undo">
                                            </telerik:EditorSplitButton>
                                            <telerik:EditorSplitButton Name="Redo">
                                            </telerik:EditorSplitButton>
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup Tag="InsertToolbar">
                                            <telerik:EditorTool Name="InsertImage" Text="Insert Image" />
                                            
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="LinkManager" ShortCut="CTRL+K" />
                                            <telerik:EditorTool Name="Unlink" ShortCut="CTRL+SHIFT+K" />
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorTool Name="InsertParagraph" />
                                            <telerik:EditorTool Name="InsertHorizontalRule" />
                                            
                                            <telerik:EditorSeparator />
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorDropDown Name="FormatBlock" Text="Heading">
                                            </telerik:EditorDropDown>
                                            <telerik:EditorDropDown Name="FontName" Text="Font">
                                            </telerik:EditorDropDown>
                                            <telerik:EditorDropDown Name="FontSize" Text="Size">
                                            </telerik:EditorDropDown>
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorTool Name="Bold" ShortCut="CTRL+B" />
                                            <telerik:EditorTool Name="Italic" ShortCut="CTRL+I" />
                                            <telerik:EditorTool Name="Underline" ShortCut="CTRL+U" />
                                            <telerik:EditorTool Name="StrikeThrough" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="JustifyLeft" />
                                            <telerik:EditorTool Name="JustifyCenter" />
                                            <telerik:EditorTool Name="JustifyRight" />
                                            <telerik:EditorTool Name="JustifyFull" />
                                            <telerik:EditorTool Name="JustifyNone" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="Indent" />
                                            <telerik:EditorTool Name="Outdent" />
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="InsertOrderedList" />
                                            <telerik:EditorTool Name="InsertUnorderedList" />
                                            <telerik:EditorSeparator />
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup>
                                            <telerik:EditorSplitButton Name="ForeColor">
                                            </telerik:EditorSplitButton>
                                            <telerik:EditorSplitButton Name="BackColor">
                                            </telerik:EditorSplitButton>
                                        </telerik:EditorToolGroup>
                                        <telerik:EditorToolGroup Tag="DropdownToolbar">
                                            <telerik:EditorSplitButton Name="InsertSymbol">
                                            </telerik:EditorSplitButton>
                                            <telerik:EditorToolStrip Name="InsertTable">
                                            </telerik:EditorToolStrip>
                                            <telerik:EditorSeparator />
                                            <telerik:EditorTool Name="ConvertToLower" />
                                            <telerik:EditorTool Name="ConvertToUpper" />
                                            <telerik:EditorSeparator />
                                        </telerik:EditorToolGroup>
                                    </Tools>

                                    
                                    <TemplateManager UploadPaths="~/_layouts/LPWeb/UploadFiles/EmailTemplate" ViewPaths="~/_layouts/LPWeb/UploadFiles/EmailTemplate" />

                                    <TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>

                                </telerik:RadEditor>

                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tabs-Attachments">
                    <iframe id="ifrAttachments" frameborder="0" scrolling="no" width="820px" height="740px" ></iframe>
                </div>
            </div>
        </div>
        
    </div>
    <div id="divEmailRecipientSelection" title="Email Recipient Selection" style="display: none;">
        
        <div style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <input id="btnAdd" type="button" value="Add" class="Btn-66" onclick="btnAdd_click()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="CloseDialog_SelectRecipient()" />
                    </td>
                </tr>
            </table>
        </div>
        
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
            <tr>
                <td style="width: 125px;">
                    <input id="chkUserDefinedEmail" type="checkbox" onclick="chkUserDefinedEmail_click(this)" checked /><label for="chkUserDefinedEmail"> User-defined Email:</label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmails" runat="server" Width="390px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="margin-top: 10px;">
            <input id="chkTaskOwner" type="checkbox" /><label for="chkTaskOwner"> Task Owner</label>
        </div>
        <div style="margin-top: 15px; width: 528px; height: 380px; overflow: auto; border-bottom: solid 1px #e4e7ef;">
            <div id="divRecipientRoleList" class="ColorGrid" style="width: 508px;">
                <asp:GridView ID="gridRecipientRoleList" runat="server" EmptyDataText="There is no user roles/contact roles." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll_RecipientRoleList(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Checkbox2" type="checkbox" myRoleType="<%# Eval("RecipientType") %>" myRoleID="<%# Eval("RoleID")%>" myRoleName="<%# Eval("RecipientRole")%>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RecipientType" HeaderText="Recipient Type" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="RecipientRole" HeaderText="Recipient Role" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        
    </div>
    <div id="divHiddenFields_To" style="display: none;">
        <asp:TextBox ID="hdnToEmailList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnToContactList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnToUserRoleList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnToTaskOwnerChecked" runat="server" Text="False"></asp:TextBox>
    </div>
    <div id="divHiddenFields_CC" style="display: none;">
        <asp:TextBox ID="hdnCCEmailList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnCCContactList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnCCUserRoleList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnCCTaskOwnerChecked" runat="server" Text="False"></asp:TextBox>
    </div>
    <div id="divHiddenOthers" style="display: none;">
        <input id="hdnFromWho" type="text" />
        <asp:TextBox ID="hdnIsPostBack" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnSelPrevPointFieldID" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnSelCurrentPointFieldID" runat="server"></asp:TextBox>
    </div>
    </form>
</body>
</html>