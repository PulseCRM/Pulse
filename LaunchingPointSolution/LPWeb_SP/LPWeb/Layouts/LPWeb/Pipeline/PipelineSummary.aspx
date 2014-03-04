<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Pipeline Summary View" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master"
    AutoEventWireup="true" Inherits="Pipeline_PipelineSummary" CodeBehind="PipelineSummary.aspx.cs" %>

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
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>    
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
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
            height:16px;
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

        // rule alert popup, required refresh page?
        var IsRefreshPage = false;

        $(document).ready(function () {
            //var checkItems=
            var checkAll = $(gridId + " :checkbox:eq(0)");
            checkAll.click(function () {
                var allStatus = checkAll.attr("checked");
                $(gridId + " :checkbox:gt(0)").each(function () {
                    $(this).attr("checked", allStatus);
                });
                getSelectedItems();
                var startDate = $("#" + '<%=EstStartDate.ClientID %>');
                var endDate = $("#" + '<%=EstEndDate.ClientID %>');
                startDate.datepick();
                endDate.datepick();
                startDate.attr("readonly", "true");
                endDate.attr("readonly", "true");
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
                        close: function (event, ui) { if (IsRefreshPage == true) { window.location.href = window.location.href; } }
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
                    suspend: suspendLoan,
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
                var f = document.getElementById('iframePF');
                f.src = "PointFolderSelection.aspx?fid=" + $("#" + '<%=hiSelectedLoan.ClientID %>').val() + "&tls=" + tLS + "&bid=" + bId + "&t=" + Math.random().toString();
                $('#dialog1').dialog('open');
                return false;
            }
        // End: add by peter, for Dispose button 2010-11-15        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle">
        Pipeline Summary View</div>
    <div class="SplitLine">
    </div>
    <iframe id="RateLockDetail" frameborder="0" style="display: none; border: none; padding: 0;
        margin: 0;"></iframe>
    <iframe id="taskAlertDetail" frameborder="0" style="display: none; border: none;
        padding: 0; margin: 0;"></iframe>
    <iframe id="importErrorDetail" frameborder="0" style="display: none; border: none;
        padding: 0; margin: 0;"></iframe>
    
    <div id="divRuleAlert" title="Rule Alert" style="display: none;">
        <iframe id="ifrRuleAlert" frameborder="0" scrolling="no" width="510px" height="610px"></iframe>
    </div>

    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlRegion" runat="server" DataTextField="Name" DataValueField="RegionId"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                            <asp:ListItem Text="All Regions" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlDivison" runat="server" DataTextField="Name" DataValueField="DivisionId"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlDivison_SelectedIndexChanged">
                            <asp:ListItem Text="All Divisions" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="Name" DataValueField="BranchId">
                            <asp:ListItem Text="All Branches" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlStage" runat="server" DataTextField="Name" DataValueField="StageId">
                            <asp:ListItem Text="All Stages" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                                
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="EstStartDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="EstEndDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 990px;">
                <tr>
                    <td style="width: 40px;">
                        <asp:DropDownList ID="ddlAlphabets" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlphabets_SelectedIndexChanged">
                            <asp:ListItem Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 450px;">
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
                                <asp:LinkButton ID="btnDispose" runat="server" Text="Dispose" OnClick="btnDispose_Click" OnClientClick="return false;"></asp:LinkButton><span>|</span></li>
                            <li>
                                <asp:LinkButton ID="lbtnSetup" runat="server" Text="Setup" OnClientClick="SetupSelectedItem();return false;"></asp:LinkButton></li>
                        </ul>
                        <div id="menu1" class="contextMenu" style="display: none;">
                            <ul>
                                <li id="suspend">Suspend</li>
                                <li id="deny">Deny</li>
                                <li id="cancel">Cancel</li>
                                <li id="resume">Resume</li>
                            </ul>
                        </div>
                    </td>
                    <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="true"
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
                        <input type="checkbox" onclick="onCkAllRowSelected(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("FileId") %>' onclick="onCkRowClicked(this, '<%# Eval("FileId") %>', '<%# Eval("Status") %>', '<%# Eval("BranchID") %>')" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Borrower" SortExpression="Borrower" ItemStyle-Wrap="false"
                    ItemStyle-Width="150">
                    <ItemTemplate>
                        <a href="#" class="loanDetails" tag='<%# Eval("FileId") %>'>
                            <%# Eval("Borrower")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stage" SortExpression="Stage" ItemStyle-Wrap="false"
                    ItemStyle-Width="80">
                    <ItemTemplate>
                        <%# Eval("Stage")%>
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
                <asp:TemplateField HeaderText="Est Close" SortExpression="EstClose" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-Width="60px">
                    <ItemTemplate>
                        <%# Eval("EstClose", "{0:d}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<div style='text-align: center;'>Alerts<div>" SortExpression="RateLockIcon" HeaderStyle-Width="80px" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <span class="alert">
                            <img id="imgRateLockIcon" class="RateLockIcon" tag='<%# Eval("FileId") %>' src='../images/loan/<%# Eval("RateLockIcon")%>'
                                width="16" height="16" />
                            <img id="imgImportErrorIcon" class="ImportErrorIcon" tag='<%# Eval("FileId") %>'
                                src='../images/loan/<%# Eval("ImportErrorIcon")%>' width="16" height="16" />
                            <img id="imgAlertIcon" class="AlertIcon" tag='<%# Eval("FileId") %>' src='../images/loan/<%# Eval("AlertIcon")%>'
                                width="16" height="16" />
                            <img id="imgRuleAlertIcon" class="RuleAlertIcon" tag='<%# Eval("FileId") %>' myAlertID='<%# Eval("AlertID") %>' src='../images/alert/<%# Eval("RuleAlertIcon")%>'
                                width="16" height="16" />
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loan Officer" SortExpression="Loan Officer" ItemStyle-Wrap="false" ItemStyle-Width="140px">
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
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">&nbsp;</div>
    </div>
    <asp:HiddenField ID="hfDeleteItems" runat="server" />
    <asp:HiddenField ID="hiSelectedLoan" runat="server" />
    <asp:HiddenField ID="hiSelectedLoanStatus" runat="server" />
    <asp:HiddenField ID="hiSelectedFolderId" runat="server" />
    <asp:HiddenField ID="hiSelectedDisposal" runat="server" />
    <div style="display: none;">
        <div id="dialogPointImportErrorList">
            <iframe id="iframeImportErrorList" name="iframeImportErrorList" frameborder="0" width="100%"
                height="100%"></iframe>
        </div>
        <div id="dialog1" title="Point Folder Selection">
            <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
    </div>
</asp:Content>
