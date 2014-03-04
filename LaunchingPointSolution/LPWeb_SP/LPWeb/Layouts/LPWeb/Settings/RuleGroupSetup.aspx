<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleGroupSetup.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Settings.RuleGroupSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.tinysort.min.js" type="text/javascript"></script>
    <title>Rule Group Setup</title>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
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
            $('input:checkbox', $('#' + areaID) + '.CheckBoxColumn').each(function () { $(this).attr('checked', bCheck); });
        }

        function CheckBoxClicked(me, ckAllID, hiAllIDs, hiSelectedIDs, id) {
            var sAllIDs = $('#' + hiAllIDs).val();
            var sSelectedIDs = $('#' + hiSelectedIDs).val();
            var allIDs = new Array();
            var selectedIDs = new Array();
            if (sAllIDs.length > 0)
                allIDs = sAllIDs.split(',');

            if (sSelectedIDs.length > 0)
                selectedIDs = sSelectedIDs.split(',');

            if ($(me).attr('checked'))
                selectedIDs.push(id);
            else
                selectedIDs.remove(id);

            // set the CheckAll check box checked status
            $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);
            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');
        }
    </script>
    <script type="text/javascript">
        (function ($) {
            $.fn.outerHTML = function (s) {
                return (s)
			? this.before(s).remove()
			: $('<p>').append(this.eq(0).clone()).html();
            }
        })(jQuery);

        $(document).ready(function () {

            renderDataGrid(decodeDataXml($("#" + "<%=hiCurrentData.ClientID %>").val()), "gridRuleList");

            initCfmBox('dialog1', 'Rule Selection', '', 500, 450, function () {
                if (window.frames.iframeRS && window.frames.iframeRS.returnFn) {
                    window.frames.iframeRS.returnFn();
                }
            });
            initRuleSetupWin();
            
        });

        function initCfmBox(id, title, msg, w, h, callBack) {
            $('#' + id).dialog({
                modal: false,
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
                        clearSelectWin(id);
                    }
                },
                open: function () {
                    $('.ui-dialog-buttonset').css('float', 'none');
                    $('.ui-dialog-buttonset').css('text-align', 'center');
                    $('.ui-dialog-buttonpane').find('button').addClass('Btn-66');
                }
            });
        }

        function clearSelectWin(id) {
            var f;
            if ("dialog1" == id)
                f = document.getElementById('iframeRS');
            f.src = "about:blank";
        }

        function encodeDataXml(sXml) {
            return sXml.replace(/</g, "\u0001");
        }

        function decodeDataXml(sXml) {
            return sXml.replace(/\u0001/g, "<");
        }

        function addRuleClicked() {
            var f = document.getElementById('iframeRS');
            f.src = "RuleSelection.aspx?currIds=" + $("#" + '<%=hiAllIds.ClientID %>').val() + "&t=" + Math.random().toString();
            $('#dialog1').dialog('open');
            return false;
        }

        function getRuleSelectionReturn(sReturn) {
            $('#dialog1').dialog('close');

            var theDom = $(decodeDataXml(sReturn));
            var nodes = $("tr", theDom);
            var sCurrAllIds = "";
            if (nodes.length > 20) {
                alert("You can have up 20 rules in a Rule Group.");
                clearSelectWin('dialog1');
                return;
            }
            for (var i = 0; i < nodes.length; i++) {
                var sId = nodes[i].attributes.getNamedItem("RuleId").value;

                // set allids
                if (sCurrAllIds.length > 0)
                    sCurrAllIds += ",";
                sCurrAllIds += sId;
            }
            $("#" + '<%=hiAllIds.ClientID %>').val(sCurrAllIds);

            $("#" + '<%=hiCurrentData.ClientID %>').val(sReturn);
            renderDataGrid(decodeDataXml(sReturn), "gridRuleList");
            clearSelectWin('dialog1');
        }

        function removeSelectedRules(arrRules) {
            var theDom = $(decodeDataXml($("#" + '<%=hiCurrentData.ClientID %>').val()));
            for (var i = 0; i < arrRules.length; i++) {
                $("tr[RuleId='" + arrRules[i] + "']", theDom).each(function () { $(this).remove(); });
            }
            $("#" + '<%=hiCurrentData.ClientID %>').val(encodeDataXml(theDom.outerHTML()));
        }

        function removeRuleClicked() {
            var sIdToRemove = $("#" + '<%=hiCheckedIds.ClientID %>').val();
            var arrIds = sIdToRemove.split(",");
            if (sIdToRemove.length <= 0 || arrIds.length <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else if (confirm("The selected Rule may be used in the rule alert monitoring. "
                    + "Removing it will stop the monitoring for the rule. Do you want to continue?")) {
                removeSelectedRules(arrIds);
                renderDataGrid(decodeDataXml($("#" + "<%=hiCurrentData.ClientID %>").val()), "gridRuleList");
            }
        }

        function onCreateBtnClicked() {
            var sIds = $("#" + "<%=hiAllIds.ClientID %>").val();
            if (sIds.length > 0) {
                var arrIds = sIds.split(",");
                if (arrIds.length >= 20) {
                    alert("You can have up 20 rules in a Rule Group.");
                    return;
                }
            }
            showRuleSetupWin("0", "");
        }

        function disableRuleBtnClicked() {
            var sIdToDis = $("#" + '<%=hiCheckedIds.ClientID %>').val();
            var arrIds = sIdToDis.split(",");
            if (sIdToDis.length <= 0 || arrIds.length <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else
                return confirm("The selected Rule may be used in the rule alert monitoring. "
                    + "Disabling it will stop the monitoring for the rule. Do you want to continue?");
        }

        function deleteRuleBtnClicked() {
            // first delete from database, second delete from user interface
            var sIdToDel = $("#" + '<%=hiCheckedIds.ClientID %>').val();
            var arrIds = sIdToDel.split(",");
            if (sIdToDel.length <= 0 || arrIds.length <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else
                return confirm("The selected Rule may be used in the rule alert monitoring. "
                    + "Deleting it will stop the monitoring for the rule. This operation is not reverisble, do you want to continue?");
        }

        function afterSelectedRuleDeleted() {
            var sIdToDel = $("#" + '<%=hiCheckedIds.ClientID %>').val();
            var arrIds = sIdToDel.split(",");
            if (sIdToDel.length <= 0 || arrIds.length <= 0) {
                return;
            }
            else {
                // 1.delete rules from page 
                removeSelectedRules(arrIds);
                renderDataGrid(decodeDataXml($("#" + "<%=hiCurrentData.ClientID %>").val()), "gridRuleList");
            }
        }

        function getSelectedCount() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (sIds.length > 0) {
                var arrIds = sIds.split(",");
                return arrIds.length;
            }
            else
                return 0;
        }

        function onUpdateBtnClick() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            var arrIds = sIds.split(",");
            if (sIds.length <= 0 || arrIds.length <= 0) {
                alert("No record has been selected.");
            }
            else if (arrIds.length == 1) {
                showRuleSetupWin("1", arrIds[0]);
            }
            else {
                alert("You can select only one record.");
            }
        }

        function renderDataGrid(sDom, sGridId, sSortCol, sSortOrder) {
            var sEmptyRow = '<tr class="EmptyDataRow" align="center"><td colspan="3">There is no record in database.</td></tr>';
            var sGridHeader = '<tr><th class="CheckBoxHeader" scope="col">'
                + '<input id="ckbAll" type="checkbox" onclick="CheckAllClicked(this, \'gridRuleList\', \'<%=hiAllIds.ClientID %>\', \'<%=hiCheckedIds.ClientID %>\')" /></th>'
                + '<th scope="col"><a href="#" onclick="sortGrid(\'Name\');">Rule</a></th><th scope="col"><a href="#" onclick="sortGrid(\'AlertEmailTpltName\');">Alert Email Template</a></th></tr>';
            var sGridRow = '<tr class="{0}"><td class="CheckBoxColumn">'
                + '<input type="checkbox" onclick="CheckBoxClicked(this, \'ckbAll\', \'<%=hiAllIds.ClientID %>\', \'<%=hiCheckedIds.ClientID %>\', \'{1}\')" />'
                + '</td><td><a onclick="showRuleSetupWin(\'1\', \'{2}\'); return false;" href=\'#\'>{3}</a></td>'
                + '<td><a onclick="UpdateEmailTemplate(\'{4}\'); return false;" href=\'#\'>{5}</a></td></tr>';

            var theDom = $(sDom);
            if (sSortCol == null || typeof (sSortCol) == "undefined")
                sSortCol = "Name";
            if (sSortOrder == null || typeof (sSortOrder) == "undefined")
                sSortOrder = "asc";
            $("tr", theDom).tsort({ order: sSortOrder, attr: sSortCol });
            var nodes = $("tr", theDom);
            var grid = $("#" + sGridId);
            var sCurrAllIds = "";
            $("tr", grid).each(function () { $(this).remove(); });
            if (nodes.length > 0) {
                $(sGridHeader).appendTo(grid);
                var bEven = false;
                for (var i = 0; i < nodes.length; i++) {
                    var sId = nodes[i].attributes.getNamedItem("RuleId").value;
                    var sName = nodes[i].attributes.getNamedItem("Name").value;
                    var sTpltId = nodes[i].attributes.getNamedItem("AlertEmailTemplId").value;
                    var sTpltName = nodes[i].attributes.getNamedItem("AlertEmailTpltName").value;
                    var sNewRow = "";
                    sNewRow = sGridRow.replace("{0}", bEven ? "EvenRow" : ""); bEven = !bEven;
                    sNewRow = sNewRow.replace("{1}", sId);
                    sNewRow = sNewRow.replace("{2}", sId);
                    sNewRow = sNewRow.replace("{3}", sName);
                    sNewRow = sNewRow.replace("{4}", sTpltId);
                    sNewRow = sNewRow.replace("{5}", sTpltName);
                    $(sNewRow).appendTo(grid);

                    // set allids
                    if (sCurrAllIds.length > 0)
                        sCurrAllIds += ",";
                    sCurrAllIds += sId;
                }
            }
            else {
                $(sEmptyRow).appendTo(grid);
            }
            $("#" + '<%=hiCheckedIds.ClientID %>').val("");
            $("#" + '<%=hiAllIds.ClientID %>').val(sCurrAllIds);
        }

        var sRuleNameOrder = "asc";
        var sEmailTpltNameOrder = "asc";
        function sortGrid(sAttr) {
            var sOrder = "asc";
            switch (sAttr) {
                case "Name":
                    sOrder = sRuleNameOrder;
                    sRuleNameOrder = sRuleNameOrder == "asc" ? "desc" : "asc";
                    sEmailTpltNameOrder = "asc";
                    break;
                case "AlertEmailTpltName":
                    sOrder = sEmailTpltNameOrder;
                    sEmailTpltNameOrder = sEmailTpltNameOrder == "asc" ? "desc" : "asc";
                    sRuleNameOrder = "asc";
                    break;
                default:
                    break;
            }
            var sDom = decodeDataXml($("#" + '<%=hiCurrentData.ClientID %>').val());
            renderDataGrid(sDom, "gridRuleList", sAttr, sOrder);
        }
    </script>
    <script type="text/javascript">
        function initRuleSetupWin() {
            $('#dialogRule').dialog({
                modal: false,
                autoOpen: false,
                title: 'Rule Setup',
                width: 830,
                height: 640,
                resizable: false,
                close: clearRuleSetupWin
            });
        }

        function showRuleSetupWin(mode, id) {
            var f = document.getElementById('ifrRuleEdit');
            if (null == mode || "" == mode)
                mode = "0";
            if (null == id)
                id = "";
            if (mode == "0")
                f.src = "RuleAdd.aspx?needret=1&t=" + Math.random().toString();
            else
                f.src = "RuleEdit.aspx?needret=1&RuleID=" + id + "&t=" + Math.random().toString();
            $('#dialogRule').dialog('open');
        }

        function clearRuleSetupWin() {
            var f = document.getElementById('ifrRuleEdit');
            f.src = "about:blank";
        }

        function newRuleAdded(sNewRule) {
            var theDom = $(decodeDataXml($("#" + '<%=hiCurrentData.ClientID %>').val()));
            var newDom = $(decodeDataXml(sNewRule));
            var newNodes = $("tr", newDom);
            if (newNodes.length > 0) {
                var newNode = $(newNodes[0]).clone();
                var sId = newNode.attr("RuleId");
                $("tr[RuleId='" + sId + "']", theDom).each(function () { $(this).remove(); });
                newNode.appendTo(theDom)
                $("#" + '<%=hiCurrentData.ClientID %>').val(encodeDataXml(theDom.outerHTML()));
                renderDataGrid(theDom.outerHTML(), "gridRuleList");
            }
            closeRuleSetupWin();
        }

        function closeRuleSetupWin() {
            $('#dialogRule').dialog('close');
        }

        function ruleUpdated(sReturn) {
            newRuleAdded(sReturn);
        }

        function ruleDeleted(sId) {
            removeSelectedRules([sId]);
            renderDataGrid(decodeDataXml($("#" + "<%=hiCurrentData.ClientID %>").val()), "gridRuleList");
        }
    </script>
    <script type="text/javascript">

        //#region Email Template Setup

        function UpdateEmailTemplate(EmailTemplateID) {

            var result = confirm("If you leave this page without save your changes, you will lose the changes you have made so far. You probably want to save the changes first.\r\n\r\nDo you still want to continue and lose your changes?");
            if (result == false) {

                return;
            }

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "EmailTemplateEditParent.aspx?sid=" + sid + "&EmailTemplateID=" + EmailTemplateID;

        }

        //#region Show Dialog

        function ShowDialog(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divEmailTemplateSetup").attr("title", Title);

            $("#ifrEmailTemplateSetup").attr("src", "");
            $("#ifrEmailTemplateSetup").attr("src", iFrameSrc);
            $("#ifrEmailTemplateSetup").width(iFrameWidth);
            $("#ifrEmailTemplateSetup").height(iFrameHeight);

            // show modal
            $("#divEmailTemplateSetup").dialog({
                height: divHeight,
                width: divWidth,
                modal: false,
                resizable: false,
                close: function (event, ui) { $("#divEmailTemplateSetup").dialog("destroy"); }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog() {

            $("#divEmailTemplateSetup").dialog("close");
        }

        //#endregion

        //#endregion
        

        function CloseDialog_AddRule() {
            $("#dialogRule").dialog("close");
        }
        function CloseDialog_EditRule() {
            $("#dialogRule").dialog("close");
        }
        
        function cloneBtnClicked() {
            $("#" + '<%=hiCloned.ClientID %>').val("1");
        }
    </script>
    <script type="text/javascript">
        function deleteRuleGroupButtonClicked() {
            var sSign = $("#" + "<%=hiReferenced.ClientID %>").val();
            if (sSign == "1")
                return confirm('The Rule Group has been referenced by rule alert monitoring. '
                    + 'Deleting this Rule Group will also stop the rule monitoring of the associated rules. '
                    + 'This operation is not reversible, do you want to continue?');
            else
                return confirm('This operation is not reversible, do you want to continue?');
        }

        function saveSuccess() {
            alert("Save successfully!");
            closeBox(true, true);
        }

        function closeBox(isRefresh, bReset) {
            if (bReset === false)
                bReset = false;
            else
                bReset = true;
            self.parent.closeRuleGroupSetupWin(isRefresh, bReset);
            return false;
        }
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="aspnetForm">
        <div class="Heading">
            Rule Group Setup</div>
        <div class="SplitLine">
        </div>
        <div class="DetailsContainer">
            <div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="white-space: nowrap;">
                            Rule Group Name:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:TextBox ID="tbGroupName" runat="server" class="iTextBox" Style="width: 402px;"
                                MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvGroupName" runat="server" ControlToValidate="tbGroupName"
                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2" style="text-align:right; padding-right:20px;">
                            Enabled&nbsp;&nbsp;<asp:CheckBox ID="ckbEnabled" runat="server" Checked="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px; white-space: nowrap;">
                            Description:
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="3">
                            <asp:TextBox ID="tbDescription" runat="server" TextMode="MultiLine" Width="690px"
                                Height="44px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px; white-space: nowrap;">
                            Scope:
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:DropDownList ID="ddlScope" runat="server" DataTextField="ScopeName" DataValueField="ScopeId" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-top: 9px; white-space: nowrap; text-align:right; width:60px;">
                            Target:
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:DropDownList ID="ddlTarget" runat="server" DataTextField="TargetName" DataValueField="TargetId" Width="178px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hiReferenced" runat="server" Value="0" />
                <asp:HiddenField ID="hiCloned" runat="server" Value="0" />
            </div>
            <div style="padding-left: 10px; padding-right: 10px;">
                <div id="divToolBar" style="margin-top: 13px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td style="width: 350px;">
                                <ul class="ToolStrip">
                                    <li>
                                        <asp:LinkButton ID="lbtnAdd" runat="server" OnClientClick="addRuleClicked(); return false;">Add</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnRemove" runat="server" OnClientClick="removeRuleClicked(); return false;">Remove</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnCreate" runat="server" OnClientClick="onCreateBtnClicked(); return false;">Create</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnDisable" runat="server" OnClientClick="return disableRuleBtnClicked();"
                                            OnClick="lbtnDisable_Click">Disable</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" OnClientClick="return deleteRuleBtnClicked();"
                                            OnClick="lbtnDelete_Click">Delete</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnUpdate" runat="server" OnClientClick="onUpdateBtnClick(); return false;">Update</asp:LinkButton></li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="div2" class="ColorGrid" style="margin-top: 5px;">
                <table class="GrayGrid" cellspacing="0" cellpadding="3" border="0" id="gridRuleList"
                    style="border-collapse: collapse;">
                </table>
                <asp:HiddenField ID="hiAllIds" runat="server" />
                <asp:HiddenField ID="hiCheckedIds" runat="server" />
                <asp:HiddenField ID="hiCreatedId" runat="server" />
                <asp:HiddenField ID="hiCurrentData" runat="server" />
                <div class="GridPaddingBottom">
                    &nbsp;</div>
            </div>
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lbtnDisable" />
                    <asp:AsyncPostBackTrigger ControlID="lbtnDelete" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="DashedBorder" style="margin-top: 15px;">
                &nbsp;</div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnClone" runat="server" Text="Clone" class="Btn-66" OnClientClick="cloneBtnClicked()"
                                OnClick="btnClone_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" class="Btn-66" OnClientClick="return deleteRuleGroupButtonClicked();"
                                OnClick="btnDelete_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="Btn-66" OnClientClick="return closeBox(false, false);" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <div id="dialog1" title="Rule Selection">
            <iframe id="iframeRS" name="iframeRS" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="dialogRule" title="Rule Setup">
            <iframe id="ifrRuleEdit" name="ifrRuleEdit" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="divEmailTemplateSetup" title="Email Template Setup" style="display: none;">
            <iframe id="ifrEmailTemplateSetup" frameborder="0" scrolling="no" width="100px" height="100px"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
