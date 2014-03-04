<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectLoanContactTab.aspx.cs"
    Inherits="ProspectLoanContactTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/jqcontextmenu.css">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="../js/jqcontextmenu.js"></script>

    <script type="text/javascript">

        //gdc crm41 
        jQuery(document).ready(function ($) {

            if ($('a.dispose').attr("disabled") == undefined || $('a.dispose').attr("disabled") == "") {

                $('a.dispose').addcontextmenu('divDisposeMenu') //apply context menu to all images on the page
            }

            // Add Menu
            $('#aAdd').addcontextmenu('divAddMenu');
        })
        $(document).ready(function () {
            // if not active loan, disable all
            var IsActiveLoan = $("#hdnActiveLoan").val();
            if (IsActiveLoan == "False") {

                DisableAll();
            }
        });

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

        function DisableAll() {

            $("#gvContacts :checkbox").attr("disabled", "true");

            $(".ToolStrip a").each(function () {

                $(this).attr("disabled", "true");
                $(this).removeAttr("href");
                $(this).css("text-decoration", "none");
            });
        }

        function LoanContactAdd() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Contact/PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.parent.parent.CloseGlobalPopup()";

            var BaseWidth = 820;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 680;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.parent.ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function LoanContactUpdate() {
            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("No record has been selected.");
                return;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            if (checkedIds[0].indexOf("User") > -1) {
                alert("User records can only be updated from the Users Setup page.");
                return;
            }

            var ContactID = checkedIds.pop().replace(/Contract/, "");
            getUpdatePage(ContactID);
        }

        function getUpdatePage(ContactID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var checkedIds1 = getSelectedItems3();
            if (checkedIds1[0].indexOf("Borrower") > -1 || checkedIds1[0].indexOf("CoBorrowe") > -1) {
                var checkedIds = getSelectedItems2();
                var fileId = $("#<%= hfdFileId.ClientID %>").val();
                var iFrameSrc = "../Prospect/ProspectDetailPopup.aspx?sid=" + RadomStr + "&ProspectID=" + ContactID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()&RefreshCodes=window.parent.RefreshLoanDetailInfo()";

                var BaseWidth = 800;
                var iFrameWidth = BaseWidth + 2;
                var divWidth = iFrameWidth + 25;

                var BaseHeight = 600;
                var iFrameHeight = BaseHeight + 2;
                var divHeight = iFrameHeight + 40;

                window.parent.parent.ShowGlobalPopup("Client Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

            }
            else {

                var iFrameSrc = "../Contact/PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&ContactID=" + ContactID + "&CloseDialogCodes=window.parent.parent.parent.CloseGlobalPopup()";

                var BaseWidth = 820;
                var iFrameWidth = BaseWidth + 2;
                var divWidth = iFrameWidth + 25;

                var BaseHeight = 680;
                var iFrameHeight = BaseHeight + 2;
                var divHeight = iFrameHeight + 40;

                window.parent.parent.ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

            }


        }
        //gdc crm41 end

        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gvContacts.ClientID %> :checkbox").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        function getSelectedItems2() {
            var selctedItems = new Array();
            $("#<%=gvContacts.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag") + "_" + item.attr("ContactRoleId"));
                }
            });
            return selctedItems;
        }

        //gdc crm41 

        function RemoveContact() {

            // show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        var IsActiveLoan = $("#hdnActiveLoan").val();
                        if (IsActiveLoan == "False") {
                            return;
                        }
                        var checkedIds = getSelectedItems2();

                        if (checkedIds.length == 0) {
                            alert("No record has been selected.");
                            return;
                        }

                        var checkedIds1 = getSelectedItems3();
                        var isErrMsg = false;
                        for (var i = 0; i < checkedIds1.length; i++) {
                            if (checkedIds1[i].indexOf("Borrower") > -1 || checkedIds1[i].indexOf("CoBorrowe") > -1) {

                                isErrMsg = true;
                                break;
                            }
                        }
                        if (isErrMsg) {

                            alert("The borrower or coborrower can not be deleted.");
                            return;
                        }


                        var r1 = confirm("Are you sure you want to remove the selected contact from the loan?");
                        if (r1 == false) {

                            return;
                        }

                        $("#<%= hfContactIDs.ClientID %>").val(checkedIds);

                        __doPostBack("btnRemove", "");
                    }
                }
            });




        }


        function getSelectedItems3() {
            var selctedItems = new Array();
            $("#<%=gvContacts.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("ContactRole"));
                }
            });
            return selctedItems;
        }
        function Reassign() {

            // show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        var checkedIds = getSelectedItems2();
                        if (checkedIds.length == 0) {
                            alert("Please select a contact to reassign.");
                            return;
                        }

                        if (checkedIds == null || checkedIds.length != 1) {
                            alert("Only one record can be selected for this operation.");
                            return;
                        }

                        var checkedIds1 = getSelectedItems3();
                        if (checkedIds1[0].indexOf("Borrower") > -1 || checkedIds1[0].indexOf("CoBorrowe") > -1) {

                            alert("The borrower or coborrower can not be reassigned.");
                            return;
                        }

                        if (checkedIds[0].indexOf("User") > -1) {
                            LoanReassign(checkedIds[0]);
                        }
                        else {
                            ContactReassign(checkedIds[0]);
                        }
                    }
                }
            });
        }

        function LoanReassign(userandrole) {
            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            var fileId = $("#<%= hfdFileId.ClientID %>").val();
            //            var checkedIds = getSelectedItems();
            //            $("#ifrLoanReassign").attr("src", "ReassignLoan.aspx?FileID=" + fileId + "&UserID=" + checkedIds.pop() + "&sid=" + radomStr);
            //            $("#ifrLoanReassign").attr("src", "ReassignLoan.aspx?FileID=" + fileId + "&uandr=" + userandrole + "&sid=" + radomStr);
            //            $("#divLoanReassign").dialog({
            //                height: 320,
            //                width: 630,
            //                modal: true,
            //                resizable: false
            //            });
            //            $(".ui-dialog").css("border", "solid 3px #aaaaaa")

            var iFrameSrc = "../LoanDetails/ReassignLoan.aspx?FileID=" + fileId + "&uandr=" + userandrole + "&sid=" + radomStr + "&CloseDialogCodes=window.parent.parent.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 320;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Loan Reassign", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc)

        }

        function ContactReassign(userandrole) {
            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            var fileId = $("#<%= hfdFileId.ClientID %>").val();
            //            $("#ifrContactReassign").attr("src", "LoanReassignContactTab.aspx?FileID=" + fileId + "&uandr=" + userandrole + "&sid=" + radomStr);
            //            $("#divContactReassign").dialog({
            //                height: 460,
            //                width: 660,
            //                modal: true,
            //                resizable: false
            //            });

            //            $(".ui-dialog").css("border", "solid 3px #aaaaaa")

            var iFrameSrc = "../LoanDetails/LoanReassignContactTab.aspx?FileID=" + fileId + "&uandr=" + userandrole + "&sid=" + radomStr + "&CloseDialogCodes=window.parent.parent.parent.CloseGlobalPopup()";

            var BaseWidth = 660;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 460;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Contact Reassign", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc)

        }

        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gvContacts.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        //gdc crm41 end 

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

        // show popup for send email
        function ShowDialog_SendEmail() {

            var checkedIds = getSelectedItems();
            //            if (checkedIds.length == 0) {
            //                alert("Please select one row.");
            //                return;
            //            }

            //            if (checkedIds == null || checkedIds.length != 1) {
            //                alert("Only one record can be selected for this operation.");
            //                return;
            //            }

            //var ProspectID = checkedIds.pop().replace(/Contract/, "").replace(/User/, ""); ; //
            var LoanID = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "../LoanDetails/EmailSendPopup.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            //var iFrameSrc = "../LoanDetails/EmailSendPopup.aspx?sid=" + RadomStr + "&ProspectID=" + ProspectID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            window.parent.parent.ShowGlobalPopup("Send Email", 605, 530, 630, 570, iFrameSrc);

        }


        //gdc crm41
        function CloseDialog_SendEmail() {

            $("#divSendEmail").dialog("close");
        }

        function RefreshPage() {

            if ($.browser.msie == true) {   // ie 

                window.parent.document.frames("ifrLoanInfo").location.reload();
            }
            else {  // firefox 

                window.parent.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
            }

            // refresh current page
            window.location.href = window.location.href;

        }

        function BeforeSendNow() {

            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("No record has been selected.");
                return false;
            }

            $("#<%= hfContactIDs.ClientID %>").val(checkedIds);
            return true;
        }

        function aAssignLoanContact_onclick() {

            // show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        var p = "";
                        if ($.browser.msie == true) {   // ie

                            p = "window.parent.parent.document.frames['ifrGlider'+(window.parent.parent.idivIndex+1)].document.frames['tabFrame'].document.frames['tabFrame'].AssignLoanContact";
                        }
                        else {   // firefox

                            p = "window.parent.parent.document.getElementById(window.parent.parent.document.getElementsByTagName('iframe')['ifrGlider'+(window.parent.parent.idivIndex+1)].id).contentDocument.getElementById('tabFrame').contentDocument.getElementById('tabFrame').contentWindow.AssignLoanContact";
                        }

                        var iFrameSrc = "../LoanDetails/AssignLoanContactPopup.aspx?sid=" + sid + "&FileID=" + FileID + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.parent.CloseGlobalPopup()";

                        var BaseWidth = 700;
                        var iFrameWidth = BaseWidth + 2;
                        var divWidth = iFrameWidth + 25;

                        var BaseHeight = 570;
                        var iFrameHeight = BaseHeight + 2;
                        var divHeight = iFrameHeight + 40;

                        window.parent.parent.ShowGlobalPopup("Assign Loan Contact", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
                    }
                }
            });

        }

        function AssignLoanContact(ContactID, ContactRoleID) {

            window.parent.parent.CloseGlobalPopupNoRefreshPage();
            window.parent.parent.parent.ShowWaitingDialog3("Please wait...");

            var FileID = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // check exist
            $.getJSON("../LoanDetails/AnssignLoanContact_Ajax.aspx?sid=" + Radom + "&FileID=" + FileID + "&ContactID=" + ContactID + "&ContactRoleID=" + ContactRoleID, AfterAssignLoanContact);
        }

        function AfterAssignLoanContact(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Completed.");

                    window.location.href = window.location.href;
                }
                window.parent.parent.parent.CloseWaitingDialog3();

            }, 2000);
        }

        function aAssignLoanTeam_onclick() {

            // show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        var p = "";
                        if ($.browser.msie == true) {   // ie

                            p = "window.parent.parent.document.frames['ifrGlider'+(window.parent.parent.idivIndex+1)].document.frames['tabFrame'].document.frames['tabFrame'].AssignLoanTeam";
                        }
                        else {   // firefox

                            p = "window.parent.parent.document.getElementById(window.parent.parent.document.getElementsByTagName('iframe')['ifrGlider'+(window.parent.parent.idivIndex+1)].id).contentDocument.getElementById('tabFrame').contentDocument.getElementById('tabFrame').contentWindow.AssignLoanTeam";
                        }

                        var iFrameSrc = "../LoanDetails/AssignLoanTeamPopup.aspx?sid=" + sid + "&FileID=" + FileID + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.parent.CloseGlobalPopup()";

                        var BaseWidth = 700;
                        var iFrameWidth = BaseWidth + 2;
                        var divWidth = iFrameWidth + 25;

                        var BaseHeight = 570;
                        var iFrameHeight = BaseHeight + 2;
                        var divHeight = iFrameHeight + 40;

                        window.parent.parent.ShowGlobalPopup("Assign Loan Team", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
                    }
                }
            });
        }

        function AssignLoanTeam(UserID, LoanRoleID) {

            window.parent.parent.CloseGlobalPopupNoRefreshPage();
            window.parent.parent.parent.ShowWaitingDialog3("Please wait...");

            var FileID = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // check exist
            $.getJSON("../LoanDetails/AnssignLoanTeam_Ajax.aspx?sid=" + Radom + "&FileID=" + FileID + "&UserID=" + UserID + "&LoanRoleID=" + LoanRoleID, AfterAssignLoanTeam);
        }

        function AfterAssignLoanTeam(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Completed.");

                    window.location.href = window.location.href;
                }
                window.parent.parent.parent.CloseWaitingDialog3();

            }, 2000);
        }

        //gdc crm41 end 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 700px;">
                <tr>
                    <td>
                        <div id="div1" style="margin-left: 10px;">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li>
                                	<asp:HyperLink ID="aAdd" NavigateUrl="javascript:return false;" runat="server">Add</asp:HyperLink><span>|</span>
                                </li>
                                <li>
                                    <asp:HyperLink ID="btnUpdate" NavigateUrl="javascript:LoanContactUpdate();" runat="server">Update</asp:HyperLink>
                                    <span>|</span></li>
                                <li>
                                    <a id="aRemove" runat="server" href="javascript:RemoveContact();">Remove</a>
                                    <asp:LinkButton ID="btnRemove" runat="server" OnClick="btnRemove_Click" style="display:none;">Remove</asp:LinkButton>
                                    <span>|</span></li>
                                <li>
                                    <a id="btnReassign" runat="server" href="javascript:Reassign();">Reassign</a>
                                    <span>|</span></li>
                                <%--<li>
                                    <input id="btnSendEmail" type="button" value="Send Email" class="Btn-91" onclick="javascript:ShowDialog_SendEmail()" />
                                </li>--%>
                                <li>
                                    <asp:HyperLink ID="btnSendEmail" NavigateUrl="javascript:ShowDialog_SendEmail()"
                                        runat="server">Send Email</asp:HyperLink>
                                    <span>|</span></li>

                                <%--<li><a id="aSendLogin" disabled='disabled'>Send Login</a> <span>|</span> </li>
                                <li>
                                    <asp:HyperLink ID="btnSendReport" NavigateUrl="javascript:return fasle;" CssClass="dispose" runat="server">Send Report</asp:HyperLink>
                                </li>--%>

                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
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
    <div id="divGrid" class="ColorGrid" style="width: 990px; margin-top: 5px; margin-left: 10px;">
        <asp:GridView ID="gvContacts" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
            OnSorting="gvContacts_Sorting" CellPadding="3" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate> 
                        <input type="checkbox" onclick="CheckAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("ContactId") %>' contactroleid='<%# Eval("ContactRoleId") %>'  ContactRole='<%# Eval("ContactRole") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Role" SortExpression="ContactRole" HeaderStyle-Width="100">
                    <ItemTemplate>
                        <%# Eval("ContactRole")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" Name" SortExpression="ContactsName" ItemStyle-Wrap="false"
                    ItemStyle-Width="200">
                    <ItemTemplate>
                        <%# Eval("ContactsName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Home Phone" SortExpression="HomePhone" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("HomePhone")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cell Phone" SortExpression="CellPhone" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("CellPhone")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Business Phone" SortExpression="BusinessPhone" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("BusinessPhone")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email" SortExpression="Email" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("Email")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>

    <div id="divAddContact" title="Loan Contact Setup" style="display: none;">
        <iframe id="ifrLoanContactAdd" frameborder="0" scrolling="no" width="720px" height="280px">
        </iframe>
    </div>
    <div id="divEditContact" title="Loan Contact Setup" style="display: none;">
        <iframe id="ifrLoanContactEdit" frameborder="0" scrolling="no" width="720px" height="280px">
        </iframe>
    </div>
    <div id="divLoanReassign" title="Loan Reassign" style="display: none;">
        <iframe id="ifrLoanReassign" frameborder="0" scrolling="no" width="600px" height="280px">
        </iframe>
    </div>
    <div id="divContactReassign" title="Contact Reassign" style="display: none;">
        <iframe id="ifrContactReassign" frameborder="0" width="630px" height="440px"></iframe>
    </div>

    <div id="divSendEmail" title="Send Email" style="display: none;">
        <iframe id="ifrSendEmail" frameborder="0" scrolling="no" width="605px" height="530px">
        </iframe>
    </div>

    <ul id="divDisposeMenu" class="jqcontextmenu">
        <li>
            <asp:LinkButton ID="lbtnSendNow" runat="server" OnClientClick="return BeforeSendNow();" OnClick="lbtnSendNow_Click">Send Now</asp:LinkButton></li>
        <li>
            <asp:LinkButton ID="lbtnSendDaily" runat="server"  OnClientClick="return BeforeSendNow();" OnClick="lbtnSendDaily_Click">Send Daily</asp:LinkButton></li>
        <li>
            <asp:LinkButton ID="lbtnSendWeekly" runat="server" OnClientClick="return BeforeSendNow();" OnClick="lbtnSendWeekly_Click">Send Weekly</asp:LinkButton></li>
        <li>
            <asp:LinkButton ID="lbtnDisable" runat="server" OnClientClick="return BeforeSendNow();" OnClick="lbtnDisable_Click">Disable</asp:LinkButton></li>
    </ul>
    <ul id="divAddMenu" class="jqcontextmenu" style="width: 80px;">
        <li>
            <a id="aAssignLoanContact" href="javascript:aAssignLoanContact_onclick()">Contact</a>
        </li>
        <li>
            <a id="aAssignLoanTeam" href="javascript:aAssignLoanTeam_onclick()">User</a>
        </li>
    </ul>
    <asp:HiddenField ID="hdnActiveLoan" runat="server" />
    <asp:HiddenField ID="hfdFileId" runat="server" />
    <asp:HiddenField ID="hfContactIDs" runat="server" />
    </form>
</body>
</html>
