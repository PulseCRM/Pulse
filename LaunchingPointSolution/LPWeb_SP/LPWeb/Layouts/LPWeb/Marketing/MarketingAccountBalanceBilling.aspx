<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="LPWeb.Layouts.LPWeb.Marketing.Billing" CodeBehind="MarketingAccountBalanceBilling.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> 
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />

    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
     
    <script src="../js/jquery.datepick.js" type="text/javascript"></script> 
    <script src="../js/date.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        var ddlStartedBy = "";

        $(document).ready(function () {

            $(".DateField").datepick();

        });

        function GoToEvents(CampaignId) {

            window.parent.SetTab('MarketingActivitiesEvents.aspx', 1, CampaignId);

        }

        function GoToLoanDetails(LoanID) {
            window.parent.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=" + encodeURIComponent(window.location.href) + "&fieldid=" + LoanID + "&fieldids=" + LoanID;
        }
// ]]>
    </script>
    <style type="text/css">
        #card tr
        {
            height:40px;
            
        }
        #card td 
        {
            padding:4px;
        }
        #card .left
        {
            width: 400px; text-align:left; padding-left:10px;
        }
        #card .right
        {
            width: 210px; text-align:right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" >
    <div style="text-align:center;">
        <asp:HiddenField ID="hfCardID" runat="server" />
                <table cellpadding="0" cellspacing="0" id="card" >
                    <tr>
                        <td class="right">
                            Card Type:
                        </td>
                        <td class="left">
                            <asp:DropDownList ID="ddlCardType" runat="server" Width="120px">
                            <asp:ListItem Value="" >-select card type-</asp:ListItem>
                            <asp:ListItem Value="0">VISA</asp:ListItem>
                            <asp:ListItem Value="1">MasterCard</asp:ListItem>
                            <asp:ListItem Value="2">Amex</asp:ListItem>
                            <asp:ListItem Value="3">Discover</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                            Card Number:
                        </td>
                        <td class="left">
                            <asp:TextBox ID="txbCardNumber" runat="server" Width="170"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                           Name on Card:
                        </td>
                        <td class="left">
                            <asp:TextBox ID="txbNameonCard" runat="server" Width="170"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                           Card Expiration (mm/yy):
                        </td>
                        <td class="left">
                            <asp:TextBox ID="txbCardExpiration" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                           Card CSC code:
                        </td>
                        <td class="left">
                            <asp:TextBox ID="txbCardCSCcode" runat="server" Width="40"></asp:TextBox>
                           
                            <span style="margin-left:35px; padding-top:5px;"> <asp:Button ID="btnUpdateCard" runat="server" Text="Update Card" OnClick="btnUpdateCard_Click" CssClass="Btn-91"/></span>
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                    <td></td>
                    </tr>
                    <tr>
                        <td class="right">
                           Balance:
                        </td>
                        <td class="left" style=" padding-left:2px;">
                            $ <asp:TextBox ID="txbBalance" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="right">
                           $
                        </td>
                        <td class="left">
                            <asp:TextBox ID="txbAddBalance" runat="server"></asp:TextBox>
                            <span style="margin-left:10px;"> <asp:Button ID="btnAddBalance" runat="server" Text="Add Balance" OnClick="btnAddBalance_Click"  CssClass="Btn-91" /></span>
                        </td>
                    </tr>
                </table>

        </div>   
    </form>
</body>
</html>