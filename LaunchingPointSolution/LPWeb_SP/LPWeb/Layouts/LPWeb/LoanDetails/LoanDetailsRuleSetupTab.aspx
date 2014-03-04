<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailsRuleSetupTab.aspx.cs" Inherits="LoanDetails_LoanDetailsRuleSetupTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan Details - Rule Setup Tab</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        // show popup for add email template

        $(document).ready(function () {

            // loan status
            var LoanStatus = $("#hdnLoanStatus").val();
            var ProspectLoanStatus = $("#hdnProspectLoanStatus").val();
            var RuleNum = $("#hdnRuleNum").val();

            if (LoanStatus == "Processing" || (LoanStatus == "Prospect" && ProspectLoanStatus == "Active")) {
                if (RuleNum == "true") {

                    $("#aAdd").removeAttr("href");
                    $("#aAdd").removeAttr("onclick");
                    $("#aAdd").attr("title", "Each loan can have up to 20 rules or rule groups.");
                }
                else {
                    // left click context menu
                    $("#aAdd").contextMenu({ menu: 'divActionMenu', leftButton: true }, function (action, el, pos) { contextMenuWork(action, el, pos); });
                }
            }
            else if (LoanStatus != "Prospect") {

                $("#aAdd").removeAttr("href");
                $("#aAdd").removeAttr("onclick");
                $("#aAdd").attr("title", "Can't add rule to Non-Processing loan.");

                $("#aRemove").removeAttr("href");
                $("#aRemove").removeAttr("onclick");
                $("#aRemove").attr("title", "Can't remove rule from Non-Processing loan.");

                $("#aEnable").removeAttr("href");
                $("#aEnable").removeAttr("onclick");
                $("#aEnable").attr("title", "Can't enable rule in Non-Processing loan.");

                $("#aDisable").removeAttr("href");
                $("#aDisable").removeAttr("onclick");
                $("#aDisable").attr("title", "Can't disable rule in Non-Processing loan.");
            }
            else if (LoanStatus == "Prospect") {

                $("#aAdd").removeAttr("href");
                $("#aAdd").removeAttr("onclick");
                $("#aAdd").attr("title", "Can't add rule to Non-Active loan.");

                $("#aRemove").removeAttr("href");
                $("#aRemove").removeAttr("onclick");
                $("#aRemove").attr("title", "Can't remove rule from Non-Active loan.");

                $("#aEnable").removeAttr("href");
                $("#aEnable").removeAttr("onclick");
                $("#aEnable").attr("title", "Can't enable rule in Non-Active loan.");

                $("#aDisable").removeAttr("href");
                $("#aDisable").removeAttr("onclick");
                $("#aDisable").attr("title", "Can't disable rule in Non-Active loan.");
            }

        });

        // left click context menu
        function contextMenuWork(action, el, pos) {

            switch (action) {
                case "RuleGroup":
                    {
                        ShowDialog_AddRuleGroup();
                        break;
                    }
                case "Rule":
                    {
                        ShowDialog_AddRule();
                        break;
                    }
            }
        }

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridRuleList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridRuleList tr td :checkbox").attr("checked", "");
            }
        }

        //#region Add Rule

        function ShowDialog_AddRule() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrAdd").attr("src", "RuleSelectionPopup.aspx?sid=" + RadomStr + "&LoanID=" + LoanID);

            // show modal
            $("#divAdd").attr("title", "Select Rules");
            $("#divAdd").dialog({
                height: 450,
                width: 435,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_AddRule() {

            $("#divAdd").dialog("close");
        }

        //#endregion

        //#region Add Rule Group

        function ShowDialog_AddRuleGroup() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrAdd").attr("src", "RuleGroupSelectionPopup.aspx?sid=" + RadomStr + "&LoanID=" + LoanID);

            // show modal
            $("#divAdd").attr("title", "Select Rule Groups");
            $("#divAdd").dialog({
                height: 450,
                width: 435,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_AddRuleGroup() {

            $("#divAdd").dialog("close");
        }

        //#endregion

        //#region Remove

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {

            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(SuccessMsg);
            $('#aClose').show();
        }

        function aRemove_onclick() {

            if ($("#gridRuleList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No rule has been selected.");
                return;
            }

            var Result = confirm("The Alert Rule(s) have been referenced in the LoanAlerts. Removing the selected Rule(s) or Rule Group(s) will also remove the references in the LoanAlerts and the operation is not reversible. Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            $("#divContainer").height(600);

            // show waiting dialog
            ShowWaitingDialog("Removing selected rule(s)...");

            var LoanID = GetQueryString1("FileID");

            // selected rule ids
            var LoanRuleIDs = "";
            $("#gridRuleList tr:not(:first) td :checkbox:checked").each(function (i) {

                var LoanRuleID = $(this).attr("myLoanRuleID");
                if (LoanRuleIDs == "") {

                    LoanRuleIDs = LoanRuleID;
                }
                else {

                    LoanRuleIDs += "," + LoanRuleID;
                }
            });

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("LoanRuleRemove_Background.aspx?sid=" + Radom + "&LoanID=" + LoanID + "&LoanRuleIDs=" + encodeURIComponent(LoanRuleIDs), AfterRemove);
        }

        function AfterRemove(data) {

            if (data.ExecResult == "Failed") {
                $("#divContainer").unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Removed selected rule(s) successfully.')", 2000);
        }

        //#endregion

        //#region Enable

        function aEnable_onclick() {

            if ($("#gridRuleList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No rule has been selected.");
                return;
            }

            $("#divContainer").height(600);

            // show waiting dialog
            ShowWaitingDialog("Enabling selected rule(s)...");

            var LoanID = GetQueryString1("FileID");

            // selected rule ids
            var LoanRuleIDs = "";
            $("#gridRuleList tr:not(:first) td :checkbox:checked").each(function (i) {

                var LoanRuleID = $(this).attr("myLoanRuleID");
                if (LoanRuleIDs == "") {

                    LoanRuleIDs = LoanRuleID;
                }
                else {

                    LoanRuleIDs += "," + LoanRuleID;
                }
            });

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("LoanRuleEnable_Background.aspx?sid=" + Radom + "&Action=Enable&LoanID=" + LoanID + "&LoanRuleIDs=" + encodeURIComponent(LoanRuleIDs), AfterEnable);
        }

        function AfterEnable(data) {

            if (data.ExecResult == "Failed") {
                $("#divContainer").unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Enabled selected rule(s) successfully.')", 2000);
        }

        //#endregion

        //#region Disable

        function aDisable_onclick() {

            if ($("#gridRuleList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No rule has been selected.");
                return;
            }

            $("#divContainer").height(600);

            // show waiting dialog
            ShowWaitingDialog("Disabling selected rule(s)...");

            var LoanID = GetQueryString1("FileID");

            // selected rule ids
            var LoanRuleIDs = "";
            $("#gridRuleList tr:not(:first) td :checkbox:checked").each(function (i) {

                var LoanRuleID = $(this).attr("myLoanRuleID");
                if (LoanRuleIDs == "") {

                    LoanRuleIDs = LoanRuleID;
                }
                else {

                    LoanRuleIDs += "," + LoanRuleID;
                }
            });

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("LoanRuleEnable_Background.aspx?sid=" + Radom + "&Action=Disable&LoanID=" + LoanID + "&LoanRuleIDs=" + encodeURIComponent(LoanRuleIDs), AfterDisable);
        }

        function AfterDisable(data) {

            if (data.ExecResult == "Failed") {
                $("#divContainer").unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Disabled selected rule(s) successfully.')", 2000);
        }

        //#endregion

        function RefreshPage() {

//            if ($.browser.msie == true) {   // ie

//                window.parent.document.frames("ifrLoanInfo").location.reload();
//            }
//            else {   // firefox

//                window.parent.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
//            }
            
            // refresh current page
            window.location.href = window.location.href;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 1000px; height: 560px; border: solid 0px red; overflow: auto;">
        <div id="divToolBar">
            <ul class="ToolStrip" style="margin-left: 0px;">
                <li><a id="aAdd" href="" onclick="return false;">Add</a><span>|</span></li>
                <li><a id="aRemove" href="" onclick="aRemove_onclick(); return false;">Remove</a><span>|</span></li>
                <li><a id="aEnable" href="" onclick="aEnable_onclick();return false;">Enable</a><span>|</span></li>
                <li><a id="aDisable" href="" onclick="aDisable_onclick();return false;">Disable</a></li>
            </ul>
        </div>
        <div id="divRuleList" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridRuleList" runat="server" DataSourceID="RuleSqlDataSource" EmptyDataText="There is no rule applied to this loan." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkSelected" type="checkbox" myLoanRuleID="<%# Eval("LoanRuleId")%>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RuleType" HeaderText="Type" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="RuleName" HeaderText="Name" />
                    <asp:BoundField DataField="Enabled" HeaderText="Enabled" ItemStyle-Width="60px" />
                    <asp:BoundField DataField="Applied" HeaderText="Applied" ItemStyle-Width="140px" HtmlEncode="false" DataFormatString="{0:d}" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:SqlDataSource ID="RuleSqlDataSource" runat="server" DataSourceMode="DataReader">
        </asp:SqlDataSource>
        <div id="divAdd" title="Select Rules" style="display: none;">
            <iframe id="ifrAdd" frameborder="0" scrolling="no" width="410px" height="410px"></iframe>
        </div>
        <ul id="divActionMenu" class="contextMenu">
            <li><a href="#RuleGroup">Rule Group</a></li>
            <li><a href="#Rule">Rule</a></li>
        </ul>
        <div id="divWaiting" style="display: none; padding: 5px;">
		    <table style="margin-left: auto; margin-right: auto;">
			    <tr>
				    <td>
					    <img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
				    </td>
				    <td style="padding-left: 5px;">
					    <label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>&nbsp;&nbsp;
					    <a id="aClose" href="javascript:RefreshPage()" style="font-weight: bold; color: #6182c1;">[Close]</a>
				    </td>
			    </tr>
		    </table>
	    </div>
        <asp:HiddenField ID="hdnLoanStatus" runat="server" />
        <asp:HiddenField ID="hdnProspectLoanStatus" runat="server" />
        <asp:HiddenField ID="hdnRuleNum" runat="server" />
    </div>
    </form>
</body>
</html>