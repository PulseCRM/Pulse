<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="Settings_CompanyAlerts" CodeBehind="CompanyAlerts.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();
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
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                <li id="current"><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
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
                                <span style="display: block; width: 40px;"></span>
                            </td>
                            <td align="center">
                                Alert
                            </td>
                            <td align="center">
                                Task
                            </td>
                            <td align="center" colspan="2">
                                Rate Lock
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="center" style="white-space: nowrap;">
                                Business Days after Alert Detected
                            </td>
                            <td align="center" style="white-space: nowrap; padding-left: 8px; width: 300px">
                                Business Days Before Due
                            </td>
                            <td align="center" style="white-space: nowrap; padding-left: 8px;" colspan="2">
                                Business Days Before Expiration
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right" style="padding-right:55px;">
                                <img src="../images/alert/Alert-Yellow.gif" />
                                <asp:TextBox ID="txtAlertYellow" runat="server" Width="20px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAlertYellow" runat="server" ErrorMessage="*" ControlToValidate="txtAlertYellow"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvAlertYellow" runat="server" ErrorMessage="*" ControlToValidate="txtAlertYellow" ControlToCompare="txtAlertRed" Type="Integer" Operator="LessThan"></asp:CompareValidator>
                            </td>
                            <td align="right" style="padding-right:120px;">
                                <img src="../images/loan/TaskYellow.png" />
                                <asp:TextBox ID="txtTaskYellow" runat="server" Width="20px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTaskYellow" runat="server" ErrorMessage="*" ControlToValidate="txtTaskYellow"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvTaskYellow" runat="server" ErrorMessage="*" ControlToValidate="txtTaskYellow" ControlToCompare="txtTaskRed" Type="Integer" Operator="GreaterThan"></asp:CompareValidator>
                            </td>
                            <td align="right">
                            </td>
                            <td align="right"  style="padding-right:55px;">
                                <img src="../images/loan/lockYellow.gif" />
                                <asp:TextBox ID="txtRateLockYellowDays" runat="server" Width="20px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRateLockYellowDays" runat="server" ErrorMessage="*" ControlToValidate="txtRateLockYellowDays"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvRateLockYellowDays" runat="server" ErrorMessage="*" ControlToValidate="txtRateLockYellowDays" ControlToCompare="txtRateLockRedDays" Type="Integer" Operator="GreaterThan"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="right" style="padding-right:55px;">
                                <img src="../images/alert/Alert-Red.gif" />
                                <asp:TextBox ID="txtAlertRed" runat="server" Width="20px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAlertRed" runat="server" ErrorMessage="*" ControlToValidate="txtAlertRed"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*" ControlToCompare="txtAlertYellow" ControlToValidate="txtAlertRed" Type="Integer" Operator="GreaterThan"></asp:CompareValidator>
                            </td>
                            <td align="right" style="padding-right:120px;">
                                <img src="../images/loan/TaskRed.png" />
                                <asp:TextBox ID="txtTaskRed" runat="server" Width="20px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTaskRed" runat="server" ErrorMessage="*" ControlToValidate="txtTaskRed"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*" ControlToCompare="txtTaskYellow" ControlToValidate="txtTaskRed" Type="Integer" Operator="LessThan"></asp:CompareValidator>
                            </td>
                            <td align="right">
                            </td>
                            <td align="right" style="padding-right:55px;">
                                <img src="../images/loan/lockRed.gif" />
                                <asp:TextBox ID="txtRateLockRedDays" runat="server" Width="20px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRateLockRedDays" runat="server" ErrorMessage="*" ControlToValidate="txtRateLockRedDays"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="*" ControlToCompare="txtRateLockYellowDays" ControlToValidate="txtRateLockRedDays" Type="Integer" Operator="LessThan"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                            <td></td>
                            <td colspan="4">
                                <asp:CheckBox ID="cbSendMail" runat="server" Text=" Send email notification for new custom tasks." />
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                            <td></td>
                            <td colspan="4">
                                  &nbsp;&nbsp;Custom Task Email Notification: <asp:DropDownList ID="ddlTaskEmail" runat="server" DataTextField="Name" DataValueField="TemplEmailId">
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr style="height: 20px;">
                            <td>
                            </td>
                        </tr>

                        <tr style="height: 20px;">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" /><asp:Button
                                    ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
