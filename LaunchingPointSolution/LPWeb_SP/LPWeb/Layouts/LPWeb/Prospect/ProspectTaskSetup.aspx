<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectTaskSetup.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Prospect.ProspectTaskSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <title>Client Task Setup</title>
    <script type="text/javascript">
        $(document).ready(function () {
            var tbDue = $("#" + '<%=tbDue.ClientID %>');
            tbDue.datepick();
            tbDue.attr("readonly", "true");
        });

        function closeBox(isRefresh, bReset) {
            if (bReset === false)
                bReset = false;
            else
                bReset = true;
            self.parent.closeProspectTaskSetupWin(isRefresh, bReset);
            return false;
        }

        function previewEmailTemplate(ddlId) {
            var etId = $("#" + ddlId).val();
            if ("0" == etId) {
                alert("No email template has been selected.");
            }
            else {
                OpenWindow("../Settings/EmailTemplatePreview.aspx?id=" + etId, "_PreviewEmailTemplate3", 760, 700, "no", "center");
            }
            return false;
        }
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="aspnetForm">
        <div class="DetailsContainer">
            <div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="white-space: nowrap;">
                            Client
                        </td>
                        <td style="padding-left: 15px;" colspan="3">
                            <asp:Label ID="lblClient" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap; padding-top: 9px;">
                            Task Name
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:TextBox ID="tbTaskName" runat="server" class="iTextBox" Width="300px" MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbTaskName"
                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2" style="padding-left: 39px; padding-top: 9px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        Enabled
                                    </td>
                                    <td style="padding-left: 8px;">
                                        <asp:CheckBox ID="ckbEnabled" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px;">
                            Description
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="3">
                            <asp:TextBox ID="tbDesc" runat="server" TextMode="MultiLine" Height="44px" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px; white-space: nowrap;">
                            Owner
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:DropDownList ID="ddlOwner" runat="server" Width="310px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 39px; padding-top: 9px;">
                            Due Date
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:TextBox ID="tbDue" runat="server" class="DateField"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px; white-space: nowrap;">
                            Warning Email
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="3">
                            <asp:DropDownList ID="ddlWarningEmail" runat="server" Width="310px">
                            </asp:DropDownList>
                            <asp:Button ID="btnPreWarn" runat="server" Text="Preview" CssClass="Btn-66" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px; white-space: nowrap;">
                            Overdue Email
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="3">
                            <asp:DropDownList ID="ddlOverdueEmail" runat="server" Width="310px">
                            </asp:DropDownList>
                            <asp:Button ID="btnPreOverdue" runat="server" Text="Preview" CssClass="Btn-66" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px; white-space: nowrap;">
                            Completion Email
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="3">
                            <asp:DropDownList ID="ddlComleEmail" runat="server" Width="310px">
                            </asp:DropDownList>
                            <asp:Button ID="btnPreComple" runat="server" Text="Preview" CssClass="Btn-66" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnClone" runat="server" Text="Clone" class="Btn-66" OnClick="btnClone_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" class="Btn-66" OnClientClick="return confirm('Are you sure you want to delete this task?');"
                                OnClick="btnDelete_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="Btn-66" OnClientClick="return closeBox(false, false);" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
