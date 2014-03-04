<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserForReassignSelection.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Settings.UserForReassignSelection" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Selection</title>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
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
        function btnSelectClicked() {
            if (SelectedItemCount() <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else if (SelectedItemCount() > 1) {
                alert("Only one user can be selected for this operation.");
                return false;
            }
            else {
                var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
                var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
                if (CloseDialogCodes == "") {
                    window.parent.UserForReassignSelectionSelected(sIds);
                }
                else {
                    // return selected user ids to parent window
                    var selectClickedCode = CloseDialogCodes.replace("returnValue", sIds);
                    eval(selectClickedCode);
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="aspnetForm">
        <div id="divToolBar" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 40px;">
                        <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="Btn-66" OnClientClick="return btnSelectClicked();" />
                    </td>
                    <td style="width: 350px;">
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
                            OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                            FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                            ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                            LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div1" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridList" runat="server" DataKeyNames="UserId,FirstName,LastName"
                EmptyDataText="There is no record in database." AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound"
                OnPreRender="gridList_PreRender" OnSorting="gridList_Sorting" CellPadding="3"
                CssClass="GrayGrid" GridLines="None" AllowSorting="true">
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
                    <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="Name" />
                    <asp:TemplateField HeaderText="Branch">
                        <ItemTemplate>
                            <asp:Label ID="lblBranch" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">
                &nbsp;</div>
            <asp:HiddenField ID="hiAllIds" runat="server" />
            <asp:HiddenField ID="hiCheckedIds" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
