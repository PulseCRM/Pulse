<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="LoanDetailTab" CodeBehind="LoanDetailTab.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css"
        runat="server" />
</head>
<body style="width:700px">
    <form id="form1" runat="server">
    <div>
        <table width="700px">
        
            <tr style="height:18px;"><td></td></tr>
            <tr>
                <td style="width: 350px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Loan Amount"></asp:Label>
                                <asp:Label ID="lbLoanAmount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Sales Price :"></asp:Label>
                                <asp:Label ID="lbSalesPrice" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Appraised Value:"></asp:Label>
                                <asp:Label ID="lbAppraisedValue" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Rate:"></asp:Label>
                                <asp:Label ID="lbRate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="Loan Type:"></asp:Label>
                                <asp:Label ID="lbLoanType" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Down Payment:"></asp:Label>
                                <asp:Label ID="lbDownPayment" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Monthly Payment:"></asp:Label>
                                <asp:Label ID="lbMonthlyPayment" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="LTV:"></asp:Label>
                                <asp:Label ID="lbLTV" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="CLTV:"></asp:Label>
                                <asp:Label ID="lbCLTV" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="Term/Due(months):"></asp:Label>
                                <asp:Label ID="lbTermDue" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Purpose:"></asp:Label>
                                <asp:Label ID="lbPurpose" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label23" runat="server" Text="Lien Position:"></asp:Label>
                                <asp:Label ID="lbLienPosition" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label25" runat="server" Text="Occupancy:"></asp:Label>
                                <asp:Label ID="lbOccupancy" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label27" runat="server" Text="Program:"></asp:Label>
                                <asp:Label ID="lbProgram" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label29" runat="server" Text="CC Scenario:"></asp:Label>
                                <asp:Label ID="lbCCScenario" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label31" runat="server" Text="County:"></asp:Label>
                                <asp:Label ID="lbCounty" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label33" runat="server" Text="Lender:"></asp:Label>
                                <asp:Label ID="lbLender" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table style="margin-top: -50px; padding-top: 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Borrower details section"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Date of birth:"></asp:Label>
                                <asp:Label ID="lbDateBirth" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Social security number:"></asp:Label>
                                <asp:Label ID="lbSecurityNumber" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Experian/FICO score:"></asp:Label>
                                <asp:Label ID="lbExperianFico" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="TransUnion/Emprica score:"></asp:Label>
                                <asp:Label ID="lbTransUnion" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label22" runat="server" Text="Equifax/BEACON score:"></asp:Label>
                                <asp:Label ID="lbEquifax" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="CoBorrower details section"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="Date of birth:"></asp:Label>
                                <asp:Label ID="lbCDateBirth" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Social security number:"></asp:Label>
                                <asp:Label ID="lbCSecurityNumber" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label24" runat="server" Text="Experian/FICO score:"></asp:Label>
                                <asp:Label ID="lbCExperianFico" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label30" runat="server" Text="TransUnion/Emprica score:"></asp:Label>
                                <asp:Label ID="lbCTransUnion" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label35" runat="server" Text="Equifax/BEACON score:"></asp:Label>
                                <asp:Label ID="lbCEquifax" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label47" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <asp:Label ID="Label48" runat="server" Text="Notes:"></asp:Label>
                                <asp:Label ID="lbNotes" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
