<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Branch/Location Setup" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="LPWeb.Settings.Settings_BranchSetup" CodeBehind="BranchSetup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/PriviewImage.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
        .HiddenColomn
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            // add onchange event
            //   ctl00_PlaceHolderMain_MainArea_ddlBranchName
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranchName").change(ddlBranchName_onchange);

            // set max-length
            // $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtGroupDesc").maxlength(500);

            $("#<%=fuldWebLogo.ClientID %>").PreviewImage({ ImageClientId: "<%=Image1.ClientID %>", MaxWidth: "300", MaxHeight: "300" });

            AddValidators();


            $("[cid='default'] input").change(function () {

                var len = $("span[cid='default'] input[checked='checked']").size();
                //alert(len);

                if (len >= 1 && $(this).attr("checked") == true) {
                    var rval = window.confirm('There is already a "Default" Point folder selected. Do you want to deselect the other folder?');
                    if (rval != true) {


                        if ($(this).attr("checked") == true || $(this).attr("checked") == 'checked') {
                            $(this).attr("checked", "");
                        }
                        else {

                            $("[cid='default'] input").each(function (item) {
                                //alert("Item #" + i + ": " + n);
                                item.attr("checked", "");
                            });

                            $(this).attr("checked", "checked");

                        }
                        $(this).blur();
                        return;
                    }
                    else {

                        $.each($("[cid='default'] input"), function (i, item) {
                            //alert("Item #" + i + ": " + n);
                            $(item).attr("checked", "");
                        });
                        $(this).attr("checked", "checked");
                    }

                }


                //alert(rval);
                var hifId = "<%=hifId.ClientID %>";
                var btnSetDeault = "<%=btnSetDeault.ClientID %>";
                var id = $(this).parent().attr("title");
                var IsCancel = 'false';
                if ($(this).attr("checked") == "" || $(this).attr("checked") == "false") {
                    IsCancel = 'true';

                }
                $("#" + hifId).val(id + ":" + IsCancel);

                //alert($("#" + hifId).val());
                /*
                $("#" + btnSetDeault).trigger("click");
                */

            });

            $(" select[cid='DropDownList1'] ").change(function () {
                //1 Processing 
                if ($(this).val() == "1") {

                    //alert($(this).parent().parent().html());
                    $(this).parent().parent().find("[cid='default']").removeAttr("disabled")
                    $(this).parent().parent().find("[cid='default'] input").removeAttr("disabled");

                } else {
                    $(this).parent().parent().find("[cid='default']").attr("disabled", "disabled")
                    $(this).parent().parent().find("[cid='default'] input").attr("disabled", "disabled");
                }

            });


        });

        // add jQuery Validators
        function AddValidators() {
            var MarketingEnabled = $("<%=hdnMarketingEnabled.ClientID %>").val();
            if (MarketingEnabled == "1") {
                $("#aspnetForm").validate({

                    rules: {

                        ctl00$ctl00$PlaceHolderMain$MainArea$txbLicense1: {
                            required: true
                        },
                        ctl00$ctl00$PlaceHolderMain$MainArea$txbDisclaimer: {
                            required: true
                        }
                    },
                    messages: {

                        ctl00$ctl00$PlaceHolderMain$MainArea$txbLicense1: {
                            required: "*"
                        },
                        ctl00$ctl00$PlaceHolderMain$MainArea$txbDisclaimer: {
                            required: "*"
                        }
                    }
                });
            }
        }

        // onchange for Branch name list
        function ddlBranchName_onchange() {

            var SelectedBranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranchName").val();
            window.location.href = "BranchSetup.aspx?BranchID=" + SelectedBranchID;
        }

        // check/decheck all
        function CheckAll2(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

        // add member
        function AddPointMember() {

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox").attr("checked", "");

            // check exist members
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[title]").each(function (i) {
                var BranchID = $(this).attr("title");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderSelectionList tr td :checkbox[title=" + BranchID + "]").attr("checked", "true");
            });

            // show modal dialog
            $("#divBranchManagerSelection").dialog({
                height: 480,
                width: 440,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // remove member
        function RemovePointMember() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().remove();
        }

        // Enable Point folder member
        function EnablePointFolder() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }

            //2011-9-12 remove the restrictions
            //            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "2"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "3"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "4"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "5"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "7") {

            //                alert("Can only enable a \"Prospect\" or \"Processing\" folder.");
            //                return;
            //            }

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find(":input[id^='txtEnabled']").val("Yes")

            var EnabledFolderIDs = "";
            var DisabledFolderIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox").each(function () {

                var Enabled = $(this).parent().parent().find(":input[id^='txtEnabled']").val();
                var FolderID = $(this).attr("title");

                if (Enabled == "Yes") {
                    if (EnabledFolderIDs == "") {
                        EnabledFolderIDs = FolderID;
                    }
                    else {
                        EnabledFolderIDs += "," + FolderID;
                    }
                }
                else if (Enabled == "No") {
                    if (DisabledFolderIDs == "") {
                        DisabledFolderIDs = FolderID;
                    }
                    else {
                        DisabledFolderIDs += "," + FolderID;
                    }
                };
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnEnableFolderIDs").val(EnabledFolderIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnDisableFolderIDs").val(DisabledFolderIDs);
        }

        // remove member
        function DisablePointFolder() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }

            //            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "2"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "3"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "4"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "5"
            //            || $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find("td select option:selected").val() == "7") {

            //                alert("Can only disable a \"Prospect\" or \"Processing\" folder.");
            //                return;
            //            }

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox[checked=true]").parent().parent().find(":input[id^='txtEnabled']").val("No")

            var EnabledFolderIDs = "";
            var DisabledFolderIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox").each(function () {

                var Enabled = $(this).parent().parent().find(":input[id^='txtEnabled']").val();
                var FolderID = $(this).attr("title");

                if (Enabled == "Yes") {
                    if (EnabledFolderIDs == "") {
                        EnabledFolderIDs = FolderID;
                    }
                    else {
                        EnabledFolderIDs += "," + FolderID;
                    }
                }
                else if (Enabled == "No") {
                    if (DisabledFolderIDs == "") {
                        DisabledFolderIDs = FolderID;
                    }
                    else {
                        DisabledFolderIDs += "," + FolderID;
                    }
                };
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnEnableFolderIDs").val(EnabledFolderIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnDisableFolderIDs").val(DisabledFolderIDs);
        }

        // add member
        function AddMember() {

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox").attr("checked", "");

            // check exist members
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[title]").each(function (i) {
                var UserID = $(this).attr("title");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox[title=" + UserID + "]").attr("checked", "true");
            });

            // show modal dialog
            $("#dialog-modal").dialog({
                height: 480,
                width: 440,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // remove member
        function RemoveMember() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[checked=true]").parent().parent().remove();
        }
        // add user
        function btnPointSelectAdd_onclick() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderSelectionList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }

            // no data, add columns
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr th").length == 0) {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr:first").remove();
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderSelectionList tr:first").clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList");

            }

            // clear user list
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr:not(:first)").remove();

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderSelectionList tr td :checkbox[checked=true]").parent().parent().clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList");

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox").attr("checked", "");


            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr:not(:first)").each(function (i) {

                var iTitle = $(this).find("td :checkbox").attr("title");
                var iCheckBox = $(this).find("td :checkbox");
                var iSelect = $(this).find("td select");

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderSelectionList tr:not(:first)").each(function (j) {
                    if ($(this).find("td :checkbox").attr("title") == iTitle) {
                        iSelect.val($(this).find("td select option:selected").val()); ;
                    }
                });

            });
            // close modal
            $("#divBranchManagerSelection").dialog("close");
        }

        // popup cancel
        function btnPointSelectCancel_onclick() {

            // close modal
            $("#divBranchManagerSelection").dialog("close");
        }


        // add user
        function btnSelectAdd_onclick() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }

            // no data, add columns
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr th").length == 0) {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr:first").remove();
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr:first").clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList");

            }

            // clear user list
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr:not(:first)").remove();

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox[checked=true]").parent().parent().clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList");

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox").attr("checked", "");

            // close modal
            $("#dialog-modal").dialog("close");
        }

        // popup cancel
        function btnSelectCancel_onclick() {

            // close modal
            $("#dialog-modal").dialog("close");
        }
        // do sth. before saving
        function BeforeSave() {

            //#chkHomeBranch
            var chkHomeBranch = $("[cid='chkHomeBranch'] input ");
            if (chkHomeBranch.attr("checked") == true) {

                var homeBranchID = $("[cid='chkHomeBranch']").attr("HomeBranchID");
                var BranchID = $("[cid='chkHomeBranch']").attr("BranchID");

                if (BranchID != homeBranchID && homeBranchID != 0) {
                    var ral = window.confirm('There is already another branch marked as "Home Branch". Do you want to deselect the other one and mark this one as the "Home Branch"?');
                    if (ral != true) {

                        chkHomeBranch.attr("checked", "");
                        return false;
                    }
                }

            }


            var MarketingEnabled = $("#<%= hdnMarketingEnabled.ClientID%>").val();
            if (MarketingEnabled == "1") {
                if ($("#<%= txbLicense1.ClientID%>").val() == "") {

                    SetTab(1);
                    $("#<%= txbLicense1.ClientID%>").focus();
                    alert("Please enter License 1.");
                    return false;
                }
                if ($("#<%= txbDisclaimer.ClientID%>").val() == "") {
                    SetTab(1);
                    $("#<%= txbDisclaimer.ClientID%>").focus();
                    alert("Please enter Marketing Disclaimer.");
                    return false;
                }
            }
            var FolderIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr:not(:first)").each(function (i) {

                var FolderID = $(this).find("td :checkbox").attr("title") + "," + $(this).find("td select option:selected").val();
                if (i == 0) {
                    FolderIDs = FolderID;
                }
                else {
                    FolderIDs += "|" + FolderID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnFolderIDs").val(FolderIDs);

            var Managers = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox").each(function (i) {

                var UserID = $(this).attr("title");
                if (i == 0) {
                    Managers = UserID;
                }
                else {
                    Managers += "," + UserID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnManagers").val(Managers);

            var EnabledFolderIDs = "";
            var DisabledFolderIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridPointFolderList tr td :checkbox").each(function () {

                var Enabled = $(this).parent().parent().find(":input[id^='txtEnabled']").val();
                var FolderID = $(this).attr("title");

                if (Enabled == "Yes") {
                    if (EnabledFolderIDs == "") {
                        EnabledFolderIDs = FolderID;
                    }
                    else {
                        EnabledFolderIDs += "," + FolderID;
                    }
                }
                else if (Enabled == "No") {
                    if (DisabledFolderIDs == "") {
                        DisabledFolderIDs = FolderID;
                    }
                    else {
                        DisabledFolderIDs += "," + FolderID;
                    }
                };
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnEnableFolderIDs").val(EnabledFolderIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnDisableFolderIDs").val(DisabledFolderIDs);

            return true;
        }

        // show popup for enter Branch name
        function EnterBranchName() {

            // clear Branch name
            $("#txtBranchName").val("")

            // show modal
            $("#divNewBranchName").dialog({
                height: 160,
                width: 380,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // create Branch
        function CreateBranch() {

            var BranchName = $.trim($("#txtBranchName").val());
            if (BranchName == "") {

                alert("Please enter Branch Name.");
                return;
            }
            else {

                if (BranchName.length < 2) {

                    alert("Please enter more than 2 characters for Branch name.");
                    return;
                }

                var Regex = /^[0-9a-zA-Z_\-\.\,\s]{2,50}$/;
                var bIsValid = Regex.test(BranchName);
                if (bIsValid == false) {
                    alert("Use only letters[a-z or A-Z], numbers[0-9], common (,), period (.), dash (-) and white space.");
                    return;
                }
            }

            // show waiting message
            $("#divMsg").show();

            $.ajax({
                type: "POST",
                url: "SettingsWebService.aspx/CreateBranch",
                data: '{sBranchName:"' + BranchName + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: CreateGroup_Callback,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert("Failed to create Branch."); $("#divNewBranchName").dialog("close"); }
            });
        }

        // callback of create Branch
        function CreateGroup_Callback(data) {

            if (data.d != "" && isID(data.d) == false) {
                $("#divMsg").hide();    // hide waiting message
                alert(data.d);
                return;
            }

            // show success message
            setTimeout("$('#lbMsg').text('Create Branch successfully.');", 2000);

            // close modal dialog
            setTimeout("$('#divNewBranchName').dialog('close');window.location.href='BranchSetup.aspx?BranchID=" + data.d + "'", 4000);
        }


        // callback of create Branch


        function ClosePopupBranchName() {

            // close modal
            $("#divNewBranchName").dialog("close");
        }
// ]]>
    </script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            DrawTab();
            DrawSubTab();
            SetTab(0);
            SetSubTab(1);

            var iframeid = $("#ifrMailChimpTab").attr("id");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var BranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranchName").val();

            var src = "MarketingMailChimpTab.aspx?sid=" + Radom + "&BranchID=" + BranchID

            window.document.getElementById(iframeid).contentWindow.location.replace(src);

        });

        function SetTab(i) {
            if (i == 0) {

                $("#divGeneral").show();
                $("#divMarketing").hide();
            }
            else {
                $("#divGeneral").hide();
                $("#divMarketing").show();
            }
            $("#tabs10 #current").removeAttr("id");
            $("#tabs10 ul li").eq(i).attr("id", "current");
        }

        function SetSubTab(i) {
            if (i == 0) {

                $("#divMarketingTab").show();
                $("#divMailChimpTab").hide();
            }
            else {
                $("#divMarketingTab").hide();
                $("#divMailChimpTab").show();
            }
            $("#tabsubs10 #currentsub").removeAttr("id");
            $("#tabsubs10 ul li").eq(i).attr("id", "currentsub");
        }


        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $.blockUI({ message: $('#divWaiting'), css: { width: '500px'} });
        }

        function CloseWaitingDialog() {

            $.unblockUI();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div class="Heading">
        Branch/Location Setup</div>
    <div class="SplitLine">
    </div>
    <div class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Branch Name
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlBranchName" runat="server" DataValueField="BranchID" DataTextField="Name"
                            Width="173px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 20px;">
                        <asp:CheckBox ID="ckbEnabled" runat="server" Text="  Enabled" />
                    </td>
                    <td style="padding-left: 60px;">
                        Group Access
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlGroupAccess" Width="177px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-top: 9px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 65px;">
                        Description
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="txbDescription" runat="server" Height="42px" Rows="2" TextMode="MultiLine"
                            Width="577px" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-top: 9px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 65px;">
                        Address
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="txbAddress" runat="server" Width="258px" MaxLength="255"></asp:TextBox>
                    </td>
                    <td style="padding-left: 25px;">
                        City
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="txbCity" runat="server" Width="120px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="padding-left: 25px;">
                        State
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlState" runat="server" Width="62px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 25px;">
                        Zip
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="txbZip" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Phone
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="txbPhone" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="padding-left: 25px;">
                        Fax
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="txbFax" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email
                    </td>
                    <td colspan="6" style="padding-left: 15px;">
                        <asp:TextBox ID="txbEmail" runat="server" Width="450px" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Branch Website
                    </td>
                    <td colspan="3" style="padding-left: 15px;">
                        <asp:TextBox ID="txbWebURL" runat="server" Width="450px" MaxLength="255"></asp:TextBox>
                    </td>
                    <td colspan="3" style="padding-left: 15px;">
                        <asp:CheckBox ID="chkHomeBranch" Checked="false" runat="server" Text="Home Branch" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-top: 15px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Logo for Borrower/Partner Website
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:FileUpload ID="fuldWebLogo" runat="server" Width="259px" />
                    </td>
                    <td style="padding-left: 15px;">
                        <div>
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/YourLogo.jpg" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="DashedBorder" style="margin-top: 15px;">
            &nbsp;</div>
        <div id="divProspectTabs">
            <div class="JTab">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="width: 10px;">
                            &nbsp;
                        </td>
                        <td>
                            <div id="tabs10">
                                <ul>
                                    <li id="current"><a href="#" onclick="SetTab(0);return false;"><span>General</span></a></li>
                                    <li><a href="#" onclick="SetTab(1);return false;"><span>Marketing</span></a></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="TabBody">
                    <div id="TabLine1" class="TabLeftLine">
                        &nbsp;</div>
                    <div id="TabLine2" class="TabRightLine">
                        &nbsp;</div>
                    <div class="TabContent">
                        <div id="divGeneral" style="margin-top: 5px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 15px; vertical-align: top;">
                                        <div id="divToolBar" style="margin-top: 13px;">
                                            <ul class="ToolStrip">
                                                <li><a id="aAdd" href="javascript:AddPointMember()">Add Point Folder</a><span>|</span></li>
                                                <li><a id="aEnable" href="javascript:EnablePointFolder()">Enable</a><span>|</span></li>
                                                <li><a id="aDisable" href="javascript:DisablePointFolder()">Disable</a><span>|</span></li>
                                                <li><a id="aRemove" href="javascript:RemovePointMember()">Remove</a></li>
                                            </ul>
                                        </div>
                                        <div id="divPointFolderList" class="ColorGrid" style="margin-top: 2px; width: 352px;">
                                            <asp:GridView ID="gridPointFolderList" runat="server" DataKeyNames="FolderId" EmptyDataText="There is no data to display."
                                                OnRowDataBound="gridPointFolderList_RowDataBound" AutoGenerateColumns="false"
                                                CellPadding="3" CssClass="GrayGrid" GridLines="None">
                                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                                <AlternatingRowStyle CssClass="EvenRow" />
                                                <Columns>
                                                    <asp:BoundField DataField="LoanStatus" HeaderText="Loan Status" />
                                                    <asp:BoundField DataField="Default" HeaderText="DefaultValue" />
                                                    <asp:BoundField DataField="FolderId" HeaderText="FolderId" />
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <input id="Checkbox2" type="checkbox" title="<%# Eval("FolderId") %>" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                                        <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Name" HeaderText="Point Folder" />
                                                    <asp:TemplateField ItemStyle-Width="100px">
                                                        <HeaderTemplate>
                                                            Loan Status
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="DropDownList1" runat="server">
                                                                <asp:ListItem Value="7">Archive Loans</asp:ListItem>
                                                                <asp:ListItem Value="2">Canceled</asp:ListItem>
                                                                <asp:ListItem Value="3">Closed</asp:ListItem>
                                                                <asp:ListItem Value="4">Denied</asp:ListItem>
                                                                <asp:ListItem Value="1">Active Loans</asp:ListItem>
                                                                <asp:ListItem Value="6">Active Leads</asp:ListItem>
                                                                <asp:ListItem Value="8"> Archive Leads</asp:ListItem>
                                                                <%--  CR063 <asp:ListItem Value="5">Suspended</asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Enabled" HeaderStyle-Width="55">
                                                        <ItemTemplate>
                                                            <input id="txtEnabled" readonly="readonly" style="border-style: none; background-color: transparent;
                                                                width: 55px;" value="<%# Eval("Enabled").ToString().ToLower() == "true" ? "Yes" : "No" %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Default" HeaderStyle-Width="40">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbDefault" runat="server" Style="margin-left: 4px;" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div class="GridPaddingBottom">
                                                &nbsp;</div>
                                        </div>
                                    </td>
                                    <td style="width: 30px;">
                                    </td>
                                    <td style="padding-left: 15px; vertical-align: top;">
                                        <div id="div1" style="margin-top: 13px;">
                                            <ul class="ToolStrip">
                                                <li><a id="a1" href="javascript:AddMember()">Add Branch Manager</a><span>|</span></li>
                                                <li><a id="a2" href="javascript:RemoveMember()">Remove</a></li>
                                            </ul>
                                        </div>
                                        <div id="div2" class="ColorGrid" style="margin-top: 2px; width: 302px;">
                                            <asp:GridView ID="gridUserList" runat="server" DataKeyNames="UserId" AutoGenerateColumns="false"
                                                EmptyDataText="There is no data to display." CellPadding="3" CssClass="GrayGrid"
                                                GridLines="None">
                                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                                <AlternatingRowStyle CssClass="EvenRow" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                                        <HeaderTemplate>
                                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <input id="Checkbox2" type="checkbox" title="<%# Eval("UserID") %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FullName" HeaderText="User Name" />
                                                </Columns>
                                            </asp:GridView>
                                            <div class="GridPaddingBottom">
                                                &nbsp;</div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="display: none;">
                                <asp:Button ID="btnSetDeault" OnClick="btnSetDeault_Click" runat="server" Text="SetDeault" />
                                <asp:HiddenField ID="hifId" Value="" runat="server" />
                            </div>
                        </div>
                        <div id="divMarketing">
                            <div class="subJTab">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 10px;">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <div id="tabsubs10">
                                                <ul>
                                                    <%--<li id="currentsub"><a href="#" onclick="SetSubTab(0);return false;"><span>Leadstar</span></a></li>--%>
                                                    <li id="currentsub"><a href="#" onclick="SetSubTab(1);return false;"><span>Mail Chimp</span></a></li>
                                                </ul>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div id="TabsubBody">
                                    <div id="TabsubLine1" class="TabLeftLine">
                                        &nbsp;</div>
                                    <div id="TabsubLine2" class="TabRightLine">
                                        &nbsp;</div>
                                    <div class="TabSubContent">
                                        <div id="divMarketingTab">
                                            <table>
                                                <tr style="height: 45px;">
                                                    <td style="width: 70px;">
                                                        <asp:Label ID="Label1" runat="server" Text="License 1:"></asp:Label>
                                                    </td>
                                                    <td style="width: 220px;">
                                                        <asp:TextBox ID="txbLicense1" Width="200px" MaxLength="255" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 20px;">
                                                    </td>
                                                    <td style="width: 70px;">
                                                        <asp:Label ID="Label2" runat="server" Text="License 2:"></asp:Label>
                                                    </td>
                                                    <td style="width: 220px;">
                                                        <asp:TextBox ID="txbLicense2" Width="200px" MaxLength="255" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="height: 45px;">
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="License 3:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txbLicense3" Width="200px" MaxLength="255" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="License 4:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txbLicense4" Width="200px" MaxLength="255" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style="height: 45px;">
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server" Text="License 5:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txbLicense5" Width="200px" MaxLength="255" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr style="height: 100px;">
                                                    <td style="width: 70px;">
                                                        <asp:Label ID="Label5" runat="server" Text="Marketing Disclaimer:"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox ID="txbDisclaimer" Width="510px" TextMode="MultiLine" MaxLength="1500"
                                                            Height="78px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divMailChimpTab" style="padding-left: 15px; padding-top: 15px;">

            <iframe id="ifrMailChimpTab" frameborder="0" scrolling="no" style="border: solid 0px red;width: 756px; height: 150px;" 
            src=" ">
            </iframe>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return BeforeSave()"
                            CssClass="Btn-66" OnClick="btnSave_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="Button2" type="button" value="New" class="Btn-66" onclick="EnterBranchName()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divNewBranchName" title="Enter Branch Name" style="display: none; margin: 10px;">
            <table>
                <tr>
                    <td style="width: 90px;">
                        Branch Name:
                    </td>
                    <td>
                        <input id="txtBranchName" type="text" maxlength="50" style="width: 200px;" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="text-align: center;">
                <input id="btnCreate" type="button" value="Create" class="Btn-66" onclick="CreateBranch()" />&nbsp;&nbsp;
                <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="ClosePopupBranchName()" />
            </div>
            <div id="divMsg" style="margin-top: 10px; text-align: center; display: none;">
                <img id="imgWaiting" src='../images/waiting.gif' style='position: relative; top: 2px;' />
                <label id="lbMsg" style='font-weight: bold;'>
                    Please wait...</label>
            </div>
        </div>
        <div id="dialog-modal" title="Branch Manager Selection" style="display: none;">
            <br />
            <div style="width: 418px; height: 370px; overflow: auto;">
                <div id="divUserSelectionList" class="ColorGrid" style="width: 400px;">
                    <asp:GridView ID="gridUserSelectionList" runat="server" DataKeyNames="UserId" EmptyDataText="There is no data to display."
                        CellPadding="3" CssClass="GrayGrid" GridLines="None">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                <HeaderTemplate>
                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input id="Checkbox2" type="checkbox" title="<%# Eval("UserID") %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        </Columns>
                    </asp:GridView>
                    <div class="GridPaddingBottom">
                        &nbsp;</div>
                </div>
            </div>
            <div class="SplitLine" style="position: absolute; left: -8px; top: 380px; width: 446px;">
                &nbsp;
            </div>
            <div style="margin-top: 20px; text-align: center;">
                <input id="btnSelectAdd" type="button" value="Add" class="Btn-66" onclick="return btnSelectAdd_onclick()" />&nbsp;&nbsp;
                <input id="btnSelectCancel" type="button" value="Cancel" class="Btn-66" onclick="return btnSelectCancel_onclick()" />
            </div>
        </div>
        <div id="divBranchManagerSelection" title="Point Folder Selection" style="display: none;">
            <br />
            <div style="width: 418px; height: 370px; overflow: auto;">
                <div id="div4" class="ColorGrid" style="width: 400px;">
                    <asp:GridView ID="gridPointFolderSelectionList" runat="server" DataKeyNames="FolderId"
                        EmptyDataText="There is no data to display." OnRowDataBound="gridPointFolderSelectionList_RowDataBound"
                        CellPadding="3" CssClass="GrayGrid" GridLines="None">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:BoundField DataField="LoanStatus" HeaderText="LoanStatus" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input id="Checkbox2" type="checkbox" title="<%# Eval("FolderId") %>" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Point Folder" />
                            <asp:TemplateField ItemStyle-Width="100px">
                                <HeaderTemplate>
                                    Loan Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server">
                                        <asp:ListItem Value="7">Archive Loans</asp:ListItem>
                                        <asp:ListItem Value="2">Canceled</asp:ListItem>
                                        <asp:ListItem Value="3">Closed</asp:ListItem>
                                        <asp:ListItem Value="4">Denied</asp:ListItem>
                                        <asp:ListItem Value="1">Active Loans</asp:ListItem>
                                        <asp:ListItem Value="6">Active Leads</asp:ListItem>
                                        <asp:ListItem Value="8">Archive Leads</asp:ListItem>
                                        <%--  CR063 <asp:ListItem Value="5">Suspended</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div class="GridPaddingBottom">
                        &nbsp;</div>
                </div>
            </div>
            <div class="SplitLine" style="position: absolute; left: -8px; top: 380px; width: 446px;">
                &nbsp;
            </div>
            <div style="margin-top: 20px; text-align: center;">
                <input id="Button1" type="button" value="Add" class="Btn-66" onclick="return btnPointSelectAdd_onclick()" />&nbsp;&nbsp;
                <input id="Button3" type="button" value="Cancel" class="Btn-66" onclick="return btnPointSelectCancel_onclick()" />
            </div>
        </div>
        <asp:HiddenField ID="hdnFolderIDs" runat="server" />
        <asp:HiddenField ID="hdnDisableFolderIDs" runat="server" />
        <asp:HiddenField ID="hdnEnableFolderIDs" runat="server" />
        <asp:HiddenField ID="hdnManagers" runat="server" />
        <asp:HiddenField ID="hdnMarketingEnabled" runat="server" />
    </div>
    <div id="divWaiting" style="display: none; padding: 5px;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <img id="img1" src="../images/waiting.gif" />
                </td>
                <td style="padding-left: 5px;">
                    <label id="WaitingMsg" style="color: #818892; font-weight: bold;">
                    </label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
