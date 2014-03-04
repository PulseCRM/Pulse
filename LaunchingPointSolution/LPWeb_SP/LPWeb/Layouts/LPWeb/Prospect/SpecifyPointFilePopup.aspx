<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecifyPointFilePopup.aspx.cs" Inherits="SpecifyPointFilePopup"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <title>Specify Point File Popup</title>
    <script type="text/javascript">


        // call back
        function callBack(sReturn) {
            //            if(window.parent && window.parent.getFolderSelectionReturn)
            //                window.parent.getFolderSelectionReturn(sReturn);
            Cancel()
        }

        function Cancel() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);

        }


    </script>
</head>
<body>
    <form id="form1" runat="server" style=" margin:10px;">
    <label id=""> <b>Specify Point File</b></label>
    <div style=" padding-left:10px;">
   <table style="margin-top: 5px;">
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-91" OnClick="btnSave_Click" />
                </td>
                 <td>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-91" OnClientClick="Cancel(); return false;" />
                </td>
            </tr>
   </table>
   <table style="margin-top: 5px;">
            <tr>
                <td>
                    Folder:
                </td>
                 <td>
                     <asp:DropDownList ID="ddlFolder" runat="server"  Width="210">
                     </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Filename:
                </td>
                 <td>
                     <asp:TextBox ID="txbFilename" runat="server" Width="200"></asp:TextBox>
                </td>
            </tr>
   </table>
   </div>
    </form>
</body>
</html>