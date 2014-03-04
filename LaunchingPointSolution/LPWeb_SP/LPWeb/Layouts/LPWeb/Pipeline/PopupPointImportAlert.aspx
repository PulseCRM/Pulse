<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupPointImportAlert.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Pipeline.PopupPointImportAlert"%>

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
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <style>
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
        $().ready(function () {
            $(document).bind("keydown.prekey8", function () {
                if (event && event.keyCode == 8) {
                    event.keyCode = 0;
                    event.returnValue = false;
                }
            });
            $("#BtnDelete").click(DeleteHandller);
            $("#BtnClose").click(CloseCurrentWindowHandller);
        });

        function DeleteHandller() {
            $("#divConfirm").dialog({
                height: 130,
                width: 330,
                modal: false,
                title: "",
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseCurrentWindowHandller() {
            if (parent != null) {
                parent.closeImportErrorDialog();
            }
            return false;
        }

        function DeleteRecord() {
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);
            var hisId = $("#<%= hfdHisId.ClientID %>").val();
            $.getJSON("PopupPointImportAlertDelete_Background.aspx?sid=" + Radom + "&fileId=" + hisId, AfterTaskDelete);
        }

        function AfterTaskDelete(data) {

            if (data.ExecResult == "Failed") {
                alert(data.ErrorMsg);
                $("#divConfirm").dialog("close");
                return;
            }
            alert("Delete successful");
            window.parent.location.href = window.parent.location.href;
            $("#divConfirm").dialog("close");
            parent.closeImportErrorDialog(true);
        }

        // show popup fo

        function CloseTheConfirmWindow() {
            $("#divConfirm").dialog("close");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfdHisId" runat="server" />
    <div id="divDetail">
        <table  class="detail">
            <tr>
                <td width="20px">
                    <img id="imgIcon" runat="server" alt="" />
                </td>
                <td colspan="2">
                    Point File:
                    <asp:Label ID="lblPointFile" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="padding-right: 80px;">
                    Borrower:
                    <asp:Label ID="lblBorrower" runat="server"></asp:Label>
                </td>
                <td>
                     Loan Officer:
                    <asp:Label ID="lblLoanOfficer" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Time:
                    <asp:Label ID="lblTime" runat="server"></asp:Label>
                </td>
                <td>
                  
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Error Message:
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="tbxErrorMessages" runat="server" width="552px" TextMode="MultiLine" Rows="10" Columns="90" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div id="divButtons" class="opt">
        <input id="BtnDelete" type="button" value="Delete" class="Btn-91"/>
        <input id="BtnClose" type="button" value="Close" class="Btn-91"/>
    </div>
    <div id="divConfirm" style="display:none">
        This operation is not reversible. Are you sure you want to delete this record?
        <br />
        <div style="text-align: center;">
            <input id="btnOK" type="button" value="Yes" class="Btn-66" onclick="return DeleteRecord()" />&nbsp;&nbsp;
            <input id="btnCancel" type="button" value="No" class="Btn-66" onclick="return CloseTheConfirmWindow();" />
        </div>
    </div>
    </form>
</body>
</html>
