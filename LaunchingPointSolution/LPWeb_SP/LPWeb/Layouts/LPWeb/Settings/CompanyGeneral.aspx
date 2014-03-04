<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="Settings_CompanyGeneral" CodeBehind="CompanyGeneral.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate1.8.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();

            $("#aspnetForm").validate({

                rules: {
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtMyEmailInboxUrl: {
                        url: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtMyCalendarURL: {
                        url: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtRatesURL: {
                        url: true
                    }
                }
            });
        });
    </script>
    <style>
        .TabContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabContent input[type="text"], select, input[type="file"]
        {
            margin-left: 15px;
            margin-right: 15px;
        }
        .TabContent table td
        {
            padding-top: 9px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        Company Setup</div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
<%--                                <li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine" style="width: 242px">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">
                    &nbsp;</div>
                <div class="TabContent">
                    <table>
                        <tr>
                            <td>
                                AD OU Filter Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtADOUFilterName" runat="server" Text="" MaxLength="255"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnImportUsers" runat="server" Text="Import AD User" CausesValidation="false"
                                    CssClass="Btn-115" OnClick="btnImportUsers_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Company Name
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCompanyName" runat="server" Width="458" MaxLength="50" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ErrorMessage="*" ControlToValidate="txtCompanyName"></asp:RequiredFieldValidator>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Group Access
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlGroupAccess" runat="server" Width="462" AppendDataBoundItems="true">
                                    <asp:ListItem Text="-Select-" Value="" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvGroupAccess" runat="server" ErrorMessage="*" ControlToValidate="ddlGroupAccess"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Description
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDescription" runat="server" Width="458" MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtAddress" runat="server" Width="458" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                City
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" runat="server" Width="180px" MaxLength="100"></asp:TextBox>
                            </td>
                            <td style="padding-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            State
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlState" runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvState" runat="server" ErrorMessage="*" ControlToValidate="ddlState"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            Zip
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtZip" runat="server" MaxLength="5"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvZip" runat="server" ErrorMessage="*" ControlToValidate="txtZip"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                         <tr>
                            <td>
                                Phone
                            </td>
                            <td >
                                <asp:TextBox ID="txtPhone" runat="server" Width="100" MaxLength="20"></asp:TextBox>
                            </td>

                            <td  style="padding-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                              Fax
                                        </td>
                                        <td style=" padding-left:4px;">
                                           <asp:TextBox ID="txtFax" runat="server" Width="100" MaxLength="20"></asp:TextBox>
                                        </td>
                                        <td>
                                              
                                        </td>
                                        <td>
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                Email
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEmail" runat="server" Width="458" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                Company Website
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtWebURL" runat="server" Width="458" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Security Token
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSecToken" runat="server" Width="458" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                Import user interval
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlInterval" runat="server" Width="40">
                                    <asp:ListItem Text="24" Value="24" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                                Hours
                            </td>
                        </tr>
                        
                    </table>
                    <table>
                    		<tr>
                            <td>
                                My Email Inbox URL
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtMyEmailInboxUrl" runat="server" Width="458" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                My Calendar URL
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtMyCalendarURL" runat="server" Width="458" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Rates URL
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRatesURL" runat="server" Width="458" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" /><asp:Button
                                    ID="btnSaveAs" runat="server" Text="New" CssClass="Btn-66" OnClick="btnSaveAs_Click"
                                    Visible="false" /><asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
