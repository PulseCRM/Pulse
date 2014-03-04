<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerRolesSetup.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Contact.PartnerRolesSetup" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
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

        function SelectedItemCount() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (null == sIds || sIds.length == 0)
                return 0;
            var arrIds = sIds.split(",");
            return arrIds.length;
        }
    </script>
    <script type="text/javascript">
        function btnRemoveClicked() {
            if (SelectedItemCount() <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else
                return confirm('Deleting the selected Contact Role(s) will also remove the loan contact information. Are you sure you want to continue?');
        }
        function btnEnableClicked() {
            if (SelectedItemCount() <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else
                return true;
        }
        function btnDisableClicked() {
            if (SelectedItemCount() <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else
                return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle">
        Partner Role Setup</div>
    <div class="SplitLine">
    </div>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Contact Role
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="tbRoleName" runat="server" class="iTextBox" Style="width: 240px;"
                            MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="padding-left: 15px; padding-top: 3px;">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="Btn-66" OnClick="btnAdd_Click" />
                    </td>
                </tr>
            </table>
        </div>
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
                                <asp:LinkButton ID="lbtnRemove" runat="server" OnClientClick="return btnRemoveClicked();"
                                    OnClick="lbtnRemove_Click">Remove</asp:LinkButton><span>|</span></li>
                            <li>
                                <asp:LinkButton ID="lbtnEnable" runat="server" OnClientClick="return btnEnableClicked();"
                                    OnClick="lbtnEnable_Click">Enable</asp:LinkButton><span>|</span></li>
                            <li>
                                <asp:LinkButton ID="lbtnDisable" runat="server" OnClientClick="return btnDisableClicked();"
                                    OnClick="lbtnDisable_Click">Disable</asp:LinkButton></li>
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
    <div id="div1" class="ColorGrid" style="margin-top: 5px; width: 420px;">
        <asp:GridView ID="gridList" runat="server" DataKeyNames="ContactRoleId" EmptyDataText="There is no contact role in database."
            AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound" CellPadding="3"
            OnPreRender="gridList_PreRender" CssClass="GrayGrid" GridLines="None" OnSorting="gridList_Sorting"
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
                <asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Contact Role" />
                <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled">
                    <ItemTemplate>
                        <asp:Label ID="lblEnabled" runat="server" Text='<%# Eval("Enabled").ToString().ToLower() == "true" ? "Yes" : "No" %>'></asp:Label>
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
</asp:Content>
