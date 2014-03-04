<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Prospect Pipeline Summary - Loan View" Language="C#" AutoEventWireup="true"
    CodeBehind="ProspectPipelineSummaryLoan.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Pipeline.ProspectPipelineSummaryLoan"
    MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqcontextmenu.css" rel="stylesheet" type="text/css" />

	<script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.minV1.8.17.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jqcontextmenu.js"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" >

        $(document).ready(function () {

            $("#" + '<%=lbtnDispose.ClientID %>').addcontextmenu("menu1");

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStages option[value^='All Stages-']").wrapAll("<optgroup label='All Stages'></optgroup>");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStages option[value^='All Completed Stages-']").wrapAll("<optgroup label='All Completed Stages'></optgroup>");

        })

        function DisposeClick() {
            //             if (SelectedLoanCount() == 0) {
            //                 alert("No record has been selected.");
            //                 return false;
            //             }
            //             if (SelectedLoanCount() > 1) {
            //                 alert("Only one record can be selected for this operation.");
            //                 return false;
            //             }
            //             else {

            var loanId = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            var loan = GetLoanInfoById(loanId);
            var s = loan.Status.toLowerCase();
            if (s == "active") {
                $("#Resume").remove();
            }
            else {
                $("#Archive").remove();
                $("#Convert").remove();
            }
            return false;
            //            }
        };

    </script>

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
    <style type="text/css">
        .TabContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabContent input[type="text"], select, input[type="file"]
        {
            margin-left: 15px;
        }
        #divUserList td
        {
            margin-left: 0px;
            margin-right: 0px;
            padding: 0;
        }
    </style>
    <script type="text/javascript">
        Array.prototype.remove = function (s) {
            var nIndex = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i] == s) {
                    nIndex = i;
                    break;
                }
            }
            if (nIndex != -1) {
                this.splice(nIndex, 1);
                return true;
            }
            else
                return false;
        }

        function CheckAllClicked(me, areaID, hiAllIDs, hiSelectedIDs) {
            var bCheck = $(me).attr('checked');
            if (bCheck) {
                // copy all ids to selected id holder
                $('#' + hiSelectedIDs).val($('#' + hiAllIDs).val());
            }
            else
                $('#' + hiSelectedIDs).val('');
            //$('input:checkbox', $('#' + areaID)).each(function () { $(this).attr('checked', bCheck); });

            $("#<%=gridList.ClientID %> .CheckBoxColumn input").each(function () { $(this).attr('checked', bCheck); });
        }

        function CheckBoxClicked(me, ckAllID, hiAllIDs, hiSelectedIDs, id, hiSelectContactIDs, contactID) {
            var sAllIDs = $('#' + hiAllIDs).val();
            var sSelectedIDs = $('#' + hiSelectedIDs).val();
            var sSelectContactIDs = $('#' + hiSelectContactIDs).val();
            var allIDs = new Array();
            var selectedIDs = new Array();
            var selectedContactIDs = new Array();
            if (sAllIDs.length > 0)
                allIDs = sAllIDs.split(',');

            if (sSelectedIDs.length > 0)
                selectedIDs = sSelectedIDs.split(',');

            if (sSelectContactIDs.length > 0)
                selectedContactIDs = sSelectContactIDs.split(',');

            if ($(me).attr('checked')) {
                selectedIDs.push(id);
                selectedContactIDs.push(contactID);
            }
            else {
                selectedIDs.remove(id);
                selectedContactIDs.remove(contactID);
            }

            // set the CheckAll check box checked status
            // $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);

            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');

            if (selectedContactIDs.length > 0)
                $('#' + hiSelectContactIDs).val(selectedContactIDs);
            else
                $('#' + hiSelectContactIDs).val('');
        }
    </script>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        var bId = -1;
        var fId = -1;
        var sHasView = "<%=sHasViewRight %>";
        $(document).ready(function () {
            DrawTab();


            var startDate = $("#" + '<%=tbEstCloseDateStart.ClientID %>');
            var endDate = $("#" + '<%=tbEstCloseDateEnd.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");

            ChangeDateType();

            initSearchWin();
            initTaskAlertWin();
            initImportErrorWin();
            initRuleAlertWin();
            initImportErrorListWin();


            initCfmBox('dialogPointFolder', 'Point Folder Selection', '', 500, 450, function () {
                if (window.frames.iframePointFolder && window.frames.iframePointFolder.returnFn) {
                    window.frames.iframePointFolder.returnFn();
                }
            });
            initCfmBox('dialogWkTplt', 'Workflow Template Selection', '', 500, 450, function () {
                if (window.frames.iframeWfTplt && window.frames.iframeWfTplt.returnFn) {
                    window.frames.iframeWfTplt.returnFn();
                }
            });

        });



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
                    }
                },
                open: function () {
                    $('.ui-dialog-buttonset').css('float', 'none');
                    $('.ui-dialog-buttonset').css('text-align', 'center');
                    $('.ui-dialog-buttonpane').find('button').addClass('Btn-66');
                }
            });
        }
