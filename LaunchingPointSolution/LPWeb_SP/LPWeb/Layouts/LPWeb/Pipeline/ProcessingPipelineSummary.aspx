<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Processing Pipeline Summary" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master"
    AutoEventWireup="true" CodeBehind="ProcessingPipelineSummary.aspx.cs" Inherits="Pipeline_ProcessingPipelineSummary" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.minV1.8.17.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../css/jqcontextmenu.css" />
    <script type="text/javascript" src="../js/jqcontextmenu.js"></script>
    <style type="text/css">
        .alert
        {
            width: 65px;
        }
        .alert img
        {
            float: left;
        }
        .progressbar
        {
        }
        .progressbar table
        {
            margin: 0;
            padding: 0;
            border-collapse: collapse;
            border: 0;
            border-color: transparent;
            font-size: 8px;
            line-height: 20px;
        }
        .progressbar table td
        {
            margin: 0;
            padding: 0;
            border: 0;
            border-color: transparent;
            font-size: 8px;
            line-height: 20px;
        }
        .progressbar table td
        {
            margin: 0;
            padding: 0;
            border: 0;
            border-color: transparent;
            font-size: 8px;
            line-height: 20px;
        }
        .ProgressContainer, completed, content
        {
            background-image: url(../images/ProgressBarBG.gif);
            background-repeat: no-repeat;
            width: 142px;
            height: 20px;
            padding: 0;
            margin: 0;
            position: relative;
        }
        .completed
        {
            background-image: url(../images/Progress.gif);
            background-repeat: repeat-x;
            background-position: 0 50%;
            position: absolute;
            max-width: 139.5px;
            width: 0;
            left: 1px;
            top: 2px;
            height: 16px;
        }
        .content
        {
            background-image: none;
            background-color: transparent;
            width: 142px;
            height: 20px;
            text-align: center;
            position: absolute;
            left: 0;
            top: 3px;
        }
        a.loanDetails:link, :visited, :active
        {
            color: #818892;
        }
        a.loanDetails:hover
        {
            color: Blue;
        }
    </style>
    <style>
        .TabContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabContent input, select
        {
            margin-left: 10px;
        }
        #divUserList td
        {
            margin-left: 0px;
            margin-right: 0px;
            padding: 0;
        }
    </style>
    <script language="javascript" type="text/javascript">
        // Start: add by peter, for Dispose button 2010-11-15
        Array.prototype.removeLoan = function (s) {
            var index = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i].ID == s) {
                    index = i;
                    break;
                }
            }
            if (index >= 0) {
                this.splice(index, 1);
                return true;
            }
            return false;
        };
        function SelectedLoan(sID, sStatus, sBId) {
            this.ID = sID;
            this.Status = sStatus;
            this.BranchID = sBId;
        }
        var tLS = -1;
        var bId = -1;
        var allLoan = new Array();
        var allSelectedLoan = new Array();

        function initCfmBox(id, title, msg, w, h, callBack) {
            $('#' + id).dialog({
                modal: true,
                autoOpen: false,
                width: w,
                height: h,
                resizable: false,
                title: title,
                buttons: {
                    Yes: function () {
                        callBack();
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                        clearFolderSelWin(id);
                    }
                },
                open: function () {
                    $('.ui-dialog-buttonset').css('float', 'none');
                    $('.ui-dialog-buttonset').css('text-align', 'center');
                    $('.ui-dialog-buttonpane').find('button').addClass('Btn-66');
                }
            });
        }
        function clearFolderSelWin(id) {
            var f = "";
            if ("dialog1" == id)
                f = document.getElementById('iframePF');
            f.src = "about:blank";
        }
        // End: add by peter, for Dispose button 2010-12-21

        var gridId = "#<%=gvPipelineView.ClientID %>";

        // when rule alert popup close, refresh page or not (neo)
        var IsRefreshPage = false;

        $(document).ready(function () {
            DrawTab();
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
                window.location.href = '../LoanDetails/LoanDetails.aspx?FromPage=<%= FromURL %>&fieldid=' + $("#<%=hfDeleteItems.ClientID %>").val() + "&fieldids=" + selctedItems.join(",");
            });
            $("a.loanDetails").each(function () {
                $(this).click(function () {
                    var selctedItems = new Array();
                    $(gridId + " :checkbox:gt(0)").each(function () {
                        var item = $(this);
                        selctedItems.push(item.attr("tag"));
                    });
                    window.location.href = '../LoanDetails/LoanDetails.aspx?FromPage=<%= FromURL %>&fieldid=' + $(this).attr("tag") + "&fieldids=" + selctedItems.join(",");
                });
            });

            $("#<%= btnRemove.ClientID %>").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }
                else {
                    return confirm("This operation is not reversible.The workflow history and loan history will be deleted.Are you sure you want to continue?");
                }
            });

            $("#<%= btnSync.ClientID %>").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }
                //                else {
                //                    return confirm("Import Loans");
                //                }
            });

            $(".alert img[src$='Unknown.png'],img[src$='TaskGreen.png']").each(function () {
                $(this).hide();
            });

            $(".alert img.RateLockIcon").each(function () {
                $(this).css("cursor", "pointer");
                $(this).click(function () {
                    var dialog = $("#RateLockDetail");
                    var iconStart = $(this).attr("src").lastIndexOf("/") + 1;
                    dialog.attr("src", "PopupAlertDetail.aspx?fileId=" + $(this).attr("tag") + "&icon=" + $(this).attr("src").substring(iconStart));
                    dialog.dialog({
                        modal: true,
                        title: "Rate Lock Alert",
                        width: 580,
                        height: 320,
                        resizable: false,
                        close: function (event, ui) { $(this).dialog('destroy') },
                        open: function (event, ui) { $(this).css("width", "100%") }
                    });
                    $(".ui-dialog").css("border", "solid 3px #aaaaaa");
                });
            });

            $(".alert img.AlertIcon").each(function () {
                $(this).css("cursor", "pointer");
                $(this).click(function () {
                    var dialog = $("#taskAlertDetail");
                    dialog.attr("src", "TaskAlertDetail.aspx?fileID=" + $(this).attr("tag"));
                    dialog.dialog({
                        modal: true,
                        title: "Task Alert Detail",
                        width: 580,
                        height: 320,
                        resizable: false,
                        close: function (event, ui) { $(this).dialog('destroy') },
                        open: function (event, ui) { $(this).css("width", "100%") }
                    });
                    $(".ui-dialog").css("border", "solid 3px #aaaaaa");
                });
            });

            $(".alert img.RuleAlertIcon").each(function () {

                $(this).css("cursor", "pointer");
                $(this).click(function () {

                    var LoanID = $(this).attr("tag");
                    var AlertID = $(this).attr("myAlertID");

                    var RadomNum = Math.random();
                    var RadomStr = RadomNum.toString().substr(2);

                    $("#ifrRuleAlert").attr("src", "../LoanDetails/RuleAlertPopup.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&AlertID=" + AlertID);

                    // show modal
                    $("#divRuleAlert").dialog({
                        height: 650,
                        width: 535,
                        modal: true,
                        resizable: false,
                        close: function (event, ui) { $("#divRuleAlert").dialog('destroy'); if (IsRefreshPage == true) { window.location.href = window.location.href; } }
                    });
                    $(".ui-dialog").css("border", "solid 3px #aaaaaa");
                });
            });

            // Start: add by peter, 2010-11-15
            $(".alert img.ImportErrorIcon").each(function () {
                $(this).css("cursor", "pointer");
                $(this).click(function () {
                    var dialog = $("#importErrorDetail");
                    var iconStart = $(this).attr("src").lastIndexOf("/") + 1;
                    dialog.attr("src", "PopupPointImportAlert.aspx?fileId=" + $(this).attr("tag") + "&icon=" + $(this).attr("src").substring(iconStart));
                    dialog.dialog({
                        modal: true,
                        title: "Point Import Alert Detail",
                        width: 640,
                        height: 520,
                        resizable: false,
                        close: function (event, ui) { $(this).dialog('destroy') },
                        open: function (event, ui) { $(this).css("width", "100%") }
                    });
                    $(".ui-dialog").css("border", "solid 3px #aaaaaa");
                });
            });

            // init handle import errors list window
            initImportErrorListWin();
            initSearchWin();
            // End: add by peter, 2010-11-15




            // Start: add by peter, for Dispose button 2010-12-21
            $("#" + '<%=btnDispose.ClientID %>').contextMenu("menu1", {
                menuStyle: {
                    listStyle: 'none',
                    padding: '1px',
                    margin: '0px',
                    backgroundColor: '#fff',
                    border: '1px solid #999',
                    width: '150px'
                },
                itemStyle: {
                    margin: '0px',
                    color: '#000',
                    display: 'block',
                    cursor: 'default',
                    padding: '3px',
                    border: '1px solid #fff',
                    backgroundColor: 'transparent'
                },
                itemHoverStyle: {
                    border: '1px solid #0a246a',
                    backgroundColor: '#b6bdd2'
                },
                bindings: {
                    convertToLead: convertToLead,
                    //CR063 suspend: suspendLoan,
                    deny: denyLoan,
                    cancel: cancelLoan,
                    resume: resumeLoan
                },
                onContextMenu: function (e) {
                    if (allSelectedLoan.length > 1) {
                        alert("Only one record can be selected for this operation.");
                        return false;
                    }
                    else if (allSelectedLoan.length == 1) {
                        var s = allSelectedLoan[0].Status.toLowerCase();
                        if (s == "processing" || s == "suspended" || s == "denied" || s == "canceled" || s == "closed")
                        //return CheckPointFileStatus(allSelectedLoan[0].ID);
                            return true;
                        else {
                            alert("Unable to process the request for the selected loan. The loan status is unknown.");
                            return false;
                        }
                    }
                    else {
                        alert("No record has been selected.");
                        return false;
                    }
                },
                onShowMenu: function (e, menu) {
                    var s = allSelectedLoan[0].Status.toLowerCase();
                    if (s == "closed") {
                        $("*", menu).remove();
                        alert("You cannot dispose of a closed loan.");
                        return menu;
                    }
                    if (s == "processing") {
                        $("#resume", menu).remove();
                    }
                    else if (s == "suspended" || s == "denied" || s == "canceled") {
                        $("#suspend, #deny, #cancel", menu).remove();
                    }
                    else {
                        $("*", menu).remove();
                        alert("Unable to process the request for the selected loan. The loan status is unknown.");
                        return menu;
                    }
                    $("#" + '<%=hiSelectedLoan.ClientID %>').val(allSelectedLoan[0].ID);
                    $("#" + '<%=hiSelectedLoanStatus.ClientID %>').val(allSelectedLoan[0].Status);
                    bId = allSelectedLoan[0].BranchID;
                    return menu;
                }
            });
            initCfmBox('dialog1', 'Point Folder Selection', '', 500, 450, function () {
                if (window.frames.iframePF && window.frames.iframePF.returnFn) {
                    window.frames.iframePF.returnFn();
                }
            });
            // End: add by peter, for Dispose button 2010-12-21

            //bug 2689,Add by Alex
            var sWidth = $("#" + '<%=gvPipelineView.ClientID %>').width();
            var sDivWidth = $("#" + '<%=divTab.ClientID %>').width();
            if (sWidth > sDivWidth) {
                $("#" + '<%=divTab.ClientID %>').width(sWidth + 40);
                var iLineWidth = $("#TabLine2").width();
                $("#TabLine2").width(iLineWidth + sWidth - sDivWidth);
            }
        });

        // CR50 Start CheckPointFileStatus
        function CheckPointFileStatus(FileId , Fun) {
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);
            var isAsync = false;
            if (Fun != null) {
                isAsync = true;
            }
            //var FileId = GetQueryString1("FileID");
            var res = false;
            $.blockUI({ message: "Please wait..." });
            $.ajax({
                url: "../CheckPointFileStatus_BG_JSON.aspx?sid=" + Radom + "&FileId=" + FileId,
                async: isAsync,
                cache: false,
                dataType: "json",
                success: function (data) {

                    if (data.ExecResult == "Failed") {
                        //$('#divDetail').unblock();
                        alert(data.ErrorMsg);
                        //return false;
                        res = false;
                    }
                    else {
                        //return true;
                        res = true;
                        Fun();
                    }
                    $.unblockUI();
                }
            });
            return res;
        }

        //End CheckPointFileStatus

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

        function closeDialog() {
            $("#taskAlertDetail").dialog('destroy');
            window.location.href = window.location.href;
        }

        // Start: add by peter, 2010-11-15
        function closeImportErrorDialog() {
            $("#importErrorDetail").dialog('destroy');
        }
        // End: add by peter, 2010-11-15

        function closeDialogR() {
            $("#RateLockDetail").dialog('destroy');
        }

        function CloseDialog_RuleAlert() {

            $("#divRuleAlert").dialog("close");
        }

        function SetupSelectedItem() {
            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }

            var items = $("#<%=hfDeleteItems.ClientID %>").val().split(",");
            if (items.length > 1) {
                alert("Only one record can be selected for this operation..");
                return false;
            }
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                selctedItems.push(item.attr("tag"));
            });



            window.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=<%= FromURL %>&fieldid=" + $("#<%=hfDeleteItems.ClientID %>").val() + "&fieldids=" + selctedItems.join(",") + "&tab=WorkflowSetupTab&tabIndex=3";
            //            window.location.href = "../LoanDetails/LoanDetail.aspx?tab=WorkflowSetupTab&tabIndex=3&FileID=" + $("#<%=hfDeleteItems.ClientID %>").val();
        }
    </script>
    <script type="text/javascript">
        // Start: add by peter, 2010-11-15
        jQuery.extend
        ({
            'QueryString': window.location.search.length <= 1 ? new Array() :
                function (a) {
                    var b = new Array();
                    for (var i = 0; i < a.length; ++i) {
                        var p = a[i].split('=');
                        b[p[0]] = unescape(p[1]);
                    }
                    return b;
                } (window.location.search.substr(1).split('&'))
        });

        function initImportErrorListWin()
        {
            $('#dialogPointImportErrorList').dialog({
                modal: true,
                autoOpen: false,
                title: 'Handle Point Errors',
                width: 880,
                height: 750,
			    resizable: false,
                close: clearImportErrorListWin
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
        }
        function initSearchWin()
        {
            $('#dialogPointImportErrorList1').dialog({
                modal: true,
                autoOpen: false,
                title: 'Search Loans',
                width: 600,
                height: 350,
			    resizable: false,
                close: clearSearchWin
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
        }
        function onImportErrorBtnClick() {
            var f = document.getElementById('iframeImportErrorList');
            var q = $.QueryString["q"];
            f.src = "HandlePointErrors.aspx?q=" + q + "&t=" + Math.random().toString();
            $('#dialogPointImportErrorList').dialog('open');
        }
        function closeImportErrorWin(bRefresh) {
            $('#dialogPointImportErrorList').dialog('close');
            if (bRefresh === true)
                RefreshList();
        }
        function RefreshList() {
            <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>
        }
        function clearImportErrorListWin()
        {
            var f = document.getElementById('iframeImportErrorList');
            f.src="about:blank";
        }
        function clearSearchWin()
        {
            var f = document.getElementById('iframeImportErrorList1');
            f.src="about:blank";
        }
        // End: add by peter, 2010-11-15
        
        // Start: add by peter, for Dispose button 2010-11-15
        function onCkAllRowSelected(me)
        {
            var bCheck = $(me).attr('checked');
            if (bCheck)
                allSelectedLoan = allLoan; // copy all allLoan items to allSelectedLoan
            else
                allSelectedLoan = new Array();
            $('input:checkbox', $('#' + '<%=gvPipelineView.ClientID %>')).each(function() { $(this).attr('checked', bCheck); });
        }
        function onCkRowClicked(me, sID, sStatus, sBranchId)
        {

            if ($(me).attr("checked"))
                allSelectedLoan.push(new SelectedLoan(sID, sStatus, sBranchId));
            else
                allSelectedLoan.removeLoan(sID);
        }

        function convertToLead()
        {

            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('ConvertToLead');
            tLS= 6;
            ShowFolerSelWin();
        }

        function suspendLoan()
        {

            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Suspended');
            tLS= 5;
            ShowFolerSelWin();
        }

        function denyLoan()
        {
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Denied');
            tLS= 4;
            ShowFolerSelWin();
        }

        function cancelLoan()
        {            
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Canceled');
            tLS= 2;
            ShowFolerSelWin();
        }

        function resumeLoan()
        {
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Processing');
            tLS= 1;
            ShowFolerSelWin();
        }

        function getFolderSelectionReturn(sReturn)
        {
            $("#"+'<%=hiSelectedFolderId.ClientID %>').val(sReturn);
            $('#dialog1').dialog('close');
            <%=this.ClientScript.GetPostBackEventReference(this.btnDispose, null) %>
            clearFolderSelWin('dialog1');
        }

        function ShowFolerSelWin() {
                
             var loanId = $("#" + '<%=hiSelectedLoan.ClientID %>').val();
             var res =  CheckPointFileStatus(loanId,function(){
                    var f = document.getElementById('iframePF');
                    f.src = "PointFolderSelection.aspx?fid=" + $("#" + '<%=hiSelectedLoan.ClientID %>').val() + "&tls=" + tLS + "&bid=" + bId + "&type=dispose&t=" + Math.random().toString();
                    $('#dialog1').dialog('open');
                });
                return false;
            }
        // End: add by peter, for Dispose button 2010-11-15  
        
        function onSearchClick() {
            var f = document.getElementById('iframeImportErrorList1');
            f.src = "SearchLoans.aspx?type=active&t=" + Math.random().toString();
            $('#dialogPointImportErrorList1').dialog('open');
        } 
        
       function closeSearchWin() {
            $('#dialogPointImportErrorList1').dialog('close');
        }

        function getSearchFilterReturn(sFilter) {
            if (null != sFilter)
            {
                $("#" + "<%=hiSearchFilter.ClientID %>").val(sFilter);
                <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>;
            }
        }  

        function SearchLastName()
        {
           var lname = $.trim($("#" + "<%=btnBorrowerLastName.ClientID %>").val());

           if(lname.indexOf("'")!=-1)
           {
                alert("LastName, please do not use single quotes.");
               return false;
           }

            var sFilter = "lname\u0002" + lname;
            
            $("#" + "<%=hiSearchFilter.ClientID %>").val(sFilter);
            <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>;

            return true;
        }

        function onMoveClick()
        {
            if (allSelectedLoan.length > 1) {
                alert("Only one record can be selected for this operation.");
                return false;
            }
            else if (allSelectedLoan.length == 0) {
                alert("No record has been selected.");
                return false;
            }

            //CR50
           var isLock = CheckPointFileStatus(allSelectedLoan[0].ID,
               function (){ 
                   $("#" + '<%=hiSelectedLoan.ClientID %>').val(allSelectedLoan[0].ID);
                    $("#" + '<%=hiSelectedLoanStatus.ClientID %>').val(allSelectedLoan[0].Status);
                    var SelStatus = allSelectedLoan[0].Status;
                    bId = allSelectedLoan[0].BranchID;
                     $("#"+'<%=hiSelectedDisposal.ClientID %>').val('move');
                     var f = document.getElementById('iframePF');
                    f.src = "PointFolderSelection.aspx?fid=" +  $("#" + '<%=hiSelectedLoan.ClientID %>').val() + "&tls="+ SelStatus  +"&bid=" + bId + "&t=" + Math.random().toString();
                    $('#dialog1').dialog('open');
               }
           );
           return false;
//           if(!isLock)
//           {
//                return false;
//           }

            
            return false;
        }

        function ClearSearch()
        {
//            $("#" + "<%=hiSearchFilter.ClientID %>").val("");
//            <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>;
            window.location.href=window.location.pathname;
        }      



        //gdc CR45

        function SaveViewDialog()
        {
             $('#SaveViewShow').dialog({
                        modal: false,
                        title: "Save View",
                        width: 350,
                        height: 120,
                        resizable: false                    
                    });
            $('#SaveViewShow').dialog('open');

        }

        function onExportClick()
        {
            
            if (allSelectedLoan.length == 0) {
                alert("Please select the loans to be exported.");
                return false;
            }

            var IDs = "";

            $(allSelectedLoan).each(function(n,obj){
                
                IDs += obj.ID + ",";
            });

            //hidFilterQueryCondition
            var queryCondition = $("#<%=hidFilterQueryCondition.ClientID %>").val();
            //alert(queryCondition);
            var recordTotal = $("#<%=hidrecordTotal.ClientID %>").val();
            var IsAll = $("#CkAll:checked").size();
            //alert(IsAll);
            var f = document.getElementById('iframeExport');
            f.src = "ExportPipeline.aspx?IDs=" +  IDs + "&action=Loans&recordTotal=" + recordTotal + "&IsAll=" + IsAll + "&queryCondition=" + queryCondition + "&t=" + Math.random().toString();
//          f.src = "ExportPipelineFnm32.aspx?IDs=" +  IDs + "&action=Loans&recordTotal=" + recordTotal + "&IsAll=" + IsAll + "&queryCondition=" + queryCondition + "&t=" + Math.random().toString();
           
            return false;
            
        }

        function onManagePipelineviews()
        {

           var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "../ManagePipelineViewsPopup.aspx?sid=" + sid

            var BaseWidth = 450;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.ShowGlobalPopup("Manage Pipeline Views", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            
        }

         function onAdvFilters()
        {

           var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "../LoanDetails/AdvancedLoanFilters.aspx?sid=" + sid

            window.location= iFrameSrc;
//            var BaseWidth = 450;
//            var iFrameWidth = BaseWidth + 2;
//            var divWidth = iFrameWidth + 25;

//            var BaseHeight = 500;
//            var iFrameHeight = BaseHeight + 2;
//            var divHeight = iFrameHeight + 40;

//            window.ShowGlobalPopup("Manage Pipeline Views", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            
        }

    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            // add optgroup
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Region']").wrapAll("<optgroup label='Region'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Division']").wrapAll("<optgroup label='Division'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Branch']").wrapAll("<optgroup label='Branch'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='LoanOfficer']").wrapAll("<optgroup label='Loan Officer'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Processor']").wrapAll("<optgroup label='Processor'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Underwriter']").wrapAll("<optgroup label='Underwriter'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Closer']").wrapAll("<optgroup label='Closer'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Doc Prep']").wrapAll("<optgroup label='Doc Prep'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Assistant']").wrapAll("<optgroup label='Loan Officer Assistant'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='Shipper']").wrapAll("<optgroup label='Shipper'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlOrgan option[value^='JrProcessor']").wrapAll("<optgroup label='JrProcessor'></optgroup>");

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLeadSource option[value^='LeadSource']").wrapAll("<optgroup label='Lead Source'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLeadSource option[value^='Partner']").wrapAll("<optgroup label='Partner'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLeadSource option[value^='Referral']").wrapAll("<optgroup label='Referral'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLeadSource option[value^='Lender']").wrapAll("<optgroup label='Lender'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLeadSource option[value^='LoanProgram']").wrapAll("<optgroup label='Loan Program'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLeadSource option[value^='Purpose']").wrapAll("<optgroup label='Purpose'></optgroup>");

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStage option[value^='All Stages-']").wrapAll("<optgroup label='All Stages'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStage option[value^='All Completed Stages-']").wrapAll("<optgroup label='All Completed Stages'></optgroup>");

            var startDate = $("#" + '<%=EstStartDate.ClientID %>');
            var endDate = $("#" + '<%=EstEndDate.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");
        });


        jQuery(document).ready(function ($) {

            $('#aMailChimp').addcontextmenu('divMailChimpMenu') //apply context menu to all images on the page 
        })

        function Subscribe() {


            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("contactid"));
                }
            });

            var ContactID = selctedItems.join(",");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var p = "";
            if ($.browser.msie == true) {   // ie

                p = "window.parent.SubscribeMailChimp";
            }
            else {   // firefox

                p = "window.parent.SubscribeMailChimp";
            }

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + ContactID + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            return false;
        }

        function SubscribeMailChimp(LID) {

            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("contactid"));
                }
            });

            var ContactID = selctedItems.join(",");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.parent.CloseGlobalPopup();
            window.parent.ShowWaitingDialog("Please wait...");
            
            // check exist
            $.getJSON("../Prospect/ProspectMailChimp_CheckExist_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactID + "&LID=" + LID, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        window.parent.parent.CloseWaitingDialog();
                        return;
                    }

                    var RadomNum2 = Math.random();
                    var Radom2 = RadomNum2.toString().substr(2);
                    
                    // Ajax
                    $.getJSON("../Prospect/ProspectMailChimp_Subscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactID + "&LID=" + LID, AfterSubscribeMailChimp);

                }, 2000);
            });
        }

        function AfterSubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Subscribe successfully.");

                    window.parent.CloseWaitingDialog();
                    window.location.href = window.location.href;
                }
                window.parent.CloseWaitingDialog();

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
                    selctedItems.push(item.attr("contactid"));
                }
            });

            var ContactID = selctedItems.join(",");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var p = "";
            if ($.browser.msie == true) {   // ie

                p = "window.parent.UnsubscribeMailChimp";
            }
            else {   // firefox

                p = "window.parent.UnsubscribeMailChimp";
            }

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + ContactID + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            
        }


        function UnsubscribeMailChimp(LID) {

            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("contactid"));
                }
            });

            var ContactID = selctedItems.join(",");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.parent.CloseGlobalPopup();
            window.parent.ShowWaitingDialog("Please wait...");
            // Ajax
            $.getJSON("../Prospect/ProspectMailChimp_Unsubscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactID + "&LIDs=" + LID, AfterUnsubscribeMailChimp);

        }

        function AfterUnsubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Unsubscribe successfully.");

                    window.parent.CloseWaitingDialog();
                    window.location.href = window.location.href;
                }
                window.parent.CloseWaitingDialog();

            }, 2000);
        }

