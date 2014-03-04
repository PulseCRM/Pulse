<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectTaskEmailSendCompletionPopup.aspx.cs" Inherits="Prospect_ProspectTaskEmailSendCompletionPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Send Completion Email</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#gridRecipientList tr td:contains('No emaill address found for the selected recipient(s).')").css("color", "red");
        });

        function btnPreview_onclick() {

            var EmailTemplateID = GetQueryString1("EmailTemplateID");
            var LoanID = GetQueryString1("prospectID");

            OpenWindow("../LoanDetails/EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&ProspectID=" + LoanID + "&AppendMyPic=0", "_Preview7", 760, 580, "no", "center");

            return true;
        }

        function BeforeSend() {

            var Sender = $("#lbSender").text();
            if (Sender == "There is no sender.") {

                alert("Could not find the From email address.");
                return false;
            }

            if ($("#gridRecipientList tr").length == 1) {

                alert("No emaill address found for the selected recipient(s).");
                return false;
            }

            if ($("#gridRecipientList tr td:contains('There is no emaill address')").length == $("#gridRecipientList tr").length - 1) {

                alert("No emaill address found for the selected recipient(s).");
                return false;
            }

            var ToEmails = "";
            $("#gridRecipientList tr").each(function () {

                var Email = $(this).text();
                if (ToEmails == "") {

                    ToEmails = Email;
                }
                else {

                    ToEmails += "$" + Email;
                }
            });

            $("#txtToEmails").val(ToEmails);

            return true;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 600px; margin-top: 10px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 90px;">Email Template:</td>
                <td>
                    <asp:Label ID="lbEmailTemplate" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
            <tr>
                <td style="width: 90px;">Sender:</td>
                <td>
                    <asp:Label ID="lbSender" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
        <div style="margin-top: 15px;">Recipients:</div>
        <div id="divRecipientList" class="ColorGrid" style="margin-top: 3px; width: 590px;">
            
            <asp:GridView ID="gridRecipientList" runat="server" EmptyDataText="There is no recipient." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:BoundField DataField="RecipientType" HeaderText="Type" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="FullName" HeaderText="Name" ItemStyle-Width="170px" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="Btn-66" OnClientClick="return BeforeSend()" onclick="btnSend_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnPreview" type="button" value="Preview" class="Btn-66" onclick="btnPreview_onclick()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="window.parent.CloseDialog_SendCompletionEmail();" />
                    </td>
                </tr>
            </table>
        </div>
        
    </div>
    </form>
</body>
</html>
