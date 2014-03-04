﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PointFolderSelectionForDetails.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Pipeline.PointFolderSelectionForDetails" %>

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
    <title>Point Folder Selection</title>
    <script type="text/javascript">
        function ckbClicked(me)
        {
            var bCheck = $(me).attr('checked');
            if (bCheck)
            {
                $('input:checkbox', $('#' + '<%=gvFolder.ClientID %>')).each(function() { 
                    if (this != me)
                        $(this).attr('checked', false);
                });
            }
        }

        // call back
        function callBack(sReturn) {

            alert("callback11");
            window.parent.CloseGlobalPopup();
        }

        function Cancel() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);

        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table style="margin-top: 5px;">
            <tr>
                <td>
                    <asp:Button ID="btnSel" runat="server" Text="Select" CssClass="Btn-91" OnClick="btnSel_Click" />
                </td>
                 <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-91" OnClientClick="Cancel(); return false;" />
                </td>
            </tr>
        </table>
    <div style="margin-top: 5px; height:360px; OVERFLOW-Y: auto; OVERFLOW-X:hidden;" >
    <div id="div1" class="ColorGrid" style=" width: 460px;">
        <asp:GridView ID="gvFolder" runat="server" DataKeyNames="FolderId,Path" EmptyDataText="There is no folder found in database."
            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" onclick="ckbClicked(this)" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Path" HeaderText="Path" />
                <asp:BoundField DataField="StatusName" HeaderText="Loan Status" />
            </Columns>
            <SelectedRowStyle BackColor="#E4E7EF" />
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
   </div>
    </form>
</body>
</html>

