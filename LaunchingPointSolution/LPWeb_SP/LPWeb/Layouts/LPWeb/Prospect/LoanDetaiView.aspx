<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetaiView.aspx.cs"
    Inherits="LoanDetaiView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css"
        runat="server" /> 
    <script src="../js/jquery.js" type="text/javascript"></script>  
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[


        // cancel
        function btnCancel_onclick() { 
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server" style="width: 720px;">
    <div>
        <div id="divModuleName" class="ModuleTitle">
            Loan Detail</div>
        <asp:HiddenField ID="hfFileID" runat="server" />
        <table style="margin-left: 20px;">
            <tr>
                <td>
                    Loan Officer :
                </td>
                <td colspan="4">
                    <asp:Label ID="ddlLoanOfficer" runat="server">
                    </asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Ranking :
                </td>
                <td>
                    <asp:Label ID="ddlRanking" runat="server" Width="141px"> 
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Borrower :
                </td>
                <td colspan="4">
                    <asp:Label ID="ddlBorrower" runat="server" Width="235px">
                    </asp:Label>
                </td>
                <td>
                </td>
                <td>
                    CoBorrower :
                </td>
                <td>
                    <asp:Label ID="ddlCoBorrower" runat="server" Width="235px">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Amount :
                </td>
                <td colspan="4">
                    <asp:Label ID="txbAmount" runat="server" Width="110px"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Interest Rate :
                </td>
                <td>
                    <asp:Label ID="txbInterestRate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Estimated Close Date:
                </td>
                <td colspan="4">
                    <asp:Label ID="txbEstimatedDate" runat="server" Width="110px" MaxLength="10"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Loan Program:
                </td>
                <td>
                    <asp:Label ID="ddlLoanProgram" runat="server" Width="235px">
                    </asp:Label>
                </td>
            </tr>
                       <tr>
                <td>
                    Purpose:
                </td>
                <td colspan="4">
                    <asp:Label ID="ddlPurpose" runat="server" Width="110px" MaxLength="10"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Lien Position:
                </td>
                <td>
                    <asp:Label ID="ddlLien" runat="server" Width="235px">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Property Address :
                </td>
                <td colspan="4">
                    <asp:Label ID="txbPropertyAddress" runat="server" MaxLength="50" Width="235px"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    City :
                </td>
                <td>
                    <asp:Label ID="txbCity" runat="server" MaxLength="50"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    State :
                </td>
                <td colspan="4">
                    <asp:Label ID="ddlState" runat="server" Width="68px"> 
                    </asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Zip :
                </td>
                <td>
                    <asp:Label ID="txbZip" runat="server" MaxLength="10" Width="128px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Point Folder:
                </td>
                <td colspan="4">
                    <asp:Label ID="ddlPointFolder" runat="server" Height="18px" Width="235px">
                    </asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Point Filename:
                </td>
                <td>
                    <asp:Label ID="txbPointFileName" runat="server" Width="235px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                Created On:
                                <asp:Label ID="lbCreatedOn" runat="server" Text=""></asp:Label>
                            </td>
                            <td colspan="2">
                                Created By:
                                <asp:Label ID="lbCreatedBy" runat="server" Text=""></asp:Label>
                            </td>
                            <td colspan="2">
                                Modified On:
                                <asp:Label ID="lbModifiedOn" runat="server" Text=""></asp:Label>
                            </td>
                            <td colspan="2">
                                Modified By:
                            </td>
                            <td>
                                <asp:Label ID="lbModifiedBy" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="8" align="right">
                    <input id="btnCancel" type="button" value=" OK " class="Btn-66" onclick="btnCancel_onclick()" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
