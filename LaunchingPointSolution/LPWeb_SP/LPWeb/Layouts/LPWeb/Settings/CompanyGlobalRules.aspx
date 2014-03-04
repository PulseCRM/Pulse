<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyGlobalRules.aspx.cs" Inherits="CompanyGlobalRules" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            DrawTab();

            InitTableSorter();

            // left click context menu
            $("#aAdd").contextMenu({ menu: 'divContactMenuAdd', leftButton: true, onShowMenu: function () { $("#divContactMenuAdd").css({ top: 103, left: 30 }).fadeIn(-1); } }, function (action, el, pos) { contextMenuWork1(action, el, pos); });
            $("#aCreate").contextMenu({ menu: 'divContactMenuCreate', leftButton: true, onShowMenu: function () { $("#divContactMenuCreate").css({ top: 103, left: 73 }).fadeIn(-1); } }, function (action, el, pos) { contextMenuWork2(action, el, pos); });
        });

        function InitTableSorter() {

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList").tablesorter({

                headers: {
                    0: { sorter: false }
                },
                widgets: ['zebra']
            });
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr td :checkbox").attr("checked", "");
            }
        }

        //#region Show Dialog

        function ShowDialog(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divPopup").attr("title", Title);

            $("#ifrPopup").attr("src", "");
            $("#ifrPopup").attr("src", iFrameSrc);
            $("#ifrPopup").width(iFrameWidth);
            $("#ifrPopup").height(iFrameHeight);

            // show modal
            $("#divPopup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) { $("#divPopup").dialog("destroy"); }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog() {

            $("#divPopup").dialog("close");
        }

        //#endregion

        //#region context menu - Add

        function contextMenuWork1(action, el, pos) {

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

        function ShowDialog_AddRuleGroup() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/GlobalRuleGroupSelectionPopup.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseDialog()";

            ShowDialog("Select Rule Group", 410, 410, 435, 450, iFrameSrc);
        }

        function ShowDialog_AddRule() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/GlobalRuleSelectionPopup.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseDialog()";

            ShowDialog("Select Rule", 410, 410, 435, 450, iFrameSrc);
        }

        //#endregion

        //#region contact menu - Create

        function contextMenuWork2(action, el, pos) {

            switch (action) {
                case "RuleGroup":
                    {
                        ShowDialog_CreateRuleGroup();
                        break;
                    }
                case "Rule":
                    {
                        ShowDialog_CreateRule();
                        break;
                    }
            }
        }

        function ShowDialog_CreateRuleGroup() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "RuleGroupSetup.aspx?mode=0&id=&t=" + RadomStr;

            ShowDialog("Rule Group Setup", 855, 770, 880, 810, iFrameSrc);
        }

        function ShowDialog_CreateRule() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "RuleAdd.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseDialog()";

            ShowDialog("Rule Setup", 807, 700, 830, 740, iFrameSrc);
        }

        //#endregion

        //#region disable/remove/delete

        function GetSelectedRuleIDs() {

            var SelRuleIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").each(function (i) {

                var RuleID = $(this).attr("myRuleID");
                if (i == 0) {

                    SelRuleIDs = RuleID;
                }
                else {

                    SelRuleIDs += "," + RuleID;
                }
            });

            return SelRuleIDs;
        }

        function GetSelectedRuleTypes() {

            var SelRuleTypes = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").each(function (i) {

                var RuleType = $(this).attr("myRuleType");
                if (i == 0) {

                    SelRuleTypes = RuleType;
                }
                else {

                    SelRuleTypes += "," + RuleType;
                }
            });

            return SelRuleTypes;
        }

        function BeforeDisable() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No rule record has been selected.");
                return false;
            }

            var SelRuleIDs = GetSelectedRuleIDs();
            //alert(SelRuleIDs);

            var SelRuleTypes = GetSelectedRuleTypes();
            //alert(SelRuleTypes);

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedRuleIDs").val(SelRuleIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedRuleTypes").val(SelRuleTypes);

            return true;
        }

        function BeforeRemove() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No rule record has been selected.");
                return false;
            }

            var SelRuleIDs = GetSelectedRuleIDs();
            //alert(SelRuleIDs);

            var SelRuleTypes = GetSelectedRuleTypes();
            //alert(SelRuleTypes);

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedRuleIDs").val(SelRuleIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedRuleTypes").val(SelRuleTypes);

            return true;
        }

        function BeforeDelete() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No rule record has been selected.");
                return false;
            }

            var result = confirm("Deleting the rule(s) will cause the rule monitoring to stop. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }

            var SelRuleIDs = GetSelectedRuleIDs();
            //alert(SelRuleIDs);

            var SelRuleTypes = GetSelectedRuleTypes();
            //alert(SelRuleTypes);

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedRuleIDs").val(SelRuleIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedRuleTypes").val(SelRuleTypes);

            return true;
        }

        //#endregion

        //#region update rule

        function aUpdate_onclick() {

            var SelectedCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No rule record has been selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one rule record can be selected.");
                return;
            }

            var RuleType = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").attr("myRuleType");
            var RuleID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").attr("myRuleID");

            EditRuleOrRuleGroup(RuleType, RuleID);
        }
        
        function EditRuleOrRuleGroup(RuleType, RuleID) {

            if (RuleType == "Rule") {

                ShowDialog_EditRule(RuleID)
            }
            else {

                ShowDialog_EditRuleGroup(RuleID)
            }
        }

        function ShowDialog_EditRuleGroup(RuleGroupID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "RuleGroupSetup.aspx?mode=1&id=" + RuleGroupID + "&t=" + RadomStr;

            ShowDialog("Rule Group Setup", 855, 770, 880, 810, iFrameSrc);
        }
        
        function ShowDialog_EditRule(RuleID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "RuleEdit.aspx?sid=" + RadomStr + "&RuleID=" + RuleID + "&CloseDialogCodes=window.parent.CloseDialog()&iframe=ifrPopup";

            ShowDialog("Rule Setup", 807, 700, 830, 740, iFrameSrc);
        }

        //#endregion

        //#region Email Template

        function UpdateEmailTemplate(EmailTemplateID) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "EmailTemplateEditParent.aspx?sid=" + sid + "&EmailTemplateID=" + EmailTemplateID;

        }

        //#endregion
