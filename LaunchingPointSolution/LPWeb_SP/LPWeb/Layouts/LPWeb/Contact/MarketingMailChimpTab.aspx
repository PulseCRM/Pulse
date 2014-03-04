<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingMailChimpTab.aspx.cs"
    Inherits="WebApplication3.forms.MarketingMailChimpTab" %>

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

    <script type="text/javascript">

        function BeforeSave() {

            if ($("#<%= txbMCKey.ClientID %>").val() = "") {

                return false;

            }

            return true;
        }

        $(document).ready(function() {

            $("#<%= cbEnableMailChimp.ClientID %>").click(function() {

                if ($(this).attr("checked") == true) {

                    $("#<%= txbMCKey.ClientID %>").removeAttr("disabled");
                }
                else {
                    $("#<%= txbMCKey.ClientID %>").attr("disabled", true);
                }
                
            });

        });
        
 
    </script>

    <style type="text/css">
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-left: 15px; padding-top: 15px;">
        <table>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="cbEnableMailChimp" Text=" Enable Mail Chimp" runat="server" />
                </td>
            </tr>
            <tr style="padding-top: 10px;">
                <td style="width: 140px;">
                    <asp:Label ID="Label1" runat="server" Text="Mail Chimp API Key:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txbMCKey" runat="server" Width="420px" MaxLength="255" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px;" colspan="2">
                    <asp:Button ID="btnSave" runat="server" CssClass="Btn-66" OnClientClick="return BeforeSave();"
                        Text="Save" OnClick="btnSave_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSync" runat="server" CssClass="Btn-91" Text="Sync Now" OnClick="btnSync_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
