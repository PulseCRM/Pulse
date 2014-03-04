<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReassignLoan.aspx.cs" Inherits="ReassignLoan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css"
        runat="server" />
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
    <script type="text/javascript">

        // cancel
        function btnCancel_onclick() {
            var CloseDialogCodes = $("#hdnCloseDialogCodes").val();
            if (CloseDialogCodes == "") {
                window.parent.DialogLoanAssignClose();
                return;
            }

            eval(CloseDialogCodes);
        }

        //Save click
        function btnSave_OnClick() {
            var UserID = $("#<%=ddlUsers.ClientID %>").val();
            if (UserID == null || UserID == 0 || UserID == "") {

                alert("Please select an user.");
                return false;
            }

            return true;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 580px;">
        <div>
            <div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Borrower:
                        </td>
                        <td style="padding-left: 15px; width: 150px;">
                            <asp:Label ID="lbBorrower" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            Property:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:Label ID="lbProperty" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 15px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Role:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlRole" runat="server" DataValueField="RoleId" DataTextField="Name"
                                Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            User:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlUsers" runat="server" DataValueField="UserID" DataTextField="Username"
                                Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return btnSave_OnClick();" OnClick="btnSave_Click"/>
                        </td>
                        <td style="padding-left: 8px; display: none;">
                            &nbsp;
                        </td>
                        <td style="padding-left: 8px;">
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <input id="hdnEstCloseDate" runat="server" type="text" style="display: none;" />
        <input id="hdnNow" runat="server" type="text" style="display: none;" />
        
        <asp:HiddenField ID="hdnCloseDialogCodes" runat="server" />
    </div>
    </form>
</body>
</html>