// ]]>
    </script>
    <script type="text/javascript">
        function initSearchWin() {
            $('#dialogSearchLoans').dialog({
                modal: true,
                autoOpen: false,
                title: 'Search Loans',
                width: 600,
                height: 350,
                resizable: false,
                close: clearSearchWin
            });
        }
        function showSearchWin() {
            var f = document.getElementById('iframeSearchLoans');
            f.src = "SearchLoans.aspx?t=" + Math.random().toString();
            $('#dialogSearchLoans').dialog('open');
        }
        function clearSearchWin() {
            var f = document.getElementById('iframeSearchLoans');
            f.src = "about:blank";
        }
        function closeSearchWin(bRefresh, bResetPager) {
            $('#dialogSearchLoans').dialog('close');
            if (bRefresh === true) {
            }
        }

        function getSearchFilterReturn(sFilter) {
            if (null != sFilter)
            {
                $("#" + "<%=hiSearchFilter.ClientID %>").val(sFilter);
                <%=this.ClientScript.GetPostBackEventReference(this.lbtnSearchFilter, null) %>;
            }
        }
    </script>
    <script type="text/javascript">
        var sTargetStatus = "";
        function showPFWin() {
            var f = document.getElementById('iframePointFolder');
            f.src = "PointFolderSelection.aspx?forProspect=1&fid=" + fId + "&tls=" + sTargetStatus + "&bid=" + bId + "&t=" + Math.random().toString();
            $('#dialogPointFolder').dialog('open');
        }
        function clearPFWin() {
            var f = document.getElementById('iframePointFolder');
            f.src = "about:blank";
        }

        //        function getFolderSelectionReturn(sReturn) {
        //            $("#" + '<%=hiSelectedFolderId.ClientID %>').val(sReturn);
        //            $('#dialogPointFolder').dialog('close');
        //            clearPFWin();
        //            InvokeDisposePostback();
        //        }
    </script>
    <script type="text/javascript">
        function initTaskAlertWin() {
            $('#dialogTaskAlert').dialog({
                modal: true,
                autoOpen: false,
                title: 'Task Alert Detail',
                width: 600,
                height: 330,
                resizable: false,
                close: clearTaskAlertWin
            });
        }
        function showTaskAlertWin(mode, id) {
            var f = document.getElementById('iframeTaskAlert');
            if (null == mode || "" == mode)
                mode = "0";
            if (null == id)
                id = "";
            f.src = "TaskAlertDetail.aspx?mode=" + mode + "&fileID=" + id + "&t=" + Math.random().toString();
            $('#dialogTaskAlert').dialog('open');
        }
        function clearTaskAlertWin() {
            var f = document.getElementById('iframeTaskAlert');
            f.src = "about:blank";
        }
        // call by task alert detail page
        function closeDialog() {
            $('#dialogTaskAlert').dialog('close');
        }
    </script>
    <script type="text/javascript">
        function showWkTpltWin() {
            var f = document.getElementById('iframeWfTplt');
            f.src = "WfTpltSelection.aspx?wt=Processing&t=" + Math.random().toString();
            $('#dialogWkTplt').dialog('open');
        }
        function clearWkTpltWin() {
            var f = document.getElementById('iframeWfTplt');
            f.src = "about:blank";
        }

        function getWfTpltSelectionReturn(sReturn) {
            $("#" + '<%=hiWfTpltId.ClientID %>').val(sReturn);
            $('#dialogWkTplt').dialog('close');
            clearWkTpltWin();
            <%=this.ClientScript.GetPostBackEventReference(this.lbtnGenWorkflow, null) %>;
        }
    </script>
    <script type="text/javascript">
        function LoanInfo(sID, sStatus, sBId) {
            this.ID = sID;
            this.Status = sStatus;
            this.BranchID = sBId;
        }
        function SelectedLoanCount() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (null == sIds || sIds.length == 0)
                return 0;
            var arrIds = sIds.split(",");
            return arrIds.length;
        }
        function GetLoanInfoById(sId) {
            var sLoanInfo = $("#" + "<%=hiLoanInfo.ClientID %>").val();
            var arrLoanInfo = sLoanInfo.split(";");
            for (var i = 0; i < arrLoanInfo.length; i++) {
                var arrTemp = arrLoanInfo[i].split(":");
                if (arrTemp.length == 3 && arrTemp[0] == sId) {
                    return new LoanInfo(arrTemp[0], arrTemp[1], arrTemp[2]);
                }
            }
            return null;
        }

        function detailBtnClicked(fId) { 
            var allLoanIds = $("#" + "<%=hiAllIds.ClientID %>").val();
            if (null == fId || "" == fId)
            {
                if (SelectedLoanCount() > 1) {
                    alert("Only one record can be selected for this operation.");
                    return false;
                }
                else if (SelectedLoanCount() == 1) {
                    var loanId = $("#" + "<%=hiCheckedIds.ClientID %>").val();
                    if (sHasView =="1") {
                    window.location.href = '../Prospect/ProspectLoanDetails.aspx?FromPage=<%= FromURL %>&FileID=' + loanId + "&FileIDs=" + allLoanIds;
                    }
                }
                else {
                    alert("No record has been selected.");
                    return false;
                }
            }
            else
            {
                if (sHasView =="1") {
                window.location.href = '../Prospect/ProspectLoanDetails.aspx?FromPage=<%= FromURL %>&FileID=' + fId + "&FileIDs=" + allLoanIds;
                }
            }
       }
        function syncBtnClicked() {
            if (SelectedLoanCount() <= 0) {
                alert("No record has been selected.");
                return false;
            }
        }

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

        function markasbakLoan()
        {        
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Bad');
            sTargetStatus = "bak";
            showPFWin();
        }
        function convertedLoan()
        {

            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Converted');
            sTargetStatus = "converted";
            // show folder selection window
            showPFWin();

        }
        function cancelLoan()
        {
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Canceled');
            sTargetStatus = "cancel";
            showPFWin();
        }
        function suspendLoan()
        {
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Suspended');
            sTargetStatus = "suspend";
            showPFWin();
        }
        function activateLoan()
        {
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Active');
            sTargetStatus = "active";
            showPFWin();
        }
        function InvokeDisposePostback()
        {
            <%=this.ClientScript.GetPostBackEventReference(this.lbtnDispose, null) %>
        }

        function removeBtnClicked() {
            if (SelectedLoanCount() <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else {
                return confirm('This will delete the loan, email, alert, workflow history and activities records. '
                    + 'Are you sure you want to continue?');
            }
        }
    </script>
    <script type="text/javascript">
// <![CDATA[
        //#region neo codes

        function GoToProspectLoanDetails(CurrentFileID) {

            var FileIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr td a[id='aBorrower']").each(function (i) {

                if (i == 0) {

                    FileIDs = $(this).attr("myFileID");
                }
                else {

                    FileIDs += "," + $(this).attr("myFileID");
                }
            });

            //alert(FileIDs);

            window.location.href = "../Prospect/ProspectLoanDetails.aspx?FromPage=" + encodeURIComponent(window.location.href) + "&FileID=" + CurrentFileID + "&FileIDs=" + FileIDs;
        }

        //#endregion
