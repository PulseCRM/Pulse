<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyMarketingAdd.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Pipeline.CompanyMarketingAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rate Lock Alert Detail</title>
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
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
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
            margin-top:20px;
        }
        .detail td
        {
            line-height:20px;
            white-space:nowrap;
        }
    </style>
    <script type="text/javascript">
      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfdFileId" runat="server" />
    <asp:HiddenField ID="hfdExpDate" runat="server" />
    
    <div id="divDetail">
        <table class="detail">
            <tr>
                <td>
                   
                </td>
                <td>
                   
                </td>
                <td>
                   
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                  
                </td>
                <td>
                  
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                   
                </td>
                <td>
                   
                </td>
            </tr>
           
        </table>
    </div>
    <br />
    <div id="divButtons" class="opt">
        <input id="BtnExtendRateLock" type="button" value="Extend Ratelock" onclick="OpenExtendRateLockWindow();" class="Btn-91"/>
        <asp:Button ID="BtnSendEmail" runat="server" Text="Send Email" class="Btn-91" OnClientClick="WindowOpen_SendEmail(); return false;"/>
        <input id="BtnClose" type="button" value="Close" class="Btn-91"/>
    </div>
    
    </form>
</body>
</html>
