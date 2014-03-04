<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="LoanDetails_Contacts" Codebehind="Contacts.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css" runat="server" />
    <style>
        .opt
        {
        }
        .opt input
        {
            margin-left: 8px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                        <tr>
                            <td style="width: 600px;">
                                <ul class="ToolStrip" style="margin-left: 0px;">
                                    <li><a id="aNew" href="#">New</a><span>|</span></li>
                                    <li><a id="aSave" href="#">Save</a><span>|</span></li>
                                    <li><a id="aRemove" href="#">Remove</a><span>|</span></li>
                                    <li><a id="aReassign" href="#">Reassign</a><span>|</span></li>
                                    <li><a id="aSend Email" href="#">Send Email</a><span>|</span></li>
                                    <li><a id="aSend Login" href="#">Send Login</a></li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
             <tr>
                <td>
                    <div id="divUserList" class="Widget" style="width: 760px; margin-top: 5px;">
                        <div class="Widget_Grid_Header">
                            <div>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 22px; text-align: center;">
                                            <input id="Checkbox1" type="checkbox" />
                                        </td>
                                        <td style="width: 80px;">
                                            Type
                                        </td>
                                        <td style="width: 80px;">
                                            Name
                                        </td>
                                        <td style="width: 80px;">
                                            Company
                                        </td>
                                        <td style="width: 85px;">
                                            Email Address
                                        </td>
                                        <td style="width: 80px;">
                                            Phone
                                        </td>
                                        <td style="width: 80px;">
                                            Cell
                                        </td>
                                        <td style="width: 80px;">
                                            Fax
                                        </td>
                                        <td style="width: 80px;">
                                            Address
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="Widget_Body">
                            <div class="GridRow24_Odd" style="">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox2" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                            <div class="GridRow24_Even" style="">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox3" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                            <div class="GridRow24_Odd" style="">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox4" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                            <div class="GridRow24_Even" style="">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox5" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                            <div class="GridRow24_Odd" style="">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox6" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                            <div class="GridRow24_Even" style="">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox7" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                            <div class="GridRow24_Odd" style="">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox8" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                            <div class="GridRow24_Even" style="">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 23px; border-right: solid 1px #e4e7ef; text-align: center;">
                                            <input id="Checkbox9" type="checkbox" />
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 80px; border-right: solid 1px #e4e7ef;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <div class="DashedBorder">
                                    &nbsp;</div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
