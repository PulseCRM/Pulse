<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Contact Management - Partner Contacts" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master"
    AutoEventWireup="true" Inherits="PartnerContactsPage" CodeBehind="PartnerContactsPage.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../css/jqcontextmenu.css" />
    <script type="text/javascript" src="../js/jqcontextmenu.js"></script>
    <script language="javascript" type="text/javascript">
        var gridId = "#<%=gvPartnerContacts.ClientID %>";

        var sHasCreate = "<%=sHasCreate %>";
        var sHasModify = "<%=sHasModify %>";
        var sHasView = "<%=sHasView %>";
        var sHasAssign = "<%=sHasAssign %>";
        var sHasMerge = "<%=sHasMerge %>";

        $(document).ready(function () {

            if (sHasCreate == "0") {
                DisableLink("aCreate");
            }
            if (sHasModify == "0") {
                DisableLink("aUpdate");
            }
            if (sHasAssign == "0") {
                DisableLink("aAssign");
            }
            if (sHasMerge == "0") {
                DisableLink("aMerge");
            }
            if (sHasView == "0") {
                DisableLink("btnDetails");
            }
            // Alphabet
            var Alphabet = GetQueryString1("Alphabet");
            if (Alphabet != "") {

                $("#ddlAlphabet").val(Alphabet);
            }

            // ServiceType
            var ServiceType = GetQueryString1("ServiceType");
            if (ServiceType != "") {

                $("#<%=ddlServiceType.ClientID %>").val(ServiceType);
            }

            // ServiceType
            var CompanyID = GetQueryString1("CompanyID");
            if (CompanyID != "") {

                $("#<%=ddlCompanies.ClientID %>").val(CompanyID);
            }

            // BranchID
            var BranchID = GetQueryString1("BranchID");
            if (BranchID != "") {

                $("#<%=ddlBranchs.ClientID %>").val(BranchID);
            }

            // Referral
            var Referral = GetQueryString1("Referral");
            if (Referral != "") {

                $("#<%=ddlReferral.ClientID %>").val("Top 10 Referrals");
            }

            //var checkItems=
            var checkAll = $(gridId + " :checkbox:eq(0)");
            checkAll.click(function () {
                var allStatus = checkAll.attr("checked");
                $(gridId + " :checkbox:gt(0)").each(function () {
                    $(this).attr("checked", allStatus);
                });
                getSelectedItems();
            });
            //
            $(gridId + " :checkbox:gt(0)").each(function () {
                $(this).unbind("click").click(function () {
                    if ($(this).attr("checked") == false) {
                        checkAll.attr("checked", false);
                    }
                    getSelectedItems();
                });
            });

            $("#btnDetails").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }

                var items = $("#<%=hfDeleteItems.ClientID %>").val().split(",");
                if (items.length > 1) {
                    alert("Only one record can be selected for this operation..");
                    return false;
                }
                //                fieldid=2&fieldids=1,2,3,4,5,6

                var selctedItems = new Array();
                $(gridId + " :checkbox:gt(0)").each(function () {
                    var item = $(this);
                    selctedItems.push(item.attr("tag"));
                });
                window.location.href = 'PartnerContactDetailView.aspx?FromPage=<%= FromURL %>&ContactID=' + $("#<%=hfDeleteItems.ClientID %>").val();
            });


            $("#aCreate").click(function () {

                ShowDialog_AddContact();

                return;
            });

            $("#aUpdate").click(function () {

                ShowDialog_UpdateContact();

                return;
            });

            $("#aSearch").click(function () {

                ShowDialog_Search();

                return;
            });

            $("#aAssign").click(function () {

                ShowDialog_AssignContact();

                return;
            });

            $("#aMerge").click(function () {

                ShowDialog_MergeContact();

                return;
            });
        });


        function getSelectedItems() {
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            $("#<%=hfDeleteItems.ClientID %>").val(selctedItems.join(","));
        }

        function ShowDialog_AddContact() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 980
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 680;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowDialog_UpdateContact() {

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }

            var items = $("#<%=hfDeleteItems.ClientID %>").val().split(",");
            if (items.length > 1) {
                alert("Only one record can be selected for this operation..");
                return false;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&ContactID=" + $("#<%=hfDeleteItems.ClientID %>").val() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

           

            var BaseWidth = 920
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 780;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function ShowDialog_Search() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SearchContactsPopup.aspx?sid=" + RadomStr + "&GetSearchCondition=window.parent.GetSearchCondition&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 600
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 240;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Search", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowDialog_MergeContact() {

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "MergeContactsPopup.aspx?sid=" + RadomStr + "&contacts=" + $("#<%=hfDeleteItems.ClientID %>").val() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 700;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Merge", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function ShowDialog_AssignContact() {

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "AssignContactAccess.aspx?sid=" + RadomStr + "&contacts=" + $("#<%=hfDeleteItems.ClientID %>").val() + "&CloseDialogCodes=";

            var BaseWidth = 700;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Assign Contact", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function GetSearchCondition(ContactIDs) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "";

            // Alphabet 
            var Alphabet = $("#ddlAlphabet").val();
            if (Alphabet != "") {

                sQueryStrings += "&Alphabet=" + encodeURIComponent(Alphabet);
            }

            // ServiceType
            var ServiceType = $("#<%=ddlServiceType.ClientID %>").val();
            if (ServiceType != "") {

                sQueryStrings += "&ServiceType=" + encodeURIComponent(ServiceType);
            }

            // Company
            var CompanyID = $("#<%=ddlCompanies.ClientID %>").val();
            if (CompanyID != "") {

                sQueryStrings += "&CompanyID=" + encodeURIComponent(CompanyID);
            }

            // BranchID
            var BranchID = $("#<%=ddlBranchs.ClientID %>").val();
            if (BranchID != "") {

                sQueryStrings += "&BranchID=" + encodeURIComponent(BranchID);
            }

            // Referral
            var Referral = $("#<%=ddlReferral.ClientID %>").val();
            if (Referral != "All") {

                sQueryStrings += "&Referral=Top10";
            }

            // PageIndex
            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "" && PageIndex != "1") {

                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            // ContactIDs
            //            var ContactIDs = GetQueryString1("ContactIDs");
            if (ContactIDs != "") {

                sQueryStrings += "&ContactIds=" + encodeURIComponent(ContactIDs);
            }



            if (sQueryStrings == "") {

                window.location.href = window.location.pathname;

            }
            else {

                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
        }

        function btnFilter_onclick() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "";

            // ServiceType
            var ServiceType = $("#<%=ddlServiceType.ClientID %>").val();
            if (ServiceType != "" && ServiceType != "0") {

                sQueryStrings += "&ServiceType=" + encodeURIComponent(ServiceType);
            }

            // Company
            var CompanyID = $("#<%=ddlCompanies.ClientID %>").val();
            if (CompanyID != "" && CompanyID != "0") {

                sQueryStrings += "&CompanyID=" + encodeURIComponent(CompanyID);
            }

            // BranchID
            var BranchID = $("#<%=ddlBranchs.ClientID %>").val();
            if (BranchID != "" && BranchID != "0") {

                sQueryStrings += "&BranchID=" + encodeURIComponent(BranchID);
            }

            // Referral
            var Referral = $("#<%=ddlReferral.ClientID %>").val();
            if (Referral != "All") {

                sQueryStrings += "&Referral=Top10";
            }

            // ddlAlphabet
            var Alphabet = $("#ddlAlphabet").val();
            if (Alphabet != "") {

                sQueryStrings += "&Alphabet=" + encodeURIComponent(Alphabet);
            }
            // Assign UserIDs
            var sSelUserIDs = $("#<%=hiSelAssignUserIDs.ClientID %>").val();

            if (sSelUserIDs != "") {
                sQueryStrings += "&AssignUserIDs=" + sSelUserIDs;
            }
            // Sel Contact
            var selctedItems = $("#<%=hfDeleteItems.ClientID %>").val();
            if (selctedItems != "") {
                sQueryStrings += "&SelContactIDs=" + selctedItems;
            }

            // PageIndex
            var PageIndex = 1; //GetQueryString1("PageIndex");
            if (PageIndex != "" && PageIndex != "1") {

                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            // ContactIDs
            var ContactIDs = GetQueryString1("ContactIds");
            if (ContactIDs != "") {

                sQueryStrings += "&ContactIds=" + ContactIDs;
            }
            if (sQueryStrings == "") {

                window.location.href = window.location.pathname;

            }
            else {

                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
        }

        function AssignContactAccessPopupSelected(sSelUserIDs) {

            $("#<%=hiSelAssignUserIDs.ClientID %>").val(sSelUserIDs);

            btnFilter_onclick();

        }

        function GoToContactDetail(ContactID) {

            if (sHasView != "1") {

                alert("You have no privilege to do this operation.");
                return;
            }
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            window.location.href = "PartnerContactDetailView.aspx?sid=" + RadomStr + "&ContactID=" + ContactID;

        }

        function GoToUpdateContactBranch(BranchId) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerBranchEdit.aspx?sid=" + RadomStr + "&ContactBranchId=" + BranchId;
        }

        function GoToServiceType(ServiceTypeID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerServiceTypeSetup.aspx?sid=" + RadomStr + "&ServiceTypeID=" + ServiceTypeID;
        }

        //#region Bug840 neo

        //#region show/close waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $.blockUI({ message: $('#divWaiting'), css: { width: '500px'} });
        }

        //#endregion

        function aDelete_onclick() {

            var SelCount = $(gridId + " tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No record has been selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one record can be selected.");
                return;
            }

            var result = confirm("Deleting a partner contact will also delete the history information.\r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return;
            }

            ShowWaitingDialog("Checking if partner contact's being referenced...");

            var ContactID = $(gridId + " tr:not(:first) td :checkbox:checked").attr("tag");

            // Ajax - check whether contact is ref. or not
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);
            $.getJSON("CheckContactRef.ashx?sid=" + Radom + "&ContactID=" + ContactID, AfterCheckContactRef);
        }

        function AfterCheckContactRef(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    $.unblockUI();
                    return;
                }

                if (data.IsRef == "true") {

                    alert("The current partner contact has been assigned roles in the loans. You will need to assign another partner contact to replace him/her in the loans.");

                    $.unblockUI();

                    ShowDialog_AssignContactBeforeDelete();
                }
                else {

                    DeleteContact();
                }

            }, 2000);
        }

        function ShowDialog_AssignContactBeforeDelete() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var DelContactID = $(gridId + " tr:not(:first) td :checkbox:checked").attr("tag");

            var iFrameSrc = "AssignContactBeforeDelete.aspx?DelContactID=" + DelContactID + "&sid=" + RadomStr;

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 510;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Search and Assign Contact", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function DeleteContact() {

            ShowWaitingDialog("Deleting...");

            var DelContactID = $(gridId + " tr:not(:first) td :checkbox:checked").attr("tag");
            $.getJSON("DeletePartnerContact_Background.aspx?DelContactID=" + DelContactID, AfterDeleteContact);
        }

        function AfterDeleteContact(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    $.unblockUI();
                    return;
                }

                alert("Deleted selected contact successfully.");

                window.location.href = window.location.href;

            }, 2000);
        }

        //#endregion

        function GoToLoanDetails(FileIDs) {

            if (FileIDs == "") {

                alert("There is no loan that the partner contact has as referral.");
                return;
            }

            var FirstFileID = "";
            var Pos = FileIDs.indexOf(",");
            if (Pos == -1) {

                FirstFileID = FileIDs;
            }
            else {

                FirstFileID = FileIDs.substr(0, Pos);
            }
            //alert(FirstFileID);

            window.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=" + encodeURIComponent(window.location.href) + "&fieldid=" + FirstFileID + "&fieldids=" + FileIDs;
        }

    </script>

    <script language="javascript" type="text/javascript">
     
        jQuery(document).ready(function ($) {

            $('#aMailChimp').addcontextmenu('divMailChimpMenu') //apply context menu to all images on the page 
        })

        function CloseWaitingDialog() {

            $.unblockUI();
        }

        function Subscribe() {

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            var ContactIDs = selctedItems.join(",");
             
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var p = "";
            if ($.browser.msie == true) {   // ie

                p = "window.parent.SubscribeMailChimp";
            }
            else {   // firefox

                p = "window.parent.SubscribeMailChimp";
            }

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + ContactIDs + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            return false;
        }

        function SubscribeMailChimp(LID) {

            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            var ContactIDs = selctedItems.join(",");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            CloseGlobalPopup();
            ShowWaitingDialog("Please wait...");

            // check exist
            $.getJSON("../Prospect/ProspectMailChimp_CheckExist_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactIDs + "&LID=" + LID, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        CloseWaitingDialog();
                        return;
                    }

                    var RadomNum2 = Math.random();
                    var Radom2 = RadomNum2.toString().substr(2);
                    // Ajax
                    $.getJSON("../Prospect/ProspectMailChimp_Subscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactIDs + "&LID=" + LID, AfterSubscribeMailChimp);

                }, 2000);
            });
        }

        function AfterSubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Completed.");

                    CloseWaitingDialog();
                    window.location.href = window.location.href;
                }
                CloseWaitingDialog();

            }, 2000);
        }

        function Unsubscribe() {

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            var ContactIDs = selctedItems.join(",");
            
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var p = "";
            if ($.browser.msie == true) {   // ie

                p = "window.parent.UnsubscribeMailChimp";
            }
            else {   // firefox

                p = "window.parent.UnsubscribeMailChimp";
            }

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + ContactIDs + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }


        function UnsubscribeMailChimp(LID) {

            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            var ContactIDs = selctedItems.join(",");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            CloseGlobalPopup();
            ShowWaitingDialog("Please wait...");
            // Ajax
            $.getJSON("../Prospect/ProspectMailChimp_Unsubscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactIDs + "&LIDs=" + LID, AfterUnsubscribeMailChimp);

        }

        function AfterUnsubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Completed.");

                    CloseWaitingDialog();
                    window.location.href = window.location.href;
                }
                CloseWaitingDialog();

            }, 2000);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle">Contact Management - Partner Contacts</div>
    <div class="SplitLine">
    </div>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlServiceType" runat="server" Width="150px" DataTextField="Name"
                            DataValueField="ServiceTypeId">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlCompanies" runat="server" Width="150px" DataTextField="Name"
                            DataValueField="ContactCompanyId">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlBranchs" runat="server" Width="150px" DataTextField="Name"
                            DataValueField="ContactBranchId">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                    	<asp:DropDownList ID="ddlReferral" runat="server" Width="150px">
                    		<asp:ListItem>All</asp:ListItem>
                    		<asp:ListItem>Top 10 Referrals</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <input id="btnFilter" type="button" value="Filter" class="Btn-66" onclick="btnFilter_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 13px; width: 950px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 40px;">
                        <select id="ddlAlphabet" style="width: 40px;" onchange="btnFilter_onclick()">
                            <option value=''></option>
                            <option value="A">A</option>
                            <option value="B">B</option>
                            <option value="C">C</option>
                            <option value="D">D</option>
                            <option value="E">E</option>
                            <option value="F">F</option>
                            <option value="G">G</option>
                            <option value="H">H</option>
                            <option value="I">I</option>
                            <option value="J">J</option>
                            <option value="K">K</option>
                            <option value="L">L</option>
                            <option value="M">M</option>
                            <option value="N">N</option>
                            <option value="O">O</option>
                            <option value="P">P</option>
                            <option value="Q">Q</option>
                            <option value="R">R</option>
                            <option value="S">S</option>
                            <option value="T">T</option>
                            <option value="U">U</option>
                            <option value="V">V</option>
                            <option value="W">W</option>
                            <option value="X">X</option>
                            <option value="Y">Y</option>
                            <option value="Z">Z</option>
                        </select>
                    </td>
                    <td>
                        <ul class="ToolStrip">
                            <li><a id="btnDetails" href="" onclick="return false;">Detail</a><span>|</span></li>
                            <li><a id="aCreate" href="" onclick="return false;">Create</a><span>|</span></li>
                            <li><a id="aUpdate" href="" onclick="return false;">Update</a><span>|</span></li>
                            <li>
                                <a id="aDelete" href="javascript:aDelete_onclick()">Delete</a><span>|</span>
                            </li>
                            <li><a id="aAssign" href="" onclick="return false;">Assign</a><span>|</span></li>
                            <li><a id="aMerge" href="" onclick="return false;">Merge</a><span>|</span></li>
                            <li><a id="aSearch"  href="" onclick="return false;">Search</a></li>
                            <li><span>|</span><a id="aMailChimp" href="#">Mail Chimp</a></li>
                        </ul>
                    </td>                
                    <td colspan="2" style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                            CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divContactList" class="ColorGrid" style="margin-top: 5px; width: 950px;">
        <asp:GridView ID="gvPartnerContacts" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
            OnSorting="gvPartnerContacts_Sorting" CellPadding="3" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <input type="checkbox" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("ContactId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Contact " SortExpression="ContactName" ItemStyle-Width="265px">
                    <ItemTemplate>
                        <a href="javascript:GoToContactDetail('<%# Eval("ContactId")%>')">
                            <%# Eval("ContactName")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Branch " SortExpression="BranchName" ItemStyle-Width="205px">
                    <ItemTemplate>
                        <a href="javascript:GoToUpdateContactBranch('<%# Eval("ContactBranchId")%>')">
                            <%# Eval("BranchName")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Service Type " SortExpression="ServiceType" ItemStyle-Width="150px">
                    <ItemTemplate>
                        <a href="javascript:GoToServiceType('<%# Eval("ServiceTypeID")%>')">
                            <%# Eval("ServiceType")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Referral $" SortExpression="TotalReferral" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <a href="javascript:GoToLoanDetails('<%# Eval("TotalReferralFileIDs")%>')"><%# Eval("TotalReferral") == DBNull.Value ? string.Empty : Convert.ToDecimal(Eval("TotalReferral")).ToString("c0") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Referral Funded $" SortExpression="TotalReferralFunded" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <a href="javascript:GoToLoanDetails('<%# Eval("TotalReferralFundedFileIDs")%>')"><%# Eval("TotalReferralFunded") == DBNull.Value ? string.Empty : Convert.ToDecimal(Eval("TotalReferralFunded")).ToString("c0") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Win Ratio" SortExpression="WinRatio" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# Eval("WinRatio") == DBNull.Value ? string.Empty : Convert.ToDecimal(Eval("WinRatio")).ToString("p2")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" Enable" SortExpression="Enabled" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <%# Eval("Enabled").ToString().ToLower() == "true" ? "Yes" : "No" %>                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    <div id="divWaiting" style="display: none; padding: 5px;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting" src="../images/waiting.gif" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
				</td>
			</tr>
		</table>
	</div>
        <ul id="divMailChimpMenu" class="jqcontextmenu">
            <li>
                <asp:LinkButton ID="lbtnSubscribe" runat="server" OnClientClick="Subscribe();return false;">Subscribe</asp:LinkButton></li>
            <li>
                <asp:LinkButton ID="lbtnUnsubscribe" runat="server" OnClientClick="Unsubscribe();return false;">Unsubscribe</asp:LinkButton></li>
        </ul>
    <asp:HiddenField ID="hfDeleteItems" runat="server" />
    <asp:HiddenField ID="hiSelAssignUserIDs" runat="server" />
</asp:Content>
