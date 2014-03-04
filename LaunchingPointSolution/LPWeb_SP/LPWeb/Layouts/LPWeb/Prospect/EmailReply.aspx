<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailReply.aspx.cs" Inherits="Prospect_EmailReply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.cleditor.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.tabs.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        var HtmlEditor;

        $(document).ready(function () {

            InitHtmlEditor();

            $("#txtSubject").maxlength("500");

        });

        // init cleditor
        function InitHtmlEditor() {

            if (HtmlEditor == undefined && $("#txtBody").is(":visible") == true) {

                HtmlEditor = $("#txtBody").cleditor({
                    width: 620,
                    height: 250,
                    bodyStyle: "font:11px Arial",
                    docType: '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">'
                });
            }
        }

        //#region Show/Close Dialog for Select Recipient

        function ShowDialog_SelectRecipient_To() {

            // clear
            $("#txtEmails").val("")
            $("#gridRecipientList tr :checkbox").attr("checked", "");

            // copy email
            $("#txtEmails").val($("#hdnToEmailList").val());

            // check exists
            var ExistEmailList = "";
            $("#gridToList tr td :checkbox").each(function () {

                var RoleType = $(this).attr("RoleType");
                var UserID = $(this).attr("UserID");

                if (RoleType == "Contact") {

                    $("#gridRecipientList tr td :checkbox[RoleType='Contact'][UserID='" + UserID + "']").attr("checked", "true");
                }
                else if (RoleType == "User") {

                    $("#gridRecipientList tr td :checkbox[RoleType='User'][UserID='" + UserID + "']").attr("checked", "true");
                }
            });

            // set flag
            $("#hdnFromWho").val("To");

            // show modal
            $("#divRecipientSelection").dialog({
                height: 580,
                width: 560,
                modal: true,
                resizable: false,
                close: function (event, ui) { $("#divRecipientSelection").dialog("destroy"); }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
        }

        function ShowDialog_SelectRecipient_CC() {

            // clear
            $("#txtEmails").val("")
            $("#gridRecipientList tr :checkbox").attr("checked", "");

            $("#txtEmails").val($("#hdnCCEmailList").val());

            // check exists
            var ExistEmailList = "";
            $("#gridCCList tr td :checkbox").each(function () {

                var RoleType = $(this).attr("RoleType");
                var UserID = $(this).attr("UserID");

                if (RoleType == "Contact") {

                    $("#gridRecipientList tr td :checkbox[RoleType='Contact'][UserID='" + UserID + "']").attr("checked", "true");
                }
                else if (RoleType == "User") {

                    $("#gridRecipientList tr td :checkbox[RoleType='User'][UserID='" + UserID + "']").attr("checked", "true");
                }
            });

            // set flag
            $("#hdnFromWho").val("CC");

            // show modal
            $("#divRecipientSelection").dialog({
                height: 580,
                width: 560,
                modal: true,
                resizable: false,
                close: function (event, ui) { $("#divRecipientSelection").dialog("destroy"); }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
        }

        function CloseDialog_SelectRecipient() {

            $("#divRecipientSelection").dialog("close");
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

        //#endregion

        //#region Check All

        function CheckAll_RecipientList(CheckBox) {

            if (CheckBox.checked) {

                $("#gridRecipientList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridRecipientList tr td :checkbox").attr("checked", "");
            }
        }

        function CheckAll_ToList(CheckBox) {

            if (CheckBox.checked) {

                $("#gridToList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridToList tr td :checkbox").attr("checked", "");
            }
        }

        function CheckAll_CCList(CheckBox) {

            if (CheckBox.checked) {

                $("#gridCCList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridCCList tr td :checkbox").attr("checked", "");
            }
        }

        //#endregion

        //#region Add To/CC List

        function btnAdd_click() {

            var FromWho = $("#hdnFromWho").val();

            if (FromWho == "") {

                return;
            }

            if (FromWho == "To") {

                AddRecipient_To();
            }
            else {

                AddRecipient_CC();
            }
        }

        function AddRecipient_To() {

            // user-defined emails
            var EmailArray;
            if ($("#chkUserDefinedEmail").attr("checked") == true) {

                var EmailListString = $.trim($("#txtEmails").val());
                if (EmailListString != "") {

                    // valid email list
                    var Pattern = /^([a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)(\;[a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)*$/;
                    var IsValid = Pattern.test(EmailListString);
                    if (IsValid == false) {

                        alert("Please enter a valid email with the correct format: techsupport@a.com or tech.support@b.com");
                        return;
                    }

                    // split
                    EmailArray = EmailListString.split(";");

                    // check duplicate
                    for (var k = 0; k < EmailArray.length; k++) {

                        var EmailStd = EmailArray[k];

                        for (var n = k + 1; n < EmailArray.length; n++) {

                            if (EmailStd == EmailArray[n]) {

                                alert("Duplicate email was not allowed.");
                                return;
                            }
                        }
                    }

                    // copy email list
                    $("#hdnToEmailList").val(EmailListString);
                }
            }

            // role list
            var SelectedCount = $("#gridRecipientList tr:not(:first) td :checkbox:checked").length;
            if (EmailArray == undefined && SelectedCount == 0) {

                alert("Please enter user-defined email, or select user/contact as recipient.");
                return;
            }

            var m = 1;

            // clear tr
            $("#gridToList").empty();

            // add th
            $("#gridToList").append("<tr><th class='CheckBoxHeader' scope='col'><input type='checkbox' onclick='CheckAll_ToList(this)' /></th><th scope='col'>Role/Type</th><th scope='col'>Name/Email</th></tr>");

            if (EmailArray != undefined) {

                for (var j = 0; j < EmailArray.length; j++) {

                    var Tr = BuildTrString("Email", "", "", "", EmailArray[j]);
                    $("#gridToList").append(Tr);

                    m++;
                }
            }

            $("#gridRecipientList tr:not(:first) td :checkbox:checked").each(function (i) {

                var RoleType = $(this).attr("RoleType");
                var RoleName = $(this).attr("RoleName");
                var UserID = $(this).attr("UserID");
                var FullName = $(this).attr("FullName");

                var Tr = BuildTrString(RoleType, RoleName, UserID, FullName, "");
                $("#gridToList").append(Tr);

            });

            // set even
            $("#gridToList tr:not(:first):odd").attr("class", "EvenRow");
            $("#gridToList tr:not(:first):even").removeAttr("class");

            CloseDialog_SelectRecipient();
        }

        function AddRecipient_CC() {

            // user-defined emails
            var EmailArray;
            if ($("#chkUserDefinedEmail").attr("checked") == true) {

                var EmailListString = $.trim($("#txtEmails").val());
                if (EmailListString != "") {

                    // valid email list
                    var Pattern = /^([a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)(\;[a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+)*$/;
                    var IsValid = Pattern.test(EmailListString);
                    if (IsValid == false) {

                        alert("Please enter a valid email with the correct format: techsupport@a.com or tech.support@b.com");
                        return;
                    }

                    // split
                    EmailArray = EmailListString.split(";");

                    // check duplicate
                    for (var k = 0; k < EmailArray.length; k++) {

                        var EmailStd = EmailArray[k];

                        for (var n = k + 1; n < EmailArray.length; n++) {

                            if (EmailStd == EmailArray[n]) {

                                alert("Duplicate email was not allowed.");
                                return;
                            }
                        }
                    }

                    // copy email list
                    $("#hdnCCEmailList").val(EmailListString);
                }
            }

            // role list
            var SelectedCount = $("#gridRecipientList tr td :checkbox:checked").length;
            if (EmailArray == undefined && SelectedCount == 0) {

                alert("Please enter user-defined email, or select user/contact as recipient.");
                return;
            }

            var m = 1;

            // clear tr
            $("#gridCCList").empty();

            // add th
            $("#gridCCList").append("<tr><th class='CheckBoxHeader' scope='col'><input type='checkbox' onclick='CheckAll_CCList(this)' /></th><th scope='col'>Role/Type</th><th scope='col'>Name/Email</th></tr>");

            if (EmailArray != undefined) {

                for (var j = 0; j < EmailArray.length; j++) {

                    var Tr = BuildTrString("Email", "", "", "", EmailArray[j]);
                    $("#gridCCList").append(Tr);

                    m++;
                }
            }

            $("#gridRecipientList tr:not(:first) td :checkbox:checked").each(function (i) {

                var RoleType = $(this).attr("RoleType");
                var RoleName = $(this).attr("RoleName");
                var UserID = $(this).attr("UserID");
                var FullName = $(this).attr("FullName");

                var Tr = BuildTrString(RoleType, RoleName, UserID, FullName, "");
                $("#gridCCList").append(Tr);

            });

            // set even
            $("#gridCCList tr:not(:first):odd").attr("class", "EvenRow");
            $("#gridCCList tr:not(:first):even").removeAttr("class");

            CloseDialog_SelectRecipient();
        }

        function BuildTrString(RoleType, RoleName, UserID, FullName, Email) {

            var ShowText = "";
            if (RoleType == "Email") {

                ShowText = Email;
            }
            else {

                ShowText = FullName;
            }

            return "<tr><td class='CheckBoxColumn'><input type='checkbox' RoleType='" + RoleType + "' RoleName='" + RoleName + "' UserID='" + UserID + "' FullName='" + FullName + "' Email='" + Email + "' /></td><td style='width: 150px;'>" + RoleName + "</td><td>" + ShowText + "</td></tr>";

        }

        //#endregion

        //#region Remove Recipient

        function aRemoveTo_onclick() {

            var SelectedCount = $("#gridToList tr:not(:first) td :checkbox:checked").length;
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
                $("#gridToList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no To recipient.</td></tr>");
            }
            else {

                $("#gridToList tr td :checkbox:checked").parent().parent().remove();

                // set even
                $("#gridToList tr:not(:first):odd").attr("class", "EvenRow");
                $("#gridToList tr:not(:first):even").removeAttr("class");
            }
        }

        function aRemoveCC_onclick() {

            var SelectedCount = $("#gridCCList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No CC recipient was selected.");
                return;
            }

            var Result = confirm("Do you want to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if ($("#gridCCList tr td :checkbox:checked").length == $("#gridCCList tr td :checkbox").length) {

                $("#gridCCList").empty();
                $("#gridCCList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no CC recipient.</td></tr>");
            }
            else {

                $("#gridCCList tr td :checkbox:checked").parent().parent().remove();

                // set even
                $("#gridCCList tr:not(:first):odd").attr("class", "EvenRow");
                $("#gridCCList tr:not(:first):even").removeAttr("class");
            }
        }

        //#endregion

        function BeforeSend() {

            var Subject = $.trim($("#txtSubject").val());
            if (Subject == "") {

                alert("Please enter email Subject.");
                return false;
            }

            if ($("#gridToList tr td :checkbox").length == 0) {

                alert("Please select To recipient.");
                return false;
            }

            //#region To List

            var ToContactList = "";
            var ToUserList = "";
            $("#gridToList tr:not(:first) td :checkbox").each(function (i) {

                var RoleType = $(this).attr("RoleType");
                var UserID = $(this).attr("UserID");

                if (RoleType == "Contact") {

                    if (ToContactList == "") {

                        ToContactList = UserID;
                    }
                    else {

                        ToContactList += "," + UserID;
                    }
                }
                else if (RoleType == "User") {

                    if (ToUserList == "") {

                        ToUserList = UserID;
                    }
                    else {

                        ToUserList += "," + UserID;
                    }
                }
            });

            $("#hdnToContactList").val(ToContactList);
            $("#hdnToUserList").val(ToUserList);

            //alert($("#hdnToContactList").val());
            //alert($("#hdnToUserList").val());

            //#endregion

            //#region CC List

            var CCContactList = "";
            var CCUserList = "";
            $("#gridCCList tr:not(:first) td :checkbox").each(function (i) {

                var RoleType = $(this).attr("RoleType");
                var UserID = $(this).attr("UserID");

                if (RoleType == "Contact") {

                    if (CCContactList == "") {

                        CCContactList = UserID;
                    }
                    else {

                        CCContactList += "," + UserID;
                    }
                }
                else if (RoleType == "User") {

                    if (CCUserList == "") {

                        CCUserList = UserID;
                    }
                    else {

                        CCUserList += "," + UserID;
                    }
                }
            });

            $("#hdnCCContactList").val(CCContactList);
            $("#hdnCCUserList").val(CCUserList);

            //alert($("#hdnCCContactList").val());
            //alert($("#hdnCCUserList").val());

            //#endregion

            return true;
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divPopupContainer" style="border: solid 0px red; height: 700px; overflow: auto;">
        <table>
            <tr>
                <td style="width: 50px;">Subject:</td> 
                <td>
                    <asp:TextBox ID="txtSubject" runat="server" TextMode="MultiLine" Height="30px" Width="500px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="margin-top: 10px;">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="Btn-66" OnClientClick="return BeforeSend()" onclick="btnSend_Click" />
                    </td>
                    <td>
                        <input id="btnCancel" type="button" value="Cancel" onclick="btnCancel_onclick()" class="Btn-66" />
                    </td>
                </tr>
            </table>
            
        </div>
        <div style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 30px;">To:</td>
                    <td>
                        <div id="divToolBarTo">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aAddTo" href="javascript:ShowDialog_SelectRecipient_To()">Add</a><span>|</span></li>
                                <li><a id="aRemoveTo" href="javascript:aRemoveTo_onclick()">Remove</a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToList" class="ColorGrid" style="margin-top: 0px; width: 620px;">
            <asp:GridView ID="gridToList" runat="server" EmptyDataText="There is no To recipient." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input type="checkbox" onclick="CheckAll_ToList(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" RoleType="<%# Eval("RoleType") %>" RoleName="<%# Eval("RoleName")%>" UserID="<%# Eval("UserID")%>" FullName='<%# Eval("FullName")%>' Email='<%# Eval("Email")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RoleName" HeaderText="Role/Type" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="FullNameOrEmail" HeaderText="Name/Email" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <div style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 30px;">CC:</td>
                    <td>
                        <div id="divToolBarCC">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aAddCC" href="javascript:ShowDialog_SelectRecipient_CC()">Add</a><span>|</span></li>
                                <li><a id="aRemoveCC" href="javascript:aRemoveCC_onclick()">Remove</a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divCCList" class="ColorGrid" style="margin-top: 0px; width: 620px;">
            <asp:GridView ID="gridCCList" runat="server" EmptyDataText="There is no CC recipient." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input type="checkbox" onclick="CheckAll_CCList(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" RoleType="<%# Eval("RoleType") %>" RoleName="<%# Eval("RoleName")%>" UserID="<%# Eval("UserID")%>" FullName="<%# Eval("FullName")%>" Email="<%# Eval("Email")%>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RoleName" HeaderText="Role/Type" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="FullNameOrEmail" HeaderText="Name/Email" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <table style="margin-top: 10px;">
            <tr>
                <td>
                    <asp:CheckBox ID="chkAppendMyPicture" runat="server" />
                </td>
                <td>
                    <label for="chkAppendMyPicture">Append my picture and signature at the bottom</label>
                </td>
            </tr>
        </table>
        <div style="margin-top: 10px;">
            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Height="200px"></asp:TextBox>
        </div>
    </div>
    <div id="divRecipientSelection" title="Email Recipient Selection" style="display: none;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 125px;">
                    <input id="chkUserDefinedEmail" type="checkbox" onclick="chkUserDefinedEmail_click(this)" checked /><label for="chkUserDefinedEmail"> User-defined Email:</label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmails" runat="server" Width="390px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="margin-top: 15px; width: 535px; height: 420px; overflow: auto; border-bottom: solid 1px #e4e7ef;">
            <div id="divRecipientList" class="ColorGrid">
                <asp:GridView ID="gridRecipientList" runat="server" EmptyDataText="There is no user recipient." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input type="checkbox" onclick="CheckAll_RecipientList(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" RoleType="<%# Eval("RoleType") %>" RoleName="<%# Eval("RoleName")%>" UserID="<%# Eval("UserID")%>" FullName="<%# Eval("FullName")%>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoleName" HeaderText="Role" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="FullName" HeaderText="Name" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        <div style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <input id="btnAdd" type="button" value="Add" class="Btn-66" onclick="btnAdd_click()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="CloseDialog_SelectRecipient()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divHiddenFields_To" style="display: none;">
        <asp:TextBox ID="hdnToEmailList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnToContactList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnToUserList" runat="server"></asp:TextBox>
    </div>
    <div id="divHiddenFields_CC" style="display: none;">
        <asp:TextBox ID="hdnCCEmailList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnCCContactList" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnCCUserList" runat="server"></asp:TextBox>
    </div>
    <asp:HiddenField ID="hdnFromWho" runat="server" />
    </form>
</body>
</html>