// ]]>
    </script>

    <script type="text/javascript">
        function closeRuleGroupSetupWin(bRefresh, bResetPager)
        {
            $('#divPopup').dialog('close');
            if (bRefresh === true)
            {
                <%=this.ClientScript.GetPostBackEventReference(this.lbtnEmpty, null) %>;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer">
        <div id="divModuleName" class="Heading">Company Setup</div>
        <div style="margin-top: 15px;">
            <div class="JTab">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 10px;">&nbsp;</td>
                        <td>
                            <div id="tabs10">
                                <ul>
                                    <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                    <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                    <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                    <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                    <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                     <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                               <li id="current"><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
<%--                                <li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>


                              
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="TabBody">
                    <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
                    <div id="TabLine2" class="TabRightLine">&nbsp;</div>
                    <div class="TabContent">
                        <div id="divToolBar" style="margin-top: 10px;">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aAdd" href="" onclick="return false;">Add</a><span>|</span></li>
                                <li><a id="aCreate" href="" onclick="return false;">Create</a><span>|</span></li>
                                <li><a id="aUpdate" href="javascript:aUpdate_onclick()">Update</a><span>|</span></li>
                                <li><asp:LinkButton ID="lnkDisable" runat="server" OnClientClick="return BeforeDisable()" onclick="lnkDisable_Click">Disable</asp:LinkButton><span>|</span></li>
                                <li><asp:LinkButton ID="lnkRemove" runat="server" OnClientClick="return BeforeRemove()" OnClick="lnkRemove_Click">Remove</asp:LinkButton><span>|</span></li>
                                <li><asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return BeforeDelete()" onclick="lnkDelete_Click">Delete</asp:LinkButton></li>
                            </ul>
                        </div>
                        <div id="divRuleList" class="ColorGrid" style="margin-top: 1px; width: 800px;">
                            <asp:GridView ID="gridRuleList" runat="server" EmptyDataText="There is no rule and rule group." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid tablesorter" GridLines="None">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="chkChecked" type="checkbox" myRuleType="<%# Eval("RuleType") %>" myRuleID="<%# Eval("RuleID") %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RuleType" HeaderText="Type" ItemStyle-Width="70px" />
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <a href="javascript:EditRuleOrRuleGroup('<%# Eval("RuleType") %>', '<%# Eval("RuleID") %>')"><%# Eval("RuleName")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Alert Email">
                                        <ItemTemplate>
                                            <a href="javascript:UpdateEmailTemplate('<%# Eval("AlertEmailTemplID")%>')"><%# Eval("AlertEmail")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Enabled" HeaderText="Enabled" ItemStyle-Width="70px" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <ul id="divContactMenuAdd" class="contextMenu">
        <li><a href="#RuleGroup">Rule Group</a></li>
        <li><a href="#Rule">Rule</a></li>
    </ul>
    <ul id="divContactMenuCreate" class="contextMenu">
        <li><a href="#RuleGroup">Rule Group</a></li>
        <li><a href="#Rule">Rule</a></li>
    </ul>
    <div id="divHiddenFields">
        <asp:HiddenField ID="hdnSelectedRuleIDs" runat="server" />
        <asp:HiddenField ID="hdnSelectedRuleTypes" runat="server" />
    </div>
    <div id="divPopup" title="Popup" style="display: none;">
        <iframe id="ifrPopup" frameborder="0" scrolling="no" width="100px" height="100px"></iframe>
    </div>
    <div style="display:none;">
        <asp:LinkButton ID="lbtnEmpty" runat="server" OnClick="lbtnEmpty_Click"></asp:LinkButton>
    </div>
</asp:Content>