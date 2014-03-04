<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanConditionAddNote.aspx.cs" Inherits="LoanDetails_LoanConditionAddNote" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Add Condition Note</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#txtNote").maxlength(4000);
        });

        function btnCancel_onclick() {

            window.top.CloseGlobalPopup();
        }

        function BeforeSave() {

            var Note = $.trim($("#txtNote").val());
            if (Note == "") {

                alert("Please enter Note.");
                return false;
            }

            return true;
        }
// ]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        <div class="margin-top-10">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" OnClick="btnSave_Click" />
            &nbsp;&nbsp;<input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
        </div>
        <div class="margin-top-10">
            <table cellpadding="5" cellspacing="5">
                <tr>
                    <td style="width: 250px;">Borrower: <asp:Label ID="lbBorrower" runat="server" Text="Label"></asp:Label></td>
                    <td style="width: 450px;">Property Address: <asp:Label ID="lbPropertyAddress" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td>Point Folder: <asp:Label ID="lbPointFolder" runat="server" Text="Label"></asp:Label></td>
                    <td>Point filename: <asp:Label ID="lbPointfilename" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">Condition Name: <asp:Label ID="lbConditionName" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkExternalViewingEnabled" runat="server" Text=" External Viewing Enabled" />
                    </td>
                </tr>
            </table>
            
            <table cellpadding="5" cellspacing="5">
                <tr>
                    <td style="vertical-align: top; padding-top: 10px; width: 50px;">Note:</td>
                    <td>
                        <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Width="600px" Height="120px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>