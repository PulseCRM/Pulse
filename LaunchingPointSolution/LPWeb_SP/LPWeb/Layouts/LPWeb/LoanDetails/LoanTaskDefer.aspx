<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanTaskDefer.aspx.cs" Inherits="LoanDetails_LoanTaskDefer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Defer Loan Task</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            AddValidators();

            $("#txtDeferDays").onlypressnum();
        });

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {
                    txtDeferDays: {
                        required: true
                    }
                },
                messages: {
                    txtDeferDays: {
                        required: "*"
                    }
                }
            });
        }

        // cancel
        function btnCancel_onclick() {

            window.parent.CloseDialog_DeferTask();
        }

        function BeforeSave() {


            var t = document.getElementById("<%=txtDeferDays.ClientID%>").value;
                //alert(t);

                if (t != null && typeof (t) != "undefined" && t != "") {
                    if (/^[0-9]*[1-9][0-9]*$/.test(t)) {//这个正则表达式为整数                         
                        if (t < 1 || t >= 100) {
                            alert("Enter 1 to 99 between!");

                            return false;
                        }
                    }
                    else {
                        alert("Defer Days is a positive integer!");

                        return false;
                    }
                }             

            return true;

        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 270px;">
        <div class="DetailsContainer">
            <div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 130px;">
                            Number of days to defer:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDeferDays" runat="server" Width="50px" MaxLength="2"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" onclick="btnSave_Click" OnClientClick="return BeforeSave();" />
                        </td>
                        <td style="padding-left: 8px;">
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" 
                                </td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>