//        var UserPipilineViewIsDefault = "<%=UserPipilineViewIsDefault.ClientID %>";

//        $("#<%=ddlLoanStatus.ClientID %>,#<%=ddlOrganType.ClientID %>,#<%=ddlOrgan.ClientID %>,#<%=ddlStage.ClientID %>,#<%=ddlLeadSourceType.ClientID %>,#<%=ddlLeadSource.ClientID %>,#<%=ddlDateType.ClientID %>,#<%=EstStartDate.ClientID %>,#<%=EstEndDate.ClientID %>").change(function(){
//            $("#"+ UserPipilineViewIsDefault).val(0);
//            alert(1);
//        });

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab" id="divTab" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li><a href="ProspectPipelineSummaryLoan.aspx"><span>Leads</span></a></li>
                                <li id="current"><a href="ProcessingPipelineSummary.aspx"><span>Loans</span></a></li>
                                <li><a href="ProspectPipelineSummary.aspx"><span>Clients</span></a></li>
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
                    <div id="divModuleName" class="ModuleTitle" style="margin-top: 10px; margin-left: 10px;">
                        Loans Pipeline Summary &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total
                        # Loans :&nbsp;<asp:Label ID="labelTotalLoans" runat="server" Text="Label"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total Volume :&nbsp;<asp:Label ID="labelTotalVolume"
                            runat="server" Text="Label"></asp:Label>
                    </div>
                    <div class="SplitLine" style="margin-top: 5px;">
                    </div>
                    <iframe id="RateLockDetail" frameborder="0" style="display: none; border: none; padding: 0;
                        margin: 0;"></iframe>
                    <iframe id="taskAlertDetail" frameborder="0" style="display: none; border: none;
                        padding: 0; margin: 0;"></iframe>
                    <iframe id="importErrorDetail" frameborder="0" style="display: none; border: none;
                        padding: 0; margin: 0;"></iframe>
                    <div id="divRuleAlert" title="Rule Alert" style="display: none;">
                        <iframe id="ifrRuleAlert" frameborder="0" scrolling="no" width="510px" height="610px">
                        </iframe>
                    </div>
                    <div style="padding-left: 0px; padding-right: 10px;">
                        <div id="divFilter" style="margin-top: 15px;">
                            <div>
                                <asp:DropDownList ID="ddlLoanStatus" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlLoanStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="Active Loans"></asp:ListItem>
                                    <asp:ListItem Text="All Loans"></asp:ListItem>
                                    <asp:ListItem Text="Archived Loans"></asp:ListItem>
                                </asp:DropDownList>
                                 
                                <asp:DropDownList ID="ddlUserPipelineView" runat="server" AutoPostBack="true" DataTextField="ViewName" DataValueField="UserPipelineViewID" style="width:150px;" OnSelectedIndexChanged="ddlUserPipelineView_SelectedIndexChanged">
                                </asp:DropDownList>

                                <asp:HiddenField ID="UserPipilineViewIsDefault" Value ="0" runat="server" />
                                 
                                 Borrower Last Name:
                                <asp:TextBox ID="btnBorrowerLastName" runat="server"></asp:TextBox>
                                <asp:Button ID="btnSearchLastName"  runat="server" Text="Search" class="Btn-66" OnClientClick="return SearchLastName();" />
                                &nbsp;&nbsp; Rate:<asp:TextBox ID="txbRateStart" Width="35" runat="server"></asp:TextBox> - <asp:TextBox ID="txbRateEnd" Width="35" runat="server"></asp:TextBox>
                                <input id="btSaveView" type="button" value=" Save View " onclick="SaveViewDialog();" class="Btn-91" /> 
                                <input id="btnManageView" type="button" value=" Manage View " class="Btn-91" onclick="onManagePipelineviews();return false;" />
                                <input id="btnAdvFilters" type="button" value=" Advanced Filters " class="Btn-91" onclick="onAdvFilters();return false;" />                                 
                                    
                            </div>
                            
                            <table cellpadding="0" cellspacing="0" style="margin-top: 3px;">
                                <tr id="trnone1" style=" display:none;">
                                    <td>
                                        <asp:DropDownList ID="ddlOrganType" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlOrganType_SelectedIndexChanged">
                                            <asp:ListItem Text="All organization types"></asp:ListItem>
                                            <asp:ListItem Text="Region"></asp:ListItem>
                                            <asp:ListItem Text="Division"></asp:ListItem>
                                            <asp:ListItem Text="Branch"></asp:ListItem>
                                            <asp:ListItem Text="Closer"></asp:ListItem>
                                            <asp:ListItem Text="Doc Prep"></asp:ListItem>
                                            <asp:ListItem Text="Loan Officer"></asp:ListItem>
                                            <asp:ListItem Text="Loan Officer Assistant"></asp:ListItem>
                                            <asp:ListItem Text="Processor"></asp:ListItem>
                                            <asp:ListItem Text="Shipper"></asp:ListItem>
                                            <asp:ListItem Text="Underwriter"></asp:ListItem>
                                            <asp:ListItem Text="Jr Processor"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrgan" runat="server" Width="120px" DataValueField="OrganID"
                                            DataTextField="OrganName">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStage" runat="server" Width="125px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLeadSourceType" runat="server" Width="100px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlLeadSourceType_SelectedIndexChanged">
                                            <asp:ListItem Text="All"></asp:ListItem>
                                            <asp:ListItem Text="Lead Source"></asp:ListItem>
                                            <asp:ListItem Text="Partner"></asp:ListItem>
                                            <asp:ListItem Text="Referral"></asp:ListItem>
                                            <asp:ListItem Text="Lender"></asp:ListItem>
                                            <asp:ListItem Text="Loan Program"></asp:ListItem>
                                            <asp:ListItem Text="Purpose"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLeadSource" runat="server" DataTextField="LeadSourceName"
                                            DataValueField="LeadSourceID" Width="100px">
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDateType" runat="server" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="EstStartDate" runat="server" CssClass="DateField"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="EstEndDate" runat="server" CssClass="DateField"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                                        </asp:Button>
                                        <asp:HiddenField ID="hidFilterQueryCondition" runat="server" />
                                        <asp:HiddenField ID="hidrecordTotal" runat="server" />
                                    </td>
                                </tr>
                                <%--<tr>

                                    <td colspan="3">&nbsp;&nbsp; Rate:<asp:TextBox ID="txbRateStart" Width="35" runat="server"></asp:TextBox> - <asp:TextBox ID="txbRateEnd" Width="35" runat="server"></asp:TextBox>
                                    <input id="btSaveView" type="button" value=" Save View " onclick="SaveViewDialog();" class="Btn-91" /> 
                                     <input id="btnManageView" type="button" value=" Manage View " class="Btn-91" onclick="onManagePipelineviews();return false;" />
                                      <input id="btnAdvFilters" type="button" value=" Advanced Filters " class="Btn-91" onclick="onAdvFilters();return false;" />
                                    </td>
                                    <td colspan="5"></td>
                                </tr>--%>
                            </table>
                            <div id="SaveViewShow" style=" display:none;">
                                <div style=" margin-top:15px;">
                                View Name:<input type="text" id="txtViewName" value="" style=" width:140px;" /> <input type="button" id="btnSaveVN" class="Btn-66" value="Save" />
                                </div>
                            </div> 
                            
                            <div style="display:none" ><asp:TextBox ID="txtSaveViewName" runat="server" Width="140"></asp:TextBox>  <asp:Button ID="btnSaveView" runat="server" Text="Save" class="Btn-66" OnClientClick="return checkViewName()" OnClick="btnSaveView_OnClick" /> </div>
                            
                            <script>
                                $("#btnSaveVN").click(function () {
                                    if ($.trim($("#txtViewName").val()) == "") {
                                        alert("Please enter View Name.");
                                        return ;
                                    }
                                    //alert("<%=txtSaveViewName.ClientID%>"+"&"+<%=btnSaveView.ClientID%>);
                                    $("#<%=txtSaveViewName.ClientID%>").val($("#txtViewName").val());
                                    $("#<%=btnSaveView.ClientID%>").click();

                                });
                            </script>
                        </div>
                        <div id="divToolBar" style="margin-top: 15px;">
                            <table cellpadding="0" cellspacing="0" style="width: 1000px;">
                                <tr>
                                    <td style="width: 40px;">
                                        <asp:DropDownList ID="ddlAlphabets" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlphabets_SelectedIndexChanged">
                                            <asp:ListItem Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 720px;">
                                        <ul class="ToolStrip">
                                            <li style="display: none;"><a id="aCreate" href="#">Setup</a><span>|</span></li>
                                            <li><a id="btnDetails" href="#">Details</a><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnSync" runat="server" Text="Sync" OnClick="btnSync_Click"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnImportError" runat="server" Text="Handle Point Errors" OnClientClick="onImportErrorBtnClick(); return false;"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnDispose" runat="server" Text="Dispose" OnClick="btnDispose_Click"
                                                    OnClientClick="return false;"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="lbtnSetup" runat="server" Text="Setup" OnClientClick="SetupSelectedItem();return false;"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnMove" OnClientClick="onMoveClick();return false;" runat="server"
                                                    Text="Move File"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnSearch" runat="server" Text="Search" OnClientClick="onSearchClick(); return false;"></asp:LinkButton><span>|</span></li>
                                            <li><a id="aClear" href="javascript:ClearSearch()">Clear</a></li>
                                            <li><span>|</span><a id="aMailChimp" href="#">Mail Chimp</a></li>
                                            <li><span>|</span><asp:LinkButton ID="aExport" OnClientClick="onExportClick();return false;" runat="server"
                                                    Text="Export"></asp:LinkButton></li>
                                        </ul>
                                        <div id="menu1" class="contextMenu" style="display: none;">
                                            <ul>
                                                <li id="convertToLead">Convert to Lead</li>
                                                <%--CR063 <li id="suspend">Suspend</li>--%>
                                                <li id="deny">Deny</li>
                                                <li id="cancel">Cancel</li>
                                                <li id="resume">Resume</li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td style="text-align: right;">
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
                    <div id="divDivision" class="ColorGrid" style="width: 1000px; margin-top: 5px;">
                        <asp:GridView ID="gvPipelineView" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                            OnSorting="gvPipelineView_Sorting" CellPadding="3" GridLines="None" DataKeyNames="FileId,Status,BranchID"
                            OnRowDataBound="gvPipelineView_RowDataBound" OnPreRender="gvPipelineView_PreRender">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate>
                                        <input id="CkAll" type="checkbox" onclick="onCkAllRowSelected(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="checkbox" tag='<%# Eval("FileId") %>' onclick="onCkRowClicked(this, '<%# Eval("FileId") %>', '<%# Eval("Status") %>', '<%# Eval("BranchID") %>')"
                                            contactid='<%# Eval("ContactId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Borrower" SortExpression="Borrower" ItemStyle-Wrap="false"
                                    ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <a href="#" class="loanDetails" tag='<%# Eval("FileId") %>'>
                                            <%# Eval("Borrower")%></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Stage" SortExpression="Stage" ItemStyle-Wrap="false"
                                    ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <label <%# this.GetCurrentStageTooltip(Convert.ToInt32(Eval("FileId").ToString())) %> ><%# Eval("Stage")%></label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Purpose" SortExpression="Purpose"
                                    ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Purpose")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Last Compl Stage" SortExpression="LastCompletedStage"
                                    ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("LastCompletedStage")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Completed Stage Date" SortExpression="LastStageComplDate"
                                    ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("LastStageComplDate", "{0:d}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Alerts" SortExpression="RateLockIcon"
                                    HeaderStyle-Width="80px" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <span class="alert">
                                            <img id="imgRateLockIcon" class="RateLockIcon" tag='<%# Eval("FileId") %>' src='../images/loan/<%# Eval("RateLockIcon")%>'
                                                width="16" height="16" />
                                            <img id="imgImportErrorIcon" class="ImportErrorIcon" tag='<%# Eval("FileId") %>'
                                                src='../images/loan/<%# Eval("ImportErrorIcon")%>' width="16" height="16" />
                                            <img id="imgAlertIcon" class="AlertIcon" tag='<%# Eval("FileId") %>' src='../images/loan/<%# Eval("AlertIcon")%>'
                                                width="16" height="16" />
                                            <img id="imgRuleAlertIcon" class="RuleAlertIcon" tag='<%# Eval("FileId") %>' myalertid='<%# Eval("AlertID") %>'
                                                src='../images/alert/<%# Eval("RuleAlertIcon")%>' width="16" height="16" />
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Progress" SortExpression="Progress" HeaderStyle-Width="145">
                                    <ItemTemplate>
                                        <div class="ProgressContainer">
                                            <div class="completed" style='width: <%# Eval("Progress")%>%'>
                                                &nbsp;</div>
                                            <div class="content">
                                                <%# Eval("Progress")%>%</div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Est Close" SortExpression="EstClose" ItemStyle-HorizontalAlign="Right"
                                    ItemStyle-Wrap="false" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <%# Eval("EstClose", "{0:d}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Loan Officer" SortExpression="Loan Officer" ItemStyle-Wrap="false"
                                    ItemStyle-Width="140px">
                                    <ItemTemplate>
                                        <%# Eval("Loan Officer")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount" ItemStyle-Wrap="false"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("Amount", "{0:c0}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lien" SortExpression="Lien" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Lien")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" SortExpression="Rate" ItemStyle-Wrap="false"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("Rate","{0:N4}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lender" SortExpression="Lender" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Lender")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lock Expiration Date" SortExpression="Lock Expiration Date"
                                    ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Lock Expiration Date","{0:d}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch" SortExpression="Branch" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Branch")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Processor" SortExpression="Processor" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Processor")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Task Count" SortExpression="Task Count" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Task Count")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Point Folder" SortExpression="Point Folder" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Point Folder")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Filename" SortExpression="Filename" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Filename")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                

                                <asp:TemplateField HeaderText="Closer" SortExpression="Closer" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Closer")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc Prep" SortExpression="DocPrep" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("DocPrep")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Assistant" SortExpression="Assistant" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Assistant")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shipper" SortExpression="Shipper" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Shipper")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Loan Program" SortExpression="LoanProgram" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("LoanProgram")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Jr Processor" SortExpression="JrProcessor" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("JrProcessor")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Last Note" SortExpression="LastNote" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                       <div  title='<%# this.GetLastNoteTooltip(Eval("LastNote").ToString()) %>' > <%# Eval("LastNote").ToString().Length > 80 ? Eval("LastNote").ToString().Substring(0,80) + "..." : Eval("LastNote").ToString() %></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">
                            &nbsp;</div>
                    </div>
                    <asp:HiddenField ID="hfDeleteItems" runat="server" />
                    <asp:HiddenField ID="hiSelectedLoan" runat="server" />
                    <asp:HiddenField ID="hiSelectedLoanStatus" runat="server" />
                    <asp:HiddenField ID="hiSelectedFolderId" runat="server" />
                    <asp:HiddenField ID="hiSelectedDisposal" runat="server" />
                    <asp:HiddenField ID="hiSearchFilter" runat="server" />
                    <div style="display: none;">
                        <div id="dialogPointImportErrorList">
                            <iframe id="iframeImportErrorList" name="iframeImportErrorList" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="dialog1" title="Point Folder Selection">
                            <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
                            </iframe>
                        </div>
                        <div id="dialogPointImportErrorList1">
                            <iframe id="iframeImportErrorList1" name="iframeImportErrorList1" frameborder="0"
                                width="100%" height="100%"></iframe>
                        </div>
                        <div id="dialogPointFolder">
                            <iframe id="iframePointFolder" name="iframePointFolder" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="ExportDiv">
                            <iframe id="iframeExport" name="iframeExport" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <ul id="divMailChimpMenu" class="jqcontextmenu">
            <li>
                <asp:LinkButton ID="lbtnSubscribe" runat="server" OnClientClick="Subscribe();return false;">Subscribe</asp:LinkButton></li>
            <li>
                <asp:LinkButton ID="lbtnUnsubscribe" runat="server" OnClientClick="Unsubscribe();return false;">Unsubscribe</asp:LinkButton></li>
        </ul>

    </div>
</asp:Content>
