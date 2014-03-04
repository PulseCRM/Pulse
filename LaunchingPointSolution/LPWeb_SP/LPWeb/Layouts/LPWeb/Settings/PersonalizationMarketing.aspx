<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalizationMarketing.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Settings.PersonalizationMarketing" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <style type="text/css">
        .TabContent table td
        {
            padding-top: 9px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();

            initCampaignsSelectWin();
        });
    </script>
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
            // $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);

            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');
        }

        function SelectedItemCount() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (null == sIds || sIds.length == 0)
                return 0;
            var arrIds = sIds.split(",");
            return arrIds.length;
        }
    </script>
    <script type="text/javascript">
        // Template Stage dropdownlist
        function loanTypeChanged(me, ddlTemplStage1Id, ddlTemplStage2Id, hiTemplId) {
            var sLoanType = $(me).val();
            var stage1 = $("#" + ddlTemplStage1Id);
            var stage2 = $("#" + ddlTemplStage2Id);
            var hiStage = $("#" + hiTemplId);
            switch (sLoanType.toLowerCase()) {
                case "active":                    
                case "archived":
                    stage1.show();
                    stage2.hide();
                    hiStage.val(stage1.val());
                    break;
                case "opportunities":
                    stage1.hide();
                    stage2.show();
                    hiStage.val(stage2.val());
                    break;
                default:
                    stage1.hide();
                    stage2.hide();
                    hiStage.val("");
                    break;
            }
        }

        function ddlTemplStageChanged(me, hiTemplId) {
            var hiStage = $("#" + hiTemplId);
            $(hiStage).val($(me).val());
        }
    </script>
    <script type="text/javascript">
        // add or remove campaigns
        function initCampaignsSelectWin() {
            $('#dialogCampaignsSelect').dialog({
                modal: true,
                autoOpen: false,
                title: 'Select Marketing Campaign',
                width: 850,
                height: 620,
                resizable: false,
                close: clearCampaignsSelectWin
            });
        }
        function showCampaignsSelectWin() {
            var f = document.getElementById('iframeCampaignsSelect');
            f.src = "../Marketing/SelectMarketingCampaignForRule.aspx?type=2&CloseDialogCodes=parent.closeCampaignsSelectWin()&t=" + Math.random().toString();
            $('#dialogCampaignsSelect').dialog('open');
        }
        function clearCampaignsSelectWin() {
            var f = document.getElementById('iframeCampaignsSelect');
            f.src = "about:blank";
        }
        function closeCampaignsSelectWin() {
            $('#dialogCampaignsSelect').dialog('close');
        }
        function getCampaignSelectionReturn(sIds) {
            var SelID = result.substring(0, sIds.indexOf("^"));
            $('#' + '<%=hiReturnedIds.ClientID %>').val(SelID);
            $('#dialogCampaignsSelect').dialog('close');
            if (sIds.length > 0)
            {
                ShowTargetSelectWin();
            }
        }
        function ShowTargetSelectWin() {
            $("#dialogTargetSelect").dialog({
                height: 150,
                width: 320,
                modal: true,
                title: "",
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }
        function SaveTarget() { 
            $("#<%=hiLoanTypeSel.ClientID %>").val($("#<%=ddlLoanTypeSel.ClientID %>").val());
            if($("#<%=hiLoanTypeSel.ClientID %>").val() == "" || $("#<%=hiTemplStageSel.ClientID %>").val() == "")
            {
                alert("The Target and Stage Completion cannot be blank.");
                return false;
            }
            <%=this.ClientScript.GetPostBackEventReference(this.lbtnAdd, null) %>;
        }

        function addCampaignsBtnClicked() {
            showCampaignsSelectWin();
        }

        function removeCampaignsBtnClicked() {
            if (SelectedItemCount() <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else if (SelectedItemCount() > 1)
            {
                alert("Only one record can be selected for this operation.");
                return false;
            }
            else {
                return confirm('Are you sure you want to delete the selected campaign?');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        User Personalization</div>
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
                                <li><a href="PersonalizationSettings.aspx"><span>Settings</span></a></li>
                                <li><a href="PersonalizationPreferences.aspx"><span>Preferences</span></a></li>
                                <li id="current"><a href="PersonalizationMarketing.aspx"><span>Marketing</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine" style="width: 242px">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">
                    &nbsp;</div>
                <div class="TabContent">
                    <div id="divFilter" style="margin-top: 10px;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbEnableMarketing" runat="server" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Enable marketing features
                                </td>
                                <td style="padding-left: 35px;">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-top: 10px;">Auto campaigns:</div>
                    <div id="divToolBar" style="margin-top: 10px;">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <ul class="ToolStrip" style="margin-left: 0px;">
                                        <li>
                                            <asp:LinkButton ID="lbtnAdd" runat="server" OnClientClick="addCampaignsBtnClicked(); return false;" OnClick="lbtnAdd_Click">Add</asp:LinkButton><span>|</span></li>
                                            <asp:HiddenField ID="hiReturnedIds" runat="server" />
                                        <li>
                                            <asp:LinkButton ID="lbtnRemove" runat="server" OnClientClick="return removeCampaignsBtnClicked();" OnClick="lbtnRemove_Click">Remove</asp:LinkButton></li>
                                    </ul>
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
                    <div id="divList" class="ColorGrid" style="margin-top: 3px; width: 800px;">
                        <asp:GridView ID="gridList" runat="server" DataKeyNames="CampaignId,LoanType,TemplStageId" EmptyDataText="There is no campaigns in database."
                            AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound" OnPreRender="gridList_PreRender"
                            CellPadding="3" CssClass="GrayGrid" GridLines="None" OnSorting="gridList_Sorting"
                            AllowSorting="true">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="ckbAll" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbSelect" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CampaignName" SortExpression="CampaignName" HeaderText="Marketing Campaign" />
                                <asp:TemplateField SortExpression="LoanType" HeaderText="Target">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlLoanType" runat="server" Width="150px">
                                            <asp:ListItem Value="">-- select --</asp:ListItem>
                                            <asp:ListItem Value="Active">Active Loans</asp:ListItem>
                                            <asp:ListItem Value="Archived">Archived Loans</asp:ListItem>
                                            <asp:ListItem Value="Opportunities">Leads</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLoanType" runat="server" ErrorMessage="Target required." ControlToValidate="ddlLoanType" Text="*"></asp:RequiredFieldValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="TemplStageName" HeaderText="Stage Completion">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlTemplStage1" runat="server" DataValueField="TemplStageId"
                                            DataTextField="Name" Width="150px">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlTemplStage2" runat="server" DataValueField="TemplStageId"
                                            DataTextField="Name" Width="150px">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="tbTeplStageId" runat="server" Value='<%# Eval("TemplStageId") %>' style="display:none;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvStage" runat="server" ErrorMessage="Stage Completion required." ControlToValidate="tbTeplStageId" Text="*"></asp:RequiredFieldValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Enabled" HeaderText="Enabled">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ckbEnabled" runat="server" Checked='<%# Eval("Enabled") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">
                            &nbsp;</div>
                        <asp:HiddenField ID="hiAllIds" runat="server" />
                        <asp:HiddenField ID="hiCheckedIds" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <div id="dialogCampaignsSelect">
            <iframe id="iframeCampaignsSelect" name="iframeCampaignsSelect" frameborder="0" width="100%"
                height="100%"></iframe>
        </div>
        <div id="dialogTargetSelect" title="Target and Stage Completion Selection" style="display: none">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="padding-top: 15px;">
                        Target:
                    </td>
                    <td style="padding-top: 15px; padding-left: 10px">
                        <asp:dropdownlist id="ddlLoanTypeSel" runat="server" width="150px">
                            <asp:ListItem Value="">-- select --</asp:ListItem>
                            <asp:ListItem Value="Active">Active Loans</asp:ListItem>
                            <asp:ListItem Value="Archived">Archived Loans</asp:ListItem>
                            <asp:ListItem Value="Opportunities">Leads</asp:ListItem>
                        </asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">
                        Stage Completion:
                    </td>
                    <td style="padding-top: 10px; padding-left: 10px">
                        <asp:dropdownlist id="ddlTemplStageSel1" runat="server" datavaluefield="TemplStageId"
                            datatextfield="Name" width="150px" style="display: none;"></asp:dropdownlist>
                        <asp:dropdownlist id="ddlTemplStageSel2" runat="server" datavaluefield="TemplStageId"
                            datatextfield="Name" width="150px" style="display: none;"></asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 15px;">
                        <center><input id="btnOK" type="button" value="Yes" class="Btn-66" onclick="return SaveTarget()" /></center>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hiLoanTypeSel" runat="server" />
        <asp:HiddenField ID="hiTemplStageSel" runat="server" />
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
</asp:Content>
