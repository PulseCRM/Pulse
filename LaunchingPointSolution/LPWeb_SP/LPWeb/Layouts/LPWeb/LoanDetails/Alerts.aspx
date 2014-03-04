<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="LoanDetails_Alerts" Codebehind="Alerts.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="700px">
            <tr>
                <td>
                    <asp:Image ID="Image1" ImageUrl="../images/loan/Task-Overdue.gif" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="CheckBox3" runat="server" Text="  Notify borrower of the rate change 06/12/2010" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Image ID="Image2" ImageUrl="../images/loan/Task-DueIn3Days.gif" runat="server" />
                </td>
                <td>
                    <asp:CheckBox ID="CheckBox4" runat="server" Text="  Send GFE pagework to the borrower 06/17/2010" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
