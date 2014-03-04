<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupAlertDetailConfirm.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Pipeline.PopupAlertDetailConfirm" %>

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
</head>
<body>
    <form id="form1" runat="server">
    <div id="divExtendRateLock">
        <table>
            <tr>
                <td>
                    Number of days to extend the rate lock:
                </td>
                <td>
                    <asp:TextBox ID="tbxExtendDays" runat="server" width="45px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div style="text-align: center;">
         <asp:Button ID="btnCreate" CssClass="Btn-66" runat="server" Text="Extend" OnClick="btnCreate_Click"/>&nbsp;&nbsp;
            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="parent.CloseTheUpdateExtendWindow();" />
        </div>
        <div id="divMsg" style="margin-top: 10px; text-align: center; display: none;">
            <img id="imgWaiting" src='../images/waiting.gif' style='position: relative; top: 2px;' />
            <label id="lbMsg" style='font-weight: bold;'>
                Please wait...</label>
        </div>
    </div>
    </form>
</body>
</html>
