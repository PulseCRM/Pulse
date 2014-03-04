<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerBranchEdit.aspx.cs" Inherits="Contact_PartnerBranchEdit" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/jqcontextmenu.css" />
    <script type="text/javascript" src="../js/jqcontextmenu.js"></script>

            <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.formatCurrency.js" type="text/javascript"></script>

    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">
        // <!CDATA[
        var gridId = "#<%=gridContactList.ClientID %>";
        var sHasCreate = "<%=sHasCreate %>";
        var sHasModify = "<%=sHasModify %>";
        var sHasDelete = "<%=sHasDelete %>";
        var sHasAssign = "<%=sHasAssign %>";
        var sHasView = "<%=sHasView %>";
        var sHasMerge = "<%=sHasMerge %>";

        jQuery(document).ready(function ($) {

            $('#aMailChimp').addcontextmenu('divMailChimpMenu') //apply context menu to all images on the page 

           
        })

        $(document).ready(function () {

            InitSearchInput();

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



            //AddValidators();


            //txtBizPhone
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbPhone").mask("(999) 999-999?9");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbFax").mask("(999) 999-999?9");

            $("[required]").bind("change blur",function () {

                if ($(this).val() == "") {
                    $("[sid=" + $(this).attr("cid") + "]").show();
                    $(this).attr("rqd", "0");
                }
                else {
                    $("[sid=" + $(this).attr("cid") + "]").hide();
                    $(this).attr("rqd", "1");
                }

            });

            $("[cid='btnSave']").click(function () {
                $.each($("[required]"), function () {
                    $(this).blur();
                });
                if ($("[required][rqd=1]").size() >= 6) {
                    return true;
                }
                else {

                    return false;
                }

            });

            

            $("#<%= btnRemove.ClientID %>").click(function () {
                var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridContactList tr td :checkbox:checked").length;
                if (SelCount == 0) {

                    alert("No partner contact was selected.");
                    return false;
                }
                return true;
            });

            $("#<%= btnDisable.ClientID %>").click(function () {
                var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridContactList tr td :checkbox:checked").length;
                if (SelCount == 0) {

                    alert("No partner contact was selected.");
                    return false;
                }
                return true;
            });

            $("#<%= btnDeleteContact.ClientID %>").click(function () {
                var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridContactList tr td :checkbox:checked").length;
                if (SelCount == 0) {

                    alert("No partner contact was selected.");
                    return false;
                }
                return true;
            });
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

        });

        function InitSearchInput() {

            // Alphabet
            var Alphabet = GetQueryString1("Alphabet");
            if (Alphabet != "") {

                $("#ddlAlphabet").val(Alphabet);
            }
        }

        // add jQuery Validators
        function AddValidators() {

            $("#aspnetForm").validate({

                rules: {

                    ctl00$ctl00$PlaceHolderMain$MainArea$tbBranch: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbAddress: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbCity: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlStates: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbZip: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbPhone: {
                        required: true
                    }
//                    ,
//                    ctl00$ctl00$PlaceHolderMain$MainArea$tbFax: {
//                        required: true
//                    }
                },
                messages: {

                    ctl00$ctl00$PlaceHolderMain$MainArea$tbBranch: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbAddress: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbCity: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlStates: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbZip: {
                        required: "*"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbPhone: {
                        required: "*"
                    }
//                    ,
//                    ctl00$ctl00$PlaceHolderMain$MainArea$tbFax: {
//                        required: "*"
//                    }
                }
            });
        }

        function btnFilter_onclick() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "";

            // Alphabet
            var Alphabet = $("#ddlAlphabet").val();
            if (Alphabet != "") {

                sQueryStrings += "&Alphabet=" + encodeURIComponent(Alphabet);
            }

            // PageIndex
            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "" && PageIndex != "1") {

                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            // Assign UserIDs
            var sSelUserIDs = $("#<%=hiSelAssignUserIDs.ClientID %>").val();
            var sContactBranchID = "<%=sContactBranchID %>";
            if (sSelUserIDs != "") {
                sQueryStrings += "&AssignUserIDs=" + sSelUserIDs;
            }
            // Sel Contact
            var selctedItems = $("#<%=hiSelectedContact.ClientID %>").val();
            if (selctedItems != "") {
                sQueryStrings += "&SelContactIDs=" + selctedItems;
            }

            var sContactBranchID = "<%=sContactBranchID %>";
            if (sContactBranchID != "") {
                sQueryStrings += "&ContactBranchId=" + sContactBranchID;
            }


            if (sQueryStrings == "") {

                window.location.href = window.location.pathname;
            }
            else {

                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "");
            }
        }

        function aCreate_onclick() {

            window.location.href = "PartnerBranchSetup.aspx";
        }

        function aUpdate_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one partner branch can be selected.");
                return;
            }

            var BranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").attr("BranchID");

            GoToUpdateContactBranch(BranchID);
        }

        function GoToUpdateContactBranch(BranchId) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerBranchEdit.aspx?sid=" + RadomStr + "&ContactBranchId=" + BranchId;
        }

        function BeforeSave() {
            //            //  CheckDuplication();
            //            var BranchName = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_tbBranch").val());
            //            var Company = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCompany").val());
            ////            alert("BranchName=" + BranchName);
            ////            alert("Company=" + Company);
            //            if (BranchName == "") {
            //                alert("Plesae enter branch name.");
            //                return false;
            //            }

            //            if (Company == "0") {
            //                alert("Plesae select a company.");
            //                return false;
            //            }
            //            return true;
        }

        function BeforeDelete() {

            var result = confirm("Deleting the partner branch will also delete the branch and the associated contacts and history records. Are you sure you want to continue?");
            if (result == false) {

                return false;
            }

            return true;
        }

        function add_onclick() {

            OpenSelectContact('add');
        }

        function remove_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridContactList tr td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner contact was selected.");
                return false;
            }
            return true;
        }

        function OpenSelectContact(stype) {
            var BranchID = "<%=sContactBranchID %>";

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SelectContactsPopup.aspx?ContactBranchId=" + BranchID + "&type=" + stype + "&t=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 800
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 680;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function closeDialog() {
            $("#dialogSelectContact").dialog('destroy');
            window.location.hre = window.location.href;
        }



        function getSelectedItems() {
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            $("#<%=hiSelectedContact.ClientID %>").val(selctedItems.join(","));
        }

        function ShowDialog_AddContact() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 700
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowDialog_UpdateContact() {

            if ($("#<%=hiSelectedContact.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return;
            }

            var items = $("#<%=hiSelectedContact.ClientID %>").val().split(",");
            if (items.length > 1) {
                alert("Only one record can be selected for this operation..");
                return;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&ContactID=" + $("#<%=hiSelectedContact.ClientID %>").val() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 700
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function ShowDialog_MergeContact() {

            if ($("#<%=hiSelectedContact.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return;
            }

            var items = $("#<%=hiSelectedContact.ClientID %>").val().split(",");
            if (items.length > 1) {
                alert("Only one record can be selected for this operation..");
                return;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "MergeContactsPopup.aspx?sid=" + RadomStr + "&contacts=" + $("#<%=hiSelectedContact.ClientID %>").val() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 700
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Merge", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function ShowDialog_AssignContact() {
            if ($("#<%=hiSelectedContact.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "AssignContactAccess.aspx?sid=" + RadomStr + "&CloseDialogCodes=";

            var BaseWidth = 700;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Assign Contact", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function AssignContactAccessPopupSelected(sSelUserIDs) {
            $("#<%=hiSelAssignUserIDs.ClientID %>").val(sSelUserIDs);
            btnFilter_onclick();
        }

        function LinkContact(sContactID) {
            if (sHasView == "0") {
                alert("You have no privilege to do this operation.");
                return;
            }
            window.location.href = 'PartnerContactDetailView.aspx?ContactID=' + sContactID;
        }

        //#region show/close waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $.blockUI({ message: $('#divWaiting'), css: { width: '500px'} });
        }

        function CloseWaitingDialog() {

            $.unblockUI();
        }

        //#endregion

        function Subscribe() {

            if ($("#<%=hiSelectedContact.ClientID %>").val() == "") {

                alert("Please select a contact from the list.");
                return false;
            }

            var ContactIDs = $("#<%=hiSelectedContact.ClientID %>").val();

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

            var ContactIDs = $("#<%=hiSelectedContact.ClientID %>").val();

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

            if ($("#<%=hiSelectedContact.ClientID %>").val() == "") {

                alert("Please select a contact from the list.");
                return false;
            }

            var ContactIDs = $("#<%=hiSelectedContact.ClientID %>").val();

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

            var ContactIDs = $("#<%=hiSelectedContact.ClientID %>").val();

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

        // ]]>
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer">
        <div id="divModuleName" class="Heading">Partner Branch Setup</div>
        <div class="SplitLine"></div>
        <div id="divFilters" style="margin-top: 20px;">
            <table >
                <tr>
                    <td style="width: 120px; height:23px" align="left">
                       &nbsp;&nbsp;&nbsp;&nbsp;Company:
                    </td>
                    <td style="width: 420px;" colspan="4">
                        <asp:DropDownList ID="ddlCompany" runat="server" DataValueField="ContactCompanyId" DataTextField="Name" Width="400px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 110px;">
                       &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                       &nbsp;&nbsp;&nbsp;&nbsp;Branch Name:
                    </td>
                    <td style="width: 420px;" colspan="4">
                        <asp:TextBox ID="tbBranch"  cid="tbBranch" required  runat="server" style=" width:400px" ></asp:TextBox>
                        <span sid="tbBranch" style=" display:none; color:Red">*</span>
                    </td>
                    <td style="width: 110px;">
                        <asp:CheckBox ID="chkEnable" runat="server"  />&nbsp;&nbsp;Enabled
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;Address:
                    </td>
                    <td style="width: 420px;" colspan="4">
                        <asp:TextBox ID="tbAddress" cid="tbAddress" required  runat="server" style=" width:400px" ></asp:TextBox>
                        <span sid="tbAddress" style=" display:none; color:Red">*</span>
                    </td>
                    <td style="width: 110px;">
                        &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;City:
                    </td>
                    <td style="width: 160px;">
                        <asp:TextBox ID="tbCity" cid="tbCity" required  runat="server" style=" width:140px" ></asp:TextBox>
                        <span sid="tbCity" style=" display:none; color:Red">*</span>
                    </td>
                     <td style="width: 60px;" align="left">
                      &nbsp;&nbsp;State:
                    </td>
                    <td style="width: 110px;">
                        <asp:DropDownList ID="ddlStates" cid="ddlStates" required  runat="server"  Width="80px">
                        </asp:DropDownList>
                        <span sid="ddlStates" style=" display:none; color:Red">*</span>
                    </td>
                     <td style="width: 60px;" align="left">
                      &nbsp;&nbsp;Zip:
                    </td>
                    <td style="width: 110px;">
                       <asp:TextBox ID="tbZip" cid="tbZip" required  runat="server" style=" width:80px" ></asp:TextBox>
                       <span sid="tbZip" style=" display:none; color:Red">*</span>
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;Phone:
                    </td>
                    <td style="width: 160px;">
                        <asp:TextBox ID="tbPhone"  cid="tbPhone" required   runat="server" style=" width:140px" ></asp:TextBox>
                        <span sid="tbPhone" style=" display:none; color:Red">*</span>
                    </td>
                     <td style="width: 60px;" align="left">
                      &nbsp;&nbsp;Fax:
                    </td>
                    <td style="width: 160px;" colspan="2">
                        <asp:TextBox ID="tbFax" runat="server" style=" width:150px" ></asp:TextBox>
                    </td>
                    <td style="width: 100px;">
                       &nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:23px" align="left">
                      &nbsp;&nbsp;&nbsp;&nbsp;Primary Contact:
                    </td>
                    <td style="width: 240px;" colspan="2">
                         <asp:DropDownList ID="ddlContact" runat="server" DataValueField="ContactId" DataTextField="Contact"  Width="230px">
                        </asp:DropDownList>
                    </td>
                     <td style="width: 80px;" align="left" colspan="3">
                      &nbsp;&nbsp;
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px; height:25px" align="right">
                      <asp:Button ID="btnSave" runat="server" cid="btnSave" Text ="Save" CssClass="Btn-66"   OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td  colspan="5" align="left" >
                       <asp:Button ID="btnDelete" runat="server" Text ="Delete" CssClass="Btn-66"  OnClientClick="return BeforeDelete();" OnClick="btnDelete_Click" />
                    </td>
                </tr>

            </table>
        </div>
         <div id="divToolBar" style="width: 900px; margin-top: 20px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50px;">
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
                    <td style="width: 650px;">
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aCreate" href="javascript:ShowDialog_AddContact()">Create</a><span>|</span></li>
                            <li><a id="aUpdate" href="javascript:ShowDialog_UpdateContact()">Update</a><span>|</span></li>
                            <li><asp:LinkButton ID="btnDisable" runat="server" Text="Disable" OnClick="btnDisable_Click"></asp:LinkButton><span>|</span></li>
                            <li><asp:LinkButton ID="btnDeleteContact" runat="server" Text="Delete" OnClick="btnDeleteContact_Click"></asp:LinkButton><span>|</span></li>
                            <li><a id="a1" href="javascript:add_onclick()">Add</a><span>|</span></li>
                            <li><asp:LinkButton ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click"></asp:LinkButton><span>|</span></li>
                            <li><a id="aAssign" href="javascript:ShowDialog_AssignContact()">Assign</a><span>|</span></li>
                            <li><a id="aMerge" href="javascript:ShowDialog_MergeContact()">Merge</a></li>
                            <li><span>|</span><a id="aMailChimp" href="#">Mail Chimp</a></li>
                            <input type="button" style="display:none" onclick="" />
                        </ul>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                            CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divCompanyList" class="ColorGrid" style="margin-top: 5px; width: 700px;">
            <asp:GridView ID="gridContactList" runat="server" DataSourceID="BranchSqlDataSource" AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox"  tag='<%# Eval("ContactId") %>'  />
                            </ItemTemplate>
                            <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                            <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact" SortExpression="Contact">
                            <ItemTemplate>
                                <a href="javascript:LinkContact('<%# Eval("ContactId") %>')"><%# Eval("Contact")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone" SortExpression="CellPhone">
                            <ItemTemplate>
                                <%# Eval("CellPhone")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Email" SortExpression="Email">
                            <ItemTemplate>
                                <%# Eval("Email")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled">
                            <ItemTemplate>
                                <%# Eval("Enabled")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Access" SortExpression="UserAccess">
                            <ItemTemplate>
                                <%# Eval("UserAccess")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:SqlDataSource ID="BranchSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
            <SelectParameters>
                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="Contact" />
                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select ContactId,ISNULL(Lastname,'')+ (CASE ISNULL(Lastname,'') when '' then ' ' else ', ' end) +ISNULL(Firstname,'') as Contact,
Fax,Email,CellPhone,HomePhone,ContactBranchId,case when ISNULL([Enabled],1)=1 then 'Yes' else 'No' end as [Enabled],(select COUNT(UserId) from ContactUsers where ContactUsers.ContactId=Contacts.ContactId) as UserAccess
from Contacts) t" />
                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="15" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:HiddenField ID="hiSelectedContact" runat="server" />
        <asp:HiddenField ID="hiSelAssignUserIDs" runat="server" />
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
          	<a href="aSubscribe" onclick="Subscribe(); return false;">Subscribe</a></li>
          <li>
              <a href="aUnsubscribe" onclick="Unsubscribe(); return false;">Unsubscribe</a></li>
      </ul>
</asp:Content>
