<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectNoteAdd.aspx.cs" Inherits="LPWeb.Prospect.ProspectNoteAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.thickbox.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <style>
        div.opt
        {
            margin-top: 20px;
        }
        div.opt input
        {
            margin-right: 10px;
        }
        form
        {
            padding-left: 10px;
        }
        .detail
        {
            margin-top: 20px;
        }
        .detail td
        {
            line-height: 20px;
            white-space: nowrap;
        }
    </style>
    <script type="text/javascript">
        $().ready(function () {
            $("#btnCancel").click(CloseCurrentWindow);
        });

        function CloseCurrentWindow() {
            parent.ClosePopupWindow();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divDetail">
        <table class="detail">
            <tr>
                <td style="padding-right: 80px;">
                    Client:
                    <asp:Label ID="lblBorrower" runat="server"></asp:Label>
                </td>
                <td>
                    Address:
                    <asp:Label ID="lblProperty" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Sender:
                    <asp:Label ID="lblSender" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Note:
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="tbxNote" MaxLength="500" runat="server" TextMode="MultiLine" Rows="5"
                        Columns="55"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hfdContactId" runat="server" />
    <br />
    <div id="divButtons" class="opt">
<%--        <input id="btnSave" type="button" value="Save" class="Btn-91" />--%>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-91" OnClick="btnSave_Click"/>
        <input id="btnCancel" type="button" value="Cancel" class="Btn-91" onclick="parent.ClosePopupWindowDirectly();" />
    </div>
    </form>
</body>
</html>
