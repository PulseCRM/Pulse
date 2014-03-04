<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskReminderSnooze.aspx.cs" Inherits="LPWeb_TaskReminderSnooze" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Snooze Task</title>
    <link href="css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="css/style.custom.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/urlparser.js" type="text/javascript"></script>
    <script src="js/jquery.only_press_num.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {

            $("#txtMinutes").OnlyInt();

        });

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        <br />
        

        <div style="text-align: center;">
            Snooze <asp:TextBox ID="txtMinutes" runat="server" Text="5" Width="50px" MaxLength="2" style="text-align:right;"></asp:TextBox> minutes
            <br />
            <br />
            <asp:Button ID="btnSnooze" runat="server" Text="Snooze" OnClick="btnSnooze_Click" CssClass="Btn-66" />
            <input id="btnCancel" type="button" value="Cancel" onclick="btnCancel_onclick()" class="Btn-66" />
        </div>

    </div>
    </form>
</body>
</html>