<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchContactsPopup.aspx.cs"
    Inherits="SearchContactsPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ddlContactTypeClientID = "#<%=ddlContactType.ClientID %>";

        $(document).ready(function () {

            // add onchange event 
            $(ddlContactTypeClientID).change(ddlContactType_onchange);

        });

        function ddlContactType_onchange() {

            var ddlContactTypeSelectValue = $(ddlContactTypeClientID).val();

            var ServiceType = $("#<%=ddlServiceType.ClientID %>");
            if (ddlContactTypeSelectValue == "Client") {
                ServiceType.attr("disabled", true);
                $("#<%=txbCompany.ClientID %>").val("");
                $("#<%=txbCompany.ClientID %>").attr("disabled", true);
                $("#<%=txbBranch.ClientID %>").val("");
                $("#<%=txbBranch.ClientID %>").attr("disabled", true);
            }
            else {
                ServiceType.attr("disabled", false);
                ServiceType[0].selectedIndex = 0;
                $("#<%=txbCompany.ClientID %>").attr("disabled", false);
                $("#<%=txbBranch.ClientID %>").attr("disabled", false);
            };

        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr style="height: 35px">
                <td>
                    Contact Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlContactType" runat="server" Width="203px">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Client</asp:ListItem>
                        <asp:ListItem>Partner</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 30px;">
                </td>
                <td>
                    Service Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlServiceType" runat="server" Width="200px" DataTextField="Name"
                        DataValueField="ServiceTypeId">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="height: 35px">
                <td>
                    Company:
                </td>
                <td>
                    <asp:TextBox ID="txbCompany" Width="200px" runat="server"></asp:TextBox>
                </td>
                <td style="width: 30px;">
                </td>
                <td>
                    Branch:
                </td>
                <td>
                    <asp:TextBox ID="txbBranch" Width="200px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="height: 35px">
                <td>
                    Last Name:
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txbLastName" Width="200px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="height: 35px">
                <td>
                    Address:
                </td>
                <td>
                    <asp:TextBox ID="txbAddress" Width="200px" runat="server"></asp:TextBox>
                </td>
                <td style="width: 30px;">
                </td>
                <td>
                    City:
                </td>
                <td>
                    <asp:TextBox ID="txbCity" Width="100px" runat="server"></asp:TextBox>&nbsp;
                    <asp:Label ID="Label1" runat="server" Text="State:"></asp:Label>&nbsp;
                    <asp:DropDownList ID="ddlState" runat="server" Width="60px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="height: 35px">
                <td colspan="5">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" class="Btn-66" OnClick="btnSearch_Click" />
                    &nbsp;<input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
