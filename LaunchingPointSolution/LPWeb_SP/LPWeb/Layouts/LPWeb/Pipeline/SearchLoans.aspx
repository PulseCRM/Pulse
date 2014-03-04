<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchLoans.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Pipeline.SearchLoans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <title>Search Loans</title>
    <script type="text/javascript">
        // call back
        function callBack(sReturn)
        {
            if (window.parent && window.parent.getSearchFilterReturn)
                window.parent.getSearchFilterReturn(sReturn);
        }

        function cancelBtnClick() {
            if (window.parent && window.parent.closeSearchWin)
                window.parent.closeSearchWin();
        }
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="aspnetForm">
        <div class="Heading">
            Search Loans</div>
        <div class="SplitLine">
        </div>
        <div class="DetailsContainer">
            <div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="padding-top: 9px;">
                            Borrower<br /> Last Name
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:TextBox ID="tbLastName" runat="server" class="iTextBox" Style="width: 167px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="padding-left: 39px; padding-top: 9px; white-space: nowrap;">
                            Borrower<br /> First Name
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="3">
                            <asp:TextBox ID="tbFirstName" runat="server" class="iTextBox" Style="width: 167px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trStatus" runat="server">
                        <td style="padding-top: 9px; white-space: nowrap;">
                           <asp:Label ID="lbStatus" Text="Prospect" runat="server" ></asp:Label><br /> Loan Status
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="5">
                            <asp:DropDownList ID="ddlLoanStatus" runat="server">
                              
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px;">
                            Property<br /> Address
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="5">
                            <asp:TextBox ID="tbProAddr" runat="server" class="iTextBox" Style="width: 450px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 9px;">
                            City
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:TextBox ID="tbCity" runat="server" class="iTextBox" Style="width: 100px;" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="padding-left: 39px; padding-top: 9px;">
                            State
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:DropDownList ID="ddlState" runat="server">
                                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                                <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                                <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                                <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                                <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                                <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                                <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                                <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                                <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                                <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                                <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                                <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                                <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                                <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                                <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                                <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                                <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                                <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                                <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                                <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                                <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                                <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                                <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                                <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                                <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                                <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                                <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                                <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                                <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                                <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                                <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                                <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                                <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                                <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                                <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                                <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                                <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                                <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                                <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                                <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                                <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                                <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                                <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                                <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                                <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                                <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                                <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 39px; padding-top: 9px;">
                            Zip
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;">
                            <asp:TextBox ID="tbZip" runat="server" class="iTextBox" Style="width: 55px;" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" class="Btn-66" OnClick="btnSearch_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="Btn-66" OnClientClick="cancelBtnClick(); return false;" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
