<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="Pipeline_NoteList" CodeBehind="NoteList.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="padding-bottom: 12px; padding-top: 6px;">
            <input id="Button1" type="button" value="Add" class="Btn-66" />
        </div>
        <div style="border-bottom: solid 2px #e4e7ef; margin-top: 5px;">
        </div>
        <table>
            <tr style="height: 20px;">
                <td align="right" style="border-right: 1px solid #e4e7ef; padding-right: 10px;">
                    Datetime
                </td>
                <td align="left" style="border-right: 1px solid #e4e7ef; padding-left: 10px;">
                    Sender
                </td>
                <td align="left" style="padding-left: 10px;">
                    Subject
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div class="DashedBorder">
                        &nbsp;</div>
                </td>
            </tr>
            <tr style="font-weight: bolder;">
                <td width="180px" align="right" style="padding-right: 10px; font-weight: bolder;">
                    04-16-2010 10:25:45AM
                </td>
                <td width="135px" style="padding-left: 10px; font-weight: bolder;">
                    Frank Smith
                </td>
                <td width="455" style="padding-left: 10px; font-weight: bolder;">
                    Follow up on the rate change
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    Please call the borrower to inform him of the rate change today.
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div class="DashedBorder">
                        &nbsp;</div>
                </td>
            </tr>
            <tr style="font-weight: bolder;">
                <td width="180px" align="right" style="padding-right: 10px; font-weight: bolder;">
                    04-16-2010 12:25:45AM
                </td>
                <td width="135px" style="padding-left: 10px; font-weight: bolder;">
                    Jennifer Wilson
                </td>
                <td width="455" style="padding-left: 10px; font-weight: bolder;">
                    Rate change
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    Frank, Please check the rate change to make sure we're still in the range
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div class="DashedBorder">
                        &nbsp;</div>
                </td>
            </tr>
            <tr style="font-weight: bolder;">
                <td width="180px" align="right" style="padding-right: 10px; font-weight: bolder;">
                    04-16-2010 10:25:45AM
                </td>
                <td width="135px" style="padding-left: 10px; font-weight: bolder;">
                    Frank Smith
                </td>
                <td width="455" style="padding-left: 10px; font-weight: bolder;">
                    Follow up on the rate change
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    Frank, Please check the rate change to make sure we're still in the range
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div class="DashedBorder">
                        &nbsp;</div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
