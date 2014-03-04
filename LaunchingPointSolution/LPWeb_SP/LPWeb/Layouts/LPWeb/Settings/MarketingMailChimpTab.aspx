<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingMailChimpTab.aspx.cs"
    Inherits="MarketingMailChimpTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mail Chimp Tab</title>
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
    
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            $("#<%= cbEnableMailChimp.ClientID %>").click(function () {

                if ($(this).attr("checked") == true) {

                    $("#<%= txbMCKey.ClientID %>").removeAttr("disabled");
                     
                }
                else { 
                    $("#<%= txbMCKey.ClientID %>").attr("disabled", "disabled");
                }

            });

        });

        function BeforeSaveMailChimp() {

            if ($("#<%= cbEnableMailChimp.ClientID %>").attr("checked") == true && $.trim($("#<%= txbMCKey.ClientID %>").val()) == "") {
                alert("Please enter the Mail Chimp API Key.");
                return false;

            }

            return true;
        }


        function MailChimpSyncNow() {
         
            var BranchID = GetQueryString1("BranchID"); 

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.parent.ShowWaitingDialog("Please wait...");

            // check exist
            $.getJSON("MailChimp_Sync_Ajax.aspx?sid=" + Radom + "&BranchID=" + BranchID, AfterMailChimpSync);

        }

        function AfterMailChimpSync(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Sync successfully.");

                    window.parent.CloseWaitingDialog();
                }
                window.parent.CloseWaitingDialog();

            }, 2000);
        }
         

    </script>
    <style type="text/css">
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divMailChimpTab" style="padding-left: 15px; padding-top: 15px;">
        <table>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="cbEnableMailChimp" Text=" Enable Mail Chimp" runat="server" />
                </td>
            </tr>
            <tr style="padding-top: 10px;">
                <td style="width: 140px;">
                    <asp:Label ID="Label6" runat="server" Text="Mail Chimp API Key:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txbMCKey" runat="server" Width="420px" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px;" colspan="2">
                    <asp:Button ID="btnSaveMailChimp" runat="server" CssClass="Btn-66" OnClientClick="return BeforeSaveMailChimp();"
                        Text="Save" OnClick="btnSaveMailChimp_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <input id="btnSync" runat="server" type="button" class="Btn-91" value="Sync Now"
                        onclick=" MailChimpSyncNow();return false;" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
