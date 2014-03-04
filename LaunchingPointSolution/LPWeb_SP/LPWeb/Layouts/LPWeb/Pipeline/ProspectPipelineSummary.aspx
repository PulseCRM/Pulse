<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Prospect Pipeline Summary" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master"
    AutoEventWireup="true" CodeBehind="ProspectPipelineSummary.aspx.cs" Inherits="Pipeline_ProspectPipelineSummary" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script src="../js/jqueryui/jquery.ui.dialog.minV1.8.17.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.progressbar.min.js"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    
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
        function SelectedLoan(sID, sStatus, sBId, sBranchMgrID) {
            this.ID = sID;
            this.Status = sStatus;
            this.BranchID = sBId;

            $("#<%=hfsBranchMgrID.ClientID %>").val(sBranchMgrID);
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

        var gridId = "#<%=gvPropectView.ClientID %>";
        var sHasView = "<%=sHasViewRight %>";
        var sHasCreate = "<%=sHasCreateRight %>";

        // when rule alert popup close, refresh page or not (neo)
        var IsRefreshPage = false;

        $(document).ready(function () {
            DrawTab();
            var startDate = $("#" + '<%=txbStartDate.ClientID %>');
            var endDate = $("#" + '<%=txbEndDate.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");

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
            if (sHasView == "0") {
                //                $("#btnDetails").removeAttr("href");
                //                $("#btnDetails").removeAttr("onclick");
                //                $("#btnDetails").attr("title", "Can't show the prospect details.");
                DisableLink("btnDetails");
            }

            if (sHasCreate == "0") {
                DisableLink("aCreate");
            }

            $("#<%= btnRemove.ClientID %>").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }
                else {
                    return confirm("Removing the prospect will delete the prospect records, task history, and the activities. This operation is not reversible. Are you sure you want to continue?");
                }
            });



            $(".alert img[src$='Unknown.png'],img[src$='TaskGreen.png'],img[src$='TaskGray.png']").each(function () {
                $(this).hide();
            });


            $(".alert img.AlertIcon").each(function () {
                $(this).css("cursor", "pointer");
                $(this).click(function () {
                    var dialog = $("#taskAlertDetail");
                    dialog.attr("src", "ProspectTaskAlertDetail.aspx?contactID=" + $(this).attr("tag"));
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



            // init handle import errors list window
            initImportErrorListWin();
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
                //                    active: activeLoan,
                //                    bad: badLoan,
                //                    lost: lostLoan,
                //                    suspended: suspendedLoan


            },
            onContextMenu: function (e) {
                if (allSelectedLoan.length > 1) {
                    alert("Only one record can be selected for this operation.");
                    return false;
                }
                else if (allSelectedLoan.length == 1) {
                    var s = allSelectedLoan[0].Status.toLowerCase();
                    if (s == "active")
                        return true;
                    else {
                        alert("Unable to process the request for the selected Prospect. The Prospect has Disposed.");
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
                //                    if (s == "active") {
                //                        $("#active", menu).remove();
                //                    }
                //                    else if (s == "suspended" || s == "bad" || s == "lost") {
                //                        $("#suspended, #lost, #bad", menu).remove();
                //                    }
                //                    else {
                //                        $("*", menu).remove();
                //                        alert("Unable to process the request for the selected Prospect. The prospect status is unknown.");
                //                        return menu;
                //                    }
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
        function onImportErrorBtnClick() {
            var f = document.getElementById('iframeImportErrorList');
            var q = $.QueryString["q"];
            var loanOfficer=  $("#" + '<%=hfloanOfficer.ClientID %>').val();
            f.src = "HandlePointErrors.aspx?q=" + q + "&t=" + Math.random().toString();
            $('#dialogPointImportErrorList').dialog('open');
        }

        function onCreateClick() {
            var f = document.getElementById('iframeCT');
            var q = $.QueryString["q"];
            var loanOfficer=$("#" + '<%=hfloanOfficer.ClientID %>').val();
            var refCode=$("#" + '<%=hfrefCode.ClientID %>').val();
            var status=$("#" + '<%=hfstatus.ClientID %>').val();
            var leadSource=$("#" + '<%=hfleadSource.ClientID %>').val();
            var lastName=$("#" + '<%=hflastName.ClientID %>').val();
            var address= $("#" + '<%=hfaddress.ClientID %>').val();
            var city=$("#" + '<%=hfcity.ClientID %>').val();
            var state=$("#" + '<%=hfstate.ClientID %>').val();
            var zip=$("#" + '<%=hfzip.ClientID %>').val();
            f.src = "../Prospect/ProspectDetailPopup.aspx?loanOfficer=" + loanOfficer + "&refCode=" + refCode + "&status=" + status + "&leadSource=" + leadSource + "&lastName=" + lastName + "&address=" + address + "&city=" + city + "&state=" + state + "&zip=" + zip + "&t=" + Math.random().toString();
            $('#dialogCT').dialog({
                height: 620,
                width: 820,
                modal: true,
                resizable: false
            });

        }


         function closeCreatePop() {
            $('#dialogCT').dialog('close');
            RefreshList();
        }

        function onSearchClick() {
            var f = document.getElementById('iframeSP');
            var q = $.QueryString["q"];
            var loanOfficer=$("#" + '<%=hfloanOfficer.ClientID %>').val();
            var refCode=$("#" + '<%=hfrefCode.ClientID %>').val();
            var status=$("#" + '<%=hfstatus.ClientID %>').val();
            var leadSource=$("#" + '<%=hfleadSource.ClientID %>').val();
            var lastName=$("#" + '<%=hflastName.ClientID %>').val();
            var address= $("#" + '<%=hfaddress.ClientID %>').val();
            var city=$("#" + '<%=hfcity.ClientID %>').val();
            var state=$("#" + '<%=hfstate.ClientID %>').val();
            var zip=$("#" + '<%=hfzip.ClientID %>').val();
            f.src = "SearchProspects.aspx?loanOfficer=" + loanOfficer + "&refCode=" + refCode + "&status=" + status + "&leadSource=" + leadSource + "&lastName=" + lastName + "&address=" + address + "&city=" + city + "&state=" + state + "&zip=" + zip + "&t=" + Math.random().toString();
            $('#dialogSP').dialog({
                height: 260,
                width: 740,
                modal: true,
                resizable: false
            });

        }
        
        //#region create prospect

        function aCreate_onclick(){
        
            window.location.href = "<%= this.ResolveClientUrl("~/_layouts/LPWeb/Prospect/LeadCreate.aspx") %>";
        }

        //#endregion

         function closeSearchPop() {
            $('#dialogSP').dialog('close');
            RefreshList();
        }

         function onImportClick() {
            var f = document.getElementById('iframeIL');
            var q = $.QueryString["q"];
            f.src = "ImportLeads.aspx?t=" + Math.random().toString();
            $('#dialogIL').dialog({
                height: 320,
                width: 740,
                modal: true,
                resizable: false
            });

        }

         function closeImportPop() {
            $('#dialogIL').dialog('close');
            RefreshList();
        }

        function GetSearchHiddenValue(loanOfficer, refCode, status, leadSource, lastName, address, city, state, zip)
        {
            $("#" + '<%=hfloanOfficer.ClientID %>').val(loanOfficer);
            $("#" + '<%=hfrefCode.ClientID %>').val(refCode);
            $("#" + '<%=hfstatus.ClientID %>').val(status);
            $("#" + '<%=hfleadSource.ClientID %>').val(leadSource);
            $("#" + '<%=hflastName.ClientID %>').val(lastName);
            $("#" + '<%=hfaddress.ClientID %>').val(address);
            $("#" + '<%=hfcity.ClientID %>').val(city);
            $("#" + '<%=hfstate.ClientID %>').val(state);
            $("#" + '<%=hfzip.ClientID %>').val(zip);

        }

          function onAssignClick() {
            var UserID=allSelectedLoan[0].BranchID;
            var SelBranchMgrID= $("#<%=hfsBranchMgrID.ClientID %>").val();
            if (allSelectedLoan.length > 1) {
                        alert("Only one record can be selected for this operation.");
            }
            else  if(UserID=="" && SelBranchMgrID=="0")
            {
                alert("There is no Branch Manager or Loan Officer user available.");
            }
            else if (allSelectedLoan.length == 1) {
                 var prospectID =allSelectedLoan[0].ID;
                 var SelUserID=UserID;
                 if(UserID=="")
                 {
                    SelUserID=SelBranchMgrID;
                 }
                var f = document.getElementById('iframeAP');
                f.src = "AssignProspect.aspx?prospectID=" + prospectID + "&UserID="+ SelUserID +"&t=" + Math.random().toString();
                $('#dialogAP').dialog({
                    height: 320,
                    width: 540,
                    modal: true,
                    resizable: false
                });
            }
            else {
                alert("No record has been selected.");
            }
        }

        function DialogAssignClose() {
            $("#dialogAP").dialog('destroy');
            AssignRefreshList();
        }

        function closeImportErrorWin(bRefresh) {
            $('#dialogPointImportErrorList').dialog('close');
            if (bRefresh === true)
                RefreshList();
        }
        function RefreshList() {
            <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>
        }
        function AssignRefreshList() {
            <%=this.ClientScript.GetPostBackEventReference(this.btnRefresh, null) %>
        }
        function clearImportErrorListWin()
        {
            var f = document.getElementById('iframeImportErrorList');
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
            //$('input:checkbox', $('#' + '<%=gvPropectView.ClientID %>')).each(function() { $(this).attr('checked', bCheck); $(this).click(); });
            $("#<%=gvPropectView.ClientID %> .CheckBoxColumn input").each(function() { $(this).attr('checked', bCheck); $(this).click();	});
        }
        function onCkRowClicked(me, sID, sStatus, sBranchId,BranchMgrID)
        {

            if ($(me).attr("checked"))
                allSelectedLoan.push(new SelectedLoan(sID, sStatus, sBranchId,BranchMgrID));
            else
            {
                allSelectedLoan.removeLoan(sID);
                $("#<%=hfsBranchMgrID.ClientID %>").val();
            }
        }

        function activeLoan()
        {

            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Active');
            tLS= 5;
            //ShowFolerSelWin();
            <%=this.ClientScript.GetPostBackEventReference(this.btnDispose, null) %>
        }

        function badLoan()
        {
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Bad');
            tLS= 4;
             //ShowFolerSelWin();
            <%=this.ClientScript.GetPostBackEventReference(this.btnDispose, null) %>
        }

        function lostLoan()
        {            
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Lost');
            tLS= 2;
            //ShowFolerSelWin();
            <%=this.ClientScript.GetPostBackEventReference(this.btnDispose, null) %>
        }

        function suspendedLoan()
        {
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val('Suspended');
            tLS= 1;
             //ShowFolerSelWin();
            <%=this.ClientScript.GetPostBackEventReference(this.btnDispose, null) %>
        }

        function getFolderSelectionReturn(sReturn)
        {
            $("#"+'<%=hiSelectedFolderId.ClientID %>').val(sReturn);
            $('#dialog1').dialog('close');
            <%=this.ClientScript.GetPostBackEventReference(this.btnDispose, null) %>
            clearFolderSelWin('dialog1');
        }

        function DisposeLoan(status)
        {
            tLS=8; 
            $("#"+'<%=hiSelectedDisposal.ClientID %>').val(status);
            ShowFolerSelWin();
        }

        function ShowFolerSelWin() {
                var f = document.getElementById('iframePF');
                f.src = "PointFolderSelection.aspx?fid=" + $("#" + '<%=hiSelectedLoan.ClientID %>').val() + "&tls=" + tLS + "&bid=" + bId + "&type=dispose&t=" + Math.random().toString();
                $('#dialog1').dialog('open');
                return false;
            }
        // End: add by peter, for Dispose button 2010-11-15        
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $(".MyProgressBar").each(function () {

                var Percent = $(this).attr("myPercent");
                var PercentNum = new Number(Percent);

                $(this).progressbar({
                    value: PercentNum
                });
            });



        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=btnMerge.ClientID %>").click(function () {
                var selctedItems = new Array();
                $(gridId + " :checkbox:gt(0)").each(function () {
                    var item = $(this);
                    if ($(this).attr("checked") == true) {
                        selctedItems.push(item.attr("tag"));
                    }
                });

                if (selctedItems.length <= 0) {
                    alert("No record has been selected.");
                    return false;
                }

                var f = document.getElementById('iframeMerge');
                f.src = "MergeProspectsPopup.aspx?prospects=" + selctedItems.join(",") + "&t=" + Math.random().toString();
                $('#dialogMerge').dialog({
                    modal: true,
                    title: "Merge Prospects",
                    width: 620,
                    height: 320,
                    resizable: false,
                    close: function (event, ui) { $(this).dialog('destroy') },
                    open: function (event, ui) { $(this).css("width", "100%") }
                });

                $(".ui-dialog").css("border", "solid 3px #aaaaaa");
                return false;
            });
        });

        function CloseDialog_MergeProspects() {
            $('#dialogMerge').dialog('close');
            this.location.href = this.location.href;
        }

        //#region Detail

        function UpdateProspect() {

            var SelectedCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gvPropectView tr:not(:first) td :checkbox:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No prospect was selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one prospect can be selected for this operation.");
                return;
            }

            var ContactID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gvPropectView tr:not(:first) td :checkbox:checked").attr("tag");

            GoToProspectDetail(ContactID);
        }

        function GoToProspectDetail(ProspectID) {

            if (sHasView != "1") {

                alert("You have no privilege to do this operation.");
                return;
            }

            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                selctedItems.push(item.attr("tag"));
            });

            var c = "ContactID=" + ProspectID + "&ContactIDs=" + selctedItems.join(",");
            var e = $.base64Encode(c);

            window.location.href = "../Prospect/ProspectDetailView.aspx?PageFrom=" + encodeURIComponent('<%= FromURL %>') + "&e=" + e;
        }

        //#endregion

        function ClearSearch() {
            //            $("#" + '<%=hfloanOfficer.ClientID %>').val("");
            //            $("#" + '<%=hfrefCode.ClientID %>').val("");
            //            $("#" + '<%=hfstatus.ClientID %>').val("");
            //            $("#" + '<%=hfleadSource.ClientID %>').val("");
            //            $("#" + '<%=hflastName.ClientID %>').val("");
            //            $("#" + '<%=hfaddress.ClientID %>').val("");
            //            $("#" + '<%=hfcity.ClientID %>').val("");
            //            $("#" + '<%=hfstate.ClientID %>').val("");
            //            $("#" + '<%=hfzip.ClientID %>').val("");

            //            $("#" + '<%=ddlOrganizationTypes.ClientID %>').val("0");
            //            $("#" + '<%=ddlOrganization.ClientID %>').val("-1");
            //            $("#" + '<%=ddlStatus.ClientID %>').val("0");
            //            $("#" + '<%=ddlLeadSourceType.ClientID %>').val("-1");
            //            $("#" + '<%=ddlLeadSource.ClientID %>').val("-1");
            //            $("#" + '<%=txbStartDate.ClientID %>').val("");
            //            $("#" + '<%=txbEndDate.ClientID %>').val("");
            //            RefreshList();

            window.location.href = window.location.pathname;
        }   
    </script>
    <style>
        .TabContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabContent input[type="text"], select, input[type="file"]
        {
            margin-left: 15px;
        }
        
        .TabSubContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabSubContent input[type="text"], select, input[type="file"]
        {
            margin-left: 15px;
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
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript">

        jQuery(document).ready(function ($) {

            $('#aMailChimp').addcontextmenu('divMailChimpMenu') //apply context menu to all images on the page 
        })

        function Subscribe() {

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
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

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + $("#<%=hfDeleteItems.ClientID %>").val() + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

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

            var ContactIDs = $("#<%=hfDeleteItems.ClientID %>").val();

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                alert("No record has been selected.");
                return false;
            }

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

            if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
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

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + $("#<%=hfDeleteItems.ClientID %>").val() + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }


        function UnsubscribeMailChimp(LID) {


            var ContactIDs = $("#<%=hfDeleteItems.ClientID %>").val();


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
                position: [$("#btSaveView").offset().top + 200, $("#btSaveView").offset().left - 180]
            });
            $('#SaveViewShow').dialog('open');

        }

        function onExportClick() {

            if (allSelectedLoan.length == 0) {
                alert("Please select the Clients to be exported.");
                return false;
            }

            var IDs = "";

            $(allSelectedLoan).each(function (n, obj) {

                IDs += obj.ID + ",";
            });

            var queryCondition = $("#<%=hidFilterQueryCondition.ClientID %>").val();

            var recordTotal = $("#<%=hidrecordTotal.ClientID %>").val();
            var IsAll = $("#CkAll:checked").size();

            var f = document.getElementById('iframeExport');
            f.src = "ExportPipeline.aspx?IDs=" + IDs + "&action=Clients&recordTotal=" + recordTotal + "&IsAll=" + IsAll + "&queryCondition=" + queryCondition + "&t=" + Math.random().toString();

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
           var lname = $.trim($("#" + "<%=txtClientLastName.ClientID %>").val());
            
            $("#" + "<%=hflastName.ClientID %>").val(lname);
            <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>;

            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
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
                                <li><a href="ProspectPipelineSummaryLoan.aspx"><span>Leads</span></a></li>
                                <li><a href="ProcessingPipelineSummary.aspx"><span>Loans</span></a></li>
                                <li id="current"><a href="ProspectPipelineSummary.aspx"><span>Clients</span></a></li>
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
                        Client Pipeline Summary</div>
                    <div class="SplitLine" style="margin-top: 5px;">
                    </div>
                    <div>
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
                            <div id="divFilter" style="margin-top: 10px;">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td colspan="1">
                                            <asp:DropDownList ID="ddlProspectStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProspectStatus_SelectedIndexChanged" Width="130px">
                                                <asp:ListItem Text="Active Clients" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="All Clients" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Inactive Clients" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        		<asp:DropDownList ID="ddlUserPipelineView" runat="server" AutoPostBack="true" DataTextField="ViewName" DataValueField="UserPipelineViewID" OnSelectedIndexChanged="ddlUserPipelineView_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="padding-left:15px;" colspan="6">
												Client Last Name: <asp:TextBox ID="txtClientLastName" runat="server" style="margin:5px;"></asp:TextBox>
												<asp:Button ID="btnSearchLastName"  runat="server" Text="Search" class="Btn-66" OnClientClick="return SearchLastName()" />
											
                                            <asp:HiddenField ID="UserPipilineViewIsDefault" Value ="0" runat="server" />
												&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <input id="btSaveView" type="button" value=" Save View " onclick="SaveViewDialog();" class="Btn-91" /> 
                                            <input id="btnManageView" type="button" value=" Manage View " class="Btn-91" onclick="onManagePipelineviews();return false;" />

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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlOrganizationTypes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOrganizationTypes_SelectedIndexChanged" Width="130px">
                                                <asp:ListItem Text="All organization types" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Region" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Division" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Branch" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Loan Officer" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlOrganization" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="5">
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100px">
                                                <asp:ListItem Text="All Statuses" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                                <asp:ListItem Text="Bad" Value="Bad"></asp:ListItem>
                                                <asp:ListItem Text="Lost" Value="Lost"></asp:ListItem>
                                                <asp:ListItem Text="Suspended" Value="Suspended"></asp:ListItem>
                                            </asp:DropDownList>
                                        
                                            <asp:DropDownList ID="ddlLeadSourceType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeadSourceType_SelectedIndexChanged" Width="100px">
                                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="Lead Sources" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Partners" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Referrals" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                        
                                            <asp:DropDownList ID="ddlLeadSource" runat="server" Width="80px">
                                                <asp:ListItem Text="All Lead Sources" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        
                                            <asp:TextBox ID="txbStartDate" runat="server" CssClass="DateField"></asp:TextBox>
                                        
                                            <asp:TextBox ID="txbEndDate" runat="server" CssClass="DateField"></asp:TextBox>
                                        </td>
                                        <td style="padding-left:5px;">
                                            <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                                            </asp:Button><asp:Button ID="btnRefresh" runat="server" Text="" style="display:none" OnClick="btnRefresh_Click">
                                            </asp:Button>

                                            <asp:HiddenField ID="hidFilterQueryCondition" runat="server" />
                                            <asp:HiddenField ID="hidrecordTotal" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divToolBar" style="margin-top: 13px;">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 1000px;">
                                    <tr>
                                        <td style="width: 40px;">
                                            <asp:DropDownList ID="ddlAlphabets" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlphabets_SelectedIndexChanged">
                                                <asp:ListItem Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 720px;">
                                            <ul class="ToolStrip">
                                                <li><a id="aCreate" href="javascript:aCreate_onclick()">Create</a><span>|</span></li>
                                                <li>
                                                    <asp:HyperLink ID="btnSearch" NavigateUrl="javascript:onSearchClick();" runat="server">Search</asp:HyperLink><span>|</span></li>
                                                <li><a id="btnDetails" href="javascript:UpdateProspect()">Details</a><span>|</span></li>
                                                <li>
                                                    <asp:HyperLink ID="btnAssign" NavigateUrl="javascript:onAssignClick();" runat="server">Assign</asp:HyperLink>
                                                    <span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="btnDispose" runat="server" Text="Dispose" OnClick="btnDispose_Click"></asp:LinkButton><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="btnMerge" runat="server" Text="Merge"></asp:LinkButton><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click"></asp:LinkButton><span>|</span></li>
                                                <li>
                                                    <asp:HyperLink ID="btnImportLeads" NavigateUrl="javascript:onImportClick();" runat="server">Import Leads</asp:HyperLink><span>|</span></li>
                                                <li><a id="aClear" href="javascript:ClearSearch()">Clear</a></li>
                                                <li><span>|</span><a id="aMailChimp" href="#">Mail Chimp</a></li>
                                                <li><span>|</span><asp:LinkButton ID="aExport" OnClientClick="onExportClick();return false;" runat="server"
                                                    Text="Export"></asp:LinkButton></li>
                                            </ul>
                                            <div id="menu1" class="contextMenu" style="display: none;">
                                                <asp:Literal ID="ltrMenuItems" runat="server"></asp:Literal>
                                            </div>
                                        </td>
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
                        <div id="divDivision" class="ColorGrid" style="width: 1000px; margin-top: 5px;">
                            <asp:GridView ID="gvPropectView" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                                Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                                OnSorting="gvPropectView_Sorting" CellPadding="3" GridLines="None" DataKeyNames="Contactid,Status,UserId"
                                OnRowDataBound="gvPropectView_RowDataBound" OnPreRender="gvPropectView_PreRender">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="CkAll" type="checkbox" onclick="onCkAllRowSelected(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="checkbox" tag='<%# Eval("ContactId") %>' onclick="onCkRowClicked(this, '<%# Eval("ContactId") %>', '<%# Eval("Status1") %>', '<%# Eval("UserID") %>', '<%# Eval("BranchMgrID") %>')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date Created" SortExpression="Created" ItemStyle-Wrap="false"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%# Eval("Created", "{0:d}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Client" SortExpression="Client" ItemStyle-Wrap="false"
                                        ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <a href="javascript:GoToProspectDetail('<%# Eval("ContactId") %>')">
                                                <%# Eval("Client")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<div style='text-align: center;'>Alerts<div>" SortExpression="AlertIcon"
                                        HeaderStyle-Width="80px" ItemStyle-Wrap="false" ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <span class="alert">
                                                <img id="imgRateLockIcon" class="AlertIcon" tag='<%# Eval("ContactId") %>' src='../images/loan/<%# Eval("AlertIcon")%>'
                                                    width="16" height="16" />
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Progress" SortExpression="Progress" HeaderStyle-Width="110"
                                        ItemStyle-Width="110">
                                        <ItemTemplate>
                                            <div class="ProgressContainer">
                                                <div class="completed" style='width: <%# Eval("Progress")%>%'>
                                                    &nbsp;</div>
                                                <div class="content">
                                                    <%# Eval("Progress")%>%</div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="Status1" ItemStyle-Wrap="false"
                                        ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <%# Eval("Status1")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Lead Source" SortExpression="LeadSource" ItemStyle-Wrap="false"
                                        ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <%# Eval("LeadSource")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ref Code" SortExpression="ReferenceCode" ItemStyle-Wrap="false"
                                        ItemStyle-Width="80">
                                        <ItemTemplate>
                                            <%# Eval("ReferenceCode")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loan Officer" SortExpression="LoanOfficer" ItemStyle-Wrap="false"
                                        ItemStyle-Width="120px">
                                        <ItemTemplate>
                                            <%# Eval("LoanOfficer")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Branch" SortExpression="Branch" ItemStyle-Wrap="false"
                                        ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <%# Eval("Branch")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Partner" SortExpression="Partner" ItemStyle-Wrap="false"
                                        ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <%# Eval("Partner")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Referral" SortExpression="Referral" ItemStyle-Wrap="false"
                                        ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <%# Eval("Referral")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        <asp:HiddenField ID="hfDeleteItems" runat="server" />
                        <asp:HiddenField ID="hiSelectedLoan" runat="server" />
                        <asp:HiddenField ID="hiSelectedLoanStatus" runat="server" />
                        <asp:HiddenField ID="hiSelectedFolderId" runat="server" />
                        <asp:HiddenField ID="hiSelectedDisposal" runat="server" />
                        <asp:HiddenField ID="hfloanOfficer" runat="server" />
                        <asp:HiddenField ID="hfrefCode" runat="server" />
                        <asp:HiddenField ID="hfstatus" runat="server" />
                        <asp:HiddenField ID="hfleadSource" runat="server" />
                        <asp:HiddenField ID="hflastName" runat="server" />
                        <asp:HiddenField ID="hfaddress" runat="server" />
                        <asp:HiddenField ID="hfcity" runat="server" />
                        <asp:HiddenField ID="hfstate" runat="server" />
                        <asp:HiddenField ID="hfzip" runat="server" />
                        <asp:HiddenField ID="hfsBranchMgrID" runat="server" />
                        <div style="display: none;">
                            <div id="dialogPointImportErrorList">
                                <iframe id="iframeImportErrorList" name="iframeImportErrorList" frameborder="0" width="100%"
                                    height="100%"></iframe>
                            </div>
                            <div id="dialog1" title="Point Folder Selection">
                                <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
                                </iframe>
                            </div>
                            <div id="dialogSP" title="Search Client">
                                <iframe id="iframeSP" name="iframeSP" frameborder="0" width="100%" height="100%">
                                </iframe>
                            </div>
                            <div id="dialogAP" title="Assign Prospect">
                                <iframe id="iframeAP" name="iframeAP" frameborder="0" width="100%" height="100%">
                                </iframe>
                            </div>
                            <div id="dialogIL" title="Import Leads">
                                <iframe id="iframeIL" name="iframeIL" frameborder="0" width="100%" height="100%">
                                </iframe>
                            </div>
                            <div id="dialogCT" title="New Client">
                                <iframe id="iframeCT" name="iframeCT" frameborder="0" width="100%" height="100%">
                                </iframe>
                            </div>
                            <div id="dialogMerge" title="Merge Prospects">
                                <iframe id="iframeMerge" name="iframeMerge" frameborder="0" width="100%" height="100%">
                                </iframe>
                            </div>

                            <div id="ExportDiv">
                            <iframe id="iframeExport" name="iframeExport" frameborder="0" width="100%"
                                height="100%"></iframe>
                        </div>

                        
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