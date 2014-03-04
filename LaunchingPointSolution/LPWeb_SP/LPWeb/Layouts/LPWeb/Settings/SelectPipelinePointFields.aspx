<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectPipelinePointFields.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.SelectPipelinePointFields" DynamicMasterPageFile="~masterurl/default.master" %>

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
    <title>Select Pipeline Point Fields Popup</title>
    <script type="text/javascript">
        // check/decheck all
        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#" + '<%=gridPointFields.ClientID %>' + " tr td input[type=checkbox]").each(function () {
                    if (this.disabled) return;
                    this.checked = true;
                });
            }
            else {
                $("#" + '<%=gridPointFields.ClientID %>' + " tr td input[type=checkbox]").each(function () {
                    if (this.disabled) return;
                    this.checked = false;
                });
            }
        }
        function IsRowSelected(args) {
            if ($("#" + '<%=gridPointFields.ClientID %>' + " tr td :checkbox[checked=true]").length == 0) {
                ShowMsg("noRowSelected");
            }
            else {
                // give confirm info
                if ("disable" == args) {
                    ShowMsg("disableCfm");
                }
                else if ("delete" == args) {
                    ShowMsg("deleteCfm");
                }
            }
            return false;
        }

       
      
    </script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSelect">
    <asp:Button ID="btnSelect"  runat="server" Text="Filter" class="Btn-66" OnClick="btnSelect_Click" />
    <asp:Button ID="btnCancel"  runat="server" Text="Filter" class="Btn-66"  />
    <div id="div1" class="ColorGrid" style="margin-top: 5px;">
        <asp:GridView ID="gridPointFields" runat="server" DataKeyNames="NameId,Name" EmptyDataText="There is no record in database."
            AutoGenerateColumns="False" OnRowDataBound="gridPointFields_RowDataBound" CellPadding="3" CssClass="GrayGrid" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="Label" HeaderText="Point Field Label" />
                <asp:BoundField DataField="Heading" HeaderText="Pipeline Column Heading" />
            </Columns>
            <SelectedRowStyle BackColor="#E4E7EF" />
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    </form>
</body>
</html>
