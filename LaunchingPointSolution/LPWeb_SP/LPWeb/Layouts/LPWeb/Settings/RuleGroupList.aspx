<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Group List" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" CodeBehind="RuleGroupList.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.RuleGroupList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
            // $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);

            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            initSetupWin();
        });
        function initSetupWin() {
            $('#dialogSetup').dialog({
                modal: true,
                autoOpen: false,
                title: 'Rule Group Setup',
                width: 880,
                height: 810,
                resizable: false,
                close: clearSetupWin
            });
        }
        function showSetupWin(mode, id)
        {
            var f = document.getElementById('iframeUS');
            if (null == mode || "" == mode)
                mode = "0";
            if (null == id)
                id = "";
            f.src = "RuleGroupSetup.aspx?mode=" + mode + "&id=" + id + "&t=" + Math.random().toString();
            $('#dialogSetup').dialog('open');
        }
        function clearSetupWin() {
            var f = document.getElementById('iframeUS');
            f.src = "about:blank";
        }
        function closeRuleGroupSetupWin(bRefresh, bResetPager)
        {
            $('#dialogSetup').dialog('close');
            if (bRefresh === true)
            {
                if (bResetPager === false)
                    <%=this.ClientScript.GetPostBackEventReference(this.lbtnEmpty, null) %>;
                else
                    <%=this.ClientScript.GetPostBackEventReference(this.lbtnEmptyReset, null) %>;
            }
        }
    </script>
    <script type="text/javascript">
        function disableBtnClicked() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (sIds.length <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else {
                return confirm('The selected Rule Group may be used in the rule alert monitoring. '
                    + 'Disabling it will stop the monitoring for the associated rules. Do you want to continue?');
            }
        }

        function enableBtnClicked() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (sIds.length <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else {
                return confirm('Are you sure you want to continue?');
            }
        }

        function deleteBtnClicked() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (sIds.length <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else {
                var arrIds = sIds.split(",");
                for (var n = 0; n < arrIds.length; n++) {
                    if (isRuleReferenced(arrIds[n]))
                        return confirm('The Rule Groups have been referenced by rule alert monitoring. '
                    + 'Deleting the Rule Groups will also stop the rule monitoring of the associated rules. This operation is not reversible, '
                    + 'do you want to continue?');
                }
                return confirm('The selected Rule Group may be referenced in the Loans. '
                    + 'Deleting it will remove the monitoring for the associated rules. This operation is not reversible, do you want to continue?');
            }
        }

        function isRuleReferenced(sRid) {
            // check reference
            var sRef = $("#" + "<%=hiReferenced.ClientID %>").val();
            var arrRef = sRef.split(";");
            for (var i = 0; i < arrRef.length; i++) {
                var arrTemp = arrRef[i].split(":");
                if (arrTemp.length == 2 && arrTemp[0] == sRid) {
                    var nCount = new Number(arrTemp[1]);
                    if (!isNaN(nCount))
                        if (nCount > 0)
                            return true;
                }
            }
            return false;
        }

        function onUpdateBtnClick() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (sIds.length <= 0) {
                alert("No record has been selected.");
                return;
            }
            var arrIds = sIds.split(",");
            if (arrIds.length < 1) {
                alert("No record has been selected.");
            }
            else if (arrIds.length == 1) {
                showSetupWin("1", arrIds[0]);
            }
            else {
                alert("You can select only one record.");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle">
        Rule Group List</div>
    <div class="SplitLine">
    </div>
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div style="padding-left: 10px; padding-right: 10px;">
                <div id="divToolBar" style="margin-top: 13px;">
                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
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
                            <td style="width: 350px;">
                                <ul class="ToolStrip">
                                    <li>
                                        <asp:LinkButton ID="lbtnCreate" runat="server" Text="Create" OnClientClick="showSetupWin('0', ''); return false;"></asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnDisable" runat="server" OnClientClick="return disableBtnClicked();"
                                            OnClick="lbtnDisable_Click">Disable</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnEnable" runat="server" OnClientClick="return enableBtnClicked();"
                                            OnClick="lbtnEnable_Click">Enable</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" OnClientClick="return deleteBtnClicked();"
                                            OnClick="lbtnDelete_Click">Delete</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnUpdate" runat="server" OnClientClick="onUpdateBtnClick(); return false;">Update</asp:LinkButton></li>
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
            </div>
            <div id="div1" class="ColorGrid"  style="width: 850px; margin-top: 5px;">
                <asp:GridView ID="gridList" runat="server" DataKeyNames="RuleGroupId,Referenced" EmptyDataText="There is no record in database."
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
                        <asp:TemplateField HeaderText="Rule Group Name" SortExpression="Name" HeaderStyle-Width="300" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnName" runat="server" Text='<%# string.Format("{0}", Eval("Name")).Replace("<", "&amp;#60;") %>' OnClientClick='<%#string.Format("showSetupWin(\"1\", \"{0}\"); return false;", Eval("RuleGroupId")) %>' ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Desc" HeaderStyle-Width="280" >
                            <ItemTemplate>
                                <asp:Label ID="lblDesc" runat="server" Text='<%# string.Format("{0}", Eval("Desc")).Replace("<", "&amp;#60;") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scope" HeaderStyle-Width="60" SortExpression="ScopeName">
                            <ItemTemplate>
                                <asp:Label ID="lbScope" runat="server" Text='<%# Eval("ScopeName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Target" HeaderStyle-Width="150" SortExpression="TargetName">
                            <ItemTemplate>
                                <asp:Label ID="lbTarget" runat="server" Text='<%# Eval("TargetName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled" HeaderStyle-Width="55" >
                            <ItemTemplate>
                                <asp:Label ID="lblEnabled" runat="server" Text='<%# Eval("Enabled").ToString().ToLower() == "true" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">
                    &nbsp;</div>
                <asp:HiddenField ID="hiAllIds" runat="server" />
                <asp:HiddenField ID="hiCheckedIds" runat="server" />
                <asp:HiddenField ID="hiReferenced" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="display: none;">
        <div id="dialogSetup">
            <iframe id="iframeUS" name="iframeRS" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <asp:LinkButton ID="lbtnEmpty" runat="server" OnClick="lbtnEmpty_Click"></asp:LinkButton>
        <asp:LinkButton ID="lbtnEmptyReset" runat="server" OnClick="lbtnEmptyReset_Click"></asp:LinkButton>
    </div>
</asp:Content>