// ]]>
    </script>
    <script type="text/javascript">
        function initImportErrorWin() {
            $('#dialogImportError').dialog({
                modal: true,
                autoOpen: false,
                title: 'Point Import Alert Detail',
                width: 640,
                height: 520,
                resizable: false,
                close: clearImportErrorWin
            });
        }
        function showImportErrorWin(id, sIcon) {
            var f = document.getElementById('iframeImportError');
            if (null == id)
                id = "";
            if (null == sIcon)
                sIcon = "";
            f.src = "PopupPointImportAlert.aspx?fileID=" + id + "&icon=" + sIcon + "&t=" + Math.random().toString();
            $('#dialogImportError').dialog('open');
        }
        function clearImportErrorWin() {
            var f = document.getElementById('iframeImportError');
            f.src = "about:blank";
        }
        // call by import error detail page
        function closeImportErrorDialog() {
            $('#dialogImportError').dialog('close');
        }
    </script>
    <script type="text/javascript">
        function initRuleAlertWin() {
            $('#dialogRuleAlert').dialog({
                modal: true,
                autoOpen: false,
                title: 'Rule Alert',
                height: 650,
                width: 535,
                resizable: false,
                close: clearRuleAlertWin
            });
        }
        function showRuleAlertWin(loanId, alertId) {
            var f = document.getElementById('iframeRuleAlert');
            if (null == loanId)
                loanId = "";
            if (null == alertId)
                alertId = "";
            f.src = "../LoanDetails/RuleAlertPopup.aspx?LoanID=" + loanId + "&AlertID=" + alertId + "&t=" + Math.random().toString();
            $('#dialogRuleAlert').dialog('open');
        }
        function clearRuleAlertWin() {
            var f = document.getElementById('iframeRuleAlert');
            f.src = "about:blank";
        }
        // call by import error detail page
        function CloseDialog_RuleAlert() {
            $('#dialogRuleAlert').dialog('close');
        }
    </script>
    <script type="text/javascript">    
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
        function onImportErrorBtnClick() {
            var f = document.getElementById('iframeImportErrorList');
            var q = $.QueryString["q"];
            if (q == null)
                q = "";
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

        function onMoveClick()
        {
          if (SelectedLoanCount() > 1) {
                alert("Only one record can be selected for this operation.");
                return false;
            }
            else if (SelectedLoanCount() == 1) {
                var loanId = $("#" + "<%=hiCheckedIds.ClientID %>").val();
                //gdc CR50
                CheckPointFileStatus(loanId,function(){
                
                    var loan = GetLoanInfoById(loanId);

                     var s = loan.Status.toLowerCase();
                     bId = loan.BranchID;
                    $("#"+'<%=hiSelectedDisposal.ClientID %>').val('move');
                    var f = document.getElementById('iframePointFolder');
                    f.src = "PointFolderSelection.aspx?forProspect=1&fid=" + loanId + "&tls="+ s +"&bid=" + bId + "&t=" + Math.random().toString();
                    $('#dialogPointFolder').dialog('open');
                });
                return false;
            }
                else {
                    alert("No record has been selected.");
                    return false;
                }
        }
          
        function ClearSearch()
        {
//         $("#" + "<%=hiSearchFilter.ClientID %>").val("");
//            <%=this.ClientScript.GetPostBackEventReference(this.lbtnSearchFilter, null) %>;
            window.location.href = window.location.pathname;
        }      

        function ChangeDateType()
        {
            var sDateType=  $("#" + "<%=ddlDataType.ClientID %>").val();
            if(sDateType=="-1")
            {
                $("#" + "<%=tbEstCloseDateStart.ClientID %>").val(""); 
                $("#" + "<%=tbEstCloseDateEnd.ClientID %>").val(""); 
                $("#" + "<%=tbEstCloseDateStart.ClientID %>").attr("disabled",true); 
                $("#" + "<%=tbEstCloseDateEnd.ClientID %>").attr("disabled",true); 
            }
            else
            {
                 $("#" + "<%=tbEstCloseDateStart.ClientID %>").attr("disabled",false); 
                $("#" + "<%=tbEstCloseDateEnd.ClientID %>").attr("disabled",false); 
            }
        }
        function getFolderSelectionReturn(sReturn)
        {
            $("#"+'<%=hiSelectedFolderId.ClientID %>').val(sReturn);
            $('#dialog1').dialog('close');
            <%=this.ClientScript.GetPostBackEventReference(this.lbtnDispose, null) %>
            clearPFWin();
        }

        function DisposeLoan(status)
        {
            if (SelectedLoanCount() > 1) {
                alert("Only one record can be selected for this operation.");
                return false;
            }
            else if (SelectedLoanCount() == 1) {
//                var loanId = $("#" + "<%=hiCheckedIds.ClientID %>").val();
//                var loan = GetLoanInfoById(loanId);
//                if (null == loan) {
//                    // unknow error
//                }
//                else {
//                    var s = loan.Status.toLowerCase();
//                    if (s == "active")
//                    {
//                    }
//                    else {
//                        alert("Unable to process the request for the selected Prospect. The Prospect has Disposed.");
//                        return false;
//                    }
//                }
            }
            else {
                alert("No record has been selected.");
                return false;
            }
            if(status =="Convert") //gdc CR50
            {
                var loanId = $("#" + "<%=hiCheckedIds.ClientID %>").val();
                 CheckPointFileStatus(loanId,function(){
                    tLS=status; 
                    ShowFolerSelWin();
                });
            }
            else
            {
                tLS=status; 
                ShowFolerSelWin();
            }
        }


        function ShowFolerSelWin() {
                
                var loanID = $("#" + '<%=hiCheckedIds.ClientID %>').val();
                var filename =$("a[myfileid=" + loanID + "]").attr("filename");
                var iFrameSrc ="../Prospect/DisposePointFolderList.aspx?LoanID=" + loanID + "&Action=" + tLS + "&BranchID=" + bId + "&sid=" + Math.random().toString() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";
                
                
                if(filename=="" && tLS!="Convert")
                {
                     $("#iFrameAction").attr("src", iFrameSrc);
                }
                else
                {
                    var BaseWidth = 601
                    var iFrameWidth = BaseWidth + 2;
                    var divWidth = iFrameWidth + 25;

                    var BaseHeight = 600;
                    var iFrameHeight = BaseHeight + 2;
                    var divHeight = iFrameHeight + 40;

                    window.parent.ShowGlobalPopup("Dispose Point Folder List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
                }
            }

    </script>
    
    <script language="javascript" type="text/javascript">

        var gridId = "#<%=gridList.ClientID %>";
        jQuery(document).ready(function ($) {

            $('#aMailChimp').addcontextmenu('divMailChimpMenu') //apply context menu to all images on the page 
        })

        function Subscribe() {

            //            var selctedItems = new Array();
            //            $(gridId + " :checkbox:gt(0)").each(function () {
            //                var item = $(this);
            //                if (item.attr("checked")) {
            //                    selctedItems.push(item.attr("contactid"));
            //                }
            //            });

            //            var ContactIDs = selctedItems.join(",");
            var ContactIDs = $("#" + "<%=hiCheckedContactIds.ClientID %>").val();

            if (ContactIDs == "") {
                alert("No record has been selected.");
                return false;
            }

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

            window.parent.parent.ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            return false;
        }

        function SubscribeMailChimp(LID) {

            //            var selctedItems = new Array();
            //            $(gridId + " :checkbox:gt(0)").each(function () {
            //                var item = $(this);
            //                if (item.attr("checked")) { 
            //                    selctedItems.push(item.attr("contactid"));
            //                }
            //            });

            //            var ContactIDs = selctedItems.join(",");
            var ContactIDs = $("#" + "<%=hiCheckedContactIds.ClientID %>").val();
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.parent.CloseGlobalPopup();
            window.parent.ShowWaitingDialog("Please wait...");

            // check exist
            $.getJSON("../Prospect/ProspectMailChimp_CheckExist_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactIDs + "&LID=" + LID, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        window.parent.parent.CloseWaitingDialog();
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

                    alert("Subscribe successfully.");

                    window.parent.CloseWaitingDialog();
                    window.location.href = window.location.href;
                }
                window.parent.CloseWaitingDialog();

            }, 2000);
        }

        function Unsubscribe() {

            //            var selctedItems = new Array();
            //            $(gridId + " :checkbox:gt(0)").each(function () {
            //                var item = $(this);
            //                if (item.attr("checked")) {
            //                    selctedItems.push(item.attr("contactid"));
            //                }
            //            });

            //            var ContactIDs = selctedItems.join(",");
            var ContactIDs = $("#" + "<%=hiCheckedContactIds.ClientID %>").val();
            if (ContactIDs == "") {
                alert("No record has been selected.");
                return false;
            }

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

            window.parent.parent.ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }


        function UnsubscribeMailChimp(LID) {

            //            var selctedItems = new Array();
            //            $(gridId + " :checkbox:gt(0)").each(function () {
            //                var item = $(this);
            //                if (item.attr("checked")) {
            //                    selctedItems.push(item.attr("contactid"));
            //                }
            //            });

            //            var ContactIDs = selctedItems.join(",");
            var ContactIDs = $("#" + "<%=hiCheckedContactIds.ClientID %>").val();
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.parent.CloseGlobalPopup();
            window.parent.ShowWaitingDialog("Please wait...");
            // Ajax
            $.getJSON("../Prospect/ProspectMailChimp_Unsubscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactIDs + "&LIDs=" + LID, AfterUnsubscribeMailChimp);

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

        //gdc CR45

        function SaveViewDialog() {
            $('#SaveViewShow').dialog({
                modal: false,
                title: "Save View",
                width: 350,
                height: 120,
                resizable: false,
//                position: [$("#btSaveView").offset().top + 200, $("#btSaveView").offset().left - 180]
            });
            $('#SaveViewShow').dialog('open');

        }

        function onExportClick() {

            var IDs = $("#" + "<%=hiCheckedIds.ClientID %>").val();

            if (IDs == "") {
                alert("Please select the leads to be exported.");
                return false;
            }

            //hidFilterQueryCondition
            var queryCondition = $("#<%=hidFilterQueryCondition.ClientID %>").val();
            //alert(queryCondition);
            var recordTotal = $("#<%=hidrecordTotal.ClientID %>").val();
            var IsAll = $("#<%=strCkAllID %>:checked").size();
            //alert(IsAll);
            var f = document.getElementById('iframeExport');
            f.src = "ExportPipeline.aspx?IDs=" + IDs + "&action=Leads&recordTotal=" + recordTotal + "&IsAll=" + IsAll + "&queryCondition=" + queryCondition + "&t=" + Math.random().toString();

            return false;

        }

        function onManagePipelineviews() {

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
		
		function SearchLastName()
        {
           var lname = $.trim($("#" + "<%=txtBorrowerLastName.ClientID %>").val());
            var sFilter = "lname\u0002" + lname;
            //alert(sFilter);
            
            $("#" + "<%=hiSearchFilter.ClientID %>").val(sFilter);
            <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="MainArea" runat="server">
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="ProspectPipelineSummaryLoan.aspx"><span>Leads</span></a></li>
                                <li><a href="ProcessingPipelineSummary.aspx"><span>Loans</span></a></li>
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
                        Leads Pipeline Summary</div>
                    <div class="SplitLine" style="margin-top: 5px;">
                    </div>
                    <div style="padding-left: 0px; padding-right: 10px;">
                        <div id="divFilter" style="margin-top: 15px;">
                            <div>
									<table cellspacing="0" cellpadding="0">
										<tr>
											<td>
												<asp:DropDownList ID="ddlViewType" runat="server" Width="150px" AutoPostBack="true"
				                                    OnSelectedIndexChanged="ddlViewType_SelectedIndexChanged">
				                                    <asp:ListItem Text="Active Leads" Value="0" Selected="True"></asp:ListItem>
				                                    <asp:ListItem Text="All Leads" Value="1"></asp:ListItem>
				                                    <asp:ListItem Text="Archived Leads" Value="2"></asp:ListItem>
				                                </asp:DropDownList>
											</td>
											<td>
												<asp:DropDownList ID="ddlUserPipelineView" runat="server" AutoPostBack="true" DataTextField="ViewName" DataValueField="UserPipelineViewID" OnSelectedIndexChanged="ddlUserPipelineView_SelectedIndexChanged">
                                				</asp:DropDownList>
                                				<asp:HiddenField ID="UserPipilineViewIsDefault" Value ="0" runat="server" />
											</td>
											<td style="padding-left:15px;">
												Borrower Last Name: <asp:TextBox ID="txtBorrowerLastName" runat="server" style="margin:5px;"></asp:TextBox>
											</td>
											<td style="padding-left:5px;">
												<asp:Button ID="btnSearchLastName"  runat="server" Text="Search" class="Btn-66" OnClientClick="return SearchLastName()" />
											</td>
											<td style="padding-left:40px;">
												<input id="btSaveView" type="button" value=" Save View " onclick="SaveViewDialog();" class="Btn-91" /> 
                                				<input id="btnManageView" type="button" value=" Manage View " class="Btn-91" onclick="onManagePipelineviews();return false;" />
											</td>
										</tr>
									</table>
                                
                                <div id="SaveViewShow" style=" display:none;">
                                    <div style=" margin-top:15px;">
                                    View Name:<input type="text" id="txtViewName" value="" style=" width:140px;" /> <input type="button" id="btnSaveVN" class="Btn-66" value="Save" />
                                    </div>
                                </div> 
                            
                                <div style="display:none" ><asp:TextBox ID="txtSaveViewName" runat="server" Width="140"></asp:TextBox>  <asp:Button ID="btnSaveView" runat="server" Text="Save" class="Btn-66" OnClick="btnSaveView_OnClick" /> </div>
                            
                                <script>
                                    $("#btnSaveVN").click(function () {
                                        //alert("<%=txtSaveViewName.ClientID%>"+"&"+<%=btnSaveView.ClientID%>);
                                        $("#<%=txtSaveViewName.ClientID%>").val($("#txtViewName").val());
                                        $("#<%=btnSaveView.ClientID%>").click();

                                    });
                                </script>
                            </div>
                            <table cellpadding="0" cellspacing="0" style="margin-top: 3px;">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlOrganizationTypes" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlOrganizationTypes_SelectedIndexChanged">
                                            <asp:ListItem Text="All organization types" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Region" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Division" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Branch" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Loan Officer" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Processor" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrganization" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLeadSourceType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeadSourceType_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Lead Sources" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Partners" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Referrals" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLeadSource" runat="server">
                                            <asp:ListItem Text="All Lead Sources" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStages" runat="server">
                                            <asp:ListItem Text="All Stages" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDataType" runat="server" onchange="javascript:ChangeDateType()">
                                            <asp:ListItem Text="All dates" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Est Close date" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Creation date" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEstCloseDateStart" runat="server" CssClass="DateField"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbEstCloseDateEnd" runat="server" CssClass="DateField"></asp:TextBox>
                                    </td>
                                    <td style="padding-left: 15px;">
                                        <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                                        </asp:Button>
                                        <asp:LinkButton ID="lbtnSearchFilter" runat="server" OnClick="lbtnSearchFilter_Click"></asp:LinkButton>

                                        <asp:HiddenField ID="hidFilterQueryCondition" runat="server" />
                                        <asp:HiddenField ID="hidrecordTotal" runat="server" />
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td align="left">
                                        <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                            <asp:ListItem Text="All Regions" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                            <asp:ListItem Text="All Divisions" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBranch" runat="server">
                                            <asp:ListItem Text="All Branches" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divToolBar" style="margin-top: 13px;">
                            <table cellpadding="0" cellspacing="0" border="0" style="width: 1000px;">
                                <tr>
                                    <td style="width: 40px;">
                                        <asp:DropDownList ID="ddlAlphabet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlphabet_SelectedIndexChanged">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem Value="A">A</asp:ListItem>
                                            <asp:ListItem Value="B">B</asp:ListItem>
                                            <asp:ListItem Value="C">C</asp:ListItem>
                                            <asp:ListItem Value="D">D</asp:ListItem>
                                            <asp:ListItem Value="E">E</asp:ListItem>
                                            <asp:ListItem Value="F">F</asp:ListItem>
                                            <asp:ListItem Value="G">G</asp:ListItem>
                                            <asp:ListItem Value="H">H</asp:ListItem>
                                            <asp:ListItem Value="I">I</asp:ListItem>
                                            <asp:ListItem Value="J">J</asp:ListItem>
                                            <asp:ListItem Value="K">K</asp:ListItem>
                                            <asp:ListItem Value="L">L</asp:ListItem>
                                            <asp:ListItem Value="M">M</asp:ListItem>
                                            <asp:ListItem Value="N">N</asp:ListItem>
                                            <asp:ListItem Value="O">O</asp:ListItem>
                                            <asp:ListItem Value="P">P</asp:ListItem>
                                            <asp:ListItem Value="Q">Q</asp:ListItem>
                                            <asp:ListItem Value="R">R</asp:ListItem>
                                            <asp:ListItem Value="S">S</asp:ListItem>
                                            <asp:ListItem Value="T">T</asp:ListItem>
                                            <asp:ListItem Value="U">U</asp:ListItem>
                                            <asp:ListItem Value="V">V</asp:ListItem>
                                            <asp:ListItem Value="W">W</asp:ListItem>
                                            <asp:ListItem Value="X">X</asp:ListItem>
                                            <asp:ListItem Value="Y">Y</asp:ListItem>
                                            <asp:ListItem Value="Z">Z</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 720px;">
                                        <ul class="ToolStrip">
                                            <li>
                                                <asp:LinkButton ID="lbtnSearch" runat="server" OnClientClick="showSearchWin(); return false;">Search</asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="lbtnDetail" runat="server" OnClientClick="detailBtnClicked();return false;">Detail</asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="lbtnSync" runat="server" OnClick="btnSync_Click" OnClientClick="return syncBtnClicked();">Sync</asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="lbtnRemove" runat="server" OnClick="btnRemove_Click" OnClientClick="return removeBtnClicked();">Remove</asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnImportError" runat="server" Text="Handle Point Errors" OnClientClick="onImportErrorBtnClick(); return false;"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="lbtnDispose" runat="server" OnClick="btnDispose_Click" OnClientClick="DisposeClick();" >Action</asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnMove" OnClientClick="onMoveClick();return false;" runat="server"
                                                    Text="Move File"></asp:LinkButton><span>|</span></li>
                                            <li><a id="aClear" href="javascript:ClearSearch()">Clear</a></li>
                                            <li><span>|</span><a id="aMailChimp" href="#">Mail Chimp</a></li>
                                            <li><span>|</span><asp:LinkButton ID="aExport" OnClientClick="onExportClick();return false;" runat="server"
                                                    Text="Export"></asp:LinkButton></li>
                                        </ul>
                                        <!--div id="menu1" style="display: none;"-->
                                            <ul id="menu1" class="jqcontextmenu" style="display: none; width:160px ">
                                                <asp:Literal ID="ltrMenuItems" runat="server" ></asp:Literal>
                                            </ul>
                                            <ul id="menu2" class="jqcontextmenu" style="display: none;">
                                                <asp:Literal ID="ltrMenuResumeItems" runat="server"></asp:Literal>
                                            </ul>
                                        <!--/div-->
                                       
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
                        <asp:GridView ID="gridList" runat="server" DataKeyNames="FileId,Ranking,ProspectLoanStatus,BranchID,AlertIcon,AlertID,ImportErrorIcon,RuleAlertIcon"
                            EmptyDataText="There is no record in database." AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound"
                            OnPreRender="gridList_PreRender" CellPadding="3" CssClass="GrayGrid" GridLines="None"
                            OnSorting="gridList_Sorting" AllowSorting="true">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ckbAll" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <%--<input ID="ckbSelect" runat="server" type="checkbox" contactid='<%# Eval("ContactId") %>'  /> --%>
                                    <asp:CheckBox ID="ckbSelect" runat="server" contactid='<%# Eval("ContactId") %>' />
                                    <asp:TextBox ID="tbContactID" runat="server" Text='<%# Eval("ContactId") %>' Visible="false" ></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ranking" SortExpression="Ranking">
                                    <ItemTemplate>
                                        <asp:Literal ID="litRanking" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Borrower" SortExpression="Borrower">
                                    <ItemTemplate>
                                        <a id="aBorrower" href="javascript:GoToProspectLoanDetails('<%# Eval("FileID") %>')"
                                            myfileid="<%# Eval("FileID") %>" filename="<%# Eval("FileName") %>">
                                            <%# Eval("Borrower") %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="AlertIcon" HeaderText="Alerts">
                                    <ItemTemplate>
                                        <asp:Image ID="imgImptError" runat="server" Width="16px" Height="16px" Style="cursor: pointer;" /><asp:Image
                                            ID="imgAlerts" runat="server" Width="16px" Height="16px" Style="cursor: pointer;" /><asp:Image
                                                ID="imgRuleAlert" runat="server" Width="16px" Height="16px" Style="cursor: pointer;" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
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
                                <asp:BoundField DataField="Stage" SortExpression="Stage" HeaderText="Stage" />
                                <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right"
                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c0}" />
                                <asp:BoundField DataField="Rate" SortExpression="Rate" HeaderText="Rate" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Loan Officer" SortExpression="Loan Officer" HeaderText="Loan Officer" />
                                <asp:BoundField DataField="Lien" SortExpression="Lien" HeaderText="Lien" />
                                <asp:BoundField DataField="Branch" SortExpression="Branch" HeaderText="Branch" />
                                <asp:BoundField DataField="Program" SortExpression="Program" HeaderText="Loan Program" />
                                <asp:BoundField DataField="LeadSource" SortExpression="LeadSource" HeaderText="Lead Source" />
                                <asp:BoundField DataField="RefCode" SortExpression="RefCode" HeaderText="Ref Code" />
                                <asp:BoundField DataField="EstClose" SortExpression="EstClose" HeaderText="Est Close" />
                                <asp:BoundField DataField="Filename" SortExpression="Filename" HeaderText="Point File" />
                                <asp:BoundField DataField="LastCompletedStage" SortExpression="LastCompletedStage" HeaderText="Last Compl Stage" />
                                <asp:BoundField DataField="LastStageComplDate" SortExpression="LastStageComplDate" HeaderText="Last Stage Compl" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Partner" SortExpression="Partner" HeaderText="Partner" />
                                <asp:BoundField DataField="Referral" SortExpression="Referral" HeaderText="Referral" />
                                
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">
                            &nbsp;</div>
                        <asp:HiddenField ID="hiAllIds" runat="server" />
                        <asp:HiddenField ID="hiCheckedIds" runat="server" />
                        <asp:HiddenField ID="hiCheckedContactIds" runat="server" />
                        <asp:HiddenField ID="hiLoanInfo" runat="server" />
                        <asp:HiddenField ID="hiSelectedDisposal" runat="server" />
                        <asp:HiddenField ID="hiSelectedFolderId" runat="server" />
                    </div>
                    <div style="display: none;">
                        <div id="dialogSearchLoans">
                            <iframe id="iframeSearchLoans" name="iframeSearchLoans" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="dialogDetail">
                            <iframe id="iframeDetail" name="iframeDetail" frameborder="0" width="100%" height="100%">
                            </iframe>
                        </div>
                        <div id="dialogPointFolder">
                            <iframe id="iframePointFolder" name="iframePointFolder" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="dialogWkTplt">
                            <iframe id="iframeWfTplt" name="iframeWfTplt" frameborder="0" width="100%" height="100%">
                            </iframe>
                        </div>
                        <div id="dialogTaskAlert">
                            <iframe id="iframeTaskAlert" name="iframeTaskAlert" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="dialogPointImportErrorList">
                            <iframe id="iframeImportErrorList" name="iframeImportErrorList" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="dialogImportError" title="Point Import Alert Detail">
                            <iframe id="iframeImportError" name="iframeImportError" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="dialogRuleAlert" title="Rule Alert">
                            <iframe id="iframeRuleAlert" name="iframeRuleAlert" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>
                        <div id="dialog1" title="Point Folder Selection">
                            <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
                            </iframe>
                        </div>

                        <div id="ExportDiv">
                            <iframe id="iframeExport" name="iframeExport" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>

                        

                        <asp:HiddenField ID="hiSearchFilter" runat="server" />
                        <asp:HiddenField ID="hiGenWfTpltFileId" runat="server" />
                        <asp:HiddenField ID="hiWfTpltId" runat="server" />
                        <asp:LinkButton ID="lbtnGenWorkflow" runat="server" OnClick="lbtnGenWorkflow_Click"></asp:LinkButton>

                        <div id="HideAction" title="Hide Action" style=" display:none;">
                            <iframe id="iFrameAction" src="" ></iframe>
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